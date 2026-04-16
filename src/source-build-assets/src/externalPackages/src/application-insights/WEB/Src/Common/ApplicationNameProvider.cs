namespace Microsoft.ApplicationInsights.Common
{
    using System;

    internal sealed class ApplicationNameProvider
    {
        public ApplicationNameProvider()
        {
            this.Name = GetApplicationName();
        }

        public string Name { get; private set; }

        private static string GetApplicationName()
        {
            string name;
            try
            {
                name = AppDomain.CurrentDomain.FriendlyName;
            }
            catch (Exception exp)
            {
                name = "Undefined " + exp.Message ?? exp.ToString();
            }

            return name;
        }
    }
}
