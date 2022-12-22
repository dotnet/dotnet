// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using NuGet.Configuration;

namespace Test.Utility
{
    public class PackageSourceMappingUtility
    {
        public static PackageSourceMapping GetpackageSourceMapping(string packagePatterns)
        {
            string[] sections = packagePatterns.Split('|');
            var patterns = new Dictionary<string, IReadOnlyList<string>>();

            foreach (string section in sections)
            {
                string[] parts = section.Split(',');
                string sourceKey = parts[0];

                if (string.IsNullOrWhiteSpace(sourceKey))
                {
                    continue;
                }

                var patternsList = new List<string>();
                for (int i = 1; i < parts.Length; i++)
                {
                    patternsList.Add(parts[i]);
                }

                patterns[sourceKey] = patternsList;
            }
            ;
            return new PackageSourceMapping(patterns);
        }
    }
}
