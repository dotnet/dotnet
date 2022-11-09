// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Linq;
using Microsoft.Cci.Extensions.CSharp;

namespace Microsoft.Cci.Writers.CSharp
{
    public partial class CSDeclarationWriter
    {
        private void WriteEventDefinition(IEventDefinition evnt)
        {
            // Adder and Remover modifiers should be same.
            IMethodDefinition accessor = evnt.Accessors.First().ResolvedMethod;

            if (!evnt.ContainingTypeDefinition.IsInterface)
            {
                WriteAttributes(evnt.Attributes);
                if (!accessor.IsExplicitInterfaceMethod())
                    WriteVisibility(evnt.Visibility);
                WriteMethodModifiers(accessor);
            }

            if (evnt.GetHiddenBaseEvent(_filter) != Dummy.Event)
                WriteKeyword("new");

            if (accessor.Attributes.HasIsReadOnlyAttribute() && (LangVersion >= LangVersion8_0))
            {
                WriteKeyword("readonly");
            }

            WriteKeyword("event");
            WriteTypeName(evnt.Type, evnt.Attributes);
            WriteIdentifier(evnt.Name);

            if (_forCompilation && !evnt.IsAbstract())
            {
                WriteSpace();
                WriteSymbol("{", addSpace: true);
                WriteEventBody("add");
                WriteEventBody("remove");
                WriteSymbol("}");
            }
            else
            {
                WriteSymbol(";");
            }
        }

        private void WriteEventBody(string keyword)
        {
            WriteKeyword(keyword);
            WriteSymbol("{", addSpace: true);
            WriteSymbol("}", addSpace: true);
        }
    }
}
