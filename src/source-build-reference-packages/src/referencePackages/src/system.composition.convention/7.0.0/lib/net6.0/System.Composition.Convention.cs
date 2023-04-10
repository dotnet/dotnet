// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------
[assembly: System.Runtime.CompilerServices.CompilationRelaxations(8)]
[assembly: System.Runtime.CompilerServices.RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: System.Diagnostics.Debuggable(System.Diagnostics.DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: System.Runtime.Versioning.TargetFramework(".NETCoreApp,Version=v6.0", FrameworkDisplayName = ".NET 6.0")]
[assembly: System.Reflection.AssemblyMetadata(".NETFrameworkAssembly", "")]
[assembly: System.Reflection.AssemblyMetadata("Serviceable", "True")]
[assembly: System.Reflection.AssemblyMetadata("PreferInbox", "True")]
[assembly: System.Reflection.AssemblyDefaultAlias("System.Composition.Convention")]
[assembly: System.Resources.NeutralResourcesLanguage("en-US")]
[assembly: System.CLSCompliant(true)]
[assembly: System.Runtime.InteropServices.DefaultDllImportSearchPaths(System.Runtime.InteropServices.DllImportSearchPath.AssemblyDirectory | System.Runtime.InteropServices.DllImportSearchPath.System32)]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation. All rights reserved.")]
[assembly: System.Reflection.AssemblyDescription("Provides types that support using Managed Extensibility Framework with a convention-based configuration model.\r\n\r\nCommonly Used Types:\r\nSystem.Composition.Convention.ConventionBuilder\r\nSystem.Composition.Convention.ExportConventionBuilder\r\nSystem.Composition.Convention.ImportConventionBuilder\r\nSystem.Composition.Convention.PartConventionBuilder\r\nSystem.Composition.Convention.ParameterImportConventionBuilder")]
[assembly: System.Reflection.AssemblyFileVersion("7.0.22.51805")]
[assembly: System.Reflection.AssemblyInformationalVersion("7.0.0+d099f075e45d2aa6007a22b71b45a08758559f80")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET")]
[assembly: System.Reflection.AssemblyTitle("System.Composition.Convention")]
[assembly: System.Reflection.AssemblyMetadata("RepositoryUrl", "https://github.com/dotnet/runtime")]
[assembly: System.Reflection.AssemblyVersionAttribute("7.0.0.0")]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace System.Composition.Convention
{
    public partial class ConventionBuilder : AttributedModelProvider
    {
        public ConventionBuilder() { }

        public PartConventionBuilder ForType(Type type) { throw null; }

        public PartConventionBuilder<T> ForType<T>() { throw null; }

        public PartConventionBuilder ForTypesDerivedFrom(Type type) { throw null; }

        public PartConventionBuilder<T> ForTypesDerivedFrom<T>() { throw null; }

        public PartConventionBuilder ForTypesMatching(Predicate<Type> typeFilter) { throw null; }

        public PartConventionBuilder<T> ForTypesMatching<T>(Predicate<Type> typeFilter) { throw null; }

        public override Collections.Generic.IEnumerable<Attribute> GetCustomAttributes(Type reflectedType, Reflection.MemberInfo member) { throw null; }

        public override Collections.Generic.IEnumerable<Attribute> GetCustomAttributes(Type reflectedType, Reflection.ParameterInfo parameter) { throw null; }
    }

    public sealed partial class ExportConventionBuilder
    {
        public ExportConventionBuilder AddMetadata(string name, Func<Type, object> getValueFromPartType) { throw null; }

        public ExportConventionBuilder AddMetadata(string name, object value) { throw null; }

        public ExportConventionBuilder AsContractName(Func<Type, string> getContractNameFromPartType) { throw null; }

        public ExportConventionBuilder AsContractName(string contractName) { throw null; }

        public ExportConventionBuilder AsContractType(Type type) { throw null; }

        public ExportConventionBuilder AsContractType<T>() { throw null; }
    }

    public sealed partial class ImportConventionBuilder
    {
        public ImportConventionBuilder AddMetadataConstraint(string name, Func<Type, object> getConstraintValueFromPartType) { throw null; }

        public ImportConventionBuilder AddMetadataConstraint(string name, object value) { throw null; }

        public ImportConventionBuilder AllowDefault() { throw null; }

        public ImportConventionBuilder AsContractName(Func<Type, string> getContractNameFromPartType) { throw null; }

        public ImportConventionBuilder AsContractName(string contractName) { throw null; }

        public ImportConventionBuilder AsMany() { throw null; }

        public ImportConventionBuilder AsMany(bool isMany) { throw null; }
    }

    public abstract partial class ParameterImportConventionBuilder
    {
        public T Import<T>() { throw null; }

        public T Import<T>(Action<ImportConventionBuilder> configure) { throw null; }
    }

    public partial class PartConventionBuilder
    {
        public PartConventionBuilder AddPartMetadata(string name, Func<Type, object> getValueFromPartType) { throw null; }

        public PartConventionBuilder AddPartMetadata(string name, object value) { throw null; }

        public PartConventionBuilder Export() { throw null; }

        public PartConventionBuilder Export(Action<ExportConventionBuilder> exportConfiguration) { throw null; }

        public PartConventionBuilder Export<T>() { throw null; }

        public PartConventionBuilder Export<T>(Action<ExportConventionBuilder> exportConfiguration) { throw null; }

        public PartConventionBuilder ExportInterfaces() { throw null; }

        public PartConventionBuilder ExportInterfaces(Predicate<Type> interfaceFilter, Action<Type, ExportConventionBuilder> exportConfiguration) { throw null; }

        public PartConventionBuilder ExportInterfaces(Predicate<Type> interfaceFilter) { throw null; }

        public PartConventionBuilder ExportProperties(Predicate<Reflection.PropertyInfo> propertyFilter, Action<Reflection.PropertyInfo, ExportConventionBuilder> exportConfiguration) { throw null; }

        public PartConventionBuilder ExportProperties(Predicate<Reflection.PropertyInfo> propertyFilter) { throw null; }

        public PartConventionBuilder ExportProperties<T>(Predicate<Reflection.PropertyInfo> propertyFilter, Action<Reflection.PropertyInfo, ExportConventionBuilder> exportConfiguration) { throw null; }

        public PartConventionBuilder ExportProperties<T>(Predicate<Reflection.PropertyInfo> propertyFilter) { throw null; }

        public PartConventionBuilder ImportProperties(Predicate<Reflection.PropertyInfo> propertyFilter, Action<Reflection.PropertyInfo, ImportConventionBuilder> importConfiguration) { throw null; }

        public PartConventionBuilder ImportProperties(Predicate<Reflection.PropertyInfo> propertyFilter) { throw null; }

        public PartConventionBuilder ImportProperties<T>(Predicate<Reflection.PropertyInfo> propertyFilter, Action<Reflection.PropertyInfo, ImportConventionBuilder> importConfiguration) { throw null; }

        public PartConventionBuilder ImportProperties<T>(Predicate<Reflection.PropertyInfo> propertyFilter) { throw null; }

        public PartConventionBuilder NotifyImportsSatisfied(Predicate<Reflection.MethodInfo> methodFilter) { throw null; }

        public PartConventionBuilder SelectConstructor(Func<Collections.Generic.IEnumerable<Reflection.ConstructorInfo>, Reflection.ConstructorInfo> constructorSelector, Action<Reflection.ParameterInfo, ImportConventionBuilder> importConfiguration) { throw null; }

        public PartConventionBuilder SelectConstructor(Func<Collections.Generic.IEnumerable<Reflection.ConstructorInfo>, Reflection.ConstructorInfo> constructorSelector) { throw null; }

        public PartConventionBuilder Shared() { throw null; }

        public PartConventionBuilder Shared(string sharingBoundary) { throw null; }
    }

    public partial class PartConventionBuilder<T> : PartConventionBuilder
    {
        public PartConventionBuilder<T> ExportProperty(Linq.Expressions.Expression<Func<T, object>> propertySelector, Action<ExportConventionBuilder> exportConfiguration) { throw null; }

        public PartConventionBuilder<T> ExportProperty(Linq.Expressions.Expression<Func<T, object>> propertySelector) { throw null; }

        public PartConventionBuilder<T> ExportProperty<TContract>(Linq.Expressions.Expression<Func<T, object>> propertySelector, Action<ExportConventionBuilder> exportConfiguration) { throw null; }

        public PartConventionBuilder<T> ExportProperty<TContract>(Linq.Expressions.Expression<Func<T, object>> propertySelector) { throw null; }

        public PartConventionBuilder<T> ImportProperty(Linq.Expressions.Expression<Func<T, object>> propertySelector, Action<ImportConventionBuilder> importConfiguration) { throw null; }

        public PartConventionBuilder<T> ImportProperty(Linq.Expressions.Expression<Func<T, object>> propertySelector) { throw null; }

        public PartConventionBuilder<T> ImportProperty<TContract>(Linq.Expressions.Expression<Func<T, object>> propertySelector, Action<ImportConventionBuilder> importConfiguration) { throw null; }

        public PartConventionBuilder<T> ImportProperty<TContract>(Linq.Expressions.Expression<Func<T, object>> propertySelector) { throw null; }

        public PartConventionBuilder<T> NotifyImportsSatisfied(Linq.Expressions.Expression<Action<T>> methodSelector) { throw null; }

        public PartConventionBuilder<T> SelectConstructor(Linq.Expressions.Expression<Func<ParameterImportConventionBuilder, T>> constructorSelector) { throw null; }
    }
}