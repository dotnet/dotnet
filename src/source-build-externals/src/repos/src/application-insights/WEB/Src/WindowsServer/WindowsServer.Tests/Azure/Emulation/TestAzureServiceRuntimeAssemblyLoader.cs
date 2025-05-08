#if NETFRAMEWORK
namespace Microsoft.ApplicationInsights.WindowsServer.Azure.Emulation
{
    using System;
    using System.Globalization;
    using System.Reflection;        
    using Microsoft.ApplicationInsights.WindowsServer.Implementation;

    /// <summary>
    /// Used for testing AzureServiceRuntimeAssemblyLoader. Loads a random assembly and if successful populates
    /// context with test values.
    /// </summary>
    internal class TestAzureServiceRuntimeAssemblyLoader : AzureServiceRuntimeAssemblyLoader
    {
        public TestAzureServiceRuntimeAssemblyLoader()
        {
            // Loads a random assembly. (only requirement is that this assembly is foundable in GAC)
            // Microsoft.ApplicationInsights.Log4NetAppender.dll version 2.2.0.0 with the publickeytoken below is checked in to source control to be put in GAC.
            this.AssemblyNameToLoad.Name = "Microsoft.ApplicationInsights.Log4NetAppender";
            this.AssemblyNameToLoad.CultureInfo = CultureInfo.InvariantCulture;
            this.AssemblyNameToLoad.SetPublicKeyToken(new byte[] { 49, 191, 56, 86, 173, 54, 78, 53 });
            this.VersionsToAttempt = new Version[] { new Version("2.0.0.0"), new Version("2.2.0.0") };            
        }

        public override bool ReadAndPopulateContextInformation(out string roleName, out string roleInstanceId)
        {
            roleName = string.Empty;
            roleInstanceId = string.Empty;

            Assembly loadedAssembly = null;
            try
            {
                // As this is executed inside a separate AppDomain, it is safe to load assemblies here without interfering with user code.                
                loadedAssembly = AttemptToLoadAssembly(this.AssemblyNameToLoad, this.VersionsToAttempt);
                if (loadedAssembly != null)
                {
                    // This is a test loader. Just populate test values if assembly loading is successfull.
                    roleName = "TestRoleName";
                    roleInstanceId = "TestRoleInstanceId";
                }
            }
            catch (Exception ex)
            {
                WindowsServerEventSource.Log.UnknownErrorOccured("TestAzureServiceRuntimeAssemblyLoader populate context", ex.ToString());
            }

            return loadedAssembly != null;
        }
    }
}
#endif