// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualStudio.TestPlatform.CommandLine.Processors;

/// <summary>
/// Defines interface for interacting with a command line argument executor.
/// Items exporting this interface will be used in processing command line arguments.
/// </summary>
internal interface IArgumentExecutor
{
    /// <summary>
    /// Initializes the Argument Processor with the argument that was provided with the command.
    /// </summary>
    /// <param name="argument">Argument that was provided with the command.</param>
    void Initialize(string? argument);

    /// <summary>
    /// Perform the action associated with the argument processor.
    /// </summary>
    /// <returns>
    /// The <see cref="ArgumentProcessorResult"/>.
    /// </returns>
    ArgumentProcessorResult Execute();
}
