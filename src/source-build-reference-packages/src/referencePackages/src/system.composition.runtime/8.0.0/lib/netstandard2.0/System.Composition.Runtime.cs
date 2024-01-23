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
[assembly: System.Reflection.AssemblyDefaultAlias("System.Composition.Runtime")]
[assembly: System.Resources.NeutralResourcesLanguage("en-US")]
[assembly: System.CLSCompliant(true)]
[assembly: System.Reflection.AssemblyMetadata("IsTrimmable", "True")]
[assembly: System.Runtime.InteropServices.DefaultDllImportSearchPaths(System.Runtime.InteropServices.DllImportSearchPath.AssemblyDirectory | System.Runtime.InteropServices.DllImportSearchPath.System32)]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation. All rights reserved.")]
[assembly: System.Reflection.AssemblyDescription("Contains runtime components of the Managed Extensibility Framework.\r\n\r\nCommonly Used Types:\r\nSystem.Composition.CompositionContext")]
[assembly: System.Reflection.AssemblyFileVersion("8.0.23.53103")]
[assembly: System.Reflection.AssemblyInformationalVersion("8.0.0+5535e31a712343a63f5d7d796cd874e563e5ac14")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET")]
[assembly: System.Reflection.AssemblyTitle("System.Composition.Runtime")]
[assembly: System.Reflection.AssemblyMetadata("RepositoryUrl", "https://github.com/dotnet/runtime")]
[assembly: System.Reflection.AssemblyVersionAttribute("8.0.0.0")]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace System.Composition
{
    public abstract partial class CompositionContext
    {
        public object GetExport(Hosting.Core.CompositionContract contract) { throw null; }

        public object GetExport(Type exportType, string contractName) { throw null; }

        public object GetExport(Type exportType) { throw null; }

        public TExport GetExport<TExport>() { throw null; }

        public TExport GetExport<TExport>(string contractName) { throw null; }

        public Collections.Generic.IEnumerable<object> GetExports(Type exportType, string contractName) { throw null; }

        public Collections.Generic.IEnumerable<object> GetExports(Type exportType) { throw null; }

        public Collections.Generic.IEnumerable<TExport> GetExports<TExport>() { throw null; }

        public Collections.Generic.IEnumerable<TExport> GetExports<TExport>(string contractName) { throw null; }

        public abstract bool TryGetExport(Hosting.Core.CompositionContract contract, out object export);
        public bool TryGetExport(Type exportType, out object export) { throw null; }

        public bool TryGetExport(Type exportType, string contractName, out object export) { throw null; }

        public bool TryGetExport<TExport>(out TExport export) { throw null; }

        public bool TryGetExport<TExport>(string contractName, out TExport export) { throw null; }
    }

    public partial class ExportFactory<T>
    {
        public ExportFactory(Func<Tuple<T, Action>> exportCreator) { }

        public Export<T> CreateExport() { throw null; }
    }

    public partial class ExportFactory<T, TMetadata> : ExportFactory<T>
    {
        public ExportFactory(Func<Tuple<T, Action>> exportCreator, TMetadata metadata) : base(default!) { }

        public TMetadata Metadata { get { throw null; } }
    }

    public sealed partial class Export<T> : IDisposable
    {
        public Export(T value, Action disposeAction) { }

        public T Value { get { throw null; } }

        public void Dispose() { }
    }
}

namespace System.Composition.Hosting
{
    public partial class CompositionFailedException : Exception
    {
        public CompositionFailedException() { }

        public CompositionFailedException(string message, Exception innerException) { }

        public CompositionFailedException(string message) { }
    }
}

namespace System.Composition.Hosting.Core
{
    public sealed partial class CompositionContract
    {
        public CompositionContract(Type contractType, string contractName, Collections.Generic.IDictionary<string, object> metadataConstraints) { }

        public CompositionContract(Type contractType, string contractName) { }

        public CompositionContract(Type contractType) { }

        public string ContractName { get { throw null; } }

        public Type ContractType { get { throw null; } }

        public Collections.Generic.IEnumerable<Collections.Generic.KeyValuePair<string, object>> MetadataConstraints { get { throw null; } }

        public CompositionContract ChangeType(Type newContractType) { throw null; }

        public override bool Equals(object obj) { throw null; }

        public override int GetHashCode() { throw null; }

        public override string ToString() { throw null; }

        public bool TryUnwrapMetadataConstraint<T>(string constraintName, out T constraintValue, out CompositionContract remainingContract) { throw null; }
    }
}