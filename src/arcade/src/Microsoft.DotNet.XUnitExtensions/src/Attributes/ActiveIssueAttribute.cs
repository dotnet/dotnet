// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Xunit.Sdk;

namespace Xunit
{
    /// <summary>
    /// Apply this attribute to your test method to specify an active issue.
    /// </summary>
    [TraitDiscoverer("Microsoft.DotNet.XUnitExtensions.ActiveIssueDiscoverer", "Microsoft.DotNet.XUnitExtensions")]
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Assembly, AllowMultiple = true)]
    public class ActiveIssueAttribute : Attribute, ITraitAttribute
    {
        public Type CalleeType { get; private set; }
        public string[] ConditionMemberNames { get; private set; }

        public ActiveIssueAttribute(string issue, TestPlatforms platforms) { }
        public ActiveIssueAttribute(string issue, TargetFrameworkMonikers framework) { }
        public ActiveIssueAttribute(string issue, TestRuntimes runtimes) { }
        public ActiveIssueAttribute(string issue, TestPlatforms platforms = TestPlatforms.Any, TargetFrameworkMonikers framework = TargetFrameworkMonikers.Any, TestRuntimes runtimes = TestRuntimes.Any) { }
        public ActiveIssueAttribute(string issue, Type calleeType, params string[] conditionMemberNames)
        {
            CalleeType = calleeType;
            ConditionMemberNames = conditionMemberNames;
        }
    }
}
