namespace Microsoft.ApplicationInsights.Extensibility
{
    /// <summary>
    /// An interface for providing an Application Id for a given Instrumentation Key.
    /// </summary>
    public interface IApplicationIdProvider
    {
        /// <summary>
        /// Retrieves the Application Id to be used for Request.Source or Dependency.Target.
        /// </summary>
        /// <remarks>
        /// Application Insights data model defines two fields: request.source and dependency.target. 
        /// The first field identifies the component that initiated the dependency request, 
        /// and the second identifies which component returned the response of the dependency call.
        /// For more information see: https://docs.microsoft.com/azure/application-insights/application-insights-correlation .
        /// </remarks>
        /// <param name="instrumentationKey">Instrumentation Key string used to lookup associated Application Id.</param>
        /// <param name="applicationId">Application Id corresponding to the Instrumentation Key. Returns NULL if a match was not found.</param>
        /// <returns>TRUE if Application Id was successfully retrieved; FALSE otherwise.</returns>
        bool TryGetApplicationId(string instrumentationKey, out string applicationId);
    }
}
