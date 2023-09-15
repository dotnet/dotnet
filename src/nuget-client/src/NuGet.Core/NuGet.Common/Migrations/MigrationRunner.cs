// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Threading;

namespace NuGet.Common.Migrations
{
    public static class MigrationRunner
    {
        private const string MaxMigrationFilename = "1";

        public static void Run()
        {
            string migrationsDirectory = GetMigrationsDirectory();

            Run(migrationsDirectory);
        }

        internal static void Run(string migrationsDirectory)
        {
            var expectedMigrationFilename = Path.Combine(migrationsDirectory, MaxMigrationFilename);

            if (!File.Exists(expectedMigrationFilename))
            {
                // Multiple processes or threads might be trying to call this concurrently (especially via NuGetSdkResolver)
                // so use a global mutex and then check if someone else already did the work.
                using (var mutex = new Mutex(false, "NuGet-Migrations"))
                {
                    if (WaitForMutex(mutex))
                    {
                        try
                        {
                            Directory.CreateDirectory(migrationsDirectory);

                            // Only run migrations that have not already been run
                            if (!File.Exists(expectedMigrationFilename))
                            {
                                Migration1.Run();
                                // Create file for the migration run, so that if an older version of NuGet is run, it doesn't try to run migrations again.
                                File.WriteAllText(expectedMigrationFilename, string.Empty);
                            }
                        }
                        catch { }
                        finally
                        {
                            mutex.ReleaseMutex();
                        }
                    }
                }
            }

            static bool WaitForMutex(Mutex mutex)
            {
                bool captured;

                try
                {
                    captured = mutex.WaitOne(TimeSpan.FromMinutes(1), false);
                }
                catch (AbandonedMutexException)
                {
                    captured = true;
                }

                return captured;
            }
        }

        internal static string GetMigrationsDirectory()
        {
            if (RuntimeEnvironmentHelper.IsWindows)
            {
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "NuGet", "Migrations");
            }

            var XdgDataHome = Environment.GetEnvironmentVariable("XDG_DATA_HOME");

            return string.IsNullOrEmpty(XdgDataHome)
                ? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".local", "share", "NuGet", "Migrations")
                : Path.Combine(XdgDataHome, "NuGet", "Migrations");
        }
    }
}
