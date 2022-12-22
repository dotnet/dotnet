// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the License.txt file in the project root for more information.

using System;
using System.Diagnostics;
using System.IO;

namespace Microsoft.DiaSymReader
{
    public static class SymUnmanagedReaderFactory
    {
        /// <summary>
        /// Creates <see cref="ISymUnmanagedReader"/> instance and initializes it with specified PDB stream and metadata import object.
        /// </summary>
        /// <param name="pdbStream">Windows PDB stream.</param>
        /// <param name="metadataImport">IMetadataImport implementation.</param>
        /// <param name="options">Options.</param>
        /// <remarks>
        /// Tries to load the implementation of the PDB reader from Microsoft.DiaSymReader.Native.{platform}.dll library first.
        /// It searches for this library in the directory Microsoft.DiaSymReader.dll is loaded from, 
        /// the application directory, the %WinDir%\System32 directory, and user directories in the DLL search path, in this order.
        /// If not found in the above locations and <see cref="SymUnmanagedReaderCreationOptions.UseAlternativeLoadPath"/> option is specified
        /// the directory specified by MICROSOFT_DIASYMREADER_NATIVE_ALT_LOAD_PATH environment variable is also searched.
        /// If the Microsoft.DiaSymReader.Native.{platform}.dll library can't be found and <see cref="SymUnmanagedReaderCreationOptions.UseComRegistry"/> 
        /// option is specified checks if the PDB reader is available from a globally registered COM object. This COM object is provided 
        /// by .NET Framework and has limited functionality (up to <see cref="ISymUnmanagedReader3"/>).
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="pdbStream"/>is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="metadataImport"/>is null.</exception>
        /// <exception cref="DllNotFoundException">The SymReader implementation is not available or failed to load.</exception>
        /// <exception cref="NotSupportedException">The implementation doesn't support specified <genericparamref name="TSymUnmanagedReader"/> interface.</exception>
        public static TSymUnmanagedReader CreateReaderWithMetadataImport<TSymUnmanagedReader>(
            Stream pdbStream, 
            object metadataImport,
            SymUnmanagedReaderCreationOptions options = SymUnmanagedReaderCreationOptions.Default)
            where TSymUnmanagedReader : class, ISymUnmanagedReader3
        {
            if (pdbStream == null)
            {
                throw new ArgumentNullException(nameof(pdbStream));
            }

            if (metadataImport == null)
            {
                throw new ArgumentNullException(nameof(metadataImport));
            }

            object symReader = SymUnmanagedFactory.CreateObject(
                createReader: true, 
                useAlternativeLoadPath: (options & SymUnmanagedReaderCreationOptions.UseAlternativeLoadPath) != 0,
                useComRegistry: (options & SymUnmanagedReaderCreationOptions.UseComRegistry) != 0,
                moduleName: out var _, 
                loadException: out var loadException);

            if (symReader == null)
            {
                Debug.Assert(loadException != null);

                if (loadException is DllNotFoundException)
                {
                    throw loadException;
                }

                throw new DllNotFoundException(loadException.Message, loadException);
            }

            if (!(symReader is TSymUnmanagedReader symReader3))
            {
                throw new NotSupportedException();
            }

            symReader3.Initialize(pdbStream, metadataImport);
            return symReader3;
        }

        /// <summary>
        /// Creates <see cref="ISymUnmanagedReader"/> instance and initializes it with specified PDB stream and metadata provider.
        /// </summary>
        /// <param name="pdbStream">Windows PDB stream.</param>
        /// <param name="metadataProvider"><see cref="ISymReaderMetadataProvider"/> implementation.</param>
        /// <param name="options">Options.</param>
        /// <remarks>
        /// Tries to load the implementation of the PDB reader from Microsoft.DiaSymReader.Native.{platform}.dll library first.
        /// It searches for this library in the directory Microsoft.DiaSymReader.dll is loaded from, 
        /// the application directory, the %WinDir%\System32 directory, and user directories in the DLL search path, in this order.
        /// If not found in the above locations and <see cref="SymUnmanagedReaderCreationOptions.UseAlternativeLoadPath"/> option is specified
        /// the directory specified by MICROSOFT_DIASYMREADER_NATIVE_ALT_LOAD_PATH environment variable is also searched.
        /// If the Microsoft.DiaSymReader.Native.{platform}.dll library can't be found and <see cref="SymUnmanagedReaderCreationOptions.UseComRegistry"/> 
        /// option is specified checks if the PDB reader is available from a globally registered COM object. This COM object is provided 
        /// by .NET Framework and has limited functionality (up to <see cref="ISymUnmanagedReader3"/>).
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="pdbStream"/>is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="metadataProvider"/>is null.</exception>
        public static TSymUnmanagedReader CreateReader<TSymUnmanagedReader>(
            Stream pdbStream,
            ISymReaderMetadataProvider metadataProvider,
            SymUnmanagedReaderCreationOptions options = SymUnmanagedReaderCreationOptions.Default)
            where TSymUnmanagedReader : class, ISymUnmanagedReader3
        {
            if (metadataProvider == null)
            {
                throw new ArgumentNullException(nameof(metadataProvider));
            }

            return CreateReaderWithMetadataImport<TSymUnmanagedReader>(pdbStream, CreateSymReaderMetadataImport(metadataProvider), options);
        }

        /// <summary>
        /// Creates a minimal implementation of IMetadataImport required for reading PDBs based on given <see cref="ISymReaderMetadataProvider"/>.
        /// </summary>
        /// <param name="metadataProvider">Reads metadata.</param>
        /// <returns>An instance of IMetadataImport.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="metadataProvider"/>is null.</exception>
        public static object CreateSymReaderMetadataImport(ISymReaderMetadataProvider metadataProvider)
        {
            if (metadataProvider == null)
            {
                throw new ArgumentNullException(nameof(metadataProvider));
            }

            return new SymReaderMetadataAdapter(metadataProvider);
        }
    }
}
