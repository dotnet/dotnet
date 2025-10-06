// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.DotNet.XUnitExtensions;
using Xunit.Sdk;

namespace Xunit
{
    /// <summary>
    /// Apply this attribute to your test method to specify an active issue.
    /// </summary>
#if !USES_XUNIT_3
    [TraitDiscoverer("Microsoft.DotNet.XUnitExtensions.ActiveIssueDiscoverer", "Microsoft.DotNet.XUnitExtensions")]
#endif
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Assembly, AllowMultiple = true)]
    public class ActiveIssueAttribute : Attribute, ITraitAttribute
    {
#if USES_XUNIT_3
        private readonly IEnumerable<object> _ctorArgs;
#endif

        public Type CalleeType { get; private set; }
        public string[] ConditionMemberNames { get; private set; }

        public ActiveIssueAttribute(string issue, TestPlatforms platforms)
        {
#if USES_XUNIT_3
            _ctorArgs = [issue, platforms];
#endif
        }

        public ActiveIssueAttribute(string issue, TargetFrameworkMonikers framework)
        {
#if USES_XUNIT_3
            _ctorArgs = [issue, framework];
#endif
        }

        public ActiveIssueAttribute(string issue, TestRuntimes runtimes)
        {
#if USES_XUNIT_3
            _ctorArgs = [issue, runtimes];
#endif
        }

        public ActiveIssueAttribute(string issue, TestPlatforms platforms = TestPlatforms.Any, TargetFrameworkMonikers framework = TargetFrameworkMonikers.Any, TestRuntimes runtimes = TestRuntimes.Any)
        {
#if USES_XUNIT_3
            _ctorArgs = [issue, platforms, framework, runtimes];
#endif
        }

        public ActiveIssueAttribute(string issue, Type calleeType, params string[] conditionMemberNames)
        {
#if USES_XUNIT_3
            _ctorArgs = [issue, calleeType, conditionMemberNames];
#endif
            CalleeType = calleeType;
            ConditionMemberNames = conditionMemberNames;
        }

#if USES_XUNIT_3
        public IReadOnlyCollection<KeyValuePair<string, string>> GetTraits()
        {
            return DiscovererHelpers.EvaluateArguments(_ctorArgs, XunitConstants.Failing).ToArray();
        }
#endif
    }
}
