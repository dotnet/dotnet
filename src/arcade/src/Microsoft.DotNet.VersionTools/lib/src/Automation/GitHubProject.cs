// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;

namespace Microsoft.DotNet.VersionTools.Automation
{
    public class GitHubProject
    {
        public string Name { get; }
        public string Owner { get; }

        public GitHubProject(string name, string owner = null)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            Name = name;

            Owner = owner ?? "dotnet";
        }

        public string Segments => $"{Owner}/{Name}";
    }
}
