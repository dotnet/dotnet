// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

/// <summary>
/// Wraps an on-disk XML file as both a line buffer (for surgical, formatting-preserving edits)
/// and an <see cref="XDocument"/> (for structural navigation with line info). Editors mutate
/// <see cref="Lines"/> via <see cref="System.Xml.IXmlLineInfo"/> line numbers obtained from
/// <see cref="Document"/>; <see cref="Save"/> writes the buffer back with the original newline
/// convention.
/// </summary>
internal sealed class FileEditBuffer
{
    public string Path { get; }
    public string Newline { get; }
    public List<string> Lines { get; }
    public XDocument Document { get; }

    private FileEditBuffer(string path, string newline, List<string> lines, XDocument document)
    {
        Path = path;
        Newline = newline;
        Lines = lines;
        Document = document;
    }

    public static FileEditBuffer Load(string path)
    {
        string content = File.ReadAllText(path);
        string newline = content.Contains("\r\n") ? "\r\n" : "\n";
        List<string> lines = new(content.Split(new[] { newline }, System.StringSplitOptions.None));
        XDocument document = XDocument.Parse(content, LoadOptions.PreserveWhitespace | LoadOptions.SetLineInfo);
        return new FileEditBuffer(path, newline, lines, document);
    }

    public void Save() => File.WriteAllText(Path, string.Join(Newline, Lines));
}
