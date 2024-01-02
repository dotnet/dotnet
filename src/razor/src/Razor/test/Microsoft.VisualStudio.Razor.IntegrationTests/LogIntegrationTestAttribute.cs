// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Reflection;
using Xunit.Sdk;

namespace Microsoft.VisualStudio.Razor.IntegrationTests;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public class LogIntegrationTestAttribute : BeforeAfterTestAttribute
{
    public override void Before(MethodInfo methodUnderTest)
    {
        // GetLogger(methodUnderTest.Name).LogInformation("#### Integration test start.");
    }

    public override void After(MethodInfo methodUnderTest)
    {
        // GetLogger(methodUnderTest.Name).LogInformation("#### Integration test end.");
    }

    // private static ILogger GetLogger(string testName)
    // {
    //     var componentModel = ServiceProvider.GlobalProvider.GetService<SComponentModel, IComponentModel>();
    //     var loggerFactory = componentModel.GetService<IRazorLoggerFactory>();
    //     return loggerFactory.CreateLogger(testName);
    // }
}
