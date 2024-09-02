// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable enable

using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Azure.Core;
using Microsoft.DiaSymReader.Tools;
using Microsoft.SymbolStore;

namespace Microsoft.DotNet.Internal.SymbolHelper;

/// <summary>
/// Helper class for uploading symbols to a symbol server. This file assumes the logger to be thread safe.
/// All state within this is immutable after construction, and the class is thread safe. Multiple uploads
/// can be done in parallel with the same instance.
/// The usual workflow is to create a request, add files and packages to it as needed. Finally, the request
/// can be finalized with some TTL if all uploads. Otherwise, if assets fail to upload, the request can be
/// deleted.
/// There's a few options for the helper that can be controlled by the <see cref="SymbolPublisherOptions"/> passed in,
/// notably the ability to convert portable PDBs to Windows PDBs and the ability to generate a special manifest
/// for the official runtime builds.
/// </summary>
public sealed class SymbolUploadHelper
{
    public const string ConversionFolderName = "_convertedPdbs";
    private const string AzureDevOpsResource = "499b84ac-1321-427f-aa17-267ca6975798";
    private const string PathEnvVarName = "AzureDevOpsToken";
    private static readonly FrozenSet<string> s_validExtensions = FrozenSet.ToFrozenSet(["", ".exe", ".dll", ".pdb", ".so", ".dbg", ".dylib", ".dwarf", ".r2rmap"]);
    private readonly ScopedTracerFactory _tracerFactory;
    private readonly ScopedTracer _globalTracer;
    private readonly string _workingDir;
    private readonly TokenCredential _credential;
    private readonly string _commonArgs;
    private readonly string _symbolToolPath;
    private readonly PdbConverter? _pdbConverter;
    private readonly uint _symbolToolTimeoutInMins;
    private readonly bool _shouldGenerateManifest;
    private readonly bool _shouldConvertPdbs;
    private readonly bool _isDryRun;
    private readonly FrozenSet<string> _packageFileExclusions;
    private readonly bool _treatPdbConversionIssuesAsInfo;

    /// <summary>
    /// Initializes a new instance of the <see cref="SymbolUploadHelper"/> class.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="symbolToolPath">The path to the symbol tool.</param>
    /// <param name="options">The symbol publisher options.</param>
    /// <param name="workingDir">The working directory.</param>
    internal SymbolUploadHelper(ITracer logger, string symbolToolPath, SymbolPublisherOptions options, string? workingDir = null)
    {
        // These are all validated by the factory since this constructor is internal.
        // If these invariants change, the factory should be updated.
        Debug.Assert(logger is not null);
        Debug.Assert(options is not null);
        Debug.Assert(!string.IsNullOrEmpty(symbolToolPath) && (File.Exists(symbolToolPath) || options.IsDryRun));

        _tracerFactory = new ScopedTracerFactory(logger!);
        _globalTracer = _tracerFactory.CreateTracer(nameof(SymbolUploadHelper));
        _symbolToolTimeoutInMins = options.OperationTimeoutInMins;

        _commonArgs = $"-s https://{options!.AzdoOrg}.artifacts.visualstudio.com/ --patAuthEnvVar {PathEnvVarName} -t --timeout {_symbolToolTimeoutInMins}";
        if (options.VerboseClient)
        {
            // the true verbosity level is "verbose" but the tool is very chatty at that level.
            // "info" is a good balance for the errors that tend to come up in our layer.
            _commonArgs += " --tracelevel info";
        }
        else
        {
            _commonArgs += " --tracelevel warn";
        }

        _workingDir = workingDir ?? Path.GetTempPath();
        _credential = options.Credential;
        _symbolToolPath = symbolToolPath;

        // This is a special case for dotnet internal builds, particularly to control the special indexing of
        // diagnostic artifacts coming from the runtime build. Any runtime pack or cross OS diagnostic symbol
        // package needs this - and it will generate a special JSON manifest for the symbol client to consume.
        // All other builds should not set this flag in the interest of perf.
        _shouldGenerateManifest = options.DotnetInternalPublishSpecialClrFiles;

        // This is an extremely slow operation and should be used sparingly. We usually only want to do this
        // in the staging/release pipeline, not in the nightly build pipeline.
        _shouldConvertPdbs = options.ConvertPortablePdbs;
        _isDryRun = options.IsDryRun;
        _packageFileExclusions = options.PackageFileExcludeList;

        if (_shouldConvertPdbs)
        {
            _treatPdbConversionIssuesAsInfo = options.TreatPdbConversionIssuesAsInfo;
            _pdbConverter = new PdbConverter(diagnostic =>
            {
                string message = diagnostic.ToString();
                if (_treatPdbConversionIssuesAsInfo)
                {
                    _globalTracer.Information(message);
                }
                else if (options.PdbConversionTreatAsWarning.Contains((int)diagnostic.Id))
                {
                    _globalTracer.Warning(message);
                }
                else
                {
                    _globalTracer.Error(message);
                }
            });
        }
    }

