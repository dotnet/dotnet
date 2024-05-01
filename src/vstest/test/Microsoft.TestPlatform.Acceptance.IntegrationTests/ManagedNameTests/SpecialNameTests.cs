// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;
using System.Reflection;

using Microsoft.TestPlatform.AcceptanceTests;
using Microsoft.TestPlatform.AdapterUtilities.ManagedNameUtilities;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.Acceptance.IntegrationTests.ManagedNameTests;

[TestClass]
[TestCategory("Windows")]
[TestCategory("AcceptanceTests")]
public class SpecialNameTests : AcceptanceTestBase
{
    [TestMethod]
    public void VerifyThatInvalidIdentifierNamesAreParsed()
    {
        var asset = _testEnvironment.GetTestAsset("CILProject.dll", "net462");
        var assembly = Assembly.LoadFrom(asset);
        var types = assembly.GetTypes();

        // Some methods can be implemented directly on the module, and are not in any class. This is how we get them.
        var modules = assembly.GetModules().Select(m => new { Module = m, Type = m.GetType() });
        var moduleTypes = modules.Select(m =>
        {
            var runtimeTypeProperty = m.Type.GetProperty("RuntimeType", BindingFlags.Instance | BindingFlags.NonPublic);
            var moduleType = (Type?)runtimeTypeProperty!.GetValue(m.Module, null)!;

            return moduleType;
        });

        foreach (var type in types.Concat(moduleTypes).Where(t => t != null))
        {
            var methods = type!.GetMethods();

            foreach (var method in methods)
            {
                // Module methods have null Declaring type.
                if (method.DeclaringType != null && method.DeclaringType != type) continue;

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
