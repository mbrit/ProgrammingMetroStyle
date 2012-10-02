using MetroLog;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Windows.System.Threading;

namespace StreetFoo.Client
{
    public class SignalManager
    {
        // registrations...
        private List<SignalRegistration> Registrations { get; set; }

        // timer...
        private ThreadPoolTimer PollTimer { get; set; }
        private bool StartPollingOnFirstRegistration { get; set; }

        // log...
        private ILogger Logger { get; set; }

        public static SignalManager Current { get; private set; }

        private SignalManager()
        {
            this.Registrations = new List<SignalRegistration>();
            this.Logger = LogManagerFactory.DefaultLogManager.GetLogger<SignalManager>();
        }

        static SignalManager()
        {
            Current = new SignalManager();
        }

        internal void Register<T>(ISignalSink sink)
            where T : SignalBase
        {
            // first?
            bool first = !(this.Registrations.Any());

            // find...
            var existing = this.Registrations.Where(v => v.SignalType == typeof(T) && v.Sink == sink).FirstOrDefault();
            if (existing == null)
                this.Registrations.Add(new SignalRegistration(typeof(T), sink));

            // first...
            if (first && this.StartPollingOnFirstRegistration)
            {
                this.StartPollingOnFirstRegistration = false;
                this.StartPolling();
            }
        }

        internal async Task CreateSignalAsync(SignalBase signal)
        {
            var name = signal.Name;

            // find...
            var conn = StreetFooRuntime.GetSystemDatabase();
            var item = (await conn.Table<SignalItem>().Where(v => v.Name == name).ToListAsync()).FirstOrDefault();
            if (item == null)
            {
                item = new SignalItem()
                {
                    Name = name
                };
            }

            // set...
            item.Data = JsonConvert.SerializeObject(signal);

            // save...
            if (item.Id == 0)
                await conn.InsertAsync(item);
            else
                await conn.UpdateAsync(item);
        }

        private string GetViewModelName<T>()
            where T : IViewModel
        {
            return typeof(T).FullName;
        }

        internal Task PollSignalsAsync()
        {
            return Task.FromResult<bool>(true);
        }

        public void StartPolling()
        {
            // if we don't have any registrations, ignore this...
            if (!(this.Registrations.Any()))
            {
                this.StartPollingOnFirstRegistration = true;
                return;
            }

            // create a timer that will wait...
            if (this.PollTimer == null)
                this.PollTimer = ThreadPoolTimer.CreatePeriodicTimer(PollingTick, TimeSpan.FromMinutes(1));
        }

        private void StopPolling()
        {
            if (this.PollTimer != null)
            {
                try
                {
                    this.PollTimer.Cancel();
                }
                finally
                {
                    this.PollTimer = null;
                }
            }
        }

        private async void PollingTick(ThreadPoolTimer timer)
        {
            this.Logger.Info("Polling...");

            // if we have no signal registrations, do nothing...
            if (!(this.Registrations.Any()))
            {
                this.StopPolling();
                return;
            }

            // load up the signals and delete them from the database...
            var conn = StreetFooRuntime.GetSystemDatabase();
            var signals = await conn.Table<SignalItem>().ToListAsync();
            await conn.ExecuteAsync("delete from signalitem");

            // log...
            this.Logger.Info("Found '{0}' signal(s)...", signals.Count);

            // now walk them...
            foreach (var signalItem in signals)
            {
                var realSignal = signalItem.ToSignal();

                // get the registrations...
                foreach (var registration in this.Registrations.Where(v => v.SignalType.GetTypeInfo().IsAssignableFrom(
                    realSignal.GetType().GetTypeInfo())))
                {
                    try
                    {
                        this.Logger.Info("Sending '{0}' to '{1}'...", realSignal, registration.Sink);

                        // send it...
                        await registration.Sink.HandleSignalAsync(realSignal);
                    }
                    catch (Exception ex)
                    {
                        this.Logger.Warn("Failed to handle signal.", ex);
                    }
                }
            }
        }
    }
}
