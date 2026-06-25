// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;

namespace Microsoft.DotNet.Build.Tasks.Workloads.Tests
{
    internal class MsiTestFixture : IDisposable
    {
        bool _retainOutput;

        /// <summary>
        /// File system path of the root directory where test output is generated.
        /// </summary>
        public string OutputPath
        {
            get;
            init;
        }

        /// <summary>
        /// Files system path where MSIs are generated.
        /// </summary>
        public string MsiPath
        {
            get;
            init;
        }

        /// <summary>
        /// File system path where NuGet packages used by the test will be extracted
        /// </summary>
        public string PackagePath
        {
            get;
            init;
        }

        public MsiTestFixture(bool retainOutput = false)
        {
            OutputPath = Path.Combine(AppContext.BaseDirectory,
               "TEST_OUTPUT", Path.GetFileNameWithoutExtension(Path.GetRandomFileName()));
            Directory.CreateDirectory(OutputPath);

            MsiPath = Path.Combine(OutputPath, "msi");
            PackagePath = Path.Combine(OutputPath, "pkg");

            _retainOutput = retainOutput;
        }

        public void Dispose()
        {
            if (!_retainOutput)
            {
                try
                {
                    if (Directory.Exists(OutputPath))
                    {
                        // Best effort to clean up output.
                        Directory.Delete(OutputPath, recursive: true);
                    }
                }
                catch { }
            }
        }
    }
}
