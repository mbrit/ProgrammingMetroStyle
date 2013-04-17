using System;  
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace StreetFoo.Client
{
    public class RegisterServiceProxy : ServiceProxy, IRegisterServiceProxy
    {
        public RegisterServiceProxy()
            : base("Register")
        {
        }

        public async Task<RegisterResult> RegisterAsync(string username, string email, string password, string confirm)
        {
            // package up the request...
            JsonObject input = new JsonObject();
            input.Add("username", username);
            input.Add("email", email);
            input.Add("password", password);
            input.Add("confirm", confirm);

            // call...
            var executeResult = await this.ExecuteAsync(input);

            // get the user ID from the server result...
            if (!(executeResult.HasErrors))
            {
                string userId = executeResult.Output.GetNamedString("userId");
                return new RegisterResult(userId);
            }
            else
                return new RegisterResult(executeResult);
        }
    }
}
