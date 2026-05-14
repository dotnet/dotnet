// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Helpers;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers.Interfaces;

namespace Microsoft.TestPlatform.Extensions.BlameDataCollector;

#pragma warning disable CA1305

public class ProcDumpArgsBuilder : IProcDumpArgsBuilder
{
    private readonly IEnvironmentVariableHelper _environmentVariableHelper;

    public ProcDumpArgsBuilder() : this(new EnvironmentVariableHelper()) { }

    internal ProcDumpArgsBuilder(IEnvironmentVariableHelper environmentVariableHelper)
    {
        _environmentVariableHelper = environmentVariableHelper ?? throw new ArgumentNullException(nameof(environmentVariableHelper));
    }

    /// <inheritdoc />
    public string BuildTriggerBasedProcDumpArgs(int processId, string filename, IEnumerable<string> procDumpExceptionsList, bool isFullDump)
    {
        // -accepteula: Auto accept end-user license agreement
        //
        // -e: Write a dump when the process encounters an unhandled exception. Include the 1 to create dump on first chance exceptions.
        // We use -e 1 to make sure we are able to catch StackOverflow and AccessViolationException exceptions.
        // -g: Run as a native debugger in a managed process (no interop).
        // We use -g to be able to intercept  StackOverflow and AccessViolationException.
        // -t: Write a dump when the process terminates.
        // Collect the dump all the time, even if CollectAlways is not enabled to produce dumps for Environment.FailFast. We will later ignore the last
        // dump file for testhost when we know that test host did not crash.
        // -ma: Full dump argument.
        // -f: Filter the exceptions.
        // Filter the first chance exceptions only to those that are most likely to kill the whole process.

        // Fully override parameters to procdump
        var procdumpArgumentsFromEnv = _environmentVariableHelper.GetEnvironmentVariable("VSTEST_DUMP_PROCDUMPARGUMENTS")?.Trim();

        // Useful additional arguments are -n 100, to collect all dumps that you can, or -o to overwrite dump, or -f EXCEPTION_NAME to add exception to filter list
        var procdumpAdditonalArgumentsFromEnv = _environmentVariableHelper.GetEnvironmentVariable("VSTEST_DUMP_PROCDUMPADDITIONALARGUMENTS")?.Trim();
        StringBuilder procDumpArgument = new($"-accepteula -e 1 -g -t {procdumpAdditonalArgumentsFromEnv}");
        if (isFullDump)
        {
            procDumpArgument.Append("-ma ");
        }

        foreach (var exceptionFilter in procDumpExceptionsList)
        {
            procDumpArgument.Append($"-f {exceptionFilter} ");
        }

        procDumpArgument.Append($"{processId} {filename}.dmp");

        var argument = procdumpArgumentsFromEnv.IsNullOrWhiteSpace()
            ? procDumpArgument.ToString()
            : $"-accepteula {procdumpArgumentsFromEnv} {processId} {filename}.dmp";

        if (!argument.ToUpperInvariant().Contains("-accepteula".ToUpperInvariant()))
        {
            argument = $"-accepteula {argument}";
        }

        return argument;
    }

    /// <inheritdoc />
    public string BuildHangBasedProcDumpArgs(int processId, string filename, bool isFullDump)
    {
        // -accepteula: Auto accept end-user license agreement
        // -ma: Full dump argument.
        // -n: Number of dumps to capture.
        StringBuilder procDumpArgument = new("-accepteula -n 1");
        if (isFullDump)
        {
            procDumpArgument.Append(" -ma");
        }

        procDumpArgument.Append($" {processId} {filename}.dmp");
        var argument = procDumpArgument.ToString();

        return argument;
    }
}

#pragma warning restore CA1305
