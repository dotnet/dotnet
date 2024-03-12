namespace Microsoft.ApplicationInsights.WindowsServer.Implementation
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Tracing;
    using Microsoft.ApplicationInsights.Common;

    /// <summary>
    /// ETW EventSource tracing class.
    /// </summary>
#if REDFIELD
    [EventSource(Name = "Redfield-Microsoft-ApplicationInsights-Extensibility-WindowsServer")]
#else
    [EventSource(Name = "Microsoft-ApplicationInsights-Extensibility-WindowsServer")]
#endif
    [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", Justification = "appDomainName is required")]
    internal sealed class WindowsServerEventSource : EventSource
    {
        /// <summary>
        /// Instance of the WindowsServerEventSource class.
        /// </summary>
        public static readonly WindowsServerEventSource Log = new WindowsServerEventSource();
        private readonly ApplicationNameProvider applicationNameProvider = new ApplicationNameProvider();

        private WindowsServerEventSource()
        {
        }

        [Event(1, Message = "{0} loaded.", Level = EventLevel.Verbose)]
        public void TelemetryInitializerLoaded(string typeName, string appDomainName = "Incorrect")
        {
            this.WriteEvent(1, typeName ?? string.Empty, this.applicationNameProvider.Name);
        }

        [Event(2, Message = "[msg=TypeNotFound;{0};]", Level = EventLevel.Verbose)]
        public void TypeExtensionsTypeNotLoaded(string typeName, string appDomainName = "Incorrect")
        {
            this.WriteEvent(2, typeName ?? string.Empty, this.applicationNameProvider.Name);
        }

        [Event(3, Message = "[msg=AssemblyNotFound;{0};]", Level = EventLevel.Verbose)]
        public void TypeExtensionsAssemblyNotLoaded(string assemblyName, string appDomainName = "Incorrect")
        {
            this.WriteEvent(3, assemblyName ?? string.Empty, this.applicationNameProvider.Name);
        }

        [Event(
            4,
            Keywords = Keywords.UserActionable,
            Message = "BuildInfo.config file has incorrect xml structure. Context component version will not be populated. Exception: {0}.",
            Level = EventLevel.Error)]
        public void BuildInfoConfigBrokenXmlError(string msg, string appDomainName = "Incorrect")
        {
            this.WriteEvent(4, msg ?? string.Empty, this.applicationNameProvider.Name);
        }

        [Event(
            5,
            Message = "[msg=BuildInfoConfigLoaded];[path={0}]",
            Level = EventLevel.Verbose)]
        public void BuildInfoConfigLoaded(string path, string appDomainName = "Incorrect")
        {
            this.WriteEvent(5, path ?? string.Empty, this.applicationNameProvider.Name);
        }

        [Event(
            6,
            Message = "[msg=BuildInfoConfigLoaded];[path={0}]",
            Level = EventLevel.Verbose)]
        public void BuildInfoConfigNotFound(string path, string appDomainName = "Incorrect")
        {
            this.WriteEvent(6, path ?? string.Empty, this.applicationNameProvider.Name);
        }

        [Event(
            7,
            Message = "[WmiError={0}]",
            Level = EventLevel.Warning)]
        public void DeviceContextWmiFailureWarning(string error, string appDomainName = "Incorrect")
        {
            this.WriteEvent(7, error ?? string.Empty, this.applicationNameProvider.Name);
        }

        [Event(
            8,
            Message = "[TaskSchedulerOnUnobservedTaskException tracked.]",
            Level = EventLevel.Verbose)]
        public void TaskSchedulerOnUnobservedTaskException(string appDomainName = "Incorrect")
        {
            this.WriteEvent(8, this.applicationNameProvider.Name);
        }

        [Event(
            9,
            Message = "[CurrentDomainOnUnhandledException tracked.]",
            Level = EventLevel.Verbose)]
        public void CurrentDomainOnUnhandledException(string appDomainName = "Incorrect")
        {
            this.WriteEvent(9, this.applicationNameProvider.Name);
        }

        [Event(
            10,
            Message = "FirstChance exception statistics callback was called, but exception object is null.",
            Level = EventLevel.Verbose)]
        public void FirstChanceExceptionCallbackExeptionIsNull(string appDomainName = "Incorrect")
        {
            this.WriteEvent(10, this.applicationNameProvider.Name);
        }

        [Event(
            11,
            Message = "FirstChance exception statistics callback was called.",
            Level = EventLevel.Verbose)]
        public void FirstChanceExceptionCallbackCalled(string appDomainName = "Incorrect")
        {
            this.WriteEvent(11, this.applicationNameProvider.Name);
        }

        [Event(
            12,
            Message = "FirstChance exception statistics callback failed with the exception {0}.",
            Level = EventLevel.Error)]
        public void FirstChanceExceptionCallbackException(string exception, string appDomainName = "Incorrect")
        {
            this.WriteEvent(12, exception, this.applicationNameProvider.Name);
        }

        [Event(
            13,
            Message = "[UnobservedTaskException threw another exception:  {0}.]",
            Level = EventLevel.Error)]
        public void UnobservedTaskExceptionThrewUnhandledException(string exception, string appDomainName = "Incorrect")
        {
            this.WriteEvent(13, exception, this.applicationNameProvider.Name);
        }

        [Event(14, Message = "Unknown error occured in {0}. Exception: {0}", Level = EventLevel.Error)]
        public void UnknownErrorOccured(string source, string exception, string applicationName = "Incorrect")
        {
            this.WriteEvent(14, source, exception, this.applicationNameProvider.Name);
        }

        [Event(15, Level = EventLevel.Warning, Message = @"Accessing environment variable - {0} failed with exception: {1}.")]
        public void AccessingEnvironmentVariableFailedWarning(
            string environmentVariable,
            string exceptionMessage,
            string applicationName = "Incorrect")
        {
            this.WriteEvent(15, environmentVariable, exceptionMessage, this.applicationNameProvider.Name);
        }

        [Event(16, Message = "AzureRoleEnvironmentTelemetryInitializer will not be initialized as application is determined to be running in Azure WebApps.", Level = EventLevel.Informational)]
        public void AzureRoleEnvironmentTelemetryInitializerNotInitializedInWebApp(string applicationName = "Incorrect")
        {
            this.WriteEvent(16, this.applicationNameProvider.Name);
        }

        [Event(
            17,
            Message = "Successfully loaded assembly {0} from location {1} into AppDomain {2}.",
            Level = EventLevel.Informational)]
        public void AssemblyLoadSuccess(string assembly, string location, string appDomain, string applicationName = "Incorrect")
        {
            this.WriteEvent(17, assembly, location, appDomain, this.applicationNameProvider.Name);
        }

        [Event(
           18,
           Message = "Failed loading assembly {0} with exception: {1}",
           Level = EventLevel.Informational)]
        public void AssemblyLoadAttemptFailed(string assembly, string exceptionMessage, string applicationName = "Incorrect")
        {
            this.WriteEvent(18, assembly, exceptionMessage, this.applicationNameProvider.Name);
        }

        [Event(
           19,
           Message = "Failed loading any version of assembly {0}",
           Level = EventLevel.Informational)]
        public void AssemblyLoadFailedAllVersion(string assembly, string applicationName = "Incorrect")
        {
            this.WriteEvent(19, assembly, this.applicationNameProvider.Name);
        }

        [Event(
           20,
           Message = "AzureRoleEnvironmentContextReader initialize successfully completed reading context. RoleName: {0}, RoleInstanceName: {1}",
           Level = EventLevel.Informational)]
        public void AzureRoleEnvironmentContextReaderInitializedSuccess(string roleName, string roleInstanceName, string applicationName = "Incorrect")
        {
            this.WriteEvent(20, roleName, roleInstanceName, this.applicationNameProvider.Name);
        }

        [Event(
           21,
           Message = "AzureRoleEnvironmentContextReader failed to populate context. Application is assumed not be running in Azure Cloud service.",
           Level = EventLevel.Informational)]
        public void AzureRoleEnvironmentContextReaderInitializationFailed(string applicationName = "Incorrect")
        {
            this.WriteEvent(21, this.applicationNameProvider.Name);
        }

        [Event(
           22,
           Message = "AppDomain {0}, Message {1}",
           Level = EventLevel.Informational)]
        public void AzureRoleEnvironmentContextReaderAppDomainTroubleshoot(string domainName, string msg, string applicationName = "Incorrect")
        {
            this.WriteEvent(22, domainName, msg, this.applicationNameProvider.Name);
        }

        [Event(
           23,
           Message = "AzureRoleEnvironmentContextReader initialization took {0} msec.",
           Level = EventLevel.Informational)]
        public void AzureRoleEnvironmentContextReaderInitializationDuration(long durationMsec, string applicationName = "Incorrect")
        {
            this.WriteEvent(23, durationMsec, this.applicationNameProvider.Name);
        }

        [Event(
            24,
            Message = "System doesn't have access to Azure Instance Metadata Service. Azure VM instance metadata fields will not be added to heartbeat data.",
            Level = EventLevel.Informational)]
        public void CannotObtainAzureInstanceMetadata(string applicationName = "Incorrect")
        {
            this.WriteEvent(
                24,
                this.applicationNameProvider.Name);
        }

        [Event(
            25,
            Message = "Request to obtain information from the Azure Instance Metadata Service failed. Request URI is '{0}', exception: {1} (inner: '{2}')",
            Level = EventLevel.Informational)]
        public void AzureInstanceMetadataRequestFailure(string requestUrl, string ex, string innerEx, string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                25,
                requestUrl,
                ex ?? string.Empty,
                innerEx ?? string.Empty,
                this.applicationNameProvider.Name);
        }

        [Event(
            26,
            Message = "Azure IMS returned unexpected number of fields, expected:{0} recieved:{1}.",
            Level = EventLevel.Informational)]
        public void AzureInstanceMetadataFieldCountUnexpected(int expectedCount, int receivedCount, string applicationName = "Incorrect")
        {
            this.WriteEvent(
                26,
                expectedCount,
                receivedCount,
                this.applicationNameProvider.Name);
        }

        [Event(
            27,
            Message = "Azure IMS returned at least one field that was not expected, first unexpected field encountered: '{0}'.",
            Level = EventLevel.Informational)]
        public void AzureInstanceMetadataFieldNameUnexpected(string unexpectedFieldName, string applicationName = "Incorrect")
        {
            this.WriteEvent(
                27,
                unexpectedFieldName,
                this.applicationNameProvider.Name);
        }

        [Event(
            28,
            Message = "Azure IMS returned field '{0}' with an invalid/unexpected value. Not adding this value to heartbeat properties.",
            Level = EventLevel.Warning)]
        public void AzureInstanceMetadataValueForFieldInvalid(string fieldWithInvalidValue, string applicationName = "Incorrect")
        {
            this.WriteEvent(
                28,
                fieldWithInvalidValue,
                this.applicationNameProvider.Name);
        }

        [Event(
            29,
            Message = "Azure IMS field and value not added to heartbeat properties. Field name:'{0}', value:'{1}'.",
            Level = EventLevel.Informational)]
        public void AzureInstanceMetadataWasntAddedToHeartbeatProperties(string azureImsFieldName, string azureImsFieldValue, string applicationName = "Incorrect")
        {
            this.WriteEvent(
                29,
                azureImsFieldName,
                azureImsFieldValue,
                this.applicationNameProvider.Name);
        }

        [Event(
            30,
            Message = "Azure IMS data not added to heartbeat properties. Failure to obtain Azure IMS data occurred.",
            Level = EventLevel.Informational)]
        public void AzureInstanceMetadataNotAdded(string applicationName = "Incorrect")
        {
            this.WriteEvent(
                30,
                this.applicationNameProvider.Name);
        }

        [Event(
            31,
            Message = "Azure IMS data not added to heartbeat properties. Exception occurred: {0} (1st Inner exception: {1}).",
            Level = EventLevel.Informational)]
        public void AzureInstanceMetadataFailureWithException(string exception, string innerException, string applicationName = "Incorrect")
        {
            this.WriteEvent(
                31,
                exception ?? string.Empty,
                innerException ?? string.Empty,
                this.applicationNameProvider.Name);
        }

        [Event(
            32,
            Message = "Azure IMS data not added to heartbeat properties, there was a failure obtaining and setting them. Exception occurred: {0} (1st Inner exception: {1}).",
            Level = EventLevel.Informational)]
        public void AzureInstanceMetadataFailureSettingDefaultPayload(string exception, string innerException, string applicationName = "Incorrect")
        {
            this.WriteEvent(
                32,
                exception ?? string.Empty,
                innerException ?? string.Empty,
                this.applicationNameProvider.Name);
        }

        [Event(33,
            Message = "App Services Heartbeat Provider: Failed to obtain Azure App Services environment variable '{0}'. Exception raised: {1}",
            Level = EventLevel.Warning)]
        public void AppServiceHeartbeatPropertyAquisitionFailed(string envVarName, string exceptionStr, string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                33,
                envVarName ?? "unknown",
                exceptionStr ?? "unknown-exception",
                this.applicationNameProvider.Name);
        }

        [Event(34,
            Message = "App Services Heartbeat Provider: Could not obtain the Heartbeat Manager instance during initialization. Exception raised: {0}",
            Level = EventLevel.Warning)]
        public void AppServiceHeartbeatManagerAccessFailure(string exceptionStr, string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                34,
                exceptionStr ?? "unknown-exception",
                this.applicationNameProvider.Name);
        }
        
        [Event(35,
            Message = "App Services Heartbeat Provider: Accessing the Hearbeat Manager failed as it is not in the list of available modules.",
            Level = EventLevel.Warning)]
        public void AppServiceHeartbeatManagerNotAvailable(string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                35,
                this.applicationNameProvider.Name);
        }

        [Event(36,
            Message = "App Services Heartbeat Provider: Failed to set Azure App Services heartbeat values. Exception encountered: {0}",
            Level = EventLevel.Warning)]
        public void AppServiceHeartbeatPropertySettingFails(string exceptionStr, string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                36,
                exceptionStr ?? "unknown-exception",
                this.applicationNameProvider.Name);
        }

        [Event(37,
            Message = "App Services Heartbeat Provider: Request to set heartbeat properties when the heartbeat property manager is null.",
            Level = EventLevel.Warning)]
        public void AppServiceHeartbeatSetCalledWithNullManager(string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                37,
                this.applicationNameProvider.Name);
        }

        [Event(38,
            Message = "Error occured when disposing update interval timer within EnvironmentVariableMonitor. Exception: {0}",
            Level = EventLevel.Warning)]
        public void EnvironmentVarMonitorFailedDispose(string exceptionMsg, string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                38,
                exceptionMsg ?? "unknown-exception",
                this.applicationNameProvider.Name);
        }

        [Event(39,
            Message = "Security exception was thrown trying to read environment variable '{0}'. Disabling environment variable monitor to avoid future security exceptions. Exception: {1}",
            Level = EventLevel.Warning)]
        public void SecurityExceptionThrownAccessingEnvironmentVariable(string environmentVariableName, string exceptionMsg, string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                39,
                environmentVariableName,
                exceptionMsg ?? "unknown-exception",
                this.applicationNameProvider.Name);
        }

        [Event(40,
            Message = "Error occurred when trying to update environment variables. Exception: {0}",
            Level = EventLevel.Warning)]
        public void GeneralFailureOccursDuringCheckForEnvironmentVariables(string exceptionMsg, string appDomainName = "Incorrect")
        {
            this.WriteEvent(
                40,
                exceptionMsg ?? "unknown-exception",
                this.applicationNameProvider.Name);
        }

        /// <summary>
        /// Keywords for the PlatformEventSource. Those keywords should match keywords in Core.
        /// </summary>
        public sealed class Keywords
        {
            /// <summary>
            /// Key word for user actionable events.
            /// </summary>
            public const EventKeywords UserActionable = (EventKeywords)0x1;
        }
    }
}
