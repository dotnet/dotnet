// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.DotNet.SwaggerGenerator.Modeler;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Readers;
using Task = Microsoft.Build.Utilities.Task;

namespace Microsoft.DotNet.SwaggerGenerator.MSBuild
{
    public class GenerateSwaggerCode : Microsoft.Build.Utilities.Task
    {
        [Required]
        public string SwaggerDocumentUri { get; set; }

        [Required]
        public string RootNamespace { get; set; }

        [Required]
        public string ClientName { get; set; }

        [Required]
        public string OutputDirectory { get; set; }

        [Output]
        public ITaskItem[] GeneratedFiles { get; set; }

        public override bool Execute()
        {
            try
            {
                ExecuteAsync().GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                Log.LogErrorFromException(ex, true, true, null);
            }

            return !Log.HasLoggedErrors;
        }

        private async System.Threading.Tasks.Task ExecuteAsync()
        {
            var options = new GeneratorOptions
            {
                LanguageName = "csharp",
                Namespace = RootNamespace,
                ClientName = ClientName
            };

            Log.LogMessage(MessageImportance.Low, $"Reading swagger document {SwaggerDocumentUri}");
            var (diagnostic, document) = await GetSwaggerDocument(SwaggerDocumentUri);
            if (diagnostic.Errors.Any())
            {
                foreach (var error in diagnostic.Errors)
                {
                    Log.LogWarning(null, null, null, error.Pointer, 0, 0, 0, 0, error.Message);
                }
            }


            Log.LogMessage(MessageImportance.Low, $"Generating client code model");
            var generator = new ServiceClientModelFactory(options);
            ServiceClientModel model = generator.Create(document);

            Log.LogMessage(MessageImportance.Low, $"Generating code files for language '{options.LanguageName}'");
            var codeFactory = new ServiceClientCodeFactory();
            List<CodeFile> code = codeFactory.GenerateCode(model, options);

            Log.LogMessage(MessageImportance.High, $"Generating {SwaggerDocumentUri} -> {OutputDirectory}");
            var outputDirectory = new DirectoryInfo(OutputDirectory);
            outputDirectory.Create();

            var generatedFiles = new List<ITaskItem>();
            foreach ((string path, string contents) in code)
            {
                string fullPath = Path.Combine(outputDirectory.FullName, path);
                var file = new FileInfo(fullPath);
                file.Directory.Create();
                Log.LogMessage(MessageImportance.Normal, $"Writing file '{file.FullName}'");
                File.WriteAllText(file.FullName, contents);
                generatedFiles.Add(new TaskItem(file.FullName));
            }

            GeneratedFiles = generatedFiles.ToArray();
        }

        private static async Task<(OpenApiDiagnostic, OpenApiDocument)> GetSwaggerDocument(string input)
        {
            using (var client = new HttpClient(new HttpClientHandler { CheckCertificateRevocationList = true }))
            {
                using (Stream docStream = await client.GetStreamAsync(input))
                {
                    var doc = ServiceClientModelFactory.ReadDocument(docStream, out OpenApiDiagnostic diagnostic);

                    return (diagnostic, doc);
                }
            }
        }
    }
}
