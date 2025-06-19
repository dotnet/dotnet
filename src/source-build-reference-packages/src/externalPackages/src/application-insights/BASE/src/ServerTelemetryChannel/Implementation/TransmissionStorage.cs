namespace Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using Microsoft.ApplicationInsights.Channel;

    using TaskEx = System.Threading.Tasks.Task;

    internal class TransmissionStorage : IDisposable
    {
        internal const string TemporaryFileExtension = ".tmp";
        internal const string TransmissionFileExtension = ".trn";
        internal const int DefaultCapacityKiloBytes = 50 * 1024;

        private readonly ConcurrentDictionary<string, string> badFiles;
        private readonly ConcurrentQueue<IPlatformFile> files;
        private readonly object loadFilesLock;

        private IPlatformFolder folder;
        private long capacity = DefaultCapacityKiloBytes * 1024;
        private long size;
        private bool sizeCalculated;
        private Random random = new Random();
        private Timer clearBadFiles;
        // Storage dequeue is not permitted with FlushAsync
        // When this counter is set, it blocks storage dequeue
        private long flushAsyncInProcessCounter = 0;

        public TransmissionStorage()
        {
            this.files = new ConcurrentQueue<IPlatformFile>();
            this.badFiles = new ConcurrentDictionary<string, string>();
            TimeSpan clearBadFilesInterval = new TimeSpan(29, 0, 0); // Arbitrarily aligns with IIS restart policy of 29 hours in case IIS isn't restarted.
            this.clearBadFiles = new Timer((o) => this.badFiles.Clear(), null, clearBadFilesInterval, clearBadFilesInterval);
            this.loadFilesLock = new object();
            this.sizeCalculated = false;
        }

        /// <summary>
        /// Gets or sets the total amount of disk space, in bytes, allowed for storing transmission files.
        /// </summary>
        public virtual long Capacity
        {
            get
            {
                return this.capacity;
            }

            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }

                this.capacity = value;
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
        
        public virtual void Initialize(IApplicationFolderProvider applicationFolderProvider)
        {
            if (applicationFolderProvider == null)
            {
                throw new ArgumentNullException(nameof(applicationFolderProvider));
            }

            this.folder = applicationFolderProvider.GetApplicationFolder();
        }

        public virtual bool Enqueue(Func<Transmission> transmissionGetter)
        {
            if (this.folder == null)
            {
                return false;
            }

            this.EnsureSizeIsCalculated();

            if (this.size < this.Capacity)
            {
                var transmission = transmissionGetter();
                if (transmission == null)
                {
                    return false;
                }

                try
                {
                    IPlatformFile temporaryFile = this.CreateTemporaryFile();
                    long temporaryFileSize = SaveTransmissionToFile(transmission, temporaryFile);
                    ChangeFileExtension(temporaryFile, TransmissionFileExtension);
                    Interlocked.Add(ref this.size, temporaryFileSize);
                    TelemetryChannelEventSource.Log.TransmissionSavedToStorage(transmission.Id);
                    return true;
                }
                catch (UnauthorizedAccessException e)
                {
                    // Expected because the process may have lost permission to access the folder or the files in it.
                    TelemetryChannelEventSource.Log.UnauthorizedAccessExceptionOnTransmissionSaveWarning(transmission.Id, e.Message);
                }
                catch (Exception exp)
                {
                    TelemetryChannelEventSource.Log.TransmissionFailedToStoreWarning(transmission.Id, exp.ToString());
                    throw;
                }
            }
            else
            {
                TelemetryChannelEventSource.Log.StorageEnqueueNoCapacityWarning(this.size, this.Capacity);
            }

            return false;
        }

        public virtual Transmission Dequeue()
        {
            if (this.folder == null || this.flushAsyncInProcessCounter > 0)
            {
                return null;
            }

            this.EnsureSizeIsCalculated();

            while (true)
            {
                IPlatformFile file = null;
                try
                {
                    if (this.flushAsyncInProcessCounter == 0)
                    {
                        file = this.GetOldestTransmissionFileOrNull();
                        if (file == null)
                        {
                            return null; // Because there are no more transmission files.
                        }

                        long fileSize;
                        Transmission transmission = LoadFromTransmissionFile(file, out fileSize);
                        if (transmission != null)
                        {
                            Interlocked.Add(ref this.size, -fileSize);
                            return transmission;
                        }
                    }
                }
                catch (UnauthorizedAccessException uae)
                {
                    if (file == null)
                    {
                        TelemetryChannelEventSource.Log.TransmissionStorageDequeueUnauthorizedAccessException(this.folder.Name, uae.ToString());
                        return null; // Because the process does not have permission to access the folder.
                    }

                    string name = file.Name;
                    if (this.badFiles.TryAdd(name, null))
                    {
                        TelemetryChannelEventSource.Log.TransmissionStorageInaccessibleFile(name);
                    }
                    else
                    {
                        // The same file has been inaccessible more than once because the process does not have permission to modify this file.
                        TelemetryChannelEventSource.Log.TransmissionStorageUnexpectedRetryOfBadFile(name);
                    }

                    Thread.Sleep(this.random.Next(1, 100)); // Sleep for random time of 1 to 100 milliseconds to try to avoid future timing conflicts.
                }
                catch (IOException ioe)
                {
                    // This exception can happen when one thread runs out of files to process and reloads the list while another
                    // thread is still processing a file and has not deleted it yet thus allowing it to get in the list again.
                    TelemetryChannelEventSource.Log.TransmissionStorageDequeueIOError(file.Name, ioe.ToString());
                    Thread.Sleep(this.random.Next(1, 100)); // Sleep for random time of 1 to 100 milliseconds to try to avoid future timing conflicts.
                    continue; // It may be because another thread already loaded this file, we don't know yet.
                }
            }
        }

        internal void IncrementFlushAsyncCounter()
        {
            Interlocked.Increment(ref this.flushAsyncInProcessCounter);
        }

        internal void DecrementFlushAsyncCounter()
        {
            Interlocked.Decrement(ref this.flushAsyncInProcessCounter);
        }

        private static string GetUniqueFileName(string extension)
        {
            string fileName = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
            return Path.ChangeExtension(fileName, extension);
        }

        private static Transmission LoadFromTransmissionFile(IPlatformFile file, out long fileSize)
        {
            fileSize = 0;
            Transmission transmission = null;
            if (file.Exists)
            {
                // The ingestion service rejects anything older than 2 days.
                if (file.DateCreated > DateTimeOffset.Now.AddDays(-2)) 
                {
                    ChangeFileExtension(file, TemporaryFileExtension);
                    transmission = LoadFromTemporaryFile(file, out fileSize);
                }
                else
                {
                    TelemetryChannelEventSource.Log.TransmissionStorageFileExpired(file.Name, file.DateCreated.ToString(CultureInfo.InvariantCulture));
                }

                file.Delete();
            }

            return transmission;
        }

        private static Transmission LoadFromTemporaryFile(IPlatformFile file, out long fileSize)
        {
            using (Stream stream = file.Open())
            {
                try
                {
                    fileSize = stream.Length;
                    return TransmissionExtensions.Load(stream);
                }
                catch (FormatException exp)
                {
                    fileSize = 0;
                    TelemetryChannelEventSource.Log.IncorrectFileFormatWarning(exp.Message);
                    return null;
                }
            }
        }

        private static void ChangeFileExtension(IPlatformFile file, string extension)
        {
            string transmissionFileName = Path.ChangeExtension(file.Name, extension);
            file.Rename(transmissionFileName);
        }

        private static long SaveTransmissionToFile(Transmission transmission, IPlatformFile file)
        {
            using (Stream stream = file.Open())
            {
                transmission.Save(stream);
                return stream.Length;
            }
        }

        private IPlatformFile CreateTemporaryFile()
        {
            string temporaryFileName = GetUniqueFileName(TemporaryFileExtension);
            return this.folder.CreateFile(temporaryFileName);
        }

        private IEnumerable<IPlatformFile> GetTransmissionFiles()
        {
            IEnumerable<IPlatformFile> newFiles = this.folder.GetFiles();
            newFiles = newFiles.Where(f => f.Extension == TransmissionFileExtension);
            return newFiles;
        }

        private IPlatformFile GetOldestTransmissionFileOrNull()
        {
            IPlatformFile file;
            if (!this.files.TryDequeue(out file))
            {
                this.LoadFilesOrderedByDateFromFolder();
                this.files.TryDequeue(out file);
            }

            return file;
        }

        private void LoadFilesOrderedByDateFromFolder()
        {
            if (this.files.IsEmpty)
            {
                lock (this.loadFilesLock)
                {
                    if (this.files.IsEmpty)
                    {
                        // Sleep a tiny bit before (re)loading the list so that any other thread still processing
                        // a file has time to finish and delete it so that it does not get re-added to the new list.
                        Thread.Sleep(50);

                        // Exclude known bad files and then sort the collection by file creation date.
                        IEnumerable<IPlatformFile> newFiles = this.GetTransmissionFiles();
                        foreach (IPlatformFile file in newFiles.Where(f => !this.badFiles.ContainsKey(f.Name)).OrderBy(f => f.DateCreated))
                        {
                            this.files.Enqueue(file);
                        }
                    }
                }
            }
        }

        private void EnsureSizeIsCalculated()
        {
            if (!this.sizeCalculated)
            {
                lock (this.loadFilesLock)
                {
                    if (!this.sizeCalculated)
                    {
                        try
                        {
                            var storageFiles = this.GetTransmissionFiles();

                            long newSize = 0;
                            foreach (IPlatformFile platformFile in storageFiles)
                            {
                                try
                                {
                                    newSize += platformFile.Length;
                                }
                                catch (FileNotFoundException)
                                {
                                    continue; // Because another thread already dequeued this transmission file.
                                }
                            }

                            Interlocked.Exchange(ref this.size, newSize);
                        }
                        catch (UnauthorizedAccessException e)
                        {
                            TelemetryChannelEventSource.Log.UnauthorizedAccessExceptionOnCalculateSizeWarning(e.Message);
                            this.size = long.MaxValue;
                        }

                        this.sizeCalculated = true;
                        TelemetryChannelEventSource.Log.StorageSize(this.size);
                    }
                }
            }
        }

        private void Dispose(bool disposing)
        {
            if (disposing && this.clearBadFiles != null)
            {
                this.clearBadFiles.Dispose();
                this.clearBadFiles = null;
            }
        }
    }
}
