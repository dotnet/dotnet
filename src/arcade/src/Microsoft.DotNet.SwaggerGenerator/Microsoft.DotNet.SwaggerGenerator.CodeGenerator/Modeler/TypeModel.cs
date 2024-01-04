// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Microsoft.DotNet.SwaggerGenerator.Modeler
{
    public abstract class TypeModel
    {
        public abstract string Name { get; }
        public abstract string Namespace { get; }
        public abstract bool IsEnum { get; }

        public override string ToString()
        {
            return $"{Namespace}.{Name}";
        }
    }
}
