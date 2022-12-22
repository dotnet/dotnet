// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Cci.Extensions;
using Microsoft.Cci.Extensions.CSharp;
using Microsoft.Cci.Filters;
using Microsoft.Cci.Traversers;
using Microsoft.Cci.Writers.CSharp;
using Microsoft.Cci.Writers.Syntax;

namespace Microsoft.Cci.Writers
{
    public class CSharpWriter : SimpleTypeMemberTraverser, ICciWriter, IDisposable
    {
        private readonly ISyntaxWriter _syntaxWriter;
        private readonly IStyleSyntaxWriter _styleWriter;
        private readonly CSDeclarationWriter _declarationWriter;
        private readonly bool _writeAssemblyAttributes;
        private readonly bool _apiOnly;
        private readonly ICciFilter _cciFilter;
        private bool _firstMemberGroup;
        private ICciFilter _currentTypeListFilter;

        public CSharpWriter(ISyntaxWriter writer, ICciFilter filter, bool apiOnly, bool writeAssemblyAttributes = false)
            : base(filter)
        {
            _syntaxWriter = writer;
            _styleWriter = writer as IStyleSyntaxWriter;
            _apiOnly = apiOnly;
            _cciFilter = filter;
            _declarationWriter = new CSDeclarationWriter(_syntaxWriter, filter, !apiOnly);
            _writeAssemblyAttributes = writeAssemblyAttributes;
        }

        public class ConditionalTypeList
        {
            public ICciFilter Filter { get; set; }
            public string Symbol { get; set; }
        }

        public ISyntaxWriter SyntaxWriter { get { return _syntaxWriter; } }

        public ICciDeclarationWriter DeclarationWriter { get { return _declarationWriter; } }

        public bool IncludeSpaceBetweenMemberGroups { get; set; }

        public bool IncludeMemberGroupHeadings { get; set; }

        public bool HighlightBaseMembers { get; set; }

        public bool HighlightInterfaceMembers { get; set; }

        public bool PutBraceOnNewLine { get; set; }

        public IEnumerable<ConditionalTypeList> ConditionalTypeLists { get; set; }

        public string DefaultCondition { get; set; }

        public bool IncludeGlobalPrefixForCompilation
        {
            get { return _declarationWriter.ForCompilationIncludeGlobalPrefix; }
            set { _declarationWriter.ForCompilationIncludeGlobalPrefix = value; }
        }

        public string PlatformNotSupportedExceptionMessage
        {
            get { return _declarationWriter.PlatformNotSupportedExceptionMessage; }
            set { _declarationWriter.PlatformNotSupportedExceptionMessage = value; }
        }

        public bool AlwaysIncludeBase
        {
            get { return _declarationWriter.AlwaysIncludeBase; }
            set { _declarationWriter.AlwaysIncludeBase = value; }
        }

        public Version LangVersion
        {
            get { return _declarationWriter.LangVersion; }
            set { _declarationWriter.LangVersion = value; }
        }

        public void WriteAssemblies(IEnumerable<IAssembly> assemblies)
        {
            foreach (var assembly in assemblies)
                Visit(assembly);
        }

        public override void Visit(IAssembly assembly)
        {
            _declarationWriter.ModuleNullableContextValue = assembly.ModuleAttributes.GetCustomAttributeArgumentValue<byte?>(CSharpCciExtensions.NullableContextAttributeFullName);

            if (_writeAssemblyAttributes)
            {
                _declarationWriter.WriteDeclaration(assembly);
            }

            var namespaces = assembly.GetAllNamespaces();
            _currentTypeListFilter = null;

            // first pass, visit types *not* mentioned in ConditionalTypeLists
            WriteConditionStart(DefaultCondition);
            Visit(namespaces);
            WriteConditionEnd(DefaultCondition);

            // second pass, visit types mentioned in ConditionalTypeLists
            foreach (var typeList in ConditionalTypeLists ?? Enumerable.Empty<ConditionalTypeList>())
            {
                _currentTypeListFilter = typeList.Filter;

                WriteConditionStart(typeList.Symbol);
                Visit(namespaces.Where(_currentTypeListFilter.Include));
                WriteConditionEnd(typeList.Symbol);
            }
        }

