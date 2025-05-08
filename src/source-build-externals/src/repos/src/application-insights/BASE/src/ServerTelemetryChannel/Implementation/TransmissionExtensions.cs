namespace Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.IO;

    using Microsoft.ApplicationInsights.Channel;
    
    internal static class TransmissionExtensions
    {
        private const string ContentTypeHeader = "Content-Type";
        private const string ContentEncodingHeader = "Content-Encoding";

        /// <summary>
        /// Loads a new transmission from the specified <paramref name="stream"/>.
        /// </summary>
        /// <returns>Return transmission loaded from file; throws FormatException is file is corrupted.</returns>
        [SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "Disposing the StreamReader will also dispose the stream.")]
        public static Transmission Load(Stream stream)
        {
            var reader = new StreamReader(stream);
            Uri address = ReadAddress(reader);
            string contentType = ReadHeader(reader, ContentTypeHeader);
            string contentEncoding = ReadHeader(reader, ContentEncodingHeader);
            byte[] content = ReadContent(reader);
            return new Transmission(address, content, contentType, contentEncoding);
        }

        /// <summary>
        /// Saves the transmission to the specified <paramref name="stream"/>.
        /// </summary>
        [SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "Disposing the StreamWriter will also dispose the stream.")]
        public static void Save(this Transmission transmission, Stream stream)
        {
            var writer = new StreamWriter(stream);
            writer.WriteLine(transmission.EndpointAddress.ToString());

            writer.Write(ContentTypeHeader);
            writer.Write(":");
            writer.WriteLine(transmission.ContentType);

            writer.Write(ContentEncodingHeader);
            writer.Write(":");
            writer.WriteLine(transmission.ContentEncoding);

            writer.WriteLine(string.Empty);

            writer.Write(Convert.ToBase64String(transmission.Content));
            writer.Flush();
        }

        private static string ReadHeader(TextReader reader, string headerName)
        {
            string line = reader.ReadLine();
            if (string.IsNullOrEmpty(line))
            {
                throw new FormatException(string.Format(CultureInfo.InvariantCulture, "{0} header is expected.", headerName));
            }

            string[] parts = line.Split(':');
            if (parts.Length != 2)
            {
                throw new FormatException(string.Format(CultureInfo.InvariantCulture, "Unexpected header format. {0} header is expected. Actual header: {1}", headerName, line));
            }

            if (parts[0] != headerName)
            {
                throw new FormatException(string.Format(CultureInfo.InvariantCulture, "{0} header is expected. Actual header: {1}", headerName, line));
            }

            return parts[1].Trim();
        }

        private static Uri ReadAddress(TextReader reader)
        {
            string addressLine = reader.ReadLine();
            if (string.IsNullOrEmpty(addressLine))
            {
                throw new FormatException("Transmission address is expected.");
            }

            var address = new Uri(addressLine);
            return address;
        }

        private static byte[] ReadContent(TextReader reader)
        {
            string content = reader.ReadToEnd();
            if (string.IsNullOrEmpty(content) || content == Environment.NewLine)
            {
                throw new FormatException("Content is expected.");
            }

            return Convert.FromBase64String(content);
        }
    }
}