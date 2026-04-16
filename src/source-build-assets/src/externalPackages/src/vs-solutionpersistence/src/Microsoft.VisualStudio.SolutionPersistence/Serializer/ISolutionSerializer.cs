// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.SolutionPersistence.Model;

namespace Microsoft.VisualStudio.SolutionPersistence;

/// <summary>
/// Represents a solution serializer.
/// </summary>
public interface ISolutionSerializer
{
    /// <summary>
    /// Gets the name of the serializer.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Creates a default model extension for the serializer.
    /// This is added to the model to provide additional information from the serializer.
    /// </summary>
    /// <returns>A model extension object.</returns>
    ISerializerModelExtension CreateModelExtension();

    /// <summary>
    /// Opens a solution model from a moniker location.
    /// </summary>
    /// <param name="moniker">The moniker that represents the solution location.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The loaded solution model.</returns>
    Task<SolutionModel> OpenAsync(string moniker, CancellationToken cancellationToken);

    /// <summary>
    /// Saves a solution model to a moniker location.
    /// </summary>
    /// <param name="moniker">The moniker that represents the solution location.</param>
    /// <param name="model">The model to save.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Task to track the asynchronous call status.</returns>
    Task SaveAsync(string moniker, SolutionModel model, CancellationToken cancellationToken);

    /// <summary>
    /// Checks if a moniker is supported by the serializer.
    /// This doesn't validate the file contents, just the moniker type.
    /// For single file serializers, this checks the file extension.
    /// </summary>
    /// <param name="moniker">The moniker that represents the solution location.</param>
    /// <returns>If this serilizer can open the solution.</returns>
    bool IsSupported(string moniker);
}

/// <summary>
/// Represents a solution serializer.
/// </summary>
/// <typeparam name="TSettings">The settings type for the serializer.</typeparam>
public interface ISolutionSerializer<TSettings> : ISolutionSerializer
{
    /// <summary>
    /// Creates a default model extension for the serializer.
    /// This is added to the model to provide additional information from the serializer.
    /// </summary>
    /// <param name="settings">[Optional] Settings that are specific to the serializer.</param>
    /// <returns>A model extension object.</returns>
    ISerializerModelExtension CreateModelExtension(TSettings settings);
}

/// <summary>
/// Represents a solution serializer that is contained in a single file.
/// </summary>
/// <typeparam name="TSettings">The settings type for the serializer.</typeparam>
public interface ISolutionSingleFileSerializer<TSettings> : ISolutionSerializer<TSettings>
{
    /// <summary>
    /// Gets the default file extension of the serializer.
    /// </summary>
    string DefaultFileExtension { get; }

    /// <summary>
    /// Opens a solution model from a stream.
    /// </summary>
    /// <param name="stream">The stream containing the file.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The solution model of the file.</returns>
    Task<SolutionModel> OpenAsync(Stream stream, CancellationToken cancellationToken);

    /// <summary>
    /// Saves a solution model to a stream.
    /// </summary>
    /// <param name="stream">The stream to save the file..</param>
    /// <param name="model">The model to save.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Task to track the asynchronous call status.</returns>
    Task SaveAsync(Stream stream, SolutionModel model, CancellationToken cancellationToken);
}
