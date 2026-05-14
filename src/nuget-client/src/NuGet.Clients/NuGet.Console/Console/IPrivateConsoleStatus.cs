// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using NuGet.VisualStudio;

namespace NuGetConsole.Implementation.Console
{
    internal interface IPrivateConsoleStatus : IConsoleStatus
    {
        void SetBusyState(bool isBusy);
    }
}
