// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using System.Linq;

namespace Microsoft.TestPlatform.AttachVS;

internal class Program
{
    static void Main(string[] args)
    {
        _ = args ?? throw new ArgumentNullException(nameof(args));

        Trace.Listeners.Add(new ConsoleTraceListener());

        int? pid = ParsePid(args, position: 0);
        int? vsPid = ParsePid(args, position: 1);

        var exitCode = DebuggerUtility.AttachVSToProcess(pid, vsPid) ? 0 : 1;
        Environment.Exit(exitCode);
    }

    private static int? ParsePid(string[] args, int position)
    {
        var id = args.Skip(position).Take(1).SingleOrDefault();
        int? pid = id == null
            ? null
            : int.TryParse(id, out var i)
                ? i
                : null;
        return pid;
    }
}
