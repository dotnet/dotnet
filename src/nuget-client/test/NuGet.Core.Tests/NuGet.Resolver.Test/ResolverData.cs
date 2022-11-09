// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NuGet.Versioning;

namespace NuGet.Resolver.Test
{
    public static class ResolverData
    {
        private static List<ResolverPackage> CreateEntityFramework()
        {
            var packages = new List<ResolverPackage>();

            packages.Add(new ResolverPackage("EntityFramework", NuGetVersion.Parse("7.0.0-beta4"),
                new NuGet.Packaging.Core.PackageDependency[]
                {
                    new NuGet.Packaging.Core.PackageDependency("EntityFramework.Commands",
                        new VersionRange(NuGetVersion.Parse("7.0.0-beta4"), true, NuGetVersion.Parse("7.0.0-beta4"), true)),
                }
                , true, false));

            return packages;
        }

        private static List<ResolverPackage> CreateEntityFrameworkCommands()
        {
            var packages = new List<ResolverPackage>();

            packages.Add(new ResolverPackage("EntityFramework.Commands", NuGetVersion.Parse("7.0.0-beta1"),
                new NuGet.Packaging.Core.PackageDependency[]
                {
                    new NuGet.Packaging.Core.PackageDependency("EntityFramework.Migrations", new VersionRange(NuGetVersion.Parse("7.0.0-beta1"), true, NuGetVersion.Parse("7.0.0-beta4"), true)),
                }
                , true, false));

            packages.Add(new ResolverPackage("EntityFramework.Commands", NuGetVersion.Parse("7.0.0-beta2"),
                new NuGet.Packaging.Core.PackageDependency[]
                {
                    new NuGet.Packaging.Core.PackageDependency("EntityFramework.Migrations", new VersionRange(NuGetVersion.Parse("7.0.0-beta1"), true, NuGetVersion.Parse("7.0.0-beta4"), true)),
                    new NuGet.Packaging.Core.PackageDependency("Microsoft.Framework.DependencyInjection", new VersionRange(NuGetVersion.Parse("1.0.0-beta2"), true, NuGetVersion.Parse("1.0.0-beta4"), true)),
                }
                , true, false));

            packages.Add(new ResolverPackage("EntityFramework.Commands", NuGetVersion.Parse("7.0.0-beta3"),
                new NuGet.Packaging.Core.PackageDependency[]
                {
                    new NuGet.Packaging.Core.PackageDependency("EntityFramework.Relational", new VersionRange(NuGetVersion.Parse("7.0.0-beta3"), true, NuGetVersion.Parse("7.0.0-beta4"), true)),
                    new NuGet.Packaging.Core.PackageDependency("Microsoft.Framework.DependencyInjection", new VersionRange(NuGetVersion.Parse("1.0.0-beta3"), true, NuGetVersion.Parse("1.0.0-beta4"), true)),
                }
                , true, false));

            packages.Add(new ResolverPackage("EntityFramework.Commands", NuGetVersion.Parse("7.0.0-beta4"),
                new NuGet.Packaging.Core.PackageDependency[]
                {
                    new NuGet.Packaging.Core.PackageDependency("EntityFramework.Relational", new VersionRange(NuGetVersion.Parse("7.0.0-beta4"), true, NuGetVersion.Parse("7.0.0-beta4"), true)),
                    new NuGet.Packaging.Core.PackageDependency("EntityFramework.Relational.Design", new VersionRange(NuGetVersion.Parse("7.0.0-beta4"), true, NuGetVersion.Parse("7.0.0-beta4"), true)),
                    new NuGet.Packaging.Core.PackageDependency("EntityFramework.SqlServer.Design", new VersionRange(NuGetVersion.Parse("7.0.0-beta4"), true, NuGetVersion.Parse("7.0.0-beta4"), true)),
                    new NuGet.Packaging.Core.PackageDependency("Microsoft.CodeAnalysis.CSharp", new VersionRange(NuGetVersion.Parse("1.0.0-rc2"), true, NuGetVersion.Parse("1.0.0-rc2"), true)),
                    new NuGet.Packaging.Core.PackageDependency("Microsoft.Framework.DependencyInjection", new VersionRange(NuGetVersion.Parse("1.0.0-beta4"), true, NuGetVersion.Parse("1.0.0-beta4"), true)),
                }
                , true, false));

            return packages;
        }

        private static List<ResolverPackage> CreateEntityFrameworkMigrations()
        {
            var packages = new List<ResolverPackage>();

            packages.Add(new ResolverPackage("EntityFramework.Migrations", NuGetVersion.Parse("0.6.0"),
                new NuGet.Packaging.Core.PackageDependency[]
                {
                    new NuGet.Packaging.Core.PackageDependency("EntityFramework", new VersionRange(NuGetVersion.Parse("4.1.10715"))),
                }
                , true, false));

            packages.Add(new ResolverPackage("EntityFramework.Migrations", NuGetVersion.Parse("0.6.1"),
                new NuGet.Packaging.Core.PackageDependency[]
                {
                    new NuGet.Packaging.Core.PackageDependency("EntityFramework", new VersionRange(NuGetVersion.Parse("4.1.10715"))),
                    new NuGet.Packaging.Core.PackageDependency("SqlServerCompact", new VersionRange(NuGetVersion.Parse("4.0.8482.1"))),
                }
                , true, false));

            packages.Add(new ResolverPackage("EntityFramework.Migrations", NuGetVersion.Parse("0.6.2"),
                new NuGet.Packaging.Core.PackageDependency[]
                {
                    new NuGet.Packaging.Core.PackageDependency("EntityFramework", new VersionRange(NuGetVersion.Parse("4.1.10715"))),
                }
                , true, false));

            packages.Add(new ResolverPackage("EntityFramework.Migrations", NuGetVersion.Parse("0.7.0"),
                new NuGet.Packaging.Core.PackageDependency[]
                {
                    new NuGet.Packaging.Core.PackageDependency("EntityFramework", new VersionRange(NuGetVersion.Parse("4.1.10715"))),
                }
                , true, false));

            packages.Add(new ResolverPackage("EntityFramework.Migrations", NuGetVersion.Parse("0.7.0.1"),
                new NuGet.Packaging.Core.PackageDependency[]
                {
                    new NuGet.Packaging.Core.PackageDependency("EntityFramework", new VersionRange(NuGetVersion.Parse("4.2.0"))),
                }
                , true, false));

            packages.Add(new ResolverPackage("EntityFramework.Migrations", NuGetVersion.Parse("0.8.0"),
                new NuGet.Packaging.Core.PackageDependency[]
                {
                    new NuGet.Packaging.Core.PackageDependency("EntityFramework", new VersionRange(NuGetVersion.Parse("4.2.0"))),
                }
                , true, false));

            packages.Add(new ResolverPackage("EntityFramework.Migrations", NuGetVersion.Parse("0.9.0"), null, true, false));

            packages.Add(new ResolverPackage("EntityFramework.Migrations", NuGetVersion.Parse("7.0.0-beta1"),
                new NuGet.Packaging.Core.PackageDependency[]
                {
                    new NuGet.Packaging.Core.PackageDependency("EntityFramework.Relational", new VersionRange(NuGetVersion.Parse("7.0.0-beta1"))),
                }
                , true, false));

            packages.Add(new ResolverPackage("EntityFramework.Migrations", NuGetVersion.Parse("7.0.0-beta2"),
                new NuGet.Packaging.Core.PackageDependency[]
                {
                    new NuGet.Packaging.Core.PackageDependency("EntityFramework.Relational", new VersionRange(NuGetVersion.Parse("7.0.0-beta2"))),
                }
                , true, false));

            return packages;
        }

