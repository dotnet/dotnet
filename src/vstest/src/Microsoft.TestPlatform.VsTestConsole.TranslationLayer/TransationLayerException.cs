// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.TestPlatform.VsTestConsole.TranslationLayer;

/// <summary>
/// Specialized exception for TranslationLayer
/// </summary>
public class TransationLayerException : Exception
{
    /// <summary>
    /// Initializes a new instance of the TransationLayerException class.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public TransationLayerException(string? message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the TransationLayerException class.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public TransationLayerException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }
}
