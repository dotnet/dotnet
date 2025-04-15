// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.DotNet.XUnitExtensions;
using Xunit.Sdk;

namespace Xunit
{
#if !USES_XUNIT_3
    [TraitDiscoverer("Microsoft.DotNet.XUnitExtensions.ConditionalClassDiscoverer", "Microsoft.DotNet.XUnitExtensions")]
#endif
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class ConditionalClassAttribute : Attribute, ITraitAttribute
    {
        [DynamicallyAccessedMembers(StaticReflectionConstants.ConditionalMemberKinds)]
        public Type CalleeType { get; private set; }
        public string[] ConditionMemberNames { get; private set; }

        public ConditionalClassAttribute(
            [DynamicallyAccessedMembers(StaticReflectionConstants.ConditionalMemberKinds)]
            Type calleeType,
            params string[] conditionMemberNames)
        {
            CalleeType = calleeType;
            ConditionMemberNames = conditionMemberNames;
        }

#if USES_XUNIT_3
        public IReadOnlyCollection<KeyValuePair<string, string>> GetTraits()
        {
            // If evaluated to false, skip the test class entirely.
            if (!EvaluateParameterHelper())
            {
                return [new KeyValuePair<string, string>(XunitConstants.Category, XunitConstants.Failing)];
            }

            return [];
        }

        internal bool EvaluateParameterHelper()
        {
            Type calleeType = null;
            string[] conditionMemberNames = null;

            if (ConditionalTestDiscoverer.CheckInputToSkipExecution([CalleeType, ConditionMemberNames], ref calleeType, ref conditionMemberNames))
            {
                return true;
            }

            return DiscovererHelpers.Evaluate(calleeType, conditionMemberNames);
        }
#endif
    }
}