        private static List<ResolverPackage> CreateEntityFrameworkRelational()
        {
            var packages = new List<ResolverPackage>();

            packages.Add(new ResolverPackage("EntityFramework.Relational", NuGetVersion.Parse("7.0.0-beta1"),
                new NuGet.Packaging.Core.PackageDependency[]
                {
                    new NuGet.Packaging.Core.PackageDependency("EntityFramework", new VersionRange(NuGetVersion.Parse("7.0.0-beta1"))),
                    new NuGet.Packaging.Core.PackageDependency("System.Data.Common", new VersionRange(NuGetVersion.Parse("1.0.0-beta1"))),
                }
                , true, false));

            packages.Add(new ResolverPackage("EntityFramework.Relational", NuGetVersion.Parse("7.0.0-beta2"),
                new NuGet.Packaging.Core.PackageDependency[]
                {
                    new NuGet.Packaging.Core.PackageDependency("EntityFramework.Core", new VersionRange(NuGetVersion.Parse("7.0.0-beta2"))),
                    new NuGet.Packaging.Core.PackageDependency("System.Data.Common", new VersionRange(NuGetVersion.Parse("1.0.0-beta2"))),
                }
                , true, false));

            packages.Add(new ResolverPackage("EntityFramework.Relational", NuGetVersion.Parse("7.0.0-beta3"),
                new NuGet.Packaging.Core.PackageDependency[]
                {
                    new NuGet.Packaging.Core.PackageDependency("EntityFramework.Core", new VersionRange(NuGetVersion.Parse("7.0.0-beta3"))),
                }
                , true, false));

            packages.Add(new ResolverPackage("EntityFramework.Relational", NuGetVersion.Parse("7.0.0-beta4"),
                new NuGet.Packaging.Core.PackageDependency[]
                {
                    new NuGet.Packaging.Core.PackageDependency("EntityFramework.Core", new VersionRange(NuGetVersion.Parse("7.0.0-beta4"))),
                }
                , true, false));

            return packages;
        }

        private static List<ResolverPackage> CreateEntityFrameworkCore()
        {
            var packages = new List<ResolverPackage>();

            packages.Add(new ResolverPackage("EntityFramework.Core", NuGetVersion.Parse("7.0.0-beta2"),
                new NuGet.Packaging.Core.PackageDependency[]
                {
                    new NuGet.Packaging.Core.PackageDependency("Ix-Async", new VersionRange(NuGetVersion.Parse("1.2.3-beta"))),
                    new NuGet.Packaging.Core.PackageDependency("Microsoft.Framework.Logging", new VersionRange(NuGetVersion.Parse("1.0.0-beta2"))),
                    new NuGet.Packaging.Core.PackageDependency("Microsoft.Framework.OptionsModel", new VersionRange(NuGetVersion.Parse("1.0.0-beta2"))),
                    new NuGet.Packaging.Core.PackageDependency("Remotion.Linq", new VersionRange(NuGetVersion.Parse("1.15.15"))),
                    new NuGet.Packaging.Core.PackageDependency("System.Collections.Immutable", new VersionRange(NuGetVersion.Parse("1.1.32-beta"))),
                }
                , true, false));

            packages.Add(new ResolverPackage("EntityFramework.Core", NuGetVersion.Parse("7.0.0-beta3"),
                new NuGet.Packaging.Core.PackageDependency[]
                {
                    new NuGet.Packaging.Core.PackageDependency("Ix-Async", new VersionRange(NuGetVersion.Parse("1.2.3"))),
                    new NuGet.Packaging.Core.PackageDependency("Microsoft.Framework.Logging", new VersionRange(NuGetVersion.Parse("1.0.0-beta3"))),
                    new NuGet.Packaging.Core.PackageDependency("Microsoft.Framework.OptionsModel", new VersionRange(NuGetVersion.Parse("1.0.0-beta3"))),
                    new NuGet.Packaging.Core.PackageDependency("Remotion.Linq", new VersionRange(NuGetVersion.Parse("1.15.15"))),
                    new NuGet.Packaging.Core.PackageDependency("System.Collections.Immutable", new VersionRange(NuGetVersion.Parse("1.1.32-beta"))),
                }
                , true, false));

            packages.Add(new ResolverPackage("EntityFramework.Core", NuGetVersion.Parse("7.0.0-beta4"),
                new NuGet.Packaging.Core.PackageDependency[]
                {
                    new NuGet.Packaging.Core.PackageDependency("Ix-Async", new VersionRange(NuGetVersion.Parse("1.2.3"))),
                    new NuGet.Packaging.Core.PackageDependency("Microsoft.Framework.Caching.Memory", new VersionRange(NuGetVersion.Parse("1.0.0-beta4"))),
                    new NuGet.Packaging.Core.PackageDependency("Microsoft.Framework.DependencyInjection", new VersionRange(NuGetVersion.Parse("1.0.0-beta4"))),
                    new NuGet.Packaging.Core.PackageDependency("Microsoft.Framework.Logging", new VersionRange(NuGetVersion.Parse("1.0.0-beta4"))),
                    new NuGet.Packaging.Core.PackageDependency("Microsoft.Framework.OptionsModel", new VersionRange(NuGetVersion.Parse("1.0.0-beta4"))),
                    new NuGet.Packaging.Core.PackageDependency("Remotion.Linq", new VersionRange(NuGetVersion.Parse("2.0.0-alpha-002"))),
                    new NuGet.Packaging.Core.PackageDependency("System.Collections.Immutable", new VersionRange(NuGetVersion.Parse("1.1.33-beta"))),
                }
                , true, false));

            return packages;
        }

        private static List<ResolverPackage> CreateEntityFrameworkRelationalDesign()
        {
            var packages = new List<ResolverPackage>();

            packages.Add(new ResolverPackage("EntityFramework.Relational.Design", NuGetVersion.Parse("7.0.0-beta4"),
                new NuGet.Packaging.Core.PackageDependency[]
                {
                    new NuGet.Packaging.Core.PackageDependency("EntityFramework.Relational", new VersionRange(NuGetVersion.Parse("7.0.0-beta4"))),
                    new NuGet.Packaging.Core.PackageDependency("EntityFramework.Core", new VersionRange(NuGetVersion.Parse("7.0.0-beta4"))),
                }
                , true, false));

            return packages;
        }

        private static List<ResolverPackage> CreateEntityFrameworkSqlServerDesign()
        {
            var packages = new List<ResolverPackage>();

            packages.Add(new ResolverPackage("EntityFramework.SqlServer.Design", NuGetVersion.Parse("7.0.0-beta4"),
                new NuGet.Packaging.Core.PackageDependency[]
                {
                    new NuGet.Packaging.Core.PackageDependency("EntityFramework.Relational.Design", new VersionRange(NuGetVersion.Parse("7.0.0-beta4"))),
                    new NuGet.Packaging.Core.PackageDependency("EntityFramework.SqlServer", new VersionRange(NuGetVersion.Parse("7.0.0-beta4"))),
                }
                , true, false));

            return packages;
        }

        private static List<ResolverPackage> CreateEntityFrameworkSqlServer()
        {
            var packages = new List<ResolverPackage>();

            packages.Add(new ResolverPackage("EntityFramework.SqlServer", NuGetVersion.Parse("7.0.0-beta1"),
                new NuGet.Packaging.Core.PackageDependency[]
                {
                    new NuGet.Packaging.Core.PackageDependency("EntityFramework.Migrations", new VersionRange(NuGetVersion.Parse("7.0.0-beta1"))),
                    new NuGet.Packaging.Core.PackageDependency("System.Data.SqlClient", new VersionRange(NuGetVersion.Parse("1.0.0-beta1"))),
                }
                , true, false));

            packages.Add(new ResolverPackage("EntityFramework.SqlServer", NuGetVersion.Parse("7.0.0-beta2"),
                new NuGet.Packaging.Core.PackageDependency[]
                {
                    new NuGet.Packaging.Core.PackageDependency("EntityFramework.Migrations", new VersionRange(NuGetVersion.Parse("7.0.0-beta2"))),
                    new NuGet.Packaging.Core.PackageDependency("System.Data.SqlClient", new VersionRange(NuGetVersion.Parse("1.0.0-beta2"))),
                }
                , true, false));

            packages.Add(new ResolverPackage("EntityFramework.SqlServer", NuGetVersion.Parse("7.0.0-beta3"),
                new NuGet.Packaging.Core.PackageDependency[]
                {
                    new NuGet.Packaging.Core.PackageDependency("EntityFramework.Relational", new VersionRange(NuGetVersion.Parse("7.0.0-beta4"))),
                }
                , true, false));

            packages.Add(new ResolverPackage("EntityFramework.SqlServer", NuGetVersion.Parse("7.0.0-beta4"),
                new NuGet.Packaging.Core.PackageDependency[]
                {
                    new NuGet.Packaging.Core.PackageDependency("EntityFramework.Relational", new VersionRange(NuGetVersion.Parse("7.0.0-beta4"))),
                }
                , true, false));

            return packages;
        }

