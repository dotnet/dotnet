// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualStudio.TestPlatform.Common.Interfaces;

/// <summary>
/// Metadata that is available from Test Loggers.
/// </summary>
public interface ITestLoggerCapabilities : ITestExtensionCapabilities
{
    /// specifies the friendly name corresponding to the logger.
    string FriendlyName { get; }
}
