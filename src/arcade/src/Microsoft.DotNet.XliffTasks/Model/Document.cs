// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using System.Text;

namespace XliffTasks.Model
{
    internal abstract class Document
    {
        /// <summary>
        /// Indicates if content has been loaded in to the document.
        /// </summary>
        public abstract bool HasContent { get; }

        private static readonly Encoding s_utf8WithBom = new UTF8Encoding(encoderShouldEmitUTF8Identifier: true);
        private static readonly Encoding s_utf8WithoutBom = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);

        /// <summary>
        /// The encoding to use when saving the document to a stream.
        /// This is the encoding that was detected on Load, or default value of UTF8-with-BOM for new documents.
        /// </summary>
        public Encoding Encoding { get; set; } = s_utf8WithBom;

        /// <summary>
        /// Loads (or reloads) the document content from the given file path.
        /// </summary>
        public void Load(string path)
        {
            using FileStream stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            Load(stream);
        }

        /// <summary>
        /// Loads (or reloads) the document content from the given stream.
        /// </summary>
        public void Load(Stream stream)
        {
            // NOTE: It is important to pass UTF8-without-BOM here and not UTF8-with-BOM (aka Encoding.UTF8 and also same
            // as default when not passing encoding). The reason is that CurrentEncoding is only different from the encoding 
            // provided when the StreamReader encounters a BOM. As such, if we start off with UTF8-with-BOM, we'll end up with 
            // UTF8-with-BOM even if the document has no BOM, which would defeat our purpose of preserving the encoding and BOM
            // of the original document in a Load/Modify/Save cycle.

            using StreamReader reader = new(stream, s_utf8WithoutBom, detectEncodingFromByteOrderMarks: true);
            Load(reader);
            Encoding = reader.CurrentEncoding;
        }

        /// <summary>
        /// Loads (or reloads) the document content from the given reader.
        /// </summary>
        public abstract void Load(TextReader reader);

        /// <summary>
        /// Saves the document's content to the given file path.
        /// </summary>
        public void Save(string path)
        {
            //On Windows:
            // Readers will prevent the file from being overwritten due to FileShare.Read.
            // Readers can read in parallel, but when there's contention with a writer, the retrying will kick in to resolve it.
            //On Unix:
            // FileShare.Read does nothing, but...
            // File.Replace is implemented with rename system call that will mean that even though writers can overwrite while 
            // reading is happening, each reader will see file before or after overwrite, not in between

            EnsureContent();
            string tempPath = Path.Combine(Path.GetDirectoryName(path), Path.GetRandomFileName());

            using (FileStream stream = File.Open(tempPath, FileMode.Create, FileAccess.ReadWrite, FileShare.None))
            {
                Save(stream);
            }

            ExponentialRetry.ExecuteWithRetryOnIOException(() =>
            {
                if (File.Exists(path))
                {
                    File.Replace(
                        sourceFileName: tempPath,
                        destinationFileName: path,
                        destinationBackupFileName: null,
                        ignoreMetadataErrors: true);
                }
                else
                {
                    File.Move(sourceFileName: tempPath, destFileName: path);
                }
            }, maxRetryCount: 3);
        }

        /// <summary>
        /// Saves the document's content to the given stream.
        /// </summary>
        public void Save(Stream stream)
        {
            EnsureContent();

            using StreamWriter writer = new(stream, Encoding);
            Save(writer);
        }

        /// <summary>
        /// Saves the document's content to the given writer.
        /// </summary>
        public abstract void Save(TextWriter writer);

        /// <summary>
        /// Throws if this document has no content.
        /// </summary>
        /// <exception cref="InvalidOperationException"><see cref="HasContent"/> is false.</exception>
        protected void EnsureContent()
        {
            if (!HasContent)
            {
                throw new InvalidOperationException("Document has no content loaded.");
            }
        }
    }
}
