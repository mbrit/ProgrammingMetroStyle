﻿using System;  
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

        public Task Register(string username, string email, string password, string confirm, Action<RegisterResult> success, 
            FailureHandler failure)
        {
            // package up the request...
            JsonObject input = new JsonObject();
            ConfigureInputArgs(input);
            input.Add("username", username);
            input.Add("email", email);
            input.Add("password", password);
            input.Add("confirm", confirm);

            // call...
            return this.Execute(input, (output) =>
            {
                // get the user ID from the server result...
                string userId = output.GetNamedString("userId");

                // return...
                RegisterResult result = new RegisterResult(userId);
                success(result);

            }, failure);
        }
    }
}