// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Microsoft.DotNet.SwaggerGenerator.Modeler
{
    public class ClassTypeModel : TypeModel
    {
        public ClassTypeModel(
            string name,
            string @namespace,
            IEnumerable<PropertyModel> properties,
            TypeReference additionalProperties)
        {
            Name = name;
            Namespace = @namespace;
            AdditionalProperties = additionalProperties;
            Properties = properties.ToImmutableList();
        }

        public override string Name { get; }
        public override string Namespace { get; }
        public override bool IsEnum => false;
        public TypeReference AdditionalProperties { get; }
        public IImmutableList<PropertyModel> Properties { get; }

        public IEnumerable<PropertyModel> RequiredAndReadOnlyProperties =>
            Properties.Where(p => p.Required && p.ReadOnly)
                .Concat(Properties.Where(p => p.Required && !p.ReadOnly))
                .Concat(Properties.Where(p => !p.Required && p.ReadOnly));

        public IEnumerable<PropertyModel> RequiredProperties => Properties.Where(p => p.Required);
    }
}