        public override void Visit(INamespaceDefinition ns)
        {
            if (ns != null && string.IsNullOrEmpty(ns.Name.Value))
            {
                base.Visit(ns);
            }
            else
            {
                _declarationWriter.WriteDeclaration(ns);

                using (_syntaxWriter.StartBraceBlock(PutBraceOnNewLine))
                {
                    var types = ns.GetTypes(this.IncludeForwardedTypes);
                    if (ConditionalTypeLists != null)
                    {
                        // in the first pass we want all types *except* the ones in ConditionalTypeLists filters
                        // in the second pass we want *only* the types in ConditionalTypeLists filters
                        var firstPass = _currentTypeListFilter == null;
                        types = types.Where(t => ConditionalTypeLists.Any(c => c.Filter.Include(t) == !firstPass));
                    }

                    Visit(types);
                }
            }

            _syntaxWriter.WriteLine();
        }

        public override void Visit(IEnumerable<ITypeDefinition> types)
        {
            WriteMemberGroupHeader(types.FirstOrDefault(Filter.Include) as ITypeDefinitionMember);
            base.Visit(types);
        }

        public override void Visit(ITypeDefinition type)
        {
            byte? value = type.Attributes.GetCustomAttributeArgumentValue<byte?>(CSharpCciExtensions.NullableContextAttributeFullName);
            if (!(type is INestedTypeDefinition) || value != null) // Only override the value when we're not on a nested type, or if so, only if the value is not null.
            {
                _declarationWriter.TypeNullableContextValue = value;
            }

            _declarationWriter.WriteDeclaration(type);

            if (!type.IsDelegate)
            {
                using (_syntaxWriter.StartBraceBlock(PutBraceOnNewLine))
                {
                    // If we have no constructors then output a private one this
                    // prevents the C# compiler from creating a default public one.
                    var constructors = type.Methods.Where(m => m.IsConstructor && Filter.Include(m));
                    if (!type.IsStatic && !constructors.Any())
                    {
                        // HACK... this will likely not work for any thing other than CSDeclarationWriter
                        _declarationWriter.WriteDeclaration(CSDeclarationWriter.GetDummyConstructor(type));
                        _syntaxWriter.WriteLine();
                    }

                    _firstMemberGroup = true;
                    base.Visit(type);
                }
            }
            _syntaxWriter.WriteLine();
        }

        public override void Visit(IEnumerable<ITypeDefinitionMember> members)
        {
            WriteMemberGroupHeader(members.FirstOrDefault(Filter.Include));
            base.Visit(members);
        }

        public override void Visit(ITypeDefinition parentType, IEnumerable<IFieldDefinition> fields)
        {
            if (parentType.IsStruct && !_apiOnly)
            {
                // For compile-time compat, the following rules should work for producing a reference assembly. We drop all private fields,
                // but add back certain synthesized private fields for a value type (struct) as follows:

                // 1. If there is a ref field or reference type field in the struct or within the fields' type closure,
                //    it should emit a reference type and a value type dummy field.
                //    - The reference type dummy field is needed in order to inform the compiler to block
                //      taking pointers to this struct because the GC will not track updating those references.
                //    - The value type dummy field is needed in order for the compiler to error correctly on definite assignment checks in all scenarios. dotnet/roslyn#30194

                // 2. If there are no reference type fields, but there are value type fields in the struct field closure,
                //    and at least one of these fields is a nonempty struct, then we should emit a value type dummy field.

                //    - The previous rules are for definite assignment checks, so the compiler knows there is a private field
                //      that has not been initialized to error about uninitialized structs.
                //
                // 3. If the type is generic, then for every type parameter of the type, if there are any private
                //    or internal fields that are or contain any members whose type is that type parameter,
                //    we add a direct private field of that type.

                //    - Compiler needs to see all fields that have generic arguments (even private ones) to be able
                //      to validate there aren't any struct layout cycles.

                // Note: By "private", we mean not visible outside the assembly.

                // For more details see issue https://github.com/dotnet/corefx/issues/6185
                // this blog is helpful as well http://blog.paranoidcoding.com/2016/02/15/are-private-members-api-surface.html

                List<IFieldDefinition> newFields = new List<IFieldDefinition>();
                var includedVisibleFields = fields.Where(f => _cciFilter.Include(f));
                includedVisibleFields = includedVisibleFields.OrderBy(GetMemberKey, StringComparer.OrdinalIgnoreCase);

                var excludedFields = fields.Except(includedVisibleFields).Where(f => !f.IsStatic);

                if (excludedFields.Any())
                {
                    var genericTypedFields = excludedFields.Where(f => f.Type.UnWrap().IsGenericParameter());
                    foreach (var genericField in genericTypedFields)
                    {
                        IFieldDefinition fieldType = new DummyPrivateField(parentType, genericField.Type, genericField.Name.Value, genericField.Attributes.Where(a => !a.FullName().EndsWith("NullAttribute")), genericField.IsReadOnly);
                        newFields.Add(fieldType);
                    }

                    IFieldDefinition intField = DummyFieldWriterHelper(parentType, excludedFields, parentType.PlatformType.SystemInt32, "_dummyPrimitive");
                    bool hasRefPrivateField = excludedFields.Any(f => f.Type.IsOrContainsReferenceType());
                    if (hasRefPrivateField)
                    {
                        newFields.Add(DummyFieldWriterHelper(parentType, excludedFields, parentType.PlatformType.SystemObject));
                        newFields.Add(intField);
                    }
                    else
                    {
                        bool hasNonEmptyStructPrivateField = excludedFields.Any(f => f.Type.IsOrContainsNonEmptyStruct());
                        if (hasNonEmptyStructPrivateField)
                        {
                            newFields.Add(intField);
                        }
                    }
                }

                foreach (var visibleField in includedVisibleFields)
                    newFields.Add(visibleField);

                foreach (var field in newFields)
                    Visit(field);
            }
            else
            {
                base.Visit(parentType, fields);
            }
        }

