// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Helpers;
using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Tracing;
#if NETFRAMEWORK
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine;
#endif
using Microsoft.VisualStudio.TestPlatform.Execution;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace Microsoft.VisualStudio.TestPlatform.TestHost;

/// <summary>
/// The program.
/// </summary>
public class Program
{
#if NETFRAMEWORK
    private const string TestSourceArgumentString = "--testsourcepath";
#endif

    /// <summary>
    /// The main.
    /// </summary>
    /// <param name="args">
    /// The args.
    /// </param>
    public static void Main(string[]? args)
    {
        try
        {
            TestPlatformEventSource.Instance.TestHostStart();
            Run(args);
        }
        catch (Exception ex)
        {
            EqtTrace.Error("TestHost: Error occurred during initialization of TestHost : {0}", ex);

            // Throw exception so that vstest.console get the exception message.
            throw;
        }
        finally
        {
            TestPlatformEventSource.Instance.TestHostStop();
            EqtTrace.Info("Testhost process exiting.");
        }
    }

    // In UWP(App models) Run will act as entry point from Application end, so making this method public
    public static void Run(string[]? args) => Run(args, new());

    internal static void Run(string[]? args, UiLanguageOverride uiLanguageOverride)
    {
        DebuggerBreakpoint.AttachVisualStudioDebugger(WellKnownDebugEnvironmentVariables.VSTEST_HOST_DEBUG_ATTACHVS);
        DebuggerBreakpoint.WaitForNativeDebugger(WellKnownDebugEnvironmentVariables.VSTEST_HOST_NATIVE_DEBUG);
        DebuggerBreakpoint.WaitForDebugger(WellKnownDebugEnvironmentVariables.VSTEST_HOST_DEBUG);
        uiLanguageOverride.SetCultureSpecifiedByUser();
        var argsDictionary = CommandLineArgumentsHelper.GetArgumentsDictionary(args);

        // Invoke the engine with arguments
        GetEngineInvoker(argsDictionary).Invoke(argsDictionary);
    }

    private static IEngineInvoker GetEngineInvoker(IDictionary<string, string?> argsDictionary)
    {
        IEngineInvoker? invoker = null;
#if NETFRAMEWORK
        // If Args contains test source argument, invoker Engine in new appdomain
        if (argsDictionary.TryGetValue(TestSourceArgumentString, out var testSourcePath) && !testSourcePath.IsNullOrWhiteSpace())
        {
            // remove the test source arg from dictionary
            argsDictionary.Remove(TestSourceArgumentString);

            // Only DLLs and EXEs can have app.configs or ".exe.config" or ".dll.config"
            if (System.IO.File.Exists(testSourcePath) &&
                (testSourcePath.EndsWith(".dll", StringComparison.OrdinalIgnoreCase)
                 || testSourcePath.EndsWith(".exe", StringComparison.OrdinalIgnoreCase)))
            {
                invoker = new AppDomainEngineInvoker<DefaultEngineInvoker>(testSourcePath);
            }
        }
#endif
        return invoker ?? new DefaultEngineInvoker();
    }
}
