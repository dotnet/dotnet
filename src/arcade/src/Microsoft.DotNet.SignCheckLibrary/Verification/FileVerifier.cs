// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using Microsoft.SignCheck.Logging;

namespace Microsoft.SignCheck.Verification
{
    public class FileVerifier
    {
        /// <summary>
        /// A set of files and file patterns to exclude from verification.
        /// </summary>
        public Exclusions Exclusions
        {
            get;
            private set;
        }

        /// <summary>
        /// The file extension associated with the verifier.
        /// </summary>
        public string FileExtension
        {
            get;
            private set;
        }

        protected bool GenerateExclusion
        {
            get
            {
                return (Options & SignatureVerificationOptions.GenerateExclusion) == SignatureVerificationOptions.GenerateExclusion;
            }
        }

        /// <summary>
        /// The Log to use for writing output during verification.
        /// </summary>
        protected Log Log
        {
            get;
            private set;
        }

        /// <summary>
        ///
        /// </summary>
        public SignatureVerificationOptions Options
        {
            get;
            private set;
        }

        protected bool VerifyAuthenticodeTimestamps
        {
            get
            {
                return (Options & SignatureVerificationOptions.VerifyAuthentiCodeTimestamps) == SignatureVerificationOptions.VerifyAuthentiCodeTimestamps;
            }
        }

        protected bool VerifyJarSignatures
        {
            get
            {
                return (Options & SignatureVerificationOptions.VerifyJarSignatures) == SignatureVerificationOptions.VerifyJarSignatures;
            }
        }

        protected bool VerifyRecursive
        {
            get
            {
                return (Options & SignatureVerificationOptions.VerifyRecursive) == SignatureVerificationOptions.VerifyRecursive;
            }
        }

        protected bool VerifyStrongNameSignature
        {
            get
            {
                return (Options & SignatureVerificationOptions.VerifyStrongNameSignature) == SignatureVerificationOptions.VerifyStrongNameSignature;
            }
        }

        protected bool VerifyXmlSignatures
        {
            get
            {
                return (Options & SignatureVerificationOptions.VerifyXmlSignatures) == SignatureVerificationOptions.VerifyXmlSignatures;
            }
        }

        public FileVerifier()
        {

        }

        /// <summary>
        /// Create a new FileVerifier instance.
        /// </summary>
        /// <param name="log">The Log to use for writing output during verification.</param>
        /// <param name="exclusions">The set of exclusions to check to determine if a file is excluded from verification.</param>
        /// <param name="options"></param>
        /// <param name="fileExtension">The file extension associated with the FileVerifier, e.g. ".zip" or ".dll".</param>
        public FileVerifier(Log log, Exclusions exclusions, SignatureVerificationOptions options, string fileExtension)
        {
            Log = log ?? throw new ArgumentNullException("log");
            Exclusions = exclusions ?? throw new ArgumentNullException("exclusions");
            Options = options;
            FileExtension = fileExtension;
        }

        /// <summary>
        /// Retrieve a FileVerifier from the SignatureVerificationManager. The retrieval is based on the file extension. If there are no
        /// applicable verifiers, the verifier is assigned based on the file's header.
        /// </summary>
        /// <param name="path">The path of the file for which to retrieve a FileVerifier.</param>
        /// <returns>A FileVerifier that can be used to verify the signature of the specified file.</returns>
        protected FileVerifier GetFileVerifier(string path)
        {
            string extension = Path.GetExtension(path);
            FileVerifier fileVerifier = SignatureVerificationManager.GetFileVerifierByExtension(extension);

            if (fileVerifier == null)
            {
                fileVerifier = SignatureVerificationManager.GetFileVerifierByHeader(path);

                if (fileVerifier == null)
                {
                    return SignatureVerificationManager.UnsupportedFileVerifier;
                }
                else
                {
                    Log.WriteMessage(LogVerbosity.Detailed, SignCheckResources.DetailIdentifyByHeaderAsType, fileVerifier.FileExtension);
                }
            }

            return fileVerifier;
        }

