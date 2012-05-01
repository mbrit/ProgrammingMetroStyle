using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace StreetFoo.Client.Tests
{
    [TestClass()]
    public class TestLogonPage : TestBase
    {
        [TestMethod()]
        public void TestMessageShowOnFailure()
        {
            // reset...
            MockViewModelHost.Current.Reset();

            // get the view model, passing in our mock host...
            ILogonPageViewModel model = ViewModelFactory.Current.GetHandler<ILogonPageViewModel>(MockViewModelHost.Current);

            // if we run the model's command and don't pass in any properties, we should 
            // get an error message...
            model.LogonCommand.Execute(null);

            // check that we showed one error message and no fatal messages...
            Assert.AreEqual(1, MockViewModelHost.Current.NumErrorBucketMessages);
            Assert.IsNull(MockViewModelHost.Current.Fatal);
        }

        [TestMethod()]
        public void TestNoNavigationOnFailure()
        {
            // reset...
            MockViewModelHost.Current.Reset();

            // get the view model, passing in our mock host...
            ILogonPageViewModel model = ViewModelFactory.Current.GetHandler<ILogonPageViewModel>(MockViewModelHost.Current);

            // if we run the model's command and don't pass in any properties, we should 
            // get an error message...
            model.LogonCommand.Execute(null);

            // check that we didn't navigate...
            Assert.IsNull(MockViewModelHost.Current.LastPageChange);
        }

        [TestMethod()]
        public void TestMessageShowOnValidationPass()
        {
            // reset...
            MockViewModelHost.Current.Reset();

            // get the view model, passing in our mock host...
            ILogonPageViewModel model = ViewModelFactory.Current.GetHandler<ILogonPageViewModel>(MockViewModelHost.Current);

            // set the data...
            model.Username = "mbrit";
            model.Password = "password";

            // create a task wrapper that can handle the success delegate from the model and patch it
            // into a local variable for testing... (this anonymous method simulates the success callback
            // from the event...)
            TaskWrapper wrapper = new TaskWrapper();

            // this call should work...
            model.LogonCommand.Execute(wrapper);

            // wait until it's finished executing...
            wrapper.WaitIfTaskAvailable();

            // check that we showed no error messages...
            Assert.AreEqual(0, MockViewModelHost.Current.NumErrorBucketMessages);
            Assert.IsNull(MockViewModelHost.Current.Fatal);
        }

        [TestMethod()]
        public void TestCallbackRaisedOnSuccess()
        {
            // reset...
            MockViewModelHost.Current.Reset();

            // get the view model, passing in our mock host...
            ILogonPageViewModel model = ViewModelFactory.Current.GetHandler<ILogonPageViewModel>(MockViewModelHost.Current);

            // set the data...
            model.Username = "mbrit";
            model.Password = "password";

            // create a task wrapper that can handle the success delegate from the model and patch it
            // into a local variable for testing... (this anonymous method simulates the success callback
            // from the event...)
            LogonResult result = null;
            TaskWrapper wrapper = new TaskWrapper((theResult) => result = (LogonResult)theResult);

            // this call should work...
            model.LogonCommand.Execute(wrapper);

            // wait until it's finished executing...
            wrapper.WaitIfTaskAvailable();

            // check that we called the callback...
            Assert.IsTrue(wrapper.SuccessCalled);

            // check that we got a token in the result...
            Assert.IsFalse(string.IsNullOrEmpty(result.Token));
        }

        [TestMethod()]
        public void TestNavigateToLogonOnSuccess()
        {
            // reset...
            MockViewModelHost.Current.Reset();

            // get the view model, passing in our mock host...
            ILogonPageViewModel model = ViewModelFactory.Current.GetHandler<ILogonPageViewModel>(MockViewModelHost.Current);

            // set the data...
            model.Username = "mbrit";
            model.Password = "password";

            // this call should work...
            TaskWrapper wrapper = new TaskWrapper();
            model.LogonCommand.Execute(wrapper);

            // wait until it's finished executing...
            wrapper.WaitIfTaskAvailable();

            // check that we did navigate to the logon page...
            Assert.AreEqual(1, MockViewModelHost.Current.NumPageChanges);
            Assert.AreEqual(typeof(ILogonPageViewModel), MockViewModelHost.Current.LastPageChange);
        }

        [TestMethod()]
        public void TestRegistrationPageNavigation()
        {
            // reset...
            MockViewModelHost.Current.Reset();

            // get the view model, passing in our mock host...
            ILogonPageViewModel model = ViewModelFactory.Current.GetHandler<ILogonPageViewModel>(MockViewModelHost.Current);

            // this call should work...
            TaskWrapper wrapper = new TaskWrapper();
            model.RegisterCommand.Execute(wrapper);

            // wait until it's finished executing...
            wrapper.WaitIfTaskAvailable();

            // check that we did navigate to the logon page...
            Assert.AreEqual(1, MockViewModelHost.Current.NumPageChanges);
            Assert.AreEqual(typeof(IRegisterPageViewModel), MockViewModelHost.Current.LastPageChange);
        }
    }
}
