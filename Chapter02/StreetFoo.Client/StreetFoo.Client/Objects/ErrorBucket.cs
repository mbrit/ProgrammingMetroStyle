using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreetFoo.Client
{
    // holds a set of errors that we can build up as work through a process...
    public class ErrorBucket
    {
        private List<string> Errors { get; set; }

        public ErrorBucket()
        {
            this.Errors = new List<string>();
        }

        public void AddError(string error)
        {
            this.Errors.Add(error);
        }

        public bool HasErrors
        {
            get
            {
                return this.Errors.Any();
            }
        }

        public string GetErrorsAsString()
        {
            StringBuilder builder = new StringBuilder();
            foreach (string error in this.Errors)
            {
                if (builder.Length > 0)
                    builder.Append("\r\n");
                builder.Append(error);
            }

            return builder.ToString();
        }
    }
}
