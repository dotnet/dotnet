namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading.Tasks;

    internal class BaseDefaultHeartbeatPropertyProvider : IHeartbeatDefaultPayloadProvider
    {
        internal readonly List<string> DefaultFields = new List<string>()
        {
            "runtimeFramework",
            "baseSdkTargetFramework",
            "osType",
            "processSessionId",
        };

        /// <summary>
        /// A unique identifier that would help to indicate to the analytics when the current process session has
        /// restarted. 
        /// 
        /// <remarks>If a process is unstable and is being restared frequently, tracking this property
        /// in the heartbeat would help to identify this unstability.
        /// </remarks>
        /// </summary>
        private static Guid? uniqueProcessSessionId = null;

        public string Name => "Base";

        public bool IsKeyword(string keyword)
        {
            return this.DefaultFields.Contains(keyword, StringComparer.OrdinalIgnoreCase);
        }

        public Task<bool> SetDefaultPayload(IEnumerable<string> disabledFields, IHeartbeatProvider provider)
        {
            bool hasSetValues = false;
            var enabledProperties = this.DefaultFields.Except(disabledFields);

            foreach (string fieldName in enabledProperties)
            {
                // we don't need to report out any failure here, so keep this look within the Sdk Internal Operations as well
                try
                {
                    switch (fieldName)
                    {
                        case "runtimeFramework":
                            provider.AddHeartbeatProperty(fieldName, true, GetRuntimeFrameworkVer(), true);
                            hasSetValues = true;
                            break;
                        case "baseSdkTargetFramework":
                            provider.AddHeartbeatProperty(fieldName, true, GetBaseSdkTargetFramework(), true);
                            hasSetValues = true;
                            break;
                        case "osType":
                            provider.AddHeartbeatProperty(fieldName, true, GetRuntimeOsType(), true);
                            hasSetValues = true;
                            break;
                        case "processSessionId":
                            provider.AddHeartbeatProperty(fieldName, true, GetProcessSessionId(), true);
                            hasSetValues = true;
                            break;
                        default:
                            provider.AddHeartbeatProperty(fieldName, true, "UNDEFINED", true);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    CoreEventSource.Log.FailedToObtainDefaultHeartbeatProperty(fieldName, ex.ToString());
                }
            }

            return Task.FromResult(hasSetValues);
        }

        /// <summary>
        /// This will return the current running .NET framework version, based on the version of the assembly that owns
        /// the 'Object' type. The version number returned can be used to infer other things such as .NET Core / Standard.
        /// </summary>
        /// <returns>a string representing the version of the current .NET framework.</returns>
        private static string GetRuntimeFrameworkVer()
        {
#if NETFRAMEWORK
            Assembly assembly = typeof(Object).GetTypeInfo().Assembly;
            AssemblyFileVersionAttribute objectAssemblyFileVer =
                        assembly.GetCustomAttributes(typeof(AssemblyFileVersionAttribute))
                                .Cast<AssemblyFileVersionAttribute>()
                                .FirstOrDefault();
            return objectAssemblyFileVer != null ? objectAssemblyFileVer.Version : "undefined";
#elif NETSTANDARD
            return System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription;
#else
#error Unrecognized framework
            return "unknown";
#endif
        }

        /// <summary>
        /// Returns the current target framework that the assembly was built against.
        /// </summary>
        /// <returns>standard string representing the target framework.</returns>
        private static string GetBaseSdkTargetFramework()
        {
#if NET452
            return "net452";
#elif NET46
            return "net46";
#elif NETSTANDARD2_0
            return "netstandard2.0";
#else
#error Unrecognized framework
            return "undefined";
#endif
        }

        /// <summary>
        /// Runtime information for the underlying OS, should include Linux information here as well.
        /// Note that in NET452/46 the PlatformId is returned which have slightly different (more specific,
        /// such as Win32NT/Win32S/MacOSX/Unix) values than in NETSTANDARD assemblies where you will get
        /// the OS platform Windows/Linux/OSX.
        /// </summary>
        /// <returns>String representing the OS or 'unknown'.</returns>
        private static string GetRuntimeOsType()
        {
            string osValue = "unknown";
#if NETFRAMEWORK

            osValue = Environment.OSVersion.Platform.ToString();

#elif NETSTANDARD
            
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                osValue = OSPlatform.Linux.ToString();
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                osValue = OSPlatform.OSX.ToString();
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                osValue = OSPlatform.Windows.ToString();
            }
            else
            {
                osValue = RuntimeInformation.OSDescription ?? "unknown";
            }

#else
#error Unrecognized framework
#endif
            return osValue;
        }

        /// <summary>
        /// Return a unique process session identifier that will only be set once in the lifetime of a 
        /// single executable session.
        /// </summary>
        /// <returns>string representation of a unique id.</returns>
        private static string GetProcessSessionId()
        {
            if (BaseDefaultHeartbeatPropertyProvider.uniqueProcessSessionId == null)
            {
                BaseDefaultHeartbeatPropertyProvider.uniqueProcessSessionId = Guid.NewGuid();
            }

            return BaseDefaultHeartbeatPropertyProvider.uniqueProcessSessionId.ToString();
        }
    }
}
