using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetroLog.Tests
{
    public class TestLoggable : ILoggable
    {
        public void DoMagic()
        {
            this.LogInfo("In this case, Info is an extension method...");

            var buf = "like this";
            this.LogWarn("You can also use formatting, {0}", buf);
        }
    }
}
