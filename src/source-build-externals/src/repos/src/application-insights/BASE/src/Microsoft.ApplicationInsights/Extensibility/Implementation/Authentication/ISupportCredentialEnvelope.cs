namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Authentication
{
    /// <summary>
    /// This interface defines a class that accepts the <see cref="Authentication.CredentialEnvelope"/> as a property.
    /// </summary>
    internal interface ISupportCredentialEnvelope
    {
        /// <summary>
        /// Gets or sets the <see cref="Authentication.CredentialEnvelope"/>.
        /// </summary>
        CredentialEnvelope CredentialEnvelope { get; set; }
    }
}
