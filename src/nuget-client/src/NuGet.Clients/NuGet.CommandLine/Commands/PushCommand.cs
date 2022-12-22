// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using NuGet.Commands;

namespace NuGet.CommandLine
{
    [Command(typeof(NuGetCommand), "push", "PushCommandDescription;DefaultConfigDescription",
        MinArgs = 1, MaxArgs = 2, UsageDescriptionResourceName = "PushCommandUsageDescription",
        UsageSummaryResourceName = "PushCommandUsageSummary", UsageExampleResourceName = "PushCommandUsageExamples")]
    public class PushCommand : Command
    {
        [Option(typeof(NuGetCommand), "PushCommandSourceDescription", AltName = "src")]
        public string Source { get; set; }

        [Option(typeof(NuGetCommand), "CommandApiKey")]
        public string ApiKey { get; set; }

        [Option(typeof(NuGetCommand), "PushCommandSymbolSourceDescription")]
        public string SymbolSource { get; set; }

        [Option(typeof(NuGetCommand), "SymbolApiKey")]
        public string SymbolApiKey { get; set; }

        [Option(typeof(NuGetCommand), "PushCommandTimeoutDescription")]
        public int Timeout { get; set; }

        [Option(typeof(NuGetCommand), "PushCommandDisableBufferingDescription")]
        public bool DisableBuffering { get; set; }

        [Option(typeof(NuGetCommand), "PushCommandNoSymbolsDescription")]
        public bool NoSymbols { get; set; }

        [Option(typeof(NuGetCommand), "CommandNoServiceEndpointDescription")]
        public bool NoServiceEndpoint { get; set; }

        [Option(typeof(NuGetCommand), "PushCommandSkipDuplicateDescription")]
        public bool SkipDuplicate { get; set; }

        public override async Task ExecuteCommandAsync()
        {
            string packagePath = Arguments[0];
            string apiKeyValue = null;

            if (!string.IsNullOrEmpty(ApiKey))
            {
                apiKeyValue = ApiKey;
            }
            else if (Arguments.Count > 1 && !string.IsNullOrEmpty(Arguments[1]))
            {
                apiKeyValue = Arguments[1];
            }

            try
            {
                await PushRunner.Run(
                    Settings,
                    SourceProvider,
                    new[] { packagePath },
                    Source,
                    apiKeyValue,
                    SymbolSource,
                    SymbolApiKey,
                    Timeout,
                    DisableBuffering,
                    NoSymbols,
                    NoServiceEndpoint,
                    SkipDuplicate,
                    Console);
            }
            catch (TaskCanceledException ex)
            {
                string timeoutMessage = LocalizedResourceManager.GetString(nameof(NuGetResources.PushCommandTimeoutError));
                throw new AggregateException(ex, new Exception(timeoutMessage));
            }
            catch (Exception ex)
            {
                if (ex is HttpRequestException && ex.InnerException is WebException)
                {
                    throw ex.InnerException;
                }

                throw;
            }
        }
    }
}
