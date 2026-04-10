namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Platform
{
#if NETFRAMEWORK
    using System;
    using System.IO;
    using System.Security;
    using System.Security.Permissions;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Shared, platform-neutral tests for <see cref="PlatformImplementation"/> class.
    /// </summary>
    [TestClass]
    [TestCategory("WindowsOnly")] // do not run these tests on linux builds
    public class PlatformImplementationTest : IDisposable
    {
        public PlatformImplementationTest()
        {
            // Make sure configuration files created by other tests don't brake these.
            DeleteConfigurationFile();
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        [TestMethod]
        public void ReadConfigurationXmlReturnsContentsOfApplicationInsightsConfigFileInApplicationInstallationDirectory()
        {
            const string TestFileContent = "42";
            CreateConfigurationFile(TestFileContent);
            var platform = new PlatformImplementation();

            string s = platform.ReadConfigurationXml();
            
            Assert.AreEqual(TestFileContent, s);
        }

        [TestMethod]
        public void ReadConfigurationXmlIgnoresMissingApplicationInsightsConfigurationFileByReturningEmptyString()
        {
            var platform = new PlatformImplementation();

            string configuration = platform.ReadConfigurationXml();
            
            Assert.AreEqual(0, configuration.Length);
        }

        [TestMethod]
        public void FailureToReadEnvironmentVariablesDoesNotThrowExceptions()
        {
            EnvironmentPermission permission = new EnvironmentPermission(EnvironmentPermissionAccess.NoAccess, "PATH");
            try
            {
                permission.PermitOnly();
                PlatformImplementation platform = new PlatformImplementation();
                Assert.IsFalse(platform.TryGetEnvironmentVariable("PATH", out string value));
                Assert.IsNull(value);
                permission = null;
            }
            finally
            {
                EnvironmentPermission.RevertAll();
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing == true)
            {
                DeleteConfigurationFile();
            }
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
            File.Delete(Path.Combine(Environment.CurrentDirectory, "ApplicationInsights.config"));
        }

        private static Stream OpenConfigurationFile()
        {
            return File.OpenWrite(Path.Combine(Environment.CurrentDirectory, "ApplicationInsights.config"));
        }
    }

#endif
}
