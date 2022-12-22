// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reflection;

using Microsoft.TestPlatform.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.AdapterUtilities.ManagedNameUtilities.UnitTests;

[TestClass]
[TestCategory("Windows")]
[TestCategory("AcceptanceTests")]
public class SpecialNameTests
{
    [TestMethod]
    public void VerifyThatInvalidIdentifierNamesAreParsed()
    {
        var environment = new IntegrationTestEnvironment();
        var asset = environment.GetTestAsset("CILProject.dll", "net462");
        var assembly = Assembly.LoadFrom(asset);
        var types = assembly.GetTypes();

        foreach (var type in types)
        {
            var methods = type.GetMethods();

            foreach (var method in methods)
            {
                if (method.DeclaringType != type) continue;

                ManagedNameHelper.GetManagedName(method, out var typeName, out var methodName);
                var methodInfo = ManagedNameHelper.GetMethod(assembly, typeName, methodName);
                ManagedNameHelper.GetManagedName(methodInfo, out var typeName2, out var methodName2);

                Assert.IsTrue(method == methodInfo);
                Assert.AreEqual(typeName, typeName2, $"Type parse roundtrip test failed: {method} ({typeName} != {typeName2})");
                Assert.AreEqual(methodName, methodName2, $"Method parse roundtrip test failed: {method} ({methodName} != {methodName2})");
            }
        }
    }
}
