// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;

/// <summary>
/// Interface implemented to log messages and results from tests.  A class that
/// implements this interface will be available for use if it exports its type via
/// MEF, and if its containing assembly is placed in the Extensions folder.
/// </summary>
public interface ITestLogger
{
    /// <summary>
    /// Initializes the Test Logger.
    /// </summary>
    /// <param name="events">Events that can be registered for.</param>
    /// <param name="testRunDirectory">Test Run Directory</param>
    void Initialize(TestLoggerEvents events, string testRunDirectory);
}
