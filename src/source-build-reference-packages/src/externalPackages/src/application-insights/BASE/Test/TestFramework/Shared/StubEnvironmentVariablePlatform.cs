namespace Microsoft.ApplicationInsights.TestFramework
{
    using System.Collections.Generic;

    /// <summary>
    /// This class can be used to mock environment variables
    /// </summary>
    internal class StubEnvironmentVariablePlatform : StubPlatform
    {
        private readonly Dictionary<string, string> environmentVariables = new Dictionary<string, string>();

        public void SetEnvironmentVariable(string name, string value) => this.environmentVariables.Add(name, value);

        public override bool TryGetEnvironmentVariable(string name, out string value) => this.environmentVariables.TryGetValue(name, out value);

    }
}