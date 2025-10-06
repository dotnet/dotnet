// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using NuGet.Common;

namespace NuGet.ProjectModel
{
    public static class CacheFileFormat
    {
        private static JsonSerializerOptions SerializerOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Converters = { new AssetsLogMessageConverter() },
            NumberHandling = JsonNumberHandling.AllowReadingFromString,
            AllowTrailingCommas = true
        };

        /// <summary>
        /// Since Log messages property in CacheFile is an interface type, we have the following custom converter to deserialize the IAssetsLogMessage objects.
        /// </summary>
        private class AssetsLogMessageConverter : JsonConverter<IAssetsLogMessage>
        {
            public override IAssetsLogMessage Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return JsonSerializer.Deserialize<AssetsLogMessage>(ref reader, options);
            }

            public override void Write(Utf8JsonWriter writer, IAssetsLogMessage value, JsonSerializerOptions options)
            {
                JsonSerializer.Serialize(writer, (AssetsLogMessage)value, options);
            }
        }

        public static CacheFile Read(Stream stream, ILogger log, string path)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            if (log == null) throw new ArgumentNullException(nameof(log));
            if (path == null) throw new ArgumentNullException(nameof(path));

            try
            {
                var cacheFile = JsonSerializer.Deserialize<CacheFile>(utf8Json: stream, SerializerOptions);
                return cacheFile;
            }
            catch (Exception ex) when (ex is ArgumentNullException || ex is JsonException || ex is NotSupportedException)
            {
                log.LogWarning(string.Format(CultureInfo.CurrentCulture,
                    Strings.Log_ProblemReadingCacheFile,
                    path, ex.Message));
            }

            return new CacheFile(null);
        }

        public static void Write(string filePath, CacheFile lockFile)
        {
            // Create the directory if it does not exist
            var fileInfo = new FileInfo(filePath);
            fileInfo.Directory.Create();

            using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                Write(stream, lockFile);
            }
        }

        public static void Write(Stream stream, CacheFile cacheFile)
        {
#if NET5_0_OR_GREATER
            using (var textWriter = new StreamWriter(stream))
#else
            using (var textWriter = new NoAllocNewLineStreamWriter(stream))
#endif
            {
                Write(textWriter, cacheFile);
            }
        }

        private static void Write(TextWriter textWriter, CacheFile cacheFile)
        {
            textWriter.Write(JsonSerializer.Serialize(cacheFile, SerializerOptions));
        }
    }
}
