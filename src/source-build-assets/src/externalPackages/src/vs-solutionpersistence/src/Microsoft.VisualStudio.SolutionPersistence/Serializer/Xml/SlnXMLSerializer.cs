// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.SolutionPersistence.Model;
using Microsoft.VisualStudio.SolutionPersistence.Utilities;

namespace Microsoft.VisualStudio.SolutionPersistence.Serializer.Xml;

internal sealed partial class SlnXmlSerializer : SingleFileSerializerBase<SlnxSerializerSettings>
{
    private const string Extension = ".slnx";

    private const string SerializerName = "Slnx";

    [Obsolete("Use Instance")]
    public SlnXmlSerializer()
    {
    }

    /// <inheritdoc/>
    public override string Name => SerializerName;

    internal static SlnXmlSerializer Instance => Singleton<SlnXmlSerializer>.Instance;

    private protected override string FileExtension => Extension;

    /// <inheritdoc/>
    public override ISerializerModelExtension CreateModelExtension()
    {
        return this.CreateModelExtension(new SlnxSerializerSettings()
        {
            // For new documents want to do standard indentation.
            PreserveWhitespace = false,
            IndentChars = "  ",
            NewLine = Environment.NewLine,
        });
    }

    /// <inheritdoc/>
    public override ISerializerModelExtension CreateModelExtension(SlnxSerializerSettings settings)
    {
        return new SlnXmlModelExtension(this, settings);
    }

    private protected override Task<SolutionModel> ReadModelAsync(string? fullPath, Stream reader, CancellationToken cancellationToken)
    {
        Reader parser = new Reader(fullPath, reader);
        return Task.FromResult(parser.Parse());
    }

    private protected override Task WriteModelAsync(string? fullPath, SolutionModel model, Stream writerStream, CancellationToken cancellationToken)
    {
        return Writer.SaveAsync(fullPath, model, writerStream);
    }
}
