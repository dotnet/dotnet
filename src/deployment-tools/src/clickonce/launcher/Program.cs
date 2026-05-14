// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Win32;
using System;
using System.Deployment.Application;
using System.IO;

namespace Microsoft.Deployment.Launcher
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            StartLoggingIfConfigured();

            try
            {
                // Allow running as a ClickOnce app only.
                if (false == ApplicationDeployment.IsNetworkDeployed)
                {
                    throw new LauncherException(Constants.ErrorNotAClickOnceDeployment);
                }

                Launch(GetApplicationPath());

            }
            catch (Exception e)
            {
                Logger.LogError(e.Message);
            }
            finally
            {
                Logger.StopLogging();
            }
        }

        /// <summary>
        /// Launches .NET (Core) application.
        /// </summary>
        /// <param name="appToLaunchFullPath">Full path to application binary</param>
        private static void Launch(string appToLaunchFullPath)
        {
            string exe;
            string args;

            string extension = Path.GetExtension(appToLaunchFullPath).ToLower();
            if (extension == ".dll")
            {
                exe = HostFinder.GetHost(appToLaunchFullPath);
                args = appToLaunchFullPath.DoubleQuoted();
            }
            else if (extension == ".exe")
            {
                exe = appToLaunchFullPath;
                args = string.Empty;
            }
            else
            {
                // Desktop .NET (Core) applications on Windows can only be EXEs or DLLs.
                throw new LauncherException(Constants.ErrorUnsupportedExtension, appToLaunchFullPath);
            }

            // Add ClickOnce properties from ApplicationDeployment object
            // as environment variables, to be passed to child process.
            AddClickOnceEnvironmentVariables();

            ProcessHelper ph = new ProcessHelper(exe, args);
            ph.StartProcessWithRetries();
        }

        /// <summary>
        /// Gets the full path to the application from Launcher resources.
        /// Validates application filename value.
        /// </summary>
        /// <returns>Full path of app to launch.</returns>
        private static string GetApplicationPath()
        {
            string launcherPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            Logger.LogInfo(Constants.InfoLauncherPath, launcherPath);

            string applicationFilename = ResourceReader.GetApplicationFilename(launcherPath);
            if (string.IsNullOrEmpty(applicationFilename))
            {
                throw new LauncherException(Constants.ErrorApplicationFilenameCannotBeEmpty);
            }

            // Application filename cannot include paths, it has to be in the same folder as Launcher.
            // We prohibit launching anything outside of app's ClickOnce folder.
            if (string.Compare(applicationFilename, Path.GetFileName(applicationFilename), StringComparison.InvariantCultureIgnoreCase) != 0)
            {
                throw new LauncherException(Constants.ErrorApplicationFilenameCannotIncludeAPath);
            }

            return Path.Combine(Path.GetDirectoryName(launcherPath), applicationFilename);
        }

        /// <summary>
        /// Starts logging if configured in registry.
        /// User sets the log file path, which enables logging.
        /// This model follows the existing ClickOnce runtime logging.
        /// </summary>
        private static void StartLoggingIfConfigured()
        {
            string path = null;

            try
            {
                using (RegistryKey deploymentKey = Registry.CurrentUser.OpenSubKey(Constants.ClickOnceDeploymentSubkeyName))
                {
                    /// Deployment key may not be present.
                    if (deploymentKey != null)
                    {
                        path = deploymentKey.GetValue(Constants.LauncherLogFilePathRegistryString) as string;
                    }
                }
            }
            catch (Exception)
            {
                // Logging is optional, do not fail the Launcher.
            }

            if (!string.IsNullOrEmpty(path))
            {
                Logger.StartLogging(path);
            }
        }

        /// <summary>
        /// Add ClickOnce properties from ApplicationDeployment object
        /// as environment variables, to be passed to child process.
        /// 
        /// Add some extra variables, like Launcher version.
        /// </summary>
        private static void AddClickOnceEnvironmentVariables()
        {
            if (ApplicationDeployment.IsNetworkDeployed)
            {
                // ApplicationDeployment properties
                ApplicationDeployment ad = ApplicationDeployment.CurrentDeployment;
                Environment.SetEnvironmentVariable("ClickOnce_ActivationUri", ad.ActivationUri?.ToString());
                Environment.SetEnvironmentVariable("ClickOnce_CurrentVersion", ad.CurrentVersion?.ToString());
                Environment.SetEnvironmentVariable("ClickOnce_DataDirectory", ad.DataDirectory?.ToString());
                Environment.SetEnvironmentVariable("ClickOnce_IsFirstRun", ad.IsFirstRun.ToString());
                Environment.SetEnvironmentVariable("ClickOnce_TimeOfLastUpdateCheck", ad.TimeOfLastUpdateCheck.ToString());
                Environment.SetEnvironmentVariable("ClickOnce_UpdatedApplicationFullName", ad.UpdatedApplicationFullName?.ToString());
                Environment.SetEnvironmentVariable("ClickOnce_UpdatedVersion", ad.UpdatedVersion?.ToString());
                Environment.SetEnvironmentVariable("ClickOnce_UpdateLocation", ad.UpdateLocation?.ToString());

                // ClickOnce ActivationData
                string[] activationData = AppDomain.CurrentDomain?.SetupInformation?.ActivationArguments?.ActivationData;
                if (activationData != null && activationData.Length > 0)
                {
                    Environment.SetEnvironmentVariable("ClickOnce_ActivationData_Count", activationData.Length.ToString());
                    for (int i = 0; i < activationData.Length; i++)
                    {
                        Environment.SetEnvironmentVariable($"ClickOnce_ActivationData_{i}", activationData[i]);
                    }
                }
            }

            Environment.SetEnvironmentVariable("ClickOnce_IsNetworkDeployed", ApplicationDeployment.IsNetworkDeployed.ToString());
            Environment.SetEnvironmentVariable("ClickOnce_LauncherVersion", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version?.ToString());
        }

        /// <summary>
        /// Gets the text enclosed in double quotes.
        /// </summary>
        /// <param name="text">Text</param>
        /// <returns></returns>
        private static string DoubleQuoted(this string text)
        {
            return "\"" + text + "\"";
        }
    }
}
