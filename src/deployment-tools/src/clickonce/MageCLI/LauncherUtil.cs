// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using Microsoft.Deployment.MageCLI;

namespace Microsoft.Deployment.Utilities
{
    public sealed class LauncherUtil
    {
        /// <summary>
        /// Launcher binary filename.
        /// </summary>
        public static string LauncherFilename = @"Launcher.exe";

        /// <summary>
        /// Launcher binary template filename.
        /// </summary>
        public static string LauncherTemplate = @"Launcher.exe";

        /// <summary>
        /// Adds Launcher binary to specified directory and updates resource with target binary name
        /// </summary>
        /// <param name="dir">Directory in which to add Launcher</param>
        /// <param name="binaryToLaunch">Name of binary to be launched by Launcher</param>
        /// <returns>Success or failure</returns>
        public static bool AddLauncher(string dir, string binaryToLaunch)
        {
            try
            {
                if (string.Compare(binaryToLaunch, Path.GetFileName(binaryToLaunch), true) != 0)
                {
                    Application.PrintErrorMessage(ErrorMessages.InvalidBinaryToLaunch, binaryToLaunch);
                    return false;
                }

                if (!Directory.Exists(dir))
                {
                    Application.PrintErrorMessage(ErrorMessages.InvalidDirectory, dir);
                    return false;
                }

                string launcherTemplatePath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), LauncherTemplate);
                if (!File.Exists(launcherTemplatePath))
                {
                    Application.PrintErrorMessage(ErrorMessages.MissingLauncherTemplate, launcherTemplatePath);
                    return false;
                }

                // Copy template binary
                string targetFilePath = Path.Combine(dir, LauncherFilename);
                File.Copy(launcherTemplatePath, targetFilePath, true);

                if (false == ResourceUpdater.SetApplicationFilename(targetFilePath, binaryToLaunch))
                {
                    Application.PrintErrorMessage(ErrorMessages.FailedToUpdateLauncherResources, "");
                    return false;
                }
            }
            catch (Exception)
            {
                Application.PrintErrorMessage(ErrorMessages.FailedToAddLauncher, "");
                return false;
            }

            return true;
        }
    }
}

