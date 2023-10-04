// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Editor.Razor.Logging;
using Microsoft.VisualStudio.Shell;
using Xunit.Sdk;

namespace Microsoft.VisualStudio.Razor.IntegrationTests;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public class LogIntegrationTestAttribute : BeforeAfterTestAttribute
{
    public override void Before(MethodInfo methodUnderTest)
    {
        GetLogger().LogInformation("#### Integration test start: {method}", methodUnderTest.Name);
    }

    public override void After(MethodInfo methodUnderTest)
    {
        GetLogger().LogInformation("#### Integration test end: {method}", methodUnderTest.Name);
    }

    private static IOutputWindowLogger GetLogger()
    {
        var componentModel = ServiceProvider.GlobalProvider.GetService<SComponentModel, IComponentModel>();
        var logger = componentModel.GetService<IOutputWindowLogger>();
        return logger;
    }
}
