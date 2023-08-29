// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.DotNet.ScenarioTests.SdkTemplateTests;

public static class DotNetTemplateExtensions
{
    public static string GetName(this DotNetSdkTemplate template) => Enum.GetName(template)?.ToLowerInvariant() ?? throw new NotSupportedException();

    public static bool IsAspNetCore(this DotNetSdkTemplate template) =>
        template == DotNetSdkTemplate.Web
        || template == DotNetSdkTemplate.Mvc
        || template == DotNetSdkTemplate.WebApi
        || template == DotNetSdkTemplate.Razor
        || template == DotNetSdkTemplate.BlazorWasm
        || template == DotNetSdkTemplate.BlazorServer
        || template == DotNetSdkTemplate.Worker
        || template == DotNetSdkTemplate.Angular;

    public static bool isUIApp(this DotNetSdkTemplate template) =>
        template == DotNetSdkTemplate.Wpf
        || template == DotNetSdkTemplate.Winforms
        || template == DotNetSdkTemplate.WebApp;
}
