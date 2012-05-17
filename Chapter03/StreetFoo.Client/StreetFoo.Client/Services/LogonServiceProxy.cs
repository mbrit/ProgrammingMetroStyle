using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace StreetFoo.Client
{
    public class LogonServiceProxy : ServiceProxy, ILogonServiceProxy
    {
        public LogonServiceProxy()
            : base("Logon")
        {
        }

        public Task Logon(string username, string password, Action<LogonResult> success, FailureHandler failure, Action complete)
        {
            // input..
            JsonObject input = new JsonObject();
            input.Add("username", username);
            input.Add("password", password);

            // call...
            return this.Execute(input, (output) =>
            {
                // get the user ID from the server result...
                string token = output.GetNamedString("token");

                // return...
                if (success != null)
                {
                    LogonResult result = new LogonResult(token);
                    success(result);
                }

            }, failure, complete);
        }
    }
}
