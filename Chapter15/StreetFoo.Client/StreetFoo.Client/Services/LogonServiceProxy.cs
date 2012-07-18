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

        public async Task<LogonResult> LogonAsync(string username, string password)
        {
            // input..
            JsonObject input = new JsonObject();
            input.Add("username", username);
            input.Add("password", password);

            // call...
            var executeResult = await this.Execute(input);

            // get the user ID from the server result...
            if (!(executeResult.HasErrors))
            {
                string token = executeResult.Output.GetNamedString("token");

                // return...
                return new LogonResult(token);
            }
            else
                return new LogonResult(executeResult);
        }
    }
}
