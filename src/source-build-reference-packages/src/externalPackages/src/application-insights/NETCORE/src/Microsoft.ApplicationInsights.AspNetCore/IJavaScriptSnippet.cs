namespace Microsoft.ApplicationInsights.AspNetCore
{
    /// <summary>
    /// Represents factory used to generate Application Insights JavaScript snippet with dependency injection support.
    /// </summary>
    public interface IJavaScriptSnippet
    {
        /// <summary>
        /// Gets a JavaScript code snippet including the 'script' tag.
        /// </summary>
        /// <returns>JavaScript code snippet.</returns>
        string FullScript { get; }
    }
}