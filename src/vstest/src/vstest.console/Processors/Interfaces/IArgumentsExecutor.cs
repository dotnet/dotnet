// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualStudio.TestPlatform.CommandLine.Processors;

/// <summary>
/// Defines interface for interacting with a command line arguments executor.
/// Items exporting this interface will be used in processing command line arguments.
/// </summary>
internal interface IArgumentsExecutor : IArgumentExecutor
{
    /// <summary>
    /// Initializes the Argument Processor with the arguments that was provided with the command.
    /// </summary>
    /// <param name="arguments">Arguments that are provided with the command.</param>
    void Initialize(string[]? arguments);
}
