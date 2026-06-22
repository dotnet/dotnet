// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Text;
using System.Threading;
using EnvDTE;
using Moq;
using NuGet.Frameworks;
using NuGet.LibraryModel;
using NuGet.PackageManagement.VisualStudio.Projects;
using NuGet.ProjectManagement;
using NuGet.ProjectModel;
using VSLangProj;
using VSLangProj150;

namespace Test.Utility
{
    public class TestProjectSystemServices : ILegacyPackageReferenceProjectServices
    {
        public TestProjectSystemServices()
        {
            Mock.Get(ReferencesReader)
                .Setup(x => x.GetProjectReferencesAsync(
                    It.IsAny<NuGet.Common.ILogger>(), CancellationToken.None))
                .ReturnsAsync(() => new ProjectRestoreReference[] { });

            Mock.Get(ReferencesReader)
                .Setup(x => x.GetPackageReferencesAsync(
                    It.IsAny<NuGetFramework>(), CancellationToken.None))
                .ReturnsAsync(() => new LibraryDependency[] { });
        }

        public IProjectSystemCapabilities Capabilities { get; } = Mock.Of<IProjectSystemCapabilities>();

        public IProjectSystemReferencesReader ReferencesReader { get; } = Mock.Of<IProjectSystemReferencesReader>();

        public IProjectSystemService ProjectSystem { get; } = Mock.Of<IProjectSystemService>();

        public IProjectSystemReferencesService References { get; } = Mock.Of<IProjectSystemReferencesService>();

        public IProjectScriptHostService ScriptService { get; } = Mock.Of<IProjectScriptHostService>();

        public VSProject4 Project4 { get; } = Mock.Of<VSProject4>();

        public T GetGlobalService<T>() where T : class
        {
            throw new NotImplementedException();
        }

        public void SetupInstalledPackages(NuGetFramework targetFramework, params LibraryDependency[] dependencies)
        {
            Mock.Get(ReferencesReader)
                .Setup(x => x.GetPackageReferencesAsync(targetFramework, CancellationToken.None))
                .ReturnsAsync(dependencies.ToList());

            Mock.Get(Project4)
                .Setup(x => x.PackageReferences)
                .Returns(new TestPackageReferences(dependencies));
        }

        public void SetupProjectDependencies(params ProjectRestoreReference[] dependencies)
        {
            Mock.Get(ReferencesReader)
                .Setup(x => x.GetProjectReferencesAsync(It.IsAny<NuGet.Common.ILogger>(), CancellationToken.None))
                .ReturnsAsync(dependencies.ToList());

            Mock.Get(Project4)
                .SetupGet(x => x.References)
                .Returns(() =>
                {
                    Mock<References> references = new(MockBehavior.Strict);

                    references.Setup(r => r.GetEnumerator())
                        .Returns(() =>
                        {
                            var projectReferences = dependencies.Select(d =>
                            {
                                var mockReference = new Mock<Reference6>(MockBehavior.Strict);

                                mockReference.SetupGet(x => x.SourceProject).Returns(() =>
                                {
                                    var mockProject = new Mock<Project>(MockBehavior.Strict);
                                    mockProject.SetupGet(p => p.FullName).Returns(d.ProjectUniqueName);
                                    return mockProject.Object;
                                });

                                mockReference.Setup(x => x.GetMetadata(It.IsAny<Array>(), out It.Ref<Array>.IsAny, out It.Ref<Array>.IsAny))
                                    .Callback(new GetMetadataDelegate((Array desiredMetadata, out Array metadataElements, out Array metadataValues) =>
                                    {
                                        string?[] values = new string[desiredMetadata.Length];

                                        for (var i = 0; i < values.Length; i++)
                                        {
                                            string metadataName = (string)desiredMetadata.GetValue(i);
                                            string? value = GetMetadataValue(metadataName);
                                            values[i] = value;
                                        }

                                        metadataElements = desiredMetadata;
                                        metadataValues = values;

                                        string? GetMetadataValue(string metadataName)
                                        {
                                            if (metadataName.Equals("ReferenceOutputAssembly", StringComparison.OrdinalIgnoreCase))
                                            {
                                                return null;
                                            }
                                            else if (metadataName.Equals("FullPath", StringComparison.OrdinalIgnoreCase))
                                            {
                                                return d.ProjectUniqueName;
                                            }
                                            else if (metadataName.Equals(ProjectItemProperties.IncludeAssets, StringComparison.OrdinalIgnoreCase))
                                            {
                                                return NuGet.Common.MSBuildStringUtility.Convert(LibraryIncludeFlagUtils.GetFlagString(d.IncludeAssets));
                                            }
                                            else if (metadataName.Equals(ProjectItemProperties.ExcludeAssets, StringComparison.OrdinalIgnoreCase))
                                            {
                                                return NuGet.Common.MSBuildStringUtility.Convert(LibraryIncludeFlagUtils.GetFlagString(d.ExcludeAssets));
                                            }
                                            else if (metadataName.Equals(ProjectItemProperties.PrivateAssets, StringComparison.OrdinalIgnoreCase))
                                            {
                                                return NuGet.Common.MSBuildStringUtility.Convert(LibraryIncludeFlagUtils.GetFlagString(d.PrivateAssets));
                                            }
                                            throw new NotImplementedException(metadataName);
                                        }
                                    }));

                                return mockReference.Object;
                            }).ToList();
                            return projectReferences.GetEnumerator();
                        });

                    return references.Object;
                });
        }

