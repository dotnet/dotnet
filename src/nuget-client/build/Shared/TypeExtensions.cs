#if IS_NET40_CLIENT
using NuGet.Shared;

namespace System.Reflection
{
    /// <summary>
    /// Extension methods for the <see cref="Type"/> type.
    /// </summary>
    internal static class TypeExtensions
    {
        /// <summary>
        /// Expose the type itself as the type info. This is to make TypeInfo available on
        /// .NET Framework 4 Client Profile. This method is used in the automatically generated
        /// resource C# files. To avoid manual changes to an automatically generated file, expose
        /// this method as an extension method in an used namespace.
        /// </summary>
        public static TypeInfo GetTypeInfo(this Type type)
        {
            return new TypeInfo(type);
        }
    }
}
#endif
