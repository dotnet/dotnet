// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;

namespace NuGet.VisualStudio
{
    public interface IOutputConsoleProvider
    {
        /// <summary>
        /// Creates a console enabling writing to the VS build output window pane.
        /// </summary>
        /// <returns>Output console instance.</returns>
        Task<IOutputConsole> CreateBuildOutputConsoleAsync();

        /// <summary>
        /// Creates a console instance associated with the Package Manager output window.
        /// </summary>
        /// <returns>Output console instance.</returns>
        Task<IOutputConsole> CreatePackageManagerConsoleAsync();

        /// <summary>
        /// Creates host-enabled console instance.
        /// </summary>
        /// <returns>Console instance.</returns>
        Task<IConsole> CreatePowerShellConsoleAsync();
    }
}
