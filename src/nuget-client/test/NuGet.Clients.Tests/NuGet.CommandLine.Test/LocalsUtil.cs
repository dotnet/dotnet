// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;

namespace NuGet.CommandLine.Test
{
    internal class LocalsUtil
    {
        public static string CreateDummyConfigFile(string directoryPath)
        {
            string[] lines = {"<?xml version=\"1.0\" encoding=\"utf-8\"?>",
                              "<configuration>",
                              "<config>",
                              "<add key=\"foo\" value=\"bar\" />",
                              "<add key=\"kung foo\" value=\"panda\" />",
                              "</config>",
                              "</configuration>" };
            var dummyConfigPath = Path.Combine(directoryPath, @"NuGet.config");
            File.WriteAllLines(dummyConfigPath, lines);
            return dummyConfigPath;
        }
    }
}
