// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.CommandLine;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.DotNet.SwaggerGenerator.Modeler;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Reader;

namespace Microsoft.DotNet.SwaggerGenerator.CmdLine
{
    internal static class Program
    {
        private static async Task<int> Main(string[] args)
        {
            Option<string> inputOption = new("--input", "-i")
            {
                Description = "The input swagger spec uri",
            };
            Option<string> outputOption = new("--output", "-o")
            {
                Description = "The output directory for generated code",
            };
            Option<string> namespaceOption = new("--namespace", "-n", "--ns")
            {
                Description = "The namespace for generated code",
                DefaultValueFactory = _ => "Generated",
            };
            Option<string> languageOption = new("--language", "-l")
            {
                Description = "The language to generate code for",
                DefaultValueFactory = _ => "csharp",
            };
            Option<string> clientNameOption = new("--client-name", "-c")
            {
                Description = "The name of the generated client",
                DefaultValueFactory = _ => "ApiClient",
            };

            RootCommand rootCommand = new("dotnet-swaggergen")
            {
                inputOption,
                outputOption,
                namespaceOption,
                languageOption,
                clientNameOption,
            };

            rootCommand.SetAction((result, cancellationToken) =>
            {
                string input = result.GetValue(inputOption);
                string output = result.GetValue(outputOption);

                if (string.IsNullOrEmpty(input))
                {
                    return Task.FromResult(MissingArgument(nameof(input)));
                }

                if (string.IsNullOrEmpty(output))
                {
                    return Task.FromResult(MissingArgument(nameof(output)));
                }

                var generatorOptions = new GeneratorOptions
                {
                    LanguageName = result.GetValue(languageOption),
                    Namespace = result.GetValue(namespaceOption),
                    ClientName = result.GetValue(clientNameOption),
                };

                return RunAsync(input, output, generatorOptions);
            });

            return await rootCommand.Parse(args).InvokeAsync();
        }

        private static int MissingArgument(string name)
        {
            Console.Error.WriteLine($"fatal: Missing required argument {name}");
            return -1;
        }

        private static async Task<int> RunAsync(string input, string output, GeneratorOptions generatorOptions)
        {
            ILogger logger = LoggerFactory.Create(builder => builder.AddSimpleConsole()).CreateLogger("dotnet-swaggergen");

            var (diagnostic, document) = await GetSwaggerDocument(input);
            if (diagnostic.Errors.Any())
            {
                foreach (var error in diagnostic.Errors)
                {
                    Console.Error.WriteLine($"error: In {error.Pointer} '{error.Message}'");
                }

                Console.Error.WriteLine("OpenApi Document parsing resulted in errors. Output may be compromised.");
            }

            var generator = new ServiceClientModelFactory(generatorOptions);
            ServiceClientModel model = generator.Create(document);

            var codeFactory = new ServiceClientCodeFactory();
            List<CodeFile> code = codeFactory.GenerateCode(model, generatorOptions);

            var outputDirectory = new DirectoryInfo(output);
            outputDirectory.Create();

            foreach ((string path, string contents) in code)
            {
                string fullPath = Path.Combine(outputDirectory.FullName, path);
                var file = new FileInfo(fullPath);
                file.Directory.Create();
                File.WriteAllText(file.FullName, contents);
            }

            return 0;
        }

        private static async Task<(OpenApiDiagnostic, OpenApiDocument)> GetSwaggerDocument(string input)
        {
            using (var client = new HttpClient(new HttpClientHandler { CheckCertificateRevocationList = true }))
            {
                using (Stream docStream = await client.GetStreamAsync(input))
                {
                    ReadResult result = await ServiceClientModelFactory.ReadDocumentAsync(docStream);

                    return (result.Diagnostic, result.Document);
                }
            }
        }
    }
}
