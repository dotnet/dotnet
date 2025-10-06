// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Newtonsoft.Json;

namespace Microsoft.VisualStudio.Razor.LiveShare.Serialization;

internal static class LiveShareJsonConverterCollectionExtensions
{
    public static void RegisterRazorLiveShareConverters(this JsonConverterCollection collection)
    {
        if (collection is null)
        {
            throw new ArgumentNullException(nameof(collection));
        }

        if (!collection.Contains(ProjectSnapshotHandleProxyJsonConverter.Instance))
        {
            collection.Add(ProjectSnapshotHandleProxyJsonConverter.Instance);
        }
    }
}