        private static List<ResolverPackage> CreateMicrosoftFrameworkCachingMemory()
        {
            var packages = new List<ResolverPackage>();

            packages.Add(new ResolverPackage("Microsoft.Framework.Caching.Memory", NuGetVersion.Parse("1.0.0-beta4"),
                new NuGet.Packaging.Core.PackageDependency[]
                {
                    new NuGet.Packaging.Core.PackageDependency("Microsoft.Framework.OptionsModel", new VersionRange(NuGetVersion.Parse("1.0.0-beta4"))),
                    new NuGet.Packaging.Core.PackageDependency("Microsoft.Framework.Caching.Interfaces", new VersionRange(NuGetVersion.Parse("1.0.0-beta4"))),
                }
                , true, false));

            return packages;
        }

        private static List<ResolverPackage> CreateSystemIO()
        {
            var packages = new List<ResolverPackage>();

            packages.Add(new ResolverPackage("System.IO", NuGetVersion.Parse("4.0.10-beta-22231"),
                new NuGet.Packaging.Core.PackageDependency[]
                {
                    new NuGet.Packaging.Core.PackageDependency("System.Runtime", new VersionRange(NuGetVersion.Parse("4.0.0-beta-22231"))),
                }
                , true, false));

            packages.Add(new ResolverPackage("System.IO", NuGetVersion.Parse("4.0.10-beta-22416"),
                new NuGet.Packaging.Core.PackageDependency[]
                {
                    new NuGet.Packaging.Core.PackageDependency("System.Runtime", new VersionRange(NuGetVersion.Parse("4.0.10-beta-22416"))),
                }
                , true, false));

            packages.Add(new ResolverPackage("System.IO", NuGetVersion.Parse("4.0.10-beta-22605"),
                new NuGet.Packaging.Core.PackageDependency[]
                {
                    new NuGet.Packaging.Core.PackageDependency("System.Runtime", new VersionRange(NuGetVersion.Parse("4.0.10-beta-22605"))),
                }
                , true, false));

            packages.Add(new ResolverPackage("System.IO", NuGetVersion.Parse("4.0.10-beta-22816"),
                new NuGet.Packaging.Core.PackageDependency[]
                {
                    new NuGet.Packaging.Core.PackageDependency("System.Runtime", new VersionRange(NuGetVersion.Parse("4.0.10-beta-22816"))),
                }
                , true, false));

            return packages;
        }

        private static List<ResolverPackage> CreateSystemRuntime()
        {
            var packages = new List<ResolverPackage>();

            packages.Add(new ResolverPackage("System.Runtime", NuGetVersion.Parse("4.0.20-beta-22231"), null, true, false));
            packages.Add(new ResolverPackage("System.Runtime", NuGetVersion.Parse("4.0.20-beta-22416"), null, true, false));
            packages.Add(new ResolverPackage("System.Runtime", NuGetVersion.Parse("4.0.20-beta-22605"), null, true, false));
            packages.Add(new ResolverPackage("System.Runtime", NuGetVersion.Parse("4.0.20-beta-22816"), null, true, false));

            return packages;
        }

        private static List<ResolverPackage> CreateSystemThreadingTasks()
        {
            var packages = new List<ResolverPackage>();

            packages.Add(new ResolverPackage("System.Threading.Tasks", NuGetVersion.Parse("2.0.0"), null, true, false));
            packages.Add(new ResolverPackage("System.Threading.Tasks", NuGetVersion.Parse("2.0.1"), null, true, false));
            packages.Add(new ResolverPackage("System.Threading.Tasks", NuGetVersion.Parse("2.1.0"), null, true, false));
            packages.Add(new ResolverPackage("System.Threading.Tasks", NuGetVersion.Parse("2.1.1"), null, true, false));
            packages.Add(new ResolverPackage("System.Threading.Tasks", NuGetVersion.Parse("2.1.2"), null, true, false));
            packages.Add(new ResolverPackage("System.Threading.Tasks", NuGetVersion.Parse("3.0.0"), null, true, false));
            packages.Add(new ResolverPackage("System.Threading.Tasks", NuGetVersion.Parse("3.0.1"), null, true, false));
            packages.Add(new ResolverPackage("System.Threading.Tasks", NuGetVersion.Parse("3.0.2-beta1"), null, true, false));
            packages.Add(new ResolverPackage("System.Threading.Tasks", NuGetVersion.Parse("3.0.2-beta2"), null, true, false));

            //TODO: add dependencies
            packages.Add(new ResolverPackage("System.Threading.Tasks", NuGetVersion.Parse("3.1.0"), null, true, false));
            packages.Add(new ResolverPackage("System.Threading.Tasks", NuGetVersion.Parse("3.1.0-beta1"), null, true, false));
            packages.Add(new ResolverPackage("System.Threading.Tasks", NuGetVersion.Parse("3.1.0-beta2"), null, true, false));
            packages.Add(new ResolverPackage("System.Threading.Tasks", NuGetVersion.Parse("3.1.1"), null, true, false));
            packages.Add(new ResolverPackage("System.Threading.Tasks", NuGetVersion.Parse("4.0.10-beta-22231"), null, true, false));
            packages.Add(new ResolverPackage("System.Threading.Tasks", NuGetVersion.Parse("4.0.10-beta-22416"), null, true, false));
            packages.Add(new ResolverPackage("System.Threading.Tasks", NuGetVersion.Parse("4.0.10-beta-22605"), null, true, false));
            packages.Add(new ResolverPackage("System.Threading.Tasks", NuGetVersion.Parse("4.0.10-beta-22816"), null, true, false));

            return packages;
        }

        private static List<ResolverPackage> CreateMicrosoftFrameworkCachingInterfaces()
        {
            var packages = new List<ResolverPackage>();

            packages.Add(new ResolverPackage("Microsoft.Framework.Caching.Interfaces", NuGetVersion.Parse("1.0.0-beta4"), null, true, false));

            return packages;
        }

        private static List<ResolverPackage> CreateMicrosoftFrameworkDependencyInjection()
        {
            var packages = new List<ResolverPackage>();

            packages.Add(new ResolverPackage("Microsoft.Framework.DependencyInjection", NuGetVersion.Parse("1.0.0-beta1"),
                new NuGet.Packaging.Core.PackageDependency[]
                {
                    new NuGet.Packaging.Core.PackageDependency("Microsoft.Framework.ConfigurationModel", new VersionRange(NuGetVersion.Parse("1.0.0-beta1"))),
                }
                , true, false));

            packages.Add(new ResolverPackage("Microsoft.Framework.DependencyInjection", NuGetVersion.Parse("1.0.0-beta2"),
                new NuGet.Packaging.Core.PackageDependency[]
                {
                    new NuGet.Packaging.Core.PackageDependency("Microsoft.Framework.ConfigurationModel", new VersionRange(NuGetVersion.Parse("1.0.0-beta2"))),
                }
                , true, false));

            packages.Add(new ResolverPackage("Microsoft.Framework.DependencyInjection", NuGetVersion.Parse("1.0.0-beta3"),
                new NuGet.Packaging.Core.PackageDependency[]
                {
                    new NuGet.Packaging.Core.PackageDependency("Microsoft.Framework.ConfigurationModel", new VersionRange(NuGetVersion.Parse("1.0.0-beta3"))),
                }
                , true, false));

            packages.Add(new ResolverPackage("Microsoft.Framework.DependencyInjection", NuGetVersion.Parse("1.0.0-beta4"),
                new NuGet.Packaging.Core.PackageDependency[]
                {
                    new NuGet.Packaging.Core.PackageDependency("Microsoft.Framework.ConfigurationModel", new VersionRange(NuGetVersion.Parse("1.0.0-beta4"))),
                    new NuGet.Packaging.Core.PackageDependency("Microsoft.Framework.DependencyInjection.Interfaces", new VersionRange(NuGetVersion.Parse("1.0.0-beta4"))),
                }
                , true, false));

            return packages;
        }

        private static List<ResolverPackage> CreateMicrosoftFrameworkDependencyInjectionInterfaces()
        {
            var packages = new List<ResolverPackage>();

            packages.Add(new ResolverPackage("Microsoft.Framework.DependencyInjection.Interfaces", NuGetVersion.Parse("1.0.0-beta4"), null, true, false));

            return packages;
        }

