// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using System.Security.Cryptography;

namespace NuGet.Packaging.Signing.DerEncoding
{
    /// <remarks>This is public only to facilitate testing.</remarks>
    public sealed class DerGeneralizedTime
    {
        public DateTime DateTime { get; }

        private DerGeneralizedTime(DateTime datetime)
        {
            DateTime = datetime;
        }

        public static DerGeneralizedTime Read(string decodedTime)
        {
            // YYYYMMDDhhmmssZ
            var minimumValidLength = 15;

            if (string.IsNullOrEmpty(decodedTime) || decodedTime.Length < minimumValidLength)
            {
                throw new CryptographicException(SR.Cryptography_Der_Invalid_Encoding);
            }

            if (decodedTime[decodedTime.Length - 1] != 'Z')
            {
                throw new CryptographicException(SR.Cryptography_Der_Invalid_Encoding);
            }

#if NETCOREAPP
            var decimalIndex = decodedTime.IndexOf('.', StringComparison.Ordinal);
#else
            var decimalIndex = decodedTime.IndexOf('.');
#endif
            var stringToParse = decodedTime;
            string format;

            if (decimalIndex == -1)
            {
                format = "yyyyMMddHHmmssZ";
            }
            else
            {
                // 20180214095812.3456789Z
                //                ^-----^
                var fractionalSecondDigits = decodedTime.Length - 2 - decimalIndex;

                // Disallow decimal with no following digits
                if (fractionalSecondDigits < 1)
                {
                    throw new CryptographicException(SR.Cryptography_Der_Invalid_Encoding);
                }

                // Disallow trailing zero
                if (decodedTime[decodedTime.Length - 2] == '0')
                {
                    throw new CryptographicException(SR.Cryptography_Der_Invalid_Encoding);
                }

                if (fractionalSecondDigits > 7)
                {
                    // DateTime is precise to 1 ten-millionth of a second or 7 digits after the decimal.
                    // If decodedTime precision is greater than this, ignore the trailing digits.
                    // The length of the maximum format string minus the trailing "Z" is 22 characters:
                    //
                    //     20180214095812.3456789Z
                    //     ^--------------------^
                    stringToParse = $"{decodedTime.Substring(0, 22)}Z";
                    fractionalSecondDigits = 7;
                }

                format = $"yyyyMMddHHmmss.{new string('F', fractionalSecondDigits)}Z";
            }

            DateTime datetime;

            if (DateTime.TryParseExact(
                stringToParse,
                format,
                CultureInfo.InvariantCulture,
                DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal,
                out datetime))
            {
                return new DerGeneralizedTime(datetime);
            }

            throw new CryptographicException(SR.Cryptography_Der_Invalid_Encoding);
        }
    }
}
