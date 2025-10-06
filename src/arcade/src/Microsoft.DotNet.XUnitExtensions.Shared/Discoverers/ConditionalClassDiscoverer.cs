// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if !USES_XUNIT_3
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Microsoft.DotNet.XUnitExtensions
{
    /// <summary>
    /// This class discovers all of the tests and test classes that have
    /// applied the ConditionalClass attribute
    /// </summary>
    public class ConditionalClassDiscoverer : ITraitDiscoverer
    {
        /// <summary>
        /// Gets the trait values from the Category attribute.
        /// </summary>
        /// <param name="traitAttribute">The trait attribute containing the trait values.</param>
        /// <returns>The trait values.</returns>
        public IEnumerable<KeyValuePair<string, string>> GetTraits(IAttributeInfo traitAttribute)
        {
            // If evaluated to false, skip the test class entirely.
            if (!EvaluateParameterHelper(traitAttribute))
            {
                yield return new KeyValuePair<string, string>(XunitConstants.Category, XunitConstants.Failing);
            }
        }

        internal static bool EvaluateParameterHelper(IAttributeInfo traitAttribute)
        {
            // Parse the traitAttribute. We make sure it contains two parts:
            // 1. Type 2. nameof(conditionMemberName)
            object[] conditionArguments = traitAttribute.GetConstructorArguments().ToArray();
            Debug.Assert(conditionArguments.Count() == 2);

            Type calleeType = null;
            string[] conditionMemberNames = null;

            if (ConditionalTestDiscoverer.CheckInputToSkipExecution(conditionArguments, ref calleeType, ref conditionMemberNames))
            {
                return true;
            }

            return DiscovererHelpers.Evaluate(calleeType, conditionMemberNames);
        }
    }
}
#endif
