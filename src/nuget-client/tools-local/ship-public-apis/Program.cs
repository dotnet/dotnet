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
                ? new Argument<DirectoryInfo>("path")
                : new Argument<DirectoryInfo>("path") { DefaultValueFactory = _ => nugetSlnDirectory };

            var resortOption = new Option<bool>("--resort");

            var rootCommand = new RootCommand()
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

        // The public API analyzer records an intentionally removed public API by adding a line to
        // PublicAPI.Unshipped.txt with this prefix followed by the exact signature that currently
        // exists in PublicAPI.Shipped.txt. Shipping such an entry means deleting the matching line
        // from PublicAPI.Shipped.txt; the marker itself must not be written into either file.
        private const string RemovedApiPrefix = "*REMOVED*";

        private static async Task<int> MoveUnshippedApisToShippedAsync(string shippedTxtPath, string unshippedTxtPath)
        {
            var shippedLines = new List<string>();
            var unshippedLines = new List<string>();
            var removedApis = new List<string>();
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
                        else if (line.StartsWith(RemovedApiPrefix, StringComparison.Ordinal))
                        {
                            removedApis.Add(line.Substring(RemovedApiPrefix.Length));
                            unshippedApiCount++;
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

            foreach (var removedApi in removedApis)
            {
                int index = shippedLines.FindIndex(shippedLine => PublicAPIAnalyzerLineComparer.Instance.Compare(shippedLine, removedApi) == 0);
                if (index < 0)
                {
                    Console.Error.WriteLine($"warning: {shippedTxtPath}: '{RemovedApiPrefix}{removedApi}' has no matching entry in PublicAPI.Shipped.txt; nothing to remove.");
                }
                else
                {
                    shippedLines.RemoveAt(index);
                }
            }

            shippedLines.Sort(PublicAPIAnalyzerLineComparer.Instance);

            await File.WriteAllLinesAsync(shippedTxtPath, shippedLines);
            await File.WriteAllLinesAsync(unshippedTxtPath, unshippedLines);

            return unshippedApiCount;
        }
    }
}
