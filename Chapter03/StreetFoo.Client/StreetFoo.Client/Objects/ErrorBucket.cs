using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreetFoo.Client
{
    //// delegate for error handler methods...
    //public delegate void FailureHandler(object sender, ErrorBucket bucket);

    // holds a set of errors that we can build up as work through a process...
    public class ErrorBucket
    {
        // standard error message...
        private List<string> Errors { get; set; }

        // holds a fatal error reference...
        public Exception Fatal { get; internal set; }

        public ErrorBucket()
        {
            this.Errors = new List<string>();
        }

        // special constructor for fatal exceptions...
        private ErrorBucket(Exception ex)
            : this()
        {
            this.Fatal = ex;
        }

        // special constructor for cloning another error bucket...
        protected ErrorBucket(ErrorBucket donor)
            : this()
        {
            CopyFrom(donor);
        }

        public void AddError(string error)
        {
            this.Errors.Add(error);
        }

        public bool HasErrors
        {
            get
            {
                return this.Errors.Count > 0 || this.HasFatal;
            }
        }

        public bool HasFatal
        {
            get
            {
                return this.Fatal != null;
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

            // fatal?
            if (this.HasFatal)
            {
                if (builder.Length > 0)
                    builder.Append("\r\n-------------------------\r\n");

                // make this prettier (well, take some of the detail out so it's not so overwhelming. 
                // ideally you should log this information somewhere...
                List<Exception> exes = new List<Exception>();

                // if we have an aggregate exception, flatten it, otherwise seed the set...
                if (this.Fatal is AggregateException)
                    exes.Add(((AggregateException)this.Fatal).Flatten());
                else
                    exes.Add(this.Fatal);

                // walk...
                int index = 0;
                while (index < exes.Count)
                {
                    if (exes[index].InnerException != null)
                        exes.Add(exes[index].InnerException);

                    // add...
                    if (index > 0)
                        builder.Append("\r\n");
                    builder.Append(exes[index].Message);

                    // next...
                    index++;
                }
            }

            // return...
            return builder.ToString();
        }

        internal static ErrorBucket CreateFatalBucket(Exception ex)
        {
            return new ErrorBucket(ex);
        }

        public void CopyFrom(ErrorBucket donor)
        {
            // copy the normal errors...
            this.Errors.Clear();
            this.Errors.AddRange(donor.Errors);

            // copy the fatal error...
            this.Fatal = donor.Fatal;
        }
    }
}
