// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;

namespace Microsoft.VisualStudio.TestPlatform.Client;

/// <summary>
/// The factory class that provides an instance of the test platform.
/// </summary>
public class TestPlatformFactory
{
    private static ITestPlatform? s_testPlatform;

    /// <summary>
    /// Gets an instance of the test platform.
    /// </summary>
    /// <returns> The <see cref="ITestPlatform"/> instance. </returns>
    public static ITestPlatform GetTestPlatform() => s_testPlatform ??= new TestPlatform();
}
