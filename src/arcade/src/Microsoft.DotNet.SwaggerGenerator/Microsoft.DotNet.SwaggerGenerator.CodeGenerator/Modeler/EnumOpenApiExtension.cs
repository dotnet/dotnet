// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Text.Json.Nodes;
using Microsoft.OpenApi;

namespace Microsoft.DotNet.SwaggerGenerator.Modeler
{
    public class EnumOpenApiExtension : IOpenApiExtension
    {
        private JsonObject _value;

        public EnumOpenApiExtension(JsonObject value)
        {
            _value = value;
        }

        public string Name
        {
            get => _value["name"]?.GetValue<string>();
            set => _value["name"] = JsonValue.Create(value);
        }

        public static IOpenApiExtension Parse(JsonNode value, OpenApiSpecVersion version)
        {
            if (value is not JsonObject obj)
            {
                throw new ArgumentException("x-ms-enum extension only accepts an object");
            }
            return new EnumOpenApiExtension(obj);
        }

        public void Write(IOpenApiWriter writer, OpenApiSpecVersion specVersion)
        {
            writer.WriteAny(_value);
        }
    }
}
