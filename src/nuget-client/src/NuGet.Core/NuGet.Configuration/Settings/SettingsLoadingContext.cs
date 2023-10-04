// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.IO;
using NuGet.Common;

namespace NuGet.Configuration
{
    /// <summary>
    /// Represents a cache context based on file paths when loading <see cref="SettingsFile" /> objects so that they are only read once.
    /// If the file changes on disk, it is not reloaded.
    /// </summary>
    public sealed class SettingsLoadingContext : IDisposable
    {
        private readonly ConcurrentDictionary<string, Lazy<SettingsFile>> _cache = new ConcurrentDictionary<string, Lazy<SettingsFile>>(PathUtility.GetStringComparerBasedOnOS());

        private bool _isDisposed;

        /// <summary>
        /// Occurs when a file is read.
        /// </summary>
        internal event EventHandler<string> FileRead;

        /// <inheritdoc cref="IDisposable.Dispose" />
        public void Dispose()
        {
            if (!_isDisposed)
            {
                _isDisposed = true;

                _cache.Clear();
            }

            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Gets or creates a settings file for the specified path.
        /// </summary>
        /// <param name="filePath">The file path to create a <see cref="SettingsFile" /> object for.</param>
        /// <param name="isMachineWide">An optional value indicating whether or not the settings file is machine-wide.</param>
        /// <param name="isReadOnly">An optional value indicating whether or not the settings file is read-only.</param>
        /// <returns></returns>
        /// <exception cref="ObjectDisposedException">When the current object has been disposed.</exception>
        /// <exception cref="ArgumentNullException">When <paramref name="filePath" /> is <see langword="null" />.</exception>
        internal SettingsFile GetOrCreateSettingsFile(string filePath, bool isMachineWide = false, bool isReadOnly = false)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(nameof(SettingsLoadingContext));
            }

            if (filePath == null)
            {
                throw new ArgumentNullException(nameof(filePath));
            }

            if (!Path.IsPathRooted(filePath))
            {
                throw new ArgumentException("SettingsLoadingContext requires a rooted path", nameof(filePath));
            }

            Lazy<SettingsFile> settingsLazy = _cache.GetOrAdd(
                filePath,
                (key) => new Lazy<SettingsFile>(() =>
                {
                    var fileInfo = new FileInfo(key);

                    // Load the settings file, this will throw an exception if something is wrong with the file
                    var settingsFile = new SettingsFile(fileInfo.DirectoryName, fileInfo.Name, isMachineWide, isReadOnly);

                    // Fire the FileRead event so unit tests can detect when a file was actually read versus cached
                    FileRead?.Invoke(this, fileInfo.FullName);

                    return settingsFile;
                }));

            return settingsLazy.Value;
        }
    }
}
