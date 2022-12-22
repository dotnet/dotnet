// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Microsoft.Deployment.Launcher
{
    public static class Constants
    {
        // Logging - registry keys/values
        public static readonly string ClickOnceDeploymentSubkeyName = @"SOFTWARE\Classes\Software\Microsoft\Windows\CurrentVersion\Deployment";
        public static readonly string LauncherLogFilePathRegistryString = "LauncherLogFilePath";

        // Error messages
        public static readonly string ErrorApplicationFilenameCannotBeEmpty = "Filename cannot be empty.";
        public static readonly string ErrorApplicationFilenameCannotIncludeAPath = "Filename cannot include a path.";
        public static readonly string ErrorApplicationAssemblyIdentity = "Cannot obtain assembly identity from application binary: {0}";
        public static readonly string ErrorInvalidStartProcessWithRetryParameters = "Invalid StartProcessWithRetry() parameters.";
        public static readonly string ErrorNotAClickOnceDeployment = "Launcher cannot be run outside of a ClickOnce deployment.";
        public static readonly string ErrorProcessFailedToLaunch = "Failed to launch \"{0}\" {1}";
        public static readonly string ErrorProcessStart = "Failed to start the process with error: {0}";
        public static readonly string ErrorProcessNotStarted = "Process did not start.";
        public static readonly string ErrorResourceMissing = "Resource does not exist: {0}";
        public static readonly string ErrorResourceFailedToLoad = "LoadResource() failed to read the resource: {0}";
        public static readonly string ErrorResourceFailedToLock = "LockResource() failed to lock the resource for reading: {0}";
        public static readonly string ErrorUnsupportedExtension = "Filename has unsupported extension: {0}";

        // Info/status messages
        public static readonly string InfoLauncherPath = "Launcher path: {0}";
        public static readonly string InfoProcessStartWaitRetry = "Waiting {0} miliseconds before retrying...";
        public static readonly string InfoProcessToLaunch = "Launching: \"{0}\" {1}";

        // Various constants

        // We do not currently retry - this matches the ClickOnce model of launching apps.
        // If this decision changes, the number of attempts and delay should be modified accordingly.
        public static readonly int NumberOfProcessStartAttempts = 1; 
        public static readonly int DelayBeforeRetryMiliseconds = 1000;
    }
}
