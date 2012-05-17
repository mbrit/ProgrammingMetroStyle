﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace StreetFoo.Client.Tests
{
    public abstract class TestBase
    {
        [TestInitialize()]
        public void TestInitialize()
        {
            StreetFooRuntime.Start("Tests");

            // replace the real proxy handlers with mock proxy handlers...
            ServiceProxyFactory.Current.SetHandler(typeof(IRegisterServiceProxy), typeof(MockRegisterServiceProxy));
            ServiceProxyFactory.Current.SetHandler(typeof(ILogonServiceProxy), typeof(MockLogonServiceProxy));
        }
    }
}
