// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reflection;

using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestPlatform.Common.UnitTests.Utilities;

[TestClass]
public class AssemblyResolveEventArgsTests
{
    [TestMethod]
    public void SingleArgConstructorSetsNameAndLeavesRequestingAssemblyNull()
    {
        var args = new AssemblyResolveEventArgs("MyAssembly, Version=1.0.0.0");

        Assert.AreEqual("MyAssembly, Version=1.0.0.0", args.Name);
        Assert.IsNull(args.RequestingAssembly);
    }

    [TestMethod]
    public void TwoArgConstructorSetsNameAndRequestingAssembly()
    {
        Assembly requesting = typeof(AssemblyResolveEventArgsTests).Assembly;
        var args = new AssemblyResolveEventArgs("MyAssembly, Version=1.0.0.0", requesting);

        Assert.AreEqual("MyAssembly, Version=1.0.0.0", args.Name);
        Assert.AreSame(requesting, args.RequestingAssembly);
    }

    [TestMethod]
    public void TwoArgConstructorWithNullRequestingAssemblyLeavesPropertyNull()
    {
        var args = new AssemblyResolveEventArgs("MyAssembly, Version=1.0.0.0", requestingAssembly: null);

        Assert.AreEqual("MyAssembly, Version=1.0.0.0", args.Name);
        Assert.IsNull(args.RequestingAssembly);
    }
}
