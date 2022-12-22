// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Microsoft.DotNet.ApiCompat.Tests
{
    public static class Helpers
    {
        public static string RunApiCompat(string left, string rightDirs, ApiCompatFrontend frontend) => RunApiCompat(left, rightDirs, null, null, frontend);

        public static string RunApiCompat(string left, string rightDirs, string leftName, string rightName, ApiCompatFrontend frontend) => RunApiCompat(left, new string[] { rightDirs }, Enumerable.Empty<string>(), leftName, rightName, frontend);

        public static string RunApiCompat(string left, IEnumerable<string> rightDirs, IEnumerable<string> excludeAttributesFiles, string leftName, string rightName, ApiCompatFrontend frontend)
        {
            using var writer = new StringWriter();
            string frameworkRuntimePath = Path.GetDirectoryName(typeof(object).Assembly.Location);

            if (frontend == ApiCompatFrontend.Console)
            {
                string[] args = GetApiCompatArgs(left, rightDirs, excludeAttributesFiles, leftName, rightName, frameworkRuntimePath);
                new ApiCompatRunner(writer).Run(args);
            }
            else if (frontend == ApiCompatFrontend.MSBuildTask)
            {
                new ApiCompatTask(writer)
                {
                    Contracts = new string[] { left },
                    ImplementationDirectories = rightDirs
                        .Concat(new string[] { frameworkRuntimePath }).ToArray(),
                    ContractDepends = new string[] { frameworkRuntimePath },
                    LeftOperand = leftName,
                    RightOperand = rightName,
                    ExcludeAttributes = excludeAttributesFiles?.ToArray()
                }.Execute();
            }

            return writer.ToString();
        }

        private static string[] GetApiCompatArgs(string left, IEnumerable<string> rightDirs, IEnumerable<string> excludeAttributesFiles, string leftName, string rightName, string frameworkRuntimePath)
        {
            List<string> args = new()
            {
                left,
                "-i",
                $"{string.Join(",", rightDirs.ToArray())},{frameworkRuntimePath}",
                "--contract-depends",
                frameworkRuntimePath
            };

            if (!string.IsNullOrEmpty(leftName))
            {
                args.Add("-l");
                args.Add(leftName);
            }

            if (!string.IsNullOrEmpty(rightName))
            {
                args.Add("-r");
                args.Add(rightName);
            }

            if (excludeAttributesFiles.Count() > 0)
            {
                args.Add("--exclude-attributes");
                args.Add(string.Join(",", excludeAttributesFiles));
            }

            return args.ToArray();
        }
    }
}
