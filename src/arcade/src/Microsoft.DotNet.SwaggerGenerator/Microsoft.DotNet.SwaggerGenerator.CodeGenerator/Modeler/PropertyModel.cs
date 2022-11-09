namespace Microsoft.DotNet.SwaggerGenerator.Modeler
{
    public class PropertyModel
    {
        public PropertyModel(string name, bool required, bool readOnly, TypeReference type)
        {
            Name = name;
            Required = required;
            ReadOnly = readOnly;
            Type = type;
        }

        public string Name { get; }
        public bool Required { get; }
        public bool ReadOnly { get; }
        public TypeReference Type { get; }

        public override string ToString()
        {
            return $"{Type} {Name}; Required={Required}, ReadOnly={ReadOnly}";
        }
    }
}
