// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RouteTests.TestApplication;

internal class ActionDescriptorInfo
{
    public ActionDescriptorInfo()
    {
    }

    public ActionDescriptorInfo(ActionDescriptor actionDescriptor)
    {
        this.AttributeRouteInfo = actionDescriptor.AttributeRouteInfo?.Template;

        this.ActionParameters = [];
        foreach (var item in actionDescriptor.Parameters)
        {
            this.ActionParameters.Add(item.Name);
        }

        if (actionDescriptor is PageActionDescriptor pad)
        {
            this.PageActionDescriptorSummary = new PageActionDescriptorInfo(pad.RelativePath, pad.ViewEnginePath);
        }

        if (actionDescriptor is ControllerActionDescriptor cad)
        {
            this.ControllerActionDescriptorSummary = new ControllerActionDescriptorInfo(cad.ControllerName, cad.ActionName);
        }
    }

    [JsonPropertyName("AttributeRouteInfo.Template")]
    public string? AttributeRouteInfo { get; set; }

    [JsonPropertyName("Parameters")]
#pragma warning disable CA2227
    public IList<string>? ActionParameters { get; set; }
#pragma warning restore CA2227

    [JsonPropertyName("ControllerActionDescriptor")]
    public ControllerActionDescriptorInfo? ControllerActionDescriptorSummary { get; set; }

    [JsonPropertyName("PageActionDescriptor")]
    public PageActionDescriptorInfo? PageActionDescriptorSummary { get; set; }
}
