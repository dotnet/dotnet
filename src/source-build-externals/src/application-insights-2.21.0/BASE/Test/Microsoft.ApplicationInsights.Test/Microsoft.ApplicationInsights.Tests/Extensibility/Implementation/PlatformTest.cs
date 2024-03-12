namespace Microsoft.ApplicationInsights.Extensibility.Implementation
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Text;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Platform;
    using Microsoft.ApplicationInsights.TestFramework;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class PlatformTest
    {
        [TestCleanup]
        public void TestCleanup()
        {
            PlatformSingleton.Current = null;
        }

        [TestMethod]
        public void ClassIsPublicToAllowTestingOnWindowsRuntime()
        {
            Assert.IsFalse(typeof(PlatformSingleton).GetTypeInfo().IsPublic);
        }

        [TestMethod]
        public void ClassIsStaticToServeOnlyAsSingletonFactory()
        {
            Assert.IsTrue(typeof(PlatformSingleton).GetTypeInfo().IsAbstract && typeof(PlatformSingleton).GetTypeInfo().IsSealed);
        }

        [TestMethod]
        public void CurrentIsAutomaticallyInitializedForEasyAccess()
        {
            IPlatform current = PlatformSingleton.Current;
            Assert.IsNotNull(current);
        }

        [TestMethod]
        public void CurrentCanBeSetToEnableMocking()
        {
            var platform = new StubPlatform();
            PlatformSingleton.Current = platform;
            Assert.AreSame(platform, PlatformSingleton.Current);
        }
        
        [TestMethod]
        public void ReadConfigurationXmlReturnsContentsOfApplicationInsightsConfigFileInApplicationInstallationDirectory()
        {
            CreateConfigurationFile("42");
            try
            {
#if NETCOREAPP
                Assert.IsNull(PlatformSingleton.Current.ReadConfigurationXml());
#else
                Assert.AreEqual("42", PlatformSingleton.Current.ReadConfigurationXml());
#endif
            }
            finally
            {
                DeleteConfigurationFile();
            }
        }

        [TestMethod]
        public void ReadConfigurationXmlIgnoresMissingApplicationInsightsConfigurationFileByReturningEmptyString()
        {
            string configuration = PlatformSingleton.Current.ReadConfigurationXml();
#if NETCOREAPP
            Assert.IsNull(PlatformSingleton.Current.ReadConfigurationXml());
#else
            Assert.IsNotNull(configuration);
            Assert.AreEqual(0, configuration.Length);
#endif
        }

        private static void CreateConfigurationFile(string content)
        {
            using (Stream fileStream = OpenConfigurationFile())
            {
                byte[] configurationBytes = Encoding.UTF8.GetBytes(content);
                fileStream.Write(configurationBytes, 0, configurationBytes.Length);
            }           
        }
        
        private static void DeleteConfigurationFile()
        {
            File.Delete(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ApplicationInsights.config"));
        }

        private static Stream OpenConfigurationFile()
        {
            return File.OpenWrite(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ApplicationInsights.config"));
        }
    }
}
