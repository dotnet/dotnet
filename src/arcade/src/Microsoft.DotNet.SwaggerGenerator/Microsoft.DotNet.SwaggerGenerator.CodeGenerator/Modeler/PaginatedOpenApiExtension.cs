// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Text.Json.Nodes;
using Microsoft.OpenApi;

namespace Microsoft.DotNet.SwaggerGenerator.Modeler
{
    public class PaginatedOpenApiExtension : IOpenApiExtension
    {
        private JsonObject _value;

        public PaginatedOpenApiExtension(JsonObject value)
        {
            _value = value;
        }

        public string PageParameterName
        {
            get => _value["page"]?.GetValue<string>();
            set => _value["page"] = JsonValue.Create(value);
        }

        public string PageSizeParameterName
        {
            get => _value["pageSize"]?.GetValue<string>();
            set => _value["pageSize"] = JsonValue.Create(value);
        }

        public static IOpenApiExtension Parse(JsonNode value, OpenApiSpecVersion version)
        {
            if (value is not JsonObject obj)
            {
                throw new ArgumentException("x-ms-paginated extension only accepts an object");
            }
            return new PaginatedOpenApiExtension(obj);
        }

        public void Write(IOpenApiWriter writer, OpenApiSpecVersion specVersion)
        {
            writer.WriteAny(_value);
        }
    }
}
