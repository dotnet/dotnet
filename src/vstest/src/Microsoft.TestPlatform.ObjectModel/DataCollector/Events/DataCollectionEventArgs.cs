// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;

/// <summary>
/// Base class for all execution event arguments
/// </summary>
#if NETFRAMEWORK
[Serializable]
#endif
public abstract class DataCollectionEventArgs : EventArgs
{
    /// <summary>
    /// Default constructor added for serialization/deserialization.
    /// </summary>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public DataCollectionEventArgs()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
        // TODO: Make private
    }

    /// <summary>
    /// Initializes the instance by storing the given information
    /// </summary>
    /// <param name="context">Context information for the event</param>
    protected DataCollectionEventArgs(DataCollectionContext context)
    {
        Context = context;
    }

    /// <summary>
    /// Gets the context information for the event
    /// </summary>
    public DataCollectionContext Context
    {
        get;
        internal set;
    }
}
