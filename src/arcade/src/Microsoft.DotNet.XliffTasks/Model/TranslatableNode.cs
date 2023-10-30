// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace XliffTasks.Model
{
    internal abstract class TranslatableNode
    {
        protected TranslatableNode(string id, string source, string note)
        {
            Validation.ThrowIfNullOrEmpty(id, nameof(id));
            Validation.ThrowIfNullOrEmpty(source, nameof(source));

            Id = id;
            Source = source;
            Note = note;
        }

        /// <summary>
        /// The unique ID of the node within a translatable document.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// The original text of the node before any translation.
        /// </summary>
        public string Source { get; }

        /// <summary>
        /// A comment associated with the node.
        /// Null if this node cannot have comments, empty if it has none.
        /// </summary>
        public string Note { get; }

        /// <summary>
        /// Mutates the parent document such that subsequent calls to <see cref="TranslatableDocument.Save(string)" />
        /// will replace <see cref="Source"/> with <paramref name="translation"/> in this node.
        /// <summary>
        public abstract void Translate(string translation);
    }
}
