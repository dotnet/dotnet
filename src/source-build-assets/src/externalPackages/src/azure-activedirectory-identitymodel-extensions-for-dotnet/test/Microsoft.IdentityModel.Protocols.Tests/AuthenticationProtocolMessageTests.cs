// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using Microsoft.IdentityModel.TestUtils;
using Xunit;

namespace Microsoft.IdentityModel.Protocols.Tests
{
    /// <summary>
    /// Tests for AuthenticationProtocolMessage.
    /// </summary>
    public class AuthenticationProtocolMessageTests
    {
        [Fact]
        public void Defaults()
        {
            var context = new CompareContext();
            string issuerAddress = "http://www.gotjwt.com";
            var script = "<script language=\"javascript\">window.setTimeout(function() {document.forms[0].submit();}, 0);</script>";

            AuthenticationProtocolMessage authenticationProtocolMessage = new DerivedAuthenticationProtocolMessage();
            IdentityComparer.AreStringsEqual(authenticationProtocolMessage.IssuerAddress, string.Empty, context);

            authenticationProtocolMessage = new DerivedAuthenticationProtocolMessage() { IssuerAddress = issuerAddress };
            IdentityComparer.AreStringsEqual(authenticationProtocolMessage.IssuerAddress, issuerAddress, context);

            if (!authenticationProtocolMessage.Script.Equals(script))
                context.Diffs.Add("The value of authenticationProtocolMessage.Script should be '" + script + "'.");

            if (authenticationProtocolMessage.Parameters == null)
                context.Diffs.Add("authenticationProtocolMessage.Parameters == null");

            if (authenticationProtocolMessage.Parameters.Count != 0)
                context.Diffs.Add("authenticationProtocolMessage.Parameters.Count != 0");

            TestUtilities.AssertFailIfErrors(context);
        }

        [Fact]
        public void GetSets()
        {
            var authenticationProtocolMessage = new DerivedAuthenticationProtocolMessage() { IssuerAddress = "http://www.gotjwt.com" };

            var properties = new List<string>()
            {
                "IssuerAddress",
                "PostTitle",
                "Script",
                "ScriptButtonText",
                "ScriptDisabledText",
            };

            var context = new GetSetContext();
            foreach (string property in properties)
            {
                TestUtilities.SetGet(authenticationProtocolMessage, property, null, ExpectedException.ArgumentNullException(substringExpected: property), context);
                TestUtilities.SetGet(authenticationProtocolMessage, property, "", ExpectedException.NoExceptionExpected, context);
                TestUtilities.SetGet(authenticationProtocolMessage, property, property, ExpectedException.NoExceptionExpected, context);
                TestUtilities.SetGet(authenticationProtocolMessage, property, "    ", ExpectedException.NoExceptionExpected, context);
                TestUtilities.SetGet(authenticationProtocolMessage, property, "\t\n\r", ExpectedException.NoExceptionExpected, context);
            }

            TestUtilities.AssertFailIfErrors(context.Errors);
        }

        [Fact]
        public void Publics()
        {
            string value1 = "value1";
            string value2 = "value2";
            string param1 = "param1";
            string param2 = "param2";

            AuthenticationProtocolMessage authenticationProtocolMessage = new DerivedAuthenticationProtocolMessage() { IssuerAddress = "http://www.gotjwt.com" };
            ExpectedException expectedException = ExpectedException.ArgumentNullException(substringExpected: "parameter");
            try
            {
                authenticationProtocolMessage.GetParameter(null);
                expectedException.ProcessNoException();
            }
            catch (Exception exception)
            {
                expectedException.ProcessException(exception);
            }

            expectedException = ExpectedException.ArgumentNullException(substringExpected: "parameter");
            try
            {
                authenticationProtocolMessage.RemoveParameter(null);
                expectedException.ProcessNoException();
            }
            catch (Exception exception)
            {
                expectedException.ProcessException(exception);
            }

            expectedException = ExpectedException.ArgumentNullException(substringExpected: "parameter");
            try
            {
                authenticationProtocolMessage.SetParameter(null, null);
                expectedException.ProcessNoException();
            }
            catch (Exception exception)
            {
                expectedException.ProcessException(exception);
            }

            authenticationProtocolMessage.SetParameter(param1, value1);
            authenticationProtocolMessage.RemoveParameter(param2);
            Assert.Equal(authenticationProtocolMessage.GetParameter(param1), value1);

            authenticationProtocolMessage.RemoveParameter(param1);
            Assert.Null(authenticationProtocolMessage.GetParameter(param1));

            authenticationProtocolMessage.SetParameter(param1, value1);
            authenticationProtocolMessage.SetParameter(param1, value2);
            authenticationProtocolMessage.SetParameter(param2, value2);
            authenticationProtocolMessage.SetParameter(param2, value1);

            Assert.Equal(authenticationProtocolMessage.GetParameter(param1), value2);
            Assert.Equal(authenticationProtocolMessage.GetParameter(param2), value1);

            authenticationProtocolMessage = new DerivedAuthenticationProtocolMessage() { IssuerAddress = "http://www.gotjwt.com" };
            authenticationProtocolMessage.SetParameter("bob", "     ");

            string queryString = authenticationProtocolMessage.BuildRedirectUrl();
            Assert.NotNull(queryString);
            Assert.Contains("bob", queryString);

            authenticationProtocolMessage.IssuerAddress = string.Empty;
            queryString = authenticationProtocolMessage.BuildRedirectUrl();
            Assert.NotNull(queryString);
        }

        /// <summary>
        /// AuthenticationProtocolMessage is abstract use this to test.
        /// </summary>
        private class DerivedAuthenticationProtocolMessage : AuthenticationProtocolMessage
        {
            public DerivedAuthenticationProtocolMessage()
            { }
        }
    }
}