        private static List<ResolverPackage> CreateMicrosoftFrameworkConfigurationModel()
        {
            var packages = new List<ResolverPackage>();

            packages.Add(new ResolverPackage("Microsoft.Framework.ConfigurationModel", NuGetVersion.Parse("1.0.0-beta1"), null, true, false));
            packages.Add(new ResolverPackage("Microsoft.Framework.ConfigurationModel", NuGetVersion.Parse("1.0.0-beta2"), null, true, false));
            packages.Add(new ResolverPackage("Microsoft.Framework.ConfigurationModel", NuGetVersion.Parse("1.0.0-beta3"), null, true, false));

            packages.Add(new ResolverPackage("Microsoft.Framework.ConfigurationModel", NuGetVersion.Parse("1.0.0-beta4"),
                new NuGet.Packaging.Core.PackageDependency[]
                {
                    new NuGet.Packaging.Core.PackageDependency("Microsoft.Framework.ConfigurationModel.Interfaces", new VersionRange(NuGetVersion.Parse("1.0.0-beta4"))),
                }
                , true, false));

            return packages;
        }

        private static List<ResolverPackage> CreateMicrosoftFrameworkConfigurationModelInterfaces()
        {
            var packages = new List<ResolverPackage>();

            packages.Add(new ResolverPackage("Microsoft.Framework.ConfigurationModel.Interfaces", NuGetVersion.Parse("1.0.0-beta4"), null, true, false));

            return packages;
        }

        private static List<ResolverPackage> CreateMicrosoftFrameworkLogging()
        {
            var packages = new List<ResolverPackage>();

            packages.Add(new ResolverPackage("Microsoft.Framework.Logging", NuGetVersion.Parse("1.0.0-beta1"),
                new NuGet.Packaging.Core.PackageDependency[]
                {
                    new NuGet.Packaging.Core.PackageDependency("Microsoft.Framework.Logging.Interfaces", new VersionRange(NuGetVersion.Parse("1.0.0-beta1"))),
                }
                , true, false));

            packages.Add(new ResolverPackage("Microsoft.Framework.Logging", NuGetVersion.Parse("1.0.0-beta2"),
                new NuGet.Packaging.Core.PackageDependency[]
                {
                    new NuGet.Packaging.Core.PackageDependency("Microsoft.Framework.Logging.Interfaces", new VersionRange(NuGetVersion.Parse("1.0.0-beta2"))),
                }
                , true, false));

            packages.Add(new ResolverPackage("Microsoft.Framework.Logging", NuGetVersion.Parse("1.0.0-beta3"),
                new NuGet.Packaging.Core.PackageDependency[]
                {
                    new NuGet.Packaging.Core.PackageDependency("Microsoft.Framework.Logging.Interfaces", new VersionRange(NuGetVersion.Parse("1.0.0-beta3"))),
                }
                , true, false));

            packages.Add(new ResolverPackage("Microsoft.Framework.Logging", NuGetVersion.Parse("1.0.0-beta4"),
                new NuGet.Packaging.Core.PackageDependency[]
                {
                    new NuGet.Packaging.Core.PackageDependency("Microsoft.Framework.DependencyInjection.Interfaces", new VersionRange(NuGetVersion.Parse("1.0.0-beta4"))),
                    new NuGet.Packaging.Core.PackageDependency("Microsoft.Framework.Logging.Interfaces", new VersionRange(NuGetVersion.Parse("1.0.0-beta4"))),
                }
                , true, false));

            return packages;
        }

        private static List<ResolverPackage> CreateMicrosoftFrameworkLoggingInterfaces()
        {
            var packages = new List<ResolverPackage>();

            packages.Add(new ResolverPackage("Microsoft.Framework.Logging.Interfaces", NuGetVersion.Parse("1.0.0-beta1"), null, true, false));
            packages.Add(new ResolverPackage("Microsoft.Framework.Logging.Interfaces", NuGetVersion.Parse("1.0.0-beta2"), null, true, false));
            packages.Add(new ResolverPackage("Microsoft.Framework.Logging.Interfaces", NuGetVersion.Parse("1.0.0-beta3"), null, true, false));
            packages.Add(new ResolverPackage("Microsoft.Framework.Logging.Interfaces", NuGetVersion.Parse("1.0.0-beta4"), null, true, false));

            return packages;
        }

        private static List<ResolverPackage> CreateSystemGlobalization()
        {
            var packages = new List<ResolverPackage>();

            packages.Add(new ResolverPackage("System.Globalization", NuGetVersion.Parse("4.0.10-beta-22231"), null, true, false));
            packages.Add(new ResolverPackage("System.Globalization", NuGetVersion.Parse("4.0.10-beta-22416"), null, true, false));
            packages.Add(new ResolverPackage("System.Globalization", NuGetVersion.Parse("4.0.10-beta-22605"), null, true, false));
            packages.Add(new ResolverPackage("System.Globalization", NuGetVersion.Parse("4.0.10-beta-22816"), null, true, false));

            return packages;
        }

        private static List<ResolverPackage> CreateSystemXmlReaderWriter()
        {
            var packages = new List<ResolverPackage>();

            packages.Add(new ResolverPackage("System.Xml.ReaderWriter", NuGetVersion.Parse("4.0.10-beta-22231"),
                new NuGet.Packaging.Core.PackageDependency[]
                {
                    new NuGet.Packaging.Core.PackageDependency("System.IO", new VersionRange(NuGetVersion.Parse("4.0.0-beta-22231"))),
                    new NuGet.Packaging.Core.PackageDependency("System.Runtime", new VersionRange(NuGetVersion.Parse("4.0.0-beta-22231"))),
                    new NuGet.Packaging.Core.PackageDependency("System.Text.Encoding", new VersionRange(NuGetVersion.Parse("4.0.0-beta-22231"))),
                    new NuGet.Packaging.Core.PackageDependency("System.Threading.Tasks", new VersionRange(NuGetVersion.Parse("4.0.0-beta-22231"))),
                }
            , true, false));

            packages.Add(new ResolverPackage("System.Xml.ReaderWriter", NuGetVersion.Parse("4.0.10-beta-22416"),
                new NuGet.Packaging.Core.PackageDependency[]
                {
                    new NuGet.Packaging.Core.PackageDependency("System.IO", new VersionRange(NuGetVersion.Parse("4.0.10-beta-22416"))),
                    new NuGet.Packaging.Core.PackageDependency("System.Runtime", new VersionRange(NuGetVersion.Parse("4.0.10-beta-22416"))),
                    new NuGet.Packaging.Core.PackageDependency("System.Text.Encoding", new VersionRange(NuGetVersion.Parse("4.0.10-beta-22416"))),
                    new NuGet.Packaging.Core.PackageDependency("System.Threading.Tasks", new VersionRange(NuGetVersion.Parse("4.0.10-beta-22416"))),
                }
            , true, false));

            packages.Add(new ResolverPackage("System.Xml.ReaderWriter", NuGetVersion.Parse("4.0.10-beta-22605"),
                new NuGet.Packaging.Core.PackageDependency[]
                {
                    new NuGet.Packaging.Core.PackageDependency("System.IO", new VersionRange(NuGetVersion.Parse("4.0.10-beta-22605"))),
                    new NuGet.Packaging.Core.PackageDependency("System.Runtime", new VersionRange(NuGetVersion.Parse("4.0.10-beta-22605"))),
                    new NuGet.Packaging.Core.PackageDependency("System.Text.Encoding", new VersionRange(NuGetVersion.Parse("4.0.10-beta-22605"))),
                    new NuGet.Packaging.Core.PackageDependency("System.Threading.Tasks", new VersionRange(NuGetVersion.Parse("4.0.10-beta-22605"))),
                }
            , true, false));

            packages.Add(new ResolverPackage("System.Xml.ReaderWriter", NuGetVersion.Parse("4.0.10-beta-22816"),
                new NuGet.Packaging.Core.PackageDependency[]
                {
                    new NuGet.Packaging.Core.PackageDependency("System.IO", new VersionRange(NuGetVersion.Parse("4.0.10-beta-22816"))),
                    new NuGet.Packaging.Core.PackageDependency("System.Runtime", new VersionRange(NuGetVersion.Parse("4.0.10-beta-22816"))),
                    new NuGet.Packaging.Core.PackageDependency("System.Text.Encoding", new VersionRange(NuGetVersion.Parse("4.0.10-beta-22816"))),
                    new NuGet.Packaging.Core.PackageDependency("System.Threading.Tasks", new VersionRange(NuGetVersion.Parse("4.0.10-beta-22816"))),
                }
            , true, false));

            return packages;
        }

