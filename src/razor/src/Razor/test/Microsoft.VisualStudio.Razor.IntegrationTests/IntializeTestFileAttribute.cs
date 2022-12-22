// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Reflection;
using Xunit.Sdk;

namespace Microsoft.VisualStudio.Razor.IntegrationTests;

public class IntializeTestFileAttribute : BeforeAfterTestAttribute
{
    public override void Before(MethodInfo methodUnderTest)
    {
        var typeName = methodUnderTest.ReflectedType.Name;
        if (typeof(RazorSemanticTokensTests).GetTypeInfo().IsAssignableFrom(methodUnderTest.DeclaringType.GetTypeInfo()))
        {
            RazorSemanticTokensTests.FileName = $"Semantic/TestFiles/{typeName}/{methodUnderTest.Name}";
        }
    }

    public override void After(MethodInfo methodUnderTest)
    {
        if (typeof(RazorSemanticTokensTests).GetTypeInfo().IsAssignableFrom(methodUnderTest.DeclaringType.GetTypeInfo()))
        {
            RazorSemanticTokensTests.FileName = null;
        }
    }
}
