// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.Build.Tasks.Deployment.ManifestUtilities;

namespace Microsoft.Deployment.Utilities
{
    /// <summary>
    /// Helper methods.
    /// </summary>
    internal sealed class AppMan
    {
        public delegate void LockedFileReporter(string filePath);
        public delegate void UpdateProgressEventHandler(object sender, UpdateProgressEventArgs args);

        public enum Action
        {
            /// <summary>
            /// The update-reference process is beginning
            /// </summary>
            Begin,

            /// <summary>
            /// A file has been discovered in the search path but is already referenced by the manifest
            /// </summary>
            AlreadyPresent,

            /// <summary>
            /// A reference has been added to the manifest
            /// </summary>
            Added,

            /// <summary>
            /// A reference has been updated (not currently supported)
            /// </summary>
            Updated,

            /// <summary>
            /// The file search phase is complete
            /// </summary>
            SearchComplete,

            /// <summary>
            /// Renaming a file.
            /// </summary>
            Renaming,

            /// <summary>
            /// The reference update phase is complete
            /// </summary>
            Complete
        }

        /// <summary>
        /// Simple class for update progress event arguments.
        /// </summary>
        public class UpdateProgressEventArgs : System.EventArgs
        {
            private readonly Action action;
            private readonly string path;
            private readonly ArrayList errors;

            public UpdateProgressEventArgs(Action action, string path)
            {
                this.action = action;
                this.path = path;
                errors = new ArrayList();
            }

            public UpdateProgressEventArgs(Action action, string path, ArrayList errors)
            {
                this.action = action;
                this.path = path;
                this.errors = errors;
            }

            public Action Action { get { return action; } }
            public string Path { get { return path; } }
            public ArrayList Errors { get { return errors; } }
        }

        /// <summary>
        /// Adds manifest references - a public method that calls into private recursive method
        /// that updates file and assembly references.
        /// </summary>
        /// <param name="manifest">Manifest whose references are to be updated</param>
        /// <param name="addDeploy">Specifies is '.deploy' should be appended</param>
        /// <param name="fromDirectory">Directory at which to begin the recursive search</param>
        /// <param name="filesToIgnore">Files that should not be included in the manifest</param>
        /// <param name="lockedFileReporter">Delegate via which locked files will be reported</param>
        /// <param name="sender">Sender</param>
        /// <param name="updateProgress">UpdateProgress event handler</param>
        /// <param name="overwrite">Overwrite event handler</param>
        /// <param name="errors">List of errors</param>
        public static void AddReferences(ApplicationManifest manifest,
            bool addDeploy,
            string fromDirectory, List<string> filesToIgnore,
            LockedFileReporter lockedFileReporter, object sender, UpdateProgressEventHandler updateProgress, OverwriteEventHandler overwrite, ArrayList errors)
        {
            if ((manifest == null) || (fromDirectory == null))
            {
                return;
            }

            // Strip a leading .\ from the path, if present
            if ((fromDirectory.Length >= 2) && fromDirectory.Substring(0, 2) == ".\\")
            {
                fromDirectory = fromDirectory.Substring(2);
            }

            // If stripping a leading .\ yields an empty string, use "."
            if (fromDirectory == "")
            {
                fromDirectory = ".";
            }

            // Append a trailing \ if necessary
            if (fromDirectory.LastIndexOf('\\') != fromDirectory.Length - 1)
            {
                fromDirectory += '\\';
            }

            // Add application manifest file to the ignore list
            string manifestName = (manifest.AssemblyIdentity.Name + ".manifest").ToLower();
            filesToIgnore.Add(manifestName);

            // We need to add the full path as well since the addDeploy
            // method is using the full filename for comparison.
            filesToIgnore.Add(manifest.SourcePath + manifestName);

            if (addDeploy)
            {
                filesToIgnore.Add(manifestName + ".deploy");
                filesToIgnore.Add(manifest.SourcePath + manifestName + ".deploy");
            }

            // Set the source path for the new references
            manifest.SourcePath = fromDirectory;

            updateProgress?.Invoke(sender, new UpdateProgressEventArgs(Action.Begin, ""));

            // Recursively search fromDirectory to get new references
            AddReferences(manifest, addDeploy, fromDirectory, fromDirectory, "", filesToIgnore, lockedFileReporter, sender, updateProgress, overwrite, errors);

            updateProgress?.Invoke(sender, new UpdateProgressEventArgs(Action.SearchComplete, ""));
        }

        public delegate bool OverwriteEventHandler(string fileName);

