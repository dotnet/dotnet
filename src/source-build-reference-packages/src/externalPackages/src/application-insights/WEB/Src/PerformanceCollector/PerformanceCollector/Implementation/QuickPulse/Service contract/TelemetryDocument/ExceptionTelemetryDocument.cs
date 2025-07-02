namespace Microsoft.ManagementServices.RealTimeDataProcessing.QuickPulseService
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.Serialization;

    [DataContract]
    internal struct ExceptionTelemetryDocument : ITelemetryDocument
    {
        [DataMember(EmitDefaultValue = false)]
        public Guid Id { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Version { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public string SeverityLevel { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public string Exception { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string ExceptionType { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string ExceptionMessage { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string OperationId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public KeyValuePair<string, string>[] Properties { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string OperationName { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string InternalNodeName { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string CloudRoleName { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string CloudRoleInstance { get; set; }

        [DataMember(EmitDefaultValue = false)]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "DataMember cannot be static")]
        public string DocumentType
        {
            get
            {
                return TelemetryDocumentType.Exception.ToString();
            }

            private set
            {
            }
        }

        [DataMember(EmitDefaultValue = false)]
        public string[] DocumentStreamIds { get; set; }
    }
}
