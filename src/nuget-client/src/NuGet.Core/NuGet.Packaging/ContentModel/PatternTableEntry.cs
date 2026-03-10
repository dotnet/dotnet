// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace NuGet.ContentModel
{
    public class PatternTableEntry
    {
        /// <summary>
        /// PropertyName moniker
        /// </summary>
        public string PropertyName { get; }

        /// <summary>
        /// Item name
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Item replacement value
        /// </summary>
        public object Value { get; }

        public PatternTableEntry(string propertyName, string name, object value)
        {
            if (propertyName == null)
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            PropertyName = propertyName;
            Name = name;
            Value = value;
        }
    }
}
