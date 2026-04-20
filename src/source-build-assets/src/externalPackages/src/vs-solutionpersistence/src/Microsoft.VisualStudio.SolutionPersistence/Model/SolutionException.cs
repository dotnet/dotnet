// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Xml;
using Microsoft.VisualStudio.SolutionPersistence.Serializer.Xml.XmlDecorators;

namespace Microsoft.VisualStudio.SolutionPersistence.Model;

/// <summary>
/// An exception that is thrown when a solution file is not in the expected format.
/// </summary>
[Serializable]
public class SolutionException : FormatException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SolutionException"/> class.
    /// </summary>
    public SolutionException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SolutionException"/> class.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    public SolutionException(string message)
        : base(message)
    {
        this.ErrorType = SolutionErrorType.Undefined;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SolutionException"/> class.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="inner">The exception that is the cause of the current exception.</param>
    public SolutionException(string message, Exception inner)
        : base(message, inner)
    {
        this.ErrorType = SolutionErrorType.Undefined;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SolutionException"/> class.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="errorType">The type of error associated to this exception.</param>
    public SolutionException(string message, SolutionErrorType errorType)
        : base(message)
    {
        this.ErrorType = errorType;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SolutionException"/> class.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="inner">The exception that is the cause of the current exception.</param>
    /// <param name="errorType">The type of error associated to this exception.</param>
    public SolutionException(string message, Exception inner, SolutionErrorType errorType)
        : base(message, inner)
    {
        this.ErrorType = errorType;
    }

#if NETFRAMEWORK
    /// <summary>
    /// Initializes a new instance of the <see cref="SolutionException"/> class.
    /// Used for serialization in .NET Framework.
    /// </summary>
    /// <param name="info">Serialization info.</param>
    /// <param name="context">Contextual info.</param>
    [SuppressMessage("ApiDesign", "RS0016:Add public types and members to the declared API", Justification = "Only in .NET Framework.")]
    protected SolutionException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
        : base(info, context)
    {
        this.File = info.GetString("File");
        int line = info.GetInt32("Line");
        int column = info.GetInt32("Column");
        this.Line = line < 0 ? null : line;
        this.Column = column < 0 ? null : column;
    }
#endif

    /// <summary>
    /// Gets error type.
    /// </summary>
    public SolutionErrorType? ErrorType { get; init; }

    /// <summary>
    /// Gets file the error occurred in if known.
    /// </summary>
    public string? File { get; init; }

    /// <summary>
    /// Gets line number the error occurred on if known.
    /// </summary>
    public int? Line { get; init; }

    /// <summary>
    /// Gets column number the error occurred on if known.
    /// </summary>
    public int? Column { get; init; }

#if NETFRAMEWORK
    /// <inheritdoc/>
    [SuppressMessage("ApiDesign", "RS0016:Add public types and members to the declared API", Justification = "Only in .NET Framework.")]
    public override void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
    {
        base.GetObjectData(info, context);
        info.AddValue("File", this.File);
        info.AddValue("Line", this.Line ?? -1);
        info.AddValue("Column", this.Column ?? -1);
    }
#endif

    internal static SolutionException Create(string message, XmlDecorator location, SolutionErrorType errorType = SolutionErrorType.Undefined)
    {
        return location?.XmlElement is IXmlLineInfo lineInfo && lineInfo.HasLineInfo() ?
            new SolutionException(message, errorType) { Line = lineInfo.LineNumber, Column = lineInfo.LinePosition, File = location.Root.FullPath } :
            new SolutionException(message, errorType) { File = location?.Root.FullPath };
    }

    internal static SolutionException Create(Exception innerException, XmlDecorator location, string? message = null, SolutionErrorType errorType = SolutionErrorType.Undefined)
    {
        message ??= innerException.Message;
        return location?.XmlElement is IXmlLineInfo lineInfo && lineInfo.HasLineInfo() ?
            new SolutionException(message, innerException, errorType) { Line = lineInfo.LineNumber, Column = lineInfo.LinePosition, File = location.Root.FullPath } :
            new SolutionException(message, innerException, errorType) { File = location?.Root.FullPath };
    }

    // Checks if an exception caught during serialization should be wrapped by a SolutionException to add position information.
    internal static bool ShouldWrap(Exception ex)
    {
        return ex is not SolutionException and not OperationCanceledException;
    }
}
