// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Microsoft.SignCheck.Interop.PortableExecutable;
using Microsoft.SignCheck.Logging;

namespace Microsoft.SignCheck.Verification
{
    public class SignatureVerificationManager
    {
        // Dictionary holding the known verifiers, indexed by file extension
        private static Dictionary<string, FileVerifier> _fileVerifiers = null;
        private static FileVerifier _unsupportedFileVerifier = new UnsupportedFileVerifier();
        private List<SignatureVerificationResult> _results;

        /// <summary>
        /// The Log instance to use for writing output.
        /// </summary>
        public Log Log
        {
            get;
            set;
        }

        /// <summary>
        /// A set of files to exclude from signature verification.
        /// </summary>
        public Exclusions Exclusions
        {
            get;
            private set;
        }

        /// <summary>
        /// The results after verifying all the files.
        /// </summary>
        public List<SignatureVerificationResult> Results
        {
            get
            {
                if (_results == null)
                {
                    _results = new List<SignatureVerificationResult>();
                }

                return _results;
            }
            private set
            {
                _results = value;
            }
        }

        public SignatureVerificationOptions Options
        {
            get;
            private set;
        }

        /// <summary>
        /// Controls the verbosity of the output written to the Log.
        /// 
        /// </summary>
        public LogVerbosity Verbosity
        {
            get;
            private set;
        }

        public static FileVerifier UnsupportedFileVerifier
        {
            get
            {
                return _unsupportedFileVerifier;
            }
        }

        public SignatureVerificationManager(Exclusions exclusions, Log log, SignatureVerificationOptions options)
        {
            Exclusions = exclusions;
            Log = log;
            Options = options;

#if NETFRAMEWORK
            AddFileVerifier(new AuthentiCodeVerifier(log, exclusions, options, ".js"));
            AddFileVerifier(new AuthentiCodeVerifier(log, exclusions, options, ".psd1"));
            AddFileVerifier(new AuthentiCodeVerifier(log, exclusions, options, ".psm1"));
            AddFileVerifier(new AuthentiCodeVerifier(log, exclusions, options, ".ps1"));
            AddFileVerifier(new AuthentiCodeVerifier(log, exclusions, options, ".ps1xml"));
            AddFileVerifier(new CabVerifier(log, exclusions, options, ".cab"));
            AddFileVerifier(new JarVerifier(log, exclusions, options));
            AddFileVerifier(new MsiVerifier(log, exclusions, options));
            AddFileVerifier(new MspVerifier(log, exclusions, options));
            AddFileVerifier(new MsuVerifier(log, exclusions, options));
            AddFileVerifier(new VsixVerifier(log, exclusions, options));
#else
            AddFileVerifier(new DebVerifier(log, exclusions, options));
            AddFileVerifier(new MachOVerifier(log, exclusions, options, ".dylib"));
            AddFileVerifier(new MachOVerifier(log, exclusions, options, ".macho"));
            AddFileVerifier(new MachOVerifier(log, exclusions, options, ".so"));
            AddFileVerifier(new MachOVerifier(log, exclusions, options, ".a"));
            AddFileVerifier(new PkgVerifier(log, exclusions, options, ".pkg"));
            AddFileVerifier(new PkgVerifier(log, exclusions, options, ".app"));
            AddFileVerifier(new TarVerifier(log, exclusions, options, ".tar"));
            AddFileVerifier(new TarVerifier(log, exclusions, options, ".tgz"));
            AddFileVerifier(new TarVerifier(log, exclusions, options, ".gz"));
            AddFileVerifier(new RpmVerifier(log, exclusions, options));
#endif
            AddFileVerifier(new ExeVerifier(log, exclusions, options, ".exe"));
            AddFileVerifier(new LzmaVerifier(log, exclusions, options));
            AddFileVerifier(new NupkgVerifier(log, exclusions, options));
            AddFileVerifier(new PortableExecutableVerifier(log, exclusions, options, ".dll"));
            AddFileVerifier(new XmlVerifier(log, exclusions, options));
            AddFileVerifier(new ZipVerifier(log, exclusions, options));
        }

        /// <summary>
        /// Verify the signatures of a set of files.
        /// </summary>
        /// <param name="files">A set of files to verify.</param>
        /// <returns>An IEnumerable containing the verification results of each file.</returns>
        public IEnumerable<SignatureVerificationResult> VerifyFiles(IEnumerable<string> files)
        {
            foreach (string file in files)
            {
                FileVerifier fileVerifier = GetFileVerifier(file);
                SignatureVerificationResult result;
                result = fileVerifier.VerifySignature(file, parent: null, virtualPath: Path.GetFileName(file));

                if ((Options & SignatureVerificationOptions.GenerateExclusion) == SignatureVerificationOptions.GenerateExclusion)
                {
                    result.ExclusionEntry = String.Join(";", String.Join("|", file, String.Empty), String.Empty, String.Empty);
                    Log.WriteMessage(LogVerbosity.Diagnostic, SignCheckResources.DiagGenerateExclusion, result.Filename, result.ExclusionEntry);
                }

                result.IsDoNotSign = Exclusions.IsDoNotSign(file, parent: null, virtualPath: null, containerPath: null);

                if ((result.IsDoNotSign) && (result.IsSigned))
                {
                    // Report errors if a DO-NOT-SIGN file is signed.
                    result.AddDetail(DetailKeys.Error, SignCheckResources.DetailDoNotSignFileSigned, result.Filename);
                }

                if ((!result.IsDoNotSign) && (!result.IsSigned))
                {
                    result.IsExcluded = Exclusions.IsExcluded(file, parent: null, virtualPath: null, containerPath: null);

                    if ((result.IsExcluded))
                    {
                        result.AddDetail(DetailKeys.File, SignCheckResources.DetailExcluded);
                    }
                }

                Results.Add(result);
            }

            return Results;
        }

