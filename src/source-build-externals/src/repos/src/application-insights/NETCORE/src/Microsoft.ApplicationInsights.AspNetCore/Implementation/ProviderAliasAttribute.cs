namespace Microsoft.Extensions.Logging
{
    using System;

    /// <summary>
    /// Controls logger provider alias used for configuration.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    internal class ProviderAliasAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProviderAliasAttribute" /> class.
        /// </summary>
        /// <param name="alias">Sets an alias that can be used instead of full type name.</param>
        public ProviderAliasAttribute(string alias) => this.Alias = alias;

        /// <summary>
        /// Gets an alias that can be used instead of full type name during configuration.
        /// </summary>
        public string Alias { get; }
    }
}