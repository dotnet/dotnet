// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Runtime.Serialization;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;

/// <summary>
/// Event arguments used for raising Test Run Message events.
/// </summary>
[DataContract]
public class TestRunMessageEventArgs : EventArgs
{
    /// <summary>
    /// Initializes with the level and the message for the event.
    /// </summary>
    /// <param name="level">Level of the message.</param>
    /// <param name="message">The message.</param>
    public TestRunMessageEventArgs(TestMessageLevel level, string message)
    {
        ValidateArg.NotNullOrWhiteSpace(message, nameof(message));

        if (level is < TestMessageLevel.Informational or > TestMessageLevel.Error)
        {
            throw new ArgumentOutOfRangeException(nameof(level));
        }

        Level = level;
        Message = message;
    }

    /// <summary>
    /// The message.
    /// </summary>
    [DataMember]
    public string Message { get; set; }

    /// <summary>
    /// Level of the message.
    /// </summary>
    [DataMember]
    public TestMessageLevel Level { get; set; }

}
