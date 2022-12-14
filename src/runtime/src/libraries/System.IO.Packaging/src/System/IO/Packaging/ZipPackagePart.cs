// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.IO.Compression;

namespace System.IO.Packaging
{
    /// <summary>
    /// This class represents a Part within a Zip container.
    /// This is a part of the Packaging Layer APIs.
    /// This implementation is specific to the Zip file format.
    /// </summary>
    public sealed class ZipPackagePart : PackagePart
    {
        #region Public Methods

        /// <summary>
        /// Custom Implementation for the GetStream Method
        /// </summary>
        /// <param name="streamFileMode">Mode in which the stream should be opened</param>
        /// <param name="streamFileAccess">Access with which the stream should be opened</param>
        /// <returns>Stream Corresponding to this part</returns>
        protected override Stream? GetStreamCore(FileMode streamFileMode, FileAccess streamFileAccess)
        {
            if (_zipArchiveEntry != null)
            {
                // Reset the stream when FileMode.Create is specified.  Since ZipArchiveEntry only
                // ever supports opening once when the backing archive is in Create mode, we'll avoid
                // calling SetLength since the stream returned won't be seekable. You could still open
                // an archive in Update mode then call part.GetStream(FileMode.Create), in which case
                // we'll want this call to SetLength.
                if (streamFileMode == FileMode.Create && _zipArchiveEntry.Archive.Mode != ZipArchiveMode.Create)
                {
                    using (var tempStream = _zipStreamManager.Open(_zipArchiveEntry, streamFileAccess))
                    {
                        tempStream.SetLength(0);
                    }
                }

                var stream = _zipStreamManager.Open(_zipArchiveEntry, streamFileAccess);
                return stream;
            }
            return null;
        }

        #endregion Public Methods

        #region Internal Constructors

        /// <summary>
        /// Constructs a ZipPackagePart for an atomic (i.e. non-interleaved) part.
        /// This is called from the ZipPackage class as a result of GetPartCore,
        /// GetPartsCore or CreatePartCore methods
        /// </summary>
        /// <param name="zipPackage"></param>
        /// <param name="zipArchive"></param>
        /// <param name="zipArchiveEntry"></param>
        /// <param name="zipStreamManager"></param>
        /// <param name="partUri"></param>
        /// <param name="compressionOption"></param>
        /// <param name="contentType"></param>
        internal ZipPackagePart(ZipPackage zipPackage,
            ZipArchive zipArchive,
            ZipArchiveEntry zipArchiveEntry,
            ZipStreamManager zipStreamManager,
            PackUriHelper.ValidatedPartUri partUri,
            string contentType,
            CompressionOption compressionOption)
            : base(zipPackage, partUri, contentType, compressionOption)
        {
            _zipPackage = zipPackage;
            _zipArchive = zipArchive;
            _zipStreamManager = zipStreamManager;
            _zipArchiveEntry = zipArchiveEntry;
        }

        #endregion Internal Constructors

        #region Internal Properties

        /// <summary>
        /// Obtain the ZipFileInfo descriptor of an atomic part.
        /// </summary>
        internal ZipArchiveEntry ZipArchiveEntry
        {
            get
            {
                return _zipArchiveEntry;
            }
        }

        #endregion Internal Properties

        #region Private Variables

        private readonly ZipPackage _zipPackage;
        private readonly ZipArchiveEntry _zipArchiveEntry;
        private readonly ZipArchive _zipArchive;
        private readonly ZipStreamManager _zipStreamManager;

        #endregion Private Variables
    }
}
