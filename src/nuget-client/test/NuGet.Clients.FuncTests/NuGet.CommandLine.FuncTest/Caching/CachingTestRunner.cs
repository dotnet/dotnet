// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NuGet.Test.Utility;
using Test.Utility;

namespace NuGet.CommandLine.Test.Caching
{
    public static class CachingTestRunner
    {
        public static async Task<IEnumerable<CachingValidations>> ExecuteAsync(ICachingTest test, ICachingCommand command, INuGetExe nuGetExe, CachingType caching, ServerType server)
        {
            using (var testFolder = TestDirectory.Create())
            using (var mockServer = new MockServer())
            {
                var tc = new CachingTestContext(testFolder, mockServer, nuGetExe);

                // Enable this flag to launch the debugger when the nuget.exe process starts. This also increases
                // logging verbosity and command timeout.
                //
                // tc.Debug = true;

                tc.NoCache = caching.HasFlag(CachingType.NoCache);
                tc.DirectDownload = caching.HasFlag(CachingType.DirectDownload);
                tc.CurrentSource = server == ServerType.V2 ? tc.V2Source : tc.V3Source;

                tc.ClearHttpCache();
                var validations = new List<CachingValidations>();
                for (var i = 0; i < test.IterationCount; i++)
                {
                    var args = await test.PrepareTestAsync(tc, command);
                    var result = tc.Execute(args);
                    validations.Add(test.Validate(tc, command, result));
                }

                return validations;

            }
        }

        public static async Task<IEnumerable<CachingValidations>> ExecuteAsync(Type testType, Type commandType, INuGetExe nuGetExe, CachingType caching, ServerType server)
        {
            var test = (ICachingTest)Activator.CreateInstance(testType);
            var command = (ICachingCommand)Activator.CreateInstance(commandType);

            return await ExecuteAsync(
                test,
                command,
                nuGetExe,
                caching,
                server);
        }
    }
}
