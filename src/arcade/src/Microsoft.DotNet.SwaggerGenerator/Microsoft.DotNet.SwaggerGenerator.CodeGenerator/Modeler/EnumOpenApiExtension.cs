using System;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Interfaces;
using Microsoft.OpenApi.Writers;

namespace Microsoft.DotNet.SwaggerGenerator.Modeler
{
    public class EnumOpenApiExtension : IOpenApiExtension
    {
        private OpenApiObject _value;

        public EnumOpenApiExtension(OpenApiObject value)
        {
            _value = value;
        }

        public string Name
        {
            get => ((OpenApiString) _value["name"]).Value;
            set => _value["name"] = new OpenApiString(value);
        }

        public static IOpenApiExtension Parse(IOpenApiAny value, OpenApiSpecVersion version)
        {
            if (value.AnyType != AnyType.Object)
            {
                throw new ArgumentException("x-ms-enum extension only accepts an object");
            }
            var obj = (OpenApiObject) value;
            return new EnumOpenApiExtension(obj);
        }

        public void Write(IOpenApiWriter writer, OpenApiSpecVersion specVersion)
        {
            _value.Write(writer, specVersion);
        }
    }
}
