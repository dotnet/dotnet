// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.VisualStudio
{
    public interface IConsoleStatus
    {
        /// <summary>
        /// Returns whether the console is busy executing a command.
        /// </summary>
        bool IsBusy { get; }
    }
}
