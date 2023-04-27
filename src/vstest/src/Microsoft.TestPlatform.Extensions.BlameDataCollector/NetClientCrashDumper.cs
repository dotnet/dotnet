// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;

using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers.Interfaces;

namespace Microsoft.TestPlatform.Extensions.BlameDataCollector;

internal class NetClientCrashDumper : ICrashDumper
{
    private string? _outputDirectory;
    private readonly IFileHelper _fileHelper;

    public NetClientCrashDumper(IFileHelper fileHelper)
    {
        _fileHelper = fileHelper;
    }

    public void AttachToTargetProcess(int processId, string outputDirectory, DumpTypeOption dumpType, bool collectAlways, Action<string> logWarning)
    {
        // we don't need to do anything directly here, we setup the env variables
        // in the dumper configuration, including the path
        _outputDirectory = outputDirectory;
    }

    public void DetachFromTargetProcess(int processId)
    {
        // here we might consider renaming the files to have timestamp
    }

    public IEnumerable<string> GetDumpFiles(bool _)
    {
        return _fileHelper.DirectoryExists(_outputDirectory)
            ? _fileHelper.GetFiles(_outputDirectory, "*_crashdump*.dmp", SearchOption.AllDirectories)
            : Array.Empty<string>();
    }

    public void WaitForDumpToFinish()
    {
    }
}
