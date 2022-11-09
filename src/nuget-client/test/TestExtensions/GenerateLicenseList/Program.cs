// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace GenerateLicenseList
{
    class Program
    {
        static int Main(string[] args)
        {
            if (args.Count() != 3)
            {
                Console.WriteLine("This tool expects 3 arguments: licenses.json, exceptions.json, targetFileLocation.");
                Console.WriteLine(@"This tool should be run only through .\scripts\utils\UpdateNuGetLicenseSPDXList.ps1");
                return -1;
            }

            var licenseJson = Path.GetFullPath(args[0]);
            var exceptionJson = Path.GetFullPath(args[1]);
            var targetFile = Path.GetFullPath(args[2]);

            Console.WriteLine($"License json: {licenseJson}");
            Console.WriteLine($"Exception json: {exceptionJson}");
            Console.WriteLine($"Target file: {targetFile}");

            var licenseDataCodeGenerator = new LicenseDataCodeGenerator(licenseJson, exceptionJson);

            var node = licenseDataCodeGenerator.GenerateLicenseDataFile();
            if (node != null)
            {
                var codeIssues = node.GetDiagnostics();
                if (!codeIssues.Any())
                {
                    using (var writer = File.CreateText(targetFile))
                    {
                        node.WriteTo(writer);
                        writer.Flush();
                    }
                    Console.WriteLine($"Completed the update of {targetFile}");
                    return 0;
                }

                foreach (var codeIssue in codeIssues)
                {
                    var issue = $"ID: {codeIssue.Id}, Message: {codeIssue.GetMessage()}, Location: {codeIssue.Location.GetLineSpan()}, Severity: {codeIssue.Severity}";
                    Console.WriteLine(issue);
                }
                Console.WriteLine("License List file generation failed.");
            }
            return -1;
        }
    }
}
