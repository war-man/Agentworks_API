//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using TransactionRunner.ViewModels;
//
//namespace TransactionRunner.Test.Tests
//{
//    [TestClass]
//    public class MainWindowViewModelUnitTest
//    {
//        private MainWindowViewModel _mainWindowVm;
//
//        [TestInitialize]
//        public void init()
//        {
//            _mainWindowVm = new MainWindowViewModel();
//        }
//
//        [TestMethod]
//        public void ViewModelHasChildViewModels()
//        {
//            Assert.IsNotNull(_mainWindowVm.AgentSelectorViewModel, "MainWindowViewModel did not contain AgentSelectorViewModel");
//            Assert.IsNotNull(_mainWindowVm.SendParametersViewModel, "MainWindowViewModel did not contain SendParametersViewModel");
//            Assert.IsNotNull(_mainWindowVm.ResultsPaneViewModel, "MainWindowViewModel did not contain ResultsPaneViewModel");
//        }
//
//        [TestMethod]
//        public void ViewModelHasVersion()
//        {
//            Assert.AreEqual(TrVersion.Version, _mainWindowVm.AppVersion);
//        }
//    }
//}
