// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Linq;
using Xunit.Sdk;
using Xunit.v3;

namespace Microsoft.DotNet.XUnitExtensions.Tests
{
    /// <summary>
    /// Orders test cases alphabetically so that we can test 'failing' test cases through
    /// side-effects.
    /// </summary>
    public class AlphabeticalOrderer : ITestCaseOrderer
    {
        public IReadOnlyCollection<TTestCase> OrderTestCases<TTestCase>(IReadOnlyCollection<TTestCase> testCases)
            where TTestCase : notnull, ITestCase
        {
            return testCases.OrderBy(tc => tc.TestCaseDisplayName).ToList();
        }
    }
}
