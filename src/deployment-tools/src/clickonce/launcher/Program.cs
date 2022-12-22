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
