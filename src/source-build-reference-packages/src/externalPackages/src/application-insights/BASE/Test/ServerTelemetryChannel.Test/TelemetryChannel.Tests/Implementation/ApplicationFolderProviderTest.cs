namespace Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation
{
    using System.Collections;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Security.AccessControl;
    using System.Security.Principal;
    using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Helpers;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ApplicationFolderProviderTest
    {
        private const string NonWindowsStorageProbePathVarTmp = "/var/tmp/";
        private const string NonWindowsStorageProbePathTmp = "/tmp/";

        private DirectoryInfo testDirectory;

        [TestInitialize]
        public void TestInitialize()
        {
            this.testDirectory = new DirectoryInfo(Path.Combine(Path.GetTempPath(), Path.GetRandomFileName()));
        }

        [TestCleanup]
        public void TestCleanup()
        {
            if (this.testDirectory.Exists)
            {
                this.testDirectory.Delete(true);
            }
        }

        [TestMethod]
        public void GetApplicationFolderReturnsValidPlatformFolder()
        {
            IApplicationFolderProvider provider = new ApplicationFolderProvider();
            IPlatformFolder applicationFolder = provider.GetApplicationFolder();
            Assert.IsNotNull(applicationFolder);
        }

        [TestMethod]
        [TestCategory("WindowsOnly")]
        public void GetApplicationFolderReturnsSubfolderFromLocalAppDataFolder()
        {
            DirectoryInfo localAppData = this.CreateTestDirectory(@"AppData\Local");
            var environmentVariables = new Hashtable { { "LOCALAPPDATA", localAppData.FullName } };
            var provider = new ApplicationFolderProvider(environmentVariables);

            IPlatformFolder applicationFolder = provider.GetApplicationFolder();

            Assert.IsNotNull(applicationFolder);
            Assert.AreEqual(1, localAppData.GetDirectories().Length);

            localAppData.Delete(true);
        }

        [TestCategory("WindowsOnly")]
        [TestMethod]
        public void GetApplicationFolderReturnsCustomFolderWhenConfiguredAndExists()
        {
            DirectoryInfo localAppData = this.CreateTestDirectory(@"AppData\Local");
            DirectoryInfo customFolder = this.CreateTestDirectory(@"Custom");

            try
            {
                var environmentVariables = new Hashtable { { "LOCALAPPDATA", localAppData.FullName } };
                var provider = new ApplicationFolderProvider(environmentVariables, customFolder.FullName);
                IPlatformFolder applicationFolder = provider.GetApplicationFolder();
                Assert.IsNotNull(applicationFolder);
                Assert.AreEqual(customFolder.Name, ((PlatformFolder)applicationFolder).Folder.Name, "Configured custom folder should be used when available.");
            }
            finally
            {
                DeleteIfExists(localAppData);
                DeleteIfExists(customFolder);
            }
        }

        [TestMethod]
        [TestCategory("WindowsOnly")]
        public void GetApplicationFolderReturnsNullWhenCustomFolderConfiguredAndNotExists()
        {
            DirectoryInfo localAppData = this.CreateTestDirectory(@"AppData\Local");
            DirectoryInfo temp = this.CreateTestDirectory(@"AppData\Temp");
            DirectoryInfo customFolder = new DirectoryInfo(@"Custom");
            Assert.IsFalse(customFolder.Exists);
            try
            {
                var environmentVariables = new Hashtable();
                environmentVariables.Add("LOCALAPPDATA", localAppData.FullName);
                environmentVariables.Add("TEMP", localAppData.FullName);
                var provider = new ApplicationFolderProvider(environmentVariables, customFolder.FullName);
                IPlatformFolder applicationFolder = provider.GetApplicationFolder();

                // VALIDATE
                // That the applicationfolder returned is null.
                Assert.IsNull(applicationFolder);

                // Also validate that SDK does not
                // create the customFolder.
                Assert.IsFalse(customFolder.Exists);
            }
            finally
            {
                DeleteIfExists(localAppData);
                DeleteIfExists(temp);
                DeleteIfExists(customFolder);
            }
        }

        [TestMethod]
        [TestCategory("WindowsOnly")]
        public void GetApplicationFolderReturnsSubfolderFromCustomFolderFirst()
        {
            DirectoryInfo localAppData = this.CreateTestDirectory(@"AppData\Local");
            DirectoryInfo customFolder = this.CreateTestDirectory(@"Custom");

            var environmentVariables = new Hashtable { { "LOCALAPPDATA", localAppData.FullName } };
            var provider = new ApplicationFolderProvider(environmentVariables, customFolder.FullName);

            IPlatformFolder applicationFolder = provider.GetApplicationFolder();

            Assert.IsNotNull(applicationFolder);
            Assert.AreEqual(((PlatformFolder)applicationFolder).Folder.Name, customFolder.Name, "Sub-folder for custom folder should not be created.");

            localAppData.Delete(true);
            customFolder.Delete(true);
        }

        [TestMethod]
        [TestCategory("WindowsOnly")]
        public void GetApplicationFolderReturnsSubfolderFromTempFolderIfLocalAppDataIsNotAvailable()
        {
            DirectoryInfo temp = this.CreateTestDirectory("Temp");
            var environmentVariables = new Hashtable { { "TEMP", temp.FullName } };
            var provider = new ApplicationFolderProvider(environmentVariables);

            IPlatformFolder applicationFolder = provider.GetApplicationFolder();

            Assert.IsNotNull(applicationFolder);
            Assert.AreEqual(1, temp.GetDirectories().Length);

            temp.Delete(true);
        }

        [TestMethod]
        [TestCategory("WindowsOnly")]
        public void GetApplicationFolderReturnsSubfolderFromTempFolderIfLocalAppDataIsAvailableButNotAccessible()
        {
            DirectoryInfo localAppData = this.CreateTestDirectory(@"AppData\Local", FileSystemRights.CreateDirectories, AccessControlType.Deny);
            DirectoryInfo temp = this.CreateTestDirectory("Temp");
            var environmentVariables = new Hashtable
            {
                { "LOCALAPPDATA", localAppData.FullName },
                { "TEMP", temp.FullName },
            };
            var provider = new ApplicationFolderProvider(environmentVariables);

            IPlatformFolder applicationFolder = provider.GetApplicationFolder();

            Assert.IsNotNull(applicationFolder);
            Assert.AreEqual(1, temp.GetDirectories().Length);

            localAppData.Delete(true);
            temp.Delete(true);
        }

        [TestMethod]
        [TestCategory("WindowsOnly")]
        public void GetApplicationFolderReturnsSubfolderFromTempFolderIfLocalAppDataIsInvalid()
        {
            DirectoryInfo temp = this.CreateTestDirectory("Temp");
            var environmentVariables = new Hashtable
            {
                { "LOCALAPPDATA", " " },
                { "TEMP", temp.FullName },
            };
            var provider = new ApplicationFolderProvider(environmentVariables);

            IPlatformFolder applicationFolder = provider.GetApplicationFolder();

            Assert.IsNotNull(applicationFolder);
            Assert.AreEqual(1, temp.GetDirectories().Length);

            temp.Delete(true);
        }

        [TestMethod]
        [TestCategory("WindowsOnly")]
        public void GetApplicationFolderReturnsSubfolderFromTempFolderIfLocalAppDataIsUnmappedDrive()
        {
            DirectoryInfo temp = this.CreateTestDirectory("Temp");
            var environmentVariables = new Hashtable
            {
                { "LOCALAPPDATA", @"L:\Temp" },
                { "TEMP", temp.FullName },
            };
            var provider = new ApplicationFolderProvider(environmentVariables);

            IPlatformFolder applicationFolder = provider.GetApplicationFolder();

            Assert.IsNotNull(applicationFolder);
            Assert.AreEqual(1, temp.GetDirectories().Length);

            temp.Delete(true);
        }

        [TestMethod]
        [TestCategory("WindowsOnly")]
        public void GetApplicationFolderReturnsSubfolderFromTempFolderIfLocalAppDataIsTooLong()
        {
            // For reasons i don't understand, DotNetCore is able to create longer directory names. I've increased the length of this test path to guarantee that it fails.
            string longDirectoryName = Path.Combine(this.testDirectory.FullName, new string('A', 300)); //Windows has a max directory length of 248 characters.
            DirectoryInfo temp = this.CreateTestDirectory("Temp");

            // Initialize ApplicationfolderProvider
            var environmentVariables = new Hashtable
            {
                { "LOCALAPPDATA", longDirectoryName },
                { "TEMP", temp.FullName },
            };
            var provider = new ApplicationFolderProvider(environmentVariables);
            IPlatformFolder applicationFolder = provider.GetApplicationFolder();

            // Evaluate
            Assert.IsNotNull(applicationFolder);
            Assert.IsFalse(Directory.Exists(longDirectoryName), "TEST ERROR: This directory should not be created.");
            Assert.IsTrue(Directory.Exists(temp.FullName), "TEST ERROR: This directory should be created.");
            Assert.AreEqual(1, temp.GetDirectories().Length, "TEST FAIL: TEMP subdirectories were not created");

            temp.Delete(true);
        }

        [TestMethod]
        [TestCategory("WindowsOnly")]
        public void GetApplicationFolderReturnsNullWhenNeitherLocalAppDataNorTempFoldersAreAccessible()
        {
            var environmentVariables = new Hashtable
            {
                { "LOCALAPPDATA", this.CreateTestDirectory(@"AppData\Local", FileSystemRights.CreateDirectories, AccessControlType.Deny).FullName },
                { "TEMP", this.CreateTestDirectory("Temp", FileSystemRights.CreateDirectories, AccessControlType.Deny).FullName },
            };
            var provider = new ApplicationFolderProvider(environmentVariables);

            IPlatformFolder applicationFolder = provider.GetApplicationFolder();

            Assert.IsNull(applicationFolder);
        }

        [TestMethod]
        [TestCategory("WindowsOnly")]
        public void GetApplicationFolderReturnsNullWhenUnableToSetSecurityPolicyOnDirectory()
        {
            var environmentVariables = new Hashtable
            {
                { "LOCALAPPDATA", this.CreateTestDirectory(@"AppData\Local").FullName },
                { "TEMP", this.CreateTestDirectory("Temp").FullName },
            };

            var provider = new ApplicationFolderProvider(environmentVariables);

            // Override to return false indicating applying security failed.
            provider.OverrideApplySecurityToDirectory( (dirInfo) => { return false; } );

            IPlatformFolder applicationFolder = provider.GetApplicationFolder();

            Assert.IsNull(applicationFolder);
        }

        [TestMethod]
        [TestCategory("WindowsOnly")]
        public void GetApplicationFolderReturnsNullWhenFolderAlreadyExistsButDeniesRightToListDirectoryContents()
        {
            this.GetApplicationFolderReturnsNullWhenFolderAlreadyExistsButNotAccessible(FileSystemRights.ListDirectory);
        }

        [TestMethod]
        [TestCategory("WindowsOnly")]
        public void GetApplicationFolderReturnsNullWhenFolderAlreadyExistsButDeniesRightToCreateFiles()
        {
            this.GetApplicationFolderReturnsNullWhenFolderAlreadyExistsButNotAccessible(FileSystemRights.CreateFiles);
        }

        [TestMethod]
        [TestCategory("WindowsOnly")]
        public void GetApplicationFolderReturnsNullWhenFolderAlreadyExistsButDeniesRightToWrite()
        {
            this.GetApplicationFolderReturnsNullWhenFolderAlreadyExistsButNotAccessible(FileSystemRights.Write);
        }

        [TestMethod]
        [TestCategory("WindowsOnly")]
        public void GetApplicationFolderReturnsNullWhenFolderAlreadyExistsButDeniesRightToRead()
        {
            this.GetApplicationFolderReturnsNullWhenFolderAlreadyExistsButNotAccessible(FileSystemRights.Read);
        }

        [TestMethod]
        [TestCategory("WindowsOnly")]
        public void AclsAreAppliedToLocalAppData()
        {
            DirectoryInfo localAppData = this.CreateTestDirectory(@"AppData\Local");
            var environmentVariables = new Hashtable { { "LOCALAPPDATA", localAppData.FullName } };
            var provider = new ApplicationFolderProvider(environmentVariables);

            PlatformFolder applicationFolder = provider.GetApplicationFolder() as PlatformFolder;

            var accessControl = applicationFolder.Folder.GetAccessControl();
            var rulesCollection = accessControl.GetAccessRules(true, true, typeof(SecurityIdentifier));

            Assert.AreEqual(2, rulesCollection.Count);

            Assert.IsFalse(rulesCollection[0].IsInherited);
            Assert.AreEqual(new SecurityIdentifier(WellKnownSidType.BuiltinAdministratorsSid, null).Value, rulesCollection[0].IdentityReference.Value);

            Assert.IsFalse(rulesCollection[1].IsInherited);
            Assert.AreEqual(WindowsIdentity.GetCurrent().User.Value, rulesCollection[1].IdentityReference.Value);

            localAppData.Delete(true);
        }

        [TestMethod]
        [TestCategory("WindowsOnly")]
        public void AclsAreAppliedToTemp()
        {
            DirectoryInfo localAppData = this.CreateTestDirectory(@"AppData\Local", FileSystemRights.CreateDirectories, AccessControlType.Deny);
            DirectoryInfo temp = this.CreateTestDirectory("Temp");
            var environmentVariables = new Hashtable
            {
                { "LOCALAPPDATA", localAppData.FullName },
                { "TEMP", temp.FullName },
            };

            var provider = new ApplicationFolderProvider(environmentVariables);

            PlatformFolder applicationFolder = provider.GetApplicationFolder() as PlatformFolder;

            var accessControl = applicationFolder.Folder.GetAccessControl();
            var rulesCollection = accessControl.GetAccessRules(true, true, typeof(SecurityIdentifier));

            Assert.AreEqual(2, rulesCollection.Count);

            Assert.IsFalse(rulesCollection[0].IsInherited);
            Assert.AreEqual(new SecurityIdentifier(WellKnownSidType.BuiltinAdministratorsSid, null).Value, rulesCollection[0].IdentityReference.Value);

            Assert.IsFalse(rulesCollection[1].IsInherited);
            Assert.AreEqual(WindowsIdentity.GetCurrent().User.Value, rulesCollection[1].IdentityReference.Value);

            localAppData.Delete(true);
        }

#if !NET452

        [TestMethod]
        public void GetApplicationFolderReturnsSubfolderFromTmpDirFolderInNonWindows()
        {
            if (!ApplicationFolderProvider.IsWindowsOperatingSystem())
            {
                DirectoryInfo tmpDir = this.testDirectory.CreateSubdirectory(@"tmpdir");
                var environmentVariables = new Hashtable { { "TMPDIR", tmpDir.FullName } };
                var provider = new ApplicationFolderProvider(environmentVariables);

                IPlatformFolder applicationFolder = provider.GetApplicationFolder();
                Assert.IsNotNull(applicationFolder);
                Assert.AreEqual(1, tmpDir.GetDirectories().Length);
                tmpDir.Delete(true);
            }
        }

        [TestMethod]
        public void GetApplicationFolderReturnsSubfolderFromCustomFolderFirstInNonWindows()
        {
            if (!ApplicationFolderProvider.IsWindowsOperatingSystem())
            {
                DirectoryInfo tmpDir = this.testDirectory.CreateSubdirectory(@"tmpdir");
                DirectoryInfo customFolder = this.testDirectory.CreateSubdirectory(@"Custom");

                var environmentVariables = new Hashtable { { "TMPDIR", tmpDir.FullName } };
                var provider = new ApplicationFolderProvider(environmentVariables, customFolder.FullName);

                IPlatformFolder applicationFolder = provider.GetApplicationFolder();

                Assert.IsNotNull(applicationFolder);
                Assert.AreEqual(((PlatformFolder)applicationFolder).Folder.Name, customFolder.Name, "Sub-folder for custom folder should not be created.");

                tmpDir.Delete(true);
                customFolder.Delete(true);
            }
        }

        [TestMethod]
        public void GetApplicationFolderReturnsSubfolderFromVarTmpFolderIfTmpDirIsNotAvailableInNonWindows()
        {
            if (!ApplicationFolderProvider.IsWindowsOperatingSystem())
            {
                var dir = new System.IO.DirectoryInfo(NonWindowsStorageProbePathVarTmp);
                var provider = new ApplicationFolderProvider();

                IPlatformFolder applicationFolder = provider.GetApplicationFolder();

                Assert.IsNotNull(applicationFolder);
                Assert.IsTrue(dir.GetDirectories().Any(r => r.Name.Equals("Microsoft")));


                dir.EnumerateDirectories().ToList().ForEach(d => { if (d.Name == "Microsoft") d.Delete(true); });
            }
        }

        [TestMethod]
        public void GetApplicationFolderReturnsSubfolderFromTmpFolderIfVarTmpIsNotAvailableInNonWindows()
        {
            if (!ApplicationFolderProvider.IsWindowsOperatingSystem())
            {
                var dir = new System.IO.DirectoryInfo(NonWindowsStorageProbePathTmp);

                var provider = new ApplicationFolderProvider();
                var vartmpPathFieldInfo = provider.GetType().GetField("nonWindowsStorageProbePathVarTmp", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                vartmpPathFieldInfo.SetValue(provider, "");

                IPlatformFolder applicationFolder = provider.GetApplicationFolder();

                Assert.IsNotNull(applicationFolder);
                Assert.IsTrue(dir.GetDirectories().Any(r => r.Name.Equals("Microsoft")));


                dir.EnumerateDirectories().ToList().ForEach(d => { if (d.Name == "Microsoft") d.Delete(true); });
            }
        }

        [TestMethod]
        public void GetApplicationFolderReturnsNullWhenNoFolderAvailableToStoreDataInNonWindows()
        {
            if (!ApplicationFolderProvider.IsWindowsOperatingSystem())
            {
                var provider = new ApplicationFolderProvider();
                var vartmpPathFieldInfo = provider.GetType().GetField("nonWindowsStorageProbePathVarTmp", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                vartmpPathFieldInfo.SetValue(provider, "");
                var tmpPathFieldInfo = provider.GetType().GetField("nonWindowsStorageProbePathTmp", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                tmpPathFieldInfo.SetValue(provider, "");

                IPlatformFolder applicationFolder = provider.GetApplicationFolder();
                Assert.IsNull(applicationFolder);
            }
        }

        [TestMethod]
        public void GetApplicationFolderReturnsSubfolderFromVarTmpIfTmpDirIsTooLongInNonWindows()
        {
            if (!ApplicationFolderProvider.IsWindowsOperatingSystem())
            {
                string longDirectoryName = Path.Combine(this.testDirectory.FullName, new string('A', 300));
                var varTmpdir = new System.IO.DirectoryInfo(NonWindowsStorageProbePathVarTmp);

                // Initialize ApplicationfolderProvider
                var environmentVariables = new Hashtable
                {
                    { "TMPDIR", longDirectoryName },
                };

                var provider = new ApplicationFolderProvider(environmentVariables);

                IPlatformFolder applicationFolder = provider.GetApplicationFolder();

                // Evaluate
                Assert.IsNotNull(applicationFolder);
                Assert.IsFalse(Directory.Exists(longDirectoryName), "TEST ERROR: This directory should not be created.");
                Assert.IsTrue(Directory.Exists(varTmpdir.FullName), "TEST ERROR: This directory should be created.");
                Assert.IsTrue(varTmpdir.GetDirectories().Any(r => r.Name.Equals("Microsoft")), "TEST FAIL: TEMP subdirectories were not created");
                varTmpdir.EnumerateDirectories().ToList().ForEach(d => { if (d.Name == "Microsoft") d.Delete(true); });
            }
        }

#endif

        // TODO: Find way to detect denied FileSystemRights.DeleteSubdirectoriesAndFiles
        public void GetApplicationFolderReturnsNullWhenFolderAlreadyExistsButDeniesRightToDeleteSubdirectoriesAndFiles()
        {
            this.GetApplicationFolderReturnsNullWhenFolderAlreadyExistsButNotAccessible(FileSystemRights.DeleteSubdirectoriesAndFiles);
        }

        private void GetApplicationFolderReturnsNullWhenFolderAlreadyExistsButNotAccessible(FileSystemRights rights)
        {
            DirectoryInfo localAppData = this.CreateTestDirectory(@"AppData\Local");
            var environmentVariables = new Hashtable { { "LOCALAPPDATA", localAppData.FullName } };
            var provider = new ApplicationFolderProvider(environmentVariables);

            // Create the application folder and make it inaccessible
            provider.GetApplicationFolder();
            DirectoryInfo microsoft = localAppData.GetDirectories().Single();
            DirectoryInfo applicationInsights = microsoft.GetDirectories().Single();
            DirectoryInfo application = applicationInsights.GetDirectories().Single();
            using (new DirectoryAccessDenier(application, rights))
            {
                // Try getting the inaccessible folder
                Assert.IsNull(provider.GetApplicationFolder());
            }
        }

        private DirectoryInfo CreateTestDirectory(string path, FileSystemRights rights = FileSystemRights.FullControl, AccessControlType access = AccessControlType.Allow)
        {
            DirectoryInfo directory = this.testDirectory.CreateSubdirectory(path);
            DirectorySecurity security = directory.GetAccessControl();
            security.AddAccessRule(new FileSystemAccessRule(WindowsIdentity.GetCurrent().Name, rights, access));
            directory.SetAccessControl(security);
            return directory;
        }

        private void DeleteIfExists(DirectoryInfo directoryToDelete)
        {
            if (directoryToDelete.Exists)
            {
                directoryToDelete.Delete(true);
            }
        }
    }
}
