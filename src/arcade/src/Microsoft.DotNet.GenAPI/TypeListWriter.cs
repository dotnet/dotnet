// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Cci;
using Microsoft.Cci.Writers;
using System.IO;
using Microsoft.Cci.Traversers;
using Microsoft.Cci.Writers.Syntax;
using Microsoft.Cci.Filters;
using Microsoft.Cci.Writers.CSharp;

namespace Microsoft.DotNet.GenAPI
{
    internal class TypeListWriter : SimpleTypeMemberTraverser, ICciWriter
    {
        private readonly ISyntaxWriter _syntaxWriter;
        private readonly ICciDeclarationWriter _declarationWriter;

        public TypeListWriter(ISyntaxWriter writer, ICciFilter filter)
            : base(filter)
        {
            _syntaxWriter = writer;
            _declarationWriter = new CSDeclarationWriter(_syntaxWriter, filter, false);
        }

        public void WriteAssemblies(IEnumerable<IAssembly> assemblies)
        {
            foreach (IAssembly assembly in assemblies)
                Visit(assembly);
        }

        public override void Visit(IAssembly assembly)
        {
            _syntaxWriter.Write("assembly " + assembly.Name.Value);

            using (_syntaxWriter.StartBraceBlock())
            {
                base.Visit(assembly);
            }
        }

        public override void Visit(INamespaceDefinition ns)
        {
            _declarationWriter.WriteDeclaration(ns);

            using (_syntaxWriter.StartBraceBlock())
            {
                base.Visit(ns);
            }
        }

        public override void Visit(ITypeDefinition type)
        {
            _declarationWriter.WriteDeclaration(type);
            _syntaxWriter.WriteLine();
        }
    }
}
