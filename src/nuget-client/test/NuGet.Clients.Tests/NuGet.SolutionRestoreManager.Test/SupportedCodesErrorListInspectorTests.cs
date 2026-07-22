// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.Shell.TableControl;
using NuGet.SolutionRestoreManager.ErrorListFixers;
using Xunit;

namespace NuGet.SolutionRestoreManager.Test
{
    public class SupportedCodesErrorListInspectorTests
    {
        private static IErrorListEntryInspector CreateInspector(params string[] supportedCodes)
        {
            return new SupportedCodesErrorListInspector(new HashSet<string>(supportedCodes, StringComparer.OrdinalIgnoreCase));
        }

        [Fact]
        public void Constructor_WithNullSupportedCodes_ThrowsArgumentNullException()
        {
            // Arrange
            ISet<string>? supportedCodes = null;

            // Act
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => new SupportedCodesErrorListInspector(supportedCodes!));

            // Assert
            Assert.Equal("supportedCodes", exception.ParamName);
        }

        [Fact]
        public void IsSupportedCode_WithSupportedCode_ReturnsTrue()
        {
            // Arrange
            ITableEntryHandle entry = ErrorListEntryTestUtility.CreateEntry("NU1903");
            IErrorListEntryInspector inspector = CreateInspector("NU1901", "NU1903");

            // Act
            bool result = inspector.IsSupportedCode(entry);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsSupportedCode_WithUnsupportedCode_ReturnsFalse()
        {
            // Arrange
            ITableEntryHandle entry = ErrorListEntryTestUtility.CreateEntry("NU1605");
            IErrorListEntryInspector inspector = CreateInspector("NU1903");

            // Act
            bool result = inspector.IsSupportedCode(entry);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void TryGetErrorCode_WhenCodeMissing_ReturnsFalse()
        {
            // Arrange
            ITableEntryHandle entry = ErrorListEntryTestUtility.CreateEntry(code: null);

            // Act
            bool result = SupportedCodesErrorListInspector.TryGetErrorCode(entry, out string code);

            // Assert
            Assert.False(result);
            Assert.Equal(string.Empty, code);
        }
    }
}
