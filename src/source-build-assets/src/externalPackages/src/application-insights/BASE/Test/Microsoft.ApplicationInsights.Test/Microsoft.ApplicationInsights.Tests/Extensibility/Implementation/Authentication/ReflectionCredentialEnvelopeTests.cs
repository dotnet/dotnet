#if !NET452 && !NET46 && !REDFIELD
namespace Microsoft.ApplicationInsights.TestFramework.Extensibility.Implementation.Authentication
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;

    using Azure.Core;

    using Microsoft.ApplicationInsights.Extensibility.Implementation.Authentication;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    /// <summary>
    /// The <see cref="ReflectionCredentialEnvelope"/> cannot take a dependency on <see cref="Azure.Core.TokenCredential"/>.
    /// We must use reflection to interact with this class.
    /// These tests are to confirm that we can correctly identity classes that implement TokenCredential and address it's methods.
    /// </summary>
    /// <remarks>
    /// These tests do not run in NET452 OR NET46. 
    /// In these cases, the test runner is NET452 or NET46 and Azure.Core.TokenCredential is NOT SUPPORTED in these frameworks.
    /// This does not affect the end user because we REQUIRE the end user to create their own instance of TokenCredential.
    /// This ensures that the end user is consuming the AI SDK in one of the newer frameworks.
    /// </remarks>
    [TestClass]
    [TestCategory("AAD")]
    public class ReflectionCredentialEnvelopeTests
    {
        [TestMethod]
        public void VerifyCanIdentifyValidClass()
        {
            var testClass2 = new TestClassInheritsTokenCredential();
            _ = new ReflectionCredentialEnvelope(testClass2);
            // NO ASSERT. This test is valid if no exception is thrown. :)
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void VerifyCanIdentityInvalidClass()
        {
            var notTokenCredential2 = new NotTokenCredential2();
            _ = new ReflectionCredentialEnvelope(notTokenCredential2);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void VerifyCannotSetInvalidType()
        {
            _ = new ReflectionCredentialEnvelope(Guid.Empty);
        }

        [TestMethod]
        public void VerifyCanMakeTokenRequestContext()
        {
            var testScope = new string[] { "test/scope" };

            var requestContext = new TokenRequestContext(testScope);

            var tokenRequestContextViaReflection = ReflectionCredentialEnvelope.AzureCore.MakeTokenRequestContext(testScope);
            Assert.IsInstanceOfType(tokenRequestContextViaReflection, typeof(TokenRequestContext));
            Assert.AreEqual(requestContext, tokenRequestContextViaReflection);
        }

        [TestMethod]
        public void VerifyGetToken_UsingCompileTimeTypes()
        {
            var mockCredential = new MockCredential();
            var requestContext = new TokenRequestContext(new string[] { "test/scope" });
            var tokenFromCredential = mockCredential.GetToken(requestContext, CancellationToken.None);

            var tokenFromReflection = ReflectionCredentialEnvelope.AzureCore.InvokeGetToken(mockCredential, requestContext, CancellationToken.None);
            
            Assert.AreEqual(tokenFromCredential.Token, tokenFromReflection.Token);
            Assert.AreEqual(tokenFromCredential.ExpiresOn, tokenFromReflection.ExpiresOn);
        }

        [TestMethod]
        public async Task VerifyGetTokenAsync_UsingCompileTimeTypes()
        {
            var mockCredential = new MockCredential();
            var requestContext = new TokenRequestContext(new string[] { "test/scope" });
            var tokenFromCredential = mockCredential.GetToken(requestContext, CancellationToken.None);

            var tokenFromReflection = await ReflectionCredentialEnvelope.AzureCore.InvokeGetTokenAsync(mockCredential, requestContext, CancellationToken.None);

            Assert.AreEqual(tokenFromCredential.Token, tokenFromReflection.Token);
            Assert.AreEqual(tokenFromCredential.ExpiresOn, tokenFromReflection.ExpiresOn);
        }

        /// <summary>
        /// This more closely represents how this would be used in a production environment.
        /// </summary>
        [TestMethod]
        public void VerifyGetToken_UsingDynamicTypes()
        {
            var mockCredential = new MockCredential();
            var requestContext = new TokenRequestContext(new string[] { "test/scope" });
            var tokenFromCredential = mockCredential.GetToken(requestContext, CancellationToken.None);

            var requestContextFromReflection = ReflectionCredentialEnvelope.AzureCore.MakeTokenRequestContext(new[] { "test/scope" });
            var tokenFromReflection = ReflectionCredentialEnvelope.AzureCore.InvokeGetToken((object)mockCredential, requestContextFromReflection, CancellationToken.None);
            
            Assert.AreEqual(tokenFromCredential.Token, tokenFromReflection.Token);
            Assert.AreEqual(tokenFromCredential.ExpiresOn, tokenFromReflection.ExpiresOn);
        }

        /// <summary>
        /// This more closely represents how this would be used in a production environment.
        /// </summary>
        [TestMethod]
        public async Task VerifyGetTokenAsync_UsingDynamicTypes()
        {
            var mockCredential = new MockCredential();
            var requestContext = new TokenRequestContext(new string[] { "test/scope" });
            var tokenFromCredential = mockCredential.GetToken(requestContext, CancellationToken.None);

            var requestContextFromReflection = ReflectionCredentialEnvelope.AzureCore.MakeTokenRequestContext(new[] { "test/scope" });
            var tokenFromReflection = await ReflectionCredentialEnvelope.AzureCore.InvokeGetTokenAsync((object)mockCredential, requestContextFromReflection, CancellationToken.None);

            Assert.AreEqual(tokenFromCredential.Token, tokenFromReflection.Token);
            Assert.AreEqual(tokenFromCredential.ExpiresOn, tokenFromReflection.ExpiresOn);
        }

        /// <summary>
        /// This test verifies that both <see cref="Azure.Core"/> and <see cref="ReflectionCredentialEnvelope"/> return identical tokens.
        /// </summary>
        [TestMethod]
        public void VerifyGetToken_ReturnsValidToken()
        {
            var requestContext = new TokenRequestContext(scopes: AuthConstants.GetScopes());
            var mockCredential = new MockCredential();
            var tokenFromCredential = mockCredential.GetToken(requestContext, CancellationToken.None);

            var reflectionCredentialEnvelope = new ReflectionCredentialEnvelope(mockCredential);
            var tokenFromReflection = reflectionCredentialEnvelope.GetToken();

            Assert.AreEqual(tokenFromCredential.Token, tokenFromReflection.Token);
            Assert.AreEqual(tokenFromCredential.ExpiresOn, tokenFromReflection.ExpiresOn);
        }

        /// <summary>
        /// This test verifies that both <see cref="Azure.Core"/> and <see cref="ReflectionCredentialEnvelope"/> return identical tokens.
        /// </summary>
        [TestMethod]
        public async Task VerifyGetTokenAsync_ReturnsValidToken()
        {
            var requestContext = new TokenRequestContext(scopes: AuthConstants.GetScopes());
            var mockCredential = new MockCredential();
            var tokenFromCredential = await mockCredential.GetTokenAsync(requestContext, CancellationToken.None);

            var reflectionCredentialEnvelope = new ReflectionCredentialEnvelope(mockCredential);
            var tokenFromReflection = await reflectionCredentialEnvelope.GetTokenAsync();

            Assert.AreEqual(tokenFromCredential.Token, tokenFromReflection.Token);
            Assert.AreEqual(tokenFromCredential.ExpiresOn, tokenFromReflection.ExpiresOn);
        }

        [TestMethod]
        public void VerifyGetToken_IfCredentialThrowsException_EnvelopeReturnsNull()
        {
            Mock<TokenCredential> mockTokenCredential = new Mock<TokenCredential>();
            mockTokenCredential.Setup(x => x.GetToken(It.IsAny<TokenRequestContext>(), It.IsAny<CancellationToken>())).Throws(new NotImplementedException());
            var mockCredential = mockTokenCredential.Object;

            var reflectionCredentialEnvelope = new ReflectionCredentialEnvelope(mockCredential);
            var token = reflectionCredentialEnvelope.GetToken();
            Assert.AreEqual(default(AuthToken), token);
        }
        
        [TestMethod]
        public async Task VerifyGetTokenAsync_IfCredentialThrowsException_EnvelopeReturnsNull()
        {
            Mock<TokenCredential> mockTokenCredential = new Mock<TokenCredential>();
            mockTokenCredential.Setup(x => x.GetTokenAsync(It.IsAny<TokenRequestContext>(), It.IsAny<CancellationToken>())).Throws(new NotImplementedException());
            var mockCredential = mockTokenCredential.Object;

            var reflectionCredentialEnvelope = new ReflectionCredentialEnvelope(mockCredential);
            var token = await reflectionCredentialEnvelope.GetTokenAsync();
            Assert.AreEqual(default(AuthToken), token);
        }

        [TestMethod]
        public void TestReflection()
        {
            Type typeTokenCredential = Type.GetType("Azure.Core.TokenCredential, Azure.Core");
            Type typeTokenRequestContext = Type.GetType("Azure.Core.TokenRequestContext, Azure.Core");
            Type typeCancellationToken = typeof(CancellationToken);

            var parameterExpression_tokenCredential = Expression.Parameter(type: typeTokenCredential, name: "parameterExpression_TokenCredential");
            var parameterExpression_requestContext = Expression.Parameter(type: typeTokenRequestContext, name: "parameterExpression_RequestContext");
            var parameterExpression_cancellationToken = Expression.Parameter(type: typeCancellationToken, name: "parameterExpression_CancellationToken");

            var exprGetToken = Expression.Call(
                instance: parameterExpression_tokenCredential,
                method: typeTokenCredential.GetMethod(name: "GetToken", types: new Type[] { typeTokenRequestContext, typeCancellationToken }),
                arg0: parameterExpression_requestContext,
                arg1: parameterExpression_cancellationToken);

            var exprTokenProperty = Expression.Property(
                expression: exprGetToken,
                propertyName: "Token");

            var exprExpiresOnProperty = Expression.Property(
                expression: exprGetToken,
                propertyName: "ExpiresOn");

            Type typeAuthToken = typeof(AuthToken);
            ConstructorInfo authTokenCtor = typeAuthToken.GetConstructor(new Type[] { typeof(string), typeof(DateTimeOffset) });

            var exprAuthTokenCtor = Expression.New(authTokenCtor, exprTokenProperty, exprExpiresOnProperty);
                //Expression.init


            var compiledExpression = Expression.Lambda(
                    //body: exprTokenProperty,
                    body: exprAuthTokenCtor,
                    parameters: new ParameterExpression[]
                    {
                        parameterExpression_tokenCredential,
                        parameterExpression_requestContext,
                        parameterExpression_cancellationToken,
                    }).Compile();

            //----------------------------------

            var mockCredential = new MockCredential();
            var requestContext = new TokenRequestContext(new string[] { "test/scope" });
            CancellationToken ct = default;

            var test = (AuthToken)compiledExpression.DynamicInvoke(mockCredential, requestContext, ct);

        }


        [TestMethod]
        public void TestReflection2()
        {
            Type typeTokenCredential = Type.GetType("Azure.Core.TokenCredential, Azure.Core");
            Type typeTokenRequestContext = Type.GetType("Azure.Core.TokenRequestContext, Azure.Core");
            Type typeCancellationToken = typeof(CancellationToken);

            var parameterExpression_tokenCredential = Expression.Parameter(type: typeTokenCredential, name: "parameterExpression_TokenCredential");
            var parameterExpression_requestContext = Expression.Parameter(type: typeTokenRequestContext, name: "parameterExpression_RequestContext");
            var parameterExpression_cancellationToken = Expression.Parameter(type: typeCancellationToken, name: "parameterExpression_CancellationToken");

            var exprGetToken = Expression.Call(
                instance: parameterExpression_tokenCredential,
                method: typeTokenCredential.GetMethod(name: "GetToken", types: new Type[] { typeTokenRequestContext, typeCancellationToken }),
                arg0: parameterExpression_requestContext,
                arg1: parameterExpression_cancellationToken);

            var compiledExpression1 = Expression.Lambda(
                    body: exprGetToken,
                    parameters: new ParameterExpression[]
                    {
                        parameterExpression_tokenCredential,
                        parameterExpression_requestContext,
                        parameterExpression_cancellationToken,
                    }).Compile();

            // ------------------------------

            var mockCredential = new MockCredential();
            var requestContext = new TokenRequestContext(new string[] { "test/scope" });
            CancellationToken ct = default;

            var objAccessToken = compiledExpression1.DynamicInvoke(mockCredential, requestContext, ct);


            // ----------------------------

            Type typeAccessToken = Type.GetType("Azure.Core.AccessToken, Azure.Core");

            var parameterExpression_AccessToken = Expression.Parameter(typeAccessToken, "parameterExpression_AccessToken");

            var exprTokenProperty = Expression.Property(
                expression: parameterExpression_AccessToken,
                propertyName: "Token");

            var exprExpiresOnProperty = Expression.Property(
                expression: parameterExpression_AccessToken,
                propertyName: "ExpiresOn");

            Type typeAuthToken = typeof(AuthToken);
            ConstructorInfo authTokenCtor = typeAuthToken.GetConstructor(new Type[] { typeof(string), typeof(DateTimeOffset) });

            var exprAuthTokenCtor = Expression.New(authTokenCtor, exprTokenProperty, exprExpiresOnProperty);
            
            var compiledExpression2 = Expression.Lambda(
                    //body: exprTokenProperty,
                    body: exprAuthTokenCtor,
                    parameters: new ParameterExpression[]
                    {
                        parameterExpression_AccessToken,
                    }).Compile();

            //----------------------------------

            var test = compiledExpression2.DynamicInvoke(objAccessToken);

        }
#region TestClasses

        /// <summary>
        /// This class inherits <see cref="MockCredential"/> which inherits <see cref="Azure.Core.TokenCredential"/>.
        /// This class is used to verify that the <see cref="ReflectionCredentialEnvelope"/> can correctly identify tests that inherit <see cref="Azure.Core.TokenCredential"/>.
        /// </summary>
        private class TestClassInheritsTokenCredential : MockCredential { }

        private abstract class NotTokenCredential
        {
            public abstract AccessToken GetToken(TokenRequestContext requestContext, CancellationToken cancellationToken);

            public abstract ValueTask<AccessToken> GetTokenAsync(TokenRequestContext requestContext, CancellationToken cancellationToken);
        }

        private class NotTokenCredential1 : NotTokenCredential
        {
            public override AccessToken GetToken(TokenRequestContext requestContext, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }

            public override ValueTask<AccessToken> GetTokenAsync(TokenRequestContext requestContext, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }
        }

        private class NotTokenCredential2 : NotTokenCredential1 { }
#endregion
    }
}
#endif
