using System;
using System.Linq;
using Microsoft.DotNet.SwaggerGenerator.Modeler;

namespace Microsoft.DotNet.SwaggerGenerator
{
    internal static class DefaultHelpers
    {
        [HelperMethod]
        public static bool Or(params bool[] arguments)
        {
            return arguments.Any(arg => arg);
        }

        [HelperMethod]
        public static bool And(params bool[] arguments)
        {
            return arguments.All(arg => arg);
        }

        [HelperMethod]
        public static bool Not(bool value)
        {
            return !value;
        }

        [HelperMethod]
        public static string PascalCase(string value)
        {
            return Helpers.PascalCase(value.AsSpan());
        }

        [HelperMethod]
        public static string CamelCase(string value)
        {
            return Helpers.CamelCase(value.AsSpan());
        }

        [HelperMethod]
        public static string UpperCase(string value)
        {
            return value.ToUpperInvariant();
        }

        [HelperMethod]
        public static string LowerCase(string value)
        {
            return value.ToLowerInvariant();
        }

        [HelperMethod]
        public static bool And(bool left, bool right)
        {
            return left && right;
        }

        [HelperMethod]
        public static bool Or(bool left, bool right)
        {
            return left || right;
        }

        [HelperMethod]
        public static bool IsBlob(TypeReference type)
        {
            return type == TypeReference.File;
        }

        [HelperMethod]
        public static bool IsVoid(TypeReference type)
        {
            return type == TypeReference.Void;
        }
    }
}
