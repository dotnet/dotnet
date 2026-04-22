// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Newtonsoft.Json;

namespace NewtonSoftDependencyMissing;

public class Account
{
    public string Email { get; set; }
    public bool Active { get; set; }
}

[TestClass]
public class UnitTest1
{
    [TestMethod]
    public void TestUsingNewtonsoftWithoutShippingDll()
    {
        // This test uses Newtonsoft.Json but the project excludes the runtime asset.
        // At runtime, Newtonsoft.Json.dll is NOT present next to this assembly,
        // so vstest's own copy is used. This should be tracked in telemetry.
        string json = @"{""Email"": ""john@example.com"", ""Active"": true}";

        Account account = JsonConvert.DeserializeObject<Account>(json);

        Assert.IsNotNull(account);
        Assert.AreEqual("john@example.com", account.Email);
    }
}
