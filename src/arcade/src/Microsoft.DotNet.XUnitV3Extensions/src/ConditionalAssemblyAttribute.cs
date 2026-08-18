// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.DotNet.XUnitExtensions;
using Xunit.Sdk;

namespace Xunit
{
    /// <summary>
    /// An assembly-level attribute that conditionally marks all tests in the assembly with a trait
    /// based on the evaluation of one or more static boolean members. When any of the referenced
    /// condition members evaluates to <c>false</c>, the attribute contributes a <c>category=failing</c>
    /// trait so that the test runner can exclude the affected tests.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public sealed class ConditionalAssemblyAttribute : Attribute, ITraitAttribute
    {
        [DynamicallyAccessedMembers(StaticReflectionConstants.ConditionalMemberKinds)]
        public Type CalleeType { get; private set; }
        public string[] ConditionMemberNames { get; private set; }

        public ConditionalAssemblyAttribute(
            [DynamicallyAccessedMembers(StaticReflectionConstants.ConditionalMemberKinds)]
            Type calleeType,
            params string[] conditionMemberNames)
        {
            CalleeType = calleeType;
            ConditionMemberNames = conditionMemberNames;
        }

        public IReadOnlyCollection<KeyValuePair<string, string>> GetTraits()
        {
            // If evaluated to false, mark all tests in the assembly with the failing category so the runner can exclude them.
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
    }
}
