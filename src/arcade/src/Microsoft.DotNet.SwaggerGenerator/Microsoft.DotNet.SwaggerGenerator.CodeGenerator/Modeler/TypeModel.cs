namespace Microsoft.DotNet.SwaggerGenerator.Modeler
{
    public abstract class TypeModel
    {
        public abstract string Name { get; }
        public abstract string Namespace { get; }
        public abstract bool IsEnum { get; }

        public override string ToString()
        {
            return $"{Namespace}.{Name}";
        }
    }
}
