using System;
using System.Collections.Generic;
using System.CommandLine;
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
                ? new CliArgument<DirectoryInfo>("path")
                : new CliArgument<DirectoryInfo>("path") { DefaultValueFactory = _ => nugetSlnDirectory };

            var resortOption = new CliOption<bool>("--resort");

            var rootCommand = new CliRootCommand()
            {
                pathArgument,
                resortOption
            };

            rootCommand.Description = "Copy and merge contents of PublicAPI.Unshipped.txt to PublicAPI.Shipped.txt. See https://github.com/NuGet/NuGet.Client/tree/dev/docs/nuget-sdk.md#Shipping_NuGet for more details.";

            rootCommand.SetAction(async (ParseResult, CancellationToken) =>
            {
                var path_Argument = ParseResult.GetValue<DirectoryInfo>(pathArgument);
                var resort_Option = ParseResult.GetValue<bool>(resortOption);
                if (path_Argument is not null)
                {
                    await MainAsync(path_Argument, resort_Option);
                }
            });

            return await rootCommand.Parse(args).InvokeAsync();
        }

        private static DirectoryInfo? FindNuGetSlnDirectory()
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
            foreach (FileInfo unshippedTxtPath in path.EnumerateFiles("PublicAPI.Unshipped.txt", new EnumerationOptions() { MatchCasing = MatchCasing.CaseInsensitive, RecurseSubdirectories = true }))
            {
                foundAtLeastOne = true;
                if (unshippedTxtPath.Length == 0 && !resort)
                {
                    Console.WriteLine(unshippedTxtPath.FullName + ": Up to date");
                    continue;
                }

                if (unshippedTxtPath.DirectoryName == null)
                {
                    throw new Exception("Found a file that's not in a directory?");
                }
                var shippedTxtPath = Path.Combine(unshippedTxtPath.DirectoryName, "PublicAPI.Shipped.txt");
                if (!File.Exists(shippedTxtPath))
                {
                    throw new FileNotFoundException($"Cannot migrate APIs from {unshippedTxtPath.FullName}. {shippedTxtPath} not found.");
                }

                int unshippedApiCount = await MoveUnshippedApisToShippedAsync(shippedTxtPath, unshippedTxtPath.FullName);
                Console.WriteLine($"{unshippedTxtPath.FullName}: Shipped {unshippedApiCount} APIs.");
            }

            if (!foundAtLeastOne)
            {
                Console.Error.WriteLine("Did not find any PublicAPI.Unshipped.txt files under " + path.FullName);
                return -3;
            }

            return 0;
        }

        private static async Task<int> MoveUnshippedApisToShippedAsync(string shippedTxtPath, string unshippedTxtPath)
        {
            var shippedLines = new List<string>();
            var unshippedLines = new List<string>();
            int unshippedApiCount = 0;

            using (var stream = File.OpenText(unshippedTxtPath))
            {
                string? line;
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
                string? line;
                while ((line = await stream.ReadLineAsync()) != null)
                {
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        shippedLines.Add(line);
                    }
                }
            }

            shippedLines.Sort(PublicAPIAnalyzerLineComparer.Instance);

            await File.WriteAllLinesAsync(shippedTxtPath, shippedLines);
            await File.WriteAllLinesAsync(unshippedTxtPath, unshippedLines);

            return unshippedApiCount;
        }
    }
}
