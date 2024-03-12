namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Authentication
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;

    /// <summary>
    /// This is an envelope for an instance of Azure.Core.TokenCredential.
    /// This class uses reflection to interact with the Azure.Core library.
    /// </summary>
    /// <remarks>
    /// Our SDK currently targets net452, net46, and netstandard2.0.
    /// Azure.Core.TokenCredential is only available for netstandard2.0.
    /// </remarks>
    internal class ReflectionCredentialEnvelope : CredentialEnvelope
    {
#if REDFIELD
        private static volatile string azureCoreAssemblyName = "Azure.Identity.ILRepack";
#else
        private static volatile string azureCoreAssemblyName = "Azure.Core";
#endif

        private readonly object tokenCredential;
        private readonly object tokenRequestContext;

        /// <summary>
        /// Create an instance of <see cref="ReflectionCredentialEnvelope"/>.
        /// </summary>
        /// <param name="tokenCredential">An instance of Azure.Core.TokenCredential.</param>
        public ReflectionCredentialEnvelope(object tokenCredential)
        {
            this.tokenCredential = tokenCredential ?? throw new ArgumentNullException(nameof(tokenCredential));

            if (IsValidType(tokenCredential))
            {
                this.tokenRequestContext = AzureCore.MakeTokenRequestContext(scopes: AuthConstants.GetScopes());
            }
            else
            {
                throw new ArgumentException($"The provided {nameof(tokenCredential)} must inherit Azure.Core.TokenCredential", nameof(tokenCredential));
            }
        }

        /// <summary>
        /// Gets the TokenCredential instance held by this class.
        /// </summary>
        internal override object Credential => this.tokenCredential;

        /// <summary>
        /// Gets an Azure.Core.AccessToken.
        /// </summary>
        /// <remarks>
        /// Whomever uses this MUST verify that it's called within <see cref="SdkInternalOperationsMonitor.Enter"/> otherwise dependency calls will be tracked.
        /// </remarks>
        /// <param name="cancellationToken">The System.Threading.CancellationToken to use.</param>
        /// <returns>A valid Azure.Core.AccessToken.</returns>
        public override AuthToken GetToken(CancellationToken cancellationToken = default)
        {
            try
            {
                return AzureCore.InvokeGetToken(this.tokenCredential, this.tokenRequestContext, cancellationToken);
            }
            catch (Exception ex)
            {
                CoreEventSource.Log.FailedToGetToken(ex.ToInvariantString());
                return default;
            }
        }

        /// <summary>
        /// Gets an Azure.Core.AccessToken.
        /// </summary>
        /// <remarks>
        /// Whomever uses this MUST verify that it's called within <see cref="SdkInternalOperationsMonitor.Enter"/> otherwise dependency calls will be tracked.
        /// </remarks>
        /// <param name="cancellationToken">The System.Threading.CancellationToken to use.</param>
        /// <returns>A valid Azure.Core.AccessToken.</returns>
        public override async Task<AuthToken> GetTokenAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                return await AzureCore.InvokeGetTokenAsync(this.tokenCredential, this.tokenRequestContext, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                CoreEventSource.Log.FailedToGetToken(ex.ToInvariantString());
                return default;
            }
        }

        private static bool IsValidType(object inputTokenCredential) => inputTokenCredential.GetType().IsSubclassOf(GetTokenCredentialType());

        /// <summary>
        /// Use reflection to get a <see cref="Type"/> of "Azure.Core.TokenCredential".
        /// This will fail if the Azure.Core library is not loaded into the AppDomain.CurrentDomain.
        /// </summary>
        /// <remarks>
        /// It is unlikely that customers will have a direct dependency on Azure.Core.
        /// This is expected to be an indirect dependency from Azure.Identity.
        /// </remarks>
        /// <returns>
        /// Returns a <see cref="Type"/> of "Azure.Core.TokenCredential".
        /// </returns>
        private static Type GetTokenCredentialType()
        {
            var typeName = $"Azure.Core.TokenCredential, {azureCoreAssemblyName}";

            Type typeTokenCredential = null;

            try
            {
                typeTokenCredential = Type.GetType(typeName);
            }
            catch (Exception ex)
            {
                throw new Exception("An error has occurred while trying to get type Azure.Core.TokenCredential. See inner exception.", ex);
            }

            if (typeTokenCredential == null)
            {
                if (AppDomain.CurrentDomain.GetAssemblies().Any(x => x.FullName.StartsWith(azureCoreAssemblyName, StringComparison.Ordinal)))
                {
                    throw new Exception("An unknown error has occurred. Failed to get type Azure.Core.TokenCredential. Detected that Azure.Core is loaded in AppDomain.CurrentDomain.");
                }
                else
                {
                    throw new Exception("Failed to get type Azure.Core.TokenCredential. Azure.Core is not found in AppDomain.CurrentDomain.");
                }
            }

            return typeTokenCredential;
        }

        /// <summary>
        /// This class provides Reflection based wrappers around types found in the Azure.Core library.
        /// Because of framework incompatibilities, we cannot take a direct reference on these types.
        /// 
        /// This class uses compiled Expression Trees. Read more here: 
        /// (https://docs.microsoft.com/dotnet/csharp/programming-guide/concepts/expression-trees/).
        /// (https://docs.microsoft.com/dotnet/csharp/expression-trees).
        /// </summary>
        internal static class AzureCore
        {
            private static readonly Delegate GetTokenValue = BuildDelegateGetToken();
            private static readonly Delegate GetTokenAsyncAsTask = BuildDelegateGetTokenAsync();
            private static readonly Delegate GetTaskResult = BuildGetTaskResult();
            private static readonly Delegate AccessTokenToAuthToken = BuildDelegateAccessTokenToAuthToken();

            internal static AuthToken InvokeGetToken(object tokenCredential, object tokenRequestContext, CancellationToken cancellationToken)
            {
                var objAccessToken = GetTokenValue.DynamicInvoke(tokenCredential, tokenRequestContext, cancellationToken);
                return (AuthToken)AccessTokenToAuthToken.DynamicInvoke(objAccessToken);
            }

            internal static async Task<AuthToken> InvokeGetTokenAsync(object tokenCredential, object tokenRequestContext, CancellationToken cancellationToken)
            {
                var task = (Task)GetTokenAsyncAsTask.DynamicInvoke(tokenCredential, tokenRequestContext, cancellationToken);
                await task.ConfigureAwait(false);

                var objAccessToken = GetTaskResult.DynamicInvoke(task);
                return (AuthToken)AccessTokenToAuthToken.DynamicInvoke(objAccessToken);
            }

            /// <summary>
            /// This is a wrapper for the following constructor:
            /// <code>public TokenRequestContext (string[] scopes, string? parentRequestId = default, string? claims = default);</code>
            /// (https://docs.microsoft.com/dotnet/api/azure.core.tokenrequestcontext.-ctor).
            /// </summary>
            internal static object MakeTokenRequestContext(string[] scopes)
            {
                return Activator.CreateInstance(
                    type: Type.GetType($"Azure.Core.TokenRequestContext, {azureCoreAssemblyName}"),
                    args: new object[] { scopes, null, });
            }

            /// <summary>
            /// This is a wrapper for Azure.Core.AccessToken:
            /// <code>public struct AccessToken</code>
            /// (https://docs.microsoft.com/dotnet/api/azure.core.accesstoken).
            /// </summary>
            /// <returns>
            /// Returns a delegate that receives an Azure.Core.AccessToken and emits an <see cref="AuthToken"/>.
            /// </returns>
            private static Delegate BuildDelegateAccessTokenToAuthToken()
            {
                Type typeAccessToken = Type.GetType($"Azure.Core.AccessToken, {azureCoreAssemblyName}");

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

                return Expression.Lambda(
                    body: exprAuthTokenCtor,
                    parameters: new ParameterExpression[]
                    {
                        parameterExpression_AccessToken,
                    }).Compile();
            }

            /// <summary>
            /// This creates a wrapper for the following method:
            /// <code>public abstract Azure.Core.AccessToken GetToken (Azure.Core.TokenRequestContext requestContext, System.Threading.CancellationToken cancellationToken).</code>
            /// (https://docs.microsoft.com/dotnet/api/azure.core.tokencredential.gettoken).
            /// </summary>
            /// <returns>
            /// Returns a delegate that receives an Azure.Core.TokenCredential and emits an Azure.Core.AccessToken.
            /// </returns>
            private static Delegate BuildDelegateGetToken()
            {
                Type typeTokenCredential = Type.GetType($"Azure.Core.TokenCredential, {azureCoreAssemblyName}");
                Type typeTokenRequestContext = Type.GetType($"Azure.Core.TokenRequestContext, {azureCoreAssemblyName}");
                Type typeCancellationToken = typeof(CancellationToken);

                var parameterExpression_tokenCredential = Expression.Parameter(type: typeTokenCredential, name: "parameterExpression_TokenCredential");
                var parameterExpression_requestContext = Expression.Parameter(type: typeTokenRequestContext, name: "parameterExpression_RequestContext");
                var parameterExpression_cancellationToken = Expression.Parameter(type: typeCancellationToken, name: "parameterExpression_CancellationToken");

                var exprGetToken = Expression.Call(
                    instance: parameterExpression_tokenCredential,
                    method: typeTokenCredential.GetMethod(name: "GetToken", types: new Type[] { typeTokenRequestContext, typeCancellationToken }),
                    arg0: parameterExpression_requestContext,
                    arg1: parameterExpression_cancellationToken);

                return Expression.Lambda(
                    body: exprGetToken,
                    parameters: new ParameterExpression[]
                    {
                        parameterExpression_tokenCredential,
                        parameterExpression_requestContext,
                        parameterExpression_cancellationToken,
                    }).Compile();
            }

            /// <summary>
            /// This is a wrapper for the following method:
            /// <code>public abstract System.Threading.Tasks.ValueTask&lt;Azure.Core.AccessToken> GetTokenAsync (Azure.Core.TokenRequestContext requestContext, System.Threading.CancellationToken cancellationToken);</code>
            /// (https://docs.microsoft.com/dotnet/api/azure.core.tokencredential.gettokenasync).
            /// </summary>
            /// <returns>
            /// Returns a delegate that is a wrapper around GetTokenAsync which returns a System.Threading.Tasks.ValueTask of Azure.Core.AccessToken.
            /// Then converts that System.Threading.Tasks.ValueTask to <see cref="Task"/> which can be awaited.
            /// NOTE: The Expression Tree library cannot handle async methods.
            /// NOTE: ValueTask is not recognized by older versions of .NET Framework.
            /// </returns>
            private static Delegate BuildDelegateGetTokenAsync()
            {
                Type typeTokenCredential = Type.GetType($"Azure.Core.TokenCredential, {azureCoreAssemblyName}");
                Type typeTokenRequestContext = Type.GetType($"Azure.Core.TokenRequestContext, {azureCoreAssemblyName}");
                Type typeCancellationToken = typeof(CancellationToken);

                var parameterExpression_TokenCredential = Expression.Parameter(type: typeTokenCredential, name: "parameterExpression_TokenCredential");
                var parameterExpression_RequestContext = Expression.Parameter(type: typeTokenRequestContext, name: "parameterExpression_RequestContext");
                var parameterExpression_CancellationToken = Expression.Parameter(type: typeCancellationToken, name: "parameterExpression_CancellationToken");

                var methodInfo_GetTokenAsync = typeTokenCredential.GetMethod(name: "GetTokenAsync", types: new Type[] { typeTokenRequestContext, typeCancellationToken });

                var exprGetTokenAsync = Expression.Call(
                    instance: parameterExpression_TokenCredential,
                    method: methodInfo_GetTokenAsync,
                    arg0: parameterExpression_RequestContext,
                    arg1: parameterExpression_CancellationToken);

                var methodInfo_AsTask = methodInfo_GetTokenAsync.ReturnType.GetMethod("AsTask");

                var exprAsTask = Expression.Call(
                    instance: exprGetTokenAsync,
                    method: methodInfo_AsTask);

                return Expression.Lambda(
                    body: exprAsTask,
                    parameters: new ParameterExpression[]
                    {
                        parameterExpression_TokenCredential,
                        parameterExpression_RequestContext,
                        parameterExpression_CancellationToken,
                    }).Compile();
            }

            /// <summary>
            /// This is a wrapper for a <see cref="Task"/> that came from Azure.Core.AccessToken.GetTokenAsync.
            /// <code>public abstract System.Threading.Tasks.ValueTask&lt;Azure.Core.AccessToken> GetTokenAsync (Azure.Core.TokenRequestContext requestContext, System.Threading.CancellationToken cancellationToken);</code>
            /// (https://docs.microsoft.com/dotnet/api/azure.core.tokencredential.gettokenasync).
            /// </summary>
            /// <returns>
            /// Returns a delegate which receives a <see cref="Task"/> and emits an Azure.Core.AccessToken.
            /// </returns>
            private static Delegate BuildGetTaskResult()
            {
                Type typeTokenCredential = Type.GetType($"Azure.Core.TokenCredential, {azureCoreAssemblyName}");
                Type typeTokenRequestContext = Type.GetType($"Azure.Core.TokenRequestContext, {azureCoreAssemblyName}");
                Type typeCancellationToken = typeof(CancellationToken);
                var methodInfo_GetTokenAsync = typeTokenCredential.GetMethod(name: "GetTokenAsync", types: new Type[] { typeTokenRequestContext, typeCancellationToken });
                var methodInfo_AsTask = methodInfo_GetTokenAsync.ReturnType.GetMethod("AsTask");
                
                var parameterExpression_Task = Expression.Parameter(type: methodInfo_AsTask.ReturnType, name: "parameterExpression_Task");

                var exprResultProperty = Expression.Property(
                    expression: parameterExpression_Task,
                    propertyName: "Result");

                return Expression.Lambda(
                    body: exprResultProperty,
                    parameters: new ParameterExpression[]
                    {
                        parameterExpression_Task,
                    }).Compile();
            }
        }
    }
}
