// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;

namespace NuGet.SolutionRestoreManager.Test
{
    [DebuggerDisplay("{Name} = {Value}")]
    internal class VsProjectProperty : IVsProjectProperty
    {
        public string Name { get; }

        public string Value { get; }

        public VsProjectProperty(string name, string value)
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
