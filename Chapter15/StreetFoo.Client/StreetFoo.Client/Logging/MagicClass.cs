using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreetFoo.Client.Logging
{
    public abstract class MagicClass
    {
        public abstract Task<string> GetMagicSpellAsync();
    }

    // uses async...
    public class FooMagicClass : MagicClass
    {
        public override async Task<string> GetMagicSpellAsync()
        {
            return await MagicHelper.GetSpellFromNetwork();
        }
    }

    // doesn't use async...
    public class BarMagicClass : MagicClass
    {
        public override Task<string> GetMagicSpellAsync()
        {
            return Task.FromResult<string>("I'm a made up spell!");
        }
    }

    public class Wizard
    {
        public async void DoMagic()
        {
            MagicClass magic = new BarMagicClass();
            var spell = await magic.GetMagicSpellAsync();
        }
    }



    public static class MagicHelper
    {
        internal static Task<string> GetSpellFromNetwork()
        {
            throw new NotImplementedException();
        }
    }
}
