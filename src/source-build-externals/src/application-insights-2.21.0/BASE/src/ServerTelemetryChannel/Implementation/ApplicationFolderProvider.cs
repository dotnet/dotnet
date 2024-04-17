namespace Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Security;
    using System.Security.AccessControl;
    using System.Security.Cryptography;
    using System.Security.Principal;
    using System.Text;
    using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Shared.Implementation;

    internal class ApplicationFolderProvider : IApplicationFolderProvider
    {
        internal Func<DirectoryInfo, bool> ApplySecurityToDirectory;

        private readonly IDictionary environment;
        private readonly string customFolderName;
        private readonly IIdentityProvider identityProvider;

        // Creating readonly instead of constant, from test we could use reflection to replace the value of these fields.
        private readonly string nonWindowsStorageProbePathVarTmp = "/var/tmp/";
        private readonly string nonWindowsStorageProbePathTmp = "/tmp/";

        public ApplicationFolderProvider(string folderName = null)
            : this(Environment.GetEnvironmentVariables(), folderName)
        {
        }

        internal ApplicationFolderProvider(IDictionary environment, string folderName = null)
        {
            if (environment == null)
            {
                throw new ArgumentNullException(nameof(environment));
            }

            if (IsWindowsOperatingSystem())
            {
                this.identityProvider = new WindowsIdentityProvider();
                this.ApplySecurityToDirectory = this.SetSecurityPermissionsToAdminAndCurrentUserWindows;
            }
            else
            {
                this.identityProvider = new NonWindowsIdentityProvider();
                this.ApplySecurityToDirectory = this.SetSecurityPermissionsToAdminAndCurrentUserNonWindows;
            }

            this.environment = environment;
            this.customFolderName = folderName;
        }

        public IPlatformFolder GetApplicationFolder()
        {
            var errors = new List<string>(this.environment.Count + 1);

            var result = this.CreateAndValidateApplicationFolder(this.customFolderName, createSubFolder: false, errors: errors);

            // User configured custom folder and SDK is unable to use it.
            // Log the error message and return without attempting any other folders.
            if (!string.IsNullOrEmpty(this.customFolderName) && result == null)
            {
                TelemetryChannelEventSource.Log.TransmissionCustomStorageError(string.Join(Environment.NewLine, errors), this.identityProvider.GetName(), this.customFolderName);
                return result;
            }

            if (IsWindowsOperatingSystem())
            {
                if (result == null)
                {
                    object localAppData = this.environment["LOCALAPPDATA"];
                    if (localAppData != null)
                    {
                        result = this.CreateAndValidateApplicationFolder(localAppData.ToString(), createSubFolder: true, errors: errors);
                    }
                }

                if (result == null)
                {
                    object temp = this.environment["TEMP"];
                    if (temp != null)
                    {
                        result = this.CreateAndValidateApplicationFolder(temp.ToString(), createSubFolder: true, errors: errors);
                    }
                }
            }
            else
            {
                if (result == null)
                {
                    object tmpdir = this.environment["TMPDIR"];
                    if (tmpdir != null)
                    {
                        result = this.CreateAndValidateApplicationFolder(tmpdir.ToString(), createSubFolder: true, errors: errors);
                    }
                }

                if (result == null)
                {
                    result = this.CreateAndValidateApplicationFolder(this.nonWindowsStorageProbePathVarTmp, createSubFolder: true, errors: errors);
                }

                if (result == null)
                {
                    result = this.CreateAndValidateApplicationFolder(this.nonWindowsStorageProbePathTmp, createSubFolder: true, errors: errors);
                }
            }

            if (result == null)
            {
                TelemetryChannelEventSource.Log.TransmissionStorageAccessDeniedError(string.Join(Environment.NewLine, errors), this.identityProvider.GetName(), this.customFolderName);
            }

            return result;
        }

        internal static bool IsWindowsOperatingSystem()
        {
#if NET452
            return true;
#else
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return true;
            }
            else
            {
                return false;
            }
#endif
        }

        /// <summary>
        /// Test hook to allow testing of non-windows scenario.
        /// </summary>
        /// <param name="applySecurityToDirectory">The method to be invoked to set directory access.</param>
        internal void OverrideApplySecurityToDirectory(Func<DirectoryInfo, bool> applySecurityToDirectory)
        {
            this.ApplySecurityToDirectory = applySecurityToDirectory;
        }

        private static string GetPathAccessFailureErrorMessage(Exception exp, string path)
        {
            return "Path: " + path + "; Error: " + exp.Message + Environment.NewLine;
        }

        /// <summary>
        /// Throws <see cref="UnauthorizedAccessException" /> if the process lacks the required permissions to access the <paramref name="telemetryDirectory"/>.
        /// </summary>
        private static void CheckAccessPermissions(DirectoryInfo telemetryDirectory)
        {
            string testFileName = Path.GetRandomFileName();
            string testFilePath = Path.Combine(telemetryDirectory.FullName, testFileName);

            // FileSystemRights.CreateFiles
            using (var testFile = new FileStream(testFilePath, FileMode.CreateNew, FileAccess.ReadWrite))
            {
                // FileSystemRights.Write
                testFile.Write(new[] { default(byte) }, 0, 1);
            }

            // FileSystemRights.ListDirectory and FileSystemRights.Read
            telemetryDirectory.GetFiles(testFileName);

            // FileSystemRights.DeleteSubdirectoriesAndFiles
            File.Delete(testFilePath);
        }

        private static string GetSHA256Hash(string input)
        {
            var hashString = new StringBuilder();

            byte[] inputBits = Encoding.Unicode.GetBytes(input);
            using (var sha256 = CreateSHA256())
            {
                byte[] hashBits = sha256.ComputeHash(inputBits);
                foreach (byte b in hashBits)
                {
                    hashString.Append(b.ToString("x2", CultureInfo.InvariantCulture));
                }
            }

            return hashString.ToString();
        }

        private static SHA256 CreateSHA256()
        {
#if NETSTANDARD
            return SHA256.Create();
#else
            return new SHA256CryptoServiceProvider();
#endif
        }

        private IPlatformFolder CreateAndValidateApplicationFolder(string rootPath, bool createSubFolder, IList<string> errors)
        {
            string errorMessage = null;
            IPlatformFolder result = null;

            try
            {
                if (!string.IsNullOrEmpty(rootPath))
                {
                    var telemetryDirectory = new DirectoryInfo(rootPath);
                    if (createSubFolder)
                    {
                        telemetryDirectory = this.CreateTelemetrySubdirectory(telemetryDirectory);
                        if (!this.ApplySecurityToDirectory(telemetryDirectory))
                        {
                            throw new SecurityException("Unable to apply security restrictions to the storage directory.");
                        }
                    }

                    CheckAccessPermissions(telemetryDirectory);
                    TelemetryChannelEventSource.Log.StorageFolder(telemetryDirectory.FullName);

                    result = new PlatformFolder(telemetryDirectory);
                }
            }
            catch (UnauthorizedAccessException exp)
            {
                errorMessage = GetPathAccessFailureErrorMessage(exp, rootPath);
                TelemetryChannelEventSource.Log.TransmissionStorageIssuesWarning(errorMessage, this.identityProvider.GetName());
            }
            catch (ArgumentException exp)
            {
                // Path does not specify a valid file path or contains invalid DirectoryInfo characters.
                errorMessage = GetPathAccessFailureErrorMessage(exp, rootPath);
                TelemetryChannelEventSource.Log.TransmissionStorageIssuesWarning(errorMessage, this.identityProvider.GetName());
            }
            catch (DirectoryNotFoundException exp)
            {
                // The specified path is invalid, such as being on an unmapped drive.
                errorMessage = GetPathAccessFailureErrorMessage(exp, rootPath);
                TelemetryChannelEventSource.Log.TransmissionStorageIssuesWarning(errorMessage, this.identityProvider.GetName());
            }
            catch (IOException exp)
            {
                // The subdirectory cannot be created. -or- A file or directory already has the name specified by path. -or-  The specified path, file name, or both exceed the system-defined maximum length. .
                errorMessage = GetPathAccessFailureErrorMessage(exp, rootPath);
                TelemetryChannelEventSource.Log.TransmissionStorageIssuesWarning(errorMessage, this.identityProvider.GetName());
            }
            catch (SecurityException exp)
            {
                // The caller does not have code access permission to create the directory.
                errorMessage = GetPathAccessFailureErrorMessage(exp, rootPath);
                TelemetryChannelEventSource.Log.TransmissionStorageIssuesWarning(errorMessage, this.identityProvider.GetName());
            }

            if (!string.IsNullOrEmpty(errorMessage))
            {
                errors.Add(errorMessage);
            }

            return result;
        }

        private DirectoryInfo CreateTelemetrySubdirectory(DirectoryInfo root)
        {
            string baseDirectory = string.Empty;

#if NETFRAMEWORK
            baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
#else
            baseDirectory = AppContext.BaseDirectory;
#endif

            string appIdentity = this.identityProvider.GetName() + "@" + Path.Combine(baseDirectory, Process.GetCurrentProcess().ProcessName);
            string subdirectoryName = GetSHA256Hash(appIdentity);
            string subdirectoryPath = Path.Combine("Microsoft", "ApplicationInsights", subdirectoryName);
            DirectoryInfo subdirectory = root.CreateSubdirectory(subdirectoryPath);

            return subdirectory;
        }

        private bool SetSecurityPermissionsToAdminAndCurrentUserNonWindows(DirectoryInfo subdirectory)
        {
            // For non-windows simply return true to skip security policy.
            // This is until .net core exposes an Api to do this.
            return true;
        }

        private bool SetSecurityPermissionsToAdminAndCurrentUserWindows(DirectoryInfo subdirectory)
        {
            try
            {
                var directorySecurity = subdirectory.GetAccessControl();

                // Grant access only to admins and current user
                var adminitrators = new SecurityIdentifier(WellKnownSidType.BuiltinAdministratorsSid, null);
                directorySecurity.AddAccessRule(
                    new FileSystemAccessRule(
                            adminitrators,
                            FileSystemRights.FullControl,
                            InheritanceFlags.None,
                            PropagationFlags.NoPropagateInherit,
                            AccessControlType.Allow));

                directorySecurity.AddAccessRule(
                    new FileSystemAccessRule(
                            this.identityProvider.GetName(),
                            FileSystemRights.FullControl,
                            InheritanceFlags.None,
                            PropagationFlags.NoPropagateInherit,
                            AccessControlType.Allow));

                // Do not inherit from parent folder
                directorySecurity.SetAccessRuleProtection(isProtected: true, preserveInheritance: false);

                subdirectory.SetAccessControl(directorySecurity);

                return true;
            }
            catch (Exception ex)
            {
                TelemetryChannelEventSource.Log.FailedToSetSecurityPermissionStorageDirectory(subdirectory.FullName, ex.Message);
                return false;
            }
        }
    }
}
