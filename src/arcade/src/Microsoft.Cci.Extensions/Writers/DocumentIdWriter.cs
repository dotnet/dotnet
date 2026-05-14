// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Cci.Filters;
using Microsoft.Cci.Extensions;
using Microsoft.Cci.Traversers;

namespace Microsoft.Cci.Writers
{
    public class DocumentIdWriter : SimpleTypeMemberTraverser, ICciWriter
    {
        private readonly TextWriter _writer;
        private readonly DocIdKinds _kinds;

        public DocumentIdWriter(TextWriter writer, ICciFilter filter, DocIdKinds kinds)
            : base(filter)
        {
            _writer = writer;
            _kinds = kinds;
        }

        public void WriteAssemblies(IEnumerable<IAssembly> assemblies)
        {
            if (_kinds != 0)
            {
                assemblies = assemblies.OrderBy(a => a.Name.Value, StringComparer.OrdinalIgnoreCase);
                foreach (IAssembly assembly in assemblies)
                    Visit(assembly);
            }
        }

        public override void Visit(IAssembly assembly)
        {
            if ((_kinds & DocIdKinds.Assembly) != 0)
                _writer.WriteLine(assembly.DocId());

            base.Visit(assembly);
        }

        public override void Visit(INamespaceDefinition ns)
        {
            if ((_kinds & DocIdKinds.Namespace) != 0)
                _writer.WriteLine(ns.DocId());

            base.Visit(ns);
        }

        public override void Visit(ITypeDefinition type)
        {
            if ((_kinds & DocIdKinds.Type) != 0)
                _writer.WriteLine(type.DocId());

            base.Visit(type);
        }

        public override void Visit(ITypeDefinitionMember member)
        {
            if ((_kinds & GetMemberKind(member)) != 0)
                _writer.WriteLine(member.DocId());

            base.Visit(member);
        }

        private DocIdKinds GetMemberKind(ITypeDefinitionMember member)
        {
            if (member is IMethodDefinition)
            {
                return DocIdKinds.Method;
            }

            if (member is IPropertyDefinition)
            {
                return DocIdKinds.Property;
            }

            if (member is IEventDefinition)
            {
                return DocIdKinds.Event;
            }

            if (member is IFieldDefinition)
            {
                return DocIdKinds.Field;
            }

            throw new ArgumentException($"Unknown member type {member}", "member");
        }
    }

    [Flags]
    public enum DocIdKinds
    {
        All = A | N | T | F | P | M | E,
        Assembly = 1 << 1,
        A = Assembly,
        Namespace = 1 << 2,
        N = Namespace,
        Type = 1 << 3,
        T = Type,
        Field = 1 << 4,
        F = Field,
        Property = 1 << 5,
        P = Property,
        Method = 1 << 6,
        M = Method,
        Event = 1 << 7,
        E = Event,
    }
}
