namespace Microsoft.ApplicationInsights.WindowsServer
{
    using System;
    using System.ComponentModel;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.Extensibility;
    
    /// <summary>
    /// Obsolete. A telemetry context initializer that used to populate role instance name. Preserved for backward compatibility. 
    /// Note that role instance will still be populated with the machine name as in the previous versions.
    /// </summary>
    [Obsolete("A telemetry context initializer that used to populate role instance name. Preserved for backward compatibility.")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class DomainNameRoleInstanceTelemetryInitializer : ITelemetryInitializer
    {   
        /// <summary>
        /// Obsolete method.
        /// </summary>
        /// <param name="telemetry">The telemetry to initialize.</param>
        public void Initialize(ITelemetry telemetry)
        {            
        }
    }
}
