// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Newtonsoft.Json;

namespace Microsoft.VisualStudio.LiveShare.Razor.Serialization;

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
