// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MessagePack;
using MessagePack.Resolvers;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.AspNetCore.Razor.Serialization.MessagePack.Resolvers;
using Microsoft.CodeAnalysis.Razor.Logging;

namespace Microsoft.CodeAnalysis.Razor.Serialization;

internal class RazorProjectInfoFileSerializer(ILoggerFactory loggerFactory) : IRazorProjectInfoFileSerializer, IDisposable
{
    private static readonly MessagePackSerializerOptions s_options = MessagePackSerializerOptions.Standard
        .WithResolver(CompositeResolver.Create(
            RazorProjectInfoResolver.Instance,
            StandardResolver.Instance));

    private readonly ILogger _logger = loggerFactory.GetOrCreateLogger<RazorProjectInfoFileSerializer>();
    private readonly List<string> _filePathsToDelete = [];

    public void Dispose()
    {
        if (_filePathsToDelete.Count > 0)
        {
            foreach (var filePath in _filePathsToDelete)
            {
                DeleteFile(filePath);
            }
        }
    }

    public async Task<string> SerializeToTempFileAsync(RazorProjectInfo projectInfo, CancellationToken cancellationToken)
    {
        var filePath = Path.GetTempFileName();

        using var stream = File.OpenWrite(filePath);
        await MessagePackSerializer.SerializeAsync(stream, projectInfo, s_options, cancellationToken).ConfigureAwait(false);

        return filePath;
    }

    public async Task<RazorProjectInfo> DeserializeFromFileAndDeleteAsync(string filePath, CancellationToken cancellationToken)
    {
        RazorProjectInfo projectInfo;

        using (var stream = File.OpenRead(filePath))
        {
            projectInfo = await MessagePackSerializer.DeserializeAsync<RazorProjectInfo>(stream, s_options, cancellationToken).ConfigureAwait(false);
        }

        if (!DeleteFile(filePath))
        {
            _filePathsToDelete.Add(filePath);
        }

        return projectInfo;
    }

    private bool DeleteFile(string filePath)
    {
        try
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                return true;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"{ex.GetType().FullName} encountered when attempting to delete '{filePath}'");
        }

        return false;
    }
}
