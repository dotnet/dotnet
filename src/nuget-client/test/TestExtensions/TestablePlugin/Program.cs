// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NuGet.Test.TestExtensions.TestablePlugin
{
    internal static class Program
    {
        private const int Success = 0;
        private const int Error = 1;

        private static readonly FileNotFoundException Exception = new FileNotFoundException("This exception is being thrown to simulate an exception in a plugin.", "DependencyFileName");

        private static int Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            DebugBreakIfPluginDebuggingIsEnabled();

            Arguments parsedArgs;

            if (!Arguments.TryParse(args, out parsedArgs))
            {
                return Error;
            }

            if (parsedArgs.Freeze)
            {
                while (true)
                {
                    Thread.Sleep(millisecondsTimeout: 50);
                }
            }

            if (parsedArgs.ThrowException == ThrowException.Unhandled)
            {
                throw Exception;
            }

            try
            {
                if (parsedArgs.ThrowException == ThrowException.Handled)
                {
                    throw Exception;
                }

                Start(parsedArgs);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.ToString());

                return Error;
            }

            return Success;
        }

        private static void Start(Arguments arguments)
        {
            using (var cancellationTokenSource = new CancellationTokenSource())
            using (var responses = new BlockingCollection<Response>())
            {
                Process process;

                if (!TryGetProcess(arguments.TestRunnerProcessId, out process))
                {
                    return;
                }

                using (process)
                {
                    process.Exited += (sender, args) =>
                    {
                        try
                        {
                            cancellationTokenSource.Cancel();
                        }
                        catch (Exception)
                        {
                        }
                    };

                    try
                    {
                        process.EnableRaisingEvents = true;
                    }
                    catch (Win32Exception)
                    {
                        // Can occur while debugging.
                    }

                    var responseReceiver = new ResponseReceiver(arguments.PortNumber, responses);

                    using (var testablePlugin = new TestablePlugin(responses))
                    {
                        var tasks = new[]
                        {
                            Task.Factory.StartNew(
                                () => responseReceiver.StartListeningAsync(cancellationTokenSource.Token),
                                TaskCreationOptions.LongRunning | TaskCreationOptions.DenyChildAttach),
                            testablePlugin.StartAsync(arguments.CauseProtocolException, cancellationTokenSource.Token)
                        };

                        Task.WaitAny(tasks);

                        try
                        {
                            cancellationTokenSource.Cancel();
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
            }
        }

        private static void DebugBreakIfPluginDebuggingIsEnabled()
        {
            if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("NUGET_PLUGIN_DEBUG")))
            {
                Debugger.Break();
            }
        }

        private static bool TryGetProcess(int processId, out Process process)
        {
            try
            {
                process = Process.GetProcessById(processId);

                return true;
            }
            catch (Exception)
            {
            }

            process = null;

            return false;
        }
    }
}
