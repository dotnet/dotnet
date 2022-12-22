using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Test;

using Xunit;

namespace Microsoft.ApplicationInsights.AspNetCore.Tests.Extensions.ApplicationInsightsExtensionsTests
{
    /// <summary>
    /// When working with IServiceCossection, it can store three types of Implementations:
    /// ImplementationFactory, ImplementationInstance, and ImplementationType.
    /// We want to be able to add a Singleton but only if a user hasn't already done so.
    /// This class is to test all the various edge cases.
    /// </summary>
    public class AddSingletonTests : BaseTestClass
    {
        [Fact]
        /// <summary>
        /// When iterating the services collection, if we forget to check the ImplementationFactory this will throw NullRefExceptions.
        /// </summary>
        public static void VerifyAddSingletonIfNotExists_CanDetectImplemnationFactory()
        {
            var services = GetServiceCollectionWithContextAccessor();
            services.AddTransient<MyService>(MyServiceFactory.Create);

            services.AddSingletonIfNotExists<IService, MyService>();

            //VALIDATE
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            var count = serviceProvider.GetServices<IService>().OfType<MyService>().Count();
            Assert.Equal(1, count);
        }

        [Fact]
        /// <summary>
        /// AddSingleton is the most common way to register a type. The framework methods will check for this.
        /// This test is to confirm that we haven't broken any expected behavior.
        /// </summary>
        public static void VerifyAddSingletonIfNotExists_CanDetectImplemnationType()
        {
            var services = GetServiceCollectionWithContextAccessor();
            services.AddSingleton<IService, MyService>();

            services.AddSingletonIfNotExists<IService, MyService>();

            //VALIDATE
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            var count = serviceProvider.GetServices<IService>().OfType<MyService>().Count();
            Assert.Equal(1, count);
        }

        [Fact]
        ///<summary>
        /// This is the case that's hardest to check for. The framework methods won't check for this.
        /// </summary>
        public static void VerifyAddSingletonIfNotExists_CanDetectImplemnationInstance()
        {
            var services = GetServiceCollectionWithContextAccessor();
            services.AddSingleton<IService>(new MyService());

            services.AddSingletonIfNotExists<IService, MyService>();

            //VALIDATE
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            var count = serviceProvider.GetServices<IService>().OfType<MyService>().Count();
            Assert.Equal(1, count);
        }

        private interface IService
        {
        }

        private class MyService : IService
        {
        }

        private class MyService2 : IService
        {
        }

        private static class MyServiceFactory
        {
            public static MyService Create(IServiceProvider serviceProvider)
            {
                return new MyService();
            }
        }
    }
}
