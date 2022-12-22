// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Management.Automation;
using Microsoft.PowerShell.Commands;

namespace NuGet.PackageManagement.PowerShellCmdlets
{
    internal static class PSPathUtility
    {
        /// <summary>
        /// Translate a PSPath into a System.IO.* friendly Win32 path.
        /// Does not resolve/glob wildcards.
        /// </summary>
        /// <param name="session">The SessionState to use.</param>
        /// <param name="psPath">
        /// The PowerShell PSPath to translate which may reference PSDrives or have
        /// provider-qualified paths which are syntactically invalid for .NET APIs.
        /// </param>
        /// <param name="path">The translated PSPath in a format understandable to .NET APIs.</param>
        /// <param name="exists">Returns null if not tested, or a bool representing path existence.</param>
        /// <param name="errorMessage">If translation failed, contains the reason.</param>
        /// <returns>True if successfully translated, false if not.</returns>
        [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "2#", Justification = "Following BCL TryParse pattern.")]
        [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "3#", Justification = "Following BCL TryParse pattern.")]
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "ps", Justification = "ps is a common powershell prefix")]
        public static bool TryTranslatePSPath(SessionState session, string psPath, out string path, out bool? exists, out string errorMessage)
        {
            if (String.IsNullOrEmpty(psPath))
            {
                throw new ArgumentException(
                    String.Format(
                        CultureInfo.CurrentCulture,
                        Resources.Argument_Cannot_Be_Null_Or_Empty, "psPath"));
            }

            bool succeeded = false;

            path = null;
            exists = null;
            errorMessage = null;

            // session is null during unit tests
            if (session == null)
            {
                return false;
            }

            if (!session.Path.IsValid(psPath))
            {
                errorMessage = String.Format(
                    CultureInfo.CurrentCulture,
                    Resources.Cmdlet_InvalidPathSyntax, psPath);
            }
            else
            {
                try
                {
                    // we do not glob wildcards (literalpath.)
                    exists = session.InvokeProvider.Item.Exists(psPath, force: false, literalPath: true);

                    ProviderInfo provider;
                    PSDriveInfo drive;

                    // translate pspath, not trying to glob.
                    path = session.Path.GetUnresolvedProviderPathFromPSPath(psPath, out provider, out drive);

                    // ensure path is on the filesystem (not registry, certificate, variable etc.)
                    if (provider.ImplementingType != typeof(FileSystemProvider))
                    {
                        errorMessage = Resources.Cmdlet_InvalidProvider;
                    }
                    else
                    {
                        succeeded = true;
                    }
                }
                catch (ProviderNotFoundException)
                {
                    errorMessage = Resources.Cmdlet_InvalidProvider;
                }
                catch (DriveNotFoundException ex)
                {
                    errorMessage = String.Format(
                        CultureInfo.CurrentCulture,
                        Resources.Cmdlet_InvalidPSDrive, ex.ItemName);
                }
            }
            return succeeded;
        }
    }
}
