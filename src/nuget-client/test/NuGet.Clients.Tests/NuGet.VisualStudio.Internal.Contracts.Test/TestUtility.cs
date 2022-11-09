// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using NuGet.Common;
using NuGet.Packaging.Signing;
using Xunit;

namespace NuGet.VisualStudio.Internal.Contracts.Test
{
    internal static class TestUtility
    {
        internal static void AssertEqual(ILogMessage? expectedResult, ILogMessage? actualResult)
        {
            if (expectedResult is null)
            {
                Assert.Null(actualResult);

                return;
            }
            else
            {
                Assert.NotNull(actualResult);
            }

            Assert.Equal(expectedResult.Code, actualResult!.Code);
            Assert.Equal(expectedResult.Level, actualResult.Level);
            Assert.Equal(expectedResult.Message, actualResult.Message);
            Assert.Equal(expectedResult.ProjectPath, actualResult.ProjectPath);
            Assert.Equal(expectedResult.Time, actualResult.Time);
            Assert.Equal(expectedResult.WarningLevel, actualResult.WarningLevel);

            if (expectedResult is PackagingLogMessage expectedPackagingResult)
            {
                Assert.IsType<PackagingLogMessage>(actualResult);

                var actualPackagingResult = (PackagingLogMessage)actualResult;

                Assert.Equal(expectedPackagingResult.EndColumnNumber, actualPackagingResult.EndColumnNumber);
                Assert.Equal(expectedPackagingResult.EndLineNumber, actualPackagingResult.EndLineNumber);
                Assert.Equal(expectedPackagingResult.FilePath, actualPackagingResult.FilePath);
                Assert.Equal(expectedPackagingResult.StartColumnNumber, actualPackagingResult.StartColumnNumber);
                Assert.Equal(expectedPackagingResult.StartLineNumber, actualPackagingResult.StartLineNumber);
            }
            else if (expectedResult is RestoreLogMessage expectedRestoreResult)
            {
                Assert.IsType<RestoreLogMessage>(actualResult);

                var actualRestoreResult = (RestoreLogMessage)actualResult;

                Assert.Equal(expectedRestoreResult.EndColumnNumber, actualRestoreResult.EndColumnNumber);
                Assert.Equal(expectedRestoreResult.EndLineNumber, actualRestoreResult.EndLineNumber);
                Assert.Equal(expectedRestoreResult.FilePath, actualRestoreResult.FilePath);
                Assert.Equal(expectedRestoreResult.LibraryId, actualRestoreResult.LibraryId);
                Assert.Equal(expectedRestoreResult.ShouldDisplay, actualRestoreResult.ShouldDisplay);
                Assert.Equal(expectedRestoreResult.StartColumnNumber, actualRestoreResult.StartColumnNumber);
                Assert.Equal(expectedRestoreResult.StartLineNumber, actualRestoreResult.StartLineNumber);
                Assert.Equal(expectedRestoreResult.TargetGraphs, actualRestoreResult.TargetGraphs);
            }
            else if (expectedResult is SignatureLog expectedSignatureResult)
            {
                Assert.IsType<SignatureLog>(actualResult);

                var actualSignatureResult = (SignatureLog)actualResult;

                Assert.Equal(expectedSignatureResult.LibraryId, actualSignatureResult.LibraryId);
            }
            else
            {
                Assert.IsType<LogMessage>(expectedResult);
                Assert.IsType<LogMessage>(actualResult);
            }
        }

        internal static void AssertEqual(
            IReadOnlyList<ILogMessage>? expectedResults,
            IReadOnlyList<ILogMessage>? actualResults)
        {
            if (expectedResults is null)
            {
                Assert.Null(actualResults);
            }
            else
            {
                Assert.NotNull(actualResults);
                Assert.Equal(expectedResults.Count, actualResults!.Count);

                for (var i = 0; i < expectedResults.Count; ++i)
                {
                    TestUtility.AssertEqual(expectedResults[i], actualResults[i]);
                }
            }
        }
    }
}