        private static List<ResolverPackage> CreateSystemTextEncoding()
        {
            var packages = new List<ResolverPackage>();

            packages.Add(new ResolverPackage("System.Text.Encoding", NuGetVersion.Parse("4.0.10-beta-22231"),
                new NuGet.Packaging.Core.PackageDependency[]
                {
                    new NuGet.Packaging.Core.PackageDependency("System.Runtime", new VersionRange(NuGetVersion.Parse("4.0.10-beta-22231"))),
                }
            , true, false));

            packages.Add(new ResolverPackage("System.Text.Encoding", NuGetVersion.Parse("4.0.10-beta-22416"),
                new NuGet.Packaging.Core.PackageDependency[]
                {
                    new NuGet.Packaging.Core.PackageDependency("System.Runtime", new VersionRange(NuGetVersion.Parse("4.0.10-beta-22416"))),
                }
            , true, false));

            packages.Add(new ResolverPackage("System.Text.Encoding", NuGetVersion.Parse("4.0.10-beta-22605"),
                new NuGet.Packaging.Core.PackageDependency[]
                {
                    new NuGet.Packaging.Core.PackageDependency("System.Runtime", new VersionRange(NuGetVersion.Parse("4.0.10-beta-22605"))),
                }
            , true, false));

            packages.Add(new ResolverPackage("System.Text.Encoding", NuGetVersion.Parse("4.0.10-beta-22816"),
                new NuGet.Packaging.Core.PackageDependency[]
                {
                    new NuGet.Packaging.Core.PackageDependency("System.Runtime", new VersionRange(NuGetVersion.Parse("4.0.10-beta-22816"))),
                }
            , true, false));

            return packages;
        }

        private static List<ResolverPackage> CreateValidation()
        {
            var packages = new List<ResolverPackage>();

            packages.Add(new ResolverPackage("Validation", NuGetVersion.Parse("1.0.0.12259"), null, true, false));
            packages.Add(new ResolverPackage("Validation", NuGetVersion.Parse("2.0.0.12319"), null, true, false));
            packages.Add(new ResolverPackage("Validation", NuGetVersion.Parse("2.0.1.12362"), null, true, false));
            packages.Add(new ResolverPackage("Validation", NuGetVersion.Parse("2.0.2.13022"), null, true, false));
            packages.Add(new ResolverPackage("Validation", NuGetVersion.Parse("2.0.3.13323"), null, true, false));
            packages.Add(new ResolverPackage("Validation", NuGetVersion.Parse("2.0.4.14103"), null, true, false));
            packages.Add(new ResolverPackage("Validation", NuGetVersion.Parse("2.0.5.14286"), null, true, false));
            packages.Add(new ResolverPackage("Validation", NuGetVersion.Parse("2.0.6.15003"), null, true, false));

            return packages;
        }


        private static List<ResolverPackage> CreateMicrosoftFrameworkOptionsModel()
        {
            var packages = new List<ResolverPackage>();

            packages.Add(new ResolverPackage("Microsoft.Framework.OptionsModel", NuGetVersion.Parse("1.0.0-beta1"),
                new NuGet.Packaging.Core.PackageDependency[]
                {
                    new NuGet.Packaging.Core.PackageDependency("Microsoft.Framework.ConfigurationModel", new VersionRange(NuGetVersion.Parse("1.0.0-beta1"))),
                    new NuGet.Packaging.Core.PackageDependency("Microsoft.Framework.DependencyInjection", new VersionRange(NuGetVersion.Parse("1.0.0-beta1"))),
                }
                , true, false));

            packages.Add(new ResolverPackage("Microsoft.Framework.OptionsModel", NuGetVersion.Parse("1.0.0-beta2"),
                new NuGet.Packaging.Core.PackageDependency[]
                {
                    new NuGet.Packaging.Core.PackageDependency("Microsoft.Framework.ConfigurationModel", new VersionRange(NuGetVersion.Parse("1.0.0-beta2"))),
                    new NuGet.Packaging.Core.PackageDependency("Microsoft.Framework.DependencyInjection", new VersionRange(NuGetVersion.Parse("1.0.0-beta2"))),
                }
                , true, false));

            packages.Add(new ResolverPackage("Microsoft.Framework.OptionsModel", NuGetVersion.Parse("1.0.0-beta3"),
                new NuGet.Packaging.Core.PackageDependency[]
                {
                    new NuGet.Packaging.Core.PackageDependency("Microsoft.Framework.ConfigurationModel", new VersionRange(NuGetVersion.Parse("1.0.0-beta3"))),
                    new NuGet.Packaging.Core.PackageDependency("Microsoft.Framework.DependencyInjection", new VersionRange(NuGetVersion.Parse("1.0.0-beta3"))),
                }
                , true, false));

            packages.Add(new ResolverPackage("Microsoft.Framework.OptionsModel", NuGetVersion.Parse("1.0.0-beta4"),
                new NuGet.Packaging.Core.PackageDependency[]
                {
                    new NuGet.Packaging.Core.PackageDependency("Microsoft.Framework.ConfigurationModel", new VersionRange(NuGetVersion.Parse("1.0.0-beta4"))),
                    new NuGet.Packaging.Core.PackageDependency("Microsoft.Framework.DependencyInjection.Interfaces", new VersionRange(NuGetVersion.Parse("1.0.0-beta4"))),
                }
                , true, false));

            return packages;
        }

        private static List<ResolverPackage> CreateMicrosoftCodeAnalysisAnalyzers()
        {
            var packages = new List<ResolverPackage>();

            packages.Add(new ResolverPackage("Microsoft.CodeAnalysis.Analyzers", NuGetVersion.Parse("1.0.0-rc2"), null, true, false));

            return packages;
        }

        private static List<ResolverPackage> CreateMicrosoftCodeAnalysisCSharp()
        {
            var packages = new List<ResolverPackage>();

            packages.Add(new ResolverPackage("Microsoft.CodeAnalysis.CSharp", NuGetVersion.Parse("0.6.4033103-beta"),
                new NuGet.Packaging.Core.PackageDependency[]
                {
                    new NuGet.Packaging.Core.PackageDependency("Microsoft.CodeAnalysis.Common", new VersionRange(NuGetVersion.Parse("0.6.4033103-beta"))),
                }
                , true, false));

            packages.Add(new ResolverPackage("Microsoft.CodeAnalysis.CSharp", NuGetVersion.Parse("0.7.4052301-beta"),
                new NuGet.Packaging.Core.PackageDependency[]
                {
                    new NuGet.Packaging.Core.PackageDependency("Microsoft.CodeAnalysis.Common", new VersionRange(NuGetVersion.Parse("0.7.4052301-beta"))),
                }
                , true, false));

            packages.Add(new ResolverPackage("Microsoft.CodeAnalysis.CSharp", NuGetVersion.Parse("0.7.4080704-beta"),
                new NuGet.Packaging.Core.PackageDependency[]
                {
                    new NuGet.Packaging.Core.PackageDependency("Microsoft.CodeAnalysis.Common", new VersionRange(NuGetVersion.Parse("0.7.4080704-beta"), true, NuGetVersion.Parse("0.7.4080704-beta"), true)),
                }
                , true, false));

            packages.Add(new ResolverPackage("Microsoft.CodeAnalysis.CSharp", NuGetVersion.Parse("0.7.4091001-beta"),
                new NuGet.Packaging.Core.PackageDependency[]
                {
                    new NuGet.Packaging.Core.PackageDependency("Microsoft.CodeAnalysis.Common", new VersionRange(NuGetVersion.Parse("0.7.4091001-beta"), true, NuGetVersion.Parse("0.7.4091001-beta"), true)),
                }
                , true, false));

            packages.Add(new ResolverPackage("Microsoft.CodeAnalysis.CSharp", NuGetVersion.Parse("0.7.4091001-CTP4"),
                new NuGet.Packaging.Core.PackageDependency[]
                {
                    new NuGet.Packaging.Core.PackageDependency("Microsoft.CodeAnalysis.Common", new VersionRange(NuGetVersion.Parse("0.7.4091001-beta"), true, NuGetVersion.Parse("0.7.4091001-beta"), true)),
                }
                , true, false));

            packages.Add(new ResolverPackage("Microsoft.CodeAnalysis.CSharp", NuGetVersion.Parse("1.0.0-beta2"),
                new NuGet.Packaging.Core.PackageDependency[]
                {
                    new NuGet.Packaging.Core.PackageDependency("Microsoft.CodeAnalysis.Common", new VersionRange(NuGetVersion.Parse("1.0.0-beta2"), true, NuGetVersion.Parse("1.0.0-beta2"), true)),
                }
                , true, false));

            packages.Add(new ResolverPackage("Microsoft.CodeAnalysis.CSharp", NuGetVersion.Parse("1.0.0-beta1-20141031-01"),
                new NuGet.Packaging.Core.PackageDependency[]
                {
                    new NuGet.Packaging.Core.PackageDependency("Microsoft.CodeAnalysis.Common", new VersionRange(NuGetVersion.Parse("1.0.0-beta1-20141031-01"), true, NuGetVersion.Parse("1.0.0-beta1-20141031-01"), true)),
                }
                , true, false));

            packages.Add(new ResolverPackage("Microsoft.CodeAnalysis.CSharp", NuGetVersion.Parse("1.0.0-rc1"),
                new NuGet.Packaging.Core.PackageDependency[]
                {
                    new NuGet.Packaging.Core.PackageDependency("Microsoft.CodeAnalysis.Common", new VersionRange(NuGetVersion.Parse("1.0.0-rc1"), true, NuGetVersion.Parse("1.0.0-rc1"), true)),
                }
                , true, false));

            packages.Add(new ResolverPackage("Microsoft.CodeAnalysis.CSharp", NuGetVersion.Parse("1.0.0-rc2"),
                new NuGet.Packaging.Core.PackageDependency[]
                {
                    new NuGet.Packaging.Core.PackageDependency("Microsoft.CodeAnalysis.Common", new VersionRange(NuGetVersion.Parse("1.0.0-rc2"), true, NuGetVersion.Parse("1.0.0-rc2"), true)),
                }
                , true, false));

            return packages;
        }

