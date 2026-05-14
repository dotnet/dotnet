// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.Protocol.Plugins
{
    internal enum PluginState
    {
        Started,
        Idle,
        Exited,
        Disposing,
        Disposed
    }
}
