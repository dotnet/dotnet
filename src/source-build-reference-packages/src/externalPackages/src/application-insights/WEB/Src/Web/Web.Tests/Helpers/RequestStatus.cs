// <copyright file="RequestStatus.cs" company="Microsoft">
// Copyright © Microsoft. All Rights Reserved.
// </copyright>

namespace Microsoft.ApplicationInsights.Web.Helpers
{
    internal enum RequestStatus
    {
        Success = 0,
        RequestFailed,
        ApplicationFailed
    }
}
