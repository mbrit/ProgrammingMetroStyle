using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetroWnsPush
{
    public enum WnsPushResult
    {
        Received = 0,
        Dropped = 1,
        ChannelThrottled = 2
    }
}
