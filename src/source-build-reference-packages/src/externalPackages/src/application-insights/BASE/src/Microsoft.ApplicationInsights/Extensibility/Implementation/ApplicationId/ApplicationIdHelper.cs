namespace Microsoft.ApplicationInsights.Extensibility.Implementation.ApplicationId
{
    using System.Globalization;
    using System.Text;
    using System.Text.RegularExpressions;

    internal static class ApplicationIdHelper
    {
        /// <summary>
        /// special string which describes that ID was taken from Breeze.
        /// </summary>
        private const string Format = "cid-v1:{0}";

        /// <summary>
        /// Max length of Application Id allowed in response from Breeze.
        /// </summary>
        private const int ApplicationIdMaxLength = 50;

        /// <summary>
        /// Format an Application Id string (ex: 00000000-0000-0000-0000-000000000000) 
        /// as (ex: cid-v1:00000000-0000-0000-0000-000000000000).
        /// </summary>
        /// <param name="applicationId">Application Id is expected to be a Guid string.</param>
        /// <remarks>
        /// To protect against injection attacks, Application Id will be truncated to a maximum length.
        /// Application Ids are expected to Http Header safe, and all non-ASCII characters will be removed.
        /// </remarks>
        internal static string ApplyFormatting(string applicationId)
        {
            // Arbitrary maximum length to guard against injections.
            applicationId = EnforceMaxLength(applicationId, ApplicationIdMaxLength);
            if (string.IsNullOrWhiteSpace(applicationId))
            {
                return null;
            }

            // String must be sanitized to include only characters safe for http header.
            applicationId = SanitizeString(applicationId);

            return string.Format(CultureInfo.InvariantCulture, Format, applicationId);
        }

        /// <summary>
        /// Application Id will eventually end up in Http Headers.
        /// Remove all characters which are not header safe.
        /// </summary>
        /// <remarks>
        /// Input is expected to be a GUID. For performance, only use the Regex after an unsupported character is discovered.
        /// </remarks>
        internal static string SanitizeString(string input)
        {
            if (input == null)
            {
                return null;
            }

            foreach (var ch in input)
            {
                if (!IsCharHeaderSafe(ch))
                {
                    return Regex.Replace(input, @"[^\u0020-\u007F]", string.Empty);
                }
            }

            return input;
        }

        /// <summary>
        /// Check a strings length and trim to a max length if needed.
        /// </summary>
        private static string EnforceMaxLength(string input, int maxLength)
        {
            if (input != null && input.Length > maxLength)
            {
                input = input.Substring(0, maxLength);
            }

            return input;
        }

        /// <summary>
        /// US-ASCII characters (hex: 0x00 - 0x7F) (decimal: 0-127) (PARTIALLY ALLOWED)
        /// ASCII Extended characters (hex: 0x80 - 0xFF) (decimal: 128-255) (NOT ALLOWED)
        /// Non-Printable ASCII characters are (hex: 0x00 - 0x1F) (decimal: 0-31) (NOT ALLOWED)
        /// Printable ASCII characters are (hex: 0x20 - 0xFF) (decimal: 32-255) (PARTIALLY ALLOWED)
        /// ALLOWED characters are (hex: 0x20 - 0x7F) (decimal: 32-127).
        /// </summary>
        private static bool IsCharHeaderSafe(char ch) => (uint)(ch - 0x20) <= (0x7F - 0x20);
    }
}