    public async Task<int> GetClientDiagnosticInfo()
    {
        ScopedTracer logger = _tracerFactory.CreateTracer(nameof(GetClientDiagnosticInfo));
        logger.Information("Client Path: {0}", _symbolToolPath);
        return await RunSymbolCommand("help", ".", logger).ConfigureAwait(false);
    }

    /// <summary>
    /// Creates a symbol request.
    /// </summary>
    /// <param name="name">The name of the symbol request.</param>
    /// <returns>The result of the operation.</returns>
    public async Task<int> CreateRequest(string? name)
    {
        ScopedTracer logger = _tracerFactory.CreateTracer(nameof(CreateRequest));

        ValidateRequestName(name, logger);

        logger.Information("Creating symbol request: {0}", name!);
        string arguments = $"create {_commonArgs} --name {name}";
        return await RunSymbolCommand(arguments, ".", logger).ConfigureAwait(false);
    }

    /// <summary>
    /// Adds files to a symbol request.
    /// </summary>
    /// <param name="name">The name of the symbol request to append to. Must be non-finalized.</param>
    /// <param name="files">The files to add.</param>
    /// <returns>The result of the operation.</returns>
    public async Task<int> AddFiles(string? name, IEnumerable<string> files)
    {
        ScopedTracer logger = _tracerFactory.CreateTracer(nameof(AddFiles));
        ValidateRequestName(name, logger);

        if (!files.Any())
        {
            logger.WriteLine("No files to add to request {0}", name!);
            return 0;
        }

        // We create a folder and copy loose files to avoid starting a process per file.
        DirectoryInfo tempDirInfo = CreateTempDirectory();
        string tempCopyPath = tempDirInfo.FullName;
        try
        {
            foreach (string file in files)
            {
                using FileStream fs = File.OpenRead(file);
                string destPath = Path.Combine(tempCopyPath, Path.GetFileName(file));
                using FileStream fsCopy = new(destPath, FileMode.Create);
                await fs.CopyToAsync(fsCopy).ConfigureAwait(false);
            }

            if (_shouldConvertPdbs)
            {
                ConvertPortablePdbsInDirectory(logger, tempCopyPath);
            }

            logger.WriteLine("Adding files to request {0}", name!);
            return await AddDirectoryCore(name!, tempCopyPath, manifestPath: null, logger).ConfigureAwait(false);
        }
        finally
        {
            logger.Information("Cleaning up temporary directory {0}", tempDirInfo.FullName);
            try { tempDirInfo.Delete(recursive: true); } catch { }
        }
    }

    /// <summary>
    /// Adds a package to a symbol request. This respects conversion requests and manifest generation
    /// if such options were specified at helper creation time.
    /// </summary>
    /// <param name="name">The name of the symbol request.</param>
    /// <param name="packagePath">The path to the package.</param>
    /// <returns>The result of the operation.</returns>
    public async Task<int> AddPackageToRequest(string? name, string packagePath)
    {
        ScopedTracer logger = _tracerFactory.CreateTracer(nameof(AddPackagesToRequest));
        ValidateRequestName(name, logger);
        string packageName = Path.GetFileName(packagePath);
        using IDisposable scopeToken = logger.AddSubScope(packageName);
        return await AddPackageToRequestCore(name!, packagePath, logger).ConfigureAwait(false);
    }

