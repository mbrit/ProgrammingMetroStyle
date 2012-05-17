//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Microsoft.VisualStudio.TestTools.UnitTesting;

//namespace StreetFoo.Client.Tests
//{
//    [TestClass()]
//    public class TestLogonViewModel : TestBase
//    {
//        [TestMethod()]
//        public void TestMessageShowOnFailure()
//        {
//            // get the view model, passing in our mock host...
//            MockViewModelHost host = new MockViewModelHost();
//            ILogonPageViewModel model = ViewModelFactory.Current.GetHandler<ILogonPageViewModel>(host);

//            // if we run the model's command and don't pass in any properties, we should 
//            // get an error message...
//            model.LogonCommand.Execute(null);

//            // check that we showed one error message and no fatal messages...
//            Assert.AreEqual(1, host.NumErrorBucketMessages);
//            Assert.IsNull(host.Fatal);
//        }

//        [TestMethod()]
//        public void TestNoNavigationOnFailure()
//        {
//            // get the view model, passing in our mock host...
//            MockViewModelHost host = new MockViewModelHost();
//            ILogonPageViewModel model = ViewModelFactory.Current.GetHandler<ILogonPageViewModel>(host);

//            // if we run the model's command and don't pass in any properties, we should 
//            // get an error message...
//            model.LogonCommand.Execute(null);

//            // check that we didn't navigate...
//            Assert.IsNull(host.LastPageChange);
//        }

//        [TestMethod()]
//        public void TestMessageShowOnValidationPass()
//        {
//            // get the view model, passing in our mock host...
//            MockViewModelHost host = new MockViewModelHost();
//            ILogonPageViewModel model = ViewModelFactory.Current.GetHandler<ILogonPageViewModel>(host);

//            // set the data...
//            model.Username = "mbrit";
//            model.Password = "password";

//            // create a task wrapper that can handle the success delegate from the model and patch it
//            // into a local variable for testing... (this anonymous method simulates the success callback
//            // from the event...)
//            TaskWrapper wrapper = new TaskWrapper();

//            // this call should work...
//            model.LogonCommand.Execute(wrapper);

//            // wait until it's finished executing...
//            wrapper.WaitIfTaskAvailableAndSurfaceExceptions();

//            // check that we showed no error messages...
//            Assert.AreEqual(0, host.NumErrorBucketMessages);
//            Assert.IsNull(host.Fatal);
//        }

//        [TestMethod()]
//        public void TestCallbackRaisedOnSuccess()
//        {
//            // get the view model, passing in our mock host...
//            MockViewModelHost host = new MockViewModelHost();
//            ILogonPageViewModel model = ViewModelFactory.Current.GetHandler<ILogonPageViewModel>(host);

//            // set the data...
//            model.Username = "mbrit";
//            model.Password = "password";

//            // create a task wrapper that can handle the success delegate from the model and patch it
//            // into a local variable for testing... (this anonymous method simulates the success callback
//            // from the event...)
//            LogonResult result = null;
//            TaskWrapper wrapper = new TaskWrapper((theResult) => result = (LogonResult)theResult);

//            // this call should work...
//            model.LogonCommand.Execute(wrapper);

//            // wait until it's finished executing...
//            wrapper.WaitIfTaskAvailableAndSurfaceExceptions();

//            // check that we called the callback...
//            Assert.IsTrue(wrapper.SuccessCalled);

//            // check that we got a token in the result...
//            Assert.IsFalse(string.IsNullOrEmpty(result.Token));
//        }

//        [TestMethod()]
//        public void TestNavigateToLogonOnSuccess()
//        {
//            // get the view model, passing in our mock host...
//            MockViewModelHost host = new MockViewModelHost();
//            ILogonPageViewModel model = ViewModelFactory.Current.GetHandler<ILogonPageViewModel>(host);

//            // set the data...
//            model.Username = "mbrit";
//            model.Password = "password";

//            // this call should work...
//            TaskWrapper wrapper = new TaskWrapper();
//            model.LogonCommand.Execute(wrapper);

//            // wait until it's finished executing...
//            wrapper.WaitIfTaskAvailableAndSurfaceExceptions();

//            // check that we did navigate to the logon page...
//            Assert.AreEqual(1, host.NumPageChanges);
//            Assert.AreEqual(typeof(ILogonPageViewModel), host.LastPageChange);
//        }

//        [TestMethod()]
//        public void TestRegistrationPageNavigation()
//        {
//            // get the view model, passing in our mock host...
//            MockViewModelHost host = new MockViewModelHost();
//            ILogonPageViewModel model = ViewModelFactory.Current.GetHandler<ILogonPageViewModel>(host);

//            // this call should work...
//            TaskWrapper wrapper = new TaskWrapper();
//            model.RegisterCommand.Execute(wrapper);

//            // wait until it's finished executing...
//            wrapper.WaitIfTaskAvailableAndSurfaceExceptions();

//            // check that we did navigate to the logon page...
//            Assert.AreEqual(1, host.NumPageChanges);
//            Assert.AreEqual(typeof(IRegisterPageViewModel), host.LastPageChange);
//        }
//    }
//}
