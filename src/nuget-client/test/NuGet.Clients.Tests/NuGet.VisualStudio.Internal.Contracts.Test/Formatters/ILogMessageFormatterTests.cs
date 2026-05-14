// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using NuGet.Common;
using NuGet.Packaging.Signing;
using Xunit;

namespace NuGet.VisualStudio.Internal.Contracts.Test
{
    public sealed class ILogMessageFormatterTests : FormatterTests
    {
        private const string Message = "a";

        [Theory]
        [MemberData(nameof(ILogMessages))]
        public void SerializeThenDeserialize_WithValidArguments_RoundTrips(ILogMessage expectedResult)
        {
            ILogMessage? actualResult = SerializeThenDeserialize(ILogMessageFormatter.Instance, expectedResult);

            Assert.NotNull(actualResult);
            TestUtility.AssertEqual(expectedResult, actualResult!);
        }

        public static TheoryData<ILogMessage> ILogMessages => new()
            {
                { new LogMessage(LogLevel.Error, Message, NuGetLogCode.NU3000) },
                {
                    new LogMessage(LogLevel.Warning, Message, NuGetLogCode.NU3027)
                    {
                        ProjectPath = "b",
                        WarningLevel = WarningLevel.Important
                    }
                },
                { PackagingLogMessage.CreateError(Message, NuGetLogCode.NU1103) },
                { PackagingLogMessage.CreateMessage(Message, LogLevel.Verbose) },
                { PackagingLogMessage.CreateWarning(Message, NuGetLogCode.NU1103) },
                { CreatePackagingLogMessage() },
                { SignatureLog.DebugLog(Message) },
                { SignatureLog.DetailedLog(Message) },
                { SignatureLog.Error(NuGetLogCode.NU3014, Message) },
                { SignatureLog.InformationLog(Message) },
                { SignatureLog.Issue(fatal: true, NuGetLogCode.NU3030, Message) },
                { CreateSignatureLog() },
                {
                    new RestoreLogMessage(LogLevel.Error, errorString: "a")
                    {
                        Code = NuGetLogCode.NU1605,
                        EndColumnNumber = 1,
                        EndLineNumber = 2,
                        FilePath = "b",
                        Level = LogLevel.Minimal,
                        LibraryId = "c",
                        ProjectPath = "d",
                        ShouldDisplay = true,
                        StartColumnNumber = 3,
                        StartLineNumber = 4,
                        TargetGraphs = new string[] { "e", "f" },
                        Time = DateTimeOffset.UtcNow,
                        WarningLevel = WarningLevel.Important
                    }
                }
            };

        private static PackagingLogMessage CreatePackagingLogMessage()
        {
            PackagingLogMessage packagingLogMessage = PackagingLogMessage.CreateError(Message, NuGetLogCode.NU3031);

            packagingLogMessage.Code = NuGetLogCode.NU1103;
            packagingLogMessage.EndColumnNumber = 1;
            packagingLogMessage.EndLineNumber = 2;
            packagingLogMessage.FilePath = "b";
            packagingLogMessage.Level = LogLevel.Minimal;
            packagingLogMessage.ProjectPath = "c";
            packagingLogMessage.StartColumnNumber = 3;
            packagingLogMessage.StartLineNumber = 4;
            packagingLogMessage.Time = DateTimeOffset.UtcNow;
            packagingLogMessage.WarningLevel = WarningLevel.Minimal;

            return packagingLogMessage;
        }

        private static SignatureLog CreateSignatureLog()
        {
            SignatureLog signatureLog = SignatureLog.Error(NuGetLogCode.NU3031, Message);

            signatureLog.Code = NuGetLogCode.NU3017;
            signatureLog.Level = LogLevel.Verbose;
            signatureLog.LibraryId = "b";
            signatureLog.ProjectPath = "c";
            signatureLog.Time = DateTimeOffset.UtcNow;
            signatureLog.WarningLevel = WarningLevel.Important;

            return signatureLog;
        }
    }
}
