using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;
using System.Threading.Tasks;

namespace NuGet.Internal.Tools.ShipPublicApis
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            var nugetSlnDirectory = FindNuGetSlnDirectory();
            var pathArgument = nugetSlnDirectory == null
                ? new Argument<DirectoryInfo>("path")
                : new Argument<DirectoryInfo>("path", getDefaultValue: () => nugetSlnDirectory);

            var rootCommand = new RootCommand()
            {
                pathArgument,
                new Option<bool>("--resort")
            };

            rootCommand.Description = "Copy and merge contents of PublicAPI.Unshipped.txt to PublicAPI.Shipped.txt. See https://github.com/NuGet/NuGet.Client/tree/dev/docs/nuget-sdk.md#Shipping_NuGet for more details.";

            rootCommand.Handler = CommandHandler.Create<DirectoryInfo, bool>(MainAsync);

            return await rootCommand.InvokeAsync(args);
        }

        private static DirectoryInfo FindNuGetSlnDirectory()
        {
            var directory = Environment.CurrentDirectory;

            while (true)
            {
                if (File.Exists(Path.Combine(directory, "NuGet.sln")))
                {
                    return new DirectoryInfo(directory);
                }

                var parent = Path.GetDirectoryName(directory);
                if (string.IsNullOrEmpty(parent) || parent == directory)
                {
                    return null;
                }

                directory = parent;
            }
        }

        static async Task<int> MainAsync(DirectoryInfo path, bool resort)
        {
            if (path == null)
            {
                Console.Error.WriteLine("No path provided");
                return -1;
            }

            if (!path.Exists)
            {
                Console.Error.WriteLine($"Path '{path.FullName}' does not exist");
                return -2;
            }

            bool foundAtLeastOne = false;
            foreach (var unshippedTxtPath in path.EnumerateFiles("PublicAPI.Unshipped.txt", new EnumerationOptions() { MatchCasing = MatchCasing.CaseInsensitive, RecurseSubdirectories = true}))
            {
                foundAtLeastOne = true;
                if (unshippedTxtPath.Length == 0 && !resort)
                {
                    Console.WriteLine(unshippedTxtPath.FullName + ": Up to date");
                    continue;
                }

                var shippedTxtPath = Path.Combine(unshippedTxtPath.DirectoryName, "PublicAPI.Shipped.txt");
                if (!File.Exists(shippedTxtPath))
                {
                    throw new FileNotFoundException($"Cannot migrate APIs from {unshippedTxtPath.FullName}. {shippedTxtPath} not found.");
                }

                var shippedLines = new List<string>();
                var unshippedLines = new List<string>();
                int unshippedApiCount = 0;
                using (var stream = unshippedTxtPath.OpenText())
                {
                    string line;
                    while ((line = await stream.ReadLineAsync()) != null)
                    {
                        if (!string.IsNullOrWhiteSpace(line))
                        {
                            if (line.StartsWith("#"))
                            {
                                unshippedLines.Add(line);
                            }
                            else
                            {
                                shippedLines.Add(line);
                                unshippedApiCount++;
                            }
                        }
                    }
                }

                using (var stream = File.OpenText(shippedTxtPath))
                {
                    string line;
                    while ((line = await stream.ReadLineAsync()) != null)
                    {
                        if (!string.IsNullOrWhiteSpace(line))
                        {
                            shippedLines.Add(line);
                        }
                    }
                }

                shippedLines.Sort(StringComparer.Ordinal);

                await File.WriteAllLinesAsync(shippedTxtPath, shippedLines);
                await File.WriteAllLinesAsync(unshippedTxtPath.FullName, unshippedLines);

                Console.WriteLine($"{unshippedTxtPath.FullName}: Shipped {unshippedApiCount} APIs.");
            }

            if (!foundAtLeastOne)
            {
                Console.Error.WriteLine("Did not find any PublicAPI.Unshipped.txt files under " + path.FullName);
                return -3;
            }

            return 0;
        }
    }
}
