using System;
using System.Collections.Generic;
using System.Linq;
using NuGet.CommandLine;

namespace NuGet.Common
{
    public static class PackageSourceProviderExtensions
    {
        public static Configuration.PackageSource ResolveSource(IEnumerable<Configuration.PackageSource> availableSources, string source)
        {
            var resolvedSource = availableSources.FirstOrDefault(
                    f => f.Source.Equals(source, StringComparison.OrdinalIgnoreCase) ||
                        f.Name.Equals(source, StringComparison.OrdinalIgnoreCase));

            if (resolvedSource == null)
            {
                CommandLineUtility.ValidateSource(source);
                return new Configuration.PackageSource(source);
            }
            else
            {
                return resolvedSource;
            }
        }

        public static string ResolveAndValidateSource(this Configuration.IPackageSourceProvider sourceProvider, string source)
        {
            if (String.IsNullOrEmpty(source))
            {
                return null;
            }

            var sources = sourceProvider.LoadPackageSources().Where(s => s.IsEnabled);
            var result = ResolveSource(sources, source);
            CommandLineUtility.ValidateSource(result.Source);
            return result.Source;
        }
    }
}
