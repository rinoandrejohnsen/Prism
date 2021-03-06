using CommonServiceLocator;
using Grace.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Prism.Grace.Wpf.Tests.Mocks;
using Prism.IocContainer.Wpf.Tests.Support.Mocks.Views;
using Prism.Regions;
using System.Linq;

namespace Prism.Grace.Wpf.Tests
{
    [TestClass]
    public class GraceRegionNavigationContentLoaderFixture
    {
        [TestMethod]
        public void ShouldFindCandidateViewInRegion()
        {
            DependencyInjectionContainer container = new DependencyInjectionContainer();
            container.Configure(c => c.Export<object>().AsKeyed<MockView>("MockView"));

            this.ConfigureMockServiceLocator(container);

            // We cannot access the GraceRegionNavigationContentLoader directly so we need to call its
            // GetCandidatesFromRegion method through a navigation request.
            IRegion testRegion = new Region();

            MockView view = new MockView();
            testRegion.Add(view);
            testRegion.Deactivate(view);

            testRegion.RequestNavigate("MockView");

            Assert.IsTrue(testRegion.Views.Contains(view));
            Assert.IsTrue(testRegion.Views.Count() == 1);
            Assert.IsTrue(testRegion.ActiveViews.Count() == 1);
            Assert.IsTrue(testRegion.ActiveViews.Contains(view));
        }

        [TestMethod]
        public void ShouldFindCandidateViewWithFriendlyNameInRegion()
        {
            DependencyInjectionContainer container = new DependencyInjectionContainer();
            container.Configure(c => c.Export<MockView>().AsName("SomeView"));

            this.ConfigureMockServiceLocator(container);

            // We cannot access the GraceRegionNavigationContentLoader directly so we need to call its
            // GetCandidatesFromRegion method through a navigation request.
            IRegion testRegion = new Region();

            MockView view = new MockView();
            testRegion.Add(view);
            testRegion.Deactivate(view);

            testRegion.RequestNavigate("SomeView");

            Assert.IsTrue(testRegion.Views.Contains(view));
            Assert.IsTrue(testRegion.ActiveViews.Count() == 1);
            Assert.IsTrue(testRegion.ActiveViews.Contains(view));
        }

        public void ConfigureMockServiceLocator(DependencyInjectionContainer container)
        {
            MockServiceLocator serviceLocator = new MockServiceLocator(container);
            ServiceLocator.SetLocatorProvider(() => serviceLocator);
        }
    }
}