        /// <summary>
        /// Appends ".deploy" to all files
        /// </summary>
        /// <param name="filesToIgnore">List of files to ignore</param>
        /// <param name="files">Array of files</param>
        /// <param name="overwrite">Overwrite event handler</param>
        /// <param name="errors">List of errors</param>
        /// <returns>Arrays of updated file paths</returns>
        public static string[] AppendDeploy(List<string> filesToIgnore, string[] files, OverwriteEventHandler overwrite, ArrayList errors)
        {
            ArrayList deletedFiles = new ArrayList();
            string[] updatedPaths = new string[files.Length];
            for (int i = 0; i < files.Length; i++)
            {
                string fileName = files[i];
                if (fileName != null && File.Exists(fileName))
                {
                    if (!fileName.ToLower().EndsWith(".deploy") && !filesToIgnore.Contains(fileName.ToLower()))
                    {
                        try
                        {
                            string newPath = fileName + ".deploy";
                            if (File.Exists(newPath))
                            {
                                if (overwrite != null && overwrite(newPath))
                                {
                                    File.Delete(newPath);
                                    deletedFiles.Add(newPath);
                                }
                                else
                                {
                                    continue;
                                }
                            }

                            File.Move(fileName, newPath);
                            updatedPaths[i] = newPath;
                        }
                        catch (IOException)
                        {
                            errors.Add(fileName);
                            continue;
                        }
                    }
                    else
                    {
                        if (deletedFiles.Contains(fileName))
                        {
                            continue;
                        }
                        else
                        {
                            updatedPaths[i] = fileName;
                        }
                    }
                }
            }

            return updatedPaths;
        }

