// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

namespace OpenTelemetry.OpAmp.Client.Messages;

/// <summary>
/// Currently active configuration reference.
/// </summary>
public sealed class EffectiveConfigFile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EffectiveConfigFile"/> class.
    /// </summary>
    /// <param name="content">File content.</param>
    /// <param name="contentType">File content type.</param>
    /// <param name="fileName">File name.</param>
    public EffectiveConfigFile(Memory<byte> content, string contentType, string fileName)
    {
        this.Content = content;
        this.ContentType = contentType;
        this.FileName = fileName;
    }

    /// <summary>
    /// Gets the file content.
    /// </summary>
    public Memory<byte> Content { get; private set; }

    /// <summary>
    /// Gets the file content type.
    /// </summary>
    public string ContentType { get; private set; }

    /// <summary>
    /// Gets the file name.
    /// </summary>
    public string FileName { get; private set; }

    /// <summary>
    /// Creates a configuration reference from the file path.
    /// </summary>
    /// <param name="filePath">Path of the configuration file.</param>
    /// <param name="contentType">The content type of the configuration file.</param>
    /// <param name="filename">The reported filename.</param>
    /// <returns>Effective config object.</returns>
    /// <remarks>
    /// <para>
    /// The entire file is read into memory and transmitted as-is to the OpAMP server.
    /// Do not include files that contain secrets (passwords, tokens, private keys) unless
    /// the transport is secure (e.g. with TLS) and the OpAMP server is fully trusted.
    /// </para>
    /// </remarks>
    public static EffectiveConfigFile CreateFromFilePath(string filePath, string contentType, string? filename = null)
    {
        try
        {
            var content = File.ReadAllBytes(filePath);
            var fileName = filename ?? Path.GetFileName(filePath);

            return new EffectiveConfigFile(content, contentType, fileName);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Could not read configuration file.", ex);
        }
    }
}
