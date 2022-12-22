using System;
using System.Reflection.Metadata;
using SRMetadataReader = System.Reflection.Metadata.MetadataReader;
using SRPrimitiveTypeCode = System.Reflection.Metadata.PrimitiveTypeCode;

namespace Microsoft.Cci.Extensions
{
    public class CustomAttributeTypeProvider : ICustomAttributeTypeProvider<string>
    {
        public string GetSystemType()
        {
            return "[System.Runtime]System.Type";
        }

        public bool IsSystemType(string type)
        {
            return type == "[System.Runtime]System.Type"  // encountered as typeref
                || Type.GetType(type) == typeof(Type);    // encountered as serialized to reflection notation
        }

        public string GetTypeFromSerializedName(string name)
        {
            return name;
        }

        public string GetPrimitiveType(SRPrimitiveTypeCode typeCode)
        {
            switch (typeCode)
            {
                case SRPrimitiveTypeCode.Boolean:
                    return "bool";

                case SRPrimitiveTypeCode.Byte:
                    return "uint8";

                case SRPrimitiveTypeCode.Char:
                    return "char";

                case SRPrimitiveTypeCode.Double:
                    return "float64";

                case SRPrimitiveTypeCode.Int16:
                    return "int16";

                case SRPrimitiveTypeCode.Int32:
                    return "int32";

                case SRPrimitiveTypeCode.Int64:
                    return "int64";

                case SRPrimitiveTypeCode.IntPtr:
                    return "native int";

                case SRPrimitiveTypeCode.Object:
                    return "object";

                case SRPrimitiveTypeCode.SByte:
                    return "int8";

                case SRPrimitiveTypeCode.Single:
                    return "float32";

                case SRPrimitiveTypeCode.String:
                    return "string";

                case SRPrimitiveTypeCode.TypedReference:
                    return "typedref";

                case SRPrimitiveTypeCode.UInt16:
                    return "uint16";

                case SRPrimitiveTypeCode.UInt32:
                    return "uint32";

                case SRPrimitiveTypeCode.UInt64:
                    return "uint64";

                case SRPrimitiveTypeCode.UIntPtr:
                    return "native uint";

                case SRPrimitiveTypeCode.Void:
                    return "void";

                default:
                    throw new ArgumentOutOfRangeException(nameof(typeCode));
            }
        }

        public string GetSZArrayType(string elementType)
        {
            return elementType + "[]";
        }

        public SRPrimitiveTypeCode GetUnderlyingEnumType(string type) => default; // We only use this for compiler attributes that take a primitive type as a parameter.

        public string GetTypeFromDefinition(SRMetadataReader reader, TypeDefinitionHandle handle, byte rawTypeKind = 0) => null; // We only use this for compiler attributes that take a primitive type as a parameter.

        public string GetTypeFromReference(SRMetadataReader reader, TypeReferenceHandle handle, byte rawTypeKind = 0) => null; // We only use this for compiler attributes that take a primitive type as a parameter.
    }
}
