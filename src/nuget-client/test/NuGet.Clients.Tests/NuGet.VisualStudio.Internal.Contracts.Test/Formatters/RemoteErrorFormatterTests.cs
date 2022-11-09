// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using NuGet.Common;
using Xunit;

namespace NuGet.VisualStudio.Internal.Contracts.Test
{
    public sealed class RemoteErrorFormatterTests : FormatterTests
    {
        private static readonly LogMessage LogMessage = new LogMessage(LogLevel.Error, message: "a");
        private static readonly IReadOnlyList<LogMessage> LogMessages = new[] { new LogMessage(LogLevel.Warning, message: "b") };
        private const string TypeName = "c";

        [Theory]
        [MemberData(nameof(RemoteErrors))]
        public void SerializeThenDeserialize_WithValidArguments_RoundTrips(RemoteError expectedResult)
        {
            RemoteError? actualResult = SerializeThenDeserialize(RemoteErrorFormatter.Instance, expectedResult);

            Assert.NotNull(actualResult);
            Assert.Equal(expectedResult.ActivityLogMessage, actualResult!.ActivityLogMessage);
            TestUtility.AssertEqual(expectedResult.LogMessage, actualResult.LogMessage);
            TestUtility.AssertEqual(expectedResult.LogMessages, actualResult.LogMessages);
            Assert.Equal(expectedResult.ProjectContextLogMessage, actualResult.ProjectContextLogMessage);
            Assert.Equal(expectedResult.TypeName, actualResult.TypeName);
        }

        public static TheoryData RemoteErrors => new TheoryData<RemoteError>
            {
                {
                    new RemoteError(
                        TypeName,
                        LogMessage,
                        logMessages: null,
                        projectContextLogMessage: null,
                        activityLogMessage: null)
                },
                {
                    new RemoteError(
                        TypeName,
                        LogMessage,
                        LogMessages,
                        projectContextLogMessage: "d",
                        activityLogMessage: "e")
                },
            };
    }
}
