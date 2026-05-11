// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Microsoft.DotNet.Build.Tasks.Workloads.Tests
{
    internal class TestFixture : IDisposable
    {
        bool _retainOutput;

        /// <summary>
        /// File system path of the root directory for the test. 
        /// </summary>
        public string OutputPath
        {
            get;
            init;
        }

        public string MsiPath
        {
            get;
            init;
        }

        public string PackagePath
        {
            get;
            init;
        }

        public TestFixture(bool retainOutput = false)
        {
            OutputPath = Path.Combine(AppContext.BaseDirectory,
               "TEST_OUTPUT", Path.GetFileNameWithoutExtension(Path.GetRandomFileName()));
            Directory.CreateDirectory(OutputPath);

            MsiPath = Path.Combine(OutputPath, "msi");

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
