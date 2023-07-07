// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------
[assembly: System.Runtime.CompilerServices.CompilationRelaxations(8)]
[assembly: System.Runtime.CompilerServices.RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: System.Diagnostics.Debuggable(System.Diagnostics.DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: System.Reflection.AssemblyTitle("System.Composition.Runtime")]
[assembly: System.Reflection.AssemblyDescription("System.Composition.Runtime")]
[assembly: System.Reflection.AssemblyDefaultAlias("System.Composition.Runtime")]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET Framework")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: System.Reflection.AssemblyFileVersion("4.6.24705.01")]
[assembly: System.Reflection.AssemblyInformationalVersion("4.6.24705.01. Commit Hash: 4d1af962ca0fede10beb01d197367c2f90e92c97")]
[assembly: System.CLSCompliant(true)]
[assembly: System.Reflection.AssemblyMetadata(".NETFrameworkAssembly", "")]
[assembly: System.Reflection.AssemblyMetadata("Serviceable", "True")]
[assembly: System.Reflection.AssemblyVersionAttribute("1.0.31.0")]
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