        /// <summary>
        /// Adds a new FileVerifier to the manager that can be used to validate a file based on
        /// </summary>
        /// <param name="fileVerifier">The FileVerifier to add.</param>
        public static void AddFileVerifier(FileVerifier fileVerifier)
        {
            if (fileVerifier == null)
            {
                throw new ArgumentNullException("fileVerifier");
            }

            // Let the dictionary throw if we have a duplicate file extension
            _fileVerifiers.Add(fileVerifier.FileExtension, fileVerifier);
        }

        /// <summary>
        /// Retrieves a FileVerifier for the specified file. If the file's extension is unknown
        /// a FileVerifier can be assigned based on the file's header.
        /// </summary>
        /// <param name="path">The path of the file.</param>
        /// <returns>A FileVerifier that can verify the file or null if verifier could be found.</returns>
        public static FileVerifier GetFileVerifier(string path)
        {
            string extension = Path.GetExtension(path);
            FileVerifier fileVerifier = GetFileVerifierByExtension(extension);

            if (fileVerifier == null)
            {
                fileVerifier = GetFileVerifierByHeader(path);

                if (fileVerifier == null)
                {
                    return _unsupportedFileVerifier;
                }
            }

            return fileVerifier;
        }

        /// <summary>
        /// Retrieves a FileVerifier by looking at extension of the file.
        /// </summary>
        /// <param name="path">The path of the file.</param>
        /// <returns>A FileVerifier that can verify the file or null if the verifier could not be found.</returns>
        public static FileVerifier GetFileVerifierByExtension(string extension)
        {
            if (!String.IsNullOrEmpty(extension))
            {
                // If the file has an extension, try to find a matching verifier
                if (_fileVerifiers.ContainsKey(extension))
                {
                    return _fileVerifiers[extension];
                }
            }

            return null;
        }

        /// <summary>
        /// Retrieves a FileVerifier by looking at the header of the file to determine its type to assign an extension to it.
        /// </summary>
        /// <param name="path">The path of the file.</param>
        /// <returns>A FileVerifier that can verify the file or null if the verifier could not be found.</returns>
        public static FileVerifier GetFileVerifierByHeader(string path)
        {
            if (String.IsNullOrEmpty(path))
            {
                throw new ArgumentException(String.Format(SignCheckResources.ArgumentNullOrEmpty, "path"));
            }

            FileVerifier fileVerifier = null;

            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (var reader = new BinaryReader(stream))
            {
                // Test for 4-byte magic numbers
                if (stream.Length > 4)
                {
                    uint magic4 = reader.ReadUInt32();
                    if (magic4 == FileHeaders.Zip)
                    {
                        using (ZipArchive zipArchive = ZipFile.OpenRead(path))
                        {
                            if (zipArchive.Entries.Any(z => String.Equals(Path.GetExtension(z.FullName), "nuspec", StringComparison.OrdinalIgnoreCase)))
                            {
                                // NUPKGs use .zip format, but should have a .nuspec files inside
                                fileVerifier = GetFileVerifierByExtension(".nupkg");
                            }
                            else if (zipArchive.Entries.Any(z => String.Equals(Path.GetExtension(z.FullName), "vsixmanifest", StringComparison.OrdinalIgnoreCase)))
                            {
                                // If it's an SDK based VSIX there should be a vsixmanifest file
                                fileVerifier = GetFileVerifierByExtension(".vsix");
                            }
                            else if (zipArchive.Entries.Any(z => String.Equals(z.FullName, "META-INF/MANIFEST.MF", StringComparison.OrdinalIgnoreCase)))
                            {
                                // Zip file with META-INF/MANIFEST.MF file is likely a JAR
                                fileVerifier = GetFileVerifierByExtension(".jar");
                            }
                            else
                            {
                                fileVerifier = GetFileVerifierByExtension(".zip");
                            }
                        }
                    }
                    else if (magic4 == FileHeaders.Cab)
                    {
                        fileVerifier = GetFileVerifierByExtension(".cab");
                    }
                    else if (magic4 == FileHeaders.MachO32 || magic4 == FileHeaders.MachO64)
                    {
                        // Use the ".macho" extension as a placeholder for Mach-O files
                        fileVerifier = GetFileVerifierByExtension(".macho");
                    }
                }
#if NETFRAMEWORK
                reader.BaseStream.Seek(0, SeekOrigin.Begin);
                if (stream.Length > 2)
                {
                    UInt16 magic2 = reader.ReadUInt16();
                    if (magic2 == FileHeaders.Dos)
                    {
                        PortableExecutableHeader pe = new PortableExecutableHeader(path);

                        if ((pe.FileHeader.Characteristics & ImageFileCharacteristics.IMAGE_FILE_DLL) != 0)
                        {
                            fileVerifier = GetFileVerifierByExtension(".dll");
                        }
                        else if ((pe.FileHeader.Characteristics & ImageFileCharacteristics.IMAGE_FILE_EXECUTABLE_IMAGE) != 0)
                        {
                            fileVerifier = GetFileVerifierByExtension(".exe");
                        }
                    }
                }
#endif
            }

            return fileVerifier;
        }

        static SignatureVerificationManager()
        {
            _fileVerifiers = new Dictionary<string, FileVerifier>();
        }
    }
}
