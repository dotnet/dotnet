// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NuGet.Protocol.Core.Types
{
    public abstract class ResourceProvider : INuGetResourceProvider
    {
        private readonly Type _resourceType;
        private readonly string _name;
        private readonly IEnumerable<string> _after;
        private readonly IEnumerable<string> _before;

        public ResourceProvider(Type resourceType)
            : this(resourceType, string.Empty, null)
        {
        }

        public ResourceProvider(Type resourceType, string name)
            : this(resourceType, name, null)
        {
        }

        public ResourceProvider(Type resourceType, string name, string? before)
            : this(resourceType, name, ToArray(before), Enumerable.Empty<string>())
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="resourceType">Type this resource provider creates</param>
        /// <param name="name">name used for ordering</param>
        /// <param name="before">providers that this provider should have precendence over</param>
        /// <param name="after">providers that this provider should be called after</param>
        public ResourceProvider(Type resourceType, string name, IEnumerable<string> before, IEnumerable<string> after)
        {
            if (resourceType == null)
            {
                throw new ArgumentNullException(nameof(resourceType));
            }

            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (before == null)
            {
                throw new ArgumentNullException(nameof(before));
            }

            if (after == null)
            {
                throw new ArgumentNullException(nameof(after));
            }

            _resourceType = resourceType;
            _name = name;
            _before = before;
            _after = after;
        }

        public virtual IEnumerable<string> After
        {
            get { return _after; }
        }

        public virtual IEnumerable<string> Before
        {
            get { return _before; }
        }

        public virtual string Name
        {
            get { return _name; }
        }

        public virtual Type ResourceType
        {
            get { return _resourceType; }
        }

        /// <inheritdoc cref="INuGetResourceProvider.TryCreate(SourceRepository, CancellationToken)"/>
        public abstract Task<Tuple<bool, INuGetResource?>> TryCreate(SourceRepository source, CancellationToken token);

        private static IEnumerable<string> ToArray(string? s)
        {
            if (!string.IsNullOrEmpty(s))
            {
                return new string[] { s! };
            }

            return Enumerable.Empty<string>();
        }
    }
}
