// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;

namespace OpenTelemetry.Tests;

internal sealed class ConsoleCommand : IDisposable
{
    private readonly Process process;
    private readonly List<string> output = [];
    private readonly List<string> errors = [];

    private ConsoleCommand(
        string command,
        string arguments,
        Action<string>? onOutputReceived,
        Action<string>? onErrorReceived)
    {
        Console.WriteLine($"{command} {arguments}");

        var process = new Process
        {
            StartInfo = new()
            {
                FileName = command,
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                RedirectStandardInput = false,
            },
        };

        process.OutputDataReceived += (sender, args) =>
        {
            if (!string.IsNullOrEmpty(args.Data))
            {
                this.output.Add(args.Data);
                Console.WriteLine($"{command} {arguments} [OUT] {args.Data}");

                onOutputReceived?.Invoke(args.Data);
            }
        };

        process.ErrorDataReceived += (sender, args) =>
        {
            if (!string.IsNullOrEmpty(args.Data))
            {
                this.errors.Add(args.Data);
                Console.WriteLine($"[ERR] {args.Data}");

                onErrorReceived?.Invoke(args.Data);
            }
        };

        process.Start();

        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        this.process = process;
    }

    public IEnumerable<string> Output => this.output;

    public IEnumerable<string> Errors => this.errors;

    public static IEnumerable<string> Run(
        string command,
        string arguments)
    {
        Run(command, arguments, out _, out var errors);

        return errors;
    }

    public static void Run(
        string command,
        string arguments,
        out IEnumerable<string> output,
        out IEnumerable<string> errors)
    {
        var consoleCommand = new ConsoleCommand(command, arguments, onOutputReceived: null, onErrorReceived: null);
        consoleCommand.Dispose();

        output = consoleCommand.Output;
        errors = consoleCommand.Errors;
    }

    public static ConsoleCommand Start(
        string command,
        string arguments,
        Action<string>? onOutputReceived = null,
        Action<string>? onErrorReceived = null)
        => new(command, arguments, onOutputReceived, onErrorReceived);

    public void Kill()
    {
        this.process.Kill();
    }

    public void Dispose()
    {
        this.process.WaitForExit();

        this.process.CancelOutputRead();
        this.process.CancelErrorRead();

        this.process.Dispose();
    }
}
