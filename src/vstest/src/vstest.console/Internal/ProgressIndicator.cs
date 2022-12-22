// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Globalization;

using Microsoft.VisualStudio.TestPlatform.Utilities;

using Timer = System.Timers.Timer;

namespace Microsoft.VisualStudio.TestPlatform.CommandLine.Internal;

/// <summary>
/// Indicates the test run progress
/// </summary>
internal sealed class ProgressIndicator : IProgressIndicator, IDisposable
{
    private readonly object _syncObject = new();
    private int _dotCounter;
    private Timer? _timer;
    private readonly string _testRunProgressString;

    /// <summary>
    /// Used to output to the console
    /// </summary>
    public IOutput ConsoleOutput { get; private set; }

    /// <summary>
    /// Wrapper over the System Console class
    /// </summary>
    public IConsoleHelper ConsoleHelper { get; private set; }

    /// <summary>
    /// True is the indicator is to be displayed
    /// </summary>
    public bool IsRunning { get; private set; }

    public ProgressIndicator(IOutput output, IConsoleHelper consoleHelper)
    {
        ConsoleOutput = output;
        ConsoleHelper = consoleHelper;
        _testRunProgressString = string.Format(CultureInfo.CurrentCulture, "{0}...", Resources.Resources.ProgressIndicatorString);
    }

    /// <inheritdoc />
    public void Start()
    {
        lock (_syncObject)
        {
            if (_timer == null)
            {
                _timer = new Timer(1000);
                _timer.Elapsed += Timer_Elapsed;
                _timer.Start();
            }

            // Print the string based on the previous state, that is dotCounter
            // This is required for smooth transition
            ConsoleOutput.Write(_testRunProgressString.Substring(0, _testRunProgressString.Length + _dotCounter - 2), OutputLevel.Information);
            IsRunning = true;
        }
    }

    /// <summary>
    // Get the current cursor position
    // Clear the console starting given position
    // Reset the cursor position back
    /// </summary>
    /// <param name="startPos">the starting position</param>
    private void Clear(int startPos)
    {
        var currentLineCursor = ConsoleHelper.CursorTop;
        ConsoleHelper.SetCursorPosition(startPos, ConsoleHelper.CursorTop);
        ConsoleOutput.Write(new string(' ', ConsoleHelper.WindowWidth - startPos), OutputLevel.Information);
        ConsoleHelper.SetCursorPosition(startPos, currentLineCursor);
    }

    /// <summary>
    /// Sets the isRunning flag to false so that indicator is not shown.
    /// </summary>
    public void Pause()
    {
        lock (_syncObject)
        {
            IsRunning = false;
            Clear(0);
        }
    }

    /// <summary>
    /// Stops the indicator and clears the current line.
    /// </summary>
    public void Stop()
    {
        lock (_syncObject)
        {
            IsRunning = false;
            _timer?.Stop();
            Clear(0);
        }
    }

    private void Timer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
    {
        if (IsRunning)
        {
            // If running, prints dot every second.
            ConsoleOutput.Write(".", OutputLevel.Information);
            _dotCounter = ++_dotCounter % 3;

            // When counter reaches 3, that is 3 dots have been printed
            // Clear and start printing again
            if (_dotCounter == 0)
            {
                Clear(ConsoleHelper.CursorLeft - 3);
            }
        }
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}
