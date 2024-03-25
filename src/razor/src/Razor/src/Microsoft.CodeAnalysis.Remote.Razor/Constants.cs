// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace Microsoft.CodeAnalysis.Remote.Razor;

internal static class Constants
{
    /// <summary>
    /// The name we use for the "server" in cohosting, which is not really an LSP server, but we use it for telemetry to distinguish events
    /// </summary>
    public const string ExternalAccessServerName = "Razor.ExternalAccess";
}