        private delegate void GetMetadataDelegate(Array parrbstrDesiredMetadata, out Array pparrbstrMetadataElements, out Array pparrbstrMetadataValues);

        private class TestPackageReferences : PackageReferences
        {
            private readonly LibraryDependency[] _dependencies;
            public TestPackageReferences(LibraryDependency[] dependencies)
            {
                _dependencies = dependencies;
            }

            public void AddOrUpdate(string bstrName, string bstrVersion, Array pbstrMetadataElements, Array pbstrMetadataValues)
            {
                throw new NotImplementedException();
            }

            public void Remove(string bstrName)
            {
                throw new NotImplementedException();
            }

            public bool TryGetReference(string bstrName, Array parrbstrDesiredMetadata, out string? pbstrVersion, out Array? pbstrMetadataElements, out Array? pbstrMetadataValues)
            {
                var package = _dependencies.FirstOrDefault(d => d.Name.Equals(bstrName, StringComparison.OrdinalIgnoreCase));
                if (package is null)
                {
                    pbstrVersion = null;
                    pbstrMetadataElements = null;
                    pbstrMetadataValues = null;
                    return false;
                }

                string?[] metadataValues = new string[parrbstrDesiredMetadata.Length];
                for (int i = 0; i < metadataValues.Length; i++)
                {
                    string metadataName = (string)parrbstrDesiredMetadata.GetValue(i);
                    string? metadataValue = GetPackageReferenceMetadata(metadataName, package);
                    metadataValues[i] = metadataValue;
                }

                pbstrVersion = package.LibraryRange.VersionRange?.OriginalString;
                pbstrMetadataElements = parrbstrDesiredMetadata;
                pbstrMetadataValues = metadataValues;
                return true;

                string? GetPackageReferenceMetadata(string metadataName, LibraryDependency package)
                {
                    if (metadataName.Equals(ProjectItemProperties.IsImplicitlyDefined, StringComparison.OrdinalIgnoreCase))
                    {
                        return package.AutoReferenced.ToString();
                    }
                    else if (metadataName.Equals("Version", StringComparison.OrdinalIgnoreCase))
                    {
                        return package.LibraryRange.VersionRange?.OriginalString ?? package.LibraryRange.VersionRange?.ToString();
                    }
                    else if (metadataName.Equals(ProjectItemProperties.VersionOverride, StringComparison.OrdinalIgnoreCase))
                    {
                        return package.VersionOverride?.OriginalString;
                    }
                    else if (metadataName.Equals(ProjectItemProperties.GeneratePathProperty, StringComparison.OrdinalIgnoreCase))
                    {
                        return package.GeneratePathProperty.ToString();
                    }
                    else if (metadataName.Equals(ProjectItemProperties.Aliases, StringComparison.OrdinalIgnoreCase))
                    {
                        return package.Aliases;
                    }
                    else if (metadataName.Equals(ProjectItemProperties.IncludeAssets, StringComparison.OrdinalIgnoreCase))
                    {
                        string result = LibraryIncludeFlagUtils.GetFlagString(package.IncludeType);
                        return NuGet.Common.MSBuildStringUtility.Convert(result);
                    }
                    else if (metadataName.Equals(ProjectItemProperties.ExcludeAssets, StringComparison.OrdinalIgnoreCase))
                    {
                        LibraryIncludeFlags excludeFlags = LibraryIncludeFlags.All & ~package.IncludeType;
                        string result = LibraryIncludeFlagUtils.GetFlagString(excludeFlags);
                        return NuGet.Common.MSBuildStringUtility.Convert(result);
                    }
                    else if (metadataName.Equals(ProjectItemProperties.PrivateAssets, StringComparison.OrdinalIgnoreCase))
                    {
                        string result = LibraryIncludeFlagUtils.GetFlagString(package.SuppressParent);
                        return NuGet.Common.MSBuildStringUtility.Convert(result);
                    }
                    else if (metadataName.Equals(ProjectItemProperties.NoWarn, StringComparison.OrdinalIgnoreCase))
                    {
                        var noWarn = package.NoWarn;
                        if (noWarn.Length == 0) { return null; }
                        StringBuilder sb = new StringBuilder();
                        foreach (var code in noWarn)
                        {
                            if (sb.Length != 0) { sb.Append(';'); }
                            sb.Append(code);
                        }
                        return sb.ToString();
                    }
                    throw new NotImplementedException(metadataName);
                }
            }

            public DTE DTE => throw new NotImplementedException();

            public object Parent => throw new NotImplementedException();

            public Project ContainingProject => throw new NotImplementedException();

            public Array InstalledPackages
            {
                get
                {
                    string[] packages = new string[_dependencies.Length];
                    for (int i = 0; i < packages.Length; i++)
                    {
                        packages[i] = _dependencies[i].LibraryRange.Name;
                    }
                    return packages;
                }
            }
        }
    }
}
