// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace NuGet.SolutionRestoreManager.Test
{
    internal class VsReferenceItem2 : IVsReferenceItem2
    {
        public string Name { get; }

        public IReadOnlyDictionary<string, string> Metadata { get; }

        public VsReferenceItem2(string name, IReadOnlyDictionary<string, string> metadata)
        {
            Name = !string.IsNullOrEmpty(name) ? name : throw new ArgumentException("Argument cannot be null or empty", nameof(name));
            Metadata = metadata ?? throw new ArgumentNullException(nameof(metadata));
        }
    }
}
