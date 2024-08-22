using System;

namespace API.Test
{
    public static class Utils
    {
        public static void ThrowStringArgException(string value, string paramName)
        {
            if (string.IsNullOrEmpty(paramName))
            {
                throw new ArgumentException("string cannot be null or empty", nameof(paramName));
            }

            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("string cannot be null or empty", paramName);
            }
        }

        public static string GetNewGUID()
        {
            return Guid.NewGuid().ToString("d").Substring(0, 4).Replace("-", "");
        }
    }
}
