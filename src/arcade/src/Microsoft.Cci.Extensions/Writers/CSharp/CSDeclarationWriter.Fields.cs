// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections;
using System.Collections.Generic;
using Microsoft.Cci.Extensions.CSharp;

namespace Microsoft.Cci.Writers.CSharp
{
    public partial class CSDeclarationWriter
    {
        private void WriteFieldDefinition(IFieldDefinition field)
        {
            if (field.IsSpecialName)
                return;

            WriteAttributes(field.Attributes);
            if (!field.IsStatic && field.ContainingTypeDefinition.Layout == LayoutKind.Explicit && !(field is DummyPrivateField))
            {
                WriteFakeAttribute("System.Runtime.InteropServices.FieldOffsetAttribute", field.Offset.ToString());
            }

            if (field.IsNotSerialized)
            {
                WriteFakeAttribute("System.NonSerializedAttribute");
            }

            if (!field.ContainingTypeDefinition.IsEnum)
            {
                WriteVisibility(field.Visibility);
                WriteCustomModifiers(field.CustomModifiers);

                if (field.Attributes.HasRequiredMemberAttribute())
                {
                    WriteKeyword("required");
                }

                if (field.Type.IsUnsafeType())
                    WriteKeyword("unsafe");

                if (field.IsCompileTimeConstant)
                {
                    if (field.GetHiddenBaseField(_filter) != Dummy.Field)
                        WriteKeyword("new");

                    WriteKeyword("const");
                }
                else
                {
                    if (field.IsStatic)
                        WriteKeyword("static");
                    if (field.IsReadOnly)
                        WriteKeyword("readonly");
                }

                if (!field.IsCompileTimeConstant && field.GetHiddenBaseField(_filter) != Dummy.Field)
                    WriteKeyword("new");

                // If it is a dummy field we call the override that ignores reference type nullability
                WriteTypeName(field.Type, field.Attributes, includeReferenceTypeNullability: !(field is DummyPrivateField));

                string name = field.Name.Value;
                if (name.Contains("<") || name.Contains(">"))
                {
                    name = name.Replace("<", "_").Replace(">", "_");
                }
                WriteIdentifier(name, true);

                if (field.Constant != null && field.IsCompileTimeConstant)
                {
                    WriteSpace();
                    WriteSymbol("=", true);
                    if (field.Type.IsEnum)
                    {
                        WriteEnumValue(field.Constant, field.Type);
                    }
                    else
                    {
                        WriteMetadataConstant(field.Constant);
                    }
                }

                WriteSymbol(";");
            }
            else
            {
                WriteIdentifier(field.Name);
                if (field.Constant != null && field.Constant.Value != null)
                {
                    WriteSpace();
                    WriteSymbol("=", true);
                    WriteMetadataConstant(field.Constant);
                }
                WriteSymbol(",");
            }
        }
    }

    public class DummyPrivateField : IFieldDefinition
    {
        private ITypeDefinition _parentType;
        private ITypeReference _type;
        private IName _name;
        private IEnumerable<ICustomAttribute> _attributes = System.Linq.Enumerable.Empty<ICustomAttribute>();
        private bool _isReadOnly;

        public DummyPrivateField(ITypeDefinition parentType, ITypeReference type, string name)
        {
            _parentType = parentType;
            _type = type;
            _name = new NameTable().GetNameFor(name);
        }

        public DummyPrivateField(ITypeDefinition parentType, ITypeReference type, string name, IEnumerable<ICustomAttribute> attributes, bool isReadOnly) : this(parentType, type, name)
        {
            _attributes = attributes;
            _isReadOnly = isReadOnly;
        }

        public uint BitLength => 0;

        public IMetadataConstant CompileTimeValue => null;

        public ISectionBlock FieldMapping => null;

        public bool IsBitField => false;

        public bool IsCompileTimeConstant => false;

        public bool IsMapped { get { throw new System.NotImplementedException(); } }

        public bool IsMarshalledExplicitly { get { throw new System.NotImplementedException(); } }

        public bool IsNotSerialized => false;

        public bool IsReadOnly => _isReadOnly || _parentType.Attributes.HasIsReadOnlyAttribute();

        public bool IsRuntimeSpecial => false;

        public bool IsSpecialName => false;

        public IMarshallingInformation MarshallingInformation { get { throw new System.NotImplementedException(); } }

        public uint Offset => 0;

        public int SequenceNumber { get { throw new System.NotImplementedException(); } }

        public ITypeDefinition ContainingTypeDefinition => _parentType;

        public TypeMemberVisibility Visibility => TypeMemberVisibility.Private;

        public ITypeDefinition Container { get { throw new System.NotImplementedException(); } }

        public IName Name => _name;

        public IScope<ITypeDefinitionMember> ContainingScope { get { throw new System.NotImplementedException(); } }

        public IEnumerable<ICustomModifier> CustomModifiers => System.Linq.Enumerable.Empty<ICustomModifier>();

        public uint InternedKey { get { throw new System.NotImplementedException(); } }

        public bool IsModified { get { throw new System.NotImplementedException(); } }

        public bool IsStatic => false;

        public ITypeReference Type => _type;

        public IFieldDefinition ResolvedField { get { throw new System.NotImplementedException(); } }

        public ITypeReference ContainingType => _parentType;

        public ITypeDefinitionMember ResolvedTypeDefinitionMember { get { throw new System.NotImplementedException(); } }

        public IEnumerable<ICustomAttribute> Attributes => _attributes;

        public IEnumerable<ILocation> Locations { get { throw new System.NotImplementedException(); } }

        public IMetadataConstant Constant => null;

        public void Dispatch(IMetadataVisitor visitor)
        {
            throw new System.NotImplementedException();
        }

        public void DispatchAsReference(IMetadataVisitor visitor)
        {
            throw new System.NotImplementedException();
        }
    }
}
