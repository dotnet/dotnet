using System;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Interfaces;
using Microsoft.OpenApi.Writers;

namespace Microsoft.DotNet.SwaggerGenerator.Modeler
{
    public class PaginatedOpenApiExtension : IOpenApiExtension
    {
        private OpenApiObject _value;

        public PaginatedOpenApiExtension(OpenApiObject value)
        {
            _value = value;
        }

        public string PageParameterName
        {
            get => ((OpenApiString) _value["page"]).Value;
            set => _value["page"] = new OpenApiString(value);
        }

        public string PageSizeParameterName
        {
            get => ((OpenApiString) _value["pageSize"]).Value;
            set => _value["pageSize"] = new OpenApiString(value);
        }

        public static IOpenApiExtension Parse(IOpenApiAny value, OpenApiSpecVersion version)
        {
            if (value.AnyType != AnyType.Object)
            {
                throw new ArgumentException("x-ms-paginated extension only accepts an object");
            }
            var obj = (OpenApiObject) value;
            return new PaginatedOpenApiExtension(obj);
        }

        public void Write(IOpenApiWriter writer, OpenApiSpecVersion specVersion)
        {
            _value.Write(writer, specVersion);
        }
    }
}