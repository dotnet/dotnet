// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using System.Text;

namespace Test.Utility.Signing
{
    public static class DerGeneralizedTimeUtility
    {
        public static string ToDerGeneralizedTimeString(DateTimeOffset datetimeOffset)
        {
            var utc = datetimeOffset.UtcDateTime;
            var stringBuilder = new StringBuilder();

            stringBuilder.Append(utc.ToString("yyyyMMddHHmmss", CultureInfo.CurrentCulture));

            var fractionalSeconds = utc.TimeOfDay.ToString("FFFFFFF", CultureInfo.CurrentCulture);

            if (!string.IsNullOrEmpty(fractionalSeconds))
            {
                stringBuilder.Append(".");
                stringBuilder.Append(fractionalSeconds);
            }

            stringBuilder.Append("Z");

            return stringBuilder.ToString();
        }
    }
}
