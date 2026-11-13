// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Build.Framework;
using Microsoft.DotNet.UnifiedBuild.Tasks.Services;
using BuildTask = Microsoft.Build.Utilities.Task;

namespace Microsoft.DotNet.UnifiedBuild.Tasks;

public class CreateSourceArtifact : BuildTask
{
    /// <summary>
    /// Commit SHA of the VMR source to download
    /// </summary>
    [Required]
    public required string SourceCommit { get; init; }

    /// <summary>
    /// Name of the artifact to generate
    /// </summary>
    [Required]
    public required string ArtifactName { get; init; }

    /// <summary>
    /// The stable sdk version
    /// Should match the sdk tag associated with a release
    /// Used for artifact prefixing
    /// </summary>
    [Required]
    public required string SdkStableVersion { get; init; }

    /// <summary>
    /// The build/non-stable sdk version
    /// Used for artifact versioning
    /// </summary>
    [Required]
    public required string SdkNonStableVersion { get; init; }

    /// <summary>
    /// Path to the root of the repo
    /// </summary>
    [Required]
    public required string RepoRoot { get; init; }

    /// <summary>
    /// Path to the directory to place the source artifacts in
    /// </summary>
    [Required]
    public required string OutputDirectory { get; init; }

    /// <summary>
    /// Create an empty detached signature file alongside the source artifact
    /// Ensures the signature is published via darc when signing occurs post-build
    /// </summary>
    public bool CreateEmptyDetachedSignature { get; init; } = false;

    private const string GitHubRepoName = "dotnet";
    private const string GitHubTimezone = "America/Los_Angeles"; // GitHub uses this timezone for commit timestamps in zip metadata
    private const int GitArchiveTimeout = 5 * 60 * 1000; // 5 minutes
    private const int SourceBuildGroupId = 2001;
    private const int SourceBuildUserId = 2001;
    private const int TarBlockSize = 512;
    private const int TarChecksumFieldLength = 8;
    private const int TarChecksumOffset = 148;
    private const int TarGroupIdOffset = 116;
    private const int TarGroupNameOffset = 297;
    private const int TarIdFieldLength = 8;
    private const int TarNameFieldLength = 32;
    private const int TarSizeFieldLength = 12;
    private const int TarSizeOffset = 124;
    private const int TarUserIdOffset = 108;
    private const int TarUserNameOffset = 265;
    private const string TarExtension = "tar.gz";
    private const string SignatureExtension = "sig";

    public override bool Execute() => ExecuteAsync().GetAwaiter().GetResult();

    private async Task<bool> ExecuteAsync()
    {
        var processService = new ProcessService(Log, GitArchiveTimeout);

        string artifactFilePath = Path.Combine(OutputDirectory, $"{ArtifactName}-{SdkNonStableVersion}.{TarExtension}");
        string temporaryFileBasePath = Path.Combine(OutputDirectory, $".{ArtifactName}-{Guid.NewGuid():N}");
        string temporaryGitTarFilePath = $"{temporaryFileBasePath}.git.tar";
        string temporaryTarFilePath = $"{temporaryFileBasePath}.tar";
        string temporaryArtifactFilePath = $"{temporaryTarFilePath}.gz";

        Log.LogMessage(MessageImportance.High, $"Creating {TarExtension} source artifact at: {artifactFilePath}");

        try
        {
            ProcessResult result = await processService.RunProcessAsync(
                "git",
                $"archive --format=tar --output \"{temporaryGitTarFilePath}\" --prefix \"{GitHubRepoName}-{SdkStableVersion}/\" {SourceCommit}",
                environmentVariables: new List<ProcessEnvironmentVariable> { new ProcessEnvironmentVariable("TZ", GitHubTimezone) },
                workingDirectory: RepoRoot
            );

            if (result.ExitCode != 0)
            {
                Log.LogError($"[tar] Git exited with code {result.ExitCode}: {result.Error}");
                return false;
            }

            CreateArchiveWithNormalizedOwnership(temporaryGitTarFilePath, temporaryTarFilePath);

            result = await processService.RunProcessAsync(
                "gzip",
                $"-n -f \"{temporaryTarFilePath}\""
            );

            if (result.ExitCode != 0)
            {
                Log.LogError($"[gzip] Exited with code {result.ExitCode}: {result.Error}");
                return false;
            }

            File.Move(temporaryArtifactFilePath, artifactFilePath, overwrite: true);
        }
        catch (Exception ex)
        {
            Log.LogError($"[{TarExtension}] Exception during artifact creation: {ex.Message}");
            return false;
        }
        finally
        {
            File.Delete(temporaryGitTarFilePath);
            File.Delete(temporaryTarFilePath);
            File.Delete(temporaryArtifactFilePath);
        }

        if (CreateEmptyDetachedSignature)
        {
            string signatureFilePath = $"{artifactFilePath}.{SignatureExtension}";
            Log.LogMessage(MessageImportance.High, $"Creating empty detached signature at: {signatureFilePath}");

            try
            {
                using (FileStream fs = File.Create(signatureFilePath))
                {
                    // Create an empty file
                }
            }
            catch (Exception ex)
            {
                Log.LogError($"[{SignatureExtension}] Exception during signature file creation: {ex.Message}");
                return false;
            }
        }

        return true;
    }

