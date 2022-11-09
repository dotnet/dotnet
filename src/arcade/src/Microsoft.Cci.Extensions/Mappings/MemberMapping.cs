// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;

namespace Microsoft.Cci.Mappings
{
    public class MemberMapping : AttributesMapping<ITypeDefinitionMember>
    {
        public MemberMapping(TypeMapping containingType, MappingSettings settings)
            : base(settings)
        {
            this.ContainingType = containingType;
        }

        public TypeMapping ContainingType { get; private set; }
    }
}
