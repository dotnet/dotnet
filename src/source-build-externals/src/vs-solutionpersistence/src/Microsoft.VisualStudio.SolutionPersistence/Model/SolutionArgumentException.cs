// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.VisualStudio.SolutionPersistence.Model;

/// <summary>
/// Represents an argument exception inside the solution.
/// </summary>
public class SolutionArgumentException : ArgumentException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SolutionArgumentException"/> class.
    /// </summary>
    /// <param name="message">Message to be shown with the exception.</param>
    /// <param name="type">Reason for the exception.</param>
    public SolutionArgumentException(string? message, SolutionErrorType type)
        : base(message)
    {
        this.Type = type;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SolutionArgumentException"/> class.
    /// </summary>
    /// <param name="message">Message to be shown with the exception.</param>
    /// <param name="innerException">Exception that triggered this exception.</param>
    /// <param name="type">Reason for the exception.</param>
    public SolutionArgumentException(string? message, Exception? innerException, SolutionErrorType type)
        : base(message, innerException)
    {
        this.Type = type;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SolutionArgumentException"/> class.
    /// </summary>
    /// <param name="message">Message to be shown with the exception.</param>
    /// <param name="paramName">Name of parameter that triggered this exception.</param>
    /// <param name="type">Reason for the exception.</param>
    public SolutionArgumentException(string? message, string? paramName, SolutionErrorType type)
        : base(message, paramName)
    {
        this.Type = type;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SolutionArgumentException"/> class.
    /// </summary>
    /// <param name="message">Message to be shown with the exception.</param>
    /// <param name="paramName">Name of parameter that triggered this exception.</param>
    /// <param name="innerException">Exception that triggered this exception.</param>
    /// <param name="type">Reason for the exception.</param>
    public SolutionArgumentException(string? message, string? paramName, Exception? innerException, SolutionErrorType type)
        : base(message, paramName, innerException)
    {
        this.Type = type;
    }

    /// <summary>
    /// Gets reason why the exception was raised.
    /// </summary>
    public SolutionErrorType Type { get; init; }
}
