using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace StreetFoo.Client.Tests
{
    [TestClass()]
    public class TestRegisterPage : TestBase
    {
        [TestMethod()]
        public void TestMessageShowOnFailure()
        {
            // reset...
            MockViewModelHost.Current.Reset();

            // get the view model, passing in our mock host...
            IRegisterPageViewModel model = ViewModelFactory.Current.GetHandler<IRegisterPageViewModel>(MockViewModelHost.Current);

            // if we run the model's command and don't pass in any properties, we should 
            // get an error message...
            model.RegisterCommand.Execute(null);

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
            IRegisterPageViewModel model = ViewModelFactory.Current.GetHandler<IRegisterPageViewModel>(MockViewModelHost.Current);

            // if we run the model's command and don't pass in any properties, we should 
            // get an error message...
            model.RegisterCommand.Execute(null);

            // check that we didn't navigate...
            Assert.IsNull(MockViewModelHost.Current.LastPageChange);
        }

        [TestMethod()]
        public void TestMessageShowOnValidationPass()
        {
            // reset...
            MockViewModelHost.Current.Reset();

            // get the view model, passing in our mock host...
            IRegisterPageViewModel model = ViewModelFactory.Current.GetHandler<IRegisterPageViewModel>(MockViewModelHost.Current);

            // set the data...
            model.Username = "mbrit";
            model.Email = "mbrit@mbrit.com";
            model.Password = "password";
            model.Confirm = "password";

            // create a task wrapper that can handle the success delegate from the model and patch it
            // into a local variable for testing... (this anonymous method simulates the success callback
            // from the event...)
            TaskWrapper wrapper = new TaskWrapper();

            // this call should work...
            model.RegisterCommand.Execute(wrapper);

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
            IRegisterPageViewModel model = ViewModelFactory.Current.GetHandler<IRegisterPageViewModel>(MockViewModelHost.Current);

            // set the data...
            model.Username = "mbrit";
            model.Email = "mbrit@mbrit.com";
            model.Password = "password";
            model.Confirm = "password";

            // create a task wrapper that can handle the success delegate from the model and patch it
            // into a local variable for testing... (this anonymous method simulates the success callback
            // from the event...)
            RegisterResult result = null;
            TaskWrapper wrapper = new TaskWrapper((theResult) => result = (RegisterResult)theResult);

            // this call should work...
            model.RegisterCommand.Execute(wrapper);

            // wait until it's finished executing...
            wrapper.WaitIfTaskAvailable();

            // check that we called the callback...
            Assert.IsTrue(wrapper.SuccessCalled);

            // check that we got a token in the result...
            Assert.IsFalse(string.IsNullOrEmpty(result.UserId));
        }

        [TestMethod()]
        public void TestNavigateToLogonOnSuccess()
        {
            // reset...
            MockViewModelHost.Current.Reset();

            // get the view model, passing in our mock host...
            IRegisterPageViewModel model = ViewModelFactory.Current.GetHandler<IRegisterPageViewModel>(MockViewModelHost.Current);

            // set the data...
            model.Username = "mbrit";
            model.Email = "mbrit@mbrit.com";
            model.Password = "password";
            model.Confirm = "password";

            // this call should work...
            TaskWrapper wrapper = new TaskWrapper();
            model.RegisterCommand.Execute(wrapper);

            // wait until it's finished executing...
            wrapper.WaitIfTaskAvailable();

            // check that we did navigate to the logon page...
            Assert.AreEqual(1, MockViewModelHost.Current.NumPageChanges);
            Assert.AreEqual(typeof(ILogonPageViewModel), MockViewModelHost.Current.LastPageChange);
        }
    }
}
