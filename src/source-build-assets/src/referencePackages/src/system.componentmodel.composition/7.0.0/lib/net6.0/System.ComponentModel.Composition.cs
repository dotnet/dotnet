// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------
[assembly: System.Runtime.CompilerServices.CompilationRelaxations(8)]
[assembly: System.Runtime.CompilerServices.RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: System.Diagnostics.Debuggable(System.Diagnostics.DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("System.ComponentModel.Composition.Tests, PublicKey=00240000048000009400000006020000002400005253413100040000010001004b86c4cb78549b34bab61a3b1800e23bfeb5b3ec390074041536a7e3cbd97f5f04cf0f857155a8928eaa29ebfd11cfbbad3ba70efea7bda3226c6a8d370a4cd303f714486b6ebc225985a638471e6ef571cc92a4613c00b8fa65d61ccee0cbe5f36330c9a01f4183559f1bef24cc2917c6d913e3a541333a1d05d9bed22b38cb")]
[assembly: System.Runtime.Versioning.TargetFramework(".NETCoreApp,Version=v6.0", FrameworkDisplayName = ".NET 6.0")]
[assembly: System.Reflection.AssemblyMetadata(".NETFrameworkAssembly", "")]
[assembly: System.Reflection.AssemblyMetadata("Serviceable", "True")]
[assembly: System.Reflection.AssemblyMetadata("PreferInbox", "True")]
[assembly: System.Reflection.AssemblyDefaultAlias("System.ComponentModel.Composition")]
[assembly: System.Resources.NeutralResourcesLanguage("en-US")]
[assembly: System.CLSCompliant(true)]
[assembly: System.Runtime.InteropServices.DefaultDllImportSearchPaths(System.Runtime.InteropServices.DllImportSearchPath.AssemblyDirectory | System.Runtime.InteropServices.DllImportSearchPath.System32)]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation. All rights reserved.")]
[assembly: System.Reflection.AssemblyDescription("This namespace provides classes that constitute the core of the Managed Extensibility Framework, or MEF.\r\n\r\nCommonly Used Types:\r\nSystem.ComponentModel.Composition.ChangeRejectedException\r\nSystem.ComponentModel.Composition.CompositionContractMismatchException\r\nSystem.ComponentModel.Composition.CompositionError\r\nSystem.ComponentModel.Composition.CompositionException\r\nSystem.ComponentModel.Composition.ExportAttribute\r\nSystem.ComponentModel.Composition.ImportAttribute\r\nSystem.ComponentModel.Composition.ImportCardinalityMismatchException\r\nSystem.ComponentModel.Composition.Hosting.AggregateCatalog\r\nSystem.ComponentModel.Composition.Hosting.ApplicationCatalog\r\nSystem.ComponentModel.Composition.Hosting.AssemblyCatalog\r\nSystem.ComponentModel.Composition.Hosting.CompositionContainer\r\nSystem.ComponentModel.Composition.Primitives.ComposablePartException\r\nSystem.ComponentModel.Composition.Primitives.ExportDefinition\r\nSystem.ComponentModel.Composition.Primitives.ImportDefinition\r\nSystem.ComponentModel.Composition.ReflectionModel.ReflectionModelServices")]
[assembly: System.Reflection.AssemblyFileVersion("7.0.22.51805")]
[assembly: System.Reflection.AssemblyInformationalVersion("7.0.0+d099f075e45d2aa6007a22b71b45a08758559f80")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET")]
[assembly: System.Reflection.AssemblyTitle("System.ComponentModel.Composition")]
[assembly: System.Reflection.AssemblyMetadata("RepositoryUrl", "https://github.com/dotnet/runtime")]
[assembly: System.Reflection.AssemblyVersionAttribute("4.0.0.0")]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
[assembly: System.Runtime.CompilerServices.TypeForwardedTo(typeof(System.Lazy<,>))]
namespace System.ComponentModel.Composition
{
    public static partial class AdaptationConstants
    {
        public const string AdapterContractName = "System.ComponentModel.Composition.AdapterContract";
        public const string AdapterFromContractMetadataName = "FromContract";
        public const string AdapterToContractMetadataName = "ToContract";
    }

    public static partial class AttributedModelServices
    {
        public static Primitives.ComposablePart AddExportedValue<T>(this Hosting.CompositionBatch batch, T exportedValue) { throw null; }

        public static Primitives.ComposablePart AddExportedValue<T>(this Hosting.CompositionBatch batch, string contractName, T exportedValue) { throw null; }

        public static Primitives.ComposablePart AddPart(this Hosting.CompositionBatch batch, object attributedPart) { throw null; }

        public static void ComposeExportedValue<T>(this Hosting.CompositionContainer container, T exportedValue) { }

        public static void ComposeExportedValue<T>(this Hosting.CompositionContainer container, string contractName, T exportedValue) { }

        public static void ComposeParts(this Hosting.CompositionContainer container, params object[] attributedParts) { }

        public static Primitives.ComposablePart CreatePart(Primitives.ComposablePartDefinition partDefinition, object attributedPart) { throw null; }

        public static Primitives.ComposablePart CreatePart(object attributedPart, Reflection.ReflectionContext reflectionContext) { throw null; }

        public static Primitives.ComposablePart CreatePart(object attributedPart) { throw null; }

        public static Primitives.ComposablePartDefinition CreatePartDefinition(Type type, Primitives.ICompositionElement? origin, bool ensureIsDiscoverable) { throw null; }

        public static Primitives.ComposablePartDefinition CreatePartDefinition(Type type, Primitives.ICompositionElement? origin) { throw null; }

        public static bool Exports(this Primitives.ComposablePartDefinition part, Type contractType) { throw null; }

        public static bool Exports<T>(this Primitives.ComposablePartDefinition part) { throw null; }

        public static string GetContractName(Type type) { throw null; }

        public static TMetadataView GetMetadataView<TMetadataView>(Collections.Generic.IDictionary<string, object?> metadata) { throw null; }

        public static string GetTypeIdentity(Reflection.MethodInfo method) { throw null; }

        public static string GetTypeIdentity(Type type) { throw null; }

        public static bool Imports(this Primitives.ComposablePartDefinition part, Type contractType, Primitives.ImportCardinality importCardinality) { throw null; }

        public static bool Imports(this Primitives.ComposablePartDefinition part, Type contractType) { throw null; }

        public static bool Imports<T>(this Primitives.ComposablePartDefinition part, Primitives.ImportCardinality importCardinality) { throw null; }

        public static bool Imports<T>(this Primitives.ComposablePartDefinition part) { throw null; }

        public static Primitives.ComposablePart SatisfyImportsOnce(this ICompositionService compositionService, object attributedPart, Reflection.ReflectionContext reflectionContext) { throw null; }

        public static Primitives.ComposablePart SatisfyImportsOnce(this ICompositionService compositionService, object attributedPart) { throw null; }
    }

    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false, Inherited = true)]
    public partial class CatalogReflectionContextAttribute : Attribute
    {
        public CatalogReflectionContextAttribute(Type reflectionContextType) { }

        public Reflection.ReflectionContext CreateReflectionContext() { throw null; }
    }

    public partial class ChangeRejectedException : CompositionException
    {
        public ChangeRejectedException() { }

        public ChangeRejectedException(Collections.Generic.IEnumerable<CompositionError>? errors) { }

        public ChangeRejectedException(string? message, Exception? innerException) { }

        public ChangeRejectedException(string? message) { }

        public override string Message { get { throw null; } }
    }

    public partial class CompositionContractMismatchException : Exception
    {
        public CompositionContractMismatchException() { }

        protected CompositionContractMismatchException(Runtime.Serialization.SerializationInfo info, Runtime.Serialization.StreamingContext context) { }

        public CompositionContractMismatchException(string? message, Exception? innerException) { }

        public CompositionContractMismatchException(string? message) { }
    }

    public partial class CompositionError
    {
        public CompositionError(string? message, Primitives.ICompositionElement? element, Exception? exception) { }

        public CompositionError(string? message, Primitives.ICompositionElement? element) { }

        public CompositionError(string? message, Exception? exception) { }

        public CompositionError(string? message) { }

        public string Description { get { throw null; } }

        public Primitives.ICompositionElement? Element { get { throw null; } }

        public Exception? Exception { get { throw null; } }

        public override string ToString() { throw null; }
    }

    public partial class CompositionException : Exception
    {
        public CompositionException() { }

        public CompositionException(Collections.Generic.IEnumerable<CompositionError>? errors) { }

        public CompositionException(string? message, Exception? innerException) { }

        public CompositionException(string? message) { }

        public Collections.ObjectModel.ReadOnlyCollection<CompositionError> Errors { get { throw null; } }

        public override string Message { get { throw null; } }

        public Collections.ObjectModel.ReadOnlyCollection<Exception> RootCauses { get { throw null; } }
    }

    public enum CreationPolicy
    {
        Any = 0,
        Shared = 1,
        NonShared = 2
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true, Inherited = false)]
    public partial class ExportAttribute : Attribute
    {
        public ExportAttribute() { }

        public ExportAttribute(string? contractName, Type? contractType) { }

        public ExportAttribute(string? contractName) { }

        public ExportAttribute(Type? contractType) { }

        public string? ContractName { get { throw null; } }

        public Type? ContractType { get { throw null; } }
    }

    public partial class ExportFactory<T>
    {
        public ExportFactory(Func<Tuple<T, Action>> exportLifetimeContextCreator) { }

        public ExportLifetimeContext<T> CreateExport() { throw null; }
    }

    public partial class ExportFactory<T, TMetadata> : ExportFactory<T>
    {
        public ExportFactory(Func<Tuple<T, Action>> exportLifetimeContextCreator, TMetadata metadata) : base(default!) { }

        public TMetadata Metadata { get { throw null; } }
    }

    public sealed partial class ExportLifetimeContext<T> : IDisposable
    {
        public ExportLifetimeContext(T value, Action disposeAction) { }

        public T Value { get { throw null; } }

        public void Dispose() { }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Interface, AllowMultiple = true, Inherited = false)]
    public sealed partial class ExportMetadataAttribute : Attribute
    {
        public ExportMetadataAttribute(string? name, object? value) { }

        public bool IsMultiple { get { throw null; } set { } }

        public string Name { get { throw null; } }

        public object? Value { get { throw null; } }
    }

    public partial interface ICompositionService
    {
        void SatisfyImportsOnce(Primitives.ComposablePart part);
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
    public partial class ImportAttribute : Attribute
    {
        public ImportAttribute() { }

        public ImportAttribute(string? contractName, Type? contractType) { }

        public ImportAttribute(string? contractName) { }

        public ImportAttribute(Type? contractType) { }

        public bool AllowDefault { get { throw null; } set { } }

        public bool AllowRecomposition { get { throw null; } set { } }

        public string? ContractName { get { throw null; } }

        public Type? ContractType { get { throw null; } }

        public CreationPolicy RequiredCreationPolicy { get { throw null; } set { } }

        public ImportSource Source { get { throw null; } set { } }
    }

    public partial class ImportCardinalityMismatchException : Exception
    {
        public ImportCardinalityMismatchException() { }

        protected ImportCardinalityMismatchException(Runtime.Serialization.SerializationInfo info, Runtime.Serialization.StreamingContext context) { }

        public ImportCardinalityMismatchException(string? message, Exception? innerException) { }

        public ImportCardinalityMismatchException(string? message) { }
    }

    [AttributeUsage(AttributeTargets.Constructor, AllowMultiple = false, Inherited = false)]
    public partial class ImportingConstructorAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
    public partial class ImportManyAttribute : Attribute
    {
        public ImportManyAttribute() { }

        public ImportManyAttribute(string? contractName, Type? contractType) { }

        public ImportManyAttribute(string? contractName) { }

        public ImportManyAttribute(Type? contractType) { }

        public bool AllowRecomposition { get { throw null; } set { } }

        public string? ContractName { get { throw null; } }

        public Type? ContractType { get { throw null; } }

        public CreationPolicy RequiredCreationPolicy { get { throw null; } set { } }

        public ImportSource Source { get { throw null; } set { } }
    }

    public enum ImportSource
    {
        Any = 0,
        Local = 1,
        NonLocal = 2
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true, Inherited = true)]
    public partial class InheritedExportAttribute : ExportAttribute
    {
        public InheritedExportAttribute() { }

        public InheritedExportAttribute(string? contractName, Type? contractType) { }

        public InheritedExportAttribute(string? contractName) { }

        public InheritedExportAttribute(Type? contractType) { }
    }

    public partial interface IPartImportsSatisfiedNotification
    {
        void OnImportsSatisfied();
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed partial class MetadataAttributeAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = false, Inherited = false)]
    public sealed partial class MetadataViewImplementationAttribute : Attribute
    {
        public MetadataViewImplementationAttribute(Type? implementationType) { }

        public Type? ImplementationType { get { throw null; } }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed partial class PartCreationPolicyAttribute : Attribute
    {
        public PartCreationPolicyAttribute(CreationPolicy creationPolicy) { }

        public CreationPolicy CreationPolicy { get { throw null; } }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public sealed partial class PartMetadataAttribute : Attribute
    {
        public PartMetadataAttribute(string? name, object? value) { }

        public string Name { get { throw null; } }

        public object? Value { get { throw null; } }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed partial class PartNotDiscoverableAttribute : Attribute
    {
    }
}

namespace System.ComponentModel.Composition.Hosting
{
    public partial class AggregateCatalog : Primitives.ComposablePartCatalog, INotifyComposablePartCatalogChanged
    {
        public AggregateCatalog() { }

        public AggregateCatalog(Collections.Generic.IEnumerable<Primitives.ComposablePartCatalog>? catalogs) { }

        public AggregateCatalog(params Primitives.ComposablePartCatalog[]? catalogs) { }

        public Collections.Generic.ICollection<Primitives.ComposablePartCatalog> Catalogs { get { throw null; } }

        public event EventHandler<ComposablePartCatalogChangeEventArgs>? Changed { add { } remove { } }

        public event EventHandler<ComposablePartCatalogChangeEventArgs>? Changing { add { } remove { } }

        protected override void Dispose(bool disposing) { }

        public override Collections.Generic.IEnumerator<Primitives.ComposablePartDefinition> GetEnumerator() { throw null; }

        public override Collections.Generic.IEnumerable<Tuple<Primitives.ComposablePartDefinition, Primitives.ExportDefinition>> GetExports(Primitives.ImportDefinition definition) { throw null; }

        protected virtual void OnChanged(ComposablePartCatalogChangeEventArgs e) { }

        protected virtual void OnChanging(ComposablePartCatalogChangeEventArgs e) { }
    }

    public partial class AggregateExportProvider : ExportProvider, IDisposable
    {
        public AggregateExportProvider(Collections.Generic.IEnumerable<ExportProvider>? providers) { }

        public AggregateExportProvider(params ExportProvider[]? providers) { }

        public Collections.ObjectModel.ReadOnlyCollection<ExportProvider> Providers { get { throw null; } }

        public void Dispose() { }

        protected virtual void Dispose(bool disposing) { }

        protected override Collections.Generic.IEnumerable<Primitives.Export> GetExportsCore(Primitives.ImportDefinition definition, AtomicComposition? atomicComposition) { throw null; }
    }

    public partial class ApplicationCatalog : Primitives.ComposablePartCatalog, Primitives.ICompositionElement
    {
        public ApplicationCatalog() { }

        public ApplicationCatalog(Primitives.ICompositionElement definitionOrigin) { }

        public ApplicationCatalog(Reflection.ReflectionContext reflectionContext, Primitives.ICompositionElement definitionOrigin) { }

        public ApplicationCatalog(Reflection.ReflectionContext reflectionContext) { }

        string Primitives.ICompositionElement.DisplayName { get { throw null; } }

        Primitives.ICompositionElement? Primitives.ICompositionElement.Origin { get { throw null; } }

        protected override void Dispose(bool disposing) { }

        public override Collections.Generic.IEnumerator<Primitives.ComposablePartDefinition> GetEnumerator() { throw null; }

        public override Collections.Generic.IEnumerable<Tuple<Primitives.ComposablePartDefinition, Primitives.ExportDefinition>> GetExports(Primitives.ImportDefinition definition) { throw null; }

        public override string ToString() { throw null; }
    }

    public partial class AssemblyCatalog : Primitives.ComposablePartCatalog, Primitives.ICompositionElement
    {
        public AssemblyCatalog(Reflection.Assembly assembly, Primitives.ICompositionElement definitionOrigin) { }

        public AssemblyCatalog(Reflection.Assembly assembly, Reflection.ReflectionContext reflectionContext, Primitives.ICompositionElement definitionOrigin) { }

        public AssemblyCatalog(Reflection.Assembly assembly, Reflection.ReflectionContext reflectionContext) { }

        public AssemblyCatalog(Reflection.Assembly assembly) { }

        public AssemblyCatalog(string codeBase, Primitives.ICompositionElement definitionOrigin) { }

        public AssemblyCatalog(string codeBase, Reflection.ReflectionContext reflectionContext, Primitives.ICompositionElement definitionOrigin) { }

        public AssemblyCatalog(string codeBase, Reflection.ReflectionContext reflectionContext) { }

        public AssemblyCatalog(string codeBase) { }

        public Reflection.Assembly Assembly { get { throw null; } }

        string Primitives.ICompositionElement.DisplayName { get { throw null; } }

        Primitives.ICompositionElement? Primitives.ICompositionElement.Origin { get { throw null; } }

        protected override void Dispose(bool disposing) { }

        public override Collections.Generic.IEnumerator<Primitives.ComposablePartDefinition> GetEnumerator() { throw null; }

        public override Collections.Generic.IEnumerable<Tuple<Primitives.ComposablePartDefinition, Primitives.ExportDefinition>> GetExports(Primitives.ImportDefinition definition) { throw null; }

        public override string ToString() { throw null; }
    }

    public partial class AtomicComposition : IDisposable
    {
        public AtomicComposition() { }

        public AtomicComposition(AtomicComposition? outerAtomicComposition) { }

        public void AddCompleteAction(Action completeAction) { }

        public void AddRevertAction(Action revertAction) { }

        public void Complete() { }

        public void Dispose() { }

        protected virtual void Dispose(bool disposing) { }

        public void SetValue(object key, object? value) { }

        public bool TryGetValue<T>(object key, out T value) { throw null; }

        public bool TryGetValue<T>(object key, bool localAtomicCompositionOnly, out T value) { throw null; }
    }

    public partial class CatalogExportProvider : ExportProvider, IDisposable
    {
        public CatalogExportProvider(Primitives.ComposablePartCatalog catalog, bool isThreadSafe) { }

        public CatalogExportProvider(Primitives.ComposablePartCatalog catalog, CompositionOptions compositionOptions) { }

        public CatalogExportProvider(Primitives.ComposablePartCatalog catalog) { }

        public Primitives.ComposablePartCatalog Catalog { get { throw null; } }

        public ExportProvider? SourceProvider { get { throw null; } set { } }

        public void Dispose() { }

        protected virtual void Dispose(bool disposing) { }

        protected override Collections.Generic.IEnumerable<Primitives.Export> GetExportsCore(Primitives.ImportDefinition definition, AtomicComposition? atomicComposition) { throw null; }
    }

    public static partial class CatalogExtensions
    {
        public static CompositionService CreateCompositionService(this Primitives.ComposablePartCatalog composablePartCatalog) { throw null; }
    }

    public partial class ComposablePartCatalogChangeEventArgs : EventArgs
    {
        public ComposablePartCatalogChangeEventArgs(Collections.Generic.IEnumerable<Primitives.ComposablePartDefinition> addedDefinitions, Collections.Generic.IEnumerable<Primitives.ComposablePartDefinition> removedDefinitions, AtomicComposition? atomicComposition) { }

        public Collections.Generic.IEnumerable<Primitives.ComposablePartDefinition> AddedDefinitions { get { throw null; } }

        public AtomicComposition? AtomicComposition { get { throw null; } }

        public Collections.Generic.IEnumerable<Primitives.ComposablePartDefinition> RemovedDefinitions { get { throw null; } }
    }

    public partial class ComposablePartExportProvider : ExportProvider, IDisposable
    {
        public ComposablePartExportProvider() { }

        public ComposablePartExportProvider(bool isThreadSafe) { }

        public ComposablePartExportProvider(CompositionOptions compositionOptions) { }

        public ExportProvider? SourceProvider { get { throw null; } set { } }

        public void Compose(CompositionBatch batch) { }

        public void Dispose() { }

        protected virtual void Dispose(bool disposing) { }

        protected override Collections.Generic.IEnumerable<Primitives.Export>? GetExportsCore(Primitives.ImportDefinition definition, AtomicComposition? atomicComposition) { throw null; }
    }

    public partial class CompositionBatch
    {
        public CompositionBatch() { }

        public CompositionBatch(Collections.Generic.IEnumerable<Primitives.ComposablePart>? partsToAdd, Collections.Generic.IEnumerable<Primitives.ComposablePart>? partsToRemove) { }

        public Collections.ObjectModel.ReadOnlyCollection<Primitives.ComposablePart> PartsToAdd { get { throw null; } }

        public Collections.ObjectModel.ReadOnlyCollection<Primitives.ComposablePart> PartsToRemove { get { throw null; } }

        public Primitives.ComposablePart AddExport(Primitives.Export export) { throw null; }

        public void AddPart(Primitives.ComposablePart part) { }

        public void RemovePart(Primitives.ComposablePart part) { }
    }

    public static partial class CompositionConstants
    {
        public const string ExportTypeIdentityMetadataName = "ExportTypeIdentity";
        public const string GenericContractMetadataName = "System.ComponentModel.Composition.GenericContractName";
        public const string GenericParametersMetadataName = "System.ComponentModel.Composition.GenericParameters";
        public const string ImportSourceMetadataName = "System.ComponentModel.Composition.ImportSource";
        public const string IsGenericPartMetadataName = "System.ComponentModel.Composition.IsGenericPart";
        public const string PartCreationPolicyMetadataName = "System.ComponentModel.Composition.CreationPolicy";
    }

    public partial class CompositionContainer : ExportProvider, ICompositionService, IDisposable
    {
        public CompositionContainer() { }

        public CompositionContainer(CompositionOptions compositionOptions, params ExportProvider[]? providers) { }

        public CompositionContainer(params ExportProvider[]? providers) { }

        public CompositionContainer(Primitives.ComposablePartCatalog? catalog, bool isThreadSafe, params ExportProvider[]? providers) { }

        public CompositionContainer(Primitives.ComposablePartCatalog? catalog, CompositionOptions compositionOptions, params ExportProvider[]? providers) { }

        public CompositionContainer(Primitives.ComposablePartCatalog? catalog, params ExportProvider[]? providers) { }

        public Primitives.ComposablePartCatalog? Catalog { get { throw null; } }

        public Collections.ObjectModel.ReadOnlyCollection<ExportProvider> Providers { get { throw null; } }

        public void Compose(CompositionBatch batch) { }

        public void Dispose() { }

        protected virtual void Dispose(bool disposing) { }

        protected override Collections.Generic.IEnumerable<Primitives.Export>? GetExportsCore(Primitives.ImportDefinition definition, AtomicComposition? atomicComposition) { throw null; }

        public void ReleaseExport(Primitives.Export export) { }

        public void ReleaseExport<T>(Lazy<T> export) { }

        public void ReleaseExports(Collections.Generic.IEnumerable<Primitives.Export> exports) { }

        public void ReleaseExports<T>(Collections.Generic.IEnumerable<Lazy<T>> exports) { }

        public void ReleaseExports<T, TMetadataView>(Collections.Generic.IEnumerable<Lazy<T, TMetadataView>> exports) { }

        public void SatisfyImportsOnce(Primitives.ComposablePart part) { }
    }

    [Flags]
    public enum CompositionOptions
    {
        Default = 0,
        DisableSilentRejection = 1,
        IsThreadSafe = 2,
        ExportCompositionService = 4
    }

    public partial class CompositionScopeDefinition : Primitives.ComposablePartCatalog, INotifyComposablePartCatalogChanged
    {
        protected CompositionScopeDefinition() { }

        public CompositionScopeDefinition(Primitives.ComposablePartCatalog catalog, Collections.Generic.IEnumerable<CompositionScopeDefinition> children, Collections.Generic.IEnumerable<Primitives.ExportDefinition> publicSurface) { }

        public CompositionScopeDefinition(Primitives.ComposablePartCatalog catalog, Collections.Generic.IEnumerable<CompositionScopeDefinition> children) { }

        public virtual Collections.Generic.IEnumerable<CompositionScopeDefinition> Children { get { throw null; } }

        public virtual Collections.Generic.IEnumerable<Primitives.ExportDefinition> PublicSurface { get { throw null; } }

        public event EventHandler<ComposablePartCatalogChangeEventArgs>? Changed { add { } remove { } }

        public event EventHandler<ComposablePartCatalogChangeEventArgs>? Changing { add { } remove { } }

        protected override void Dispose(bool disposing) { }

        public override Collections.Generic.IEnumerator<Primitives.ComposablePartDefinition> GetEnumerator() { throw null; }

        public override Collections.Generic.IEnumerable<Tuple<Primitives.ComposablePartDefinition, Primitives.ExportDefinition>> GetExports(Primitives.ImportDefinition definition) { throw null; }

        protected virtual void OnChanged(ComposablePartCatalogChangeEventArgs e) { }

        protected virtual void OnChanging(ComposablePartCatalogChangeEventArgs e) { }
    }

    public partial class CompositionService : ICompositionService, IDisposable
    {
        internal CompositionService() { }

        public void Dispose() { }

        public void SatisfyImportsOnce(Primitives.ComposablePart part) { }
    }

    public partial class DirectoryCatalog : Primitives.ComposablePartCatalog, INotifyComposablePartCatalogChanged, Primitives.ICompositionElement
    {
        public DirectoryCatalog(string path, Primitives.ICompositionElement definitionOrigin) { }

        public DirectoryCatalog(string path, Reflection.ReflectionContext reflectionContext, Primitives.ICompositionElement definitionOrigin) { }

        public DirectoryCatalog(string path, Reflection.ReflectionContext reflectionContext) { }

        public DirectoryCatalog(string path, string searchPattern, Primitives.ICompositionElement definitionOrigin) { }

        public DirectoryCatalog(string path, string searchPattern, Reflection.ReflectionContext reflectionContext, Primitives.ICompositionElement definitionOrigin) { }

        public DirectoryCatalog(string path, string searchPattern, Reflection.ReflectionContext reflectionContext) { }

        public DirectoryCatalog(string path, string searchPattern) { }

        public DirectoryCatalog(string path) { }

        public string FullPath { get { throw null; } }

        public Collections.ObjectModel.ReadOnlyCollection<string> LoadedFiles { get { throw null; } }

        public string Path { get { throw null; } }

        public string SearchPattern { get { throw null; } }

        string Primitives.ICompositionElement.DisplayName { get { throw null; } }

        Primitives.ICompositionElement? Primitives.ICompositionElement.Origin { get { throw null; } }

        public event EventHandler<ComposablePartCatalogChangeEventArgs>? Changed { add { } remove { } }

        public event EventHandler<ComposablePartCatalogChangeEventArgs>? Changing { add { } remove { } }

        protected override void Dispose(bool disposing) { }

        public override Collections.Generic.IEnumerator<Primitives.ComposablePartDefinition> GetEnumerator() { throw null; }

        public override Collections.Generic.IEnumerable<Tuple<Primitives.ComposablePartDefinition, Primitives.ExportDefinition>> GetExports(Primitives.ImportDefinition definition) { throw null; }

        protected virtual void OnChanged(ComposablePartCatalogChangeEventArgs e) { }

        protected virtual void OnChanging(ComposablePartCatalogChangeEventArgs e) { }

        public void Refresh() { }

        public override string ToString() { throw null; }
    }

    public abstract partial class ExportProvider
    {
        public event EventHandler<ExportsChangeEventArgs>? ExportsChanged { add { } remove { } }

        public event EventHandler<ExportsChangeEventArgs>? ExportsChanging { add { } remove { } }

        public Lazy<T>? GetExport<T>() { throw null; }

        public Lazy<T>? GetExport<T>(string? contractName) { throw null; }

        public Lazy<T, TMetadataView>? GetExport<T, TMetadataView>() { throw null; }

        public Lazy<T, TMetadataView>? GetExport<T, TMetadataView>(string? contractName) { throw null; }

        public T? GetExportedValue<T>() { throw null; }

        public T? GetExportedValue<T>(string? contractName) { throw null; }

        public T? GetExportedValueOrDefault<T>() { throw null; }

        public T? GetExportedValueOrDefault<T>(string? contractName) { throw null; }

        public Collections.Generic.IEnumerable<T> GetExportedValues<T>() { throw null; }

        public Collections.Generic.IEnumerable<T> GetExportedValues<T>(string? contractName) { throw null; }

        public Collections.Generic.IEnumerable<Primitives.Export> GetExports(Primitives.ImportDefinition definition, AtomicComposition? atomicComposition) { throw null; }

        public Collections.Generic.IEnumerable<Primitives.Export> GetExports(Primitives.ImportDefinition definition) { throw null; }

        public Collections.Generic.IEnumerable<Lazy<object, object>> GetExports(Type type, Type? metadataViewType, string? contractName) { throw null; }

        public Collections.Generic.IEnumerable<Lazy<T>> GetExports<T>() { throw null; }

        public Collections.Generic.IEnumerable<Lazy<T>> GetExports<T>(string? contractName) { throw null; }

        public Collections.Generic.IEnumerable<Lazy<T, TMetadataView>> GetExports<T, TMetadataView>() { throw null; }

        public Collections.Generic.IEnumerable<Lazy<T, TMetadataView>> GetExports<T, TMetadataView>(string? contractName) { throw null; }

        protected abstract Collections.Generic.IEnumerable<Primitives.Export>? GetExportsCore(Primitives.ImportDefinition definition, AtomicComposition? atomicComposition);
        protected virtual void OnExportsChanged(ExportsChangeEventArgs e) { }

        protected virtual void OnExportsChanging(ExportsChangeEventArgs e) { }

        public bool TryGetExports(Primitives.ImportDefinition definition, AtomicComposition? atomicComposition, out Collections.Generic.IEnumerable<Primitives.Export>? exports) { throw null; }
    }

    public partial class ExportsChangeEventArgs : EventArgs
    {
        public ExportsChangeEventArgs(Collections.Generic.IEnumerable<Primitives.ExportDefinition> addedExports, Collections.Generic.IEnumerable<Primitives.ExportDefinition> removedExports, AtomicComposition? atomicComposition) { }

        public Collections.Generic.IEnumerable<Primitives.ExportDefinition> AddedExports { get { throw null; } }

        public AtomicComposition? AtomicComposition { get { throw null; } }

        public Collections.Generic.IEnumerable<string> ChangedContractNames { get { throw null; } }

        public Collections.Generic.IEnumerable<Primitives.ExportDefinition> RemovedExports { get { throw null; } }
    }

    public partial class FilteredCatalog : Primitives.ComposablePartCatalog, INotifyComposablePartCatalogChanged
    {
        public FilteredCatalog(Primitives.ComposablePartCatalog catalog, Func<Primitives.ComposablePartDefinition, bool> filter) { }

        public FilteredCatalog Complement { get { throw null; } }

        public event EventHandler<ComposablePartCatalogChangeEventArgs>? Changed { add { } remove { } }

        public event EventHandler<ComposablePartCatalogChangeEventArgs>? Changing { add { } remove { } }

        protected override void Dispose(bool disposing) { }

        public override Collections.Generic.IEnumerator<Primitives.ComposablePartDefinition> GetEnumerator() { throw null; }

        public override Collections.Generic.IEnumerable<Tuple<Primitives.ComposablePartDefinition, Primitives.ExportDefinition>> GetExports(Primitives.ImportDefinition definition) { throw null; }

        public FilteredCatalog IncludeDependencies() { throw null; }

        public FilteredCatalog IncludeDependencies(Func<Primitives.ImportDefinition, bool> importFilter) { throw null; }

        public FilteredCatalog IncludeDependents() { throw null; }

        public FilteredCatalog IncludeDependents(Func<Primitives.ImportDefinition, bool> importFilter) { throw null; }

        protected virtual void OnChanged(ComposablePartCatalogChangeEventArgs e) { }

        protected virtual void OnChanging(ComposablePartCatalogChangeEventArgs e) { }
    }

    public partial class ImportEngine : ICompositionService, IDisposable
    {
        public ImportEngine(ExportProvider sourceProvider, bool isThreadSafe) { }

        public ImportEngine(ExportProvider sourceProvider, CompositionOptions compositionOptions) { }

        public ImportEngine(ExportProvider sourceProvider) { }

        public void Dispose() { }

        protected virtual void Dispose(bool disposing) { }

        public void PreviewImports(Primitives.ComposablePart part, AtomicComposition? atomicComposition) { }

        public void ReleaseImports(Primitives.ComposablePart part, AtomicComposition? atomicComposition) { }

        public void SatisfyImports(Primitives.ComposablePart part) { }

        public void SatisfyImportsOnce(Primitives.ComposablePart part) { }
    }

    public partial interface INotifyComposablePartCatalogChanged
    {
        event EventHandler<ComposablePartCatalogChangeEventArgs>? Changed;
        event EventHandler<ComposablePartCatalogChangeEventArgs>? Changing;
    }

    public static partial class ScopingExtensions
    {
        public static bool ContainsPartMetadata<T>(this Primitives.ComposablePartDefinition part, string key, T value) { throw null; }

        public static bool ContainsPartMetadataWithKey(this Primitives.ComposablePartDefinition part, string key) { throw null; }

        public static bool Exports(this Primitives.ComposablePartDefinition part, string contractName) { throw null; }

        public static FilteredCatalog Filter(this Primitives.ComposablePartCatalog catalog, Func<Primitives.ComposablePartDefinition, bool> filter) { throw null; }

        public static bool Imports(this Primitives.ComposablePartDefinition part, string contractName, Primitives.ImportCardinality importCardinality) { throw null; }

        public static bool Imports(this Primitives.ComposablePartDefinition part, string contractName) { throw null; }
    }

    public partial class TypeCatalog : Primitives.ComposablePartCatalog, Primitives.ICompositionElement
    {
        public TypeCatalog(Collections.Generic.IEnumerable<Type> types, Primitives.ICompositionElement definitionOrigin) { }

        public TypeCatalog(Collections.Generic.IEnumerable<Type> types, Reflection.ReflectionContext reflectionContext, Primitives.ICompositionElement definitionOrigin) { }

        public TypeCatalog(Collections.Generic.IEnumerable<Type> types, Reflection.ReflectionContext reflectionContext) { }

        public TypeCatalog(Collections.Generic.IEnumerable<Type> types) { }

        public TypeCatalog(params Type[] types) { }

        string Primitives.ICompositionElement.DisplayName { get { throw null; } }

        Primitives.ICompositionElement? Primitives.ICompositionElement.Origin { get { throw null; } }

        protected override void Dispose(bool disposing) { }

        public override Collections.Generic.IEnumerator<Primitives.ComposablePartDefinition> GetEnumerator() { throw null; }

        public override string ToString() { throw null; }
    }
}

namespace System.ComponentModel.Composition.Primitives
{
    public abstract partial class ComposablePart
    {
        public abstract Collections.Generic.IEnumerable<ExportDefinition> ExportDefinitions { get; }
        public abstract Collections.Generic.IEnumerable<ImportDefinition> ImportDefinitions { get; }

        public virtual Collections.Generic.IDictionary<string, object?> Metadata { get { throw null; } }

        public virtual void Activate() { }

        public abstract object? GetExportedValue(ExportDefinition definition);
        public abstract void SetImport(ImportDefinition definition, Collections.Generic.IEnumerable<Export> exports);
    }

    public abstract partial class ComposablePartCatalog : Collections.Generic.IEnumerable<ComposablePartDefinition>, Collections.IEnumerable, IDisposable
    {
        public virtual Linq.IQueryable<ComposablePartDefinition> Parts { get { throw null; } }

        public void Dispose() { }

        protected virtual void Dispose(bool disposing) { }

        public virtual Collections.Generic.IEnumerator<ComposablePartDefinition> GetEnumerator() { throw null; }

        public virtual Collections.Generic.IEnumerable<Tuple<ComposablePartDefinition, ExportDefinition>> GetExports(ImportDefinition definition) { throw null; }

        Collections.IEnumerator Collections.IEnumerable.GetEnumerator() { throw null; }
    }

    public abstract partial class ComposablePartDefinition
    {
        public abstract Collections.Generic.IEnumerable<ExportDefinition> ExportDefinitions { get; }
        public abstract Collections.Generic.IEnumerable<ImportDefinition> ImportDefinitions { get; }

        public virtual Collections.Generic.IDictionary<string, object?> Metadata { get { throw null; } }

        public abstract ComposablePart CreatePart();
    }

    public partial class ComposablePartException : Exception
    {
        public ComposablePartException() { }

        protected ComposablePartException(Runtime.Serialization.SerializationInfo info, Runtime.Serialization.StreamingContext context) { }

        public ComposablePartException(string? message, ICompositionElement? element, Exception? innerException) { }

        public ComposablePartException(string? message, ICompositionElement? element) { }

        public ComposablePartException(string? message, Exception? innerException) { }

        public ComposablePartException(string? message) { }

        public ICompositionElement? Element { get { throw null; } }

        public override void GetObjectData(Runtime.Serialization.SerializationInfo info, Runtime.Serialization.StreamingContext context) { }
    }

    public partial class ContractBasedImportDefinition : ImportDefinition
    {
        protected ContractBasedImportDefinition() { }

        public ContractBasedImportDefinition(string contractName, string? requiredTypeIdentity, Collections.Generic.IEnumerable<Collections.Generic.KeyValuePair<string, Type>>? requiredMetadata, ImportCardinality cardinality, bool isRecomposable, bool isPrerequisite, CreationPolicy requiredCreationPolicy, Collections.Generic.IDictionary<string, object?> metadata) { }

        public ContractBasedImportDefinition(string contractName, string? requiredTypeIdentity, Collections.Generic.IEnumerable<Collections.Generic.KeyValuePair<string, Type>>? requiredMetadata, ImportCardinality cardinality, bool isRecomposable, bool isPrerequisite, CreationPolicy requiredCreationPolicy) { }

        public override Linq.Expressions.Expression<Func<ExportDefinition, bool>> Constraint { get { throw null; } }

        public virtual CreationPolicy RequiredCreationPolicy { get { throw null; } }

        public virtual Collections.Generic.IEnumerable<Collections.Generic.KeyValuePair<string, Type>> RequiredMetadata { get { throw null; } }

        public virtual string? RequiredTypeIdentity { get { throw null; } }

        public override bool IsConstraintSatisfiedBy(ExportDefinition exportDefinition) { throw null; }

        public override string ToString() { throw null; }
    }

    public partial class Export
    {
        protected Export() { }

        public Export(ExportDefinition definition, Func<object?> exportedValueGetter) { }

        public Export(string contractName, Collections.Generic.IDictionary<string, object?>? metadata, Func<object?> exportedValueGetter) { }

        public Export(string contractName, Func<object?> exportedValueGetter) { }

        public virtual ExportDefinition Definition { get { throw null; } }

        public Collections.Generic.IDictionary<string, object?> Metadata { get { throw null; } }

        public object? Value { get { throw null; } }

        protected virtual object? GetExportedValueCore() { throw null; }
    }

    public partial class ExportDefinition
    {
        protected ExportDefinition() { }

        public ExportDefinition(string contractName, Collections.Generic.IDictionary<string, object?>? metadata) { }

        public virtual string ContractName { get { throw null; } }

        public virtual Collections.Generic.IDictionary<string, object?> Metadata { get { throw null; } }

        public override string ToString() { throw null; }
    }

    public partial class ExportedDelegate
    {
        protected ExportedDelegate() { }

        public ExportedDelegate(object? instance, Reflection.MethodInfo method) { }

        public virtual Delegate? CreateDelegate(Type delegateType) { throw null; }
    }

    public partial interface ICompositionElement
    {
        string DisplayName { get; }

        ICompositionElement? Origin { get; }
    }

    public enum ImportCardinality
    {
        ZeroOrOne = 0,
        ExactlyOne = 1,
        ZeroOrMore = 2
    }

    public partial class ImportDefinition
    {
        protected ImportDefinition() { }

        public ImportDefinition(Linq.Expressions.Expression<Func<ExportDefinition, bool>> constraint, string? contractName, ImportCardinality cardinality, bool isRecomposable, bool isPrerequisite, Collections.Generic.IDictionary<string, object?>? metadata) { }

        public ImportDefinition(Linq.Expressions.Expression<Func<ExportDefinition, bool>> constraint, string? contractName, ImportCardinality cardinality, bool isRecomposable, bool isPrerequisite) { }

        public virtual ImportCardinality Cardinality { get { throw null; } }

        public virtual Linq.Expressions.Expression<Func<ExportDefinition, bool>> Constraint { get { throw null; } }

        public virtual string ContractName { get { throw null; } }

        public virtual bool IsPrerequisite { get { throw null; } }

        public virtual bool IsRecomposable { get { throw null; } }

        public virtual Collections.Generic.IDictionary<string, object?> Metadata { get { throw null; } }

        public virtual bool IsConstraintSatisfiedBy(ExportDefinition exportDefinition) { throw null; }

        public override string ToString() { throw null; }
    }
}

namespace System.ComponentModel.Composition.ReflectionModel
{
    public partial struct LazyMemberInfo : IEquatable<LazyMemberInfo>
    {
        private object _dummy;
        private int _dummyPrimitive;
        public LazyMemberInfo(Reflection.MemberInfo member) { }

        public LazyMemberInfo(Reflection.MemberTypes memberType, Func<Reflection.MemberInfo[]> accessorsCreator) { }

        public LazyMemberInfo(Reflection.MemberTypes memberType, params Reflection.MemberInfo[] accessors) { }

        public Reflection.MemberTypes MemberType { get { throw null; } }

        public bool Equals(LazyMemberInfo other) { throw null; }

        public override bool Equals(object? obj) { throw null; }

        public Reflection.MemberInfo[] GetAccessors() { throw null; }

        public override int GetHashCode() { throw null; }

        public static bool operator ==(LazyMemberInfo left, LazyMemberInfo right) { throw null; }

        public static bool operator !=(LazyMemberInfo left, LazyMemberInfo right) { throw null; }
    }

    public static partial class ReflectionModelServices
    {
        public static Primitives.ExportDefinition CreateExportDefinition(LazyMemberInfo exportingMember, string contractName, Lazy<Collections.Generic.IDictionary<string, object?>> metadata, Primitives.ICompositionElement? origin) { throw null; }

        public static Primitives.ContractBasedImportDefinition CreateImportDefinition(LazyMemberInfo importingMember, string contractName, string? requiredTypeIdentity, Collections.Generic.IEnumerable<Collections.Generic.KeyValuePair<string, Type>>? requiredMetadata, Primitives.ImportCardinality cardinality, bool isRecomposable, bool isPreRequisite, CreationPolicy requiredCreationPolicy, Collections.Generic.IDictionary<string, object?> metadata, bool isExportFactory, Primitives.ICompositionElement? origin) { throw null; }

        public static Primitives.ContractBasedImportDefinition CreateImportDefinition(LazyMemberInfo importingMember, string contractName, string? requiredTypeIdentity, Collections.Generic.IEnumerable<Collections.Generic.KeyValuePair<string, Type>>? requiredMetadata, Primitives.ImportCardinality cardinality, bool isRecomposable, CreationPolicy requiredCreationPolicy, Collections.Generic.IDictionary<string, object?> metadata, bool isExportFactory, Primitives.ICompositionElement? origin) { throw null; }

        public static Primitives.ContractBasedImportDefinition CreateImportDefinition(LazyMemberInfo importingMember, string contractName, string? requiredTypeIdentity, Collections.Generic.IEnumerable<Collections.Generic.KeyValuePair<string, Type>>? requiredMetadata, Primitives.ImportCardinality cardinality, bool isRecomposable, CreationPolicy requiredCreationPolicy, Primitives.ICompositionElement? origin) { throw null; }

        public static Primitives.ContractBasedImportDefinition CreateImportDefinition(Lazy<Reflection.ParameterInfo> parameter, string contractName, string? requiredTypeIdentity, Collections.Generic.IEnumerable<Collections.Generic.KeyValuePair<string, Type>>? requiredMetadata, Primitives.ImportCardinality cardinality, CreationPolicy requiredCreationPolicy, Collections.Generic.IDictionary<string, object?> metadata, bool isExportFactory, Primitives.ICompositionElement? origin) { throw null; }

        public static Primitives.ContractBasedImportDefinition CreateImportDefinition(Lazy<Reflection.ParameterInfo> parameter, string contractName, string? requiredTypeIdentity, Collections.Generic.IEnumerable<Collections.Generic.KeyValuePair<string, Type>>? requiredMetadata, Primitives.ImportCardinality cardinality, CreationPolicy requiredCreationPolicy, Primitives.ICompositionElement? origin) { throw null; }

        public static Primitives.ComposablePartDefinition CreatePartDefinition(Lazy<Type> partType, bool isDisposalRequired, Lazy<Collections.Generic.IEnumerable<Primitives.ImportDefinition>>? imports, Lazy<Collections.Generic.IEnumerable<Primitives.ExportDefinition>>? exports, Lazy<Collections.Generic.IDictionary<string, object?>>? metadata, Primitives.ICompositionElement? origin) { throw null; }

        public static Primitives.ContractBasedImportDefinition GetExportFactoryProductImportDefinition(Primitives.ImportDefinition importDefinition) { throw null; }

        public static LazyMemberInfo GetExportingMember(Primitives.ExportDefinition exportDefinition) { throw null; }

        public static LazyMemberInfo GetImportingMember(Primitives.ImportDefinition importDefinition) { throw null; }

        public static Lazy<Reflection.ParameterInfo> GetImportingParameter(Primitives.ImportDefinition importDefinition) { throw null; }

        public static Lazy<Type> GetPartType(Primitives.ComposablePartDefinition partDefinition) { throw null; }

        public static bool IsDisposalRequired(Primitives.ComposablePartDefinition partDefinition) { throw null; }

        public static bool IsExportFactoryImportDefinition(Primitives.ImportDefinition importDefinition) { throw null; }

        public static bool IsImportingParameter(Primitives.ImportDefinition importDefinition) { throw null; }

        public static bool TryMakeGenericPartDefinition(Primitives.ComposablePartDefinition partDefinition, Collections.Generic.IEnumerable<Type> genericParameters, out Primitives.ComposablePartDefinition? specialization) { throw null; }
    }
}