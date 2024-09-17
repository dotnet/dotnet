using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Interfaces;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Readers;

namespace Microsoft.DotNet.SwaggerGenerator.Modeler
{
    public class ServiceClientModelFactory
    {
        public static OpenApiDocument ReadDocument(Stream docStream, out OpenApiDiagnostic diagnostic)
        {
            var reader = new OpenApiStreamReader(
                new OpenApiReaderSettings
                {
                    ReferenceResolution = ReferenceResolutionSetting.ResolveLocalReferences,
                    ExtensionParsers =
                    {
                        ["x-ms-enum"] = EnumOpenApiExtension.Parse,
                        ["x-ms-paginated"] = PaginatedOpenApiExtension.Parse,
                    },
                });

            return reader.Read(docStream, out diagnostic);
        }

        private readonly Dictionary<string, EnumTypeModel> _enumTypeModels = new Dictionary<string, EnumTypeModel>();

        private readonly GeneratorOptions _generatorOptions;

        private readonly AsyncLocal<Stack<string>> _propertyNameStack = new AsyncLocal<Stack<string>>();
        private readonly AsyncLocal<Stack<string>> _typeNameStack = new AsyncLocal<Stack<string>>();
        private readonly AsyncLocal<Stack<string>> _parameterNameStack = new AsyncLocal<Stack<string>>();
        private readonly AsyncLocal<Stack<string>> _methodNameStack = new AsyncLocal<Stack<string>>();

        private readonly Dictionary<string, TypeModel> _types = new Dictionary<string, TypeModel>();
        private OpenApiDocument _rootDocument = null;

        public ServiceClientModelFactory(GeneratorOptions options)
        {
            _generatorOptions = options;
            _typeNameStack.Value = new Stack<string>();
            _propertyNameStack.Value = new Stack<string>();
            _parameterNameStack.Value = new Stack<string>();
            _methodNameStack.Value = new Stack<string>();
        }
        private string CurrentMethodName => _methodNameStack.Value.Count != 0 ? _methodNameStack.Value.Peek() : null;

        private string CurrentParameterName => _parameterNameStack.Value.Count != 0 ? _parameterNameStack.Value.Peek() : "Value";

        private string CurrentTypeName => _typeNameStack.Value.Count != 0 ? _typeNameStack.Value.Peek() : null;

        private string CurrentPropertyName => _propertyNameStack.Value.Count != 0 ? _propertyNameStack.Value.Peek() : "Value";

        private IDisposable WithMethodName(string name)
        {
            _methodNameStack.Value.Push(name);
            return Disposable.Create(
                () =>
                {
                    if (_methodNameStack.Value.Pop() != name)
                    {
                        throw new InvalidOperationException(
                            $"Method name '{name}' popped when it wasn't the top of the stack.");
                    }
                });
        }

        private IDisposable WithParameterName(string name)
        {
            _parameterNameStack.Value.Push(name);
            return Disposable.Create(
                () =>
                {
                    if (_parameterNameStack.Value.Pop() != name)
                    {
                        throw new InvalidOperationException(
                            $"Parameter name '{name}' popped when it wasn't the top of the stack.");
                    }
                });
        }

        private IDisposable WithTypeName(string name)
        {
            _typeNameStack.Value.Push(name);
            return Disposable.Create(
                () =>
                {
                    if (_typeNameStack.Value.Pop() != name)
                    {
                        throw new InvalidOperationException(
                            $"Type name '{name}' popped when it wasn't the top of the stack.");
                    }
                });
        }

        private IDisposable WithPropertyName(string name)
        {
            _propertyNameStack.Value.Push(name);
            return Disposable.Create(
                () =>
                {
                    if (_propertyNameStack.Value.Pop() != name)
                    {
                        throw new InvalidOperationException(
                            $"Property name '{name}' popped when it wasn't the top of the stack.");
                    }
                });
        }

        public ServiceClientModel Create(OpenApiDocument document)
        {
            _rootDocument = document;
            GeneratorOptions options = _generatorOptions;
            ImmutableList<MethodGroupModel> methodGroups = document.Paths
                .SelectMany(p => p.Value.Operations.Select(o => (path: p.Key, type: o.Key, operation: o.Value)))
                .ToLookup(t => t.operation.Tags.FirstOrDefault()?.Name)
                .Select(g => CreateMethodGroupModel(g.Key, g))
                .ToImmutableList();
            return new ServiceClientModel(
                options.ClientName,
                options.Namespace,
                document.Servers?.FirstOrDefault()?.Url,
                _types.Values?.Concat(_enumTypeModels.Values).OrderBy(m => m.Name),
                methodGroups);
        }

