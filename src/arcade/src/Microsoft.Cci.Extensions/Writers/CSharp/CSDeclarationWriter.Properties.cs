// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Linq;
using Microsoft.Cci.Extensions.CSharp;

namespace Microsoft.Cci.Writers.CSharp
{
    public partial class CSDeclarationWriter
    {
        private void WritePropertyDefinition(IPropertyDefinition property)
        {
            IMethodDefinition accessor = null;
            IMethodDefinition getter = null;
            IMethodDefinition setter = null;
            if (property.Getter != null)
            {
                getter = property.Getter.ResolvedMethod;
                if (!_filter.Include(getter))
                    getter = null;
                accessor = getter;
            }

            if (property.Setter != null)
            {
                setter = property.Setter.ResolvedMethod;
                if (!_filter.Include(setter))
                    setter = null;
                if (accessor == null)
                    accessor = setter;
            }

            if (accessor == null)
                return;

            bool isIndexer = accessor.ParameterCount > (accessor == setter ? 1 : 0);

            if (isIndexer)
            {
                string id = property.Name.Value;
                int index = id.LastIndexOf(".");
                if (index >= 0)
                    id = id.Substring(index + 1);

                if (id != "Item")
                {
                    WriteFakeAttribute("System.Runtime.CompilerServices.IndexerName", "\"" + id + "\"");
                }
            }

            WriteAttributes(property.Attributes);

            // We need to preserve nullable custom attributes which are preserved by the compiler as param on the value parameter and return attributes.
            if (getter != null)
                WriteAttributes(getter.ReturnValueAttributes);
            if (setter != null)
                WriteAttributes(setter.Parameters.Last().Attributes);

            var writeVisibility = true;

            if (property.ContainingTypeDefinition.IsInterface)
            {
                writeVisibility = false;
            }

            if (accessor.IsExplicitInterfaceMethod())
            {
                writeVisibility = false;
            }

            if (writeVisibility)
            {
                WriteVisibility(property.Visibility);
            }

            if (property.Attributes.HasRequiredMemberAttribute())
            {
                WriteKeyword("required");
            }

            // Getter and Setter modifiers should be the same
            WriteMethodModifiers(accessor);

            if (property.GetHiddenBaseProperty(_filter) != Dummy.Property)
                WriteKeyword("new");

            bool getterHasIsReadOnlyAttribute = (getter?.Attributes.HasIsReadOnlyAttribute()).GetValueOrDefault();
            bool setterHasIsReadOnlyAttribute = (setter?.Attributes.HasIsReadOnlyAttribute()).GetValueOrDefault();

            // The readonly modifier is applied on the property itself if:
            //  * It has both a getter and a setter and both have IsReadOnlyAttribute
            //  * It only has a getter or a setter and it has IsReadOnlyAttribute
            // Otherwise, the attribute is applied directly on the getter/setter it exists for
            bool allAccessorsHaveIsReadOnlyAttribute = (getterHasIsReadOnlyAttribute && setterHasIsReadOnlyAttribute) ||
                                                       (getterHasIsReadOnlyAttribute && (setter is null)) ||
                                                       (setterHasIsReadOnlyAttribute && (getter is null));

            if (allAccessorsHaveIsReadOnlyAttribute && (LangVersion >= LangVersion8_0))
            {
                WriteKeyword("readonly");
            }

            if (property.ReturnValueIsByRef)
            {
                WriteKeyword("ref");

                if (property.Attributes.HasIsReadOnlyAttribute())
                    WriteKeyword("readonly");
            }

            WriteTypeName(property.Type, property.Attributes);

            if (property.IsExplicitInterfaceProperty() && _forCompilationIncludeGlobalprefix)
                Write("global::");

            WritePropertyName(property, accessor, accessor == setter, isIndexer);
            WriteSpace();
            WriteSymbol("{");

            //get
            if (getter != null)
            {
                bool isReadOnly = getterHasIsReadOnlyAttribute && !allAccessorsHaveIsReadOnlyAttribute;
                WriteAccessorDefinition(property, getter, "get", isReadOnly);
            }
            //set
            if (setter != null)
            {
                bool isReadOnly = setterHasIsReadOnlyAttribute && !allAccessorsHaveIsReadOnlyAttribute;
                WriteAccessorDefinition(property, setter, "set", isReadOnly);
            }
            WriteSpace();
            WriteSymbol("}");
        }

        private void WritePropertyName(IPropertyDefinition property, IMethodDefinition accessor, bool isSetterAccessor, bool isIndexer)
        {
            if (property.IsExplicitInterfaceProperty())
            {
                IMethodImplementation methodImplementation = accessor.GetMethodImplementation();
                object nullableAttributeArgument = methodImplementation.GetExplicitInterfaceMethodNullableAttributeArgument(_metadataReaderCache);
                if (nullableAttributeArgument != null)
                {
                    WriteTypeName(methodImplementation.ImplementedMethod.ContainingType, noSpace: true, nullableAttributeArgument: nullableAttributeArgument);
                    WriteSymbol(".");
                    if (isIndexer)
                    {
                        WriteIdentifier("this", false);
                        WriteIndexerParameters(accessor, isSetterAccessor);
                    }
                    else
                    {
                        string name = methodImplementation.ImplementedMethod.Name.Value;
                        WriteIdentifier(name.Substring(name.IndexOf("_") + 1));
                    }

                    return;
                }
            }

            if (isIndexer)
            {
                int index = property.Name.Value.LastIndexOf(".");
                if (index >= 0)
                    WriteIdentifier(property.Name.Value.Substring(0, index + 1) + "this", false); // +1 to include the '.'
                else
                    WriteIdentifier("this", false);

                WriteIndexerParameters(accessor, isSetterAccessor);
            }
            else
            {
                WriteIdentifier(property.Name);
            }
        }

        private void WriteIndexerParameters(IMethodDefinition accessor, bool isSetterAccessor)
        {
            var parameters = new List<IParameterDefinition>(accessor.Parameters);
            if (isSetterAccessor) // If setter remove value parameter.
                parameters.RemoveAt(parameters.Count - 1);

            byte? nullableContextValue = accessor.Attributes.GetCustomAttributeArgumentValue<byte?>(CSharpCciExtensions.NullableContextAttributeFullName);
            WriteParameters(parameters, accessor.ContainingType, nullableContextValue, property: true);
        }

        private void WriteAccessorDefinition(IPropertyDefinition property, IMethodDefinition accessor, string accessorType, bool isReadOnly)
        {
            WriteSpace();
            WriteAttributes(accessor.Attributes, writeInline: true);
            WriteAttributes(accessor.SecurityAttributes, writeInline: true);
            // If the accessor is an internal call (or a PInvoke) we should put those attributes here as well

            WriteMethodPseudoCustomAttributes(accessor);

            if (accessor.Visibility != property.Visibility)
                WriteVisibility(accessor.Visibility);
            if (isReadOnly && (LangVersion >= LangVersion8_0))
            {
                WriteKeyword("readonly");
            }
            WriteKeyword(accessorType, noSpace: true);
            WriteMethodBody(accessor);
        }
    }
}
