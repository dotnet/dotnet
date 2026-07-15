// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Microsoft.DotNet.XUnitExtensions;

namespace Xunit
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class CulturedConditionalTheoryAttribute : CulturedTheoryAttribute
    {
        [DynamicallyAccessedMembers(StaticReflectionConstants.ConditionalMemberKinds)]
        public Type CalleeType { get; private set; }
        public string[] ConditionMemberNames { get; private set; }

        public CulturedConditionalTheoryAttribute(
            string[] cultures,
            [DynamicallyAccessedMembers(StaticReflectionConstants.ConditionalMemberKinds)]
            Type calleeType,
            string[] conditionMemberNames,
            [CallerFilePath] string sourceFilePath = null,
            [CallerLineNumber] int sourceLineNumber = 0)
            : base(cultures, sourceFilePath, sourceLineNumber)
        {
            CalleeType = calleeType;
            ConditionMemberNames = conditionMemberNames;
            string skipReason = ConditionalTestDiscoverer.EvaluateSkipConditions(calleeType, conditionMemberNames);
            if (skipReason != null)
            {
                Skip = skipReason;
            }
        }
    }
}
