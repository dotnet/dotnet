// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if !NETCOREAPP
using System;

using Jsonite;

using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Serialization;

namespace Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;

public partial class JsonDataSerializer
{
    private static partial (int version, string? messageType) ParseHeaderFromJson(string rawMessage)
    {
        var parsed = (JsonObject)Json.Deserialize(rawMessage);
        int version = parsed.TryGetValue("Version", out var vObj) ? Convert.ToInt32(vObj, System.Globalization.CultureInfo.InvariantCulture) : 0;
        string? messageType = parsed.TryGetValue("MessageType", out var mtObj) ? (string?)mtObj : null;
        return (version, messageType);
    }

    private static partial T? DeserializePayloadCore<T>(Message message)
    {
        var parsed = (JsonObject)Json.Deserialize(message.RawMessage!);
        if (!parsed.TryGetValue("Payload", out var payloadObj))
            return default;

        return JsoniteConvert.To<T>(payloadObj, message.Version);
    }

    private static partial T? DeserializeCore<T>(string json, int version)
    {
        var obj = Json.Deserialize(json, new JsonSettings { AllowTrailingCommas = true });
        return JsoniteConvert.To<T>(obj, version);
    }

    private static partial string SerializeMessageCore(string? messageType)
    {
        var envelope = new JsonObject { ["MessageType"] = messageType! };
        return Json.Serialize(envelope);
    }

    private static partial string SerializePayloadCore(string? messageType, object? payload, int version)
    {
        if (payload is null)
            return string.Empty;

        var payloadValue = JsoniteConvert.ToJsonValue(payload, version);

        if (version > 1)
        {
            var envelope = new JsonObject
            {
                ["Version"] = version,
                ["MessageType"] = messageType!,
                ["Payload"] = payloadValue!,
            };
            return Json.Serialize(envelope);
        }
        else
        {
            var envelope = new JsonObject
            {
                ["MessageType"] = messageType!,
                ["Payload"] = payloadValue!,
            };
            return Json.Serialize(envelope);
        }
    }

    private static partial string SerializeCore<T>(T data, int version)
    {
        ValidateVersion(version);
        var jsonValue = JsoniteConvert.ToJsonValue(data, version);
        return Json.Serialize(jsonValue!);
    }

    private static void ValidateVersion(int version)
    {
        if (version is not (0 or 1 or 2 or 3 or 4 or 5 or 6 or 7))
        {
            throw new NotSupportedException($"Protocol version {version} is not supported. "
                + "Ensure it is compatible with the latest serializer or add a new one.");
        }
    }
}
#endif