        /// <summary>
        /// Adds/updates references, using a breadth-first recursive descent model.
        /// </summary>
        /// <param name="manifest">Manifest whose references are to be updated</param>
        /// <param name="addDeploy">Specifies if '.deploy' should be appended</param>
        /// <param name="root">Directory where search began (does not change during descent)</param>
        /// <param name="searchDirectory">Directory to examine for references and subdirectories</param>
        /// <param name="relativePath">Path from origin directory (codeBase) to current directory</param>
        /// <param name="filesToIgnore">Files that should not be included in the manifest</param>
        /// <param name="lockedFileReporter">Delegate via which locked files will be reported</param>
        /// <param name="sender">Sender</param>
        /// <param name="updateProgress">UpdateProgress event handler</param>
        /// <param name="overwrite">Overwrite event handler</param>
        /// <param name="errors">List of errors</param>
        private static void AddReferences(ApplicationManifest manifest,
            bool addDeploy,
            string root, string searchDirectory, string relativePath,
            List<string> filesToIgnore,
            LockedFileReporter lockedFileReporter, object sender, UpdateProgressEventHandler updateProgress, OverwriteEventHandler overwrite, ArrayList errors)
        {
            if ((manifest == null) || (searchDirectory == null))
            {
                return;
            }

            // Process files in current directory
            string[] files;
            try
            {
                files = Directory.GetFiles(searchDirectory);
            }
            catch (System.UnauthorizedAccessException)
            {
                return;
            }

            if (addDeploy)
            {
                files = AppendDeploy(filesToIgnore, files, overwrite, errors);
            }

            bool launcherBasedDeployment = false;
            string launcherPath = addDeploy ? Path.Combine(root, LauncherUtil.LauncherFilename + ".deploy") : Path.Combine(root, LauncherUtil.LauncherFilename);
            if (File.Exists(launcherPath))
            {
                launcherBasedDeployment = true;
            }

            List<BaseReference> assembliesToRemove = new List<BaseReference>();
            List<BaseReference> filesToRemove = new List<BaseReference>();
            foreach (string filePath in files)
            {
                bool bRelativePath = false;
                // Generate codebase from filePath
                string codebase = filePath;
                if (codebase == null)
                {
                    // This could be true if we renamed a file
                    continue;
                }
                string extension = Path.GetExtension(codebase).ToLower();

                // Strip a leading .\ from the path, if present
                if ((codebase.Length >= 2) && codebase.Substring(0, 2) == ".\\")
                {
                    bRelativePath = true;
                    codebase = codebase.Substring(2);
                }

                // See if this file is in the ignore list.
                // Ignorelist of the .manifest file is using the fullpath at
                // the very beggining so if a -fd . is used it does not find it.
                if (filesToIgnore.Contains(codebase.ToLower()) || 
                        (bRelativePath && 
                        extension == ".manifest" && 
                        filesToIgnore.Contains(Path.GetFullPath(codebase).ToLower())))
                {
                    continue;
                }

                // Strip the root path from the filename, if present
                if (codebase.StartsWith(root))
                {
                    codebase = codebase.Substring(root.Length);
                }

                // See if this file is in the ignore list
                if (filesToIgnore.Contains(codebase.ToLower()) || 
                    Path.GetExtension(codebase).ToLower() == ".netmodule" ||
                    (Path.GetExtension(codebase).ToLower() == ".deploy") && Path.GetExtension(codebase.Substring(0, codebase.Length - 7)).ToLower() == ".netmodule")
                {
                    continue;
                }

                // Use the presence/absence of metadata to indicate whether 
                // the file is an assembly or just a regular sort of file.

                AssemblyIdentity assembly = null;

                // If this is a Launcher-based deployment, all files except Launcher should be added as simple files
                // Launcher-based deployments are used for .NET (Core) apps - assembly identity cannot be positively
                // obtained from all types of .NET (Core) assemblies, requiring us to use simple file references.
                if (string.Equals(filePath, launcherPath, StringComparison.OrdinalIgnoreCase) ||
                    !launcherBasedDeployment)
                {
                    try
                    {
                        assembly = AssemblyIdentity.FromFile(filePath);
                    }
                    catch (BadImageFormatException)
                    {
                        // The file does not have a manifest in it
                    }
                    catch (System.Net.WebException)
                    {
                        // Internet connection might not be available
                    }
                }

                bool isAssembly = (assembly != null);

                // Make sure the file isn't locked, print an error message if 
                // it is. Without this test, ManifestUtil will throw an 
                // exception later when it tries to compute the file's hash.
                try
                {
                    FileInfo f = new FileInfo(filePath);
                    Stream s = f.OpenRead();
                    s.Close();
                }
                catch (System.Exception)
                {
                    lockedFileReporter?.Invoke(filePath);
                    continue;
                }

                // Create a reference and add it to the appropriate collection.
                Action action = Action.AlreadyPresent;

                if (isAssembly)
                {
                    if (!CollectionContains(manifest.AssemblyReferences, codebase, assembliesToRemove))
                    {
                        AssemblyReference newref = new AssemblyReference
                        {
                            TargetPath = codebase
                        };

                        manifest.AssemblyReferences.Add(newref);
                        action = Action.Added;
                        
                        // Determine if this is the EntryPoint
                        string strippedPath = codebase;
                        if (strippedPath.ToLower().EndsWith(".deploy"))
                        {
                            strippedPath = strippedPath.Substring(0, codebase.Length - 7).ToLower();
                        }

                        if (manifest.EntryPoint != null &&
                            (String.Compare(manifest.EntryPoint.TargetPath, strippedPath, true, CultureInfo.InvariantCulture) == 0))
                        {
                            manifest.EntryPoint = newref;
                        }
                    }
                }
                else
                {
                    if (!CollectionContains(manifest.FileReferences, codebase, filesToRemove))
                    {
                        FileReference newref = new FileReference
                        {
                            TargetPath = codebase
                        };
                        manifest.FileReferences.Add(newref);
                        action = Action.Added;
                    }
                }

                if (updateProgress != null)
                {
                    updateProgress(sender, new UpdateProgressEventArgs(action, codebase));
                }
            }

            // Remove files that were replaced because of renames.
            foreach (BaseReference reference in assembliesToRemove)
            {
                manifest.AssemblyReferences.Remove(reference as AssemblyReference);
            }

            foreach (BaseReference reference in filesToRemove)
            {
                manifest.FileReferences.Remove(reference as FileReference);
            }

            // Descend to subfolders
            string[] subdirs = Directory.GetDirectories(searchDirectory);

            foreach (string eachSubdir in subdirs)
            {
                string subdir = Path.GetFileName(eachSubdir);

                string newRelativePath;

                if ((relativePath.Length == 0) || (relativePath == "."))
                {
                    newRelativePath = subdir;
                }
                else
                {
                    newRelativePath = relativePath + "\\" + subdir;
                }

                AddReferences(manifest, addDeploy, root, eachSubdir, newRelativePath, filesToIgnore, lockedFileReporter, sender, updateProgress, overwrite, errors);
            }
        }

