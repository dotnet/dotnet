// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Microsoft.Cci.Comparers;
using Microsoft.Cci.Extensions;
using Microsoft.Cci.Filters;

namespace Microsoft.Cci.Mappings
{
    public class AttributesMapping<T> : ElementMapping<T> where T : class
    {
        private Dictionary<string, ElementMapping<AttributeGroup>> _attributes;

        public AttributesMapping(MappingSettings settings)
            : base(settings)
        {
        }

        public IEnumerable<ElementMapping<AttributeGroup>> Attributes
        {
            get
            {
                if (_attributes == null)
                {
                    _attributes = new Dictionary<string, ElementMapping<AttributeGroup>>();

                    for (int i = 0; i < this.ElementCount; i++)
                        if (this[i] != null)
                            AddMapping(i, GetAttributes(this[i]));
                }

                return _attributes.Values;
            }
        }

        private void AddMapping(int index, IEnumerable<ICustomAttribute> attributes)
        {
            // Use the constructor as the key to minimize the amount of collisions, so there should only be collisions
            var attrGroups = attributes.GroupBy(c => c.Constructor.DocId());

            foreach (var attrGroup in attrGroups)
            {
                ElementMapping<AttributeGroup> mapping;

                if (!_attributes.TryGetValue(attrGroup.Key, out mapping))
                {
                    mapping = new ElementMapping<AttributeGroup>(this.Settings);
                    _attributes.Add(attrGroup.Key, mapping);
                }
                else
                {
                    Contract.Assert(index != 0);
                }
                mapping.AddMapping(index, new AttributeGroup(attrGroup, this.Settings.AttributeComparer));
            }
        }

        protected virtual IEnumerable<ICustomAttribute> GetAttributes(T element)
        {
            IReference reference = element as IReference;
            if (reference != null)
                return reference.Attributes.Where(Filter.Include);

            IEnumerable<ICustomAttribute> attributes = element as IEnumerable<ICustomAttribute>;
            if (attributes != null)
                return attributes.Where(Filter.Include);

            return null;
        }
    }

    public class AttributeGroup : IEquatable<AttributeGroup>
    {
        private readonly IEqualityComparer<ICustomAttribute> _comparer;

        public AttributeGroup(IEnumerable<ICustomAttribute> attributes, IEqualityComparer<ICustomAttribute> comparer)
        {
            Contract.Requires(attributes != null);
            Contract.Requires(comparer != null);
            this.Attributes = attributes;
            _comparer = comparer;
        }

        public IEnumerable<ICustomAttribute> Attributes { get; private set; }

        public bool Equals(AttributeGroup that)
        {
            // For this comparison we want to use the full decl string for the attribute not just the docid of the constructor
            return this.Attributes.SequenceEqual(that.Attributes, _comparer);
        }
    }
}
