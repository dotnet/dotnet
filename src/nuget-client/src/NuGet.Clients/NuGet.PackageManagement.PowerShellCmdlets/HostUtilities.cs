// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Management.Automation;
using System.Reflection;
using System.Security;
using Microsoft.Win32;

namespace NuGetConsole.Host.PowerShell
{
    internal enum SuggestionMatchType
    {
        Command = 0,
        Error = 1,
        Dynamic = 2
    }

    /// <summary>
    /// Implements utility methods that might be used by Hosts.
    /// </summary>
    internal static class HostUtilities
    {
        #region GetProfileCommands

        /// <summary>
        /// Gets a PSObject whose base object is currentUserCurrentHost and with notes for the other 4 parameters.
        /// </summary>
        /// <param name="allUsersAllHosts">The profile file name for all users and all hosts.</param>
        /// <param name="allUsersCurrentHost">The profile file name for all users and current host.</param>
        /// <param name="currentUserAllHosts">The profile file name for current user and all hosts.</param>
        /// <param name="currentUserCurrentHost">The profile  name for current user and current host.</param>
        /// <returns>A PSObject whose base object is currentUserCurrentHost and with notes for the other 4 parameters.</returns>
        internal static PSObject GetDollarProfile(string allUsersAllHosts, string allUsersCurrentHost, string currentUserAllHosts, string currentUserCurrentHost)
        {
            PSObject returnValue = new PSObject(currentUserCurrentHost);
            returnValue.Properties.Add(new PSNoteProperty("AllUsersAllHosts", allUsersAllHosts));
            returnValue.Properties.Add(new PSNoteProperty("AllUsersCurrentHost", allUsersCurrentHost));
            returnValue.Properties.Add(new PSNoteProperty("CurrentUserAllHosts", currentUserAllHosts));
            returnValue.Properties.Add(new PSNoteProperty("CurrentUserCurrentHost", currentUserCurrentHost));
            return returnValue;
        }

        /// <summary>
        /// Gets an array of commands that can be run sequentially to set $profile and run the profile commands.
        /// </summary>
        /// <param name="shellId">The id identifying the host or shell used in profile file names.</param>
        /// <returns></returns>
        public static PSCommand[] GetProfileCommands(string shellId)
        {
            return GetProfileCommands(shellId, false);
        }

        /// <summary>
        /// Gets an array of commands that can be run sequentially to set $profile and run the profile commands.
        /// </summary>
        /// <param name="shellId">The id identifying the host or shell used in profile file names.</param>
        /// <param name="useTestProfile">used from test not to overwrite the profile file names from development boxes</param>
        /// <returns></returns>
        internal static PSCommand[] GetProfileCommands(string shellId, bool useTestProfile)
        {
            List<PSCommand> commands = new List<PSCommand>();
            string allUsersAllHosts = GetFullProfileFileName(null, false, useTestProfile);
            string allUsersCurrentHost = GetFullProfileFileName(shellId, false, useTestProfile);
            string currentUserAllHosts = GetFullProfileFileName(null, true, useTestProfile);
            string currentUserCurrentHost = GetFullProfileFileName(shellId, true, useTestProfile);
            PSObject dollarProfile = GetDollarProfile(allUsersAllHosts, allUsersCurrentHost, currentUserAllHosts, currentUserCurrentHost);
            PSCommand command = new PSCommand();
            command.AddCommand("set-variable");
            command.AddParameter("Name", "profile");
            command.AddParameter("Value", dollarProfile);
            command.AddParameter("Option", ScopedItemOptions.None);
            commands.Add(command);

            string[] profilePaths = { allUsersAllHosts, allUsersCurrentHost, currentUserAllHosts, currentUserCurrentHost };
            foreach (string profilePath in profilePaths)
            {
                if (!File.Exists(profilePath))
                {
                    continue;
                }
                command = new PSCommand();
                command.AddCommand(profilePath, false);
                commands.Add(command);
            }

            return commands.ToArray();
        }

        /// <summary>
        /// Used to get all profile file names for the current or all hosts and for the current or all users.
        /// </summary>
        /// <param name="shellId">null for all hosts, not null for the specified host</param>
        /// <param name="forCurrentUser">false for all users, true for the current user.</param>
        /// <param name="useTestProfile">used from test not to overwrite the profile file names from development boxes</param>
        /// <returns>The profile file name matching the parameters.</returns>
        internal static string GetFullProfileFileName(string shellId, bool forCurrentUser, bool useTestProfile)
        {
            string basePath;

            if (forCurrentUser)
            {
                basePath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                basePath = Path.Combine(basePath, "WindowsPowerShell");
            }
            else
            {
                basePath = GetAllUsersFolderPath();
                if (string.IsNullOrEmpty(basePath))
                {
                    return "";
                }
            }

            string profileName = useTestProfile ? "profile_test.ps1" : "profile.ps1";

            if (!string.IsNullOrEmpty(shellId))
            {
                profileName = shellId + "_" + profileName;
            }

            return Path.Combine(basePath, profileName);
        }

        /// <summary>
        /// Used internally in GetFullProfileFileName to get the base path for all users profiles.
        /// </summary>
        /// <returns>the base path for all users profiles.</returns>
        private static string GetAllUsersFolderPath()
        {
            string folderPath = string.Empty;
            try
            {
                folderPath = GetApplicationBase();
            }
            catch (SecurityException)
            {
            }

            return folderPath;
        }

        internal const string MonadRootKeyPath = "Software\\Microsoft\\PowerShell";
        internal const string MonadEngineKey = "PowerShellEngine";
        internal const string MonadEngine_ApplicationBase = "ApplicationBase";
        internal const string RegistryVersionKey = "1";

        internal static string GetApplicationBase()
        {
            string engineKeyPath = MonadRootKeyPath + "\\" + RegistryVersionKey + "\\" + MonadEngineKey;

            // Registration exists in both 32bit and 64bit localmachine keys, no need for explicit use of RegistryHive.
            using (RegistryKey engineKey = Registry.LocalMachine.OpenSubKey(engineKeyPath))
            {
                if (engineKey != null)
                {
                    return engineKey.GetValue(MonadEngine_ApplicationBase) as string;
                }
            }

            // The default keys aren't installed, so try and use the entry assembly to
            // get the application base. This works for managed apps like minishells...
            Assembly assem = Assembly.GetEntryAssembly();
            if (assem != null)
            {
                // For minishells, we just return the executable path.
                return Path.GetDirectoryName(assem.Location);
            }

            // FOr unmanaged host apps, look for the SMA dll, if it's not GAC'ed then
            // use it's location as the application base...
            assem = Assembly.GetAssembly(typeof(PSObject));
            if (assem != null)
            {
                // For other hosts.
                return Path.GetDirectoryName(assem.Location);
            }

            // otherwise, just give up...
            return "";
        }

        #endregion GetProfileCommands
    }
}