        /// <summary>
        /// Returns true of the collection contains a reference of the given 
        /// target path
        /// </summary>
        /// <param name="collection">Collection</param>
        /// <param name="targetPath">Target path</param>
        /// <param name="renamed">List of renamed references</param>
        /// <returns></returns>
        private static bool CollectionContains(IEnumerable collection, string targetPath, List<BaseReference> renamed)
        {
            string strippedPath = null;
            if (targetPath.ToLower().EndsWith(".deploy") && renamed != null)
            {
                strippedPath = targetPath.Substring(0, targetPath.Length - 7).ToLower();
            }

            foreach (object obj in collection)
            {
                BaseReference reference = obj as BaseReference;
                if (reference == null || reference.TargetPath == null)
                {
                    continue;
                }

                if (strippedPath != null && reference.TargetPath.ToLower() == strippedPath)
                {
                    renamed.Add(reference);
                }


                if (reference.TargetPath.ToLower() == targetPath.ToLower())
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Use simple heuristics to (attempt to) set the entry point and config file references
        /// 
        /// Possible cases are:
        /// 
        ///   Only one file exe or config file exists, regardless of its name
        ///   DO set the reference
        /// 
        ///   One file exists with the expected name, and one or more of other names
        ///   DO set, using the reference with the expected name
        /// 
        ///   Two files exist with non-expected names
        ///   DO NOT set the reference
        /// 
        ///   Two files exist of the expected name (in different paths)
        ///   DO NOT set the reference
        ///
        /// </summary>
        /// <param name="manifest"></param>
        public static void SetSpecialFiles(ApplicationManifest manifest)
        {
            AssemblyReference matchedExistingEntryPoint = null;
            AssemblyReference firstEntryPoint = null;

            if (manifest == null)
            {
                return;
            }

            string searchForExe = "";

            if ((manifest != null) && (manifest.AssemblyIdentity != null) && (manifest.AssemblyIdentity.Name != null))
            {
                searchForExe = manifest.AssemblyIdentity.Name.ToLower();
                if (!searchForExe.EndsWith("exe"))
                    searchForExe += ".exe";
            }

            if (manifest.EntryPoint == null)
            {
                int matchesFound = 0;
                int numberOfExecutablesFound = 0;
                AssemblyReference defaultEntryPoint = null;

                // search for an executable and config file with the same name as the assembly identity
                foreach (AssemblyReference ar in manifest.AssemblyReferences)
                {
                    if (ar.TargetPath == null)
                    {
                        continue;
                    }

                    string targetpath = ar.TargetPath.ToLower();
                    if ((targetpath.EndsWith("exe") || targetpath.EndsWith("exe.deploy"))
                        && (!targetpath.EndsWith(".vshost.exe") && !targetpath.EndsWith(".vshost.exe.deploy")))
                    {
                        numberOfExecutablesFound++;

                        if (numberOfExecutablesFound == 1)
                        {

                            defaultEntryPoint = ar;
                            if (firstEntryPoint == null)
                                firstEntryPoint = ar;
                        }
                        else
                        {
                            defaultEntryPoint = null;
                        }

                        if (matchedExistingEntryPoint == null && manifest.EntryPoint != null && manifest.EntryPoint.AssemblyIdentity != null && manifest.EntryPoint.AssemblyIdentity.Name.ToLower() + ".exe" == targetpath)
                            matchedExistingEntryPoint = ar;
                    }

                    if (Path.GetFileName(ar.TargetPath).ToLower() == searchForExe)
                    {
                        manifest.EntryPoint = ar;
                        matchesFound++;
                    }
                }

                // If multiple files match the exe being searched for, do not set the entry point
                if (matchesFound > 1)
                {
                    manifest.EntryPoint = null;
                }

                // If there was only one exe found, do set the entry point
                if (numberOfExecutablesFound == 1)
                {
                    manifest.EntryPoint = defaultEntryPoint;
                }
                else
                {
                    manifest.EntryPoint = firstEntryPoint;
                }
            }

            // sets the first matching EntryPoint if any
            if (manifest.EntryPoint == null)
            {
                manifest.EntryPoint = matchedExistingEntryPoint;
            }

            // Config file searching is simpler, as the config file can only 
            // have the same path as the entry point, plus ".config"
            string searchForConfig = "";

            if (manifest.EntryPoint != null)
            {
                searchForConfig = (manifest.EntryPoint.TargetPath + ".config").ToLower();
            }

            if (((manifest.ConfigFile == null) || (manifest.ConfigFile == "")) && (searchForConfig.Length > 0))
            {
                // search for a config file with the same name as the assembly identity
                foreach (FileReference fr in manifest.FileReferences)
                {
                    if (fr.TargetPath == null)
                    {
                        continue;
                    }

                    if (Path.GetFileName(fr.TargetPath).ToLower() == searchForConfig)
                    {
                        manifest.ConfigFile = fr.TargetPath;
                        break;
                    }
                }
            }
        }


        /// <summary>
        /// Resolves paths for all references, returns false if nonexistent 
        /// references are encountered.
        /// </summary>
        /// <param name="manifest">The manifest whose references are to be resolved</param>
        /// <param name="basePath">Base path from which to find referenced files</param>
        /// <param name="targetFrameworkVersion">Target framework version.</param>
        /// <returns>True if all referenced files are found, false if any are nonexistent</returns>
        public static bool ResolveReferences(Manifest manifest, string basePath, string targetFrameworkVersion)
        {
            bool result = true;

            // This ensures that Path.Combine during the ResolveFiles call (below) won't cause an exception
            foreach (FileReference fileRef in manifest.FileReferences)
            {
                Utilities.AppMan.EnsurePath(fileRef);
                fileRef.SourcePath = Path.Combine(basePath, fileRef.SourcePath);
            }

            foreach (AssemblyReference asmRef in manifest.AssemblyReferences)
            {
                Utilities.AppMan.EnsurePath(asmRef);
                asmRef.SourcePath = Path.Combine(basePath, asmRef.SourcePath);
            }

            manifest.OutputMessages.Clear();

            try
            {
                // It would be nice to be able to pass a delegate here, to 
                // update the UI as the references are resolved and when
                // references to nonexistent files are encountered.
                manifest.ResolveFiles();
            }
            catch (ArgumentException)
            {
                result = false;
            }
            
            try
            {
                // Update file sizes and hashes
                manifest.UpdateFileInfo(targetFrameworkVersion);
            }
            catch (ArgumentException)
            {
                result = false;
            }

            if (manifest.OutputMessages.ErrorCount > 0)
                result = false;

            return result;
        }

        /// <summary>
        /// Updates information about files already referenced in the manifest
        ///
        /// Update any files that have matching codebases, don’t look for
        /// new files and delete missing files.
        ///
        /// Both the ResolveFiles() and UpdateFileInfo() methods are 
        /// likely to throw an exception if mage was run in a directory
        /// other than the directory in which the references were 
        /// initially created.  They are wrapped in separate try/catch
        /// blocks so that UpdateFileInfo() will get called even if
        /// ResolveFiles() throws.  Both functions only throw after
        /// attempting to operate on every file in the manifest, so
        /// it's possible for both to do some meaningful work and then
        /// throw.
        /// </summary>
        /// <param name="manifest">the manifest whose references are to be updated</param>
        /// <param name="basePath">base path from which to find referenced files and assemblies</param>
        /// <param name="sender">not implemented</param>
        /// <param name="updateProgress">not implemented</param>
        /// <param name="targetFrameworkVersion">used to determine the hashing alogrithm</param>
        public static void UpdateReferenceInfo(Manifest manifest, string basePath, object sender, UpdateProgressEventHandler updateProgress, string targetFrameworkVersion)
        {
            ResolveReferences(manifest, basePath, targetFrameworkVersion);

            // It would be nice to be able to pass a delegate here, to 
            // update the UI as the references are resolved and when
            // references to nonexistent files are encountered.
            manifest.UpdateFileInfo(targetFrameworkVersion);
        }

        /// <summary>
        /// This encapsulates the reference-updating methods, for use by a
        /// background thread.
        /// </summary>
        /// <param name="manifest">ApplicationManifest object</param>
        /// <param name="addDeploy">Indicates if we are adding .deploy extension.</param>
        /// <param name="fromDirectory">Directory from which to obtain references</param>
        /// <param name="filesToIgnore">List of files to ignore.</param>
        /// <param name="sender">Not used</param>
        /// <param name="updateProgress">Not used</param>
        public static void AddAndUpdateReferences(ApplicationManifest manifest, bool addDeploy, string fromDirectory,
            List<string> filesToIgnore, object sender, UpdateProgressEventHandler updateProgress, OverwriteEventHandler overwrite, string targetFrameworkVersion)
        {
            ArrayList errors = new ArrayList();
            Utilities.AppMan.AddReferences(manifest, addDeploy, fromDirectory, filesToIgnore, null, sender, updateProgress, overwrite, errors);

            Utilities.AppMan.SetSpecialFiles(manifest);

            Utilities.AppMan.UpdateReferenceInfo(manifest, fromDirectory, sender, updateProgress, targetFrameworkVersion);

            updateProgress?.Invoke(sender, new UpdateProgressEventArgs(Action.Complete, "", errors));
        }


        /// <summary>
        /// Ensure that neither SourcePath nor TargetPath is null
        /// </summary>
        /// <param name="reference">Reference object</param>
        public static void EnsurePath(BaseReference reference)
        {
            if (reference.SourcePath == null)
            {
                reference.SourcePath = reference.TargetPath;
            }

            if (reference.TargetPath == null)
            {
                reference.TargetPath = reference.SourcePath;
            }

            if (reference.TargetPath == null)
            {
                reference.TargetPath = reference.SourcePath = "";
            }
        }
    }
}