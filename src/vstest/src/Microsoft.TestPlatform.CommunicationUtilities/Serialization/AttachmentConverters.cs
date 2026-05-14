// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if NETCOREAPP

using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Serialization;

/// <summary>
/// JSON converter for <see cref="AttachmentSet"/> that handles the lack of a parameterless constructor.
/// </summary>
internal class AttachmentSetConverter : JsonConverter<AttachmentSet>
{
    public override AttachmentSet? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var doc = JsonDocument.ParseValue(ref reader);
        var element = doc.RootElement;

        var uri = new Uri(element.GetProperty("Uri").GetString()!);
        var displayName = element.GetProperty("DisplayName").GetString()!;
        var attachmentSet = new AttachmentSet(uri, displayName);

        if (element.TryGetProperty("Attachments", out var attachments) && attachments.GetArrayLength() > 0)
        {
            foreach (var attachment in attachments.EnumerateArray())
            {
                if (attachment.ValueKind != JsonValueKind.Null)
                {
                    attachmentSet.Attachments.Add(JsonSerializer.Deserialize<UriDataAttachment>(attachment.GetRawText(), options)!);
                }
            }
        }

        return attachmentSet;
    }

    public override void Write(Utf8JsonWriter writer, AttachmentSet value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString("Uri", value.Uri.OriginalString);
        writer.WriteString("DisplayName", value.DisplayName);
        writer.WritePropertyName("Attachments");
        JsonSerializer.Serialize(writer, value.Attachments, options);
        writer.WriteEndObject();
    }
}

/// <summary>
/// JSON converter for <see cref="UriDataAttachment"/> that handles the lack of a parameterless constructor
/// and read-only properties.
/// </summary>
internal class UriDataAttachmentConverter : JsonConverter<UriDataAttachment>
{
    public override UriDataAttachment? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var doc = JsonDocument.ParseValue(ref reader);
        var element = doc.RootElement;

        var uri = new Uri(element.GetProperty("Uri").GetString()!);
        var description = element.TryGetProperty("Description", out var descProp) && descProp.ValueKind != JsonValueKind.Null
            ? descProp.GetString()
            : null;

        return new UriDataAttachment(uri, description);
    }

    public override void Write(Utf8JsonWriter writer, UriDataAttachment value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString("Description", value.Description);
        writer.WriteString("Uri", value.Uri.OriginalString);
        writer.WriteEndObject();
    }
}

#endif
