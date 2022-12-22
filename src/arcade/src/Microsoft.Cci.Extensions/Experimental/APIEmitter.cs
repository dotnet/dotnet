// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Cci;
using Microsoft.Cci.Extensions;
using System.IO;

namespace Microsoft.Cci.Extensions.Experimental
{
#pragma warning disable 612,618
    internal class APIEmitter : BaseMetadataTraverser
#pragma warning restore 612,618
    {
        private TextWriter _writer;
        private int _indentLevel;

        public void EmitAssembly(IAssembly assembly)
        {
            _writer = Console.Out;

            Visit(assembly);
        }

        public override void Visit(IAssembly assembly)
        {
            Visit(assembly.NamespaceRoot);
        }

        public override void Visit(INamespaceDefinition @namespace)
        {
            IEnumerable<INamespaceDefinition> namespaces = @namespace.Members.OfType<INamespaceDefinition>();
            IEnumerable<INamespaceTypeDefinition> types = @namespace.Members.OfType<INamespaceTypeDefinition>();

            if (types.Count() > 0)
            {
                EmitKeyword("namespace");
                Emit(TypeHelper.GetNamespaceName((IUnitNamespaceReference)@namespace, NameFormattingOptions.None));
                EmitNewLine();
                using (EmitBlock(true))
                {
                    foreach (var type in types)
                        Visit(type);
                }
                EmitNewLine();
            }

            foreach (var nestedNamespace in namespaces)
                Visit(nestedNamespace);
        }

        public override void Visit(INamespaceTypeDefinition type)
        {
            EmitType(type, type.Name.Value);
        }

        public override void Visit(INestedTypeDefinition nestedType)
        {
            EmitType(nestedType, nestedType.Name.Value);
        }

        public virtual void EmitType(ITypeDefinition type, string typeName)
        {
            EmitVisibility(type.GetVisibility());
            EmitKeyword("class");
            Emit(typeName);
            EmitNewLine();
            using (EmitBlock(true))
            {
                foreach (var member in type.Members)
                    Visit(member);
            }
            EmitNewLine();

            foreach (var nestedType in type.NestedTypes)
                Visit(nestedType);
        }

        public override void Visit(IFieldDefinition field)
        {
            Emit(MemberHelper.GetMemberSignature(field, NameFormattingOptions.Signature));
            EmitNewLine();
        }

        public override void Visit(IMethodDefinition method)
        {
            Emit(MemberHelper.GetMemberSignature(method, NameFormattingOptions.Signature));
            EmitNewLine();
        }

        public override void Visit(IPropertyDefinition property)
        {
            Emit(MemberHelper.GetMemberSignature(property, NameFormattingOptions.Signature));
            EmitNewLine();
        }

        public override void Visit(IEventDefinition @event)
        {
            Emit(MemberHelper.GetMemberSignature(@event, NameFormattingOptions.Signature));
            EmitNewLine();
        }

        public virtual IDisposable EmitBlock(bool ident)
        {
            return new CodeBlock(this, ident);
        }

        public virtual void EmitBlockStart(bool ident)
        {
            Emit("{");
            if (ident)
            {
                _indentLevel++;
                EmitNewLine();
            }
        }

        public virtual void EmitBlockEnd(bool ident)
        {
            if (ident)
            {
                _indentLevel--;
                EmitNewLine();
            }
            Emit("}");
        }

        public virtual void EmitVisibility(TypeMemberVisibility visibility)
        {
            switch (visibility)
            {
                case TypeMemberVisibility.Public:
                    EmitKeyword("public");
                    break;

                case TypeMemberVisibility.Private:
                    EmitKeyword("private");
                    break;

                case TypeMemberVisibility.Assembly:
                    EmitKeyword("internal");
                    break;

                case TypeMemberVisibility.Family:
                    EmitKeyword("protected");
                    break;

                case TypeMemberVisibility.FamilyOrAssembly:
                    EmitKeyword("protected internal");
                    break;

                case TypeMemberVisibility.FamilyAndAssembly:
                    EmitKeyword("private protected");
                    break;

                default:
                    EmitKeyword("<Unknown-Visibility>");
                    break;
            }
        }

        public virtual void Emit(string s)
        {
            _writer.Write(s);
        }

        public virtual void EmitIndent()
        {
            _writer.Write(new string(' ', _indentLevel * 2));
        }

        public virtual void EmitNewLine()
        {
            _writer.WriteLine();
            EmitIndent();
        }

        public virtual void EmitKeyword(string keyword)
        {
            Emit(keyword);
            Emit(" ");
        }

        internal class CodeBlock : IDisposable
        {
            private APIEmitter _apiEmitter;
            private bool _ident;
            public CodeBlock(APIEmitter apiEmitter, bool ident)
            {
                _apiEmitter = apiEmitter;
                _ident = ident;

                _apiEmitter.EmitBlockStart(_ident);
            }

            public void Dispose()
            {
                if (_apiEmitter != null)
                {
                    _apiEmitter.EmitBlockEnd(_ident);
                    _apiEmitter = null;
                }
            }
        }


        public object List { get; set; }
    }
}
