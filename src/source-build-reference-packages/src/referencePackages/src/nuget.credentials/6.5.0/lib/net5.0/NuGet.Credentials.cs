// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------
[assembly: System.Runtime.CompilerServices.CompilationRelaxations(8)]
[assembly: System.Runtime.CompilerServices.RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: System.Diagnostics.Debuggable(System.Diagnostics.DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: System.CLSCompliant(true)]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("NuGet.Credentials.Test, PublicKey=0024000004800000940000000602000000240000525341310004000001000100b5fc90e7027f67871e773a8fde8938c81dd402ba65b9201d60593e96c492651e889cc13f1415ebb53fac1131ae0bd333c5ee6021672d9718ea31a8aebd0da0072f25d87dba6fc90ffd598ed4da35e44c398c454307e8e33b8426143daec9f596836f97c8f74750e5975c64e2189f45def46b2a2b1247adc3652bf5c308055da9")]
[assembly: System.Runtime.Versioning.TargetFramework(".NETCoreApp,Version=v5.0", FrameworkDisplayName = ".NET 5.0")]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyConfiguration("release")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation. All rights reserved.")]
[assembly: System.Reflection.AssemblyDescription("NuGet client's authentication models.")]
[assembly: System.Reflection.AssemblyFileVersion("6.5.0.154")]
[assembly: System.Reflection.AssemblyInformationalVersion("6.5.0+069970c727b254636c1ad29c5a7a767081482a9a.069970c727b254636c1ad29c5a7a767081482a9a")]
[assembly: System.Reflection.AssemblyProduct("NuGet")]
[assembly: System.Reflection.AssemblyTitle("NuGet.Credentials")]
[assembly: System.Reflection.AssemblyMetadata("RepositoryUrl", "https://github.com/NuGet/NuGet.Client")]
[assembly: System.Resources.NeutralResourcesLanguage("en-US")]
[assembly: System.Reflection.AssemblyVersionAttribute("6.5.0.154")]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace NuGet.Credentials
{
    public partial class CredentialResponse
    {
        public CredentialResponse(CredentialStatus status) { }

        public CredentialResponse(System.Net.ICredentials credentials) { }

        public System.Net.ICredentials Credentials { get { throw null; } }

        public CredentialStatus Status { get { throw null; } }
    }

    public static partial class CredentialsConstants
    {
        public static readonly int ProviderTimeoutSecondsDefault;
        public static readonly string ProviderTimeoutSecondsEnvar;
        public static readonly string ProviderTimeoutSecondsSetting;
    }

    public partial class CredentialService : Configuration.ICredentialService
    {
        public CredentialService(Common.AsyncLazy<System.Collections.Generic.IEnumerable<ICredentialProvider>> providers, bool nonInteractive, bool handlesDefaultCredentials) { }

        public bool HandlesDefaultCredentials { get { throw null; } }

        public System.Threading.Tasks.Task<System.Net.ICredentials> GetCredentialsAsync(System.Uri uri, System.Net.IWebProxy proxy, Configuration.CredentialRequestType type, string message, System.Threading.CancellationToken cancellationToken) { throw null; }

        public bool TryGetLastKnownGoodCredentialsFromCache(System.Uri uri, bool isProxy, out System.Net.ICredentials credentials) { throw null; }
    }

    public enum CredentialStatus
    {
        Success = 0,
        ProviderNotApplicable = 1,
        UserCanceled = 2
    }

    public static partial class DefaultCredentialServiceUtility
    {
        public static void SetupDefaultCredentialService(Common.ILogger logger, bool nonInteractive) { }

        public static void UpdateCredentialServiceDelegatingLogger(Common.ILogger log) { }
    }

    public partial class DefaultNetworkCredentialsCredentialProvider : ICredentialProvider
    {
        public DefaultNetworkCredentialsCredentialProvider() { }

        public string Id { get { throw null; } }

        public System.Threading.Tasks.Task<CredentialResponse> GetAsync(System.Uri uri, System.Net.IWebProxy proxy, Configuration.CredentialRequestType type, string message, bool isRetry, bool nonInteractive, System.Threading.CancellationToken cancellationToken) { throw null; }
    }

    public partial interface ICredentialProvider
    {
        string Id { get; }

        System.Threading.Tasks.Task<CredentialResponse> GetAsync(System.Uri uri, System.Net.IWebProxy proxy, Configuration.CredentialRequestType type, string message, bool isRetry, bool nonInteractive, System.Threading.CancellationToken cancellationToken);
    }

    public partial class PluginCredentialProvider : ICredentialProvider
    {
        public PluginCredentialProvider(Common.ILogger logger, string path, int timeoutSeconds, string verbosity) { }

        public string Id { get { throw null; } }

        public string Path { get { throw null; } }

        public int TimeoutSeconds { get { throw null; } }

        public virtual int Execute(System.Diagnostics.ProcessStartInfo startInfo, System.Threading.CancellationToken cancellationToken, out string stdOut) { throw null; }

        public System.Threading.Tasks.Task<CredentialResponse> GetAsync(System.Uri uri, System.Net.IWebProxy proxy, Configuration.CredentialRequestType type, string message, bool isRetry, bool nonInteractive, System.Threading.CancellationToken cancellationToken) { throw null; }
    }

    public partial class PluginCredentialProviderBuilder
    {
        public PluginCredentialProviderBuilder(Configuration.IExtensionLocator extensionLocator, Configuration.ISettings settings, Common.ILogger logger, Common.IEnvironmentVariableReader envarReader) { }

        public PluginCredentialProviderBuilder(Configuration.IExtensionLocator extensionLocator, Configuration.ISettings settings, Common.ILogger logger) { }

        public System.Collections.Generic.IEnumerable<ICredentialProvider> BuildAll(string verbosity) { throw null; }
    }

    public partial class PluginCredentialRequest
    {
        public PluginCredentialRequest() { }

        public bool IsRetry { get { throw null; } set { } }

        public bool NonInteractive { get { throw null; } set { } }

        public string Uri { get { throw null; } set { } }

        public string Verbosity { get { throw null; } set { } }
    }

    public partial class PluginCredentialResponse
    {
        public PluginCredentialResponse() { }

        public System.Collections.Generic.IList<string> AuthTypes { get { throw null; } set { } }

        public bool IsValid { get { throw null; } }

        public string Message { get { throw null; } set { } }

        public string Password { get { throw null; } set { } }

        public string Username { get { throw null; } set { } }
    }

    public enum PluginCredentialResponseExitCode
    {
        Success = 0,
        ProviderNotApplicable = 1,
        Failure = 2
    }

    public partial class PluginException : System.Exception
    {
        public PluginException() { }

        public PluginException(string message, System.Exception inner) { }

        public PluginException(string message) { }

        public static PluginException Create(string path, System.Exception inner) { throw null; }

        public static PluginException CreateAbortMessage(string path, string message) { throw null; }

        public static PluginException CreateInvalidResponseExceptionMessage(string path, PluginCredentialResponseExitCode status, PluginCredentialResponse response) { throw null; }

        public static PluginException CreateNotStartedMessage(string path) { throw null; }

        public static PluginException CreatePathNotFoundMessage(string path, string attempted) { throw null; }

        public static PluginException CreateTimeoutMessage(string path, int timeoutMillis) { throw null; }

        public static PluginException CreateUnreadableResponseExceptionMessage(string path, PluginCredentialResponseExitCode status) { throw null; }
    }

    public partial class PluginUnexpectedStatusException : PluginException
    {
        public PluginUnexpectedStatusException() { }

        public PluginUnexpectedStatusException(string message, System.Exception inner) { }

        public PluginUnexpectedStatusException(string message) { }

        public static PluginException CreateUnexpectedStatusMessage(string path, PluginCredentialResponseExitCode status) { throw null; }
    }

    public static partial class PreviewFeatureSettings
    {
        public const string DefaultCredentialsAfterCredentialProvidersEnvironmentVariableName = "NUGET_CREDENTIAL_PROVIDER_OVERRIDE_DEFAULT";
        public static bool DefaultCredentialsAfterCredentialProviders { get { throw null; } set { } }
    }

    public partial class ProviderException : System.Exception
    {
        public ProviderException() { }

        public ProviderException(string message, System.Exception inner) { }

        public ProviderException(string message) { }
    }

    public sealed partial class SecurePluginCredentialProvider : ICredentialProvider
    {
        public SecurePluginCredentialProvider(Protocol.Plugins.IPluginManager pluginManager, Protocol.Plugins.PluginDiscoveryResult pluginDiscoveryResult, bool canShowDialog, Common.ILogger logger) { }

        public string Id { get { throw null; } }

        public System.Threading.Tasks.Task<CredentialResponse> GetAsync(System.Uri uri, System.Net.IWebProxy proxy, Configuration.CredentialRequestType type, string message, bool isRetry, bool nonInteractive, System.Threading.CancellationToken cancellationToken) { throw null; }
    }

    public partial class SecurePluginCredentialProviderBuilder
    {
        public SecurePluginCredentialProviderBuilder(Protocol.Plugins.IPluginManager pluginManager, bool canShowDialog, Common.ILogger logger) { }

        public System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<ICredentialProvider>> BuildAllAsync() { throw null; }
    }
}