        private MethodGroupModel CreateMethodGroupModel(
            string name,
            IEnumerable<(string path, OperationType type, OpenApiOperation operation)> operations)
        {
            var methods = new List<MethodModel>();
            foreach ((string path, OperationType type, OpenApiOperation operation) in operations)
            {
                methods.Add(CreateMethodModel(path, type, operation));
            }

            return new MethodGroupModel(name, _generatorOptions.Namespace, methods);
        }

        private MethodModel CreateMethodModel(string path, OperationType type, OpenApiOperation operation)
        {

            string name = operation.OperationId;
            string firstTag = operation.Tags.FirstOrDefault()?.Name;
            if (firstTag != null && name.StartsWith(firstTag))
            {
                name = name.Substring(firstTag.Length);
            }

            name = name.TrimStart('_');
            using (WithMethodName(name))
            {
                IList<ParameterModel> parameters = (operation.Parameters ?? Array.Empty<OpenApiParameter>())
                    .Select(CreateParameterModel)
                    .ToList();
                if (operation.RequestBody != null)
                {
                    parameters.Add(CreateParameterModel(operation.RequestBody));
                }

                TypeReference errorType = null;

                if (operation.Responses.TryGetValue("default", out OpenApiResponse errorResponse))
                {
                    errorType = ResolveTypeForResponse(errorResponse, name);
                }

                TypeReference responseType = null;
                var selectedResponse = operation.Responses.Where(r => r.Key.StartsWith("2")).Select(p => p.Value).FirstOrDefault();

                if (selectedResponse != null)
                {
                    responseType = ResolveTypeForResponse(selectedResponse, name);
                }

                if (responseType == null)
                {
                    responseType = TypeReference.Void;
                }

                PaginatedOpenApiExtension paginated = null;

                if (responseType is TypeReference.ArrayTypeReference &&
                    type == OperationType.Get &&
                    operation.Extensions.ContainsKey("x-ms-paginated"))
                {
                    paginated = operation.Extensions["x-ms-paginated"] as PaginatedOpenApiExtension;
                }

                return new MethodModel(name, path, GetHttpMethod(type), responseType, errorType, parameters, paginated);
            }
        }

        private TypeReference ResolveTypeForResponse(OpenApiResponse response, string name)
        {
            var schema = response.Content.Values.Select(t => t.Schema).FirstOrDefault(s => s != null);

            if (schema != null)
            {
                return ResolveType(schema, name);
            }

            return null;
        }

        private HttpMethod GetHttpMethod(OperationType type)
        {
            switch (type)
            {
                case OperationType.Get:
                    return HttpMethod.Get;
                case OperationType.Put:
                    return HttpMethod.Put;
                case OperationType.Post:
                    return HttpMethod.Post;
                case OperationType.Delete:
                    return HttpMethod.Delete;
                case OperationType.Options:
                    return HttpMethod.Options;
                case OperationType.Head:
                    return HttpMethod.Head;
                case OperationType.Patch:
                    return new HttpMethod("PATCH");
                default:
                    throw new NotSupportedException(type.ToString());
            }
        }

        private ParameterModel CreateParameterModel(OpenApiRequestBody body)
        {
            const string parameterName = "body"; // TODO: Get parameter name from the request body object once https://github.com/Microsoft/OpenAPI.NET/issues/378 is fixed
            using (WithParameterName(parameterName))
            {
                TypeReference type = ResolveType(body.Content["application/json"].Schema);

                return new ParameterModel(parameterName, body.Required, null, type);
            }
        }

        private ParameterModel CreateParameterModel(OpenApiParameter parameter)
        {
            using (WithParameterName(parameter.Name))
            {
                TypeReference type = ResolveType(parameter.Schema);
                return new ParameterModel(parameter.Name, parameter.Required, parameter.In, type);
            }
        }

