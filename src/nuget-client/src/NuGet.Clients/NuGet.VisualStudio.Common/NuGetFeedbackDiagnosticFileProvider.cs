// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.Internal.VisualStudio.Shell.Embeddable.Feedback;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.PackageManagement;
using NuGet.ProjectManagement;
using NuGet.ProjectModel;
using NuGet.VisualStudio.Telemetry;

#nullable enable

namespace NuGet.VisualStudio.Common
{
    [Export(typeof(IFeedbackDiagnosticFileProvider))]
    public class NuGetFeedbackDiagnosticFileProvider : IFeedbackDiagnosticFileProvider
    {
        // Don't crash VS feedback if imports fail. Maybe broken MEF is exactly what the customer is trying to report!
        [Import(AllowDefault = true)]
        public ISolutionManager? SolutionManager { get; set; }

        [Import(AllowDefault = true)]
        public INuGetTelemetryProvider? TelemetryProvider { get; set; }

        [Import(AllowDefault = true)]
        public ISettings? Settings { get; set; }

        /// <summary>Used for testing, so tests can wait for the background task to finish.</summary>
        public event EventHandler<Task>? BackgroundTaskStarted;

        public IReadOnlyCollection<string> GetFiles()
        {
#pragma warning disable CA2000 // Dispose objects before losing scope
            // fileStream gets disposed in the background task below.
            FileStream fileStream = GetOutputFile();
#pragma warning restore CA2000 // Dispose objects before losing scope

            string fileName = fileStream.Name;

            // See comments on IFeedbackDiagnosticFileProvider.GetFiles
            Task task = Task.Run(() => WriteToZipAndCloseFileAsync(fileStream));
            BackgroundTaskStarted?.Invoke(this, task);

            return new List<string>()
            {
                fileName
            };
        }

        private async Task WriteToZipAndCloseFileAsync(Stream stream)
        {
            using (stream)
            {
                try
                {
                    await WriteToZipAsync(stream);
                }
                catch (Exception exception)
                {
                    await PostFaultAsync(exception);
                }
            }
        }

        private FileStream GetOutputFile()
        {
            string fileNamePrefix = "nuget." + DateTime.UtcNow.ToString("yyyy-MM-dd.HH-mm-ss");
            for (int attempt = 0; attempt < 100; attempt++)
            {
                string fileName = attempt == 0
                    ? fileNamePrefix + ".zip"
                    : fileNamePrefix + "." + attempt + ".zip";
                string fullPath = Path.Combine(NuGetEnvironment.GetFolderPath(NuGetFolderPath.Temp), fileName);
                try
                {
                    FileStream fileStream = new FileStream(fullPath, FileMode.CreateNew);
                    return fileStream;
                }
                catch (IOException)
                {
                    // Unlikely that the customer is going to click "report a problem" more than once per second, but just in case
                }
            }

            throw new Exception("Unable to create file to attach");
        }

        public async Task WriteToZipAsync(Stream stream)
        {
            var telemetry = new TelemetryEvent("feedback");
            telemetry["IsDebuggerAttached"] = Debugger.IsAttached;
            bool successful = false;
            Stopwatch sw = Stopwatch.StartNew();
            try
            {
                using (var zip = new ZipArchive(stream, ZipArchiveMode.Create, leaveOpen: true))
                {
                    AddMefErrors(zip);
                    await AddDgSpecAsync(zip);
                }

                successful = true;
            }
            finally
            {
                sw.Stop();
                telemetry["successful"] = successful;
                telemetry["duration_ms"] = sw.Elapsed.TotalMilliseconds;
                EmitTelemetryEvent(telemetry);
            }
        }

        private void AddMefErrors(ZipArchive zip)
        {
            if (SolutionManager != null
                && TelemetryProvider != null
                && Settings != null)
            {
                return;
            }

            ZipArchiveEntry file = zip.CreateEntry("mef-errors.txt");
            using (Stream stream = file.Open())
            using (var writer = new StreamWriter(stream))
            {
                writer.WriteLine($"{nameof(SolutionManager)}  : {GetFileOutput(SolutionManager)}");
                writer.WriteLine($"{nameof(TelemetryProvider)}: {GetFileOutput(TelemetryProvider)}");
                writer.WriteLine($"{nameof(Settings)}         : {GetFileOutput(Settings)}");
            }

            static string GetFileOutput(object? o)
            {
                if (o is null)
                    return "null";
                return "not null";
            }
        }

        private async Task AddDgSpecAsync(ZipArchive zip)
        {
            try
            {
                if (SolutionManager == null) { return; }

                var context = new DependencyGraphCacheContext(NullLogger.Instance, Settings);
                DependencyGraphSpec dgspec = await DependencyGraphRestoreUtility.GetSolutionRestoreSpec(SolutionManager, context);

                ZipArchiveEntry file = zip.CreateEntry("dgspec.json");
                using (Stream fileStream = file.Open())
                {
                    dgspec.Save(fileStream);
                }
            }
            catch (Exception exception)
            {
                await PostFaultAsync(exception);
            }
        }

        private void EmitTelemetryEvent(TelemetryEvent telemetryEvent)
        {
            if (TelemetryProvider != null)
            {
                TelemetryProvider.EmitEvent(telemetryEvent);
            }
            else
            {
                TelemetryActivity.EmitTelemetryEvent(telemetryEvent);
            }
        }

        private async Task PostFaultAsync(Exception exception, [CallerMemberName] string? callerMemberName = default)
        {
            if (TelemetryProvider != null)
            {
                await TelemetryProvider.PostFaultAsync(exception, nameof(NuGetFeedbackDiagnosticFileProvider), callerMemberName);
            }
            else
            {
                await TelemetryUtility.PostFaultAsync(exception, nameof(NuGetFeedbackDiagnosticFileProvider), callerMemberName);
            }
        }
    }
}
