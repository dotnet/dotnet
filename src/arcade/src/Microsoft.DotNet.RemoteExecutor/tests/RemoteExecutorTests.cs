// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Sdk;

namespace Microsoft.DotNet.RemoteExecutor.Tests
{
    public class RemoteExecutorTests
    {
        [Fact]
        public void Action()
        {
            RemoteInvokeHandle h = RemoteExecutor.Invoke(() => { }, new RemoteInvokeOptions { RollForward = "Major" });
            using (h)
            {
                Assert.Equal(RemoteExecutor.SuccessExitCode, h.ExitCode);
            }
        }

        [Fact]
        public void AsyncAction_ThrowException()
        {
            Assert.Throws<RemoteExecutionException>(() =>
                RemoteExecutor.Invoke(async () =>
                {
                    Assert.True(false);
                    await Task.Delay(1);
                }, new RemoteInvokeOptions { RollForward = "Major" }).Dispose()
            );
        }

        [Fact]
        public void AsyncAction()
        {
            RemoteInvokeHandle h = RemoteExecutor.Invoke(async () =>
            {
                await Task.Delay(1);
            }, new RemoteInvokeOptions { RollForward = "Major" });
            using (h)
            {
                Assert.Equal(RemoteExecutor.SuccessExitCode, h.ExitCode);
            }
        }

        [Fact]
        public void AsyncFunc_ThrowException()
        {
            Assert.Throws<RemoteExecutionException>(() =>
                RemoteExecutor.Invoke(async () =>
                {
                    Assert.True(false);
                    await Task.Delay(1);
                    return 1;
                }, new RemoteInvokeOptions { RollForward = "Major" }).Dispose()
            );
        }

        [Fact]
        public void AsyncFuncFiveArgs_ThrowException()
        {
            Assert.Throws<RemoteExecutionException>(() =>
                RemoteExecutor.Invoke(async (a, b, c, d, e) =>
                {
                    Assert.True(false);
                    await Task.Delay(1);
                }, "a", "b", "c", "d", "e", new RemoteInvokeOptions { RollForward = "Major" }).Dispose()
            );
        }

        [Fact]
        public void AsyncFunc_InvalidReturnCode()
        {
            Assert.ThrowsAny<RemoteExecutionException>(() =>
                RemoteExecutor.Invoke(async () =>
                {
                    await Task.Delay(1);
                    return 1;
                }, new RemoteInvokeOptions { RollForward = "Major" }).Dispose()
            );
        }

        [Fact]
        public void AsyncFunc_NoThrow_ValidReturnCode()
        {
            RemoteExecutor.Invoke(async () =>
            {
                await Task.Delay(1);
                return RemoteExecutor.SuccessExitCode;
            }, new RemoteInvokeOptions { RollForward = "Major" }).Dispose();
        }

        [Fact]
        public static void AsyncAction_FatalError_AV()
        {
            // Invocation should report as failing on AV
            Assert.ThrowsAny<RemoteExecutionException>(() =>
                RemoteExecutor.Invoke(async () =>
                {
                    await Task.Delay(1);
                    unsafe
                    {
                        *(int*)0x10000 = 0;
                    }
                }, new RemoteInvokeOptions { RollForward = "Major" }).Dispose()
            );
        }

        [Fact]
        public static void AsyncAction_FatalError_Runtime()
        {
            // Invocation should report as failing on fatal runtime error
            Assert.ThrowsAny<RemoteExecutionException>(() =>
                RemoteExecutor.Invoke(async () =>
                {
                    await Task.Delay(1);
                    System.Runtime.InteropServices.Marshal.StructureToPtr(1, new IntPtr(1), true);
                }, new RemoteInvokeOptions { RollForward = "Major" }).Dispose()
            );
        }

        [Fact]
        public static unsafe void FatalError_AV()
        {
            // Invocation should report as failing on AV
            Assert.ThrowsAny<RemoteExecutionException>(() =>
                RemoteExecutor.Invoke(() =>
                {
                    *(int*)0x10000 = 0;
                }, new RemoteInvokeOptions { RollForward = "Major" }).Dispose()
            );
        }

        [Fact]
        public static void FatalError_Runtime()
        {
            // Invocation should report as failing on fatal runtime error
            Assert.ThrowsAny<RemoteExecutionException>(() =>
                RemoteExecutor.Invoke(() =>
                {
                    System.Runtime.InteropServices.Marshal.StructureToPtr(1, new IntPtr(1), true);
                }, new RemoteInvokeOptions { RollForward = "Major" }).Dispose()
            );
        }

        [Fact]
        public static void IgnoreExitCode()
        {
            int exitCode = 1;
            RemoteInvokeHandle h = RemoteExecutor.Invoke(
                s => int.Parse(s),
                exitCode.ToString(),
                new RemoteInvokeOptions { RollForward = "Major", CheckExitCode = false, ExpectedExitCode = 0 });
            using(h)
            {
                Assert.Equal(exitCode, h.ExitCode);
            }
        }

