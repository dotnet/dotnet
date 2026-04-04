// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using CommandLine;
using OpenTelemetry.Exporter.Geneva.Transports;
using OpenTelemetry.Trace;

namespace OpenTelemetry.Exporter.Geneva.Stress;

internal class Program
{
    private static readonly ActivitySource Source = new("OpenTelemetry.Exporter.Geneva.Stress");
    private static volatile bool s_bContinue = true;
    private static long s_nEvents;

    private static int Main(string[] args)
    {
        return Parser.Default.ParseArguments<WindowsOptions, LinuxOptions, ServerOptions, ExporterCreationOptions>(args)
            .MapResult(
                (WindowsOptions options) => EntryPoint(InitTraces, RunTraces),
                (LinuxOptions options) => RunLinux(options),
                (ServerOptions options) => RunServer(options),
                (ExporterCreationOptions options) => RunExporterCreation(),
                errs => 1);

        // return EntryPoint(InitMetrics, RunMetrics);
    }

    [Verb("Windows", HelpText = "Run stress test on Windows.")]
    private class WindowsOptions
    {
    }

    [Verb("Linux", HelpText = "Run stress test on Linux.")]
    private class LinuxOptions
    {
        [Option('p', "path", Default = "/var/run/default_fluent.socket", HelpText = "Specify a path for Unix domain socket.")]
        public string? Path { get; set; }
    }

    [Verb("server", HelpText = "Start a dummy server on Linux.")]
    private class ServerOptions
    {
        [Option('p', "path", HelpText = "Specify a path for Unix domain socket.", Required = true)]
        public string? Path { get; set; }
    }

    [Verb("ExporterCreation", HelpText = "Validate exporter dispose behavior")]
    private class ExporterCreationOptions
    {
    }

    private static int RunExporterCreation()
    {
        for (var i = 0; i < 300000; ++i)
        {
            using var dataTransport = new EtwDataTransport("OpenTelemetry");
        }

        return 0;
    }

    private static int RunLinux(LinuxOptions options)
    {
        return EntryPoint(() => InitTracesOnLinux(options.Path!), RunTraces);
    }

    private static int RunServer(ServerOptions options)
    {
        var server = new DummyServer(options.Path!);
        server.Start();
        return 0;
    }

    private static int EntryPoint(Action init, Action run)
    {
        init();

        var statistics = new long[Environment.ProcessorCount];
        Parallel.Invoke(
            () =>
            {
                Console.WriteLine("Running, press <Esc> to stop...");
                var watch = new Stopwatch();
                while (true)
                {
                    if (Console.KeyAvailable)
                    {
                        var key = Console.ReadKey(true).Key;
#pragma warning disable IDE0010 // Add missing cases
                        switch (key)
                        {
                            case ConsoleKey.Escape:
                                s_bContinue = false;
                                return;
                        }
#pragma warning restore IDE0010 // Add missing cases

                        continue;
                    }

                    s_nEvents = statistics.Sum();
                    watch.Restart();
                    Thread.Sleep(200);
                    watch.Stop();
                    var nEvents = statistics.Sum();
                    var nEventPerSecond = (int)((nEvents - s_nEvents) / (watch.ElapsedMilliseconds / 1000.0));
                    Console.Title = string.Format(CultureInfo.InvariantCulture, "Loops: {0:n0}, Loops/Second: {1:n0}", nEvents, nEventPerSecond);
                }
            },
            () =>
            {
                Parallel.For(0, statistics.Length, (i) =>
                {
                    statistics[i] = 0;
                    while (s_bContinue)
                    {
                        run();
                        statistics[i]++;
                    }
                });
            });
        return 0;
    }

    private static void InitTraces()
    {
        Sdk.CreateTracerProviderBuilder()
            .SetSampler(new AlwaysOnSampler())
            .AddSource("OpenTelemetry.Exporter.Geneva.Stress")
            .AddGenevaTraceExporter(options =>
            {
                options.ConnectionString = "EtwSession=OpenTelemetry";
                options.PrepopulatedFields = new Dictionary<string, object>
                {
                    ["cloud.role"] = "BusyWorker",
                    ["cloud.roleInstance"] = "CY1SCH030021417",
                    ["cloud.roleVer"] = "9.0.15289.2",
                };
            })
            .Build();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void RunTraces()
    {
        using var activity = Source.StartActivity("Stress");
        activity?.SetTag("http.method", "GET");
        activity?.SetTag("http.url", "https://www.wikipedia.org/wiki/Rabbit");
        activity?.SetTag("http.status_code", 200);
    }

    private static void InitTracesOnLinux(string path)
    {
        Sdk.CreateTracerProviderBuilder()
            .SetSampler(new AlwaysOnSampler())
            .AddSource("OpenTelemetry.Exporter.Geneva.Stress")
            .AddGenevaTraceExporter(options =>
            {
                options.ConnectionString = "Endpoint=unix:" + path;
                options.PrepopulatedFields = new Dictionary<string, object>
                {
                    ["cloud.role"] = "BusyWorker",
                    ["cloud.roleInstance"] = "CY1SCH030021417",
                    ["cloud.roleVer"] = "9.0.15289.2",
                };
            })
            .Build();
    }
}
