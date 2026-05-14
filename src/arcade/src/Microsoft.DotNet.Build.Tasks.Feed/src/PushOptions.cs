// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;

namespace Microsoft.DotNet.Build.Tasks.Feed
{
    public class PushOptions
    {
        public bool AllowOverwrite { get; set; }

        public bool PassIfExistingItemIdentical { get; set; }
    }
}
