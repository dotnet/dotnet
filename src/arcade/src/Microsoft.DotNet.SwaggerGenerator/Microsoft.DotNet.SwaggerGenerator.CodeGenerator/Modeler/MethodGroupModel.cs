// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Collections.Immutable;

namespace Microsoft.DotNet.SwaggerGenerator.Modeler
{
    public class MethodGroupModel
    {
        public MethodGroupModel(string name, string ns, IList<MethodModel> methods)
        {
            Name = name;
            Namespace = ns;
            Methods = methods.ToImmutableList();
        }

        public string Namespace { get; }
        public string Name { get; }
        public IImmutableList<MethodModel> Methods { get; }
    }
}
