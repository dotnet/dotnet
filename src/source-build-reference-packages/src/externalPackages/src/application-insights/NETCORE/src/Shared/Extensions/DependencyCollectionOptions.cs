#if AI_ASPNETCORE_WEB
    namespace Microsoft.ApplicationInsights.AspNetCore.Extensions
#else
namespace Microsoft.ApplicationInsights.WorkerService
#endif
{
    /// <summary>
    /// Default collection options define the custom behavior or non-default features of dependency collection.
    /// </summary>
    public class DependencyCollectionOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyCollectionOptions"/> class and fills default values.
        /// </summary>
        public DependencyCollectionOptions()
        {
            this.EnableLegacyCorrelationHeadersInjection = false;
        }

        /// <summary>
        /// Gets or sets a value indicating whether to enable legacy (x-ms*) correlation headers injection.
        /// </summary>
        public bool EnableLegacyCorrelationHeadersInjection { get; set; }
    }
}
