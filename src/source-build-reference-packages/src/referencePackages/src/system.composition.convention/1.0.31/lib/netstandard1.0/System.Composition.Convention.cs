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
[assembly: AssemblyTitle("System.Composition.Convention")]
[assembly: AssemblyDescription("System.Composition.Convention")]
[assembly: AssemblyDefaultAlias("System.Composition.Convention")]
[assembly: AssemblyCompany("Microsoft Corporation")]
[assembly: AssemblyProduct("Microsoft® .NET Framework")]
[assembly: AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: AssemblyFileVersion("4.6.24705.01")]
[assembly: AssemblyInformationalVersion("4.6.24705.01 built by: SOURCEBUILD")]
[assembly: CLSCompliant(true)]
[assembly: AssemblyMetadata("", "")]
[assembly: AssemblyVersion("1.0.31.0")]




namespace System.Composition.Convention
{
    public partial class ConventionBuilder : System.Composition.Convention.AttributedModelProvider
    {
        public ConventionBuilder() { }
        public System.Composition.Convention.PartConventionBuilder ForType(System.Type type) { throw null; }
        public System.Composition.Convention.PartConventionBuilder ForTypesDerivedFrom(System.Type type) { throw null; }
        public System.Composition.Convention.PartConventionBuilder<T> ForTypesDerivedFrom<T>() { throw null; }
        public System.Composition.Convention.PartConventionBuilder ForTypesMatching(System.Predicate<System.Type> typeFilter) { throw null; }
        public System.Composition.Convention.PartConventionBuilder<T> ForTypesMatching<T>(System.Predicate<System.Type> typeFilter) { throw null; }
        public System.Composition.Convention.PartConventionBuilder<T> ForType<T>() { throw null; }
        public override System.Collections.Generic.IEnumerable<System.Attribute> GetCustomAttributes(System.Type reflectedType, System.Reflection.MemberInfo member) { throw null; }
        public override System.Collections.Generic.IEnumerable<System.Attribute> GetCustomAttributes(System.Type reflectedType, System.Reflection.ParameterInfo parameter) { throw null; }
    }
    public sealed partial class ExportConventionBuilder
    {
        internal ExportConventionBuilder() { }
        public System.Composition.Convention.ExportConventionBuilder AddMetadata(string name, System.Func<System.Type, object> getValueFromPartType) { throw null; }
        public System.Composition.Convention.ExportConventionBuilder AddMetadata(string name, object value) { throw null; }
        public System.Composition.Convention.ExportConventionBuilder AsContractName(System.Func<System.Type, string> getContractNameFromPartType) { throw null; }
        public System.Composition.Convention.ExportConventionBuilder AsContractName(string contractName) { throw null; }
        public System.Composition.Convention.ExportConventionBuilder AsContractType(System.Type type) { throw null; }
        public System.Composition.Convention.ExportConventionBuilder AsContractType<T>() { throw null; }
    }
    public sealed partial class ImportConventionBuilder
    {
        internal ImportConventionBuilder() { }
        public System.Composition.Convention.ImportConventionBuilder AddMetadataConstraint(string name, System.Func<System.Type, object> getConstraintValueFromPartType) { throw null; }
        public System.Composition.Convention.ImportConventionBuilder AddMetadataConstraint(string name, object value) { throw null; }
        public System.Composition.Convention.ImportConventionBuilder AllowDefault() { throw null; }
        public System.Composition.Convention.ImportConventionBuilder AsContractName(System.Func<System.Type, string> getContractNameFromPartType) { throw null; }
        public System.Composition.Convention.ImportConventionBuilder AsContractName(string contractName) { throw null; }
        public System.Composition.Convention.ImportConventionBuilder AsMany() { throw null; }
        public System.Composition.Convention.ImportConventionBuilder AsMany(bool isMany) { throw null; }
    }
    public abstract partial class ParameterImportConventionBuilder
    {
        internal ParameterImportConventionBuilder() { }
        public T Import<T>() { throw null; }
        public T Import<T>(System.Action<System.Composition.Convention.ImportConventionBuilder> configure) { throw null; }
    }
    public partial class PartConventionBuilder
    {
        internal PartConventionBuilder() { }
        public System.Composition.Convention.PartConventionBuilder AddPartMetadata(string name, System.Func<System.Type, object> getValueFromPartType) { throw null; }
        public System.Composition.Convention.PartConventionBuilder AddPartMetadata(string name, object value) { throw null; }
        public System.Composition.Convention.PartConventionBuilder Export() { throw null; }
        public System.Composition.Convention.PartConventionBuilder Export(System.Action<System.Composition.Convention.ExportConventionBuilder> exportConfiguration) { throw null; }
        public System.Composition.Convention.PartConventionBuilder ExportInterfaces() { throw null; }
        public System.Composition.Convention.PartConventionBuilder ExportInterfaces(System.Predicate<System.Type> interfaceFilter) { throw null; }
        public System.Composition.Convention.PartConventionBuilder ExportInterfaces(System.Predicate<System.Type> interfaceFilter, System.Action<System.Type, System.Composition.Convention.ExportConventionBuilder> exportConfiguration) { throw null; }
        public System.Composition.Convention.PartConventionBuilder ExportProperties(System.Predicate<System.Reflection.PropertyInfo> propertyFilter) { throw null; }
        public System.Composition.Convention.PartConventionBuilder ExportProperties(System.Predicate<System.Reflection.PropertyInfo> propertyFilter, System.Action<System.Reflection.PropertyInfo, System.Composition.Convention.ExportConventionBuilder> exportConfiguration) { throw null; }
        public System.Composition.Convention.PartConventionBuilder ExportProperties<T>(System.Predicate<System.Reflection.PropertyInfo> propertyFilter) { throw null; }
        public System.Composition.Convention.PartConventionBuilder ExportProperties<T>(System.Predicate<System.Reflection.PropertyInfo> propertyFilter, System.Action<System.Reflection.PropertyInfo, System.Composition.Convention.ExportConventionBuilder> exportConfiguration) { throw null; }
        public System.Composition.Convention.PartConventionBuilder Export<T>() { throw null; }
        public System.Composition.Convention.PartConventionBuilder Export<T>(System.Action<System.Composition.Convention.ExportConventionBuilder> exportConfiguration) { throw null; }
        public System.Composition.Convention.PartConventionBuilder ImportProperties(System.Predicate<System.Reflection.PropertyInfo> propertyFilter) { throw null; }
        public System.Composition.Convention.PartConventionBuilder ImportProperties(System.Predicate<System.Reflection.PropertyInfo> propertyFilter, System.Action<System.Reflection.PropertyInfo, System.Composition.Convention.ImportConventionBuilder> importConfiguration) { throw null; }
        public System.Composition.Convention.PartConventionBuilder ImportProperties<T>(System.Predicate<System.Reflection.PropertyInfo> propertyFilter) { throw null; }
        public System.Composition.Convention.PartConventionBuilder ImportProperties<T>(System.Predicate<System.Reflection.PropertyInfo> propertyFilter, System.Action<System.Reflection.PropertyInfo, System.Composition.Convention.ImportConventionBuilder> importConfiguration) { throw null; }
        public System.Composition.Convention.PartConventionBuilder NotifyImportsSatisfied(System.Predicate<System.Reflection.MethodInfo> methodFilter) { throw null; }
        public System.Composition.Convention.PartConventionBuilder SelectConstructor(System.Func<System.Collections.Generic.IEnumerable<System.Reflection.ConstructorInfo>, System.Reflection.ConstructorInfo> constructorSelector) { throw null; }
        public System.Composition.Convention.PartConventionBuilder SelectConstructor(System.Func<System.Collections.Generic.IEnumerable<System.Reflection.ConstructorInfo>, System.Reflection.ConstructorInfo> constructorSelector, System.Action<System.Reflection.ParameterInfo, System.Composition.Convention.ImportConventionBuilder> importConfiguration) { throw null; }
        public System.Composition.Convention.PartConventionBuilder Shared() { throw null; }
        public System.Composition.Convention.PartConventionBuilder Shared(string sharingBoundary) { throw null; }
    }
    public partial class PartConventionBuilder<T> : System.Composition.Convention.PartConventionBuilder
    {
        internal PartConventionBuilder() { }
        public System.Composition.Convention.PartConventionBuilder<T> ExportProperty(System.Linq.Expressions.Expression<System.Func<T, object>> propertySelector) { throw null; }
        public System.Composition.Convention.PartConventionBuilder<T> ExportProperty(System.Linq.Expressions.Expression<System.Func<T, object>> propertySelector, System.Action<System.Composition.Convention.ExportConventionBuilder> exportConfiguration) { throw null; }
        public System.Composition.Convention.PartConventionBuilder<T> ExportProperty<TContract>(System.Linq.Expressions.Expression<System.Func<T, object>> propertySelector) { throw null; }
        public System.Composition.Convention.PartConventionBuilder<T> ExportProperty<TContract>(System.Linq.Expressions.Expression<System.Func<T, object>> propertySelector, System.Action<System.Composition.Convention.ExportConventionBuilder> exportConfiguration) { throw null; }
        public System.Composition.Convention.PartConventionBuilder<T> ImportProperty(System.Linq.Expressions.Expression<System.Func<T, object>> propertySelector) { throw null; }
        public System.Composition.Convention.PartConventionBuilder<T> ImportProperty(System.Linq.Expressions.Expression<System.Func<T, object>> propertySelector, System.Action<System.Composition.Convention.ImportConventionBuilder> importConfiguration) { throw null; }
        public System.Composition.Convention.PartConventionBuilder<T> ImportProperty<TContract>(System.Linq.Expressions.Expression<System.Func<T, object>> propertySelector) { throw null; }
        public System.Composition.Convention.PartConventionBuilder<T> ImportProperty<TContract>(System.Linq.Expressions.Expression<System.Func<T, object>> propertySelector, System.Action<System.Composition.Convention.ImportConventionBuilder> importConfiguration) { throw null; }
        public System.Composition.Convention.PartConventionBuilder<T> NotifyImportsSatisfied(System.Linq.Expressions.Expression<System.Action<T>> methodSelector) { throw null; }
        public System.Composition.Convention.PartConventionBuilder<T> SelectConstructor(System.Linq.Expressions.Expression<System.Func<System.Composition.Convention.ParameterImportConventionBuilder, T>> constructorSelector) { throw null; }
    }
}