    /// <summary>
    /// Adds multiple packages to a symbol request. This respects conversion requests and manifest generation
    /// if such options were specified at helper creation time.
    /// </summary>
    /// <param name="name">The name of the symbol request.</param>
    /// <param name="packagePaths">The paths to the packages.</param>
    /// <returns>The result of the operation.</returns>
    public async Task<int> AddPackagesToRequest(string? name, IEnumerable<string> packagePaths)
    {
        ScopedTracer logger = _tracerFactory.CreateTracer(nameof(AddPackagesToRequest));
        ValidateRequestName(name, logger);

        int result = 0;

        foreach (string package in packagePaths)
        {
            string packageName = Path.GetFileName(package);
            using IDisposable scopeToken = logger.AddSubScope(packageName);
            result = await AddPackageToRequestCore(name!, package, logger).ConfigureAwait(false);
            if (result != 0)
            {
                break;
            }
        }
        return result;
    }

    /// <summary>
    /// Finalizes a symbol request.
    /// </summary>
    /// <param name="name">The name of the symbol request.</param>
    /// <param name="daysToRetain">The number of days to retain the request.</param>
    /// <returns>The result of the operation.</returns>
    public async Task<int> FinalizeRequest(string? name, uint daysToRetain)
    {
        ScopedTracer logger = _tracerFactory.CreateTracer(nameof(FinalizeRequest));
        ValidateRequestName(name, logger);

        logger.WriteLine("Finalize symbol request: {0}", name!);
        string arguments = $"finalize {_commonArgs} --name {name} --expirationInDays {daysToRetain}";
        return await RunSymbolCommand(arguments, ".", logger).ConfigureAwait(false);
    }

    /// <summary>
    /// Deletes a symbol request.
    /// </summary>
    /// <param name="name">The name of the symbol request.</param>
    /// <returns>The result of the operation.</returns>
    public async Task<int> DeleteRequest(string? name, bool synchronous = false)
    {
        ScopedTracer logger = _tracerFactory.CreateTracer(nameof(DeleteRequest));
        ValidateRequestName(name, logger);
        logger.WriteLine("Deleting symbol request: {0}", name!);
        string arguments = $"delete {_commonArgs} --name {name} --quiet";
        if (synchronous)
        {
            arguments += " --synchronous";
        }
        return await RunSymbolCommand(arguments, ".", logger).ConfigureAwait(false);
    }

    private async Task<int> AddDirectoryCore(string name, string pathToAdd, string? manifestPath, ScopedTracer logger)
    {
        logger.WriteLine("Adding directory {0} to request {1}", pathToAdd, name);
        string arguments = $"adddirectory {_commonArgs} -n {name} --directory {pathToAdd} --recurse true";

        if (manifestPath is not null)
        {
            arguments += " --manifest " + manifestPath;
        }

        return await RunSymbolCommand(arguments, pathToAdd, logger).ConfigureAwait(false);
    }

