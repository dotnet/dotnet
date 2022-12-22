// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using NuGet.Common;
using NuGet.Configuration;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace NuGet.Test.Utility
{
    public sealed class PackageSourceTheoryAttribute : TheoryAttribute
    {
        private string _skip;

        public bool CIOnly { get; set; }

        public string ConfigFile { get; set; } = TestSources.ConfigFile;

        public string Root { get; } = TestSources.GetConfigFileRoot();

        public override string Skip
        {
            get
            {
                var skip = _skip;

                if (string.IsNullOrEmpty(skip))
                {
                    if (CIOnly && !XunitAttributeUtility.IsCI)
                    {
                        skip = "This test only runs on the CI. To run it locally set the env var CI=true";
                    }
                    else
                    {

                        var fullPath = Path.Combine(Root, ConfigFile);
                        // Skip if a file does not exist, otherwise run the test.
                        if (!File.Exists(fullPath))
                        {
                            skip = $"Required file does not exist: '{fullPath}'.";
                        }
                    }
                }

                // If this is null the test will run.
                return skip;
            }

            set => _skip = value;
        }

        public PackageSourceTheoryAttribute()
        {
        }
    }

    [DataDiscoverer("NuGet.Test.Utility.PackageSourceDataDiscoverer", "Test.Utility")]
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public sealed class PackageSourceDataAttribute : DataAttribute
    {
        public ISet<string> SourceNames { get; }

        public PackageSourceDataAttribute(params string[] sourceNames)
        {
            SourceNames = new HashSet<string>(sourceNames, StringComparer.OrdinalIgnoreCase);
        }

        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            throw new NotImplementedException();
        }
    }

    public sealed class PackageSourceDataDiscoverer : IDataDiscoverer
    {
        private readonly ConcurrentDictionary<string, PackageSource[]> _cachedSources = new ConcurrentDictionary<string, PackageSource[]>();
        private readonly string _root = TestSources.GetConfigFileRoot();

        public IEnumerable<object[]> GetData(IAttributeInfo dataAttribute, IMethodInfo testMethod)
        {
            var reflectionDataAttribute = dataAttribute as IReflectionAttributeInfo;
            var reflectionTestMethod = testMethod as IReflectionMethodInfo;

            if (reflectionDataAttribute != null && reflectionTestMethod != null)
            {
                var testMethodInfo = reflectionTestMethod.MethodInfo;

                var theoryAttribute = testMethodInfo.GetCustomAttribute<PackageSourceTheoryAttribute>();
                if (theoryAttribute == null)
                {
                    throw new ArgumentException("Theory attribute is required.");
                }

                var parameters = testMethodInfo.GetParameters();
                if (parameters.Length != 1)
                {
                    throw new ArgumentException("Invalid number of parameters. Should be 1.");
                }

                if (!string.IsNullOrEmpty(theoryAttribute.Skip))
                {
                    return Enumerable.Empty<object[]>();
                }

                var realDataAttribute = (PackageSourceDataAttribute)reflectionDataAttribute.Attribute;

                if (!string.IsNullOrEmpty(realDataAttribute.Skip))
                {
                    return Enumerable.Empty<object[]>();
                }

                var packageSources = GetTheorySources(theoryAttribute)
                    .Where(s => s.IsEnabled && realDataAttribute.SourceNames.Contains(s.Name))
                    .ToList();

                if (parameters[0].ParameterType == typeof(PackageSource))
                {
                    return packageSources.Select(s => new object[] { s });
                }
                else if (parameters[0].ParameterType == typeof(string))
                {
                    return packageSources.Select(s => new object[] { s.Source });
                }

                throw new ArgumentException("Unsupported parameter type.");
            }

            return null;
        }

        public bool SupportsDiscoveryEnumeration(IAttributeInfo dataAttribute, IMethodInfo testMethod) => true;

        private PackageSource[] GetTheorySources(PackageSourceTheoryAttribute theoryAttribute)
        {
            return _cachedSources.GetOrAdd(
                theoryAttribute.ConfigFile,
                configFile =>
                {
                    var settings = new Settings(_root, configFile);
                    var provider = new PackageSourceProvider(settings);
                    return provider.LoadPackageSources().ToArray();
                });
        }
    }
}
