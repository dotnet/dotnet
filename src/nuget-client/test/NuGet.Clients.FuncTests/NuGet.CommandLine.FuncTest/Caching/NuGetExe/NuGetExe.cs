using System.Collections.Generic;
using System.IO;
using Microsoft.Internal.NuGet.Testing.SignedPackages.ChildProcess;

namespace NuGet.CommandLine.Test.Caching
{
    public class NuGetExe : INuGetExe
    {
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

        public static NuGetExe GetBuiltNuGetExe()
        {
            return new NuGetExe(Util.GetNuGetExePath(), supportsIsolatedHttpCache: true);
        }
    }
}
