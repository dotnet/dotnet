// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Cci;

namespace Microsoft.Cci.Extensions
{
    public class FakeCustomAttribute : ICustomAttribute
    {
        public FakeCustomAttribute(string nspace, string typeName)
        {
            TypeName = typeName;
            Namespace = nspace;
        }

        public string TypeName { get; }
        public string Namespace { get; }
        public string FullTypeName { get { return Namespace + "." + TypeName; } }
        public string DocId { get { return "T:" + FullTypeName; } }

        IEnumerable<IMetadataExpression> ICustomAttribute.Arguments
        {
            get
            {
                return Enumerable.Empty<IMetadataExpression>();
            }
        }

        IMethodReference ICustomAttribute.Constructor
        {
            get
            {
                return null;
            }
        }

        IEnumerable<IMetadataNamedArgument> ICustomAttribute.NamedArguments
        {
            get
            {
                return Enumerable.Empty<IMetadataNamedArgument>();
            }
        }

        ushort ICustomAttribute.NumberOfNamedArguments
        {
            get
            {
                return 0;
            }
        }

        ITypeReference ICustomAttribute.Type
        {
            get
            {
                return null;
            }
        }
    }
}
