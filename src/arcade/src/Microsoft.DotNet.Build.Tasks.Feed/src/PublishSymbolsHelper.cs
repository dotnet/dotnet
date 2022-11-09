// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.SymbolUploader;

namespace Microsoft.DotNet.Build.Tasks.Feed
{

    public class PublishSymbolsHelper
    {
        public static async System.Threading.Tasks.Task PublishAsync(
                TaskLoggingHelper log,
                string symbolServerPath,
                string personalAccessToken,
                IEnumerable<string> inputPackages,
                IEnumerable<string> inputFiles,
                HashSet<string> packageExcludeFiles,
                int expirationInDays,
                bool convertPortablePdbsToWindowsPdbs,
                bool publishSpecialClrFiles,
                HashSet<int> pdbConversionTreatAsWarning,
                bool treatPdbConversionIssuesAsInfo,
                bool dryRun,
                bool timer,
                bool verboseLogging)
            {
                var tracer = new Tracer(log, verboseLogging);

                PublishOperation publishOperation = new PublishOperation(tracer)
                {
                    SymbolServerPath = symbolServerPath,
                    PersonalAccessToken = personalAccessToken,
                    PdbConversionTreatAsWarning = pdbConversionTreatAsWarning,
                    PublishSpecialClrFiles = publishSpecialClrFiles,
                    Timer = timer,
                    TreatPdbConversionIssuesAsInfo = treatPdbConversionIssuesAsInfo
                };

                using (publishOperation)
                {
                    if (expirationInDays != 0)
                    {
                        publishOperation.ExpirationInDays = (uint) expirationInDays;
                    }

                    IEnumerable<PublishFileInfo> fileInfos = new PublishFileInfo[0];
                    if (inputFiles != null)
                    {
                        fileInfos = fileInfos.Concat(
                            publishOperation.GetPublishFileInfo(inputFiles, convertPortablePdbsToWindowsPdbs));
                    }

                    if (inputPackages != null)
                    {
                        fileInfos = fileInfos.Concat(
                            publishOperation.GetPublishFileInfoFromPackages(inputPackages,
                                convertPortablePdbsToWindowsPdbs));
                    }

                    if (packageExcludeFiles != null)
                    {
                        publishOperation.PackageExcludeFiles = packageExcludeFiles;
                    }

                    if (dryRun)
                    {
                        publishOperation.StartTimer();
                        try
                        {
                            foreach (PublishFileInfo fileInfo in fileInfos)
                            {
                                fileInfo.Dispose();
                            }
                        }
                        catch (Exception ex)
                        {
                            tracer.Error("Dry Run FAILED: {0}", ex.Message);
                            tracer.Information(ex.ToString());
                        }

                        publishOperation.StopTimer();
                    }
                    else
                    {
                        try
                        {
                            await publishOperation.PublishFiles(fileInfos);
                        }
                        catch(Exception ex)
                        {
                            tracer.Error("Publishing symbols failed : ", ex.Message);
                            tracer.Information(ex.ToString());
                        }

                    }
                }
            }
        }

    internal class Tracer : Microsoft.SymbolStore.ITracer
    {
        readonly TaskLoggingHelper _log;
        readonly bool _verbose;

        public Tracer(TaskLoggingHelper log, bool verbose)
        {
            _log = log;
            _verbose = verbose;
        }

        public void WriteLine(string message)
        {
            WriteLine("{0}", message);
        }

        public void WriteLine(string format, params object[] arguments)
        {
            _log.LogMessage(MessageImportance.High, format, arguments);
        }

        public void Information(string message)
        {
            Information("{0}", message);
        }

        public void Information(string format, params object[] arguments)
        {
            _log.LogMessage(MessageImportance.Normal, format, arguments);
        }

        public void Warning(string message)
        {
            Warning("{0}", message);
        }

        public void Warning(string format, params object[] arguments)
        {
            _log.LogWarning(format, arguments);
        }

        public void Error(string message)
        {
            Error("{0}", message);
        }

        public void Error(string format, params object[] arguments)
        {
            _log.LogError(format, arguments);
        }

        public void Verbose(string message)
        {
            Verbose("{0}", message);
        }

        public void Verbose(string format, params object[] arguments)
        {
            _log.LogMessage(_verbose ? MessageImportance.Normal : MessageImportance.Low, format, arguments);
        }
    }
}

