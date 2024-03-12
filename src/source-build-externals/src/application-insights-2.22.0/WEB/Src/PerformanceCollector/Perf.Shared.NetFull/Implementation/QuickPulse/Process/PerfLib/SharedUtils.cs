namespace Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.QuickPulse.PerfLib
{
    using System.ComponentModel;
    using System.Security.Permissions;

    internal static class SharedUtils
    {
        public static Win32Exception CreateSafeWin32Exception(int error)
        {
            Win32Exception newException = null;

            // Need to assert SecurtiyPermission, otherwise Win32Exception
            // will not be able to get the error message. At this point the right
            // permissions have already been demanded.
            SecurityPermission securityPermission = new SecurityPermission(PermissionState.Unrestricted);
            securityPermission.Assert();
            try
            {
                newException = error == 0 ? new Win32Exception() : new Win32Exception(error);
            }
            finally
            {
                SecurityPermission.RevertAssert();
            }

            return newException;
        }
    }
}
