// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;

/// <summary>
/// Class used by data collectors to send messages to the client (e.g. Manual Test Runner, Visual Studio IDE, MSTest).
/// </summary>
public abstract class DataCollectionLogger
{
    /// <summary>
    /// Constructs a DataCollectionLogger
    /// </summary>
    protected DataCollectionLogger()
    {
    }

    /// <summary>
    /// Logs an error message.
    /// </summary>
    /// <param name="context">The context in which the message is being sent.</param>
    /// <param name="text">The error text.  Cannot be null.</param>
    /// <remarks>
    /// When a Data Collector invokes this method, Client would get called on OnCollectionError( ) with a CollectionErrorMessageEventArgs.
    /// </remarks>
    public abstract void LogError(DataCollectionContext context, string text);

    /// <summary>
    /// Logs an error message for an exception.
    /// </summary>
    /// <param name="context">The context in which the message is being sent.</param>
    /// <param name="exception">The exception.  Cannot be null.</param>
    /// <remarks>
    /// When a Data Collector invokes this method, Client would get called on OnCollectionError( ) with a CollectionErrorMessageEventArgs.
    /// </remarks>
    public void LogError(DataCollectionContext context, Exception exception)
    {
        LogError(context, string.Empty, exception);
    }

    /// <summary>
    /// Logs an error message for an exception.
    /// </summary>
    /// <param name="context">The context in which the message is being sent.</param>
    /// <param name="text">Text explaining the exception.  Cannot be null.</param>
    /// <param name="exception">The exception.  Cannot be null.</param>
    /// <remarks>
    /// When a Data Collector invokes this method, Client would get called on OnCollectionError( ) with a CollectionErrorMessageEventArgs.
    /// </remarks>
    public abstract void LogError(DataCollectionContext context, string text, Exception exception);

    /// <summary>
    /// Logs a warning.
    /// </summary>
    /// <param name="context">The context in which the message is being sent.</param>
    /// <param name="text">The warning text.  Cannot be null.</param>
    /// <remarks>
    /// When a Data Collector invokes this method, Client would get called on OnCollectionWarning( ) with a CollectionWarningMessageEventArgs.
    /// </remarks>
    public abstract void LogWarning(DataCollectionContext context, string text);

    /// <summary>
    /// Logs and given exception to the client.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="ex">The exception to be logged</param>
    /// <param name="level"> Is the exception at warning level or error level.</param>
    /// <remarks>
    /// When a Data Collector invokes this method, Client would get called on OnCollectionException( ) with a CollectionExceptionMessageEventArgs.
    /// </remarks>
    public virtual void LogException(DataCollectionContext context, Exception ex, DataCollectorMessageLevel level)
    {
    }

}
