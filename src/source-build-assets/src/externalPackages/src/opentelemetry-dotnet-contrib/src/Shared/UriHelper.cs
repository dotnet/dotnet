// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

namespace OpenTelemetry.Trace;

internal static class UriHelper
{
    private const string RedactedText = "REDACTED";

    public static Uri ScrubUserInfo(Uri uri)
    {
        var uriBuilder = new UriBuilder(uri);
        if (!string.IsNullOrEmpty(uriBuilder.UserName))
        {
            uriBuilder.UserName = RedactedText;
        }

        if (!string.IsNullOrEmpty(uriBuilder.Password))
        {
            uriBuilder.Password = RedactedText;
        }

        return uriBuilder.Uri;
    }
}
