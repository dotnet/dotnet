#if NETFRAMEWORK
namespace Microsoft.ApplicationInsights.WindowsServer.Implementation
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using System.Threading;

    internal class AzureRoleEnvironmentContextReader : IAzureRoleEnvironmentContextReader
    {
        /// <summary>
        /// The singleton instance for our reader.
        /// </summary>
        private static IAzureRoleEnvironmentContextReader instance;

        /// <summary>
        /// The Azure role name (if any).
        /// </summary>
        private string roleName = string.Empty;

        /// <summary>
        /// The Azure role instance name (if any).
        /// </summary>
        private string roleInstanceName = string.Empty;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="AzureRoleEnvironmentContextReader"/> class.
        /// </summary>
        internal AzureRoleEnvironmentContextReader()
        {
        }

        /// <summary>
        /// Gets or sets the singleton instance for our application context reader.
        /// </summary>
        public static IAzureRoleEnvironmentContextReader Instance
        {
            get
            {
                if (AzureRoleEnvironmentContextReader.instance != null)
                {
                    return AzureRoleEnvironmentContextReader.instance;
                }

                // Allows replacement for test purposes to load a different AssemblyLoaderType.
                if (AzureRoleEnvironmentContextReader.AssemblyLoaderType == null)
                {
                    AzureRoleEnvironmentContextReader.AssemblyLoaderType = typeof(AzureServiceRuntimeAssemblyLoader);
                }

                Interlocked.CompareExchange(ref AzureRoleEnvironmentContextReader.instance, new AzureRoleEnvironmentContextReader(), null);
                AzureRoleEnvironmentContextReader.instance.Initialize();
                return AzureRoleEnvironmentContextReader.instance;
            }

            // allow for the replacement for the context reader to allow for testability
            internal set
            {
                AzureRoleEnvironmentContextReader.instance = value;
            }
        }
        
        internal static Type AssemblyLoaderType { get; set; }        

        /// <summary>
        /// Initializes the current reader with respect to its environment.
        /// </summary>
        public void Initialize()
        {
            AppDomain tempDomainToLoadAssembly = null;
            string tempDomainName = "AppInsightsDomain-" + Guid.NewGuid().ToString();
            Stopwatch sw = Stopwatch.StartNew();

            // The following approach is used to load Microsoft.WindowsAzure.ServiceRuntime assembly and read the required information.
            // Create a new AppDomain and try to load the ServiceRuntime dll into it.
            // Then using reflection, read and save all the properties we care about and unload the new AppDomain.            
            // This approach ensures that if the app is running in Azure Cloud Service, we read the necessary information deterministically
            // and without interfering with any customer code which could be loading same/different version of Microsoft.WindowsAzure.ServiceRuntime.dll.
            try
            {
                AppDomainSetup domaininfo = new AppDomainSetup();
                domaininfo.ApplicationBase = Path.GetDirectoryName(AzureRoleEnvironmentContextReader.AssemblyLoaderType.Assembly.Location);

                // Create a new AppDomain
                tempDomainToLoadAssembly = AppDomain.CreateDomain(tempDomainName, null, domaininfo);
                WindowsServerEventSource.Log.AzureRoleEnvironmentContextReaderAppDomainTroubleshoot(tempDomainName, " Successfully  created with ApplicationBase: " + domaininfo.ApplicationBase);

                // Load the RemoteWorker assembly to the new domain            
                tempDomainToLoadAssembly.Load(typeof(AzureServiceRuntimeAssemblyLoader).Assembly.FullName);
                WindowsServerEventSource.Log.AzureRoleEnvironmentContextReaderAppDomainTroubleshoot(tempDomainName, " Successfully loaded assembly: " + typeof(AzureServiceRuntimeAssemblyLoader).Assembly.FullName);

                // Any method invoked on this object will be executed in the newly created AppDomain.
                AzureServiceRuntimeAssemblyLoader remoteWorker = (AzureServiceRuntimeAssemblyLoader)tempDomainToLoadAssembly.CreateInstanceAndUnwrap(AzureRoleEnvironmentContextReader.AssemblyLoaderType.Assembly.FullName, AzureRoleEnvironmentContextReader.AssemblyLoaderType.FullName);

                bool success = remoteWorker.ReadAndPopulateContextInformation(out this.roleName, out this.roleInstanceName);
                if (success)
                {
                    WindowsServerEventSource.Log.AzureRoleEnvironmentContextReaderInitializedSuccess(this.roleName, this.roleInstanceName);
                }
                else
                {                    
                    WindowsServerEventSource.Log.AzureRoleEnvironmentContextReaderInitializationFailed();
                }
            }
            catch (Exception ex)
            {
                WindowsServerEventSource.Log.UnknownErrorOccured("AzureRoleEnvironmentContextReader Initialize", ex.ToString());
            }
            finally
            {                
                try
                {
                    if (tempDomainToLoadAssembly != null)
                    {
                        AppDomain.Unload(tempDomainToLoadAssembly);                                                
                        WindowsServerEventSource.Log.AzureRoleEnvironmentContextReaderAppDomainTroubleshoot(tempDomainName, " Successfully  Unloaded.");
                        WindowsServerEventSource.Log.AzureRoleEnvironmentContextReaderInitializationDuration(sw.ElapsedMilliseconds);                        
                    }                    
                }
                catch (Exception ex)
                {
                    WindowsServerEventSource.Log.AzureRoleEnvironmentContextReaderAppDomainTroubleshoot(tempDomainName, "AppDomain  unload failed with exception: " + ex.ToString());
                }
            }
        }

        /// <summary>
        /// Gets the Azure role name.
        /// </summary>
        /// <returns>The extracted data.</returns>
        public string GetRoleName()
        {
            return this.roleName;
        }

        /// <summary>
        /// Gets the Azure role instance name.
        /// </summary>
        /// <returns>The extracted data.</returns>
        public string GetRoleInstanceName()
        {
            return this.roleInstanceName;
        }
    }    
}
#endif