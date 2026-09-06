// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.AdapterUtilities.UnitTests.TestIdProvider;

/// <summary>
/// Pins the ids produced by <see cref="AdapterUtilities.TestIdProviderXxHash128"/>, the xxHash128 based
/// successor to the SHA1 based <see cref="AdapterUtilities.TestIdProvider"/>.
/// </summary>
/// <remarks>
/// <para>
/// A test id is durable state: it ends up in TRX files, in Azure DevOps and in Test Case work item
/// association. Changing it is a breaking change, so it must never happen by accident. If a change
/// here is deliberate, bump TestIdGuid.CurrentHashVersion along with it, so old and new ids stay
/// distinguishable.
/// </para>
/// <para>
/// Every expected value below starts with '1', which is the embedded hash version, and has '8' as
/// the UUID version nibble. See TestIdGuidTests for those invariants on their own.
/// </para>
/// </remarks>
[TestClass]
public class XxHash128CompatibilityTests
{
    [TestMethod]
    [DataRow(["19aa06d3-0147-88d8-a001-c324468d497f", ""])]
    [DataRow(["1fb9b82d-2774-81e0-87d1-55d6ff4ca9e4", "adapter://", "name1"])]                                                                          // less than one block
    [DataRow(["124ba19c-96f4-818a-98b4-15150cd7f5df", "adapter://namesamplenam.testname"])]                                                             // 1 full block
    [DataRow(["1dda3055-9e36-80b8-86a7-789e702b846f", "adapter://namesamplenamespace.testname"])]                                                       // 1 full block and extra
    [DataRow(["124ba19c-96f4-818a-98b4-15150cd7f5df", "adapter://", "name", "samplenam", ".", "testname"])]                                             // 1 full block, appended in pieces
    [DataRow(["1dda3055-9e36-80b8-86a7-789e702b846f", "adapter://", "name", "samplenamespace", ".", "testname"])]                                       // 1 full block and extra, appended in pieces
    [DataRow(["1a540f60-22af-8192-bc74-3a1b6265d2b1", "adapter://namesamplenam.testnameaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa"])]                             // 2 full blocks
    [DataRow(["1a8cb45b-0362-86a6-aff3-f20e45c2a056", "adapter://namesamplenamespace.testnameaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa"])]                       // 2 full blocks and extra
    [DataRow(["1a540f60-22af-8192-bc74-3a1b6265d2b1", "adapter://", "name", "samplenam", ".", "testname", "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa"])]         // 2 full blocks, appended in pieces
    [DataRow(["1a8cb45b-0362-86a6-aff3-f20e45c2a056", "adapter://", "name", "samplenamespace", ".", "testname", "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa"])]   // 2 full blocks and extra, appended in pieces
    public void IdCompatibilityTests(string[] data)
    {
        // Arrange
        var expectedId = new Guid(data[0]);

        // Act
        var idProvider = new AdapterUtilities.TestIdProviderXxHash128();
        foreach (string d in data.Skip(1))
        {
            idProvider.AppendString(d);
        }
        Guid id = idProvider.GetId();

        // Assert
        Assert.AreEqual(expectedId, id);
    }

    [TestMethod]
    public void IdGeneration_TestVectors_EmptyString()
        => IdGeneration_TestVector(string.Empty, "19aa06d3-0147-88d8-a001-c324468d497f");

    [TestMethod]
    public void IdGeneration_TestVectors_abc()
        => IdGeneration_TestVector("abc", "1dcae961-3d3c-87ca-8340-2c89fa0d3198");

    [TestMethod]
    public void IdGeneration_TestVectors_448Bits()
        => IdGeneration_TestVector("abcdbcdecdefdefgefghfghighij", "104a4cb6-4809-8833-8bb8-ad8d0d87f655");

    [TestMethod]
    public void IdGeneration_ExtremelyLarge_TestVectors_100k_abc()
        => IdGeneration_TestRepetitionVector("abc", 100_000, "154504d8-e373-86f7-b493-b93fb9f2970a");

    [TestMethod]
    public void GetId_IsStableAcrossCalls()
    {
        var idProvider = new AdapterUtilities.TestIdProviderXxHash128();
        idProvider.AppendString("adapter://some.test.name");

        Assert.AreEqual(idProvider.GetId(), idProvider.GetId());
    }

    [TestMethod]
    public void GetHash_IsNotCorruptedByGetId()
    {
        // GetId versions the hash bytes in place, so it must not scribble on what GetHash returns.
        var idProvider = new AdapterUtilities.TestIdProviderXxHash128();
        idProvider.AppendString("adapter://some.test.name");

        byte[] before = (byte[])idProvider.GetHash().Clone();
        _ = idProvider.GetId();
        byte[] after = idProvider.GetHash();

        CollectionAssert.AreEqual(before, after, "GetId() modified the hash returned by GetHash().");
    }

    [TestMethod]
    public void AppendString_Throws_AfterHashIsCalculated()
    {
        var idProvider = new AdapterUtilities.TestIdProviderXxHash128();
        idProvider.AppendString("adapter://some.test.name");
        _ = idProvider.GetHash();

        Assert.ThrowsExactly<InvalidOperationException>(() => idProvider.AppendString("more"));
    }

    [TestMethod]
    public void AppendString_Throws_WhenStringIsNull()
        => Assert.ThrowsExactly<ArgumentNullException>(() => new AdapterUtilities.TestIdProviderXxHash128().AppendString(null!));

    private static void IdGeneration_TestVector(string testName, string expected)
    {
        // Arrange
        var idProvider = new AdapterUtilities.TestIdProviderXxHash128();

        // Act
        idProvider.AppendString(testName);
        string actual = idProvider.GetId().ToString();

        // Assert
        Assert.AreEqual(expected, actual, $"Test Id for '{testName}' is invalid!");
    }

    private static void IdGeneration_TestRepetitionVector(string input, int repetition, string expected)
    {
        // Arrange
        var idProvider = new AdapterUtilities.TestIdProviderXxHash128();

        // Act
        for (int i = 0; i < repetition; i++)
        {
            idProvider.AppendString(input);
        }

        string id = idProvider.GetId().ToString();

        // Assert
        Assert.AreEqual(expected, id, $"Test id generation for vector '{input}'*{repetition} failed! (normal path)");
    }
}
