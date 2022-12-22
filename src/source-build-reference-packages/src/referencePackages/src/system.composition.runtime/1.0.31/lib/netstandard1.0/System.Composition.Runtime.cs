// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;

[assembly: Debuggable(DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: AllowPartiallyTrustedCallers]
[assembly: ReferenceAssembly]
[assembly: AssemblyTitle("System.Composition.Runtime")]
[assembly: AssemblyDescription("System.Composition.Runtime")]
[assembly: AssemblyDefaultAlias("System.Composition.Runtime")]
[assembly: AssemblyCompany("Microsoft Corporation")]
[assembly: AssemblyProduct("Microsoft® .NET Framework")]
[assembly: AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: AssemblyFileVersion("4.6.24705.01")]
[assembly: AssemblyInformationalVersion("4.6.24705.01 built by: SOURCEBUILD")]
[assembly: CLSCompliant(true)]
[assembly: AssemblyMetadata("", "")]
[assembly: AssemblyVersion("1.0.31.0")]




namespace System.Composition
{
    public abstract partial class CompositionContext
    {
        protected CompositionContext() { }
        public object GetExport(System.Composition.Hosting.Core.CompositionContract contract) { throw null; }
        public object GetExport(System.Type exportType) { throw null; }
        public object GetExport(System.Type exportType, string contractName) { throw null; }
        public System.Collections.Generic.IEnumerable<object> GetExports(System.Type exportType) { throw null; }
        public System.Collections.Generic.IEnumerable<object> GetExports(System.Type exportType, string contractName) { throw null; }
        public System.Collections.Generic.IEnumerable<TExport> GetExports<TExport>() { throw null; }
        public System.Collections.Generic.IEnumerable<TExport> GetExports<TExport>(string contractName) { throw null; }
        public TExport GetExport<TExport>() { throw null; }
        public TExport GetExport<TExport>(string contractName) { throw null; }
        public abstract bool TryGetExport(System.Composition.Hosting.Core.CompositionContract contract, out object export);
        public bool TryGetExport(System.Type exportType, out object export) { throw null; }
        public bool TryGetExport(System.Type exportType, string contractName, out object export) { throw null; }
        public bool TryGetExport<TExport>(string contractName, out TExport export) { throw null; }
        public bool TryGetExport<TExport>(out TExport export) { throw null; }
    }
    public partial class ExportFactory<T>
    {
        public ExportFactory(System.Func<System.Tuple<T, System.Action>> exportCreator) { }
        public System.Composition.Export<T> CreateExport() { throw null; }
    }
    public partial class ExportFactory<T, TMetadata> : System.Composition.ExportFactory<T>
    {
        public ExportFactory(System.Func<System.Tuple<T, System.Action>> exportCreator, TMetadata metadata) : base (default(System.Func<System.Tuple<T, System.Action>>)) { }
        public TMetadata Metadata { get { throw null; } }
    }
    public sealed partial class Export<T> : System.IDisposable
    {
        public Export(T value, System.Action disposeAction) { }
        public T Value { get { throw null; } }
        public void Dispose() { }
    }
}
namespace System.Composition.Hosting
{
    public partial class CompositionFailedException : System.Exception
    {
        public CompositionFailedException() { }
        public CompositionFailedException(string message) { }
        public CompositionFailedException(string message, System.Exception innerException) { }
    }
}
namespace System.Composition.Hosting.Core
{
    public sealed partial class CompositionContract
    {
        public CompositionContract(System.Type contractType) { }
        public CompositionContract(System.Type contractType, string contractName) { }
        public CompositionContract(System.Type contractType, string contractName, System.Collections.Generic.IDictionary<string, object> metadataConstraints) { }
        public string ContractName { get { throw null; } }
        public System.Type ContractType { get { throw null; } }
        public System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<string, object>> MetadataConstraints { get { throw null; } }
        public System.Composition.Hosting.Core.CompositionContract ChangeType(System.Type newContractType) { throw null; }
        public override bool Equals(object obj) { throw null; }
        public override int GetHashCode() { throw null; }
        public override string ToString() { throw null; }
        public bool TryUnwrapMetadataConstraint<T>(string constraintName, out T constraintValue, out System.Composition.Hosting.Core.CompositionContract remainingContract) { throw null; }
    }
}
