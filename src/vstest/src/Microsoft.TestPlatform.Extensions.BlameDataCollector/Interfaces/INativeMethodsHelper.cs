// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.TestPlatform.Extensions.BlameDataCollector;

public interface INativeMethodsHelper
{
    /// <summary>
    /// Returns if a process is 64 bit process
    /// </summary>
    /// <param name="processHandle">Process Handle</param>
    /// <returns>Bool for Is64Bit</returns>
    bool Is64Bit(nint processHandle);
}
