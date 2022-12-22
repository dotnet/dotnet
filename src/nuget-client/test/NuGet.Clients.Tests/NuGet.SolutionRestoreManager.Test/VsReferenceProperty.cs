// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace NuGet.SolutionRestoreManager.Test
{
    internal class VsReferenceProperty : IVsReferenceProperty
    {
        public string Name { get; }

        public string Value { get; }

        public VsReferenceProperty(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Argument cannot be null or empty", nameof(name));
            }

            Name = name;
            Value = value;
        }
    }
}
