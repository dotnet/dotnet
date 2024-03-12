namespace Microsoft.ManagementServices.RealTimeDataProcessing.QuickPulseService
{
    using System;
    using System.Collections.Generic;

    internal interface ITelemetryDocument
    {
        Guid Id { get; }

        string Version { get; }

        string OperationName { get; set; }

        string InternalNodeName { get; set; }

        string CloudRoleName { get; set; }

        string CloudRoleInstance { get; set; }
        
        KeyValuePair<string, string>[] Properties { get; set; }
        
        string DocumentType { get; }

        string[] DocumentStreamIds { get; set; }
    }
}
