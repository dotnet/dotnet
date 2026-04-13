// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------
[assembly: System.Runtime.CompilerServices.CompilationRelaxations(8)]
[assembly: System.Runtime.CompilerServices.RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: System.Diagnostics.Debuggable(System.Diagnostics.DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: System.Reflection.AssemblyFileVersion("3.2.2146.50370")]
[assembly: System.Reflection.AssemblyInformationalVersion("3.2.2146")]
[assembly: System.Runtime.InteropServices.PrimaryInteropAssembly(1, 0)]
[assembly: System.Runtime.Versioning.TargetFramework(".NETStandard,Version=v2.1", FrameworkDisplayName = "")]
[assembly: System.Reflection.AssemblyCompany("Microsoft")]
[assembly: System.Reflection.AssemblyConfiguration("Release")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation. All rights reserved.")]
[assembly: System.Reflection.AssemblyDescription("Managed query API for enumerating Visual Studio setup instances using embeddable interoperability types.")]
[assembly: System.Reflection.AssemblyProduct("Visual Studio")]
[assembly: System.Reflection.AssemblyTitle("Setup Configuration Interoperability Types")]
[assembly: System.Resources.NeutralResourcesLanguage("en-US")]
[assembly: System.Reflection.AssemblyVersionAttribute("1.0.0.0")]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace Microsoft.VisualStudio.Setup.Configuration
{
    public partial interface IEnumSetupInstances
    {
        IEnumSetupInstances Clone();
        void Next(int celt, ISetupInstance[] rgelt, out int pceltFetched);
        void Reset();
        void Skip(int celt);
    }

    [System.Flags]
    public enum InstanceState : uint
    {
        Complete = uint.MaxValue,
        None = 0U,
        Local = 1U,
        Registered = 2U,
        NoRebootRequired = 4U,
        NoErrors = 8U
    }

    public partial interface ISetupConfiguration
    {
        IEnumSetupInstances EnumInstances();
        ISetupInstance GetInstanceForCurrentProcess();
        ISetupInstance GetInstanceForPath(string path);
    }

    public partial interface ISetupConfiguration2 : ISetupConfiguration
    {
        IEnumSetupInstances EnumAllInstances();
        IEnumSetupInstances EnumInstances();
        ISetupInstance GetInstanceForCurrentProcess();
        ISetupInstance GetInstanceForPath(string path);
    }

    public partial interface ISetupErrorInfo
    {
        string GetErrorClassName();
        int GetErrorHResult();
        string GetErrorMessage();
    }

    public partial interface ISetupErrorState
    {
        ISetupFailedPackageReference[] GetFailedPackages();
        ISetupPackageReference[] GetSkippedPackages();
    }

    public partial interface ISetupErrorState2 : ISetupErrorState
    {
        string GetErrorLogFilePath();
        ISetupFailedPackageReference[] GetFailedPackages();
        string GetLogFilePath();
        ISetupPackageReference[] GetSkippedPackages();
    }

    public partial interface ISetupErrorState3 : ISetupErrorState2, ISetupErrorState
    {
        string GetErrorLogFilePath();
        ISetupFailedPackageReference[] GetFailedPackages();
        string GetLogFilePath();
        ISetupErrorInfo GetRuntimeError();
        ISetupPackageReference[] GetSkippedPackages();
    }

    public partial interface ISetupFailedPackageReference : ISetupPackageReference
    {
        string GetBranch();
        string GetChip();
        string GetId();
        bool GetIsExtension();
        string GetLanguage();
        string GetType();
        string GetUniqueId();
        string GetVersion();
    }

    public partial interface ISetupFailedPackageReference2 : ISetupFailedPackageReference, ISetupPackageReference
    {
        ISetupPackageReference[] GetAffectedPackages();
        string GetBranch();
        string GetChip();
        string GetDescription();
        string[] GetDetails();
        string GetId();
        bool GetIsExtension();
        string GetLanguage();
        string GetLogFilePath();
        string GetSignature();
        string GetType();
        string GetUniqueId();
        string GetVersion();
    }

    public partial interface ISetupFailedPackageReference3 : ISetupFailedPackageReference2, ISetupFailedPackageReference, ISetupPackageReference
    {
        string GetAction();
        ISetupPackageReference[] GetAffectedPackages();
        string GetBranch();
        string GetChip();
        string GetDescription();
        string[] GetDetails();
        string GetId();
        bool GetIsExtension();
        string GetLanguage();
        string GetLogFilePath();
        string GetReturnCode();
        string GetSignature();
        string GetType();
        string GetUniqueId();
        string GetVersion();
    }

    public partial interface ISetupHelper
    {
        ulong ParseVersion(string version);
        void ParseVersionRange(string versionRange, out ulong minVersion, out ulong maxVersion);
    }

    public partial interface ISetupInstance
    {
        string GetDescription(int lcid = 0);
        string GetDisplayName(int lcid = 0);
        string GetInstallationName();
        string GetInstallationPath();
        string GetInstallationVersion();
        System.Runtime.InteropServices.ComTypes.FILETIME GetInstallDate();
        string GetInstanceId();
        string ResolvePath(string pwszRelativePath = null);
    }

    public partial interface ISetupInstance2 : ISetupInstance
    {
        string GetDescription(int lcid = 0);
        string GetDisplayName(int lcid = 0);
        string GetEnginePath();
        ISetupErrorState GetErrors();
        string GetInstallationName();
        string GetInstallationPath();
        string GetInstallationVersion();
        System.Runtime.InteropServices.ComTypes.FILETIME GetInstallDate();
        string GetInstanceId();
        ISetupPackageReference[] GetPackages();
        ISetupPackageReference GetProduct();
        string GetProductPath();
        ISetupPropertyStore GetProperties();
        InstanceState GetState();
        bool IsComplete();
        bool IsLaunchable();
        string ResolvePath(string pwszRelativePath = null);
    }

    public partial interface ISetupInstanceCatalog
    {
        ISetupPropertyStore GetCatalogInfo();
        bool IsPrerelease();
    }

    public partial interface ISetupLocalizedProperties
    {
        ISetupLocalizedPropertyStore GetLocalizedChannelProperties();
        ISetupLocalizedPropertyStore GetLocalizedProperties();
    }

    public partial interface ISetupLocalizedPropertyStore
    {
        string[] GetNames(int lcid = 0);
        object GetValue(string pwszName, int lcid = 0);
    }

    public partial interface ISetupPackageReference
    {
        string GetBranch();
        string GetChip();
        string GetId();
        bool GetIsExtension();
        string GetLanguage();
        string GetType();
        string GetUniqueId();
        string GetVersion();
    }

    public partial interface ISetupPolicy
    {
        string GetSharedInstallationPath();
        object GetValue(string pwszName);
    }

    public partial interface ISetupProductReference : ISetupPackageReference
    {
        string GetBranch();
        string GetChip();
        string GetId();
        bool GetIsExtension();
        bool GetIsInstalled();
        string GetLanguage();
        string GetType();
        string GetUniqueId();
        string GetVersion();
    }

    public partial interface ISetupProductReference2 : ISetupProductReference, ISetupPackageReference
    {
        string GetBranch();
        string GetChip();
        string GetId();
        bool GetIsExtension();
        bool GetIsInstalled();
        string GetLanguage();
        bool GetSupportsExtensions();
        string GetType();
        string GetUniqueId();
        string GetVersion();
    }

    public partial interface ISetupPropertyStore
    {
        string[] GetNames();
        object GetValue(string pwszName);
    }

    [System.Runtime.InteropServices.CoClass(typeof(SetupConfigurationClass))]
    [System.Runtime.InteropServices.TypeLibImportClass(typeof(SetupConfigurationClass))]
    public partial interface SetupConfiguration : ISetupConfiguration2, ISetupConfiguration
    {
    }

    public partial class SetupConfigurationClass
    {
    }
}