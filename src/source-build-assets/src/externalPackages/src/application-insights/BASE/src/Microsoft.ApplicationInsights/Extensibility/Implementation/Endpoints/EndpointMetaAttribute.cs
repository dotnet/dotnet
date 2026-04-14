namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Endpoints
{
    using System;
    using System.Reflection;

    /// <summary>
    /// Defines meta data for possible endpoints.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    internal class EndpointMetaAttribute : Attribute
    {
        /// <summary>Gets or sets the explicit name for overriding an endpoint within a connection string.</summary>
        public string ExplicitName { get; set; }

        /// <summary>Gets or sets the prefix (aka subdomain) for an endpoint.</summary>
        public string EndpointPrefix { get; set; }

        /// <summary>Gets or sets the default classic endpoint.</summary>
        public string Default { get; set; }

        public static EndpointMetaAttribute GetAttribute(EndpointName enumValue)
        {
            Type type = enumValue.GetType();
            string name = Enum.GetName(type, enumValue);
            return type.GetField(name).GetCustomAttribute<EndpointMetaAttribute>();
        }
    }
}
