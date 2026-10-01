// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Test.Utility;

namespace NuGet.XPlat.FuncTest
{
    internal sealed class StagePushTestServer : IDisposable
    {
        private readonly string? _expectedApiKey;
        private readonly bool _advertiseStagingResource;
        private readonly MockServer _server;

        public StagePushTestServer(
            string? expectedApiKey = null,
            bool advertiseStagingResource = true)
        {
            _expectedApiKey = expectedApiKey;
            _advertiseStagingResource = advertiseStagingResource;
            _server = new MockServer();
            _server.Get.Add("/v3/index.json", _ => CreateServiceIndex());
            _server.Put.Add("/staging/package", request => CaptureRequest("package", request, PackageStatusCode));
            _server.Put.Add("/staging/symbols", request => CaptureRequest("symbols", request, SymbolsStatusCode));
            _server.Start();
        }

        public string SourceUrl => $"{_server.Uri}v3/index.json";

        public string StagingUrl => $"{_server.Uri}staging/";

        public HttpStatusCode PackageStatusCode { get; set; } = HttpStatusCode.Created;

        public HttpStatusCode SymbolsStatusCode { get; set; } = HttpStatusCode.Created;

        public IReadOnlyList<StagePushRequest> Requests => _requests;

        private List<StagePushRequest> _requests { get; } = [];

        public void Dispose()
        {
            _server.Dispose();
        }

        private string CreateServiceIndex()
        {
            string resources = _advertiseStagingResource
                ? $$"""
                    {
                      "@id": "{{StagingUrl}}",
                      "@type": "PackageStaging/1.0.0"
                    }
                    """
                : string.Empty;

            return $$"""
                {
                  "version": "3.0.0",
                  "resources": [
                    {{resources}}
                  ]
                }
                """;
        }

        private HttpStatusCode CaptureRequest(
            string route,
            HttpListenerRequest request,
            HttpStatusCode statusCode)
        {
            string? apiKey = request.Headers["X-NuGet-ApiKey"];
            byte[] body;
            using (var stream = new MemoryStream())
            {
                request.InputStream.CopyTo(stream);
                body = stream.ToArray();
            }

            (string? formFieldName, string? fileName, string? fileContent, string? groupId) =
                ReadMultipartRequest(request.ContentType, body);

            _requests.Add(new StagePushRequest(
                Route: route,
                ApiKey: apiKey,
                GroupId: groupId,
                FormFieldName: formFieldName,
                FileName: fileName,
                FileContent: fileContent));

            if (_expectedApiKey is not null
                && !string.Equals(apiKey, _expectedApiKey, StringComparison.Ordinal))
            {
                return HttpStatusCode.Forbidden;
            }

            return statusCode;
        }

        private static (string? FormFieldName, string? FileName, string? FileContent, string? GroupId)
            ReadMultipartRequest(string? contentType, byte[] body)
        {
            const string boundaryPrefix = "multipart/form-data; boundary=";
            if (contentType is null || !contentType.StartsWith(boundaryPrefix, StringComparison.Ordinal))
            {
                return default;
            }

            string boundary = contentType[boundaryPrefix.Length..].Trim('"');
            string bodyText = Encoding.UTF8.GetString(body);
            string boundaryDelimiter = "--" + boundary;
            string[] sections = bodyText.Split(
                [boundaryDelimiter],
                StringSplitOptions.RemoveEmptyEntries);
            string? formFieldName = null;
            string? fileName = null;
            string? fileContent = null;
            string? groupId = null;

            foreach (string section in sections)
            {
                int headersEnd = section.IndexOf("\r\n\r\n", StringComparison.Ordinal);
                if (headersEnd < 0)
                {
                    continue;
                }

                string headers = section[..headersEnd];
                string content = section[(headersEnd + 4)..].TrimEnd('\r', '\n', '-');
                string? name = GetContentDispositionValue(headers, "name");
                string? currentFileName = GetContentDispositionValue(headers, "filename");

                if (currentFileName is not null)
                {
                    formFieldName = name;
                    fileName = currentFileName;
                    fileContent = content;
                }
                else if (string.Equals(name, "groupId", StringComparison.Ordinal))
                {
                    groupId = content;
                }
            }

            return (formFieldName, fileName, fileContent, groupId);
        }

        private static string? GetContentDispositionValue(string headers, string parameterName)
        {
            string? contentDisposition = headers
                .Split(["\r\n"], StringSplitOptions.RemoveEmptyEntries)
                .FirstOrDefault(header => header.StartsWith("Content-Disposition:", StringComparison.OrdinalIgnoreCase));
            if (contentDisposition is null)
            {
                return null;
            }

            string prefix = parameterName + "=";
            string? parameter = contentDisposition
                .Split(';')
                .Select(value => value.Trim())
                .FirstOrDefault(value => value.StartsWith(prefix, StringComparison.OrdinalIgnoreCase));

            return parameter is null
                ? null
                : parameter[prefix.Length..].Trim('"');
        }
    }

    internal sealed record StagePushRequest(
        string Route,
        string? ApiKey,
        string? GroupId,
        string? FormFieldName,
        string? FileName,
        string? FileContent);
}
