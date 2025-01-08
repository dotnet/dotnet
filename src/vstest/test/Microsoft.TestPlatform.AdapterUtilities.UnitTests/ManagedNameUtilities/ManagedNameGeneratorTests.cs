// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestTools.UnitTesting;

using TestClasses;

namespace Microsoft.TestPlatform.AdapterUtilities.ManagedNameUtilities.UnitTests;

[TestClass]
public class ManagedNameGeneratorTests
{
    [TestMethod]
    public void Namespaceless_ClassMembers_ShouldNotReportANamespace()
    {
        // Arrange
        var methodBase = typeof(global::NamespacelessClass).GetMethod("Method0")!;

        // Act
        ManagedNameHelper.GetManagedName(methodBase, out var managedTypeName, out var managedMethodName);

        // Assert
        Assert.AreEqual("NamespacelessClass", managedTypeName);
        Assert.AreEqual("Method0", managedMethodName);
    }

    [TestMethod]
    public void Namespaceless_RecordMembers_ShouldNotReportANamespace()
    {
        // Arrange
        var methodBase = typeof(global::NamespacelessRecord).GetMethod("Method0")!;

        // Act
        ManagedNameHelper.GetManagedName(methodBase, out var managedTypeName, out var managedMethodName);

        // Assert
        Assert.AreEqual("NamespacelessRecord", managedTypeName);
        Assert.AreEqual("Method0", managedMethodName);
    }

    [TestMethod]
    public void Namespaceless_InnerClassMembers_ShouldNotReportANamespace()
    {
        // Arrange
        var methodBase = typeof(global::NamespacelessClass.Inner).GetMethod("Method0")!;

        // Act
        ManagedNameHelper.GetManagedName(methodBase, out var managedTypeName, out var managedMethodName);

        // Assert
        Assert.AreEqual("NamespacelessClass+Inner", managedTypeName);
        Assert.AreEqual("Method0", managedMethodName);
    }

    [TestMethod]
    public void Namespaceless_InnerRecordMembers_ShouldNotReportANamespace()
    {
        // Arrange
        var methodBase = typeof(global::NamespacelessRecord.Inner).GetMethod("Method0")!;

        // Act
        ManagedNameHelper.GetManagedName(methodBase, out var managedTypeName, out var managedMethodName);

        // Assert
        Assert.AreEqual("NamespacelessRecord+Inner", managedTypeName);
        Assert.AreEqual("Method0", managedMethodName);
    }

    [TestMethod]
    public void Namespaceless_ClassMembers_ShouldNotReportANamespace_InHierarchy()
    {
        // Arrange
        var methodBase = typeof(global::NamespacelessClass).GetMethod("Method0")!;

        // Act
        ManagedNameHelper.GetManagedName(methodBase, out var managedTypeName, out var managedMethodName, out var hierarchyValues);

        // Assert
        Assert.AreEqual("NamespacelessClass", managedTypeName);
        Assert.AreEqual("Method0", managedMethodName);
        Assert.IsNull(hierarchyValues[HierarchyConstants.Levels.NamespaceIndex]);
    }

    [TestMethod]
    public void Namespaceless_RecordMembers_ShouldNotReportANamespace_InHierarch()
    {
        // Arrange
        var methodBase = typeof(global::NamespacelessRecord).GetMethod("Method0")!;

        // Act
        ManagedNameHelper.GetManagedName(methodBase, out var managedTypeName, out var managedMethodName, out var hierarchyValues);

        // Assert
        Assert.AreEqual("NamespacelessRecord", managedTypeName);
        Assert.AreEqual("Method0", managedMethodName);
        Assert.IsNull(hierarchyValues[HierarchyConstants.Levels.NamespaceIndex]);
    }

    [TestMethod]
    public void Namespaceless_InnerClassMembers_ShouldNotReportANamespace_InHierarchy()
    {
        // Arrange
        var methodBase = typeof(global::NamespacelessClass.Inner).GetMethod("Method0")!;

        // Act
        ManagedNameHelper.GetManagedName(methodBase, out var managedTypeName, out var managedMethodName, out var hierarchyValues);

        // Assert
        Assert.AreEqual("NamespacelessClass+Inner", managedTypeName);
        Assert.AreEqual("Method0", managedMethodName);
        Assert.IsNull(hierarchyValues[HierarchyConstants.Levels.NamespaceIndex]);
    }

    [TestMethod]
    public void Namespaceless_InnerRecordMembers_ShouldNotReportANamespace_InHierarchy()
    {
        // Arrange
        var methodBase = typeof(global::NamespacelessRecord.Inner).GetMethod("Method0")!;

        // Act
        ManagedNameHelper.GetManagedName(methodBase, out var managedTypeName, out var managedMethodName, out var hierarchyValues);

        // Assert
        Assert.AreEqual("NamespacelessRecord+Inner", managedTypeName);
        Assert.AreEqual("Method0", managedMethodName);
        Assert.IsNull(hierarchyValues[HierarchyConstants.Levels.NamespaceIndex]);
    }

    [TestMethod]
    public void SpecialCharacters_HierarchyShouldNotWrapMembersWithSpecialCharactersInSingleQuotes()
    {
        var methodBase = typeof(Class狧麱狵錋狾龍龪啊阿埃挨哎唉0u㐀㐁㐂㐃㐄㐅㐆㐇6ⅶ０ǒoU1U2U38丂丄丅丆丏丒丟).GetMethod("Method0")!;

        // Act
        ManagedNameHelper.GetManagedName(methodBase, out var managedTypeName, out var managedMethodName, out var _);

        // Assert
        Assert.AreEqual("TestClasses.Class狧麱狵錋狾龍龪啊阿埃挨哎唉0u㐀㐁㐂㐃㐄㐅㐆㐇6ⅶ０ǒoU1U2U38丂丄丅丆丏丒丟", managedTypeName);
        Assert.AreEqual("Method0", managedMethodName);
    }

}
