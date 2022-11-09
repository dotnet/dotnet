// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Cci.Extensions;
using Microsoft.Cci.Extensions.CSharp;
using Microsoft.Cci.Filters;
using Microsoft.Cci.Writers.Syntax;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Microsoft.Cci.Writers.CSharp
{
    public partial class CSDeclarationWriter : ICciDeclarationWriter, IDisposable
    {
        public static readonly Version LangVersion7_0 = new Version(7, 0);
        public static readonly Version LangVersion8_0 = new Version(8, 0);
        public static readonly Version LangVersion9_0 = new Version(9, 0);
        public static readonly Version LangVersion10_0 = new Version(10, 0);
        public static readonly Version LangVersion11_0 = new Version(11, 0);

        public static readonly Version LangVersionDefault = LangVersion7_0;
        public static readonly Version LangVersionLatest = LangVersion10_0;
        public static readonly Version LangVersionPreview = LangVersion11_0;

        private readonly SRMetadataPEReaderCache _metadataReaderCache;
        private readonly ISyntaxWriter _writer;
        private readonly ICciFilter _filter;
        private bool _forCompilation;
        private bool _forCompilationIncludeGlobalprefix;
        private string _platformNotSupportedExceptionMessage;
        private bool _includeFakeAttributes;
        private bool _alwaysIncludeBase;

        public CSDeclarationWriter(ISyntaxWriter writer)
            : this(writer, new PublicOnlyCciFilter())
        {
        }

        public CSDeclarationWriter(ISyntaxWriter writer, ICciFilter filter)
            : this(writer, filter, true)
        {
        }

        public CSDeclarationWriter(ISyntaxWriter writer, ICciFilter filter, bool forCompilation)
        {
            Contract.Requires(writer != null);
            _writer = writer;
            _filter = filter;
            _forCompilation = forCompilation;
            _forCompilationIncludeGlobalprefix = false;
            _platformNotSupportedExceptionMessage = null;
            _includeFakeAttributes = false;
            _alwaysIncludeBase = false;
            _metadataReaderCache = new SRMetadataPEReaderCache();
        }

        public CSDeclarationWriter(ISyntaxWriter writer, ICciFilter filter, bool forCompilation, bool includePseudoCustomAttributes = false)
            : this(writer, filter, forCompilation)
        {
            _includeFakeAttributes = includePseudoCustomAttributes;
        }

        public bool ForCompilation
        {
            get { return _forCompilation; }
            set { _forCompilation = value; }
        }

        public bool ForCompilationIncludeGlobalPrefix
        {
            get { return _forCompilationIncludeGlobalprefix; }
            set { _forCompilationIncludeGlobalprefix = value; }
        }

        public string PlatformNotSupportedExceptionMessage
        {
            get { return _platformNotSupportedExceptionMessage; }
            set { _platformNotSupportedExceptionMessage = value; }
        }

        public bool AlwaysIncludeBase
        {
            get { return _alwaysIncludeBase; }
            set { _alwaysIncludeBase = value; }
        }

        public ISyntaxWriter SyntaxtWriter { get { return _writer; } }

        public ICciFilter Filter { get { return _filter; } }

        public Version LangVersion { get; set; }

        public byte? ModuleNullableContextValue { get; set; }
        public byte? TypeNullableContextValue { get; set; }

        public void WriteDeclaration(IDefinition definition)
        {
            if (definition == null)
                return;

            IAssembly assembly = definition as IAssembly;
            if (assembly != null)
            {
                WriteAssemblyDeclaration(assembly);
                return;
            }

            INamespaceDefinition ns = definition as INamespaceDefinition;
            if (ns != null)
            {
                WriteNamespaceDeclaration(ns);
                return;
            }

            ITypeDefinition type = definition as ITypeDefinition;
            if (type != null)
            {
                WriteTypeDeclaration(type);
                return;
            }

            ITypeDefinitionMember member = definition as ITypeDefinitionMember;
            if (member != null)
            {
                WriteMemberDeclaration(member);
                return;
            }

            DummyInternalConstructor ctor = definition as DummyInternalConstructor;
            if (ctor != null)
            {
                WritePrivateConstructor(ctor.ContainingType);
                return;
            }

            INamedEntity named = definition as INamedEntity;
            if (named != null)
            {
                WriteIdentifier(named.Name);
                return;
            }

            _writer.Write("Unknown definition type {0}", definition.ToString());
        }

        public void WriteAttribute(ICustomAttribute attribute)
        {
            WriteSymbol("[");
            WriteAttribute(attribute, null);
            WriteSymbol("]");
        }

        public void WriteAssemblyDeclaration(IAssembly assembly)
        {
            WriteAttributes(assembly.Attributes, prefix: "assembly");
            WriteAttributes(assembly.SecurityAttributes, prefix: "assembly");
        }

        public void WriteMemberDeclaration(ITypeDefinitionMember member)
        {
            IMethodDefinition method = member as IMethodDefinition;
            if (method != null)
            {
                WriteMethodDefinition(method);
                return;
            }

            IPropertyDefinition property = member as IPropertyDefinition;
            if (property != null)
            {
                WritePropertyDefinition(property);
                return;
            }

            IEventDefinition evnt = member as IEventDefinition;
            if (evnt != null)
            {
                WriteEventDefinition(evnt);
                return;
            }

            IFieldDefinition field = member as IFieldDefinition;
            if (field != null)
            {
                WriteFieldDefinition(field);
                return;
            }

            _writer.Write("Unknown member definitions type {0}", member.ToString());
        }

        private void WriteVisibility(TypeMemberVisibility visibility)
        {
            switch (visibility)
            {
                case TypeMemberVisibility.Public:
                    WriteKeyword("public"); break;
                case TypeMemberVisibility.Private:
                    WriteKeyword("private"); break;
                case TypeMemberVisibility.Assembly:
                    WriteKeyword("internal"); break;
                case TypeMemberVisibility.Family:
                    WriteKeyword("protected"); break;
                case TypeMemberVisibility.FamilyOrAssembly:
                    WriteKeyword("protected"); WriteKeyword("internal"); break;
                case TypeMemberVisibility.FamilyAndAssembly:
                    WriteKeyword("private"); WriteKeyword("protected"); break;
                default:
                    WriteKeyword("<Unknown-Visibility>"); break;
            }
        }

        private void WriteCustomModifiers(IEnumerable<ICustomModifier> modifiers)
        {
            foreach (ICustomModifier modifier in modifiers)
            {
                if (modifier.Modifier.FullName() == "System.Runtime.CompilerServices.IsVolatile")
                    WriteKeyword("volatile");
            }
        }

        // Writer Helpers these are the only methods that should directly access _writer
        private void WriteKeyword(string keyword, bool noSpace = false)
        {
            _writer.WriteKeyword(keyword);
            if (!noSpace) WriteSpace();
        }

        private void WriteSymbol(string symbol, bool addSpace = false)
        {
            _writer.WriteSymbol(symbol);
            if (addSpace)
                WriteSpace();
        }

        private void Write(string literal)
        {
            _writer.Write(literal);
        }

        private void WriteNullableSymbolForReferenceType(object nullableAttributeArgument, int arrayIndex)
        {
            if (nullableAttributeArgument is null)
            {
                return;
            }

            byte attributeByteValue;
            if (nullableAttributeArgument is byte[] attributeArray)
            {
                attributeByteValue = attributeArray[arrayIndex];
            }
            else
            {
                attributeByteValue = (byte)nullableAttributeArgument;
            }

            if ((attributeByteValue & 2) != 0)
            {
                WriteNullableSymbol();
            }
        }

        private void WriteNullableSymbol()
        {
            _writer.WriteSymbol("?");
        }

        private bool IsDynamicType(object dynamicAttributeArgument, int arrayIndex)
        {
            if (dynamicAttributeArgument == null)
            {
                return false;
            }

            if (dynamicAttributeArgument is bool[] attributeArray)
            {
                return attributeArray[arrayIndex];
            }

            return (bool)dynamicAttributeArgument;
        }

        private ref struct TypeNameRecursiveState
        {
            public object DynamicAttributeArgument;
            public object NullableAttributeArgument;
            public IEnumerable<ICustomAttribute> Attributes;
        }

        private int WriteTypeNameRecursive(ITypeReference type, NameFormattingOptions namingOptions,
            string[] valueTupleNames, scoped ref int valueTupleNameIndex, scoped ref int nullableIndex, ref TypeNameRecursiveState state,
            int typeDepth = 0, int genericParameterIndex = 0, bool isValueTupleParameter = false)
        {
            object dynamicAttributeArgument = state.DynamicAttributeArgument;
            void WriteTypeNameInner(ITypeReference typeReference)
            {
                if (IsDynamicType(dynamicAttributeArgument, typeDepth))
                {
                    _writer.WriteKeyword("dynamic");
                }
                else
                {
                    string name;
                    if (typeReference is INestedTypeReference nestedType && (namingOptions & NameFormattingOptions.OmitTypeArguments) != 0)
                    {
                        name = GetTypeName(nestedType.ContainingType, namingOptions & ~NameFormattingOptions.OmitTypeArguments);
                        name += ".";
                        name += GetTypeName(nestedType, namingOptions | NameFormattingOptions.OmitContainingType);
                    }
                    else
                    {
                        name = GetTypeName(typeReference, namingOptions);
                    }

                    if (CSharpCciExtensions.IsKeyword(name))
                        _writer.WriteKeyword(name);
                    else
                        _writer.WriteTypeName(name);
                }
            }

            int genericArgumentsCount = 0;
            bool isNullableValueType = false;
            int nullableLocalIndex = nullableIndex;
            if (type is IGenericTypeInstanceReference genericType)
            {
                genericArgumentsCount = genericType.GenericArguments.Count();

                int genericArgumentsInChildTypes = 0;
                int valueTupleLocalIndex = valueTupleNameIndex;
                bool isValueTuple = genericType.IsValueTuple();
                bool shouldWriteNestedValueTuple = !isValueTupleParameter || genericParameterIndex != 7;
                isNullableValueType = genericType.IsNullableValueType();

                if (isNullableValueType)
                {
                    namingOptions &= ~NameFormattingOptions.ContractNullable;

                    if (typeDepth == 0)
                    {
                        // If we're at the root of the type and is a Nullable<T>,
                        // we need to start at -1 since a byte is not emitted in the nullable attribute for it.
                        nullableIndex--;
                    }
                }
                else
                {
                    if (isValueTuple)
                    {
                        if (shouldWriteNestedValueTuple)
                        {
                            // The compiler doesn't allow (T1) for tuples, it must have at least 2 arguments.
                            if (genericArgumentsCount > 1)
                            {
                                _writer.WriteSymbol("(");
                            }
                            else
                            {
                                WriteTypeNameInner(type);
                                _writer.WriteSymbol("<");
                            }
                        }
                        valueTupleNameIndex += genericArgumentsCount;
                    }
                    else
                    {
                        WriteTypeNameInner(type);
                        _writer.WriteSymbol("<");
                    }
                }

                int i = 0;
                foreach (var parameter in genericType.GenericArguments)
                {
                    if (i != 0)
                    {
                        _writer.WriteSymbol(",");
                        _writer.WriteSpace();
                    }

                    // Rules for nullable index are as follows.
                    // A value in the NullableAttribute(byte[]) is emitted if:
                    // It is a generic type and it is not Nullable<T>
                    // It is a reference type
                    if (!parameter.IsValueType || (parameter is IGenericTypeInstanceReference gt && !gt.IsNullableValueType()) || (parameter is IArrayType))
                    {
                        nullableIndex++;
                    }

                    string valueTupleName = isValueTuple ? valueTupleNames?[valueTupleLocalIndex + i] : null;
                    int destinationTypeDepth = typeDepth + i + genericArgumentsInChildTypes + 1;
                    genericArgumentsInChildTypes += WriteTypeNameRecursive(parameter, namingOptions, valueTupleNames, ref valueTupleNameIndex, ref nullableIndex, ref state, destinationTypeDepth, i, isValueTuple);

                    if (valueTupleName != null)
                    {
                        _writer.WriteSpace();
                        _writer.WriteIdentifier(valueTupleName);
                    }

                    i++;
                }

                if (!isNullableValueType)
                {
                    if (isValueTuple)
                    {
                        if (shouldWriteNestedValueTuple)
                        {
                            // The compiler doesn't allow (T1) for tuples, it must have at least 2 arguments.
                            if (genericArgumentsCount > 1)
                            {
                                _writer.WriteSymbol(")");
                            }
                            else
                            {
                                _writer.WriteSymbol(">");
                            }
                        }
                    }
                    else
                    {
                        _writer.WriteSymbol(">");
                    }
                }
            }
            else if (type is IArrayType arrayType)
            {
                if (!arrayType.ElementType.IsValueType)
                    nullableIndex++;

                WriteTypeNameRecursive(arrayType.ElementType, namingOptions, valueTupleNames, ref valueTupleNameIndex, ref nullableIndex,
                    ref state, typeDepth + 1);
                WriteSymbol("[");

                uint arrayDimension = arrayType.Rank - 1;
                for (; arrayDimension > 0; arrayDimension--)
                {
                    WriteSymbol(",");
                }

                WriteSymbol("]");
            }
            else if (type.IsByRef())
            {
                WriteSymbol("ref", addSpace: true);

                if (state.Attributes.HasIsReadOnlyAttribute())
                    WriteSymbol("readonly", addSpace: true);

                WriteTypeNameInner(((IManagedPointerType)type).TargetType);
            }
            else
            {
                WriteTypeNameInner(type);
            }

            if (isNullableValueType)
            {
                WriteNullableSymbol();
            }
            else if (!type.IsValueType)
            {
                WriteNullableSymbolForReferenceType(state.NullableAttributeArgument, nullableLocalIndex);
            }

            return genericArgumentsCount;
        }

        private void WriteTypeName(ITypeReference type, IEnumerable<ICustomAttribute> attributes, object methodNullableContextValue = null, bool noSpace = false, bool useTypeKeywords = true,
            bool omitGenericTypeList = false, bool includeReferenceTypeNullability = true)
        {
            attributes.TryGetAttributeOfType(CSharpCciExtensions.NullableAttributeFullName, out ICustomAttribute nullableAttribute);
            bool hasDynamicAttribute = attributes.TryGetAttributeOfType("System.Runtime.CompilerServices.DynamicAttribute", out ICustomAttribute dynamicAttribute);

            object nullableAttributeArgument = null;
            if (includeReferenceTypeNullability)
                nullableAttributeArgument = nullableAttribute.GetAttributeArgumentValue<byte>() ?? methodNullableContextValue ?? TypeNullableContextValue ?? ModuleNullableContextValue;

            object dynamicAttributeArgument = dynamicAttribute.GetAttributeArgumentValue<bool>(defaultValue: hasDynamicAttribute);

            WriteTypeName(type, noSpace, useTypeKeywords, omitGenericTypeList, nullableAttributeArgument, dynamicAttributeArgument, attributes);
        }

        private void WriteTypeName(ITypeReference type, bool noSpace = false, bool useTypeKeywords = true,
            bool omitGenericTypeList = false, object nullableAttributeArgument = null, object dynamicAttributeArgument = null, IEnumerable<ICustomAttribute> attributes = null)
        {
            NameFormattingOptions namingOptions = NameFormattingOptions.TypeParameters | NameFormattingOptions.ContractNullable | NameFormattingOptions.OmitTypeArguments;

            if (useTypeKeywords)
                namingOptions |= NameFormattingOptions.UseTypeKeywords;

            if (_forCompilationIncludeGlobalprefix)
                namingOptions |= NameFormattingOptions.UseGlobalPrefix;

            if (!_forCompilation)
                namingOptions |= NameFormattingOptions.OmitContainingNamespace;

            if (omitGenericTypeList)
                namingOptions |= NameFormattingOptions.EmptyTypeParameterList;

            int valueTupleNameIndex = 0;
            int nullableIndex = 0;
            var state = new TypeNameRecursiveState()
            {
                DynamicAttributeArgument = dynamicAttributeArgument,
                NullableAttributeArgument = nullableAttributeArgument,
                Attributes = attributes,
            };
            WriteTypeNameRecursive(type, namingOptions, attributes?.GetValueTupleNames(), ref valueTupleNameIndex, ref nullableIndex, ref state);

            if (!noSpace) WriteSpace();
        }

        public void WriteIdentifier(string id)
        {
            WriteIdentifier(id, true);
        }

        public void WriteIdentifier(string id, bool escape)
        {
            // Escape keywords
            if (escape && CSharpCciExtensions.IsKeyword(id))
                id = "@" + id;
            _writer.WriteIdentifier(id);
        }

        private void WriteIdentifier(IName name)
        {
            WriteIdentifier(name.Value);
        }

        private void WriteSpace()
        {
            _writer.Write(" ");
        }

        private void WriteList<T>(IEnumerable<T> list, Action<T> writeItem)
        {
            _writer.WriteList(list, writeItem);
        }

        public void Dispose()
        {
            _metadataReaderCache.Dispose();
        }
    }
}
