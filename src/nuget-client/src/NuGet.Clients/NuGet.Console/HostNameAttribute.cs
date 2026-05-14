// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.ComponentModel.Composition;

namespace NuGetConsole
{
    /// <summary>
    /// Specifies a MEF host name metadata to uniquely identify a host type. This is
    /// required for a host provider to be recognized by PowerConsole. PowerConsole
    /// also uses the HostName to find the associated ICommandTokenizerProvider and
    /// ICommandExpansionProvider for a host.
    /// To avoid host name collision, a host can use its full class name (or
    /// a guid).
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    [MetadataAttribute]
    public sealed class HostNameAttribute : Attribute
    {
        /// <summary>
        /// The unique name for a host.
        /// </summary>
        public string HostName { get; private set; }

        /// <summary>
        /// Specifies a unique MEF host name metadata.
        /// </summary>
        public HostNameAttribute(string hostName)
        {
            if (hostName == null)
            {
                throw new ArgumentNullException(nameof(hostName));
            }
            this.HostName = hostName;
        }
    }
}
