#if NETFRAMEWORK
namespace Microsoft.ApplicationInsights.WindowsServer.Implementation
{
    using System;    
    using System.Globalization;
    using System.Reflection;

    /// <summary>
    /// Contains logic to load AzureServiceRuntime assembly and read context using reflection.
    /// Inherits MarshalByRefObject so that methods of this class can be executed remotely in separate AppDomain.
    /// </summary>    
    internal class AzureServiceRuntimeAssemblyLoader : MarshalByRefObject
    {
        public AssemblyName AssemblyNameToLoad = new AssemblyName();
        public Version[] VersionsToAttempt;

        public AzureServiceRuntimeAssemblyLoader()
        {
            this.AssemblyNameToLoad.Name = "Microsoft.WindowsAzure.ServiceRuntime";
            this.AssemblyNameToLoad.CultureInfo = CultureInfo.InvariantCulture;
            this.AssemblyNameToLoad.SetPublicKeyToken(new byte[] { 49, 191, 56, 86, 173, 54, 78, 53 });
            this.VersionsToAttempt = new Version[] { new Version("2.7.0.0"), new Version("2.6.0.0"), new Version("2.5.0.0") };
        }

        public virtual bool ReadAndPopulateContextInformation(out string roleName, out string roleInstanceId)
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
                    ServiceRuntime serviceRuntime = new ServiceRuntime(loadedAssembly);
                    RoleEnvironment roleEnvironment = serviceRuntime.GetRoleEnvironment();

                    if (roleEnvironment.IsAvailable == true)
                    {
                        RoleInstance roleInstance = roleEnvironment.CurrentRoleInstance;
                        if (roleInstance != null)
                        {
                            roleInstanceId = roleInstance.Id;
                            Role role = roleInstance.Role;
                            roleName = role.Name;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                WindowsServerEventSource.Log.UnknownErrorOccured("AzureServiceRuntimeAssemblyLoader populate context", ex.ToString());
            }

            return loadedAssembly != null;
        }

        protected static Assembly AttemptToLoadAssembly(AssemblyName assemblyNameToLoad, Version[] versionsToAttempt)
        {
            Assembly loadedAssembly = null;                        
            foreach (Version version in versionsToAttempt)
            {                
                try
                {
                    assemblyNameToLoad.Version = version;
                    var ss = assemblyNameToLoad.GetPublicKeyToken();

                    loadedAssembly = Assembly.Load(assemblyNameToLoad);
                    if (loadedAssembly != null)
                    {
                        // Found the assembly, stop probing and return the assembly.
                        WindowsServerEventSource.Log.AssemblyLoadSuccess(assemblyNameToLoad.FullName, loadedAssembly.Location, AppDomain.CurrentDomain.FriendlyName);
                        return loadedAssembly;
                    }
                }
                catch (Exception ex)
                {
                    WindowsServerEventSource.Log.AssemblyLoadAttemptFailed(assemblyNameToLoad.FullName, ex.Message);
                }
            }

            // Failed to load assembly.
            WindowsServerEventSource.Log.AssemblyLoadFailedAllVersion(assemblyNameToLoad.Name);
            return loadedAssembly;
        }
    }
}
#endif