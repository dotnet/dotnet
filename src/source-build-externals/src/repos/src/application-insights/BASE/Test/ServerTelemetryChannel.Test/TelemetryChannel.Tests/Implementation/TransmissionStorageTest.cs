namespace Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.TestFramework;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    
    using Channel.Helpers;

    public class TransmissionStorageTest
    {
        private static StubPlatformFile CreateTransmissionFile(Uri endpointAddress = null, byte[] content = null)
        {
            endpointAddress = endpointAddress ?? new Uri("http://address");
            content = content ?? new byte[1];

            var transmission = new Transmission(endpointAddress, content, string.Empty, string.Empty);

            var fileStream = new MemoryStream();
            transmission.Save(fileStream);
            fileStream.Seek(0, SeekOrigin.Begin);

            return CreateFile("TestFile" + TransmissionStorage.TransmissionFileExtension, fileStream);
        }

        private static StubPlatformFile CreateFile(string fileName, Stream fileStream = null)
        {
            fileStream = fileStream ?? new StubStream();
            return new StubPlatformFile
            {
                OnGetName = () => fileName,
                OnGetLength = () => fileStream.Length,
                OnOpen = () => fileStream,
                OnRename = newName => { fileName = newName; },
            };
        }

        private static StubPlatformFolder CreateFolder(params StubPlatformFile[] files)
        {
            return new StubPlatformFolder { OnGetFiles = () => files };
        }

        [TestClass]
        public class Class : TransmissionStorageTest
        {
            [TestMethod]
            public void InitializeThrowsArgumentNullExceptionToPreventUsageErrors()
            {
                var transmitter = new TransmissionStorage();
                AssertEx.Throws<ArgumentNullException>(() => transmitter.Initialize(null));
            }

            // Uncomment this integration test only during investigations.
            // Create unit tests to test for specific error conditions it uncovers.
            // [TestMethod]
            public void IsThreadSafe()
            {
                const int NumberOfThreads = 16;
                const int NumberOfFilesPerThread = 64;
                var storage = new TransmissionStorage();
                storage.Initialize(new ApplicationFolderProvider());

                try
                {
                    string s = new string('c', 500);
                    byte[] content = Encoding.Unicode.GetBytes(s);

                    var tasks = new Task[NumberOfThreads];
                    for (int t = 0; t < tasks.Length; t++)
                    {
                        tasks[t] = Task.Run(async () =>
                        {
                            await Task.Delay(new Random(t).Next(50));
                            for (int f = 0; f < NumberOfFilesPerThread; f++)
                            {
                                storage.Enqueue(() => new Transmission(new Uri("http://address"), content, string.Empty, string.Empty));
                                storage.Dequeue();
                            }
                        });
                    }

                    Task.WhenAll(tasks).GetAwaiter().GetResult();
                }
                finally
                {
                    while (storage.Dequeue() != null)
                    {
                    }
                }
            }
        }

        [TestClass]
        public class Capacity : TransmissionStorageTest
        {
            [TestMethod]
            public void DefaultValueIsAppropriateForAllApplications()
            {
                var storage = new TransmissionStorage();
                storage.Initialize(new StubApplicationFolderProvider());

                Assert.AreEqual(TransmissionStorage.DefaultCapacityKiloBytes * 1024, storage.Capacity);
            }

            [TestMethod]
            public void ValueCanBeChangedByChannelToTunePerformanceForParticularPlatform()
            {
                var storage = new TransmissionStorage();
                storage.Initialize(new StubApplicationFolderProvider());

                storage.Capacity = 42;

                Assert.AreEqual(42, storage.Capacity);
            }

            [TestMethod]
            public void SetterThrowsArgumentOutOfRangeExceptionWhenValueIsLessThanZero()
            {
                var storage = new TransmissionStorage();
                storage.Initialize(new StubApplicationFolderProvider());

                AssertEx.Throws<ArgumentOutOfRangeException>(() => storage.Capacity = -1);
            }
        }

        [TestClass]
        public class Enqueue : TransmissionStorageTest
        {
            [TestMethod]
            public void CreatesNewFileWithTemporaryExtensionToPreventConflictsWithDequeueAsync()
            {
                string temporaryFileName = null;
                var folder = new StubPlatformFolder
                {
                    OnCreateFile = fileName =>
                    {
                        temporaryFileName = fileName;
                        return new StubPlatformFile();
                    }
                };
                var provider = new StubApplicationFolderProvider { OnGetApplicationFolder = () => folder };
                var storage = new TransmissionStorage();
                storage.Initialize(provider);

                storage.Enqueue(() => new StubTransmission());

                Assert.IsTrue(temporaryFileName.EndsWith(TransmissionStorage.TemporaryFileExtension, StringComparison.OrdinalIgnoreCase));
            }

            [TestMethod]
            public void CreatesFilesWithUniqueNamesToPreventConflictsWithOtherThreads()
            {
                var actualFileNames = new List<string>();
                var folder = new StubPlatformFolder
                {
                    OnCreateFile = fileName =>
                    {
                        actualFileNames.Add(fileName);
                        return new StubPlatformFile();
                    }
                };
                var provider = new StubApplicationFolderProvider { OnGetApplicationFolder = () => folder };
                var storage = new TransmissionStorage();
                storage.Initialize(provider);

                storage.Enqueue(() => new StubTransmission());
                storage.Enqueue(() => new StubTransmission());

                Assert.AreNotEqual(actualFileNames[0], actualFileNames[1]);
            }

            [TestMethod]
            public void SavesTransmissionToTheNewlyCreatedFile()
            {
                string writtenContents = null;
                StubStream fileStream = new StubStream();
                fileStream.OnDispose = disposing =>
                {
                    writtenContents = Encoding.UTF8.GetString(fileStream.ToArray());
                };

                var file = new StubPlatformFile { OnOpen = () => fileStream };
                var folder = new StubPlatformFolder { OnCreateFile = fileName => file };
                var provider = new StubApplicationFolderProvider { OnGetApplicationFolder = () => folder };
                var storage = new TransmissionStorage();
                storage.Initialize(provider);

                byte[] contents = Encoding.UTF8.GetBytes(Path.GetRandomFileName());
                var transmission = new StubTransmission(contents);
                storage.Enqueue(() => transmission);
                
                string encodedContent = writtenContents.Split(Environment.NewLine.ToCharArray()).Last();
                AssertEx.AreEqual(contents, Convert.FromBase64String(encodedContent));
            }

            [TestMethod]
            public void ClosesFileStreamToEnsureChangesAreCommittedToDisk()
            {
                bool fileStreamDisposed = false;
                var fileStream = new StubStream { OnDispose = disposing => fileStreamDisposed = true };
                var file = new StubPlatformFile { OnOpen = () => fileStream };
                var folder = new StubPlatformFolder { OnCreateFile = fileName => file };
                var provider = new StubApplicationFolderProvider { OnGetApplicationFolder = () => folder };
                var storage = new TransmissionStorage();
                storage.Initialize(provider);

                storage.Enqueue(() => new StubTransmission());
                
                Assert.IsTrue(fileStreamDisposed);
            }

            [TestMethod]
            public void ChangesTemporaryExtensionToPermanentToMakeFileAvailableForDequeueAsync()
            {
                string permanentFileName = null;
                StubPlatformFile file = CreateFile("TemporaryFile");
                file.OnRename = desiredName => permanentFileName = desiredName;
                var folder = new StubPlatformFolder { OnCreateFile = fileName => file };
                var provider = new StubApplicationFolderProvider { OnGetApplicationFolder = () => folder };
                var storage = new TransmissionStorage();
                storage.Initialize(provider);

                storage.Enqueue(() => new StubTransmission());

                Assert.IsTrue(permanentFileName.EndsWith(TransmissionStorage.TransmissionFileExtension, StringComparison.OrdinalIgnoreCase));
            }

            [TestMethod]
            public void ReturnsTrueWhenTransmissionIsSavedSuccessfully()
            {
                var storage = new TransmissionStorage();
                storage.Initialize(new StubApplicationFolderProvider());

                bool result = storage.Enqueue(() => new StubTransmission());

                Assert.IsTrue(result);
            }

            [TestMethod]
            public void ReturnsFalseAndDoesNotSaveTransmissionWhenStorageCapacityIsAlreadyExceeded()
            {
                StubPlatformFile existingFile = CreateTransmissionFile(default(Uri), new byte[42]);

                bool fileCreated = false;
                StubPlatformFolder folder = CreateFolder(existingFile);
                folder.OnCreateFile = name =>
                {
                    fileCreated = true;
                    return new StubPlatformFile();
                };
                var provider = new StubApplicationFolderProvider { OnGetApplicationFolder = () => folder };
                var storage = new TransmissionStorage { Capacity = 10 };
                storage.Initialize(provider);

                bool result = storage.Enqueue(() => new StubTransmission());
                Thread.Sleep(20);

                Assert.IsFalse(fileCreated, "file created");
                Assert.IsFalse(result);
            }

            [TestMethod]
            public void ReturnsFalseWhenPreviousTransmissionExceedsCapacity()
            {
                StubPlatformFolder folder = CreateFolder();
                var provider = new StubApplicationFolderProvider { OnGetApplicationFolder = () => folder };
                var storage = new TransmissionStorage { Capacity = 10 };
                storage.Initialize(provider);

                bool firstTransmissionSaved = storage.Enqueue(() => new Transmission(new Uri("any://address"), new byte[42], string.Empty, string.Empty));
                bool secondTransmissionSaved = storage.Enqueue(() => new Transmission(new Uri("any://address"), new byte[42], string.Empty, string.Empty));

                Assert.IsTrue(firstTransmissionSaved);
                Assert.IsFalse(secondTransmissionSaved);
            }

            [TestMethod]
            public void SavesTransmissionFileAfterPreviousUnsuccessfullAttempt()
            {
                var storage = new TransmissionStorage { Capacity = 0 };
                storage.Initialize(new StubApplicationFolderProvider());
                storage.Enqueue(() => new StubTransmission());

                storage.Capacity = 1;

                Assert.IsTrue(storage.Enqueue(() => new StubTransmission()));
            }

            [TestMethod]
            public void HandlesFileNotFoundExceptionThrownWhenCalculatingSizeBecauseTransmissionHasAlreadyBeenDequeued()
            {
                var file = new StubPlatformFile();
                file.OnGetName = () => "Dequeued" + TransmissionStorage.TransmissionFileExtension;
                file.OnGetLength = () => { throw new FileNotFoundException(); };
                StubPlatformFolder folder = CreateFolder(file);
                var provider = new StubApplicationFolderProvider { OnGetApplicationFolder = () => folder };
                var storage = new TransmissionStorage();
                storage.Initialize(provider);

                bool transmissionEnqueued = storage.Enqueue(() => new StubTransmission());

                Assert.IsTrue(transmissionEnqueued);
            }

            [TestMethod]
            public void ReturnsFalseIfApplicationFolderIsNotAvailable()
            {
                var provider = new StubApplicationFolderProvider { OnGetApplicationFolder = () => null };
                var storage = new TransmissionStorage();
                storage.Initialize(provider);

                bool result = storage.Enqueue(() => new StubTransmission());

                Assert.IsFalse(result);
            }

            [TestMethod]
            public void ReturnsFalseDoesNotEnqueueIfProcessHasNoRightToListFilesInApplicationFolder()
            {
                var folder = new StubPlatformFolder { OnGetFiles = () => { throw new UnauthorizedAccessException(); } };
                var provider = new StubApplicationFolderProvider { OnGetApplicationFolder = () => folder };
                var storage = new TransmissionStorage();
                storage.Initialize(provider);

                storage.Enqueue(() => new StubTransmission());

                bool result = storage.Enqueue(() => new StubTransmission());

                Assert.IsFalse(result);
            }

            [TestMethod]
            public void ReturnsFalseIfProcessHasNoRightToCreateFilesInApplicationFolder()
            {
                var folder = new StubPlatformFolder { OnCreateFile = name => { throw new UnauthorizedAccessException(); } };
                var provider = new StubApplicationFolderProvider { OnGetApplicationFolder = () => folder };
                var storage = new TransmissionStorage();
                storage.Initialize(provider);

                bool result = storage.Enqueue(() => new StubTransmission());

                Assert.IsFalse(result);
            }
            
            [TestMethod]
            public void ReturnsFalseIfProcessHasNoRightToWriteToFilesInApplicationFolder()
            {
                var file = new StubPlatformFile { OnOpen = () => { throw new UnauthorizedAccessException(); } };
                var folder = new StubPlatformFolder { OnCreateFile = name => file };
                var provider = new StubApplicationFolderProvider { OnGetApplicationFolder = () => folder };
                var storage = new TransmissionStorage();
                storage.Initialize(provider);

                bool result = storage.Enqueue(() => new StubTransmission());

                Assert.IsFalse(result);
            }

            [TestMethod]
            public void ReturnsFalseWhenTransmissionGetterReturnedNullIndicatingNoMoreTransmissionsInBuffer()
            {
                var storage = new TransmissionStorage();
                storage.Initialize(new StubApplicationFolderProvider());

                bool result = storage.Enqueue(() => null);

                Assert.IsFalse(result);
            }

            [TestMethod]
            public void DoesNotRemoveTransmissionFromBufferIfApplicationFolderIsNotAvailableToAvoidDroppingData()
            {
                var provider = new StubApplicationFolderProvider { OnGetApplicationFolder = () => null };
                var storage = new TransmissionStorage();
                storage.Initialize(provider);
                bool transmissionRemovedFromBuffer = false;

                storage.Enqueue(() =>
                {
                    transmissionRemovedFromBuffer = true;
                    return new StubTransmission();
                });

                Assert.IsFalse(transmissionRemovedFromBuffer);
            }

            [TestMethod]
            public void DoesNotRemoveTransmissionFromBufferIfStorageCapacityIsExceededToAvoidDroppingData()
            {
                var storage = new TransmissionStorage() { Capacity = 0 };
                storage.Initialize(new StubApplicationFolderProvider());
                bool transmissionRemovedFromBuffer = false;

                storage.Enqueue(() =>
                {
                    transmissionRemovedFromBuffer = true;
                    return new StubTransmission();
                });

                Assert.IsFalse(transmissionRemovedFromBuffer);
            }
        }

        [TestClass]
        public class DequeueAsync : TransmissionStorageTest
        {
            [TestMethod]
            public void ReturnsNullWhenFolderIsEmpty()
            {
                var storage = new TransmissionStorage();
                storage.Initialize(new StubApplicationFolderProvider());

                Transmission transmission = storage.Dequeue();

                Assert.IsNull(transmission);
            }

            [TestMethod]
            public void ChangesTransmissionFileExtensionToTemporaryToPreventConflictsWithOtherThreads()
            {
                string temporaryFileName = null;
                StubPlatformFile file = CreateTransmissionFile();
                file.OnRename = desiredName => temporaryFileName = desiredName;
                StubPlatformFolder folder = CreateFolder(file);
                var provider = new StubApplicationFolderProvider { OnGetApplicationFolder = () => folder };
                var storage = new TransmissionStorage();
                storage.Initialize(provider);

                storage.Dequeue();

                Assert.IsTrue(temporaryFileName.EndsWith(TransmissionStorage.TemporaryFileExtension, StringComparison.OrdinalIgnoreCase));
            }

            [TestMethod]
            public void LoadsTransmissionFromTemporaryFile()
            {
                var expectedAddress = new Uri("http://" + Guid.NewGuid().ToString("N"));
                StubPlatformFile file = CreateTransmissionFile(expectedAddress);
                StubPlatformFolder folder = CreateFolder(file);
                var provider = new StubApplicationFolderProvider { OnGetApplicationFolder = () => folder };
                var storage = new TransmissionStorage();
                storage.Initialize(provider);

                Transmission dequeued = storage.Dequeue();

                Assert.AreEqual(expectedAddress, dequeued.EndpointAddress);
            }

            [TestMethod]
            public void ReturnsNullWhenFolderContainsSingleCorruptFile()
            {
                StubPlatformFile file = CreateTransmissionFile();
                file.OnOpen = () => new MemoryStream();
                StubPlatformFolder folder = CreateFolder(file);
                var provider = new StubApplicationFolderProvider { OnGetApplicationFolder = () => folder };
                var storage = new TransmissionStorage();
                storage.Initialize(provider);

                Transmission dequeued = storage.Dequeue();

                Assert.IsNull(dequeued);
            }

            [TestMethod]
            public void ClosesFileStreamToEnsureFileCanBeDeleted()
            {
                bool fileStreamDisposed = false;
                var fileStream = new StubStream { OnDispose = disposing => fileStreamDisposed = true };
                StubPlatformFile file = CreateFile("TestFile" + TransmissionStorage.TransmissionFileExtension, fileStream);
                bool fileStreamDisposedBeforeDeletion = false;
                file.OnDelete = () => fileStreamDisposedBeforeDeletion = fileStreamDisposed;

                StubPlatformFolder folder = CreateFolder(file);
                var provider = new StubApplicationFolderProvider { OnGetApplicationFolder = () => folder };
                var storage = new TransmissionStorage();
                storage.Initialize(provider);

                storage.Dequeue();

                Assert.IsTrue(fileStreamDisposedBeforeDeletion);
            }

            [TestMethod]
            public void DeletesTemporaryFileAfterLoadingToFreeDiskSpace()
            {
                bool fileDeleted = false;
                StubPlatformFile file = CreateTransmissionFile();
                file.OnDelete = () => fileDeleted = true;
                StubPlatformFolder folder = CreateFolder(file);
                var provider = new StubApplicationFolderProvider { OnGetApplicationFolder = () => folder };
                var storage = new TransmissionStorage();
                storage.Initialize(provider);

                storage.Dequeue();

                Assert.IsTrue(fileDeleted);
            }

            [TestMethod]
            public void LoadsTransmissionOnlyFromFilesWithTransmissionExtension()
            {
                StubPlatformFile unknownFile = CreateFile("Unknown.file");
                var expectedAddress = new Uri("http://" + Guid.NewGuid().ToString("N"));
                StubPlatformFile transmissionFile = CreateTransmissionFile(expectedAddress);
                StubPlatformFolder folder = CreateFolder(unknownFile, transmissionFile);
                var provider = new StubApplicationFolderProvider { OnGetApplicationFolder = () => folder };
                var storage = new TransmissionStorage();
                storage.Initialize(provider);

                Transmission dequeued = storage.Dequeue();

                Assert.AreEqual(expectedAddress, dequeued.EndpointAddress);
            }

            [TestMethod]
            public void LoadsTransmissionFromTheOldestTransmissionFileInTheFolder()
            {
                var newestAddress = new Uri("http://newest");
                StubPlatformFile newestFile = CreateTransmissionFile(newestAddress);
                newestFile.OnGetDateCreated = () => DateTimeOffset.MaxValue;

                var oldestAddress = new Uri("http://oldest");
                StubPlatformFile oldestFile = CreateTransmissionFile(oldestAddress);
                oldestFile.OnGetDateCreated = () => DateTimeOffset.Now.AddDays(-1); // Create file newer than the 2 day old limit.

                var folder = CreateFolder(newestFile, oldestFile);
                var provider = new StubApplicationFolderProvider { OnGetApplicationFolder = () => folder };
                var storage = new TransmissionStorage();
                storage.Initialize(provider);

                Transmission dequeued = storage.Dequeue();

                Assert.AreEqual(oldestAddress, dequeued.EndpointAddress);
            }

            [TestMethod]
            public void GetsMultipleFilesFromFolderOnceAndCachesThemToReduceDiskAccess()
            {
                int numberOfGetFilesAsyncCalls = 0;
                var files = new [] { CreateTransmissionFile(), CreateTransmissionFile() };
                var folder = new StubPlatformFolder();
                folder.OnGetFiles = () =>
                {
                    numberOfGetFilesAsyncCalls += 1;
                    return files;
                };
                var provider = new StubApplicationFolderProvider { OnGetApplicationFolder = () => folder };
                var storage = new TransmissionStorage();
                storage.Initialize(provider);

                Assert.IsNotNull(storage.Dequeue());
                Assert.IsNotNull(storage.Dequeue());
                Assert.AreEqual(2, numberOfGetFilesAsyncCalls); // 1 for initializing size and 1 for 1 dequeue
            }

            [TestMethod]
            [Timeout(10000)]
            public void DoesNotCacheSameFilesTwiceWhenIvokedByMultipleThreads()
            {
                int numberOfGetFilesAsyncCalls = 0;
                var returnFiles = new ManualResetEventSlim();
                var files = new [] { CreateTransmissionFile(), CreateTransmissionFile() };
                var folder = new StubPlatformFolder();
                folder.OnGetFiles = () =>
                {
                    numberOfGetFilesAsyncCalls += 1;
                    returnFiles.Wait();
                    return files;
                };
                var provider = new StubApplicationFolderProvider { OnGetApplicationFolder = () => folder };
                var storage = new TransmissionStorage();
                storage.Initialize(provider);

                Task<Transmission> dequeue1 = Task.Run(() => storage.Dequeue());
                Task<Transmission> dequeue2 = Task.Run(() => storage.Dequeue());
                returnFiles.Set();
                Task.WhenAll(dequeue1, dequeue2).GetAwaiter().GetResult();

                Assert.AreEqual(2, numberOfGetFilesAsyncCalls); // 1 for initializing size and 1 for 1 dequeue
            }

            [TestMethod]
            public void SkipsCorruptTransmissionFileAndTriesLoadingFromNextTransmissionFile()
            {
                StubPlatformFile corruptFile = CreateFile("Corrupt" + TransmissionStorage.TransmissionFileExtension);
                StubPlatformFile validFile = CreateTransmissionFile();
                StubPlatformFolder folder = CreateFolder(corruptFile, validFile);
                var provider = new StubApplicationFolderProvider { OnGetApplicationFolder = () => folder };
                var storage = new TransmissionStorage();
                storage.Initialize(provider);

                Transmission dequeued = storage.Dequeue();

                Assert.IsNotNull(dequeued);
            }

            [TestMethod]
            public void SkipsTransmissionFileAlreadyLoadedByAnotherThreadAndTriesLoadingNextFile()
            {
                var files = new List<IPlatformFile>();

                StubPlatformFile loadedFile = CreateTransmissionFile();
                loadedFile.OnRename = newName =>
                {
                    files.Remove(loadedFile);
                    throw new FileNotFoundException();
                };

                StubPlatformFile nextFile = CreateTransmissionFile();

                files.Add(loadedFile);
                files.Add(nextFile);
                var folder = new StubPlatformFolder { OnGetFiles = () => files };
                var provider = new StubApplicationFolderProvider { OnGetApplicationFolder = () => folder };
                var storage = new TransmissionStorage();
                storage.Initialize(provider);

                Transmission dequeued = storage.Dequeue();

                Assert.IsNotNull(dequeued);
            }

            [TestMethod]
            public void SkipsTransmissionFileBeingCurrentlyLoadedByAnotherThreadAndTriesLoadingNextFile()
            {
                var files = new List<IPlatformFile>();

                StubPlatformFile fileBeingLoadedByAnotherThread = CreateTransmissionFile();
                fileBeingLoadedByAnotherThread.OnRename = newName =>
                {
                    files.Remove(fileBeingLoadedByAnotherThread);
                    throw new UnauthorizedAccessException();
                };

                StubPlatformFile nextFile = CreateTransmissionFile();

                files.Add(fileBeingLoadedByAnotherThread);
                files.Add(nextFile);
                var folder = new StubPlatformFolder { OnGetFiles = () => files };
                var provider = new StubApplicationFolderProvider { OnGetApplicationFolder = () => folder };
                var storage = new TransmissionStorage();
                storage.Initialize(provider);

                Transmission dequeued = storage.Dequeue();

                Assert.IsNotNull(dequeued);
            }

            // The test timeout must be large enough to account for potential conflicts in the storage dequeue that
            // cause small sleeps of up to 100 ms each plus the overhead of the test runner itself.
            [TestMethod, Timeout(1000)]
            public void DoesNotEndlesslyTryToLoadFileTheProcessNoLongerHasAccessTo()
            {
                StubPlatformFile inaccessibleFile = CreateFile("InaccessibleFile.trn");
                inaccessibleFile.OnRename = newName => { throw new UnauthorizedAccessException(); };
                StubPlatformFolder folder = CreateFolder(inaccessibleFile);
                var provider = new StubApplicationFolderProvider { OnGetApplicationFolder = () => folder };
                var storage = new TransmissionStorage();
                storage.Initialize(provider);

                Transmission result = storage.Dequeue();
                Assert.IsNull(result);
            }

            [TestMethod]
            public void MakesSpaceAvailableForNextTransmission()
            {
                var storage = new TransmissionStorage() { Capacity = 1 };
                storage.Initialize(new StubApplicationFolderProvider());
                storage.Enqueue(() => new Transmission(new Uri("any://address"), new byte[1], "any/content", "any/encoding"));

                storage.Dequeue();

                Assert.IsTrue(storage.Enqueue(() => new StubTransmission()));
            }

            [TestMethod]
            public void DoesNotMakeMoreSpaceAvailableWhenTransmissionCouldNotBeDequeued()
            {
                var storage = new TransmissionStorage { Capacity = 0 };
                storage.Initialize(new StubApplicationFolderProvider());
                storage.Enqueue(() => new StubTransmission());

                storage.Dequeue();

                Assert.IsFalse(storage.Enqueue(() => new StubTransmission()));
            }

            [TestMethod]
            public void ReturnsNullIfApplicationFolderIsNotAvailable()
            {
                var provider = new StubApplicationFolderProvider { OnGetApplicationFolder = () => null };
                var storage = new TransmissionStorage();
                storage.Initialize(provider);

                Transmission result = storage.Dequeue();

                Assert.IsNull(result);
            }

            [TestMethod]
            public void ReturnsNullIfApplicationFolderIsNotAccessible()
            {
                var folder = new StubPlatformFolder { OnGetFiles = () => { throw new UnauthorizedAccessException(); } };
                var provider = new StubApplicationFolderProvider { OnGetApplicationFolder = () => folder };
                var storage = new TransmissionStorage();
                storage.Initialize(provider);

                Transmission result = storage.Dequeue();

                Assert.IsNull(result);
            }

            [TestMethod]
            public void DeletesStoredFilesOlderThanTwoDays()
            {
                var files = new List<IPlatformFile>();

                StubPlatformFile oldFile = CreateTransmissionFile();
                oldFile.OnGetDateCreated = () => DateTimeOffset.Now.AddHours(-49);
                oldFile.OnDelete = () => files.Remove(oldFile);
                files.Add(oldFile);
                var folder = new StubPlatformFolder { OnGetFiles = () => files };
                var provider = new StubApplicationFolderProvider { OnGetApplicationFolder = () => folder };
                var storage = new TransmissionStorage();
                storage.Initialize(provider);

                Transmission dequeued = storage.Dequeue();

                Assert.IsNull(dequeued);
            }

            [TestMethod]
            public void DequeueWithFlushAsyncInProcessCounter()
            {
                var address = new Uri("http://address");
                StubPlatformFile file = CreateTransmissionFile(address);

                var folder = CreateFolder(file);
                var provider = new StubApplicationFolderProvider { OnGetApplicationFolder = () => folder };
                var storage = new TransmissionStorage();
                storage.Initialize(provider);
                
                storage.IncrementFlushAsyncCounter();
                Transmission dequeued = storage.Dequeue();
                // When FlushAsyncCounter is set, dequeue returns null.
                Assert.IsNull(dequeued);

                // DecrementFlushAsyncCounter resets FlushAsyncCounter
                // and allows storage to dequeue transmission.
                storage.DecrementFlushAsyncCounter();
                dequeued = storage.Dequeue();

                Assert.IsNotNull(dequeued);
            }
        }
    }
}
