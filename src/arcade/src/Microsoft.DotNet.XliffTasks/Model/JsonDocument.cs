// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace XliffTasks.Model;

/// <summary>
/// A <see cref="TranslatableDocument"/> backed by an XML-based format
/// </summary>
internal sealed class JsonDocument : TranslatableDocument
{
    private static readonly JsonSerializerOptions s_serializerOptions = new()
    {
        WriteIndented = true,
    };

    private Dictionary<string, string> _json;

    protected override void LoadCore(TextReader reader)
    {
        var text = reader.ReadToEnd();
        _json = JsonSerializer.Deserialize<Dictionary<string, string>>(text);
    }

    protected override void SaveCore(TextWriter writer)
    {
        writer.Write(JsonSerializer.Serialize(_json, typeof(Dictionary<string, string>), s_serializerOptions));
    }

    protected override IEnumerable<TranslatableNode> GetTranslatableNodes()
        => _json.Select(kvp => new Node(_json, kvp.Key, kvp.Value)).OrderBy(entry => entry.Id);

    private sealed class Node(Dictionary<string, string> json, string id, string source) : TranslatableNode(id, source, note: null)
    {
        public override void Translate(string translation)
        {
           json[Id] = translation;
        }
    }
}
