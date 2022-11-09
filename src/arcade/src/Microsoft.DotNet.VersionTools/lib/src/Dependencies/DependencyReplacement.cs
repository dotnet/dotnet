// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Arcade.Common;
using System;
using System.Collections.Generic;

namespace Microsoft.DotNet.VersionTools.Dependencies
{
    public class DependencyReplacement
    {
        public DependencyReplacement(
            string content,
            IEnumerable<IDependencyInfo> usedDependencyInfos)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            Content = content;
            UsedDependencyInfos = usedDependencyInfos.NullAsEmpty();
        }

        public string Content { get; }

        public IEnumerable<IDependencyInfo> UsedDependencyInfos { get; }
    }
}
