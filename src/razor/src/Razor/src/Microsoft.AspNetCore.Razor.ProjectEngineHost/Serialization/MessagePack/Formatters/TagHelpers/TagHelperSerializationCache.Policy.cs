// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.Extensions.ObjectPool;

namespace Microsoft.AspNetCore.Razor.Serialization.MessagePack.Formatters.TagHelpers;

internal sealed partial class TagHelperSerializationCache
{
    private sealed class Policy : IPooledObjectPolicy<TagHelperSerializationCache>
    {
        public static readonly Policy Instance = new();

        private Policy()
        {
        }

        public TagHelperSerializationCache Create() => new();

        public bool Return(TagHelperSerializationCache cache)
        {
            if (cache._metadataMap is { } metadataMap)
            {
                metadataMap.Dispose();
                cache._metadataMap = null;
            }

            if (cache._stringMap is { } stringMap)
            {
                stringMap.Dispose();
                cache._stringMap = null;
            }

            return true;
        }
    }
}
