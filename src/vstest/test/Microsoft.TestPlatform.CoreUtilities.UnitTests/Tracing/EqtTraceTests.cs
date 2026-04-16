// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
#if NETFRAMEWORK
using System.Diagnostics;
#endif
using System.IO;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestPlatform.CoreUtilities.UnitTests;

[TestClass]
public class EqtTraceTests
{
    private static string? s_dirPath;
    private static string? s_logFile;

    [ClassInitialize]
    public static void Init(TestContext _)
    {
        // Set DoNotInitailize to false.
        EqtTrace.DoNotInitailize = false;
        s_dirPath = Path.Combine(Path.GetTempPath(), "TraceUT");
        try
        {
            Directory.CreateDirectory(s_dirPath);
            s_logFile = Path.Combine(s_dirPath, "trace.log");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        EqtTrace.InitializeTrace(s_logFile, PlatformTraceLevel.Off);
    }

    [TestMethod]
    public void CheckInitializeLogFileTest()
    {
        Assert.AreEqual(s_logFile, EqtTrace.LogFile, $"Expected log file to be {s_logFile}");
    }

    [TestMethod]
    public void CheckIfTraceStateIsVerboseEnabled()
    {
#if NETFRAMEWORK
        EqtTrace.TraceLevel = TraceLevel.Verbose;
#else
        EqtTrace.TraceLevel = PlatformTraceLevel.Verbose;
#endif
        Assert.IsTrue(EqtTrace.IsVerboseEnabled, $"Expected trace state to be verbose actual state {EqtTrace.IsVerboseEnabled}");
    }

    [TestMethod]
    public void CheckIfTraceStateIsErrorEnabled()
    {
#if NETFRAMEWORK
        EqtTrace.TraceLevel = TraceLevel.Error;
#else
        EqtTrace.TraceLevel = PlatformTraceLevel.Error;
#endif
        Assert.IsTrue(EqtTrace.IsErrorEnabled, $"Expected trace state to be error actual state {EqtTrace.IsErrorEnabled}");
    }

    [TestMethod]
    public void CheckIfTraceStateIsInfoEnabled()
    {
#if NETFRAMEWORK
        EqtTrace.TraceLevel = TraceLevel.Info;
#else
        EqtTrace.TraceLevel = PlatformTraceLevel.Info;
#endif
        Assert.IsTrue(EqtTrace.IsInfoEnabled, $"Expected trace state to be info actual state {EqtTrace.IsInfoEnabled}");
    }

    [TestMethod]
    public void CheckIfTraceStateIsWarningEnabled()
    {
#if NETFRAMEWORK
        EqtTrace.TraceLevel = TraceLevel.Warning;
#else
        EqtTrace.TraceLevel = PlatformTraceLevel.Warning;
#endif
        Assert.IsTrue(EqtTrace.IsWarningEnabled, $"Expected trace state to be warning actual state {EqtTrace.IsWarningEnabled}");
    }

    [TestMethod]
    public void TraceShouldWriteError()
    {
#if NETFRAMEWORK
        EqtTrace.TraceLevel = TraceLevel.Error;
#else
        EqtTrace.TraceLevel = PlatformTraceLevel.Error;
#endif
        EqtTrace.Error(new NotImplementedException());
        Assert.IsNotNull(ReadLogFile(), "Expected error message");
    }

    [TestMethod]
    public void TraceShouldWriteWarning()
    {
#if NETFRAMEWORK
        EqtTrace.TraceLevel = TraceLevel.Warning;
#else
        EqtTrace.TraceLevel = PlatformTraceLevel.Warning;
#endif
        EqtTrace.Warning("Dummy Warning Message");
        Assert.Contains("Dummy Warning Message", ReadLogFile(), "Expected Warning message");
    }

    [TestMethod]
    public void TraceShouldWriteVerbose()
    {
#if NETFRAMEWORK
        EqtTrace.TraceLevel = TraceLevel.Verbose;
#else
        EqtTrace.TraceLevel = PlatformTraceLevel.Verbose;
#endif
        EqtTrace.Verbose("Dummy Verbose Message");
        Assert.Contains("Dummy Verbose Message", ReadLogFile(), "Expected Verbose message");
    }

    [TestMethod]
    public void TraceShouldWriteInfo()
    {
#if NETFRAMEWORK
        EqtTrace.TraceLevel = TraceLevel.Info;
#else
        EqtTrace.TraceLevel = PlatformTraceLevel.Info;
#endif
        EqtTrace.Info("Dummy Info Message");
        Assert.Contains("Dummy Info Message", ReadLogFile(), "Expected Info message");
    }

    [TestMethod]
    public void TraceShouldNotWriteVerboseIfTraceLevelIsInfo()
    {
#if NETFRAMEWORK
        EqtTrace.TraceLevel = TraceLevel.Info;
#else
        EqtTrace.TraceLevel = PlatformTraceLevel.Info;
#endif
        EqtTrace.Info("Dummy Info Message");
        EqtTrace.Verbose("Unexpected Dummy Verbose Message");

        var logFileContent = ReadLogFile();
        Assert.DoesNotContain("Unexpected Dummy Verbose Message", logFileContent, "Verbose message not expected");
        Assert.Contains("Dummy Info Message", logFileContent, "Expected Info message");
    }

    [TestMethod]
    public void TraceShouldNotWriteIfDoNotInitializationIsSetToTrue()
    {
        EqtTrace.DoNotInitailize = true;
#if NETFRAMEWORK
        EqtTrace.TraceLevel = TraceLevel.Info;
#else
        EqtTrace.TraceLevel = PlatformTraceLevel.Info;
#endif
        EqtTrace.Info("Dummy Info Message: TraceShouldNotWriteIfDoNotInitializationIsSetToTrue");
        Assert.DoesNotContain("Dummy Info Message: TraceShouldNotWriteIfDoNotInitializationIsSetToTrue", ReadLogFile(), "Did not expect Dummy Info message");
    }

    private static string ReadLogFile()
    {
        string? log = null;
        try
        {
            using var fs = new FileStream(s_logFile!, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var sr = new StreamReader(fs);
            log = sr.ReadToEnd();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        Assert.IsNotNull(log);
        return log;
    }
}