        /// <summary>
        /// Verifies the signature of a file.
        /// </summary>
        /// <param name="path">The path of the file to verify</param>
        /// <param name="parent">The parent file of the file to verify or null if this is a top-level file.</param>
        /// <returns>A SignatureVerificationResult containing detail about the verification result.</returns>
        public virtual SignatureVerificationResult VerifySignature(string path, string parent, string virtualPath)
        {
            return SignatureVerificationResult.UnsupportedFileTypeResult(path, parent, virtualPath);
        }

        /// <summary>
        /// Verify the signature of a single file.
        /// </summary>
        /// <param name="path">The path of the file on disk to verify.</param>
        /// <param name="parent">The name of parent container, e.g. an MSI or VSIX. Can be null when there is no parent container.</param>
        /// <param name="containerPath">The path of the file in the container. This may differ from the path on disk as containers are flattened. It's
        /// primarily intended to help with exclusions and report more readable names.</param>
        /// <returns>The verification result.</returns>
        public SignatureVerificationResult VerifyFile(string path, string parent, string virtualPath, string containerPath)
        {
            Log.WriteMessage(LogVerbosity.Detailed, String.Format(SignCheckResources.ProcessingFile, Path.GetFileName(path), String.IsNullOrEmpty(parent) ? SignCheckResources.NA : parent));

            FileVerifier fileVerifier = GetFileVerifier(path);
            SignatureVerificationResult svr = fileVerifier.VerifySignature(path, parent, virtualPath);
            svr.IsDoNotSign = Exclusions.IsDoNotSign(path, parent, virtualPath, containerPath);

            if ((svr.IsDoNotSign) && (svr.IsSigned))
            {
                // Report errors if a DO-NOT-SIGN file is signed.
                svr.AddDetail(DetailKeys.Error, SignCheckResources.DetailDoNotSignFileSigned, svr.Filename);
            }

            if ((!svr.IsDoNotSign) && (!svr.IsSigned))
            {
                svr.IsExcluded = Exclusions.IsExcluded(path, parent, svr.VirtualPath, containerPath);

                if ((svr.IsExcluded))
                {
                    svr.AddDetail(DetailKeys.File, SignCheckResources.DetailExcluded);
                }
            }

            if (GenerateExclusion)
            {
                svr.ExclusionEntry = String.Join(";", String.Join("|", path, containerPath), parent, String.Empty);
                Log.WriteMessage(LogVerbosity.Diagnostic, SignCheckResources.DiagGenerateExclusion, svr.Filename, svr.ExclusionEntry);
            }

            // Include the full path for top-level files
            if (String.IsNullOrEmpty(parent))
            {
                svr.AddDetail(DetailKeys.File, SignCheckResources.DetailFullName, svr.FullPath);
            }

            if (!String.IsNullOrEmpty(virtualPath))
            {
                svr.AddDetail(DetailKeys.File, SignCheckResources.DetailVirtualPath, svr.VirtualPath);
            }

            return svr;
        }

        /// <summary>
        /// Create a directory using the specified path.
        /// </summary>
        /// <param name="path">The directory to create.</param>
        protected void CreateDirectory(string path)
        {
            Log.WriteMessage(LogVerbosity.Diagnostic, String.Format(SignCheckResources.DiagCreatingFolder, path));
            Directory.CreateDirectory(path);
        }

        /// <summary>
        /// Deletes the specified directory and all subdirectories.
        /// </summary>
        /// <param name="path">The directory to delete.</param>
        protected void DeleteDirectory(string path)
        {
            Log.WriteMessage(LogVerbosity.Diagnostic, String.Format(SignCheckResources.DiagDeletingFolder, path));

            if (Directory.Exists(path))
            {
                Directory.Delete(path, recursive: true);
            }
        }
    }
}