    private static void CreateArchiveWithNormalizedOwnership(string inputPath, string outputPath)
    {
        // Rewriting fixed header fields preserves Git's PAX metadata and deterministic header names.
        using FileStream inputStream = File.OpenRead(inputPath);
        using FileStream outputStream = File.Create(outputPath);

        byte[] headerBuffer = new byte[TarBlockSize];
        byte[] copyBuffer = new byte[81920];
        while (true)
        {
            int bytesRead = ReadBlock(inputStream, headerBuffer);
            if (bytesRead == 0)
            {
                break;
            }

            if (bytesRead != TarBlockSize)
            {
                throw new InvalidDataException($"Unexpected end of tar archive after reading {bytesRead} bytes of a header.");
            }

            if (IsEmptyBlock(headerBuffer))
            {
                outputStream.Write(headerBuffer);
                continue;
            }

            long dataLength = ReadTarNumber(headerBuffer.AsSpan(TarSizeOffset, TarSizeFieldLength));
            NormalizeOwnership(headerBuffer);
            outputStream.Write(headerBuffer);

            long paddedDataLength = checked((dataLength + TarBlockSize - 1) / TarBlockSize * TarBlockSize);
            CopyExactly(inputStream, outputStream, paddedDataLength, copyBuffer);
        }
    }

    private static void NormalizeOwnership(Span<byte> header)
    {
        WriteTarNumber(header.Slice(TarUserIdOffset, TarIdFieldLength), SourceBuildUserId);
        WriteTarNumber(header.Slice(TarGroupIdOffset, TarIdFieldLength), SourceBuildGroupId);
        header.Slice(TarUserNameOffset, TarNameFieldLength).Clear();
        header.Slice(TarGroupNameOffset, TarNameFieldLength).Clear();

        Span<byte> checksumField = header.Slice(TarChecksumOffset, TarChecksumFieldLength);
        checksumField.Fill((byte)' ');

        int checksum = 0;
        foreach (byte value in header)
        {
            checksum += value;
        }

        string checksumText = Convert.ToString(checksum, 8);
        if (checksumText.Length > 6)
        {
            throw new InvalidDataException($"Tar header checksum {checksum} does not fit in the checksum field.");
        }

        checksumField[..6].Fill((byte)'0');
        Encoding.ASCII.GetBytes(checksumText, checksumField.Slice(6 - checksumText.Length, checksumText.Length));
        checksumField[6] = 0;
        checksumField[7] = (byte)' ';
    }

    private static long ReadTarNumber(ReadOnlySpan<byte> field)
    {
        long value = 0;
        bool foundDigit = false;

        foreach (byte character in field)
        {
            if (character is 0 or (byte)' ')
            {
                if (foundDigit)
                {
                    break;
                }

                continue;
            }

            if (character is < (byte)'0' or > (byte)'7')
            {
                throw new InvalidDataException("Tar header contains an unsupported numeric value.");
            }

            foundDigit = true;
            value = checked(value * 8 + character - (byte)'0');
        }

        return value;
    }

    private static void WriteTarNumber(Span<byte> field, int value)
    {
        string valueText = Convert.ToString(value, 8);
        if (valueText.Length >= field.Length)
        {
            throw new InvalidDataException($"Tar header value {value} does not fit in a {field.Length}-byte field.");
        }

        field.Fill((byte)'0');
        Encoding.ASCII.GetBytes(valueText, field.Slice(field.Length - valueText.Length - 1, valueText.Length));
        field[^1] = 0;
    }

    private static int ReadBlock(Stream stream, byte[] buffer)
    {
        int totalBytesRead = 0;
        while (totalBytesRead < buffer.Length)
        {
            int bytesRead = stream.Read(buffer, totalBytesRead, buffer.Length - totalBytesRead);
            if (bytesRead == 0)
            {
                break;
            }

            totalBytesRead += bytesRead;
        }

        return totalBytesRead;
    }

    private static bool IsEmptyBlock(ReadOnlySpan<byte> block)
    {
        foreach (byte value in block)
        {
            if (value != 0)
            {
                return false;
            }
        }

        return true;
    }

    private static void CopyExactly(Stream input, Stream output, long bytesToCopy, byte[] buffer)
    {
        while (bytesToCopy > 0)
        {
            int bytesRead = input.Read(buffer, 0, (int)Math.Min(buffer.Length, bytesToCopy));
            if (bytesRead == 0)
            {
                throw new InvalidDataException("Unexpected end of tar archive while reading entry data.");
            }

            output.Write(buffer, 0, bytesRead);
            bytesToCopy -= bytesRead;
        }
    }
}
