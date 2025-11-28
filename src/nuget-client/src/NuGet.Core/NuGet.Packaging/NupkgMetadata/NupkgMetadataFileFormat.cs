// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Globalization;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using NuGet.Common;
using NuGet.Packaging.NupkgMetadata;

namespace NuGet.Packaging
{
    public static class NupkgMetadataFileFormat
    {
        public static readonly int Version = 2;

        private static readonly JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions()
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            // This file is not intended to be parsed in javascript, and the contentHash property contains a Base64 encoded string that
            // can contain characters such as + that has not been encoded in the past.
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        };

        private static readonly NupkgMetadataSerializationContext JsonContext = new NupkgMetadataSerializationContext(JsonSerializerOptions);

        public static NupkgMetadataFile Read(string filePath)
        {
            return Read(filePath, NullLogger.Instance);
        }

        public static NupkgMetadataFile Read(string filePath, ILogger log)
        {
            return FileUtility.SafeRead(filePath, (stream, nupkgMetadataFilePath) => Read(stream, log, nupkgMetadataFilePath));
        }

        public static NupkgMetadataFile Read(Stream stream, ILogger log, string path)
        {
            if (stream is null) { throw new ArgumentNullException(nameof(stream)); }

            try
            {
                NupkgMetadataFile nupkgMetadata = JsonSerializer.Deserialize<NupkgMetadataFile>(stream, JsonContext.NupkgMetadataFile);
                if (nupkgMetadata == null)
                {
                    throw new InvalidDataException();
                }
                return nupkgMetadata;
            }
            catch (JsonException ex)
            {
                throw LogAndWrap(log, path, ex);
            }
            catch (Exception ex)
            {
                throw LogAndWrap(log, path, ex);
            }

            static InvalidDataException LogAndWrap(ILogger log, string path, Exception ex)
            {
                string message = (string.Format(CultureInfo.CurrentCulture,
                    Strings.Error_LoadingHashFile,
                    path, ex.Message));
                log.LogWarning(message);
                return new InvalidDataException(message, ex);
            }
        }

        public static void Write(string filePath, NupkgMetadataFile hashFile)
        {
            // Create the directory if it does not exist
            var fileInfo = new FileInfo(filePath);
            fileInfo.Directory.Create();

            using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                Write(stream, hashFile);
            }
        }

        public static void Write(Stream stream, NupkgMetadataFile hashFile)
        {
            if (stream is null) { throw new ArgumentNullException(nameof(stream)); }

            JsonSerializer.Serialize(stream, hashFile, JsonContext.NupkgMetadataFile);
        }
    }
}
