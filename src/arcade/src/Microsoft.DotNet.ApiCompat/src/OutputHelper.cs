// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Diagnostics;
using System.IO;

namespace Microsoft.DotNet.ApiCompat
{
    internal class OutputHelper
    {
        /// <summary>
        /// Opens a stream to a given file path and returns it.
        /// </summary>
        /// <param name="outFilePath">The file path to open a stream to.</param>
        /// <param name="writer">The opened TextWriter that points to the passed in file.</param>
        /// <returns>Returns true when the output was opened or created successfully.</returns>
        public static bool TryGetOutput(string outFilePath, out TextWriter writer)
        {
            if (string.IsNullOrWhiteSpace(outFilePath))
                throw new ArgumentNullException(nameof(outFilePath));

            const int NumRetries = 10;
            string exceptionMessage = null;
            for (int retries = 0; retries < NumRetries; retries++)
            {
                try
                {
                    writer = new StreamWriter(File.OpenWrite(outFilePath));
                    return true;
                }
                catch (Exception e)
                {
                    exceptionMessage = e.Message;
                    System.Threading.Thread.Sleep(100);
                }
            }

            Trace.TraceError("Cannot open output file '{0}': {1}", outFilePath, exceptionMessage);
            writer = null;
            return false;
        }
    }
}