    private async Task<int> AddPackageToRequestCore(string name, string packagePath, ScopedTracer logger)
    {
        // Create a temporary directory to extract the package contents.
        DirectoryInfo packageDirInfo = CreateTempDirectory();
        string packageExtractDir = packageDirInfo.FullName;
        try
        {
            logger.WriteLine("Processing package");
            using ZipArchive archive = ZipFile.Open(packagePath, ZipArchiveMode.Read);

            logger.Information("Extracting symbol package {0} to {1}", packagePath, packageExtractDir);

            foreach (ZipArchiveEntry entry in archive.Entries)
            {
                if (entry.FullName.EndsWith('/'))
                {
                    Debug.Assert(entry.Length == 0);
                    continue;
                }

                if (!ShouldIndexPackageFile(entry.FullName))
                {
                    logger.Verbose("Skipping {0}", entry.FullName);
                    continue;
                }

                logger.Verbose("Extracting {0}", entry.FullName);
                string entryPath = Path.Combine(packageExtractDir, entry.FullName);
                _ = Directory.CreateDirectory(Path.GetDirectoryName(entryPath)!);
                using Stream entryStream = entry.Open();
                using FileStream entryFile = File.Create(entryPath);
                await entryStream.CopyToAsync(entryFile).ConfigureAwait(false);
            }

            if (_shouldConvertPdbs)
            {
                ConvertPortablePdbsInDirectory(logger, packageExtractDir);
            }

            string? manifest = null;
            if (_shouldGenerateManifest)
            {
                manifest = Path.Combine(packageExtractDir, "correlatedSymKeysManifest.json");
                if (!SymbolManifestGenerator.SymbolManifestGenerator.GenerateManifest(logger, packageDirInfo, manifest, specialFilesRequireAdjacentRuntime: false))
                {
                    logger.Error("Failed to generate symbol manifest");
                    return -1;
                }
                logger.Verbose("Generated manifest in {0}", manifest);
            }

            logger.WriteLine("Adding package {0} to request {1}", packagePath, name);
            return await AddDirectoryCore(name, packageExtractDir, manifest, logger).ConfigureAwait(false);
        }
        finally
        {
            logger.Information("Cleaning up temporary directory {0}", packageDirInfo.FullName);
            try { packageDirInfo.Delete(recursive: true); } catch {}
        }

        bool ShouldIndexPackageFile(string relativeFilePath)
        {
            if (relativeFilePath.StartsWith("ref/")
                || relativeFilePath.StartsWith("_rels/")
                || relativeFilePath.StartsWith("package/")
                || relativeFilePath.EndsWith("_.pdb"))
            {
                // Quick bail - special nupkg files and ref assemblies are not indexed.
                return false;
            }

            relativeFilePath = relativeFilePath.Replace("//", "/");

            if (_packageFileExclusions.Contains(relativeFilePath))
            {
                return false;
            }

            string extension = Path.GetExtension(relativeFilePath);
            return s_validExtensions.Contains(extension);
        }
    }

    private void ConvertPortablePdbsInDirectory(ScopedTracer logger, string filesDir)
    {
        Action<string> logWarning = _treatPdbConversionIssuesAsInfo ? logger.Information : logger.Error;
        string convertedPdbFolder = Path.Combine(filesDir, ConversionFolderName);
        _ = Directory.CreateDirectory(convertedPdbFolder);
        foreach (string file in Directory.EnumerateFiles(filesDir, "*.pdb", SearchOption.AllDirectories))
        {
            using Stream pdbStream = File.OpenRead(file);
            if (!PdbConverter.IsPortable(pdbStream))
            {
                continue;
            }

            logger.Verbose("Converting {0} to portable format", file);

            string pePath = Path.ChangeExtension(file, ".dll");
            // Try to fall back to the framework exe scenario.
            if (!File.Exists(pePath))
            {
                pePath = Path.ChangeExtension(file, ".exe");
            }

            if (!File.Exists(pePath))
            {
                logWarning($"Conversion error: could not find matching PE file for {file}");
                continue;
            }

            string convertedPdbPath = Path.Combine(convertedPdbFolder, Path.GetFileName(file));

            try
            {
                using Stream peStream = File.OpenRead(pePath);
                using Stream convertedPdbStream = File.Create(convertedPdbPath);
                _pdbConverter!.ConvertWindowsToPortable(peStream, pdbStream, convertedPdbStream);
            }
            catch (Exception ex)
            {
                logWarning($"Conversion error: {ex.Message}");
                continue;
            }

            logger.Verbose("Converted successfully to {0}.", convertedPdbPath);
        }
    }

    private DirectoryInfo CreateTempDirectory()
    {
        string tempDir = Path.Combine(_workingDir, Path.GetRandomFileName());
        while (Directory.Exists(tempDir) || File.Exists(tempDir))
        {
            tempDir = Path.Combine(_workingDir, Path.GetRandomFileName());
        }

        return Directory.CreateDirectory(tempDir);
    }

