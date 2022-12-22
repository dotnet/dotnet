// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;

namespace Microsoft.TestPlatform.Extensions.BlameDataCollector;

internal class SigtrapDumper : IHangDumper
{
    public void Dump(int processId, string outputDirectory, DumpTypeOption type)
    {
        Process.Start("kill", $"-s SIGTRAP {processId}");
    }
}
