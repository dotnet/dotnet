// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;

/// <summary>
/// Class defining execution events that will be registered for by collectors
/// </summary>
public abstract class DataCollectionEvents
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DataCollectionEvents"/> class.
    /// </summary>
    protected DataCollectionEvents()
    {
    }

    /// <summary>
    /// Raised when test host initialized
    /// </summary>
    public abstract event EventHandler<TestHostLaunchedEventArgs>? TestHostLaunched;

    /// <summary>
    /// Raised when a session is starting
    /// </summary>
    public abstract event EventHandler<SessionStartEventArgs>? SessionStart;

    /// <summary>
    /// Raised when a session is ending
    /// </summary>
    public abstract event EventHandler<SessionEndEventArgs>? SessionEnd;

    /// <summary>
    /// Raised when a test case is starting
    /// </summary>
    public abstract event EventHandler<TestCaseStartEventArgs>? TestCaseStart;

    /// <summary>
    /// Raised when a test case is ending
    /// </summary>
    public abstract event EventHandler<TestCaseEndEventArgs>? TestCaseEnd;

}
