// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.Win32;
using NuGet.Test.Utility;
using Xunit;
using Xunit.Abstractions;

namespace NuGet.CommandLine.Test
{
    public class RegistryKeyUtilityTests
    {
        private readonly TestLogger _logger;

        public RegistryKeyUtilityTests(ITestOutputHelper output)
        {
            _logger = new TestLogger(output);
        }

        [WindowsNTFact]
        public void GetValueFromRegistryKey_UnknownKey_ReturnsNull()
        {
            // Prepare
            var guidString = new Guid().ToString();
            var registryKey = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Default);

            // Act
            var value = RegistryKeyUtility.GetValueFromRegistryKey(guidString, @"key_local_machine\test\folder", registryKey, _logger);

            // Assert
            Assert.Null(value);
            Assert.Empty(_logger.Messages);
        }

        [WindowsNTFact]
        public void GetValueFromRegistryKey_GetSampleKey_ReturnsNotNull()
        {
            // Act
            object value = RegistryKeyUtility.GetValueFromRegistryKey("ProductName", @"SOFTWARE\Microsoft\Windows NT\CurrentVersion", Registry.LocalMachine, _logger);

            // Assert
            Assert.NotNull(value);
            Assert.IsType<string>(value);
            Assert.StartsWith("Windows", (string)value);
            Assert.Empty(_logger.Messages);
        }
    }
}
