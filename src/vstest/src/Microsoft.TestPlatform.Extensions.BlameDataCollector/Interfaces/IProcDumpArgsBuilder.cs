// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace Microsoft.TestPlatform.Extensions.BlameDataCollector;

public interface IProcDumpArgsBuilder
{
    /// <summary>
    /// Arguments for procdump.exe
    /// </summary>
    /// <param name="processId">
    /// Process Id
    /// </param>
    /// <param name="filename">
    /// Filename for dump file
    /// </param>
    /// <param name="procDumpExceptionsList">
    /// List of exceptions to look for
    /// </param>
    /// <param name="isFullDump">
    /// Is full dump enabled
    /// </param>
    /// <returns>Arguments</returns>
    string BuildTriggerBasedProcDumpArgs(int processId, string filename, IEnumerable<string> procDumpExceptionsList, bool isFullDump);

    /// <summary>
    /// Arguments for procdump.exe for getting a dump in case of a testhost hang
    /// </summary>
    /// <param name="processId">
    /// Process Id
    /// </param>
    /// <param name="filename">
    /// Filename for dump file
    /// </param>
    /// <param name="isFullDump">
    /// Is full dump enabled
    /// </param>
    /// <returns>Arguments</returns>
    string BuildHangBasedProcDumpArgs(int processId, string filename, bool isFullDump);
}
