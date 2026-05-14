// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.Serialization;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;

/// <summary>
/// Session End event arguments
/// </summary>
[DataContract]
public sealed class TestHostLaunchedEventArgs : DataCollectionEventArgs
{
    /// <summary>
    /// Process id of the test host
    /// </summary>
    public int TestHostProcessId { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="TestHostLaunchedEventArgs"/> class.
    /// </summary>
    /// <param name="context">
    /// Data collection context
    /// </param>
    /// <param name="processId">
    /// Process id of test host
    /// </param>
    public TestHostLaunchedEventArgs(DataCollectionContext context, int processId)
        : base(context)
    {
        TestHostProcessId = processId;
    }

}