    private static void ValidateRequestName(string? name, ScopedTracer logger)
    {
        if (name is null or "")
        {
            logger.Error("Can't create a request with an empty name.");
            throw new ArgumentException("Name must be specified", nameof(name));
        }
    }

    private async Task<int> RunSymbolCommand(string arguments, string directory, ScopedTracer logger, CancellationToken ct = default)
    {
        // TODO: Add retry logic. Need to parse output stream for this.
        logger.Verbose("Running command: {0} {1} from '{2}'", _symbolToolPath, arguments, directory);
        using IDisposable scopedTrace = logger.AddSubScope("symbol.exe");

        if (_isDryRun)
        {
            logger.Information("Would run command: {0} {1} from '{2}'", _symbolToolPath, arguments, directory);
            return 0;
        }

        // This sentinel task is used to indicate that the output has been fully read. It's never completed.
        TaskCompletionSource<string?> outputFinishedSentinel = new();
        Stopwatch sw = Stopwatch.StartNew();

        try
        {
            AccessToken token = await _credential.GetTokenAsync(new TokenRequestContext([AzureDevOpsResource]), ct).ConfigureAwait(false);
            ProcessStartInfo info = new(_symbolToolPath, arguments)
            {
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                WorkingDirectory = directory,
                Environment = { [PathEnvVarName] = token.Token }
            };

            using CancellationTokenSource lcts = CancellationTokenSource.CreateLinkedTokenSource(ct);

            using Process process = new()
            {
                StartInfo = info
            };

            _ = process.Start();

            lcts.CancelAfter(TimeSpan.FromMinutes(_symbolToolTimeoutInMins));
            ct = lcts.Token;

            Task processExit = process.WaitForExitAsync(ct);

            StreamReader standardOutput = process.StandardOutput;
            Task<string?> outputAvailable = standardOutput.ReadLineAsync(ct).AsTask();

            StreamReader standardError = process.StandardError;
            Task<string?> errorAvailable = standardError.ReadLineAsync(ct).AsTask();

            while (!ct.IsCancellationRequested && (outputAvailable != outputFinishedSentinel.Task || errorAvailable != outputFinishedSentinel.Task))
            {
                if (processExit.IsCompleted)
                {
                    // We already did the work. Might as well drain the IO.
                    lcts.Dispose();
                    logger.Verbose("uploader completion detected after {0}. Draining I/O streams.", sw.Elapsed);
                }

                Task alertedTask = await Task.WhenAny(outputAvailable, errorAvailable).ConfigureAwait(false);

                if (alertedTask == outputAvailable)
                {
                    outputAvailable = await LogFromStreamReader(outputAvailable, standardOutput.ReadLineAsync, logger.Verbose, ct);
                }
                else if (alertedTask == errorAvailable)
                {
                    errorAvailable = await LogFromStreamReader(errorAvailable, standardError.ReadLineAsync, logger.Error, ct);
                }
            }

            if (ct.IsCancellationRequested && !process.HasExited)
            {
                try { process.Kill(); } catch (InvalidOperationException) { }
                return -1;
            }

            // This should be a no-op if the process has already exited. Since it's not the ct doing this or we'd have exited, and we drained both 
            // output streams, this is the expected scenario.
            await processExit.ConfigureAwait(false);
            logger.Information("completed after {0} with exit code {1}", sw.Elapsed, process.ExitCode);
            return process.ExitCode;
        }
        catch (Exception ex)
        {
            logger.Error("Unable to finish invocation or drain its output after {0}: {1}", sw.Elapsed, ex);
            return -1;
        }

        async Task<Task<string?>> LogFromStreamReader(Task<string?> outputTask, Func<CancellationToken, ValueTask<string?>> readLine, Action<string> logMethod, CancellationToken ct)
        {
            ValueTask<string?> vt = new(outputTask);

            while (!ct.IsCancellationRequested && outputTask.IsCompleted)
            {
                string? line = await vt.ConfigureAwait(false);
                if (line is not null)
                {
                    logMethod(line);
                    vt = readLine(ct);
                }
                else
                {
                    return outputFinishedSentinel.Task;
                }
            }

            return vt.AsTask();
        }
    }
}
