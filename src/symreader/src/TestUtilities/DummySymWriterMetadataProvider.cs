// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the License.txt file in the project root for more information.

using System;
using System.Reflection;

namespace Microsoft.DiaSymReader.Tools
{
    public sealed class DummySymWriterMetadataProvider : ISymWriterMetadataProvider
    {
        public static readonly ISymWriterMetadataProvider Instance = new DummySymWriterMetadataProvider();

        public bool TryGetEnclosingType(int nestedTypeToken, out int enclosingTypeToken) 
            => throw new NotImplementedException();

        public bool TryGetMethodInfo(int methodDefinitionToken, out string methodName, out int declaringTypeToken)
            => throw new NotImplementedException();

        public bool TryGetTypeDefinitionInfo(int typeDefinitionToken, out string namespaceName, out string typeName, out TypeAttributes attributes)
            => throw new NotImplementedException();
    }
}