        private IFieldDefinition DummyFieldWriterHelper(ITypeDefinition parentType, IEnumerable<IFieldDefinition> excludedFields, ITypeReference fieldType, string fieldName = "_dummy")
        {
            // For primitive types that have a field of their type set the dummy field to that type
            if (excludedFields.Count() == 1)
            {
                var onlyField = excludedFields.First();

                if (TypeHelper.TypesAreEquivalent(onlyField.Type, parentType))
                {
                    fieldType = parentType;
                }
            }

            return new DummyPrivateField(parentType, fieldType, fieldName);
        }

        public override void Visit(ITypeDefinitionMember member)
        {
            IDisposable style = null;

            if (_styleWriter != null)
            {
                // Favor overrides over interface implementations (i.e. consider override Dispose() as an override and not an interface implementation)
                if (this.HighlightBaseMembers && member.IsOverride())
                    style = _styleWriter.StartStyle(SyntaxStyle.InheritedMember);
                else if (this.HighlightInterfaceMembers && member.IsInterfaceImplementation())
                    style = _styleWriter.StartStyle(SyntaxStyle.InterfaceMember);
            }

            _declarationWriter.WriteDeclaration(member);

            if (style != null)
                style.Dispose();

            _syntaxWriter.WriteLine();
            base.Visit(member);
        }

        private void WriteMemberGroupHeader(ITypeDefinitionMember member)
        {
            if (IncludeMemberGroupHeadings || IncludeSpaceBetweenMemberGroups)
            {
                string header = CSharpWriter.MemberGroupHeading(member);

                if (header != null)
                {
                    if (IncludeSpaceBetweenMemberGroups)
                    {
                        if (!_firstMemberGroup)
                            _syntaxWriter.WriteLine(true);
                        _firstMemberGroup = false;
                    }

                    if (IncludeMemberGroupHeadings)
                    {
                        IDisposable dispose = null;
                        if (_styleWriter != null)
                            dispose = _styleWriter.StartStyle(SyntaxStyle.Comment);

                        _syntaxWriter.Write("// {0}", header);

                        if (dispose != null)
                            dispose.Dispose();
                        _syntaxWriter.WriteLine();
                    }
                }
            }
        }

        private void WriteConditionStart(string condition)
        {
            if (!string.IsNullOrEmpty(condition))
            {
                _syntaxWriter.Write($"#if {condition}");
                _syntaxWriter.WriteLine();
            }
        }

        private void WriteConditionEnd(string condition)
        {
            if (!string.IsNullOrEmpty(condition))
            {
                _syntaxWriter.Write($"#endif // {condition}");
                _syntaxWriter.WriteLine();
            }
        }

        public static string MemberGroupHeading(ITypeDefinitionMember member)
        {
            if (member == null)
                return null;

            IMethodDefinition method = member as IMethodDefinition;
            if (method != null)
            {
                if (method.IsConstructor)
                    return "Constructors";

                return "Methods";
            }

            IFieldDefinition field = member as IFieldDefinition;
            if (field != null)
                return "Fields";

            IPropertyDefinition property = member as IPropertyDefinition;
            if (property != null)
                return "Properties";

            IEventDefinition evnt = member as IEventDefinition;
            if (evnt != null)
                return "Events";

            INestedTypeDefinition nType = member as INestedTypeDefinition;
            if (nType != null)
                return "Nested Types";

            return null;
        }

        public void Dispose()
        {
            _declarationWriter?.Dispose();
        }
    }
}
