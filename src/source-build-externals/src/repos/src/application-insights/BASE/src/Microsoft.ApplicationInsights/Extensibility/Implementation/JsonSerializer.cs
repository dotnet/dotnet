namespace Microsoft.ApplicationInsights.Extensibility.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.IO;
    using System.IO.Compression;
    using System.Text;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.External;

    /// <summary>
    /// Serializes and compress the telemetry items into a JSON string. Compression will be done using GZIP, for Windows Phone 8 compression will be disabled because there
    /// is API support for it. 
    /// </summary>    
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class JsonSerializer
    {
        private static readonly UTF8Encoding TransmissionEncoding = new UTF8Encoding(false);

        /// <summary>
        /// Gets the compression type used by the serializer. 
        /// </summary>
        public static string CompressionType
        {
            get
            {
                return "gzip";
            }
        }

        /// <summary>
        /// Gets the content type used by the serializer. 
        /// </summary>
        public static string ContentType
        {
            get
            {
                return "application/x-json-stream";
            }
        }

        /// <summary>
        /// Serializes and compress the telemetry items into a JSON string. Each JSON object is separated by a new line. 
        /// </summary>
        /// <param name="telemetryItems">The list of telemetry items to serialize.</param>
        /// <param name="compress">Should serialization also perform compression.</param>
        /// <returns>The compressed and serialized telemetry items.</returns>
        [SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times", Justification = "Disposing a MemoryStream multiple times is harmless.")]
        public static byte[] Serialize(IEnumerable<ITelemetry> telemetryItems, bool compress = true)
        {
            if (telemetryItems == null)
            {
                throw new ArgumentNullException(nameof(telemetryItems));
            }

            var memoryStream = new MemoryStream();
            using (Stream compressedStream = compress ? CreateCompressedStream(memoryStream) : memoryStream)
            {
                using (var streamWriter = new StreamWriter(compressedStream, TransmissionEncoding))
                {
                    SerializeToStream(telemetryItems, streamWriter);
                }
            }

            return memoryStream.ToArray();
        }

        /// <summary>
        /// Converts serialized telemetry items to a byte array.
        /// </summary>
        /// <param name="telemetryItems">Serialized telemetry items.</param>
        /// <param name="compress">Should serialization also perform compression.</param>
        /// <returns>The compressed and serialized telemetry items.</returns>
        [SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times", Justification = "Disposing a MemoryStream multiple times is harmless.")]
        public static byte[] ConvertToByteArray(string telemetryItems, bool compress = true)
        {
            if (string.IsNullOrEmpty(telemetryItems))
            {
                throw new ArgumentNullException(nameof(telemetryItems));
            }

            var memoryStream = new MemoryStream();
            using (Stream compressedStream = compress ? CreateCompressedStream(memoryStream) : memoryStream)
            using (var streamWriter = new StreamWriter(compressedStream, TransmissionEncoding))
            {
                streamWriter.Write(telemetryItems);
            }

            return memoryStream.ToArray();
        }

        /// <summary>
        /// Deserializes and decompress the telemetry items into a JSON string.
        /// </summary>
        /// <param name="telemetryItemsData">Serialized telemetry items.</param>
        /// <param name="compress">Should deserialization also perform decompression.</param>
        /// <returns>Telemetry items serialized as a string.</returns>
        public static string Deserialize(byte[] telemetryItemsData, bool compress = true)
        {
            var memoryStream = new MemoryStream(telemetryItemsData);

            using (Stream decompressedStream = compress ? (Stream)new GZipStream(memoryStream, CompressionMode.Decompress) : memoryStream)
            {
                using (MemoryStream str = new MemoryStream())
                {
                    decompressedStream.CopyTo(str);
                    byte[] output = str.ToArray();
                    return Encoding.UTF8.GetString(output, 0, output.Length);
                }
            }
        }

        /// <summary>
        ///  Serialize and compress a telemetry item. 
        /// </summary>
        /// <param name="telemetryItem">A telemetry item.</param>
        /// <param name="compress">Should serialization also perform compression.</param>
        /// <returns>The compressed and serialized telemetry item.</returns>
        [SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times", Justification = "Disposing a MemoryStream multiple times is harmless.")]
        internal static byte[] Serialize(ITelemetry telemetryItem, bool compress = true)
        {
            return Serialize(new ITelemetry[] { telemetryItem }, compress);
        }

        /// <summary>
        /// Serializes <paramref name="telemetryItems"/> into a JSON string. Each JSON object is separated by a new line. 
        /// </summary>
        /// <param name="telemetryItems">The list of telemetry items to serialize.</param>
        /// <returns>A JSON string of all the serialized items.</returns>
        internal static string SerializeAsString(IEnumerable<ITelemetry> telemetryItems)
        {
            var stringBuilder = new StringBuilder();
            using (StringWriter stringWriter = new StringWriter(stringBuilder, CultureInfo.InvariantCulture))
            {
                SerializeToStream(telemetryItems, stringWriter);
                return stringBuilder.ToString();
            }
        }

        /// <summary>
        /// Serializes a <paramref name="telemetry"/> into a JSON string. 
        /// </summary>
        /// <param name="telemetry">The telemetry to serialize.</param>
        /// <returns>A JSON string of the serialized telemetry.</returns>
        internal static string SerializeAsString(ITelemetry telemetry)
        {
            return SerializeAsString(new ITelemetry[] { telemetry });
        }

        /// <summary>
        /// Creates a GZIP compression stream that wraps <paramref name="stream"/>. For windows phone 8.0 it returns <paramref name="stream"/>. 
        /// </summary>
        private static Stream CreateCompressedStream(Stream stream)
        {
            return new GZipStream(stream, CompressionMode.Compress);
        }

        private static void SerializeTelemetryItem(ITelemetry telemetryItem, JsonSerializationWriter jsonSerializationWriter)
        {
            jsonSerializationWriter.WriteStartObject();

            if (telemetryItem is IAiSerializableTelemetry serializeableTelemetry)
            {
                telemetryItem.CopyGlobalPropertiesIfExist();
                telemetryItem.FlattenIExtensionIfExists();
                SerializeHelper(telemetryItem, jsonSerializationWriter, telemetryName: serializeableTelemetry.TelemetryName, baseType: serializeableTelemetry.BaseType);
            }
            else
            {
                SerializeUnknownTelemetryHelper(telemetryItem, jsonSerializationWriter);
            }

            jsonSerializationWriter.WriteEndObject();
        }

        private static void SerializeHelper(ITelemetry telemetryItem, JsonSerializationWriter jsonSerializationWriter, string baseType, string telemetryName)
        {
            jsonSerializationWriter.WriteProperty("name", telemetryName);
            telemetryItem.WriteEnvelopeProperties(jsonSerializationWriter);
            jsonSerializationWriter.WriteStartObject("data");
            jsonSerializationWriter.WriteProperty("baseType", baseType);
            jsonSerializationWriter.WriteStartObject("baseData");
            telemetryItem.SerializeData(jsonSerializationWriter);
            jsonSerializationWriter.WriteEndObject(); // baseData
            jsonSerializationWriter.WriteEndObject(); // data
        }

        private static void SerializeUnknownTelemetryHelper(ITelemetry telemetryItem, JsonSerializationWriter jsonSerializationWriter)
        {
            DictionarySerializationWriter dictionarySerializationWriter = new DictionarySerializationWriter();
            telemetryItem.SerializeData(dictionarySerializationWriter); // Properties and Measurements are covered as part of Data if present
            telemetryItem.CopyGlobalPropertiesIfExist(dictionarySerializationWriter.AccumulatedDictionary);

            if (telemetryItem.Extension != null)
            {
                DictionarySerializationWriter extensionSerializationWriter = new DictionarySerializationWriter();
                telemetryItem.Extension.Serialize(extensionSerializationWriter); // Extension is supposed to be flattened as well

                Utils.CopyDictionary(extensionSerializationWriter.AccumulatedDictionary, dictionarySerializationWriter.AccumulatedDictionary);
                Utils.CopyDictionary(extensionSerializationWriter.AccumulatedMeasurements, dictionarySerializationWriter.AccumulatedMeasurements);
            }

            jsonSerializationWriter.WriteProperty("name", EventTelemetry.DefaultEnvelopeName);
            telemetryItem.WriteEnvelopeProperties(jsonSerializationWriter); // No need to copy Context - it's serialized here from the original item

            jsonSerializationWriter.WriteStartObject("data");
            jsonSerializationWriter.WriteProperty("baseType", typeof(EventData).Name);
            jsonSerializationWriter.WriteStartObject("baseData");

            jsonSerializationWriter.WriteProperty("ver", 2);
            jsonSerializationWriter.WriteProperty("name", Constants.EventNameForUnknownTelemetry);

            jsonSerializationWriter.WriteProperty("properties", dictionarySerializationWriter.AccumulatedDictionary);
            jsonSerializationWriter.WriteProperty("measurements", dictionarySerializationWriter.AccumulatedMeasurements);

            jsonSerializationWriter.WriteEndObject(); // baseData            
            jsonSerializationWriter.WriteEndObject(); // data
        }

        /// <summary>
        /// Serializes <paramref name="telemetryItems"/> and write the response to <paramref name="streamWriter"/>.
        /// </summary>
        private static void SerializeToStream(IEnumerable<ITelemetry> telemetryItems, TextWriter streamWriter)
        {
            // JsonWriter jsonWriter = new JsonWriter(streamWriter);
            JsonSerializationWriter jsonSerializationWriter = new JsonSerializationWriter(streamWriter);

            int telemetryCount = 0;
            foreach (ITelemetry telemetryItem in telemetryItems)
            {
                if (telemetryCount++ > 0)
                {
                    streamWriter.Write(Environment.NewLine);
                }

                telemetryItem.Context.SanitizeGlobalProperties();
                telemetryItem.Sanitize();
                SerializeTelemetryItem(telemetryItem, jsonSerializationWriter);
            }
        }
    }
}
