namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Authentication
{
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// This interface defines a class that can interact with Azure.Core.TokenCredential.
    /// See also: (https://github.com/Azure/azure-sdk-for-net/blob/master/sdk/core/Azure.Core/src/TokenCredential.cs).
    /// </summary>
    internal abstract class CredentialEnvelope
    {
        /// <summary>
        /// Gets the TokenCredential instance held by this class.
        /// </summary>
        internal abstract object Credential { get; }

        /// <summary>
        /// Gets an Azure.Core.AccessToken.
        /// </summary>
        /// <remarks>
        /// Whomever uses this MUST verify that it's called within <see cref="SdkInternalOperationsMonitor.Enter"/> otherwise dependency calls will be tracked.
        /// </remarks>
        /// <param name="cancellationToken">The System.Threading.CancellationToken to use.</param>
        /// <returns>A valid Azure.Core.AccessToken.</returns>
        public abstract AuthToken GetToken(CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets an Azure.Core.AccessToken.
        /// </summary>
        /// <remarks>
        /// Whomever uses this MUST verify that it's called within <see cref="SdkInternalOperationsMonitor.Enter"/> otherwise dependency calls will be tracked.
        /// </remarks>
        /// <param name="cancellationToken">The System.Threading.CancellationToken to use.</param>
        /// <returns>A valid Azure.Core.AccessToken.</returns>
        public abstract Task<AuthToken> GetTokenAsync(CancellationToken cancellationToken = default);
    }
}