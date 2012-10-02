using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;

namespace StreetFoo.Client
{
    public static class StringHelper
    {
        private const string ResourceMapName = "StreetFoo.Client.Resources";

        // loads a resource string and runs string.Format on it...
        public static string Format(string name, params object[] args)
        {
            var buf = new ResourceLoader(ResourceMapName).GetString("Resources/" + name);
            if (args.Any())
                return string.Format(buf, args);
            else
                return buf;
        }
    }
}
