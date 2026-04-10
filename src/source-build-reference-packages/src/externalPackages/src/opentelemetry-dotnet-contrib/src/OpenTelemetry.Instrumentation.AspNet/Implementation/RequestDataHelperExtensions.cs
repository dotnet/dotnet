// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Web;
using OpenTelemetry.Internal;

namespace OpenTelemetry.Instrumentation.AspNet.Implementation;

internal static class RequestDataHelperExtensions
{
    public static string GetHttpProtocolVersion(HttpRequestBase request)
    {
        return RequestDataHelper.GetHttpProtocolVersion(request.ServerVariables["SERVER_PROTOCOL"]);
    }
}
