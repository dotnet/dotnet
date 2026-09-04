// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Microsoft.TestPlatform.VsTestConsole.TranslationLayer.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

// This file deliberately references the deprecated interface it guards.
#pragma warning disable CS0618

namespace Microsoft.TestPlatform.VsTestConsole.TranslationLayer.UnitTests;

/// <summary>
/// Guards the deprecation shape of <see cref="IVsTestConsoleWrapperAsync"/>.
/// </summary>
[TestClass]
public class VsTestConsoleWrapperObsoletionTests
{
    /// <summary>
    /// The only members of <see cref="IVsTestConsoleWrapperAsync"/> that are not individually deprecated.
    /// </summary>
    ///
    /// <remarks>
    /// Unlike their siblings these two genuinely run asynchronously and have no synchronous replacement to
    /// name, so deprecating them is an API decision rather than hygiene. It would also emit a brand new
    /// <c>CS0618</c> at every in-repo call site that reaches them through <see cref="IVsTestConsoleWrapper"/>,
    /// and this repository treats warnings as errors, so the build would break. That belongs with the breaking
    /// changes of the deprecation clean up, so the gap is pinned here instead of closed.
    /// </remarks>
    private const string NotIndividuallyObsolete = "ProcessTestRunAttachmentsAsync";

    /// <summary>
    /// <see cref="IVsTestConsoleWrapper"/> keeps deriving from the deprecated async interface so that already
    /// compiled callers keep resolving the inherited members. Dropping the base interface would be a binary
    /// breaking change, so it is asserted here rather than left to review.
    /// </summary>
    [TestMethod]
    public void IVsTestConsoleWrapper_StillInheritsTheObsoleteAsyncInterface()
        => Assert.IsTrue(typeof(IVsTestConsoleWrapperAsync).IsAssignableFrom(typeof(IVsTestConsoleWrapper)));

    [TestMethod]
    public void IVsTestConsoleWrapperAsync_IsObsolete()
        => Assert.IsNotNull(GetObsoleteAttribute(typeof(IVsTestConsoleWrapperAsync)));

    /// <summary>
    /// The interface level attribute alone is not enough: the compiler does not report it when an inherited
    /// member is reached through <see cref="IVsTestConsoleWrapper"/>, which is how virtually every consumer
    /// uses these APIs. Every member that carries the attribute today therefore has to keep it.
    /// </summary>
    [TestMethod]
    public void IVsTestConsoleWrapperAsync_KeepsEveryMemberIndividuallyObsolete()
    {
        foreach (var member in GetIndividuallyObsoleteMembers())
        {
            Assert.IsNotNull(
                GetObsoleteAttribute(member),
                $"'{member.Name}' must stay marked with [Obsolete], otherwise callers going through "
                + $"{nameof(IVsTestConsoleWrapper)} silently lose the deprecation warning.");
        }
    }

    // This deprecation is advisory. Escalating it to error: true would break consumers that still compile
    // against the async API, so that change is deliberately kept out of this set.
    [TestMethod]
    public void IVsTestConsoleWrapperAsync_DeprecationIsAWarningAndNotAnError()
    {
        Assert.IsFalse(GetObsoleteAttribute(typeof(IVsTestConsoleWrapperAsync))?.IsError ?? false);

        foreach (var member in GetIndividuallyObsoleteMembers())
        {
            Assert.IsFalse(
                GetObsoleteAttribute(member)?.IsError ?? false,
                $"'{member.Name}' must stay a warning.");
        }
    }

    /// <summary>
    /// Fails if <see cref="NotIndividuallyObsolete"/> goes stale, either because the overloads were finally
    /// deprecated or because they were renamed or removed.
    /// </summary>
    [TestMethod]
    public void IVsTestConsoleWrapperAsync_OnlyProcessTestRunAttachmentsAsyncIsNotIndividuallyObsolete()
    {
        var notObsolete = typeof(IVsTestConsoleWrapperAsync).GetMembers()
            .Where(member => GetObsoleteAttribute(member) is null)
            .Select(member => member.Name)
            .Distinct()
            .OrderBy(name => name, StringComparer.Ordinal)
            .ToList();

        Assert.AreEqual(NotIndividuallyObsolete, string.Join(", ", notObsolete));
    }

    /// <summary>
    /// The overloads are excluded by name everywhere else, so deprecating only one of them would slip through
    /// every other assertion here. Each one is therefore checked individually.
    /// </summary>
    [TestMethod]
    public void IVsTestConsoleWrapperAsync_BothProcessTestRunAttachmentsAsyncOverloadsStayUndeprecated()
    {
        var overloads = typeof(IVsTestConsoleWrapperAsync).GetMembers()
            .Where(member => member.Name == NotIndividuallyObsolete)
            .ToList();

        Assert.HasCount(2, overloads);

        foreach (var overload in overloads)
        {
            Assert.IsNull(
                GetObsoleteAttribute(overload),
                $"'{overload}' must stay undeprecated until the breaking half of the clean up.");
        }
    }

    private static IEnumerable<MemberInfo> GetIndividuallyObsoleteMembers()
        => typeof(IVsTestConsoleWrapperAsync).GetMembers()
            .Where(member => member.Name != NotIndividuallyObsolete);

    private static ObsoleteAttribute? GetObsoleteAttribute(MemberInfo member)
        => member.GetCustomAttributes(typeof(ObsoleteAttribute), inherit: false)
            .Cast<ObsoleteAttribute>()
            .SingleOrDefault();
}
