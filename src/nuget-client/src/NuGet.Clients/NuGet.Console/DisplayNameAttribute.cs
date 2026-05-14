// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.ComponentModel.Composition;

namespace NuGetConsole
{
    /// <summary>
    /// Specifies a MEF DisplayName metadata.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    [MetadataAttribute]
    public sealed class DisplayNameAttribute : Attribute
    {
        /// <summary>
        /// The display name to be shown in UI.
        /// </summary>
        public string DisplayName { get; private set; }

        /// <summary>
        /// Specifies a display name to be shown in UI.
        /// </summary>
        /// <param name="displayName">The display name.</param>
        /// <remarks>
        /// This can potentially be a series of "[Culture=]Text" separated by
        /// "\n" (not implemented yet). Default culture is "en".
        /// </remarks>
        public DisplayNameAttribute(string displayName)
        {
            if (displayName == null)
            {
                throw new ArgumentNullException(nameof(displayName));
            }
            this.DisplayName = displayName;
        }
    }
}