        private static List<ResolverPackage> CreateMicrosoftCodeAnalysisCommon()
        {
            var packages = new List<ResolverPackage>();

            packages.Add(new ResolverPackage("Microsoft.CodeAnalysis.Common", NuGetVersion.Parse("0.6.4033103-beta"),
                new NuGet.Packaging.Core.PackageDependency[]
                {
                    new NuGet.Packaging.Core.PackageDependency("System.Collections.Immutable", new VersionRange(NuGetVersion.Parse("1.1.33-beta"))),
                    new NuGet.Packaging.Core.PackageDependency("System.Reflection.Metadata", new VersionRange(NuGetVersion.Parse("1.0.18-beta"))),
                    new NuGet.Packaging.Core.PackageDependency("Microsoft.CodeAnalysis.Analyzers", new VersionRange(NuGetVersion.Parse("1.0.0-rc2"))),
                }
                , true, false));

            packages.Add(new ResolverPackage("Microsoft.CodeAnalysis.Common", NuGetVersion.Parse("0.7.4052301-beta"),
                new NuGet.Packaging.Core.PackageDependency[]
                {
                    new NuGet.Packaging.Core.PackageDependency("System.Collections.Immutable", new VersionRange(NuGetVersion.Parse("1.1.33-beta"))),
                    new NuGet.Packaging.Core.PackageDependency("System.Reflection.Metadata", new VersionRange(NuGetVersion.Parse("1.0.18-beta"))),
                    new NuGet.Packaging.Core.PackageDependency("Microsoft.CodeAnalysis.Analyzers", new VersionRange(NuGetVersion.Parse("1.0.0-rc2"))),
                }
                , true, false));

            packages.Add(new ResolverPackage("Microsoft.CodeAnalysis.Common", NuGetVersion.Parse("0.7.4080704-beta"),
                new NuGet.Packaging.Core.PackageDependency[]
                {
                    new NuGet.Packaging.Core.PackageDependency("System.Collections.Immutable", new VersionRange(NuGetVersion.Parse("1.1.33-beta"))),
                    new NuGet.Packaging.Core.PackageDependency("System.Reflection.Metadata", new VersionRange(NuGetVersion.Parse("1.0.18-beta"))),
                    new NuGet.Packaging.Core.PackageDependency("Microsoft.CodeAnalysis.Analyzers", new VersionRange(NuGetVersion.Parse("1.0.0-rc2"))),
                }
                , true, false));

            packages.Add(new ResolverPackage("Microsoft.CodeAnalysis.Common", NuGetVersion.Parse("0.7.4091001-beta"),
                new NuGet.Packaging.Core.PackageDependency[]
                {
                    new NuGet.Packaging.Core.PackageDependency("System.Collections.Immutable", new VersionRange(NuGetVersion.Parse("1.1.33-beta"))),
                    new NuGet.Packaging.Core.PackageDependency("System.Reflection.Metadata", new VersionRange(NuGetVersion.Parse("1.0.18-beta"))),
                    new NuGet.Packaging.Core.PackageDependency("Microsoft.CodeAnalysis.Analyzers", new VersionRange(NuGetVersion.Parse("1.0.0-rc2"))),
                }
                , true, false));

            packages.Add(new ResolverPackage("Microsoft.CodeAnalysis.Common", NuGetVersion.Parse("0.7.4091001-CTP4"),
                new NuGet.Packaging.Core.PackageDependency[]
                {
                    new NuGet.Packaging.Core.PackageDependency("System.Collections.Immutable", new VersionRange(NuGetVersion.Parse("1.1.33-beta"))),
                    new NuGet.Packaging.Core.PackageDependency("System.Reflection.Metadata", new VersionRange(NuGetVersion.Parse("1.0.18-beta"))),
                    new NuGet.Packaging.Core.PackageDependency("Microsoft.CodeAnalysis.Analyzers", new VersionRange(NuGetVersion.Parse("1.0.0-rc2"))),
                }
                , true, false));

            packages.Add(new ResolverPackage("Microsoft.CodeAnalysis.Common", NuGetVersion.Parse("1.0.0-beta2"),
                new NuGet.Packaging.Core.PackageDependency[]
                {
                    new NuGet.Packaging.Core.PackageDependency("System.Collections.Immutable", new VersionRange(NuGetVersion.Parse("1.1.33-beta"))),
                    new NuGet.Packaging.Core.PackageDependency("System.Reflection.Metadata", new VersionRange(NuGetVersion.Parse("1.0.18-beta"))),
                    new NuGet.Packaging.Core.PackageDependency("Microsoft.CodeAnalysis.Analyzers", new VersionRange(NuGetVersion.Parse("1.0.0-rc2"))),
                }
                , true, false));

            packages.Add(new ResolverPackage("Microsoft.CodeAnalysis.Common", NuGetVersion.Parse("1.0.0-beta1-20141031-01"),
                new NuGet.Packaging.Core.PackageDependency[]
                {
                    new NuGet.Packaging.Core.PackageDependency("System.Collections.Immutable", new VersionRange(NuGetVersion.Parse("1.1.33-beta"))),
                    new NuGet.Packaging.Core.PackageDependency("System.Reflection.Metadata", new VersionRange(NuGetVersion.Parse("1.0.18-beta"))),
                    new NuGet.Packaging.Core.PackageDependency("Microsoft.CodeAnalysis.Analyzers", new VersionRange(NuGetVersion.Parse("1.0.0-rc2"))),
                }
                , true, false));

            packages.Add(new ResolverPackage("Microsoft.CodeAnalysis.Common", NuGetVersion.Parse("1.0.0-rc1"),
                new NuGet.Packaging.Core.PackageDependency[]
                {
                    new NuGet.Packaging.Core.PackageDependency("System.Collections.Immutable", new VersionRange(NuGetVersion.Parse("1.1.33-beta"))),
                    new NuGet.Packaging.Core.PackageDependency("System.Reflection.Metadata", new VersionRange(NuGetVersion.Parse("1.0.18-beta"))),
                    new NuGet.Packaging.Core.PackageDependency("Microsoft.CodeAnalysis.Analyzers", new VersionRange(NuGetVersion.Parse("1.0.0-rc2"))),
                }
                , true, false));

            packages.Add(new ResolverPackage("Microsoft.CodeAnalysis.Common", NuGetVersion.Parse("1.0.0-rc2"),
                new NuGet.Packaging.Core.PackageDependency[]
                {
                    new NuGet.Packaging.Core.PackageDependency("System.Collections.Immutable", new VersionRange(NuGetVersion.Parse("1.1.33-beta"))),
                    new NuGet.Packaging.Core.PackageDependency("System.Reflection.Metadata", new VersionRange(NuGetVersion.Parse("1.0.18-beta"))),
                    new NuGet.Packaging.Core.PackageDependency("Microsoft.CodeAnalysis.Analyzers", new VersionRange(NuGetVersion.Parse("1.0.0-rc2"))),
                }
                , true, false));

            return packages;
        }

