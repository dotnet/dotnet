// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Microsoft.Cci.Extensions;
using Microsoft.Cci.Extensions.CSharp;

namespace Microsoft.Cci.Writers.CSharp
{
    public partial class CSDeclarationWriter
    {
        public void WriteTypeDeclaration(ITypeDefinition type)
        {
            INamedTypeDefinition namedType = (INamedTypeDefinition)type;

            WriteAttributes(type.Attributes);
            WriteAttributes(type.SecurityAttributes);

            //TODO: We should likely add support for the SerializableAttribute something like:
            // if (type.IsSerializable) WriteFakeAttribute("System.Serializable");
            // But we need also consider if this attribute is filtered out or not but I guess
            // we have the same problem with all the fake attributes at this point.

            if ((type.IsStruct || type.IsClass) && type.Layout != LayoutKind.Auto)
            {
                FakeCustomAttribute structLayout = new FakeCustomAttribute("System.Runtime.InteropServices", "StructLayoutAttribute");
                string layoutKind = string.Format("System.Runtime.InteropServices.LayoutKind.{0}", type.Layout.ToString());

                if (_forCompilationIncludeGlobalprefix)
                    layoutKind = "global::" + layoutKind;

                var args = new List<string>();
                args.Add(layoutKind);

                if (type.SizeOf != 0)
                {
                    string sizeOf = string.Format("Size={0}", type.SizeOf);
                    args.Add(sizeOf);
                }

                if (type.Alignment != 0)
                {
                    string pack = string.Format("Pack={0}", type.Alignment);
                    args.Add(pack);
                }

                if (type.StringFormat != StringFormatKind.Ansi)
                {
                    string charset = string.Format("CharSet={0}System.Runtime.InteropServices.CharSet.{1}", _forCompilationIncludeGlobalprefix ? "global::" : "", type.StringFormat);
                    args.Add(charset);
                }

                if (IncludeAttribute(structLayout))
                    WriteFakeAttribute(structLayout.FullTypeName, args.ToArray());
            }

            WriteVisibility(TypeHelper.TypeVisibilityAsTypeMemberVisibility(type));

            IMethodDefinition invoke = type.GetInvokeMethod();
            if (invoke != null)
            {
                Contract.Assert(type.IsDelegate);

                byte? nullableContextValue = invoke.Attributes.GetCustomAttributeArgumentValue<byte?>(CSharpCciExtensions.NullableContextAttributeFullName);
                if (invoke.IsMethodUnsafe()) WriteKeyword("unsafe");
                WriteKeyword("delegate");
                WriteTypeName(invoke.Type, invoke.ReturnValueAttributes, methodNullableContextValue: nullableContextValue);
                WriteIdentifier(namedType.Name);
                if (type.IsGeneric) WriteGenericParameters(type.GenericParameters);
                WriteParameters(invoke.Parameters, invoke.ContainingType, nullableContextValue);
                if (type.IsGeneric) WriteGenericContraints(type.GenericParameters, TypeNullableContextValue); // Delegates are special, and the NullableContextValue we should fallback to is the delegate type one, not the invoke method one.
                WriteSymbol(";");
            }
            else
            {
                WriteTypeModifiers(type);
                WriteIdentifier(namedType.Name);
                Contract.Assert(!(type is IGenericTypeInstance), "Currently don't support generic type instances if we hit this then add support");
                if (type.IsGeneric) WriteGenericParameters(type.GenericParameters);
                WriteBaseTypes(type);
                if (type.IsGeneric) WriteGenericContraints(type.GenericParameters);

                if (type.IsEnum)
                    WriteEnumType(type);
            }
        }

        // Note that the metadata order for interfaces may change from one release to another.
        // This isn't an incompatibility in surface area.  So, we must sort our list of base types
        // to reflect this.
        private void WriteBaseTypes(ITypeDefinition type)
        {
            ITypeReference baseType = GetBaseType(type);
            IEnumerable<ITypeReference> interfaces = type.Interfaces.Where(IncludeBaseType).OrderBy(t => GetTypeName(t), StringComparer.OrdinalIgnoreCase);

            if (baseType == null && !interfaces.Any())
                return;

            WriteSpace();
            WriteSymbol(":", true);

            if (baseType != null)
            {
                WriteTypeName(baseType, type.Attributes, noSpace: true);
                if (interfaces.Any())
                {
                    WriteSymbol(",", addSpace: true);
                }
            }

            WriteList(GetInterfaceWriterActions(type, interfaces), i => i());
        }

