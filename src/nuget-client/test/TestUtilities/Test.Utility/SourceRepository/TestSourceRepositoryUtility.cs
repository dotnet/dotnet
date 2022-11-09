// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NuGet.Configuration;
using NuGet.Protocol.Core.Types;
using NuGet.Protocol.VisualStudio;
using NuGet.Test.Utility;

namespace Test.Utility
{
    public class TestSourceRepositoryUtility
    {
        public static PackageSource V2PackageSource = new PackageSource("https://www.nuget.org/api/v2/", "v2");
        public static PackageSource V3PackageSource = new PackageSource("https://api.nuget.org/v3/index.json", "v3");

        public IEnumerable<Lazy<INuGetResourceProvider>> ResourceProviders { get; private set; }

        private void Initialize()
        {
            ResourceProviders = Repository.Provider.GetVisualStudio();
            //var aggregateCatalog = new AggregateCatalog();
            //{
            //    aggregateCatalog.Catalogs.Add(new DirectoryCatalog(Environment.CurrentDirectory, "*.dll"));
            //    var container = new CompositionContainer(aggregateCatalog);
            //    container.ComposeParts(this);
            //    return container;
            //}
        }

        public static SourceRepositoryProvider CreateV3OnlySourceRepositoryProvider()
        {
            return CreateSourceRepositoryProvider(new List<PackageSource> { V3PackageSource });
        }

        public static SourceRepositoryProvider CreateV2OnlySourceRepositoryProvider()
        {
            return CreateSourceRepositoryProvider(new List<PackageSource> { V2PackageSource });
        }

        public static SourceRepositoryProvider CreateSourceRepositoryProvider(PackageSource packageSource)
        {
            if (packageSource == null)
            {
                throw new ArgumentNullException(nameof(packageSource));
            }
            return CreateSourceRepositoryProvider(new[] { packageSource });
        }

        public static SourceRepositoryProvider CreateSourceRepositoryProvider(IEnumerable<PackageSource> packageSources)
        {
            var thisUtility = new TestSourceRepositoryUtility();
            thisUtility.Initialize();
            var packageSourceProvider = new TestPackageSourceProvider(packageSources);

            var sourceRepositoryProvider = new SourceRepositoryProvider(packageSourceProvider, thisUtility.ResourceProviders);
            return sourceRepositoryProvider;
        }

        public static SourceRepositoryProvider CreateSourceRepositoryProvider(IEnumerable<PackageSource> packageSources, IEnumerable<Lazy<INuGetResourceProvider>> mockNuGetResourceProviders)
        {
            var thisUtility = new TestSourceRepositoryUtility();
            thisUtility.Initialize();
            var packageSourceProvider = new TestPackageSourceProvider(packageSources);

            var sourceRepositoryProvider = new SourceRepositoryProvider(packageSourceProvider, mockNuGetResourceProviders.Concat(thisUtility.ResourceProviders));
            return sourceRepositoryProvider;
        }


        public static SourceRepositoryProvider CreateSourceRepositoryProvider(IPackageSourceProvider packageSourceProvider)
        {
            var thisUtility = new TestSourceRepositoryUtility();
            thisUtility.Initialize();

            var sourceRepositoryProvider = new SourceRepositoryProvider(packageSourceProvider, thisUtility.ResourceProviders);
            return sourceRepositoryProvider;
        }
    }

    /// <summary>
    /// Provider that only returns V3 as a source
    /// </summary>
    public class TestPackageSourceProvider : IPackageSourceProvider
    {
        private IEnumerable<PackageSource> PackageSources { get; set; }

        public TestPackageSourceProvider(IEnumerable<PackageSource> packageSources)
        {
            PackageSources = packageSources;
        }

        public IEnumerable<PackageSource> LoadPackageSources() => PackageSources;

        public event EventHandler PackageSourcesChanged;

        public void SavePackageSources(IEnumerable<PackageSource> sources)
        {
            PackageSources = sources;
            PackageSourcesChanged?.Invoke(this, null);
        }

        public string ActivePackageSourceName => throw new NotImplementedException();

        public string DefaultPushSource => throw new NotImplementedException();

        public void SaveActivePackageSource(PackageSource source) => throw new NotImplementedException();

        public PackageSource GetPackageSource(string name) => PackageSources.Where(s => string.Equals(s.Name, name, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

        public void RemovePackageSource(string name) => throw new NotImplementedException();

        public void EnablePackageSource(string name) => throw new NotImplementedException();

        public void DisablePackageSource(string name) => throw new NotImplementedException();

        public PackageSource GetPackageSourceByName(string name) => throw new NotImplementedException();

        public PackageSource GetPackageSourceBySource(string source) => throw new NotImplementedException();

        public void UpdatePackageSource(PackageSource source, bool updateCredentials, bool updateEnabled) => throw new NotImplementedException();

        public void AddPackageSource(PackageSource source) => throw new NotImplementedException();

        public bool IsPackageSourceEnabled(string name) => throw new NotImplementedException();

        // TODO: Remove depracted APIs

        public void DisablePackageSource(PackageSource source) => throw new NotImplementedException();

        public bool IsPackageSourceEnabled(PackageSource source) => throw new NotImplementedException();
    }

    public static class TestPackageSourceSettings
    {
        public static string TempPackageSourceContents =
            @"<?xml version='1.0' encoding='utf-8'?>
<configuration>
  <packageSources>
    <add key='nuget.org' value='https://www.nuget.org/api/v2/' />
  </packageSources>
</configuration>";

        public static TestDirectory CreateAndGetSettingFilePath()
        {
            var tempFolder = TestDirectory.Create();
            var fileName = "nuget.config";

            File.WriteAllText(Path.Combine(tempFolder, fileName), TempPackageSourceContents);
            return tempFolder;
        }
    }
}
