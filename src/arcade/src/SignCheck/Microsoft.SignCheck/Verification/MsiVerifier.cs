// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Deployment.WindowsInstaller;
using Microsoft.Deployment.WindowsInstaller.Package;
using Microsoft.SignCheck.Interop;
using Microsoft.SignCheck.Logging;

namespace Microsoft.SignCheck.Verification
{
    public class MsiVerifier : AuthentiCodeVerifier
    {
        public MsiVerifier(Log log, Exclusions exclusions, SignatureVerificationOptions options) : base(log, exclusions, options, ".msi")
        {

        }

        public override SignatureVerificationResult VerifySignature(string path, string parent, string virtualPath)
        {
            SignatureVerificationResult svr = base.VerifySignature(path, parent, virtualPath);

            if (VerifyRecursive)
            {
                CreateDirectory(svr.TempPath);

                // TODO: Fix for MSIs with external CABs that are not present.
                using (var installPackage = new InstallPackage(svr.FullPath, DatabaseOpenMode.Transact, sourceDir: null, workingDir: svr.TempPath))
                {
                    InstallPathMap files = installPackage.Files;
                    var originalFiles = new Dictionary<string, string>();

                    // Flatten the files to avoid path too long errors. We use the File column and extension to create a unique file
                    // and record the original, relative MSI path in the result.
                    foreach (string key in installPackage.Files.Keys)
                    {
                        originalFiles[key] = installPackage.Files[key].TargetPath;
                        string name = key + Path.GetExtension(installPackage.Files[key].TargetName);
                        string targetPath = Path.Combine(svr.TempPath, name);
                        installPackage.Files[key].TargetName = name;
                        installPackage.Files[key].SourceName = name;
                        installPackage.Files[key].SourcePath = targetPath;
                        installPackage.Files[key].TargetPath = targetPath;
                    }

                    try
                    {
                        Log.WriteMessage(LogVerbosity.Diagnostic, SignCheckResources.DiagExtractingFileContents, svr.TempPath);
                        installPackage.ExtractFiles(installPackage.Files.Keys);

                        foreach (string key in installPackage.Files.Keys)
                        {
                            SignatureVerificationResult packageFileResult = VerifyFile(installPackage.Files[key].TargetPath, svr.Filename, Path.Combine(svr.VirtualPath, originalFiles[key]), containerPath: null);
                            packageFileResult.AddDetail(DetailKeys.File, SignCheckResources.DetailFullName, originalFiles[key]);
                            svr.NestedResults.Add(packageFileResult);
                        }
                    }
                    catch (Exception e)
                    {
                        Log.WriteError(e.Message);
                    }
                }

                // Extract files from the Binary table - this is where items such as custom actions are stored.
                try
                {
                    using (var installDatabase = new Database(svr.FullPath, DatabaseOpenMode.ReadOnly))
                    using (View view = installDatabase.OpenView("SELECT `Name`, `Data` FROM `Binary`"))
                    {
                        view.Execute();

                        foreach (Record record in view)
                        {
                            string binaryFile = (string)record["Name"];
                            string binaryFilePath = Path.Combine(svr.TempPath, binaryFile);
                            StructuredStorage.SaveStream(record, svr.TempPath);
                            SignatureVerificationResult binaryStreamResult = VerifyFile(binaryFilePath, svr.Filename, Path.Combine(svr.VirtualPath, binaryFile), containerPath: null);
                            binaryStreamResult.AddDetail(DetailKeys.Misc, SignCheckResources.FileExtractedFromBinaryTable);
                            svr.NestedResults.Add(binaryStreamResult);
                            record.Close();
                        }
                    }
                }
                catch (Exception e)
                {
                    Log.WriteError(e.Message);
                }

                DeleteDirectory(svr.TempPath);
            }

            return svr;
        }
    }
}
