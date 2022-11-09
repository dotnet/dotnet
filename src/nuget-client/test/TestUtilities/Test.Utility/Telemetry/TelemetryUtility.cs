// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;

namespace Test.Utility.Telemetry
{
    public static class TelemetryUtility
    {
        public static void VerifyDateTimeFormat(string datetime)
        {
            const string format = "yyyy-MM-ddTHH:mm:ss.fffffffZ";

            DateTime.ParseExact(
                datetime,
                format,
                DateTimeFormatInfo.InvariantInfo,
                DateTimeStyles.RoundtripKind);
        }
    }
}
