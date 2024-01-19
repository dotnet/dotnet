// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#if !NET
using System.Collections.Generic;
#endif
using System.Collections.Immutable;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal sealed partial class DocumentVersionCache
{
    internal TestAccessor GetTestAccessor() => new(this);

    internal class TestAccessor(DocumentVersionCache @this)
    {
        private readonly DocumentVersionCache _this = @this;

        public record struct DocumentEntry(IDocumentSnapshot? Document, int Version);

        public ImmutableDictionary<string, ImmutableArray<DocumentEntry>> GetEntries()
        {
            using var result = new PooledDictionaryBuilder<string, ImmutableArray<DocumentEntry>>();
            using var _ = _this._lock.EnterReadLock();

            foreach (var (key, entries) in _this._documentLookup_NeedsLock)
            {
                using var versions = new PooledArrayBuilder<DocumentEntry>();

                foreach (var entry in entries)
                {
                    var document = entry.Document.TryGetTarget(out var target)
                        ? target
                        : null;

                    var version = entry.Version;

                    versions.Add(new(document, version));
                }

                result.Add(key, versions.ToImmutable());
            }

            return result.ToImmutable();
        }

        public void MarkAsLatestVersion(IDocumentSnapshot document)
        {
            using (_this._lock.EnterUpgradeableReadLock())
            {
                _this.MarkAsLatestVersion(document);
            }
        }
    }
}
