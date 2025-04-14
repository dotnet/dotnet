// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.IO;
using System.Text;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.VisualStudio.Text;

namespace Microsoft.VisualStudio.LegacyEditor.Razor.Parsing;

internal sealed class TextSnapshotProjectItem : RazorProjectItem
{
    private readonly ITextSnapshot _snapshot;

    public TextSnapshotProjectItem(ITextSnapshot snapshot, string projectDirectory, string relativeFilePath, string filePath, RazorFileKind fileKind)
    {
        ArgHelper.ThrowIfNull(snapshot);
        ArgHelper.ThrowIfNullOrEmpty(projectDirectory);
        ArgHelper.ThrowIfNullOrEmpty(relativeFilePath);
        ArgHelper.ThrowIfNullOrEmpty(filePath);

        _snapshot = snapshot;
        BasePath = projectDirectory;
        FilePath = relativeFilePath;
        PhysicalPath = filePath;
        FileKind = fileKind;
    }

    public override string BasePath { get; }
    public override RazorFileKind FileKind { get; }
    public override string FilePath { get; }
    public override string PhysicalPath { get; }

    public override bool Exists => true;

    public override Stream Read()
    {
        var charArray = _snapshot.ToCharArray(0, _snapshot.Length);

        // We can assume UTF8 because the call path that reads from RazorProjectItem => SourceDocument
        // can't determine the encoding and always assumes Encoding.UTF8. This is something that we might
        // want to revisit in the future.
        var bytes = Encoding.UTF8.GetBytes(charArray);
        var memoryStream = new MemoryStream(bytes);
        return memoryStream;
    }
}
