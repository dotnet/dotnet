// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.DotNet.Build.Tasks;
using System;
using System.IO;

namespace Microsoft.DotNet.CMake.Sdk
{
    /// <summary>
    /// Creates a CMake File API query file to request codemodel information.
    /// </summary>
    public class CreateCMakeFileApiQuery : BuildTask
    {
        /// <summary>
        /// The CMake build output directory where the query should be created.
        /// </summary>
        [Required]
        public string CMakeOutputDir { get; set; }

        public override bool Execute()
        {
            try
            {
                // Create a client stateless query file with client name "Microsoft.DotNet.CMake.Sdk"
                string queryDir = Path.Combine(CMakeOutputDir, ".cmake", "api", "v1", "query", "client-Microsoft.DotNet.CMake.Sdk");
                Directory.CreateDirectory(queryDir);

                string queryFile = Path.Combine(queryDir, "codemodel-v2");
                
                // Create an empty file to request codemodel-v2 information
                File.WriteAllText(queryFile, string.Empty);
                
                Log.LogMessage(LogImportance.Low, "Created CMake File API query at: {0}", queryFile);
                
                return true;
            }
            catch (Exception ex)
            {
                Log.LogErrorFromException(ex, showStackTrace: false);
                return false;
            }
        }
    }
}