        [Theory]
        [InlineData(CrashDumpCollectionType.Mini, "1")]
        [InlineData(CrashDumpCollectionType.Heap, "2")]
        [InlineData(CrashDumpCollectionType.Triage, "3")]
        [InlineData(CrashDumpCollectionType.Full, "4")]
        public void CrashDumpCollection_SetsEnvVars(CrashDumpCollectionType dumpType, string expectedTypeValue)
        {
            RemoteExecutor.Invoke(expectedType =>
            {
                Assert.Equal("1", Environment.GetEnvironmentVariable("DOTNET_DbgEnableMiniDump"));
                Assert.Equal(expectedType, Environment.GetEnvironmentVariable("DOTNET_DbgMiniDumpType"));
                return RemoteExecutor.SuccessExitCode;
            }, expectedTypeValue, new RemoteInvokeOptions
            {
                RollForward = "Major",
                CrashDumpCollectionType = dumpType
            }).Dispose();
        }

        [Fact]
        public void DisableCrashDumpCollection_RemovesEnvVars()
        {
            // Pre-set the env vars on the StartInfo to simulate inherited values
            var options = new RemoteInvokeOptions
            {
                RollForward = "Major",
                CrashDumpCollectionType = CrashDumpCollectionType.None
            };
            options.StartInfo.Environment["DOTNET_DbgEnableMiniDump"] = "1";
            options.StartInfo.Environment["DOTNET_DbgMiniDumpType"] = "4";
            options.StartInfo.Environment["DOTNET_DbgMiniDumpName"] = "/tmp/test.dmp";

            RemoteExecutor.Invoke(() =>
            {
                Assert.Null(Environment.GetEnvironmentVariable("DOTNET_DbgEnableMiniDump"));
                Assert.Null(Environment.GetEnvironmentVariable("DOTNET_DbgMiniDumpType"));
                Assert.Null(Environment.GetEnvironmentVariable("DOTNET_DbgMiniDumpName"));
            }, options).Dispose();
        }

        [Fact]
        public void CrashDumpCollection_DefaultLeavesEnvVarsUntouched()
        {
            // When CrashDumpCollectionType is left unset, the env vars must pass through to the child unchanged.
            const string expectedEnable = "1";
            const string expectedType = "4";
            const string expectedName = "/tmp/passthrough.dmp";

            var options = new RemoteInvokeOptions { RollForward = "Major" };
            options.StartInfo.Environment["DOTNET_DbgEnableMiniDump"] = expectedEnable;
            options.StartInfo.Environment["DOTNET_DbgMiniDumpType"] = expectedType;
            options.StartInfo.Environment["DOTNET_DbgMiniDumpName"] = expectedName;

            RemoteExecutor.Invoke((enable, type, name) =>
            {
                Assert.Equal(enable, Environment.GetEnvironmentVariable("DOTNET_DbgEnableMiniDump"));
                Assert.Equal(type, Environment.GetEnvironmentVariable("DOTNET_DbgMiniDumpType"));
                Assert.Equal(name, Environment.GetEnvironmentVariable("DOTNET_DbgMiniDumpName"));
            }, expectedEnable, expectedType, expectedName, options).Dispose();
        }

        [Fact]
        public static unsafe void CrashDumpCollection_CreatesDumpOnCrash()
        {
            string dumpDir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(dumpDir);
            try
            {
                var options = new RemoteInvokeOptions
                {
                    RollForward = "Major",
                    CrashDumpCollectionType = CrashDumpCollectionType.Mini,
                    CheckExitCode = false,
                    // Point the dump path to our temp directory so we can verify the file is created.
                    // Use %p so the filename includes the PID and is unique.
                    CrashDumpPath = Path.Combine(dumpDir, "crashdump.%p.dmp")
                };

                using (RemoteInvokeHandle handle = RemoteExecutor.Invoke(() =>
                {
                    // Trigger an access violation to crash the process
                    *(int*)0x10000 = 0;
                }, options))
                {
                    // The process is expected to crash, so it should not return the success exit code.
                    Assert.NotEqual(RemoteExecutor.SuccessExitCode, handle.ExitCode);
                }

                // Poll for the dump file with a short sleep between checks rather than busy-spinning.
                bool dumpCreated = false;
                DateTime deadline = DateTime.UtcNow + TimeSpan.FromSeconds(10);
                while (DateTime.UtcNow < deadline)
                {
                    if (Directory.EnumerateFiles(dumpDir, "*.dmp").Any())
                    {
                        dumpCreated = true;
                        break;
                    }

                    Thread.Sleep(100);
                }

                Assert.True(dumpCreated, "Expected a crash dump file to be created within 10 seconds.");
            }
            finally
            {
                try { Directory.Delete(dumpDir, recursive: true); }
                catch { }
            }
        }
    }
}
