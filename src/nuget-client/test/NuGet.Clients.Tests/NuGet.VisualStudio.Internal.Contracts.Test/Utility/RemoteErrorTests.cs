// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using NuGet.Common;
using Xunit;

namespace NuGet.VisualStudio.Internal.Contracts.Test
{
    public class RemoteErrorTests
    {
        private const string ActivityLogMessage = "a";
        private static readonly LogMessage LogMessage = new LogMessage(LogLevel.Error, message: "b");
        private static readonly IReadOnlyList<LogMessage> LogMessages = new[]
            {
                new LogMessage(LogLevel.Warning, message: "c")
            };
        private const string ProjectContextLogMessage = "d";
        private const string TypeName = "e";

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Constructor_WhenTypeNameIsNullOrEmpty_Throws(string typeName)
        {
            ArgumentException exception = Assert.Throws<ArgumentException>(
                () => new RemoteError(
                    typeName,
                    LogMessage,
                    LogMessages,
                    ProjectContextLogMessage,
                    ActivityLogMessage));

            Assert.StartsWith("The argument cannot be null or empty.", exception.Message);
            Assert.Equal("typeName", exception.ParamName);
        }

        [Fact]
        public void Constructor_WhenLogMessageIsNull_Throws()
        {
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(
                () => new RemoteError(
                    TypeName,
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
                    logMessage: null,
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
                    LogMessages,
                    ProjectContextLogMessage,
                    ActivityLogMessage));

            Assert.Equal("logMessage", exception.ParamName);
        }

        [Fact]
        public void Constructor_WhenArgumentsAreValid_InitializesProperties()
        {
            var error = new RemoteError(
                TypeName,
                LogMessage,
                LogMessages,
                ProjectContextLogMessage,
                ActivityLogMessage);

            Assert.Equal(ActivityLogMessage, error.ActivityLogMessage);
            TestUtility.AssertEqual(LogMessage, error.LogMessage);
            TestUtility.AssertEqual(LogMessages, error.LogMessages);
            Assert.Equal(ProjectContextLogMessage, error.ProjectContextLogMessage);
            Assert.Equal(TypeName, error.TypeName);
        }

        [Fact]
        public void Constructor_WhenNullableArgumentsAreNull_InitializesProperties()
        {
            var error = new RemoteError(
                TypeName,
                LogMessage,
                logMessages: null,
                projectContextLogMessage: null,
                activityLogMessage: null);

            Assert.Null(error.ActivityLogMessage);
            TestUtility.AssertEqual(LogMessage, error.LogMessage);
            Assert.Null(error.LogMessages);
            Assert.Null(error.ProjectContextLogMessage);
            Assert.Equal(TypeName, error.TypeName);
        }
    }
}
