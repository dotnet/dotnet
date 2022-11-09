namespace Microsoft.ApplicationInsights.TestFramework
{
    using System;

    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;

    internal class StubPlatform : IPlatform
    {
        public Func<IDebugOutput> OnGetDebugOutput = () => new StubDebugOutput();
        public Func<string> OnReadConfigurationXml = () => null;
        public Func<string> OnGetMachineName = () => null;

        public string ReadConfigurationXml()
        {
            return this.OnReadConfigurationXml();
        }

        public IDebugOutput GetDebugOutput()
        {
            return this.OnGetDebugOutput();
        }

        public virtual bool TryGetEnvironmentVariable(string name, out string value)
        {
            value = string.Empty;

            try
            {
                value = Environment.GetEnvironmentVariable(name);
                return !string.IsNullOrEmpty(value);
            }
            catch (Exception e)
            {
                CoreEventSource.Log.FailedToLoadEnvironmentVariables(e.ToString());
            }

            return false;
        }

        public string GetMachineName()
        {
            return this.OnGetMachineName();
        }
    }
}
