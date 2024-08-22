// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;

namespace NuGet.Commands
{
    public class ResolverConflict
    {
        public string Name { get; }
        public IList<ResolverRequest> Requests { get; }

        public ResolverConflict(string name, IEnumerable<ResolverRequest> requests)
        {
            Name = name;
            Requests = requests.ToList();
        }
    }
}
