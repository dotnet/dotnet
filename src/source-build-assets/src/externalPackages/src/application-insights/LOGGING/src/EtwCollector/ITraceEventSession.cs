//-----------------------------------------------------------------------
// <copyright file="ITraceEventSession.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.ApplicationInsights.EtwCollector
{
    using System;
    using Microsoft.Diagnostics.Tracing;
    using Microsoft.Diagnostics.Tracing.Session;

    /// <summary>
    /// Abstracts properties and methods of <see cref="Microsoft.Diagnostics.Tracing.Session.TraceEventSession"/> to provide better test ability.
    /// </summary>
    internal interface ITraceEventSession : IDisposable
    {
        /// <summary>
        /// Gets the TraceEventDispatcher for the session.
        /// </summary>
        /// <remarks>
        /// If this is a real time session you can fetch the source associated with the session
        /// to start receiving events. Currently does not work on file based sources (we
        /// expect you to wait until the file is complete).
        /// </remarks>
        TraceEventDispatcher Source { get; }

        /// <summary>
        /// Enables a provider by its name, level and keywords.
        /// </summary>
        /// <param name="providerName">
        /// The name of the provider.
        /// </param>
        /// <param name="providerLevel">
        /// The verbosity to turn on.
        /// </param>
        /// <param name="matchAnyKeywords">
        /// A bit vector representing the areas to turn on. Only the low 32 bits are used
        /// by classic providers and passed as the 'flags' value. Zero is a special value
        /// which is a provider defined default, which is usually 'everything'.
        /// </param>
        /// <param name="options">
        /// Additional options for the provider (e.g. taking a stack trace), arguments ...
        /// </param>
        /// <returns>true if the session already existed and needed to be restarted.</returns>
        bool EnableProvider(string providerName, TraceEventLevel providerLevel = TraceEventLevel.Verbose, ulong matchAnyKeywords = ulong.MaxValue, TraceEventProviderOptions options = null);

        /// <summary>
        /// Enable a provider which has a given provider Guid.
        /// </summary>
        /// <param name="providerGuid">The Guid that represents the event provider enable.</param>
        /// <param name="providerLevel">The verbosity to turn on.</param>
        /// <param name="matchAnyKeywords">
        /// A bit vector representing the areas to turn on. Only the low 32 bits are used
        /// by classic providers and passed as the 'flags' value. Zero is a special value
        /// which is a provider defined default, which is usually 'everything'.
        /// </param>
        /// <param name="options">Additional options for the provider (e.g. taking a stack trace), arguments ...</param>
        /// <returns>true if the session already existed and needed to be restarted.</returns>
        bool EnableProvider(Guid providerGuid, TraceEventLevel providerLevel = TraceEventLevel.Verbose, ulong matchAnyKeywords = ulong.MaxValue, TraceEventProviderOptions options = null);

        /// <summary>
        /// Disables a provider with the given name completely.
        /// </summary>
        /// <param name="providerName">Name of the provider to disable.</param>
        void DisableProvider(string providerName);

        /// <summary>
        /// Disables a provider with the given provider ID completely.
        /// </summary>
        /// <param name="providerGuid">GUID of the provider to disable.</param>
        void DisableProvider(Guid providerGuid);
    }
}
