namespace Microsoft.ApplicationInsights.TestFramework.Extensibility.Implementation.Tracing.SelfDiagnostics
{
    using System;
    using System.Diagnostics.Tracing;
    using System.IO;
    using System.Security.AccessControl;
    using System.Security.Principal;

    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    
    [TestClass]
    public class DiagnosticsTelemetryModuleTest
    {
        [TestMethod]
        public void ModuleTracesToTempFolder()
        {
            using (FileDiagnosticsTelemetryModule module = new FileDiagnosticsTelemetryModule())
            {
                module.Initialize(null);
                using (TestEventSource eventSource = new TestEventSource())
                {
                    eventSource.TraceError("MyError");
                    Assert.IsTrue(module.LogFileName.Contains("ApplicationInsightsLog"), "Actual file name: " + module.LogFileName.Contains("ApplicationInsightsLog"));

                    string fileName = Path.Combine(module.LogFilePath, module.LogFileName);
                    Assert.IsTrue(File.Exists(fileName));
                    using (StreamReader sr = new StreamReader(fileName))
                    {
                        var content = sr.ReadToEnd();
                        Assert.IsTrue(content.Contains("MyError"), "Actual content: " + content);
                    }
                }
            }
        }       

        [TestMethod]
        public void ModuleTracesToANonExistentFolder()
        {
            string expectedPathValue = Path.Combine(Directory.GetCurrentDirectory(), Path.GetRandomFileName());
            if (Directory.Exists(expectedPathValue))
            {
                Directory.Delete(expectedPathValue, true);
            }

            using (FileDiagnosticsTelemetryModule module = new FileDiagnosticsTelemetryModule())
            {
                module.LogFilePath = expectedPathValue;
                using (TestEventSource eventSource = new TestEventSource())
                {
                    Assert.IsTrue(Directory.Exists(expectedPathValue));
                }
            }

            Directory.Delete(expectedPathValue, true);
        }

        [TestMethod]
        public void FilePathCanBeChangedForModule()
        {
            using (FileDiagnosticsTelemetryModule module = new FileDiagnosticsTelemetryModule())
            {
                module.LogFilePath = Directory.GetCurrentDirectory();

                using (TestEventSource eventSource = new TestEventSource())
                {
                    eventSource.TraceError("MyError");

                    string fileName = Path.Combine(module.LogFilePath, module.LogFileName);
                    Assert.IsTrue(File.Exists(fileName));
                    using (StreamReader sr = new StreamReader(fileName))
                    {
                        var content = sr.ReadToEnd();
                        Assert.IsTrue(content.Contains("MyError"), "Actual content: " + content);
                    }
                }
            }
        }

        [TestMethod]
        public void SeverityLevelCanBeChangedForModule()
        {
            using (FileDiagnosticsTelemetryModule module = new FileDiagnosticsTelemetryModule())
            {
                module.LogFilePath = Directory.GetCurrentDirectory();
                module.LogFileName = Guid.NewGuid() + ".txt";

                using (TestEventSource eventSource = new TestEventSource())
                {
                    eventSource.TraceVerbose("MyVerbose1");
                    module.Severity = EventLevel.Verbose.ToString();

                    eventSource.TraceVerbose("MyVerbose2");

                    Assert.AreEqual(EventLevel.Verbose.ToString(), module.Severity);

                    string fileName = Path.Combine(module.LogFilePath, module.LogFileName);
                    Assert.IsTrue(File.Exists(fileName));
                    using (StreamReader sr = new StreamReader(fileName))
                    {
                        var content = sr.ReadToEnd();
                        Assert.IsTrue(content.Contains("MyVerbose2"), "Actual content: " + content);
                    }
                }
            }
        }

        [TestMethod]
        public void FileNameCanBeChanged()
        {
            using (FileDiagnosticsTelemetryModule module = new FileDiagnosticsTelemetryModule())
            {
                module.LogFileName = Guid.NewGuid() + ".txt";

                using (TestEventSource eventSource = new TestEventSource())
                {
                    eventSource.TraceError("MyError");

                    string fileName = Path.Combine(module.LogFilePath, module.LogFileName);
                    Assert.IsTrue(File.Exists(fileName));
                    using (StreamReader sr = new StreamReader(fileName))
                    {
                        var content = sr.ReadToEnd();
                        Assert.IsTrue(content.Contains("MyError"), "Actual content: " + content);
                    }
                }
            }
        }

        [TestMethod]
        public void FilePathNotChangedForEmptyStringFilePath()
        {
            this.SetInvalidFolder(string.Empty);
        }

        [TestMethod]
        [TestCategory("WindowsOnly")] // colon ':' is an illegal character in Windows, but not Linux
        public void FilePathNotChangedForFolderWithInvalidCharacters_WindowsOnly()
        {
            this.SetInvalidFolder(Path.Combine(Directory.GetCurrentDirectory(), ":InvalidFolderName:"));
        }

        [TestMethod]
        [TestCategory("WindowsOnly")] // there's currently no .NET library for folder permissions in Linux
        public void FilePathNotChangedForNotAccessibleFolder_WindowsOnly()
        {
            DirectoryInfo directory = new DirectoryInfo(Directory.GetCurrentDirectory()).CreateSubdirectory(Path.GetRandomFileName());
            DirectorySecurity security = directory.GetAccessControl();
            security.AddAccessRule(new FileSystemAccessRule(WindowsIdentity.GetCurrent().Name, FileSystemRights.CreateDirectories, AccessControlType.Deny));
            directory.SetAccessControl(security);

            this.SetInvalidFolder(Path.Combine(directory.FullName, Path.GetRandomFileName()));
        }

        [TestMethod]
        [TestCategory("WindowsOnly")] // there's currently no .NET library for folder permissions in Linux
        public void FilePathNotChangedForFolderWithoutRightsToCreateFiles_WindowsOnly()
        {
            this.SetNotAccessibleFolder(FileSystemRights.CreateFiles);
        }

        [TestMethod]
        [TestCategory("WindowsOnly")] // there's currently no .NET library for folder permissions in Linux
        public void FilePathNotChangedForFolderWithoutRightsToWrite_WindowsOnly()
        {
            this.SetNotAccessibleFolder(FileSystemRights.Write);
        }

        private void SetNotAccessibleFolder(FileSystemRights rights)
        {            
            DirectoryInfo directory = new DirectoryInfo(Directory.GetCurrentDirectory()).CreateSubdirectory(Path.GetRandomFileName());
            DirectorySecurity security = directory.GetAccessControl();
            security.AddAccessRule(new FileSystemAccessRule(WindowsIdentity.GetCurrent().Name, rights, AccessControlType.Deny));
            directory.SetAccessControl(security);

            this.SetInvalidFolder(directory.FullName);
        }

        private void SetInvalidFolder(string directoryName)
        {
            using (FileDiagnosticsTelemetryModule module = new FileDiagnosticsTelemetryModule())
            {
                module.LogFilePath = directoryName;

                using (TestEventSource eventSource = new TestEventSource())
                {
                    eventSource.TraceError("MyError");
                    Assert.AreNotEqual(directoryName, module.LogFilePath);

                    string fileName = Path.Combine(module.LogFilePath, module.LogFileName);
                    Assert.IsTrue(File.Exists(fileName));
                    using (StreamReader sr = new StreamReader(fileName))
                    {
                        var content = sr.ReadToEnd();
                        Assert.IsTrue(content.Contains("MyError"), "Actual content: " + content);
                    }
                }
            }
        }
    }
}
