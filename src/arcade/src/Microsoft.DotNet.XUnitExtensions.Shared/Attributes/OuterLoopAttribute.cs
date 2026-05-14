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
    /// Apply this attribute to your test method to specify a outer-loop category.
    /// </summary>
#if !USES_XUNIT_3
    [TraitDiscoverer("Microsoft.DotNet.XUnitExtensions.OuterLoopTestsDiscoverer", "Microsoft.DotNet.XUnitExtensions")]
#endif
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
    public class OuterLoopAttribute : Attribute, ITraitAttribute
    {
#if USES_XUNIT_3
        private readonly object[] _ctorArgs;
#endif

        public Type CalleeType { get; private set; }
        public string[] ConditionMemberNames { get; private set; }

        public OuterLoopAttribute()
        {
#if USES_XUNIT_3
            _ctorArgs = [];
#endif
        }

        public OuterLoopAttribute(string reason)
        {
#if USES_XUNIT_3
            _ctorArgs = [reason];
#endif
        }

        public OuterLoopAttribute(string reason, TestPlatforms platforms)
        {
#if USES_XUNIT_3
            _ctorArgs = [reason, platforms];
#endif
        }

        public OuterLoopAttribute(string reason, TargetFrameworkMonikers framework)
        {
#if USES_XUNIT_3
            _ctorArgs = [reason, framework];
#endif
        }

        public OuterLoopAttribute(string reason, TestRuntimes runtimes)
        {
#if USES_XUNIT_3
            _ctorArgs = [reason, runtimes];
#endif
        }

        public OuterLoopAttribute(string reason, TestPlatforms platforms = TestPlatforms.Any, TargetFrameworkMonikers framework = TargetFrameworkMonikers.Any, TestRuntimes runtimes = TestRuntimes.Any)
        {
#if USES_XUNIT_3
            _ctorArgs = [reason, platforms, framework, runtimes];
#endif
        }

        public OuterLoopAttribute(string reason, Type calleeType, params string[] conditionMemberNames)
        {
#if USES_XUNIT_3
            _ctorArgs = [reason, calleeType, conditionMemberNames];
#endif
            CalleeType = calleeType;
            ConditionMemberNames = conditionMemberNames;
        }

#if USES_XUNIT_3
        public IReadOnlyCollection<KeyValuePair<string, string>> GetTraits()
        {
            if (_ctorArgs.Length < 2)
            {
                return new[] { new KeyValuePair<string, string>(XunitConstants.Category, XunitConstants.OuterLoop) };
            }

            return DiscovererHelpers.EvaluateArguments(_ctorArgs, XunitConstants.OuterLoop).ToArray();
        }
#endif
    }
}
