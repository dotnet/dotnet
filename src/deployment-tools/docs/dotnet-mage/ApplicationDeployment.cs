using System;

namespace ClickOnceHelper
{
    public class ApplicationDeployment
    {
        private static ApplicationDeployment currentDeployment = null;
        private static bool currentDeploymentInitialized = false;

        private static bool isNetworkDeployed = false;
        private static bool isNetworkDeployedInitialized = false;

        public static bool IsNetworkDeployed
        {
            get
            {
                if (!isNetworkDeployedInitialized)
                {
                    bool.TryParse(Environment.GetEnvironmentVariable("ClickOnce_IsNetworkDeployed"), out ApplicationDeployment.isNetworkDeployed);
                    ApplicationDeployment.isNetworkDeployedInitialized = true;
                }

                return ApplicationDeployment.isNetworkDeployed;
            }
        }

        public static ApplicationDeployment CurrentDeployment
        {
            get
            {
                if (!currentDeploymentInitialized)
                {
                    ApplicationDeployment.currentDeployment = IsNetworkDeployed ? new ApplicationDeployment() : null;
                    ApplicationDeployment.currentDeploymentInitialized = true;
                }

                return ApplicationDeployment.currentDeployment;
            }
        }

        public Uri ActivationUri
        {
            get
            {
                Uri.TryCreate(Environment.GetEnvironmentVariable("ClickOnce_ActivationUri"), UriKind.Absolute, out Uri val);
                return val;
            }
        }

        public Version CurrentVersion
        {
            get
            {
                Version.TryParse(Environment.GetEnvironmentVariable("ClickOnce_CurrentVersion"), out Version val);
                return val;
            }
        }
        public string DataDirectory
        {
            get { return Environment.GetEnvironmentVariable("ClickOnce_DataDirectory");  }
        }

        public bool IsFirstRun
        {
            get
            {
                bool.TryParse(Environment.GetEnvironmentVariable("ClickOnce_IsFirstRun"), out bool val);
                return val;
            }
        }

        public DateTime TimeOfLastUpdateCheck
        {
            get
            {
                DateTime.TryParse(Environment.GetEnvironmentVariable("ClickOnce_TimeOfLastUpdateCheck"), out DateTime value);
                return value;
            }
        }
        public string UpdatedApplicationFullName
        {
            get
            {
                return Environment.GetEnvironmentVariable("ClickOnce_UpdatedApplicationFullName");
            }
        }

        public Version UpdatedVersion
        {
            get
            {
                Version.TryParse(Environment.GetEnvironmentVariable("ClickOnce_UpdatedVersion"), out Version val);
                return val;
            }
        }

        public Uri UpdateLocation
        {
            get
            {
                Uri.TryCreate(Environment.GetEnvironmentVariable("ClickOnce_UpdateLocation"), UriKind.Absolute, out Uri val);
                return val;
            }
        }

        public Version LauncherVersion
        {
            get
            {

                Version.TryParse(Environment.GetEnvironmentVariable("ClickOnce_LauncherVersion"), out Version val);
                return val;
            }
        }

        private ApplicationDeployment()
        {
            // As an alternative solution, we could initialize all properties here
        }
    }
}
