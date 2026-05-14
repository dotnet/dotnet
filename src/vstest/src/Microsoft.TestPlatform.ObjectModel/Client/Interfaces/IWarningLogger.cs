// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;

/// <summary>
/// DO NOT use this to extend TestPlatform, it is public only because ITestPlatform is also public, and will be made internal later.
/// </summary>
public interface IWarningLogger
{
    /// <summary>
    /// Log warning message that will be shown to user.
    /// </summary>
    /// <param name="message">message string</param>
    void LogWarning(string message);
}
