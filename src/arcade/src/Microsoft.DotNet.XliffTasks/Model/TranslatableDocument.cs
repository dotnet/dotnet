// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace XliffTasks.Model
{
    /// <summary>
    /// Represents a document that can be translated by applying substitutions to nodes.
    /// </summary>
    internal abstract class TranslatableDocument : Document
    {
        /// <summary>
        /// The nodes of the document that can have substitutions.
        /// </summary>
        public IReadOnlyList<TranslatableNode> Nodes { get; private set; } = Array.Empty<TranslatableNode>();

        /// <summary>
        /// Indicates if content has been loaded in to the document.
        /// </summary>
        public override bool HasContent => _hasContent;
        private bool _hasContent;

        /// <summary>
        /// Loads (or reloads) the document content from the given reader.
        /// </summary>
        public sealed override void Load(TextReader reader)
        {
            LoadCore(reader);
            Nodes = GetTranslatableNodes().ToList().AsReadOnly();
            _hasContent = true;
        }

        /// <summary>
        /// Saves the document's content to the given file path.
        /// </summary>
        public sealed override void Save(TextWriter writer)
        {
            EnsureContent();
            SaveCore(writer);
        }

        /// <summary>
        /// Applies the given translations to the document. 
        /// Keys align with <see cref="TranslatableNode.Id"/>.
        /// Values indicate the text with which to replace <see cref="TranslatableNode.Source"/>.
        /// </summary>
        public void Translate(IReadOnlyDictionary<string, string> translations)
        {
            EnsureContent();

            foreach (TranslatableNode node in Nodes)
            {
                if (translations.TryGetValue(node.Id, out string translation))
                {
                    node.Translate(translation);
                }
            }
       }

        // rewrite nodes that point to external files (used often for icons, etc.)
        // these will have relative paths adjusted to absolute path.
        public virtual void RewriteRelativePathsToAbsolute(string sourceFullPath)
        {
        }

        protected abstract void LoadCore(TextReader reader);

        protected abstract void SaveCore(TextWriter writer);

        protected abstract IEnumerable<TranslatableNode> GetTranslatableNodes();
    }
}
