// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Protocols.OpenIdConnect.Json.Tests;
using Newtonsoft.Json.Linq;
using Xunit;

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant
#pragma warning disable SYSLIB0013 // Type or member is obsolete

namespace Microsoft.IdentityModel.Protocols.OpenIdConnect.Tests
{
    /// <summary>
    /// 
    /// </summary>
    public class OpenIdConnectMessageTests
    {
        [Theory, MemberData(nameof(ConstructorsTheoryData))]
        public void Constructors(OpenIdConnectMessageTheoryData theoryData)
        {
            TestUtilities.WriteHeader($"{this}.Constructors", theoryData);
            var context = new CompareContext($"{this}.ReadMetadata, {theoryData.TestId}");
            OpenIdConnectMessage messageFromJson;
            // TODO - these local variable has been left commented out as
            // we will need this to check a 6x -> 7x compatibility test that needs to be written.
            //OpenIdConnectMessage messageFromJsonObj;
            var diffs = new List<string>();
            try
            {
                messageFromJson = new OpenIdConnectMessage(theoryData.Json);
//#pragma warning disable CS0618 // Type or member is obsolete
//                messageFromJsonObj = new OpenIdConnectMessage6x(theoryData.JObject);
//#pragma warning restore CS0618 // Type or member is obsolete
                //IdentityComparer.AreEqual(messageFromJson, messageFromJsonObj, context);
                IdentityComparer.AreEqual(messageFromJson, theoryData.Message, context);
                theoryData.ExpectedException.ProcessNoException();
            }
            catch (Exception exception)
            {
                theoryData.ExpectedException.ProcessException(exception);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<OpenIdConnectMessageTheoryData> ConstructorsTheoryData()
        {
            return new TheoryData<OpenIdConnectMessageTheoryData>
            {
                new OpenIdConnectMessageTheoryData("EmptyString")
                {
                    ExpectedException = ExpectedException.ArgumentNullException("IDX10000:"),
                    Json = ""
                },
                new OpenIdConnectMessageTheoryData("NullString")
                {
                    ExpectedException = ExpectedException.ArgumentNullException("IDX10000:")
                },
                new OpenIdConnectMessageTheoryData("ResponseModeDuplicated")
                {
                    ExpectedException = ExpectedException.ArgumentException("IDX21106"),
                    Json =  @"{""response_mode"":""responseMode"";""respone_mode"":""duplicateResponeMode""}"
                },
                new OpenIdConnectMessageTheoryData("EmptyJsonStringEmptyJobj")
                {
                    JObject = new JObject(),
                    Json = "{}",
                    Message = new OpenIdConnectMessage()
                },
                new OpenIdConnectMessageTheoryData("ValidJson")
                {
                    JObject = JObject.Parse(@"{""response_mode"":""responseMode"", ""response_type"":""responseType"", ""refresh_token"":""refreshToken""}"),
                    Json = @"{""response_mode"":""responseMode"", ""response_type"":""responseType"", ""refresh_token"":""refreshToken""}",
                    Message = new OpenIdConnectMessage
                    {
                        RefreshToken = "refreshToken",
                        ResponseMode = "responseMode",
                        ResponseType = "responseType"
                    }
                }
            };
        }

        [Fact]
        public void Defaults()
        {
            List<string> errors = new List<string>();
            var message = new OpenIdConnectMessage6x();
            
            if (message.AcrValues != null)
                errors.Add("message.ArcValues != null");

            if (message.ClientAssertion != null)
                errors.Add("message.ClientAssertion != null");

            if (message.ClientAssertionType != null)
                errors.Add("message.ClientAssertionType != null");

            if (message.ClaimsLocales != null)
                errors.Add("message.ClaimsLocales != null");

            if (message.ClientId != null)
                errors.Add("message.ClientId != null");

            if (message.ClientSecret != null)
                errors.Add("message.ClientSecret != null");

            if (message.Code != null)
                errors.Add("message.Code != null");

            if (message.Display != null)
                errors.Add("message.Display != null");

            if (message.IdTokenHint != null)
                errors.Add("message.IdTokenHint != null");

            if (message.LoginHint != null)
                errors.Add("message.LoginHint != null");

            if (message.MaxAge != null)
                errors.Add("message.MaxAge != null");

            if (message.Prompt != null)
                errors.Add("message.Prompt != null");

            if (message.RedirectUri != null)
                errors.Add("message.RedirectUri != null");

            if (message.State != null)
                errors.Add("message.State != null");

            if (message.UiLocales != null)
                errors.Add("message.UiLocales != null");

            TestUtilities.AssertFailIfErrors("OpenIdConnectMessage_Defaults*** Test Failures:\n", errors);
        }

        [Fact]
        public void GetSets()
        {
            OpenIdConnectMessage message = new OpenIdConnectMessage();
            Type type = typeof(OpenIdConnectMessage);
            PropertyInfo[] properties = type.GetProperties();
            if (properties.Length != 48)
                Assert.True(true, "Number of public fields has changed from 48 to: " + properties.Length + ", adjust tests");

            GetSetContext context =
                new GetSetContext
                {
                    PropertyNamesAndSetGetValue = new List<KeyValuePair<string, List<object>>>
                    {
                        new KeyValuePair<string, List<object>>("AuthorizationEndpoint", new List<object>{(string)null, Guid.NewGuid().ToString(), Guid.NewGuid().ToString()}),
                        new KeyValuePair<string, List<object>>("AccessToken", new List<object>{(string)null, Guid.NewGuid().ToString(), Guid.NewGuid().ToString()}),
                        new KeyValuePair<string, List<object>>("AcrValues", new List<object>{(string)null, Guid.NewGuid().ToString(), Guid.NewGuid().ToString()}),
                        new KeyValuePair<string, List<object>>("ClaimsLocales", new List<object>{(string)null, Guid.NewGuid().ToString(), Guid.NewGuid().ToString()}),
                        new KeyValuePair<string, List<object>>("ClientAssertion", new List<object>{(string)null, Guid.NewGuid().ToString(), Guid.NewGuid().ToString()}),
                        new KeyValuePair<string, List<object>>("ClientAssertionType", new List<object>{(string)null, Guid.NewGuid().ToString(), Guid.NewGuid().ToString()}),
                        new KeyValuePair<string, List<object>>("ClientId", new List<object>{(string)null, Guid.NewGuid().ToString(), Guid.NewGuid().ToString()}),
                        new KeyValuePair<string, List<object>>("ClientSecret", new List<object>{(string)null, Guid.NewGuid().ToString(), Guid.NewGuid().ToString()}),
                        new KeyValuePair<string, List<object>>("Code", new List<object>{(string)null, Guid.NewGuid().ToString(), Guid.NewGuid().ToString()}),
                        new KeyValuePair<string, List<object>>("EnableTelemetryParameters", new List<object>{true, false, false}),
                        new KeyValuePair<string, List<object>>("Display", new List<object>{(string)null, Guid.NewGuid().ToString(), Guid.NewGuid().ToString()}),
                        new KeyValuePair<string, List<object>>("DomainHint", new List<object>{(string)null, Guid.NewGuid().ToString(), Guid.NewGuid().ToString()}),
                        new KeyValuePair<string, List<object>>("Error",  new List<object>{(string)null, Guid.NewGuid().ToString(), Guid.NewGuid().ToString()}),
                        new KeyValuePair<string, List<object>>("ErrorDescription",  new List<object>{(string)null, Guid.NewGuid().ToString(), Guid.NewGuid().ToString()}),
                        new KeyValuePair<string, List<object>>("ErrorUri",  new List<object>{(string)null, Guid.NewGuid().ToString(), Guid.NewGuid().ToString()}),
                        new KeyValuePair<string, List<object>>("ExpiresIn",  new List<object>{(string)null, Guid.NewGuid().ToString(), Guid.NewGuid().ToString()}),
                        new KeyValuePair<string, List<object>>("GrantType", new List<object>{(string)null, Guid.NewGuid().ToString(), Guid.NewGuid().ToString()}),
                        new KeyValuePair<string, List<object>>("IdToken", new List<object>{(string)null, Guid.NewGuid().ToString(), Guid.NewGuid().ToString()}),
                        new KeyValuePair<string, List<object>>("IdTokenHint", new List<object>{(string)null, Guid.NewGuid().ToString(), Guid.NewGuid().ToString()}),
                        new KeyValuePair<string, List<object>>("IdentityProvider", new List<object>{(string)null, Guid.NewGuid().ToString(), Guid.NewGuid().ToString()}),
                        new KeyValuePair<string, List<object>>("IssuerAddress", new List<object>{string.Empty, Guid.NewGuid().ToString(), Guid.NewGuid().ToString()}),
                        new KeyValuePair<string, List<object>>("MaxAge", new List<object>{(string)null, Guid.NewGuid().ToString(), Guid.NewGuid().ToString()}),
                        new KeyValuePair<string, List<object>>("Password", new List<object>{(string)null, Guid.NewGuid().ToString(), Guid.NewGuid().ToString()}),
                        new KeyValuePair<string, List<object>>("PostLogoutRedirectUri", new List<object>{(string)null, Guid.NewGuid().ToString(), Guid.NewGuid().ToString()}),
                        new KeyValuePair<string, List<object>>("Prompt", new List<object>{(string)null, Guid.NewGuid().ToString(), Guid.NewGuid().ToString()}),
                        new KeyValuePair<string, List<object>>("RedirectUri", new List<object>{(string)null, Guid.NewGuid().ToString(), Guid.NewGuid().ToString()}),
                        new KeyValuePair<string, List<object>>("RequestUri", new List<object>{(string)null, Guid.NewGuid().ToString(), Guid.NewGuid().ToString()}),
                        new KeyValuePair<string, List<object>>("ResponseMode", new List<object>{(string)null, Guid.NewGuid().ToString(), Guid.NewGuid().ToString()}),
                        new KeyValuePair<string, List<object>>("ResponseType", new List<object>{(string)null, Guid.NewGuid().ToString(), Guid.NewGuid().ToString()}),
                        new KeyValuePair<string, List<object>>("Resource", new List<object>{(string)null, Guid.NewGuid().ToString(), Guid.NewGuid().ToString()}),
                        new KeyValuePair<string, List<object>>("Scope", new List<object>{null, Guid.NewGuid().ToString(), Guid.NewGuid().ToString()}),
                        new KeyValuePair<string, List<object>>("SessionState", new List<object>{(string)null, Guid.NewGuid().ToString(), Guid.NewGuid().ToString()}),
                        new KeyValuePair<string, List<object>>("Sid", new List<object>{(string)null, Guid.NewGuid().ToString(), Guid.NewGuid().ToString()}),
                        new KeyValuePair<string, List<object>>("State", new List<object>{(string)null, Guid.NewGuid().ToString(), Guid.NewGuid().ToString()}),
                        new KeyValuePair<string, List<object>>("TargetLinkUri", new List<object>{(string)null, Guid.NewGuid().ToString(), Guid.NewGuid().ToString()}),
                        new KeyValuePair<string, List<object>>("TokenEndpoint", new List<object>{(string)null, Guid.NewGuid().ToString(), Guid.NewGuid().ToString()}),
                        new KeyValuePair<string, List<object>>("TokenType", new List<object>{(string)null, Guid.NewGuid().ToString(), Guid.NewGuid().ToString()}),
                        new KeyValuePair<string, List<object>>("UiLocales", new List<object>{(string)null, Guid.NewGuid().ToString(), Guid.NewGuid().ToString()}),
                        new KeyValuePair<string, List<object>>("UserId", new List<object>{(string)null, Guid.NewGuid().ToString(), Guid.NewGuid().ToString()}),
                        new KeyValuePair<string, List<object>>("Username", new List<object>{(string)null, Guid.NewGuid().ToString(), Guid.NewGuid().ToString()}),
                    },

                    Object = message,
                };

            TestUtilities.GetSet(context);
            TestUtilities.AssertFailIfErrors("OpenIdConnectMessage_GetSets*** Test Failures:\n", context.Errors);
        }

        [Theory, MemberData(nameof(CreateAuthenticationRequestUrlTheoryData))]
        public void OidcCreateAuthenticationRequestUrl(string testId, OpenIdConnectMessage message, string expectedMessage)
        {
            TestUtilities.WriteHeader(testId, "OidcCreateAuthenticationRequestUrl", true);
            var context = new CompareContext();
#if NET461
            if (!message.SkuTelemetryValue.Equals("ID_NET461"))
                context.Diffs.Add($"{message.SkuTelemetryValue} != ID_NET461");
#elif NET462
            if (!message.SkuTelemetryValue.Equals("ID_NET462"))
                context.Diffs.Add($"{message.SkuTelemetryValue} != ID_NET462");
#elif NET472
            if (!message.SkuTelemetryValue.Equals("ID_NET472"))
                context.Diffs.Add($"{message.SkuTelemetryValue} != ID_NET472");
#elif NET6_0
            if (!message.SkuTelemetryValue.Equals("ID_NET6_0"))
                context.Diffs.Add($"{message.SkuTelemetryValue} != ID_NET6_0");
#elif NET8_0
            if (!message.SkuTelemetryValue.Equals("ID_NET8_0"))
                context.Diffs.Add($"{message.SkuTelemetryValue} != ID_NET8_0");
#elif NET_CORE
            if (!message.SkuTelemetryValue.Equals("ID_NETSTANDARD2_0"))
                context.Diffs.Add($"{message.SkuTelemetryValue} != ID_NETSTANDARD2_0");
#endif
            IdentityComparer.AreEqual(message.CreateAuthenticationRequestUrl(), expectedMessage, context);
            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<string, OpenIdConnectMessage, string> CreateAuthenticationRequestUrlTheoryData()
        {
            string customParameterName = "Custom Parameter Name";
            string customParameterValue = "Custom Parameter Value";
            string nonce = Guid.NewGuid().ToString();
            string redirectUri = "http://gotJwt.onmicrosoft.com/signedIn";
            string resource = "location data";

            var theoryData = new TheoryData<string, OpenIdConnectMessage, string>();
            OpenIdConnectMessage.EnableTelemetryParametersByDefault = false;
            var message = new OpenIdConnectMessage();

            theoryData.Add("EmptyMessage", message, "");

            theoryData.Add(
                "Code",
                message = new OpenIdConnectMessage()
                {
                    ResponseMode = OpenIdConnectResponseMode.FormPost,
                    ResponseType = OpenIdConnectResponseType.Code,
                    Scope = OpenIdConnectScope.OpenIdProfile
                },
                string.Format(CultureInfo.InvariantCulture, @"?response_mode=form_post&response_type={0}&scope=openid%20profile", Uri.EscapeUriString(OpenIdConnectResponseType.Code))
            );

            theoryData.Add(
                "CodeIdToken",
                message = new OpenIdConnectMessage()
                {
                    ResponseMode = OpenIdConnectResponseMode.FormPost,
                    ResponseType = OpenIdConnectResponseType.CodeIdToken,
                    Scope = OpenIdConnectScope.OpenIdProfile
                },
                string.Format(CultureInfo.InvariantCulture, @"?response_mode=form_post&response_type={0}&scope=openid%20profile", Uri.EscapeUriString(OpenIdConnectResponseType.CodeIdToken))
            );

            theoryData.Add(
                "CodeIdTokenToken",
                message = new OpenIdConnectMessage()
                {
                    ResponseMode = OpenIdConnectResponseMode.FormPost,
                    ResponseType = OpenIdConnectResponseType.CodeIdTokenToken,
                    Scope = OpenIdConnectScope.OpenIdProfile
                },
                string.Format(CultureInfo.InvariantCulture, @"?response_mode=form_post&response_type={0}&scope=openid%20profile", Uri.EscapeUriString(OpenIdConnectResponseType.CodeIdTokenToken))
            );

            theoryData.Add(
                "CodeToken",
                message = new OpenIdConnectMessage()
                {
                    ResponseMode = OpenIdConnectResponseMode.FormPost,
                    ResponseType = OpenIdConnectResponseType.CodeToken,
                    Scope = OpenIdConnectScope.OpenIdProfile
                },
                string.Format(CultureInfo.InvariantCulture, @"?response_mode=form_post&response_type={0}&scope=openid%20profile", Uri.EscapeUriString(OpenIdConnectResponseType.CodeToken))
            );

            theoryData.Add(
                "IdToken",
                message = new OpenIdConnectMessage()
                {
                    ResponseMode = OpenIdConnectResponseMode.FormPost,
                    ResponseType = OpenIdConnectResponseType.IdToken,
                    Scope = OpenIdConnectScope.OpenIdProfile
                },
                string.Format(CultureInfo.InvariantCulture, @"?response_mode=form_post&response_type={0}&scope=openid%20profile", Uri.EscapeUriString(OpenIdConnectResponseType.IdToken))
            );

            theoryData.Add(
                "IdTokenToken",
                message = new OpenIdConnectMessage()
                {
                    ResponseMode = OpenIdConnectResponseMode.FormPost,
                    ResponseType = OpenIdConnectResponseType.IdTokenToken,
                    Scope = OpenIdConnectScope.OpenIdProfile
                },
                string.Format(CultureInfo.InvariantCulture, @"?response_mode=form_post&response_type={0}&scope=openid%20profile", Uri.EscapeUriString(OpenIdConnectResponseType.IdTokenToken))
            );

            theoryData.Add(
                "None",
                message = new OpenIdConnectMessage()
                {
                    ResponseMode = OpenIdConnectResponseMode.FormPost,
                    ResponseType = OpenIdConnectResponseType.None,
                    Scope = OpenIdConnectScope.OpenIdProfile
                },
                string.Format(CultureInfo.InvariantCulture, @"?response_mode=form_post&response_type={0}&scope=openid%20profile", Uri.EscapeUriString(OpenIdConnectResponseType.None))
            );

            theoryData.Add(
                "Token",
                message = new OpenIdConnectMessage()
                {
                    ResponseMode = OpenIdConnectResponseMode.FormPost,
                    ResponseType = OpenIdConnectResponseType.Token,
                    Scope = OpenIdConnectScope.OpenIdProfile
                },
                string.Format(CultureInfo.InvariantCulture, @"?response_mode=form_post&response_type={0}&scope=openid%20profile", Uri.EscapeUriString(OpenIdConnectResponseType.Token))
            );

            theoryData.Add(
                "Nonce",
                message = new OpenIdConnectMessage()
                {
                    ResponseMode = OpenIdConnectResponseMode.FormPost,
                    ResponseType = OpenIdConnectResponseType.CodeIdToken,
                    Scope = OpenIdConnectScope.OpenIdProfile,
                    Nonce = nonce
                },
                string.Format(CultureInfo.InvariantCulture, @"?response_mode=form_post&response_type=code%20id_token&scope=openid%20profile&nonce={0}", nonce)
            );

            theoryData.Add(
                "IssuerAddress",
                message = new OpenIdConnectMessage()
                {
                    IssuerAddress = Default.Issuer,
                    ResponseMode = OpenIdConnectResponseMode.FormPost,
                    ResponseType = OpenIdConnectResponseType.CodeIdToken,
                    Scope = OpenIdConnectScope.OpenIdProfile,
                    Nonce = nonce
                },
                string.Format(CultureInfo.InvariantCulture, @"{0}?response_mode=form_post&response_type=code%20id_token&scope=openid%20profile&nonce={1}", Uri.EscapeUriString(Default.Issuer), nonce)
            );

            theoryData.Add(
                "IssuerAddress",
                message = new OpenIdConnectMessage()
                {
                    IssuerAddress = Default.Issuer,
                    ResponseMode = OpenIdConnectResponseMode.FormPost,
                    ResponseType = OpenIdConnectResponseType.CodeIdToken,
                    Scope = OpenIdConnectScope.OpenIdProfile,
                    Nonce = nonce,
                    RedirectUri = redirectUri
                },
                string.Format(CultureInfo.InvariantCulture, @"{0}?response_mode=form_post&response_type=code%20id_token&scope=openid%20profile&nonce={1}&redirect_uri={2}", Uri.EscapeUriString(Default.Issuer), nonce, Uri.EscapeDataString(redirectUri))
            );

            theoryData.Add(
                "IssuerAddressEmpty",
                message = new OpenIdConnectMessage()
                {
                    IssuerAddress = string.Empty,
                    ResponseMode = OpenIdConnectResponseMode.FormPost,
                    ResponseType = OpenIdConnectResponseType.CodeIdToken,
                    Scope = OpenIdConnectScope.OpenIdProfile,
                    Nonce = nonce,
                    RedirectUri = redirectUri
                },
                string.Format(CultureInfo.InvariantCulture, @"?response_mode=form_post&response_type=code%20id_token&scope=openid%20profile&nonce={0}&redirect_uri={1}", nonce, Uri.EscapeDataString(redirectUri))
            );

            theoryData.Add(
                "Resource",
                message = new OpenIdConnectMessage()
                {
                    IssuerAddress = Default.Issuer,
                    ResponseMode = OpenIdConnectResponseMode.FormPost,
                    ResponseType = OpenIdConnectResponseType.CodeIdToken,
                    Scope = OpenIdConnectScope.OpenIdProfile,
                    Nonce = nonce,
                    RedirectUri = redirectUri,
                    Resource = resource
                },
                string.Format(CultureInfo.InvariantCulture, @"{0}?response_mode=form_post&response_type=code%20id_token&scope=openid%20profile&nonce={1}&redirect_uri={2}&resource={3}", Default.Issuer, nonce, Uri.EscapeDataString(redirectUri), Uri.EscapeDataString(resource))
            );

            message = new OpenIdConnectMessage()
            {
                IssuerAddress = Default.Issuer,
                ResponseMode = OpenIdConnectResponseMode.FormPost,
                ResponseType = OpenIdConnectResponseType.CodeIdToken,
                Scope = OpenIdConnectScope.OpenIdProfile,
                Nonce = nonce,
            };
            message.Parameters.Add(customParameterName, customParameterValue);
            message.RedirectUri = redirectUri;
            message.Resource = resource;

            theoryData.Add(
                "CustomParam",
                message,
                string.Format(CultureInfo.InvariantCulture, @"{0}?response_mode=form_post&response_type=code%20id_token&scope=openid%20profile&nonce={1}&{2}={3}&redirect_uri={4}&resource={5}", Default.Issuer, nonce, Uri.EscapeDataString(customParameterName), Uri.EscapeDataString(customParameterValue), Uri.EscapeDataString(redirectUri), Uri.EscapeDataString(resource))
            );

            message = new OpenIdConnectMessage();
            theoryData.Add(
                "Resource",
                message = new OpenIdConnectMessage()
                {
                    IssuerAddress = Default.Issuer,
                    ResponseMode = OpenIdConnectResponseMode.FormPost,
                    ResponseType = OpenIdConnectResponseType.CodeIdToken,
                    Scope = OpenIdConnectScope.OpenIdProfile,
                    Nonce = nonce,
                    RedirectUri = redirectUri,
                    Resource = resource
                },
                string.Format(CultureInfo.InvariantCulture, @"{0}?response_mode=form_post&response_type=code%20id_token&scope=openid%20profile&nonce={1}&redirect_uri={2}&resource={3}", Default.Issuer, nonce, Uri.EscapeDataString(redirectUri), Uri.EscapeDataString(resource))
            );

            OpenIdConnectMessage.EnableTelemetryParametersByDefault = true;
            message = new OpenIdConnectMessage();
            theoryData.Add(
                "Telemetry",
                message,
                string.Format(CultureInfo.InvariantCulture, $@"?x-client-SKU={message.SkuTelemetryValue}&x-client-ver={typeof(OpenIdConnectMessage).GetTypeInfo().Assembly.GetName().Version}")
            );

            // Telemetry turned off
            OpenIdConnectMessage.EnableTelemetryParametersByDefault = false;
            message = new OpenIdConnectMessage();
            message.EnableTelemetryParameters = true;
            theoryData.Add(
                "TelemetryStaticFalseInstanceTrue",
                message,
                string.Format(CultureInfo.InvariantCulture, $@"?x-client-SKU={message.SkuTelemetryValue}&x-client-ver={typeof(OpenIdConnectMessage).GetTypeInfo().Assembly.GetName().Version}")
            );

            // Telemetry turned off using static switch
            OpenIdConnectMessage.EnableTelemetryParametersByDefault = false;
            message = new OpenIdConnectMessage();
            theoryData.Add(
                "TelemetryStaticFalse",
                message,
                ""
            );

            // Telemetry turned off using static switch, but turned on on the instance
            OpenIdConnectMessage.EnableTelemetryParametersByDefault = true;
            message = new OpenIdConnectMessage();
            message.EnableTelemetryParameters = false;
            theoryData.Add(
                "TelemetryStaticTrueInstanceFalse",
                message,
                ""
            );

            OpenIdConnectMessage.EnableTelemetryParametersByDefault = true;

            return theoryData;
        }

        [Theory, MemberData(nameof(CreateLogoutRequestUrlTheoryData))]
        public void OidcCreateLogoutRequestUrl(string testId, OpenIdConnectMessage message, string expectedMessage)
        {
            TestUtilities.WriteHeader("OidcCreateLogoutRequestUrl - " + testId, true);

            var context = new CompareContext();
#if NET461
            if (!message.SkuTelemetryValue.Equals("ID_NET461"))
                context.Diffs.Add($"{message.SkuTelemetryValue} != ID_NET461");
#elif NET472
            if (!message.SkuTelemetryValue.Equals("ID_NET472"))
                context.Diffs.Add($"{message.SkuTelemetryValue} != ID_NET472");
#elif NET6_0
            if (!message.SkuTelemetryValue.Equals("ID_NET6_0"))
                context.Diffs.Add($"{message.SkuTelemetryValue} != ID_NETCOREAPP3_1");
#elif NET8_0
            if (!message.SkuTelemetryValue.Equals("ID_NET8_0"))
                context.Diffs.Add($"{message.SkuTelemetryValue} != ID_NET8_0");
#elif NET_CORE
            if (!message.SkuTelemetryValue.Equals("ID_NETSTANDARD2_0"))
                context.Diffs.Add($"{message.SkuTelemetryValue} != ID_NETSTANDARD2_0");
#endif
            IdentityComparer.AreEqual(message.CreateLogoutRequestUrl(), expectedMessage, context);
            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<string, OpenIdConnectMessage, string> CreateLogoutRequestUrlTheoryData()
        {
            var theoryData = new TheoryData<string, OpenIdConnectMessage, string>();

            bool defaultValue = OpenIdConnectMessage.EnableTelemetryParametersByDefault;

            OpenIdConnectMessage.EnableTelemetryParametersByDefault = true;
            var message = new OpenIdConnectMessage();
            theoryData.Add(
                "Telemetry",
                message,
                string.Format(CultureInfo.InvariantCulture, $@"?x-client-SKU={message.SkuTelemetryValue}&x-client-ver={typeof(OpenIdConnectMessage).GetTypeInfo().Assembly.GetName().Version}")
            );

            // Telemetry turned off using static switch
            OpenIdConnectMessage.EnableTelemetryParametersByDefault = false;
            message = new OpenIdConnectMessage();
            theoryData.Add(
                "TelemetryStaticFalse",
                message,
                ""
            );

            // Telemetry turned off using static switch
            OpenIdConnectMessage.EnableTelemetryParametersByDefault = false;
            message = new OpenIdConnectMessage();
            message.EnableTelemetryParameters = true;
            theoryData.Add(
                "TelemetryStaticFalseInstanceTrue",
                message,
                string.Format(CultureInfo.InvariantCulture, $@"?x-client-SKU={message.SkuTelemetryValue}&x-client-ver={typeof(OpenIdConnectMessage).GetTypeInfo().Assembly.GetName().Version}")
            );

            OpenIdConnectMessage.EnableTelemetryParametersByDefault = defaultValue;

            return theoryData;
        }


        [Fact]
        public void NullFormParameters()
        {
            var msg = new OpenIdConnectMessage(new List<KeyValuePair<string, string[]>>
            {
                new KeyValuePair<string, string[]>("key", new string[] { "data" }),
                new KeyValuePair<string, string[]>("nullData", new string[] { null }),
                new KeyValuePair<string, string[]>("emptyData", new string[] { string.Empty }),
                new KeyValuePair<string, string[]>(null, new string[] { null }),
                new KeyValuePair<string, string[]>(null, null)
            });

            Assert.NotNull(msg);
        }

        private void Report(string id, List<string> errors, string url, string expected)
        {
            if (!string.Equals(url, expected, StringComparison.Ordinal))
                errors.Add("id: " + id + Environment.NewLine + "message.BuildRedirectUrl( != expected" + Environment.NewLine + Environment.NewLine + url + Environment.NewLine + Environment.NewLine + expected + Environment.NewLine);
        }

        [Fact]
        public void Extensibility()
        {
            var msg = new CustomOpenIdConnectMessage()
            {
                AuthenticationRequestUrl = Guid.NewGuid().ToString(),
                LogoutRequestUrl = Guid.NewGuid().ToString(),
            };

            Assert.True(msg.AuthenticationRequestUrl == msg.CreateAuthenticationRequestUrl(), "AuthenticationRequestUrl, CreateAuthenticationRequestUrl: " + msg.AuthenticationRequestUrl + ", " + msg.CreateAuthenticationRequestUrl());
            Assert.True(msg.LogoutRequestUrl == msg.CreateLogoutRequestUrl(), "LogoutRequestUrl, CreateLogoutRequestUrl(): " + msg.LogoutRequestUrl + ", " + msg.CreateLogoutRequestUrl());
        }

        [Fact]
        public void OpenIdConnectMessage_IssuerAddressHasQuery()
        {
            List<string> errors = new List<string>();
            var address = "http://gotJwt.onmicrosoft.com/?param=value";
            var clientId = Guid.NewGuid().ToString();
            var message = new OpenIdConnectMessage() { IssuerAddress = address };

            var url = message.BuildRedirectUrl();
            Report("1", errors, url, address);

            message.ClientId = clientId;
            url = message.BuildRedirectUrl();
            var expected = $"{address}&client_id={clientId}";

            Report("2", errors, url, expected);
        }

        private class CustomOpenIdConnectMessage : OpenIdConnectMessage
        {
            public override string CreateAuthenticationRequestUrl()
            {
                return AuthenticationRequestUrl;
            }

            public override string CreateLogoutRequestUrl()
            {
                return LogoutRequestUrl;
            }

            public string AuthenticationRequestUrl { get; set; }

            public string LogoutRequestUrl { get; set; }
        }

        public class OpenIdConnectMessageTheoryData : TheoryDataBase
        {
            public OpenIdConnectMessageTheoryData() { }

            public OpenIdConnectMessageTheoryData(string testId) : base(testId) { }

            public OpenIdConnectMessage Message { get; set; }
            
            public string Json { get; set; }

            internal JObject JObject { get; set; }
        }
    }
}

#pragma warning restore SYSLIB0013 // Type or member is obsolete
#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
