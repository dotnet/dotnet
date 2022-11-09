using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace NuGet.VisualStudio
{
    /// <summary>
    /// Execute powershell scripts from package(s) in a solution
    /// </summary>
    /// <remarks>Intended for internal use only.</remarks>
    [ComImport]
    [Guid("8c6750b0-214f-4052-93b2-284f12ebb896")]
    public interface IVsGlobalPackagesInitScriptExecutor
    {
        /// <summary>
        /// Executes the init script of the given package if available.
        /// 1) If the init.ps1 script has already been executed by the powershell host, it will not be executed again.
        /// True is returned.
        /// 2) If the package is found in the global packages folder it will be used.
        /// If not, it will return false and do nothing.
        /// 3) Also, note if other scripts are executing while this call was made, it will wait for them to complete.
        /// </summary>
        /// <param name="packageId">Id of the package whose init.ps1 will be executed.</param>
        /// <param name="packageVersion">Version of the package whose init.ps1 will be executed.</param>
        /// <returns>Returns true if the script was executed or has been executed already.</returns>
        /// <remarks>This method throws if the init.ps1 being executed throws.</remarks>
        Task<bool> ExecuteInitScriptAsync(string packageId, string packageVersion);
    }
}
