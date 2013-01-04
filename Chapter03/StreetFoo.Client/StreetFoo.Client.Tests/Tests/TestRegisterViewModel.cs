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
    public class TestRegisterViewModel : TestBase
    {
        [TestMethod]
        public void TestMessageShowOnFailure()
        {
            // get the view model, passing in our mock host...
            MockViewModelHost host = new MockViewModelHost();
            IRegisterPageViewModel model = TinyIoCContainer.Current.Resolve<IRegisterPageViewModel>();
            model.Initialize(new MockViewModelHost());

            // if we run the model's command and don't pass in any properties, we should 
            // get an error message...
            model.RegisterCommand.ExecuteAndBlock();

            // check that we showed one error message...
            Assert.AreEqual(1, host.NumErrorBucketMessages);
        }

        [TestMethod]
        public void TestNoNavigationOnFailure()
        {
            // get the view model, passing in our mock host...
            MockViewModelHost host = new MockViewModelHost();
            IRegisterPageViewModel model = TinyIoCContainer.Current.Resolve<IRegisterPageViewModel>();
            model.Initialize(new MockViewModelHost());

            // if we run the model's command and don't pass in any properties, we should 
            // get an error message...
            model.RegisterCommand.ExecuteAndBlock();

            // check that we didn't navigate...
            Assert.IsNull(host.LastPageChange);
        }

        [TestMethod]
        public void TestMessageShowOnValidationPass()
        {
            // get the view model, passing in our mock host...
            MockViewModelHost host = new MockViewModelHost();
            IRegisterPageViewModel model = TinyIoCContainer.Current.Resolve<IRegisterPageViewModel>();
            model.Initialize(new MockViewModelHost());

            // set the data...
            model.Username = "mbrit";
            model.Email = "mbrit@mbrit.com";
            model.Password = "password";
            model.Confirm = "password";

            // this call should work...
            model.RegisterCommand.ExecuteAndBlock();

            // check that we showed no error messages...
            Assert.AreEqual(0, host.NumErrorBucketMessages);
        }

        [TestMethod]
        public void TestNavigateToLogonOnSuccess()
        {
            // get the view model, passing in our mock host...
            MockViewModelHost host = new MockViewModelHost();
            IRegisterPageViewModel model = TinyIoCContainer.Current.Resolve<IRegisterPageViewModel>();
            model.Initialize(new MockViewModelHost());

            // set the data...
            model.Username = "mbrit";
            model.Email = "mbrit@mbrit.com";
            model.Password = "password";
            model.Confirm = "password";

            // this call should work...
            model.RegisterCommand.ExecuteAndBlock();

            // check that we did navigate to the logon page...
            Assert.AreEqual(1, host.NumPageChanges);
            Assert.AreEqual(typeof(ILogonPageViewModel), host.LastPageChange);
        }
    }
}
