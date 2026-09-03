// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------
[assembly: System.Runtime.CompilerServices.CompilationRelaxations(8)]
[assembly: System.Runtime.CompilerServices.RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: System.Diagnostics.Debuggable(System.Diagnostics.DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: System.Runtime.Versioning.TargetFramework(".NETStandard,Version=v2.0", FrameworkDisplayName = ".NET Standard 2.0")]
[assembly: System.Reflection.AssemblyMetadata("Serviceable", "True")]
[assembly: System.Reflection.AssemblyMetadata("PreferInbox", "True")]
[assembly: System.Reflection.AssemblyDefaultAlias("System.Security.Cryptography.ProtectedData")]
[assembly: System.Resources.NeutralResourcesLanguage("en-US")]
[assembly: System.CLSCompliant(true)]
[assembly: System.Reflection.AssemblyMetadata("IsTrimmable", "True")]
[assembly: System.Reflection.AssemblyMetadata("IsAotCompatible", "True")]
[assembly: System.Runtime.InteropServices.DefaultDllImportSearchPaths(System.Runtime.InteropServices.DllImportSearchPath.AssemblyDirectory | System.Runtime.InteropServices.DllImportSearchPath.System32)]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation. All rights reserved.")]
[assembly: System.Reflection.AssemblyDescription("Provides access to Windows Data Protection Api.\r\n\r\nCommonly Used Types:\r\nSystem.Security.Cryptography.DataProtectionScope\r\nSystem.Security.Cryptography.ProtectedData")]
[assembly: System.Reflection.AssemblyFileVersion("10.0.326.7603")]
[assembly: System.Reflection.AssemblyInformationalVersion("10.0.3+c2435c3e0f46de784341ac3ed62863ce77e117b4")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET")]
[assembly: System.Reflection.AssemblyTitle("System.Security.Cryptography.ProtectedData")]
[assembly: System.Reflection.AssemblyMetadata("RepositoryUrl", "https://github.com/dotnet/dotnet")]
[assembly: System.Reflection.AssemblyVersionAttribute("10.0.0.0")]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace System.Diagnostics.CodeAnalysis
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, Inherited = false)]
    internal sealed partial class AllowNullAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, Inherited = false)]
    internal sealed partial class DisallowNullAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    internal sealed partial class DoesNotReturnAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
    internal sealed partial class DoesNotReturnIfAttribute : Attribute
    {
        public DoesNotReturnIfAttribute(bool parameterValue) { }
        public bool ParameterValue { get { throw null; } }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.ReturnValue, Inherited = false)]
    internal sealed partial class MaybeNullAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
    internal sealed partial class MaybeNullWhenAttribute : Attribute
    {
        public MaybeNullWhenAttribute(bool returnValue) { }
        public bool ReturnValue { get { throw null; } }
    }

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
    internal sealed partial class MemberNotNullAttribute : Attribute
    {
        public MemberNotNullAttribute(string member) { }
        public MemberNotNullAttribute(params string[] members) { }
        public string[] Members { get { throw null; } }
    }

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
    internal sealed partial class MemberNotNullWhenAttribute : Attribute
    {
        public MemberNotNullWhenAttribute(bool returnValue, string member) { }
        public MemberNotNullWhenAttribute(bool returnValue, params string[] members) { }
        public string[] Members { get { throw null; } }
        public bool ReturnValue { get { throw null; } }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.ReturnValue, Inherited = false)]
    internal sealed partial class NotNullAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter | AttributeTargets.ReturnValue, AllowMultiple = true, Inherited = false)]
    internal sealed partial class NotNullIfNotNullAttribute : Attribute
    {
        public NotNullIfNotNullAttribute(string parameterName) { }
        public string ParameterName { get { throw null; } }
    }

    [AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
    internal sealed partial class NotNullWhenAttribute : Attribute
    {
        public NotNullWhenAttribute(bool returnValue) { }
        public bool ReturnValue { get { throw null; } }
    }
}

namespace System.Security.Cryptography
{
    public enum DataProtectionScope
    {
        CurrentUser = 0,
        LocalMachine = 1
    }

    public static partial class ProtectedData
    {
        public static byte[] Protect(byte[] userData, byte[]? optionalEntropy, DataProtectionScope scope) { throw null; }
        public static byte[] Unprotect(byte[] encryptedData, byte[]? optionalEntropy, DataProtectionScope scope) { throw null; }
    }
}