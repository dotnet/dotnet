// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if NETFRAMEWORK
using System;
#endif

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;

/// <summary>
/// Encapsulates the context of the environment a data collector is being hosted in.
/// </summary>
#if NETFRAMEWORK
[Serializable]
#endif
public sealed class DataCollectionEnvironmentContext
{
    /// <summary>
    /// Serialization Constructor
    /// </summary>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private DataCollectionEnvironmentContext()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
    }

    /// <summary>
    /// Initializes with the DataCollectionContext
    /// </summary>
    public DataCollectionEnvironmentContext(DataCollectionContext sessionDataCollectionContext)
    {
        SessionDataCollectionContext = sessionDataCollectionContext;
    }

    /// <summary>
    /// Creates an environment context for a local (hosted) agent and controller
    /// </summary>
    /// <returns>An environment context for a local (hosted) agent and controller</returns>
    public static DataCollectionEnvironmentContext CreateForLocalEnvironment()
    {
        // TODO: This API appears unused, can we remove it?
        return CreateForLocalEnvironment(null!);
    }

    /// <summary>
    /// Creates an environment context for a local (hosted) agent and controller
    /// </summary>
    /// <param name="sessionDataCollectionContext">Session level data collection context.</param>
    /// <returns>An environment context for a local (hosted) agent and controller</returns>
    public static DataCollectionEnvironmentContext CreateForLocalEnvironment(DataCollectionContext sessionDataCollectionContext)
    {
        var dataCollectionEnvironmentContext = new DataCollectionEnvironmentContext(sessionDataCollectionContext);

        return dataCollectionEnvironmentContext;
    }

    /// <summary>
    /// DataCollectionContext for the session.
    /// </summary>
    public DataCollectionContext SessionDataCollectionContext { get; private set; }

}
