// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#include "NetCoreCheckExe.h"

#define PARAM_NAME(s) (_wcsicmp(*argv, TEXT(s)) == 0)
#define PRINT_HELP_AND_RETURN fprintf(stderr, "%s", g_help); return EXIT_FAILURE_INVALIDARGS;

// Globals
Logger *g_log;
const char* g_help = "NETCoreCheck [options]\n"
"-n, --runtimename -       Runtime name                    (Example: Microsoft.AspNetCore.App)\n"
"-v, --runtimeversion -    Runtime version in format x.y.z (Example: 1.2.3)\n"
"-r, --rollforwardpolicy - (Optional) Roll forward policy  (Example: LatestMajor)\n"
"-c, --runtimeconfigfile - Path to runtime config file     (Example: c:\\Foo\\Bar.runtimeconfig.json)\n"
"-l, --logfile -           (Optional) Path to log file\n\n"
"If runtimeconfigfile is specified then runtimename, runtimeversion and rollforwardpolicy shouldn't be.\n\n"
"If 0 is returned the runtime requirement is satisfied.\n\n"
"Examples:\n\n"
"NETCorecheck --runtimename Microsoft.AspNetCore.App --runtimeversion 3.1.0\n"
"NETCorecheck -n Microsoft.WindowsDesktop.App -v 5.0.1 -r LatestMajor\n"
"NETCorecheck -c c:\\Foo\\Bar.runtimeconfig.json -l c:\\Foo\\Bar.log\n";

int __cdecl wmain(int argc, WCHAR* argv[])
{
    LPCWSTR runtimeName = NULL;
    LPCWSTR runtimeversion = NULL;
    LPCWSTR rollForwardPolicy = NULL;
    LPCWSTR existingRuntimeConfigFilPath = NULL;
    LPCWSTR logFilePath = NULL;

    // Parse the command line options
    argv++;
    for (int i = 1; i < argc; i += 2)
    {
        if (*argv != nullptr)
        {
            if (*(argv + 1) == nullptr)
            {
                // Parameter name passed in without a value
                PRINT_HELP_AND_RETURN
            }

            if (PARAM_NAME("-n") || PARAM_NAME("--runtimename"))
            {
                runtimeName = *++argv;
            }
            else if (PARAM_NAME("-v") || PARAM_NAME("--runtimeversion"))
            {
                runtimeversion = *++argv;
            }
            else if (PARAM_NAME("-r") || PARAM_NAME("--rollforwardpolicy"))
            {
                rollForwardPolicy = *++argv;
            }
            else if (PARAM_NAME("-c") || PARAM_NAME("--runtimeconfigfile"))
            {
                existingRuntimeConfigFilPath = *++argv;
            }
            else if (PARAM_NAME("-l") || PARAM_NAME("--logfile"))
            {
                logFilePath = *++argv;
            }
            else
            {
                // Invalid parameter name
                PRINT_HELP_AND_RETURN
            }

            argv++;
        }
    }

    // Check for invalid parameter combinations
    if ((existingRuntimeConfigFilPath  &&  (runtimeName || runtimeversion || rollForwardPolicy)) ||
        (!existingRuntimeConfigFilPath && !(runtimeName && runtimeversion)))
    {
        PRINT_HELP_AND_RETURN
    }

    FileLogger logger;
    logger.Initialize(logFilePath);
    g_log = &logger;

    return CheckRuntime(runtimeName, runtimeversion, rollForwardPolicy, existingRuntimeConfigFilPath, false);
}
