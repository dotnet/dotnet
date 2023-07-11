using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Test.Utility;

namespace NuGet.CommandLine.Test.Caching
{
    public class NuGetExe : INuGetExe
    {
        private static ConcurrentDictionary<string, Task<string>> _verifiedPaths
            = new ConcurrentDictionary<string, Task<string>>();

        private readonly string _pathToExe;
        private readonly bool _supportsIsolatedHttpCache;
        private bool _hasExecuted;

        private NuGetExe(string pathToExe, bool supportsIsolatedHttpCache)
        {
            _pathToExe = pathToExe;
            _supportsIsolatedHttpCache = supportsIsolatedHttpCache;
            _hasExecuted = false;
        }

        public string GetHttpCachePath(CachingTestContext context)
        {
            if (_supportsIsolatedHttpCache)
            {
                return context.IsolatedHttpCachePath;
            }
            else
            {
                var result = Execute(context, "locals http-cache -list", debug: false);

                var stdout = result.Output.Trim();

                // Example:
                //   stdout = http-cache: C:\Users\jver\AppData\Local\NuGet\v3-cache
                //   path   = C:\Users\jver\AppData\Local\NuGet\v3-cache
                var path = stdout.Split(new[] { ':' }, 2)[1].Trim();

                return path;
            }
        }

        public void ClearHttpCache(CachingTestContext context)
        {
            if (_supportsIsolatedHttpCache)
            {
                if (_hasExecuted)
                {
                    Directory.Delete(context.IsolatedHttpCachePath, recursive: true);
                }
                else
                {
                    // Do nothing, the HTTP cache is still clean.
                }
            }
            else
            {
                Execute(context, "locals http-cache -Clear", debug: false);
            }
        }

        public CommandRunnerResult Execute(CachingTestContext context, string args)
        {
            return Execute(context, args, context.Debug);
        }

        private CommandRunnerResult Execute(CachingTestContext context, string args, bool debug)
        {
            _hasExecuted = true;

            var timeout = 60 * 1000 * 1;
            if (debug)
            {
                args += " -Verbosity detailed --debug";
                timeout *= 60;
            }

            var environmentVariables = new Dictionary<string, string>
            {
                { "NUGET_PACKAGES", context.GlobalPackagesPath }
            };

            if (_supportsIsolatedHttpCache)
            {
                environmentVariables["NUGET_HTTP_CACHE_PATH"] = context.IsolatedHttpCachePath;
            }

            return CommandRunner.Run(
                _pathToExe,
                context.WorkingPath,
                args,
                timeOutInMilliseconds: timeout,
                environmentVariables: environmentVariables);
        }

        public static async Task<NuGetExe> Get320Async()
        {
            return await DownloadNuGetExeAsync(
                "https://dist.nuget.org/win-x86-commandline/v3.2.0/nuget.exe",
                "nuget.3.2.0.exe",
                supportsIsolatedHttpCache: false);
        }

        public static async Task<NuGetExe> Get330Async()
        {
            return await DownloadNuGetExeAsync(
                "https://dist.nuget.org/win-x86-commandline/v3.3.0/nuget.exe",
                "nuget.3.3.0.exe",
                supportsIsolatedHttpCache: false);
        }

        public static async Task<NuGetExe> Get340RcAsync()
        {
            return await DownloadNuGetExeAsync(
                "https://dist.nuget.org/win-x86-commandline/v3.4.0-rc/nuget.exe",
                "nuget.3.4.3-rc.exe",
                supportsIsolatedHttpCache: false);
        }

        public static async Task<NuGetExe> Get343Async()
        {
            return await DownloadNuGetExeAsync(
                "https://dist.nuget.org/win-x86-commandline/v3.4.3/nuget.exe",
                "nuget.3.4.3.exe",
                supportsIsolatedHttpCache: false);
        }

        public static async Task<NuGetExe> Get344Async()
        {
            return await DownloadNuGetExeAsync(
                "https://dist.nuget.org/win-x86-commandline/v3.4.4/NuGet.exe",
                "nuget.3.4.4.exe",
                supportsIsolatedHttpCache: false);
        }

        public static async Task<NuGetExe> Get350Beta2Async()
        {
            return await DownloadNuGetExeAsync(
                "https://dist.nuget.org/win-x86-commandline/v3.5.0-beta2/NuGet.exe",
                "nuget.3.5.0-beta2.exe",
                supportsIsolatedHttpCache: false);
        }

        public static async Task<NuGetExe> Get350Rc1Async()
        {
            return await DownloadNuGetExeAsync(
                "https://dist.nuget.org/win-x86-commandline/v3.5.0-rc1/NuGet.exe",
                "nuget.3.5.0-rc1.exe",
                supportsIsolatedHttpCache: true);
        }

        public static NuGetExe GetBuiltNuGetExe()
        {
            return new NuGetExe(Util.GetNuGetExePath(), supportsIsolatedHttpCache: true);
        }

        private static async Task<NuGetExe> DownloadNuGetExeAsync(
            string requestUri,
            string fileName,
            bool supportsIsolatedHttpCache)
        {
            var temp = NuGetEnvironment.GetFolderPath(NuGetFolderPath.Temp);
            var path = Path.Combine(temp, fileName);

            var verifiedPath = await _verifiedPaths.GetOrAdd(
                path,
                thisPath => ConcurrencyUtilities.ExecuteWithFileLockedAsync(
                    thisPath,
                    async token =>
                    {
                        if (File.Exists(thisPath))
                        {
                            // Make sure we can run the executable.
                            var helpResult = CommandRunner.Run(
                                thisPath,
                                ".",
                                "help");

                            if (helpResult.ExitCode == 0)
                            {
                                return thisPath;
                            }
                        }

                        // Download the executable.
                        using (var httpClient = new System.Net.Http.HttpClient())
                        using (var stream = await httpClient.GetStreamAsync(requestUri))
                        using (var fileStream = new FileStream(thisPath, FileMode.Create))
                        {
                            await stream.CopyToAsync(fileStream);
                        }

                        return thisPath;
                    },
                    CancellationToken.None));

            return new NuGetExe(verifiedPath, supportsIsolatedHttpCache);
        }
    }
}
