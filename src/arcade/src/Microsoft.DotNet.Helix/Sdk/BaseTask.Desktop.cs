// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Arcade.Common.Desktop;

namespace Microsoft.DotNet.Helix
{
    public partial class BaseTask
    {
        static BaseTask()
        {
            AssemblyResolver.Enable();
        }
    }
}
