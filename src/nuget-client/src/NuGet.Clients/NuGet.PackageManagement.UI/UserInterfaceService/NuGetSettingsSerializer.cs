// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace NuGet.PackageManagement.UI
{
    internal sealed class NuGetSettingsSerializer
    {
        private const int _bufferSize = 4096;

        private readonly JsonSerializer _serializer;

        internal NuGetSettingsSerializer()
        {
            _serializer = new JsonSerializer();

            _serializer.Converters.Add(new StringEnumConverter());
        }

        internal NuGetSettings Deserialize(Stream stream)
        {
            using (var streamReader = new StreamReader(stream, Encoding.UTF8, detectEncodingFromByteOrderMarks: false, bufferSize: _bufferSize, leaveOpen: true))
            using (var jsonReader = new JsonTextReader(streamReader))
            {
                jsonReader.CloseInput = false;

                return _serializer.Deserialize<NuGetSettings>(jsonReader);
            }
        }

        internal void Serialize(Stream stream, NuGetSettings settings)
        {
            using (var streamWriter = new StreamWriter(stream, Encoding.UTF8, bufferSize: _bufferSize, leaveOpen: true))
            using (var jsonWriter = new JsonTextWriter(streamWriter))
            {
                _serializer.Serialize(jsonWriter, settings);
            }
        }
    }
}