        private static List<ResolverPackage> CreateSystemCollectionsImmutable()
        {
            var packages = new List<ResolverPackage>();

            packages.Add(new ResolverPackage("System.Collections.Immutable", NuGetVersion.Parse("1.1.32-beta"), null, true, false));
            packages.Add(new ResolverPackage("System.Collections.Immutable", NuGetVersion.Parse("1.1.33-beta"), null, true, false));
            packages.Add(new ResolverPackage("System.Collections.Immutable", NuGetVersion.Parse("1.1.34-rc"), null, true, false));
            packages.Add(new ResolverPackage("System.Collections.Immutable", NuGetVersion.Parse("1.1.36"), null, true, false));

            return packages;
        }

        private static List<ResolverPackage> CreateSystemReflectionMetadata()
        {
            var packages = new List<ResolverPackage>();

            packages.Add(new ResolverPackage("System.Reflection.Metadata", NuGetVersion.Parse("1.0.17-beta"),
                new NuGet.Packaging.Core.PackageDependency[]
                {
                    new NuGet.Packaging.Core.PackageDependency("System.Collections.Immutable", new VersionRange(NuGetVersion.Parse("1.1.32-beta"))),
                }
                , true, false));

            packages.Add(new ResolverPackage("System.Reflection.Metadata", NuGetVersion.Parse("1.0.18-beta"),
                new NuGet.Packaging.Core.PackageDependency[]
                {
                    new NuGet.Packaging.Core.PackageDependency("System.Collections.Immutable", new VersionRange(NuGetVersion.Parse("1.1.33-beta"))),
                }
                , true, false));

            packages.Add(new ResolverPackage("System.Reflection.Metadata", NuGetVersion.Parse("1.0.19-rc"),
                new NuGet.Packaging.Core.PackageDependency[]
                {
                    new NuGet.Packaging.Core.PackageDependency("System.Collections.Immutable", new VersionRange(NuGetVersion.Parse("1.1.34-rc"))),
                }
                , true, false));

            packages.Add(new ResolverPackage("System.Reflection.Metadata", NuGetVersion.Parse("1.0.21"),
                new NuGet.Packaging.Core.PackageDependency[]
                {
                    new NuGet.Packaging.Core.PackageDependency("System.Collections.Immutable", new VersionRange(NuGetVersion.Parse("1.1.36"))),
                }
                , true, false));

            return packages;
        }

        private static List<ResolverPackage> CreateSqlServerCompact()
        {
            var packages = new List<ResolverPackage>();

            packages.Add(new ResolverPackage("SqlServerCompact", NuGetVersion.Parse("4.0.8482.1"), null, true, false));
            packages.Add(new ResolverPackage("SqlServerCompact", NuGetVersion.Parse("4.0.8852.1"), null, true, false));

            packages.Add(new ResolverPackage("SqlServerCompact", NuGetVersion.Parse("4.0.8854.1"),
                new NuGet.Packaging.Core.PackageDependency[]
                {
                    new NuGet.Packaging.Core.PackageDependency("Microsoft.SqlServer.Compact", new VersionRange(NuGetVersion.Parse("4.0.8854.1"))),
                }
                , true, false));

            return packages;
        }

        private static List<ResolverPackage> CreateMicrosoftSqlServerCompact()
        {
            var packages = new List<ResolverPackage>();

            packages.Add(new ResolverPackage("Microsoft.SqlServer.Compact", NuGetVersion.Parse("4.0.8852.1"), null, true, false));
            packages.Add(new ResolverPackage("Microsoft.SqlServer.Compact", NuGetVersion.Parse("4.0.8854.1"), null, true, false));
            packages.Add(new ResolverPackage("Microsoft.SqlServer.Compact", NuGetVersion.Parse("4.0.8854.2"), null, true, false));
            packages.Add(new ResolverPackage("Microsoft.SqlServer.Compact", NuGetVersion.Parse("4.0.8876.1"), null, true, false));

            return packages;
        }

        private static List<ResolverPackage> CreateRemotionLinq()
        {
            var packages = new List<ResolverPackage>();

            packages.Add(new ResolverPackage("Remotion.Linq", NuGetVersion.Parse("1.13.111"), null, true, false));
            packages.Add(new ResolverPackage("Remotion.Linq", NuGetVersion.Parse("1.13.161"), null, true, false));
            packages.Add(new ResolverPackage("Remotion.Linq", NuGetVersion.Parse("1.13.164"), null, true, false));
            packages.Add(new ResolverPackage("Remotion.Linq", NuGetVersion.Parse("1.13.170"), null, true, false));
            packages.Add(new ResolverPackage("Remotion.Linq", NuGetVersion.Parse("1.13.171"), null, true, false));
            packages.Add(new ResolverPackage("Remotion.Linq", NuGetVersion.Parse("1.13.176"), null, true, false));
            packages.Add(new ResolverPackage("Remotion.Linq", NuGetVersion.Parse("1.13.177"), null, true, false));
            packages.Add(new ResolverPackage("Remotion.Linq", NuGetVersion.Parse("1.13.178"), null, true, false));
            packages.Add(new ResolverPackage("Remotion.Linq", NuGetVersion.Parse("1.13.179"), null, true, false));
            packages.Add(new ResolverPackage("Remotion.Linq", NuGetVersion.Parse("1.13.180"), null, true, false));
            packages.Add(new ResolverPackage("Remotion.Linq", NuGetVersion.Parse("1.13.181"), null, true, false));
            packages.Add(new ResolverPackage("Remotion.Linq", NuGetVersion.Parse("1.13.182"), null, true, false));
            packages.Add(new ResolverPackage("Remotion.Linq", NuGetVersion.Parse("1.13.183"), null, true, false));
            packages.Add(new ResolverPackage("Remotion.Linq", NuGetVersion.Parse("1.15.11"), null, true, false));
            packages.Add(new ResolverPackage("Remotion.Linq", NuGetVersion.Parse("1.15.12"), null, true, false));
            packages.Add(new ResolverPackage("Remotion.Linq", NuGetVersion.Parse("1.15.13"), null, true, false));
            packages.Add(new ResolverPackage("Remotion.Linq", NuGetVersion.Parse("1.15.15"), null, true, false));
            packages.Add(new ResolverPackage("Remotion.Linq", NuGetVersion.Parse("1.15.2"), null, true, false));
            packages.Add(new ResolverPackage("Remotion.Linq", NuGetVersion.Parse("1.15.7"), null, true, false));
            packages.Add(new ResolverPackage("Remotion.Linq", NuGetVersion.Parse("1.15.9"), null, true, false));
            packages.Add(new ResolverPackage("Remotion.Linq", NuGetVersion.Parse("2.0.0-alpha-001"), null, true, false));
            packages.Add(new ResolverPackage("Remotion.Linq", NuGetVersion.Parse("2.0.0-alpha-002"), null, true, false));
            packages.Add(new ResolverPackage("Remotion.Linq", NuGetVersion.Parse("2.0.0-alpha-003"), null, true, false));
            packages.Add(new ResolverPackage("Remotion.Linq", NuGetVersion.Parse("2.0.0-alpha-004"), null, true, false));

            return packages;
        }

        private static List<ResolverPackage> CreateIxAsync()
        {
            var packages = new List<ResolverPackage>();

            packages.Add(new ResolverPackage("Ix-Async", NuGetVersion.Parse("0.9.0"), null, true, false));
            packages.Add(new ResolverPackage("Ix-Async", NuGetVersion.Parse("0.9.0.1"), null, true, false));
            packages.Add(new ResolverPackage("Ix-Async", NuGetVersion.Parse("0.9.0.2"), null, true, false));
            packages.Add(new ResolverPackage("Ix-Async", NuGetVersion.Parse("1.2.0"), null, true, false));
            packages.Add(new ResolverPackage("Ix-Async", NuGetVersion.Parse("1.2.0-beta"), null, true, false));
            packages.Add(new ResolverPackage("Ix-Async", NuGetVersion.Parse("1.2.1-beta"), null, true, false));
            packages.Add(new ResolverPackage("Ix-Async", NuGetVersion.Parse("1.2.2"), null, true, false));
            packages.Add(new ResolverPackage("Ix-Async", NuGetVersion.Parse("1.2.3"), null, true, false));
            packages.Add(new ResolverPackage("Ix-Async", NuGetVersion.Parse("1.2.3-beta"), null, true, false));

            return packages;
        }

