namespace Microsoft.ApplicationInsights.AspNetCore.Implementation
{
    using System;
    using System.Threading;

    using Microsoft.AspNetCore.Http;

    /// <summary>
    /// Static container that holds the RoleName.
    /// </summary>
    internal class RoleNameContainer
    {
        private const string WebAppHostNameHeaderName = "WAS-DEFAULT-HOSTNAME";
        private const string WebAppHostNameEnvironmentVariable = "WEBSITE_HOSTNAME";

        private string roleName = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleNameContainer"/> class.
        /// Will set the RoleName based on an environment variable.
        /// </summary>
        /// <param name="hostNameSuffix">Host name suffix will be used to parse the prefix from the host name. The value of the prefix is the RoleName.</param>
        public RoleNameContainer(string hostNameSuffix = ".azurewebsites.net")
        {
            this.HostNameSuffix = hostNameSuffix;
            var enVarValue = Environment.GetEnvironmentVariable(WebAppHostNameEnvironmentVariable);
            this.ParseAndSetRoleName(enVarValue);

            this.IsAzureWebApp = !string.IsNullOrEmpty(enVarValue);
        }

        /// <summary>
        /// Gets or sets static instance for Initializer and DiagnosticListener to share access to RoleName variable.
        /// </summary>
        public static RoleNameContainer Instance { get; set; }

        /// <summary>
        /// Gets or sets role name of the current application.
        /// </summary>
        public string RoleName
        {
            get => this.roleName;

            set
            {
                if (value != this.roleName)
                {
                    Interlocked.Exchange(ref this.roleName, value);
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether indicates if the current app is an Azure Web App based on the presence of a specific environment variable. Set in constructor.
        /// </summary>
        public bool IsAzureWebApp { get; private set; }

        /// <summary>
        /// Gets suffix of website name. This must be changed when running in non public Azure region.
        /// Default value (Public Cloud):  ".azurewebsites.net"
        /// For US Gov Cloud: ".azurewebsites.us"
        /// For Azure Germany: ".azurewebsites.de".
        /// </summary>
        public string HostNameSuffix { get; private set; }

        /// <summary>
        /// Attempt to set the role name from a given collection of request headers.
        /// </summary>
        /// <param name="requestHeaders">Request headers to check for role name.</param>
        public void Set(IHeaderDictionary requestHeaders)
        {
            string headerValue = requestHeaders[WebAppHostNameHeaderName];
            this.ParseAndSetRoleName(headerValue);
        }

        private void ParseAndSetRoleName(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                // do nothing
            }
            else if (input.EndsWith(this.HostNameSuffix, StringComparison.OrdinalIgnoreCase))
            {
                this.RoleName = input.Substring(0, input.Length - this.HostNameSuffix.Length);
            }
            else
            {
                this.RoleName = input;
            }
        }
    }
}
