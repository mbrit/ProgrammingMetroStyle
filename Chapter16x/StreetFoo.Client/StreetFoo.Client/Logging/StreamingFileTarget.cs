using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MetroLog;
using MetroLog.Layouts;
using MetroLog.Targets;
using Windows.Storage;

namespace StreetFoo.Client
{
    internal class StreamingFileTarget : StreamingTarget
    {
        private StorageFile LogFile { get; set; }
        private DateTime LogFileDate { get; set; }

        internal StreamingFileTarget()
            : base(new SingleLineLayout())
        {
        }

        private async Task<StorageFile> EnsureInitializedAsync()
        {
            // are we done?
            var now = DateTime.Today;
            if (this.LogFile == null || now.Day != this.LogFileDate.Day || now.Month != this.LogFileDate.Month ||
                now.Year != LogFileDate.Year)
            {
                // get the folder...
                StorageFolder logFolder = null;
                const string logFolderName = "StreamingLogs";
                try
                {
                    logFolder = await ApplicationData.Current.LocalFolder.GetFolderAsync(logFolderName);
                }
                catch (FileNotFoundException ex)
                {
                    SinkWarning(ex);
                }

                // create one?
                if (logFolder == null)
                    logFolder = await ApplicationData.Current.LocalFolder.CreateFolderAsync(logFolderName);

                // get the log file...
                string filename = string.Format("StreamingFile - {0:yyyyMMdd}.log", now);
                try
                {
                    this.LogFile = await logFolder.GetFileAsync(filename);
                }
                catch (FileNotFoundException ex)
                {
                    SinkWarning(ex);
                }

                // create?
                if (this.LogFile == null)
                    this.LogFile = await logFolder.CreateFileAsync(filename);

                // set...
                this.LogFileDate = now;
            }

            // return...
            return this.LogFile;
        }

        private void SinkWarning(FileNotFoundException ex)
        {
            // no-op - just cancelling out the compiler warnings...
        }

        protected override void WriteQueuedItem(LogEventInfo entry)
        {
            string buf = this.Layout.GetFormattedString(entry);

            // run it, but block until we're finished...
            this.WriteToDisk(buf).Wait();
        }

        private async Task WriteToDisk(string buf)
        {
            // make sure that we're initialized...
            var file = await this.EnsureInitializedAsync();

            // write to it...
            await FileIO.AppendLinesAsync(file, new List<string>() { buf });
        }

        private void WriteToDebug(string buf)
        {
            Debug.WriteLine(string.Format("DEQUEUED at {0} on thread {1}: {2}", DateTime.Now.ToString("HH:mm:ss"), 
                Environment.CurrentManagedThreadId, buf));
        }
    }
}
