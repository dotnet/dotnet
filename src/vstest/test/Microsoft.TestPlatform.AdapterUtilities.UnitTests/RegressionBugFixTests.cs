// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Runtime.Serialization;

using Microsoft.TestPlatform.AdapterUtilities.ManagedNameUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.AdapterUtilities.UnitTests;

/// <summary>
/// Regression test for GH-4424 / PR #4440:
/// InvalidManagedNameException's serialization constructor must be marked [Obsolete]
/// on NET8+ (SYSLIB0051). The primary constructor (string message) must NOT be obsolete.
/// Before the fix, the serialization constructor was not marked obsolete, triggering
/// SYSLIB0051 warnings when building on NET8+.
/// </summary>
[TestClass]
public class RegressionBugFixTests
{
    public TestContext TestContext { get; set; } = null!;

    [TestMethod]
    public void SerializationConstructor_OnNet8Plus_MustHaveObsoleteAttribute()
    {
        // GH-4424: The fix added [Obsolete] on #if NET8_0_OR_GREATER to the
        // serialization constructor. If reverted, this attribute would be missing.
        var serializationCtor = typeof(InvalidManagedNameException)
            .GetConstructor(
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance,
                binder: null,
                types: [typeof(SerializationInfo), typeof(StreamingContext)],
                modifiers: null);

        Assert.IsNotNull(serializationCtor,
            "Serialization constructor (SerializationInfo, StreamingContext) must exist.");

        var obsoleteAttr = serializationCtor.GetCustomAttributes(typeof(ObsoleteAttribute), inherit: false);

#if NET8_0_OR_GREATER
        Assert.IsNotEmpty(obsoleteAttr,
            "GH-4424: On NET8+, the serialization constructor must have [Obsolete] attribute.");
#else
        Assert.IsEmpty(obsoleteAttr,
            "On pre-NET8, the serialization constructor must NOT have [Obsolete] attribute.");
#endif
    }

    [TestMethod]
    public void PrimaryConstructor_MustNotHaveObsoleteAttribute()
    {
        // The primary constructor (string message) must NOT be marked obsolete on any TFM.
        var primaryCtor = typeof(InvalidManagedNameException)
            .GetConstructor([typeof(string)]);

        Assert.IsNotNull(primaryCtor,
            "Primary constructor (string) must exist.");

        var obsoleteAttr = primaryCtor.GetCustomAttributes(typeof(ObsoleteAttribute), inherit: false);
        Assert.IsEmpty(obsoleteAttr,
            "GH-4424: The primary constructor must NOT have [Obsolete] attribute.");
    }

    [TestMethod]
    public void PrimaryConstructor_MustPreserveMessage()
    {
        // Verify the primary constructor works correctly and preserves the message.
        const string expectedMessage = "test managed name error";

        var exception = new InvalidManagedNameException(expectedMessage);

        Assert.AreEqual(expectedMessage, exception.Message,
            "GH-4424: InvalidManagedNameException must preserve the message from the primary constructor.");
    }
}
