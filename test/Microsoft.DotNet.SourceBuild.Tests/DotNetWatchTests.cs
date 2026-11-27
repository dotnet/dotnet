// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics;
using System.IO;
using Xunit;

namespace Microsoft.DotNet.SourceBuild.Tests;

public class DotNetWatchTests : SdkTests
{
    public DotNetWatchTests(ITestOutputHelper outputHelper) : base(outputHelper) { }

    [Fact]
    public void WatchTests()
    {
        string projectDirectory = DotNetHelper.ExecuteNew(DotNetTemplate.Console.GetName(), nameof(DotNetWatchTests));
        bool outputChanged = false;
        bool fileChanged = false;

        DotNetHelper.ExecuteCmd(
            "watch run --non-interactive --verbose",
            workingDirectory: projectDirectory,
            expectedExitCode: null, // The exit code does not reflect whether or not dotnet watch is working properly
            outputLineCallback: outputLineCallback);

        Assert.True(outputChanged);

        void outputLineCallback(Process process, ProcessOutputLine line)
        {
            const string waitingString = "Waiting for a file to change";
            const string expectedString = "Hello from dotnet watch!";

            OutputHelper.WriteLine(line.Content);

            if (line.StandardError)
            {
                if (line.Content.Contains(waitingString) && !fileChanged)
                {
                    OutputHelper.WriteLine("Program started, changing file on disk to trigger restart...");
                    File.WriteAllText(
                        Path.Combine(projectDirectory, "Program.cs"),
                        File.ReadAllText(Path.Combine(projectDirectory, "Program.cs")).Replace("Hello, World!", expectedString));
                    fileChanged = true;
                }
            }
            else if (line.Content.Contains(expectedString))
            {
                outputChanged = true;
                OutputHelper.WriteLine("Successfully re-ran program after code change.");
                process.Kill(true);
            }
        }
    }
}
