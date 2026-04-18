namespace Microsoft.ApplicationInsights.Tests
{
    using System.IO;
    using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.WebAppPerfCollector;

    internal class CacheHelperTests : ICachedEnvironmentVariableAccess
    {
        private bool returnJsonOne = true;

        private string jsonOne = File.ReadAllText(@"WebAppPerformanceCollector\SampleFiles\RemoteEnvironmentVariablesAllSampleOne.json");

        private string jsonTwo = File.ReadAllText(@"WebAppPerformanceCollector\SampleFiles\RemoteEnvironmentVariablesAllSampleTwo.json");

        /// <summary>
        /// Retrieves raw counter data from Environment Variables.
        /// </summary>
        /// <param name="name"> Name of the counter to be selected from JSON.</param>
        /// <param name="environmentVariable">Environment variables - unused.</param>
        /// <returns> Value of the counter.</returns>
        public long GetCounterValue(string name, AzureWebApEnvironmentVariables environmentVariable)
        {
            if (this.returnJsonOne)
            {
                this.returnJsonOne = false;
                return CacheHelper.PerformanceCounterValue(name, this.jsonOne);
            }
            else
            {
                this.returnJsonOne = true;
                return CacheHelper.PerformanceCounterValue(name, this.jsonTwo);
            }
        }
    }
}