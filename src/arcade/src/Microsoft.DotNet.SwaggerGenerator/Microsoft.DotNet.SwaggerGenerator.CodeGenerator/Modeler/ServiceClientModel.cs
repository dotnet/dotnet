using System.Collections.Generic;
using System.Collections.Immutable;

namespace Microsoft.DotNet.SwaggerGenerator.Modeler
{
    public class ServiceClientModel
    {
        public ServiceClientModel(
            string clientName,
            string @namespace,
            string baseUrl,
            IEnumerable<TypeModel> types,
            IEnumerable<MethodGroupModel> methodGroups)
        {
            Name = clientName;
            Namespace = @namespace;
            BaseUrl = baseUrl;
            Types = types.ToImmutableList();
            MethodGroups = methodGroups.ToImmutableList();
        }

        public IImmutableList<TypeModel> Types { get; }

        public IImmutableList<MethodGroupModel> MethodGroups { get; }

        public string BaseUrl { get; }

        public string Name { get; }
        public string Namespace { get; }
    }
}
