// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace Microsoft.VisualStudio.Razor.IntegrationTests;

public static class WellKnownProjectTemplates
{
    public const string BlazorProject = "Microsoft.WAP.CSharp.ASPNET.Blazor";

    public static class GroupIdentifiers
    {
        public const string Server = "Microsoft.Web.Blazor.Server";
        public const string Wasm = "Microsoft.Web.Blazor.Wasm";
    }

    public static class TemplateIdentifiers
    {
        public const string Server31 = "Microsoft.Web.Blazor.Server.CSharp.3.1";
        public const string Server50 = "Microsoft.Web.Blazor.Server.CSharp.5.0";
        public const string Server60 = "Microsoft.Web.Blazor.Server.CSharp.6.0";
    }
}
