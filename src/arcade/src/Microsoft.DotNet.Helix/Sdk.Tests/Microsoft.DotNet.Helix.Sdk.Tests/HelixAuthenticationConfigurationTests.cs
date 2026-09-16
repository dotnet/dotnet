// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Xunit;

namespace Microsoft.DotNet.Helix.Sdk.Tests
{
    public class HelixAuthenticationConfigurationTests
    {
        private const string AuthenticatedCondition =
            "'$(HelixAccessToken)' != '' Or '$(HelixUseEntraAuthentication)' == 'true'";
        private const string AnonymousCondition =
            "'$(HelixAccessToken)' == '' And '$(HelixUseEntraAuthentication)' != 'true'";

        [Fact]
        public void UnitTestQueuesAndCreatorRecognizeEntraAuthentication()
        {
            string[] conditions = GetHelixAccessTokenConditions("UnitTests.proj");

            Assert.Equal(3, conditions.Length);
            Assert.Single(conditions, condition => condition.Contains(AuthenticatedCondition));
            Assert.Equal(2, conditions.Count(condition => condition.Contains(AnonymousCondition)));
        }

        [Fact]
        public void XHarnessCreatorRecognizesEntraAuthentication()
        {
            string condition = Assert.Single(
                GetHelixAccessTokenConditions("XHarness.Tests.Common.props"));

            Assert.Contains(AnonymousCondition, condition);
        }

        private static string[] GetHelixAccessTokenConditions(string fileName)
        {
            string path = Path.Combine(
                AppContext.BaseDirectory,
                "testassets",
                "HelixAuthentication",
                fileName);
            return XDocument.Load(path)
                .Descendants()
                .Attributes("Condition")
                .Select(attribute => attribute.Value)
                .Where(condition => condition.Contains("$(HelixAccessToken)"))
                .ToArray();
        }
    }
}
