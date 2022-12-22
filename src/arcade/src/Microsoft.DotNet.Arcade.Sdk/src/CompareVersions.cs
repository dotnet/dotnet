// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using NuGet.Versioning;

namespace Microsoft.DotNet.Arcade.Sdk
{
    public class CompareVersions : Microsoft.Build.Utilities.Task
    {
        [Required]
        public string Left { get; set; }

        [Required]
        public string Right { get; set; }

        [Output]
        public int Result { get; set; }

        public override bool Execute()
        {
            ExecuteImpl();
            return !Log.HasLoggedErrors;
        }

        private void ExecuteImpl()
        {
            if (!SemanticVersion.TryParse(Left, out var left))
            {
                Log.LogError($"Invalid version: '{Left}'");
                return;
            }

            if (!SemanticVersion.TryParse(Right, out var right))
            {
                Log.LogError($"Invalid version: '{Right}'");
                return;
            }

            Result = left.CompareTo(right);
        }
    }
}