        private static List<ResolverPackage> CreateSystemDataCommon()
        {
            var packages = new List<ResolverPackage>();

            packages.Add(new ResolverPackage("System.Data.Common", NuGetVersion.Parse("1.0.0-beta1"), null, true, false));
            packages.Add(new ResolverPackage("System.Data.Common", NuGetVersion.Parse("1.0.0-beta2"), null, true, false));

            packages.Add(new ResolverPackage("System.Data.Common", NuGetVersion.Parse("4.0.0-beta-22605"),
                new NuGet.Packaging.Core.PackageDependency[]
                {
                    new NuGet.Packaging.Core.PackageDependency("System.IO", new VersionRange(NuGetVersion.Parse("4.0.0-beta-22605"))),
                    new NuGet.Packaging.Core.PackageDependency("System.Runtime", new VersionRange(NuGetVersion.Parse("4.0.0-beta-22605"))),
                    new NuGet.Packaging.Core.PackageDependency("System.Threading.Tasks", new VersionRange(NuGetVersion.Parse("4.0.0-beta-22605"))),
                }
            , true, false));

            packages.Add(new ResolverPackage("System.Data.Common", NuGetVersion.Parse("4.0.0-beta-22816"),
                new NuGet.Packaging.Core.PackageDependency[]
                {
                    new NuGet.Packaging.Core.PackageDependency("System.IO", new VersionRange(NuGetVersion.Parse("4.0.10-beta-22816"))),
                    new NuGet.Packaging.Core.PackageDependency("System.Runtime", new VersionRange(NuGetVersion.Parse("4.0.10-beta-22816"))),
                    new NuGet.Packaging.Core.PackageDependency("System.Threading.Tasks", new VersionRange(NuGetVersion.Parse("4.0.10-beta-22816"))),
                }
            , true, false));

            return packages;
        }

        private static List<ResolverPackage> CreateSystemDataSqlClient()
        {
            var packages = new List<ResolverPackage>();

            packages.Add(new ResolverPackage("System.Data.SqlClient", NuGetVersion.Parse("1.0.0-beta1"), null, true, false));
            packages.Add(new ResolverPackage("System.Data.SqlClient", NuGetVersion.Parse("1.0.0-beta1"), null, true, false));

            //TODO add dependencies
            packages.Add(new ResolverPackage("System.Data.SqlClient", NuGetVersion.Parse("1.0.0-beta1"), null, true, false));
            packages.Add(new ResolverPackage("System.Data.SqlClient", NuGetVersion.Parse("1.0.0-beta1"), null, true, false));

            return packages;
        }

        private static List<ResolverPackage> CreateMicrosoftBclImmutable()
        {
            var packages = new List<ResolverPackage>();

            packages.Add(new ResolverPackage("Microsoft.Bcl.Immutable", NuGetVersion.Parse("1.0.12-beta"), null, true, false));
            packages.Add(new ResolverPackage("Microsoft.Bcl.Immutable", NuGetVersion.Parse("1.0.12-beta"), null, true, false));
            packages.Add(new ResolverPackage("Microsoft.Bcl.Immutable", NuGetVersion.Parse("1.0.12-beta"), null, true, false));
            packages.Add(new ResolverPackage("Microsoft.Bcl.Immutable", NuGetVersion.Parse("1.0.12-beta"), null, true, false));
            packages.Add(new ResolverPackage("Microsoft.Bcl.Immutable", NuGetVersion.Parse("1.0.12-beta"), null, true, false));
            packages.Add(new ResolverPackage("Microsoft.Bcl.Immutable", NuGetVersion.Parse("1.0.12-beta"), null, true, false));
            packages.Add(new ResolverPackage("Microsoft.Bcl.Immutable", NuGetVersion.Parse("1.0.12-beta"), null, true, false));
            packages.Add(new ResolverPackage("Microsoft.Bcl.Immutable", NuGetVersion.Parse("1.0.12-beta"), null, true, false));
            packages.Add(new ResolverPackage("Microsoft.Bcl.Immutable", NuGetVersion.Parse("1.0.12-beta"), null, true, false));
            packages.Add(new ResolverPackage("Microsoft.Bcl.Immutable", NuGetVersion.Parse("1.0.12-beta"), null, true, false));

            return packages;
        }

        private static List<ResolverPackage> CreateMicrosoftBclMetadata()
        {
            var packages = new List<ResolverPackage>();

            packages.Add(new ResolverPackage("Microsoft.Bcl.Metadata", NuGetVersion.Parse("1.0.11-alpha"), null, true, false));
            packages.Add(new ResolverPackage("Microsoft.Bcl.Metadata", NuGetVersion.Parse("1.0.11-alpha"), null, true, false));
            packages.Add(new ResolverPackage("Microsoft.Bcl.Metadata", NuGetVersion.Parse("1.0.9-alpha"), null, true, false));

            return packages;
        }

        private static List<ResolverPackage> CreateSystemThreadingTasksUnofficial()
        {
            var packages = new List<ResolverPackage>();

            packages.Add(new ResolverPackage("System.Threading.Tasks.Unofficial", NuGetVersion.Parse("3.1.0"), null, true, false));

            return packages;
        }



        public static List<ResolverPackage> CreateEntityFrameworkPackageGraph()
        {
            List<ResolverPackage> available = new List<ResolverPackage>();
            available.AddRange(CreateEntityFramework());
            available.AddRange(CreateEntityFrameworkCore());
            available.AddRange(CreateEntityFrameworkCommands());
            available.AddRange(CreateEntityFrameworkMigrations());
            available.AddRange(CreateMicrosoftFrameworkDependencyInjection());
            available.AddRange(CreateEntityFrameworkRelational());
            available.AddRange(CreateEntityFrameworkRelationalDesign());
            available.AddRange(CreateEntityFrameworkSqlServerDesign());
            available.AddRange(CreateMicrosoftCodeAnalysisCSharp());
            available.AddRange(CreateSqlServerCompact());
            available.AddRange(CreateMicrosoftFrameworkConfigurationModel());
            available.AddRange(CreateMicrosoftFrameworkDependencyInjectionInterfaces());
            available.AddRange(CreateSystemDataCommon());
            available.AddRange(CreateEntityFrameworkCore());
            available.AddRange(CreateEntityFrameworkSqlServer());
            available.AddRange(CreateMicrosoftCodeAnalysisCommon());
            available.AddRange(CreateMicrosoftSqlServerCompact());
            available.AddRange(CreateMicrosoftFrameworkConfigurationModelInterfaces());
            available.AddRange(CreateSystemIO());
            available.AddRange(CreateSystemRuntime());
            available.AddRange(CreateSystemThreadingTasks());
            available.AddRange(CreateIxAsync());
            available.AddRange(CreateMicrosoftFrameworkLogging());
            available.AddRange(CreateMicrosoftFrameworkOptionsModel());
            available.AddRange(CreateRemotionLinq());
            available.AddRange(CreateSystemCollectionsImmutable());
            available.AddRange(CreateMicrosoftFrameworkCachingMemory());
            available.AddRange(CreateSystemDataSqlClient());
            available.AddRange(CreateMicrosoftBclImmutable());
            available.AddRange(CreateMicrosoftBclMetadata());
            available.AddRange(CreateSystemReflectionMetadata());
            available.AddRange(CreateMicrosoftCodeAnalysisAnalyzers());
            available.AddRange(CreateSystemTextEncoding());
            available.AddRange(CreateSystemThreadingTasksUnofficial());
            available.AddRange(CreateMicrosoftFrameworkLoggingInterfaces());
            available.AddRange(CreateMicrosoftFrameworkCachingInterfaces());
            available.AddRange(CreateSystemGlobalization());
            available.AddRange(CreateSystemXmlReaderWriter());
            available.AddRange(CreateValidation());

            return available;
        }
    }
}
