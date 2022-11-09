using System.Runtime.CompilerServices;

namespace Microsoft.DotNet.SwaggerGenerator.Modeler
{
    public abstract class TypeReference
    {
        private static readonly ConditionalWeakTable<TypeReference, TypeReference> ArrayTypeReferences =
            new ConditionalWeakTable<TypeReference, TypeReference>();

        private static readonly ConditionalWeakTable<TypeModel, TypeReference> TypeModelReferences =
            new ConditionalWeakTable<TypeModel, TypeReference>();

        public static readonly TypeReference Boolean = new PrimitiveTypeReference("boolean");
        public static readonly TypeReference Int32 = new PrimitiveTypeReference("int32");
        public static readonly TypeReference Int64 = new PrimitiveTypeReference("int64");
        public static readonly TypeReference Float = new PrimitiveTypeReference("float");
        public static readonly TypeReference Double = new PrimitiveTypeReference("double");
        public static readonly TypeReference String = new PrimitiveTypeReference("string");
        public static readonly TypeReference Byte = new PrimitiveTypeReference("byte");
        public static readonly TypeReference Date = new PrimitiveTypeReference("date");
        public static readonly TypeReference DateTime = new PrimitiveTypeReference("date-time");
        public static readonly TypeReference Uuid = new PrimitiveTypeReference("uuid");
        public static readonly TypeReference File = new PrimitiveTypeReference("file");
        public static readonly TypeReference Void = new PrimitiveTypeReference("void");
        public static readonly TypeReference Any = new PrimitiveTypeReference("any");

        public abstract string DisplayString { get; }

        public override string ToString()
        {
            return DisplayString;
        }

        public static TypeReference Array(TypeReference baseType)
        {
            if (!ArrayTypeReferences.TryGetValue(baseType, out TypeReference reference))
            {
                reference = new ArrayTypeReference(baseType);
                ArrayTypeReferences.Add(baseType, reference);
            }

            return reference;
        }

        public static TypeReference Object(TypeModel typeModel)
        {
            if (!TypeModelReferences.TryGetValue(typeModel, out TypeReference reference))
            {
                reference = new TypeModelReference(typeModel);
                TypeModelReferences.Add(typeModel, reference);
            }

            return reference;
        }

        public static TypeReference Constant(string value)
        {
            return new ConstantTypeReference(value);
        }

        public static TypeReference Dictionary(TypeReference valueType)
        {
            return new DictionaryTypeReference(valueType);
        }

        public class ConstantTypeReference : TypeReference
        {
            public ConstantTypeReference(string value)
            {
                Value = value;
            }

            public string Value { get; set; }

            public override string DisplayString => "constant: " + Value;
        }

        public class TypeModelReference : TypeReference
        {
            public TypeModelReference(TypeModel model)
            {
                Model = model;
            }

            public TypeModel Model { get; }
            public override string DisplayString => Model.Name;
        }

        public class DictionaryTypeReference : TypeReference
        {
            public DictionaryTypeReference(TypeReference valueType)
            {
                ValueType = valueType;
                DisplayString = $"Dictionary<string,{valueType.DisplayString}>";
            }

            public TypeReference ValueType { get; }
            public override string DisplayString { get; }
        }

        public class ArrayTypeReference : TypeReference
        {
            public ArrayTypeReference(TypeReference baseType)
            {
                BaseType = baseType;
                DisplayString = baseType.DisplayString + "[]";
            }

            public TypeReference BaseType { get; }
            public override string DisplayString { get; }
        }

        public class PrimitiveTypeReference : TypeReference
        {
            public PrimitiveTypeReference(string displayString)
            {
                DisplayString = displayString;
            }

            public override string DisplayString { get; }
        }
    }
}