        private IEnumerable<Action> GetInterfaceWriterActions(ITypeReference type, IEnumerable<ITypeReference> interfaces)
        {
            if (interfaces.Any())
            {
                string location = type.Locations.FirstOrDefault()?.Document?.Location;
                uint typeToken = ((IMetadataObjectWithToken)type).TokenValue;
                foreach (var interfaceImplementation in interfaces)
                {
                    object nullableAttributeValue = null;
                    if (location != null)
                    {
                        nullableAttributeValue = interfaceImplementation.GetInterfaceImplementationAttributeConstructorArgument(typeToken, location, _metadataReaderCache, CSharpCciExtensions.NullableConstructorArgumentParser);
                    }

                    yield return () => WriteTypeName(interfaceImplementation, noSpace: true, nullableAttributeArgument: nullableAttributeValue);
                }
            }
        }

        private string GetTypeName(ITypeReference type)
        {
            NameFormattingOptions formattingOptions = NameFormattingOptions.TypeParameters | NameFormattingOptions.UseTypeKeywords;

            if (!_forCompilation)
            {
                formattingOptions |= NameFormattingOptions.OmitContainingNamespace;
            }

            return GetTypeName(type, formattingOptions);
        }

        private string GetTypeName(ITypeReference type, NameFormattingOptions formattingOptions)
        {
            if ((formattingOptions & NameFormattingOptions.UseTypeKeywords) != 0)
            {
                if (TypeHelper.TypesAreEquivalent(type, type.PlatformType.SystemIntPtr))
                {
                    if ((LangVersion >= LangVersion11_0) || type.Attributes.HasNativeIntegerAttribute())
                    {
                        return "nint";
                    }
                }
                else if (TypeHelper.TypesAreEquivalent(type, type.PlatformType.SystemUIntPtr))
                {
                    if ((LangVersion >= LangVersion11_0) || type.Attributes.HasNativeIntegerAttribute())
                    {
                        return "nuint";
                    }
                }
            }
            return TypeHelper.GetTypeName(type, formattingOptions);
        }

        private ITypeReference GetBaseType(ITypeDefinition type)
        {
            if (type == Dummy.Type)
                return null;

            ITypeReference baseTypeRef = type.BaseClasses.FirstOrDefault();

            if (baseTypeRef == null)
                return null;

            if (baseTypeRef.IsDefaultCSharpBaseType(type))
                return null;

            if (!IncludeBaseType(baseTypeRef))
            {
                return GetBaseType(baseTypeRef.ResolvedType);
            }

            return baseTypeRef;
        }

        private void WriteTypeModifiers(ITypeDefinition type)
        {
            if (type.GetHiddenBaseType(_filter) != Dummy.Type)
            {
                WriteKeyword("new");
            }

            if (type.IsDelegate)
                throw new NotSupportedException("This method doesn't support delegates!");
            else if (type.IsEnum)
                WriteKeyword("enum");
            else if (type.IsValueType)
            {
                if (type.Attributes.HasIsReadOnlyAttribute())
                    WriteKeyword("readonly");

                if (type.Attributes.HasIsByRefLikeAttribute())
                    WriteKeyword("ref");

                WritePartialKeyword();
                WriteKeyword("struct");
            }
            else if (type.IsInterface)
            {
                WritePartialKeyword();
                WriteKeyword("interface");
            }
            else
            {
                if (!type.IsClass)
                    throw new NotSupportedException("Don't understand what kind of type this is!");

                if (type.IsStatic)
                    WriteKeyword("static");
                else if (type.IsSealed)
                    WriteKeyword("sealed");
                else if (type.IsAbstract)
                    WriteKeyword("abstract");

                WritePartialKeyword();
                WriteKeyword("class");
            }
        }

        private void WritePartialKeyword()
        {
            if (_forCompilation)
                WriteKeyword("partial");
        }

        private void WriteEnumType(ITypeDefinition type)
        {
            ITypeReference enumType = type.GetEnumType();

            // Don't write the default type
            if (TypeHelper.TypesAreEquivalent(enumType, type.PlatformType.SystemInt32))
                return;

            WriteSpace();
            WriteSymbol(":", addSpace: true);
            WriteTypeName(enumType, noSpace: true);
        }

        private bool IncludeBaseType(ITypeReference iface)
        {
            ITypeDefinition ifaceType = iface.ResolvedType;

            // We should by default include base types even if we cannot resolve
            // for cases where we are working with standalone assemblies.
            if (ifaceType == Dummy.Type)
                return true;

            return _alwaysIncludeBase || _filter.Include(ifaceType);
        }
    }
}
