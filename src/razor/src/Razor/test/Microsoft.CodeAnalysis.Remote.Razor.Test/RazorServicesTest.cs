// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CodeAnalysis.Remote.Razor;
using Roslyn.Test.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.CodeAnalysis.Razor.Remote;

public class RazorServicesTest(ITestOutputHelper testOutputHelper) : ToolingTestBase(testOutputHelper)
{
    private readonly static XmlDocument s_servicesFile = LoadServicesFile();

    [Theory]
    [MemberData(nameof(MessagePackServices))]
    public void MessagePackServicesAreListedProperly(Type serviceType, Type? callbackType)
    {
        VerifyService(serviceType, callbackType);
    }

    [Theory]
    [MemberData(nameof(JsonServices))]
    public void JsonServicesAreListedProperly(Type serviceType, Type? callbackType)
    {
        Assert.True(typeof(IRemoteJsonService).IsAssignableFrom(serviceType));
        VerifyService(serviceType, callbackType);
    }

    [Fact]
    public void RazorServicesContainsAllServices()
    {
        var services = new HashSet<string>(RazorServices.MessagePackServices.Select(s => s.Item1.Name));
        services.UnionWith(RazorServices.JsonServices.Select(s => s.Item1.Name));
        var serviceNodes = s_servicesFile.SelectNodes("/Project/ItemGroup/ServiceHubService");
        Assert.NotNull(serviceNodes);
        foreach (XmlNode serviceNode in serviceNodes)
        {
            Assert.NotNull(serviceNode);
            Assert.NotNull(serviceNode.Attributes);

            var serviceEntry = serviceNode.Attributes["Include"]!.Value;
            var factoryName = serviceNode.Attributes["ClassName"]!.Value;

            var factoryType = typeof(ServiceArgs).Assembly.GetType(factoryName);
            AssertEx.NotNull(factoryType, $"Could not load type for factory '{factoryName}'");

            var interfaceType = factoryType.BaseType!.GetGenericArguments()[0];
            Assert.True(services.Contains(interfaceType.Name), $"Service '{interfaceType.Name}' is not listed in RazorServices");
        }
    }

    public static IEnumerable<object?[]> MessagePackServices()
    {
        foreach (var service in RazorServices.MessagePackServices)
        {
            yield return [service.Item1, service.Item2];
        }
    }

    public static IEnumerable<object?[]> JsonServices()
    {
        foreach (var service in RazorServices.JsonServices)
        {
            yield return [service.Item1, service.Item2];
        }
    }

    private static XmlDocument LoadServicesFile()
    {
        var document = new XmlDocument();
        document.Load(Path.Combine(TestProject.GetRepoRoot(), "eng", "targets", "Services.props"));
        return document;
    }

    private static void VerifyService(Type serviceType, Type? callbackType)
    {
        const string prefix = "IRemote";
        const string suffix = "Service";

        Assert.Null(callbackType);

        var serviceName = serviceType.Name;
        Assert.StartsWith(prefix, serviceName);
        Assert.EndsWith(suffix, serviceName);

        var shortName = serviceName.Substring(prefix.Length, serviceName.Length - prefix.Length - suffix.Length);
        var servicePropsEntry = $"Microsoft.VisualStudio.Razor.{shortName}";

        var serviceNode = s_servicesFile.SelectSingleNode($"/Project/ItemGroup/ServiceHubService[@Include='{servicePropsEntry}']");
        AssertEx.NotNull(serviceNode, $"Expected entry in Services.props for {servicePropsEntry}");

        var serviceImplName = $"Microsoft.CodeAnalysis.Remote.Razor.Remote{shortName}Service";
        var factoryName = serviceNode.Attributes!["ClassName"]!.Value;
        Assert.Equal($"{serviceImplName}+Factory", factoryName);

        var serviceImplType = typeof(ServiceArgs).Assembly.GetType(serviceImplName);
        Assert.NotNull(serviceImplType);

        var factoryType = typeof(ServiceArgs).Assembly.GetType(factoryName);
        Assert.NotNull(factoryType);

        Assert.True(serviceType.IsAssignableFrom(serviceImplType));

        var interfaceType = factoryType.BaseType!.GetGenericArguments()[0];
        Assert.Equal(serviceType, interfaceType);
    }
}
