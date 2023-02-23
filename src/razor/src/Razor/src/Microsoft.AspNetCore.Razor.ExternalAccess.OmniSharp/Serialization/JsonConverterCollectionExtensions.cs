// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Razor.ExternalAccess.OmniSharp.Project;
using Microsoft.AspNetCore.Razor.LanguageServer.Common.Extensions;
using Newtonsoft.Json;

namespace Microsoft.AspNetCore.Razor.ExternalAccess.OmniSharp.Serialization;

public static class JsonConverterCollectionExtensions
{
    public static void RegisterOmniSharpRazorConverters(this JsonConverterCollection collection)
    {
        collection.RegisterRazorConverters();
        collection.Add(OmniSharpProjectSnapshotHandleJsonConverter.Instance);
    }

    private class OmniSharpProjectSnapshotHandleJsonConverter : JsonConverter
    {
        internal static readonly OmniSharpProjectSnapshotHandleJsonConverter Instance = new();

        public override bool CanConvert(Type objectType)
        {
            return typeof(OmniSharpProjectSnapshot).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            var snapshot = (OmniSharpProjectSnapshot)value!;

            serializer.Serialize(writer, snapshot.InternalProjectSnapshot);
        }
    }
}
