// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Microsoft.DotNet.Arcade.Sdk
{
    public sealed class SingleError : Microsoft.Build.Utilities.Task
    {
        private static readonly string s_cacheKeyPrefix = "SingleError-F88E25C6-1488-4E81-A458-A0921794E6E3:";

        [Required]
        public string Text { get; set; }

        public override bool Execute()
        {
            var key = s_cacheKeyPrefix + Text;

            var errorReportedSentinel = BuildEngine4.GetRegisteredTaskObject(key, RegisteredTaskObjectLifetime.Build);
            if (errorReportedSentinel != null)
            {
                Log.LogMessage(MessageImportance.Low, Text);
                return false;
            }

            BuildEngine4.RegisterTaskObject(key, new object(), RegisteredTaskObjectLifetime.Build, allowEarlyCollection: true);
            Log.LogError(Text);
            return false;
        }
    }
}