        private TypeReference ResolveType(OpenApiSchema schema)
        {
            if (schema == null)
            {
                return TypeReference.Void;
            }

            if (schema.UnresolvedReference)
            {
                schema = (OpenApiSchema) _rootDocument.ResolveReference(schema.Reference);
            }

            string type = schema.Type;
            string format = schema.Format;
            OpenApiSchema items = schema.Items;
            IList<string> enumeration = schema.Enum.OfType<OpenApiString>().Select(s => s.Value).ToList();
            IDictionary<string, IOpenApiExtension> extensions = schema.Extensions;

            switch (type)
            {
                case "boolean":
                    return TypeReference.Boolean;
                case "integer":
                    switch (format)
                    {
                        case "int32":
                            return TypeReference.Int32;
                        case "int64":
                        default:
                            return TypeReference.Int64;
                    }
                case "number":
                    switch (format)
                    {
                        case "float":
                            return TypeReference.Float;
                        case "double":
                        default:
                            return TypeReference.Double;
                    }
                case "string":
                    if (enumeration.Any())
                    {
                        if (enumeration.Count == 1)
                        {
                            return TypeReference.Constant(enumeration[0]);
                        }

                        string enumName = schema.Reference?.Id;
                        if (extensions.TryGetValue("x-ms-enum", out IOpenApiExtension ext) && ext is EnumOpenApiExtension enumExtension)
                        {
                            enumName = enumExtension.Name;
                        }

                        if (enumName == null)
                        {
                            if (CurrentTypeName != null)
                            {
                                enumName =
                                    $"{Helpers.PascalCase(CurrentTypeName.AsSpan())}{Helpers.PascalCase(CurrentPropertyName.AsSpan())}";
                            }
                            else
                            {
                                enumName =
                                    $"{Helpers.PascalCase(CurrentMethodName.AsSpan())}{Helpers.PascalCase(CurrentParameterName.AsSpan())}";

                            }
                        }


                        return TypeReference.Object(ResolveEnumTypeModel(enumeration, enumName));
                    }

                    switch (format)
                    {
                        case "byte":
                            return TypeReference.Byte;
                        case "date":
                            return TypeReference.Date;
                        case "date-time":
                            return TypeReference.DateTime;
                        case "uuid":
                            return TypeReference.Uuid;
                        default:
                            return TypeReference.String;
                    }
                case "array":
                    return TypeReference.Array(ResolveType(items));
                case "object":
                    bool hasProperties = schema.Properties?.Count > 0;
                    bool hasAdditionalProperties = schema.AdditionalProperties != null;
                    if (!hasProperties && hasAdditionalProperties)
                    {
                        return TypeReference.Dictionary(ResolveType(schema.AdditionalProperties));
                    }

                    if (!hasProperties)
                    {
                        return TypeReference.Any;
                    }

                    return TypeReference.Object(ResolveTypeModel(schema));
                case "file":
                    return TypeReference.File;
                case null:
                    return TypeReference.Any;
                case "null":
                default:
                    throw new NotSupportedException(type);
            }
        }

        private TypeModel ResolveEnumTypeModel(IList<string> enumeration, string enumName)
        {
            if (!_enumTypeModels.TryGetValue(enumName, out EnumTypeModel value))
            {
                _enumTypeModels[enumName] = value = new EnumTypeModel(
                    enumName,
                    _generatorOptions.Namespace,
                    enumeration.Select(v => v));
            }

            return value;
        }

        private TypeReference ResolveType(OpenApiSchema schema, string operationId)
        {
            using (WithTypeName(operationId))
            using (WithPropertyName("Response"))
            {
                return ResolveType(schema);
            }
        }

        private TypeModel ResolveTypeModel(OpenApiSchema schema)
        {
            if (schema.Type != "object")
            {
                throw new ArgumentException("Schema must be object", nameof(schema));
            }

            string id = schema.Reference?.Id;
            if (id == null)
            {
                id = Helpers.PascalCase((CurrentTypeName + "-" + CurrentPropertyName).AsSpan());
            }

            if (_types.TryGetValue(id, out TypeModel model))
            {
                return model;
            }

            return _types[id] = CreateTypeModel(id, schema);
        }

        private TypeModel CreateTypeModel(string name, OpenApiSchema schema)
        {
            using (WithTypeName(name))
            {
                ISet<string> requiredProperties = schema.Required;
                IEnumerable<PropertyModel> properties = schema.Properties != null
                    ? schema.Properties.Select(
                        p =>
                        {
                            using (WithPropertyName(p.Key))
                            {
                                return CreatePropertyModel(
                                    p.Key,
                                    p.Value,
                                    requiredProperties?.Contains(p.Key) ?? false);
                            }
                        })
                    : Array.Empty<PropertyModel>();
                TypeReference additionalProperties;
                using (WithPropertyName("Items"))
                {
                    additionalProperties = schema.AdditionalProperties != null
                        ? ResolveType(schema.AdditionalProperties)
                        : null;
                }

                return new ClassTypeModel(name, _generatorOptions.Namespace, properties, additionalProperties);
            }
        }

        private PropertyModel CreatePropertyModel(string name, OpenApiSchema type, bool required)
        {
            var propertyType = ResolveType(type);
            return new PropertyModel(name, required, type.ReadOnly, propertyType);
        }
    }
}
