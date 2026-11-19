// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Microsoft.DotNet.Arcade.Sdk;

public class LocateDotNet : Microsoft.Build.Utilities.Task
{
    private static readonly string s_cacheKey = "LocateDotNet-FCDFF825-F35B-4601-9CB5-74DCA498B589";

    private static readonly JsonDocumentOptions s_globalJsonOptions = new()
    {
        AllowTrailingCommas = true,
        CommentHandling = JsonCommentHandling.Skip,
    };

    private sealed class CacheEntry
    {
        public readonly DateTime LastWrite;
        public readonly string Paths;
        public readonly string Value;

        public CacheEntry(DateTime lastWrite, string paths, string value)
        {
            LastWrite = lastWrite;
            Paths = paths;
            Value = value;
        }
    }

    [Required]
    public string RepositoryRoot { get; set; }

    [Output]
    public string DotNetPath { get; set; }

    public override bool Execute()
    {
        ExecuteImpl();
        return !Log.HasLoggedErrors;
    }

    private void ExecuteImpl()
    {
        var globalJsonPath = Path.Combine(RepositoryRoot, "global.json");

        var lastWrite = File.GetLastWriteTimeUtc(globalJsonPath);
        var paths = Environment.GetEnvironmentVariable("PATH");

        var cachedResult = (CacheEntry)BuildEngine4.GetRegisteredTaskObject(s_cacheKey, RegisteredTaskObjectLifetime.Build);
        if (cachedResult != null && lastWrite == cachedResult.LastWrite && paths == cachedResult.Paths)
        {
            Log.LogMessage(MessageImportance.Low, $"Reused cached value.");
            DotNetPath = cachedResult.Value;
            return;
        }

        var globalJson = File.ReadAllText(globalJsonPath);

        if (!TryGetDotNetSdkVersion(globalJson, globalJsonPath, out var sdkVersion, out var error))
        {
            Log.LogError(error);
            return;
        }

        var fileName = (Path.DirectorySeparatorChar == '\\') ? "dotnet.exe" : "dotnet";
        var dotNetDir = paths.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault(p => File.Exists(Path.Combine(p, fileName)));

        if (dotNetDir == null || !Directory.Exists(Path.Combine(dotNetDir, "sdk", sdkVersion)))
        {
            Log.LogError($"Unable to find dotnet with SDK version '{sdkVersion}'");
            return;
        }

        DotNetPath = Path.GetFullPath(Path.Combine(dotNetDir, fileName));
        BuildEngine4.RegisterTaskObject(s_cacheKey, new CacheEntry(lastWrite, paths, DotNetPath), RegisteredTaskObjectLifetime.Build, allowEarlyCollection: true);
    }

    /// <summary>
    /// Reads the .NET SDK version that Arcade installs from global.json content: 'tools.dotnet' if
    /// specified, otherwise 'sdk.version'.
    /// Keep in sync with eng/common/tools.ps1 and eng/common/tools.sh.
    /// </summary>
    internal static bool TryGetDotNetSdkVersion(string globalJsonContent, string globalJsonPath, out string version, out string error)
    {
        version = null;
        error = null;

        JsonDocument document;
        try
        {
            document = JsonDocument.Parse(globalJsonContent, s_globalJsonOptions);
        }
        catch (JsonException e)
        {
            error = $"Unable to parse file '{globalJsonPath}': {e.Message}";
            return false;
        }

        using (document)
        {
            JsonElement root = document.RootElement;

            if (TryGetNonEmptyString(root, "tools", "dotnet", out version) ||
                TryGetNonEmptyString(root, "sdk", "version", out version))
            {
                return true;
            }

            error = $"Unable to determine dotnet version from file '{globalJsonPath}'. Specify 'tools.dotnet' or 'sdk.version'.";
            return false;
        }
    }

    private static bool TryGetProperty(JsonElement element, string name, out JsonElement value)
    {
        value = default;
        return element.ValueKind == JsonValueKind.Object && element.TryGetProperty(name, out value);
    }

    private static bool TryGetNonEmptyString(JsonElement root, string objectName, string propertyName, out string value)
    {
        value = null;
        if (TryGetProperty(root, objectName, out JsonElement obj) &&
            TryGetProperty(obj, propertyName, out JsonElement property) &&
            property.ValueKind == JsonValueKind.String)
        {
            value = property.GetString();
        }

        return !string.IsNullOrEmpty(value);
    }
}
