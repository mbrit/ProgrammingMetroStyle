using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using TinyIoC;

namespace StreetFoo.Client.Tests
{
    [TestClass]
    public class RegisterServiceProxyTests
    {
        [TestInitialize]
        public async Task Setup()
        {
            await StreetFooRuntime.Start("Tests");

            // set...
            ServiceProxyFactory.Current.SetHandler(typeof(IRegisterServiceProxy), 
                typeof(FakeRegisterServiceProxy));
        }

        [TestMethod]
        public async Task TestRegisterOk()
        {
            var proxy = ServiceProxyFactory.Current.GetHandler<IRegisterServiceProxy>();

            // ok...
            var result = await proxy.RegisterAsync("mbrit", "matt@amxmobile.com", "Password1", "Password1");
            Assert.IsFalse(result.HasErrors);
        }
    }
}
