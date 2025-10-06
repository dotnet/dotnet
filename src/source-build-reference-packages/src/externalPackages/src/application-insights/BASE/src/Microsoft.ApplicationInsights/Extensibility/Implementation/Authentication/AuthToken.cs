namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Authentication
{
    using System;

    /// <summary>
    /// This represents the Azure.Core.AccessToken returned by Azure.Core.TokenCredential.
    /// (https://github.com/Azure/azure-sdk-for-net/blob/master/sdk/core/Azure.Core/src/AccessToken.cs).
    /// </summary>
    internal struct AuthToken : IEquatable<AuthToken>
    {
        /// <summary>
        /// Initializes a new instance of <see cref="AuthToken"/>.
        /// </summary>
        /// <param name="token">Access token.</param>
        /// <param name="expiresOn">DateTimeOffset representing when the access token expires.</param>
        public AuthToken(string token, DateTimeOffset expiresOn)
        {
            this.Token = token;
            this.ExpiresOn = expiresOn;
        }

        /// <summary>
        /// Gets or sets get the access token value.
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Gets or sets the time when the provided token expires.
        /// </summary>
        public DateTimeOffset ExpiresOn { get; set; }

        /// <summary>
        /// Determine if two instance of AuthToken are equal.
        /// </summary>
        /// <param name="left">An instance of AuthToken on the left side of the operator.</param>
        /// <param name="right">An instance of AuthToken on the right side of the operator.</param>
        /// <returns>Returns a boolean indicating if the params are equal.</returns>
        public static bool operator ==(AuthToken left, AuthToken right) => left.Equals(right);

        /// <summary>
        /// Determine if two instance of AuthToken are not equal.
        /// </summary>
        /// <param name="left">An instance of AuthToken on the left side of the operator.</param>
        /// <param name="right">An instance of AuthToken on the right side of the operator.</param>
        /// <returns>Returns a boolean indicating if the params are not equal.</returns>
        public static bool operator !=(AuthToken left, AuthToken right) => !left.Equals(right);

        /// <inheritdoc />
        public override bool Equals(object obj) => (obj is AuthToken authToken) && this.Equals(authToken);

        /// <inheritdoc />
        public override int GetHashCode() => this.Token.GetHashCode() ^ this.ExpiresOn.GetHashCode();

        /// <inheritdoc />
        public bool Equals(AuthToken other) => other.ExpiresOn == this.ExpiresOn && other.Token == this.Token;
    }
}
