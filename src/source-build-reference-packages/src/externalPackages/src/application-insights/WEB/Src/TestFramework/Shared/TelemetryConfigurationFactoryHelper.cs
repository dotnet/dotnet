namespace Microsoft.ApplicationInsights.TestFramework
{
    using System;
    using System.Reflection;
    using System.Text;

    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;

    public static class TelemetryConfigurationFactoryHelper
    {
        /// <summary>
        /// TelemetryConfigurationFactory is an internal class in the BaseSDK.
        /// This method using reflection to access the Initialize method.
        /// This enables E2E testing using a sample config file.
        /// </summary>
        public static void Initialize(TelemetryConfiguration configuration, TelemetryModules modules, string serializedConfiguration)
        {
            // get the assembly qualified name using known public type:
            var typeName = typeof(Microsoft.ApplicationInsights.Extensibility.TelemetryConfiguration).AssemblyQualifiedName
                .Replace(
                    "Microsoft.ApplicationInsights.Extensibility.TelemetryConfiguration",
                    "Microsoft.ApplicationInsights.Extensibility.Implementation.TelemetryConfigurationFactory");

            var telemetryConfigurationFactoryT = Type.GetType(typeName);
            if (telemetryConfigurationFactoryT == null)
            {
                throw new ArgumentException($"Microsoft.ApplicationInsights.Extensibility.Implementation.TelemetryConfigurationFactory type not found");
            }

            var telemetryConfigurationFactoryInstanceProperty = telemetryConfigurationFactoryT.GetProperty("Instance");
            if (telemetryConfigurationFactoryInstanceProperty == null)
            {
                throw new ArgumentException($"Property 'Instance' not found in type {telemetryConfigurationFactoryT.FullName}");
            }

            var telemetryConfigurationFactoryInstance = telemetryConfigurationFactoryInstanceProperty.GetValue(null);
            if (telemetryConfigurationFactoryInstance == null)
            {
                throw new ArgumentException($"Microsoft.ApplicationInsights.Extensibility.Implementation.TelemetryConfigurationFactory.Instance should not be null");
            }

            var initTypes = new[]
                {
                    typeof(Microsoft.ApplicationInsights.Extensibility.TelemetryConfiguration),
                    typeof(Microsoft.ApplicationInsights.Extensibility.Implementation.TelemetryModules),
                    typeof(string)
                };
            var initMethod = telemetryConfigurationFactoryInstance.GetType().GetMethod("Initialize", initTypes);
            if (initMethod == null)
            {
                throw new ArgumentException($"Microsoft.ApplicationInsights.Extensibility.TelemetryConfiguration.Initialize method not found");
            }

            // initialize the AppInsights using config string:
            _ = initMethod.Invoke(telemetryConfigurationFactoryInstance, new object[] { configuration, modules, serializedConfiguration });
        }

        /// <summary>
        /// Build a configuration xml string representing the contents of a config file.
        /// All parameters are optional, allowing you to configure only what is needed for a specific test.
        /// </summary>
        public static string BuildConfiguration(string ikey = null, string connectionString = null, string module = null, string processor = null)
        {
            var sb = new StringBuilder();
            sb.AppendLine(@"<?xml version=""1.0"" encoding=""utf-8"" ?>");
            sb.AppendLine(@"<ApplicationInsights xmlns=""http://schemas.microsoft.com/ApplicationInsights/2013/Settings"">");

            if (ikey != null)
            {
                sb.AppendLine($"<InstrumentationKey>{ikey}</InstrumentationKey>");
            }

            if (connectionString != null)
            {
                sb.AppendLine($"<ConnectionString>{connectionString}</ConnectionString>");
            }

            if (module != null)
            {
                sb.AppendLine("<TelemetryModules>");
                sb.AppendLine(module);
                sb.AppendLine("</TelemetryModules>");
            }

            if (processor != null)
            {
                sb.AppendLine("<TelemetryProcessors>");
                sb.AppendLine(processor);
                sb.AppendLine("</TelemetryProcessors>");
            }

            sb.AppendLine("</ApplicationInsights>");
            return sb.ToString();
        }
    }
}