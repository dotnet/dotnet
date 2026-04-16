# Persistent Storage file based implementation

| Status | |
| ------ | --- |
| Stability | [Stable](../../README.md#stable) |
| Code Owners | [@rajkumar-rangaraj](https://github.com/rajkumar-rangaraj/) |

[![NuGet version badge](https://img.shields.io/nuget/v/OpenTelemetry.PersistentStorage.FileSystem)](https://www.nuget.org/packages/OpenTelemetry.PersistentStorage.FileSystem)
[![NuGet download count badge](https://img.shields.io/nuget/dt/OpenTelemetry.PersistentStorage.FileSystem)](https://www.nuget.org/packages/OpenTelemetry.PersistentStorage.FileSystem)
[![codecov.io](https://codecov.io/gh/open-telemetry/opentelemetry-dotnet-contrib/branch/main/graphs/badge.svg?flag=unittests-PersistentStorage)](https://app.codecov.io/gh/open-telemetry/opentelemetry-dotnet-contrib?flags[0]=unittests-PersistentStorage)

This package provides an implementation of
[persistent-storage-abstractions](../OpenTelemetry.PersistentStorage.Abstractions/README.md#Persistent-Storage-Abstractions)
based on local file system. This component can be used by OpenTelemetry
exporters to improve the reliability of data delivery.

## Installation

```shell
dotnet add package OpenTelemetry.PersistentStorage.FileSystem
```

## Usage

### Setup FileBlobProvider

```csharp
using var persistentBlobProvider = new FileBlobProvider(@"C:\temp");
```

#### Configuration

Following is the complete list of parameters that `FileBlobProvider` constructor
accepts to control its configuration:

##### path

Sets directory location where blobs are stored. `Required`.

##### maxSizeInBytes

Maximum allowed folder size. `Optional`. Default if not specified: `52428800`
bytes.

New blobs are dropped after the folder size reaches maximum limit. A log message
is written if blobs cannot be written. See [Troubleshooting](#troubleshooting)
for more information.

##### maintenancePeriodInMilliseconds

Maintenance event runs at specified interval. `Optional`. Default if not
specified: `120000`ms.

During this event, the following tasks are performed:

* Remove `*.blob` files for which the retention period has expired.
* Remove `*.tmp` files for which the write timeout period has expired.
* Update `*.lock` files to `*.blob` for which the lease period has expired.
* Update available folder space.

For more details on file extensions(.blob, .tmp, .lock) see [File
naming](#file-naming) section below.

##### retentionPeriodInMilliseconds

File retention period in milliseconds for the blob. `Optional`. Default if not
specified: `172800000`ms.

##### writeTimeoutInMilliseconds

Controls the timeout when writing a buffer to blob. `Optional`. Default if not
specified: `60000`ms.

### Blob Operations

#### CreateBlob

`TryCreateBlob(byte[] buffer, out PersistentBlob blob)` or `TryCreateBlob(byte[]
buffer, int leasePeriodMilliseconds = 0, out PersistentBlob blob)` can be used
to create a blob (single file). The file stored will have `.blob`
extension. If acquiring lease, the file will have `.lock` extension.

```csharp
// Try create blob without acquiring lease
persistentBlobProvider.TryCreateBlob(data, out var blob);

// Try create blob and acquire lease
persistentBlobProvider.TryCreateBlob(data, 1000, out var blob);
```

#### GetBlob and GetBlobs

`TryGetBlob` can be used to read single blob or `GetBlobs` can be used to get list
of all blobs stored on disk. The result will only include files with `.blob`
extension.

```csharp
// Get single blob from storage.
persistentBlobProvider.GetBlob(out var blob);

// List all blobs.
foreach (var blobItem in persistentBlobProvider.GetBlobs())
{
    Console.WriteLine(((FileBlob)blobItem).FullPath);
}
```

#### Lease

When reading data back from disk, `TryLease(int leasePeriodMilliseconds)` method
should be used first to acquire lease on blob. This prevents it to be read
concurrently, until the lease period expires. Leasing a blob changes the file
extension to `.lock`.

```csharp
blob.TryLease(1000);
```

#### Read

Once the lease is acquired on the blob, the data can be read using
`TryRead(out var data)` method.

```csharp
blob.TryRead(out var data);
```

#### Delete

`TryDelete` method can be used to delete the blob.

```csharp
blob.TryDelete();
```

### Example

```csharp
using var persistentBlobProvider = new FileBlobProvider(@"C:\temp");

var data = Encoding.UTF8.GetBytes("Hello, World!");

// Create blob.
persistentBlobProvider.TryCreateBlob(data, out var createdBlob);

// List all blobs.
foreach (var blobItem in persistentBlobProvider.GetBlobs())
{
    Console.WriteLine(((FileBlob)blobItem).FullPath);
}

// Get single blob.
if (persistentBlobProvider.TryGetBlob(out var blob))
{
    // Lease before reading
    if (blob.TryLease(1000))
    {
        // Read
        if (blob.TryRead(out var outputData))
        {
            Console.WriteLine(Encoding.UTF8.GetString(outputData));
        }

        // Delete
        if (blob.TryDelete())
        {
            Console.WriteLine("Successfully deleted the blob");
        }
    }
}
```

## File Details

### File naming

Each call to [CreateBlob](#createblob) methods create a single file(blob) at the
configured [directory path](#path). Each file that is created has unique name in
the format `datetimestamp(ISO 8601)-GUID` with current datetime. The file
extension depends on the operation. When creating a blob, the file is stored
with the `.blob` extension. If a lease is acquired on an existing file or on the
file being created, the file extension is changed to `.lock`, along with the
lease expiration time appended to its name in the format `@datetimestamp(ISO
8601)`. The `.tmp` extension is used for files while data writing is in process.

Example file names:

* `2024-05-15T174825.3027972Z-40386ee02b8a47f1b04afc281f33d712.blob`
* `2024-05-15T174825.3027972Z-40386ee02b8a47f1b04afc281f33d712.blob@2024-05-15T203551.2932278Z.lock`
* `2024-05-15T175941.2228167Z-6649ff8ce55144b88a99c440a0b9feea.blob.tmp`

### Data format and security

The data contained within the file(blob) is unencrypted and stored in its
original, unprocessed format provided in the byte array. If there is a privacy
concern, application owners SHOULD review and restrict the collection of private
data. If specific security requirements need to be met, application owners
SHOULD configure the [directory](#path) to restrict access (ensuring that the
process running your application has write access to this directory), thus
preventing unauthorized users from reading its contents.

### Data retention

A blob stored on disk persists until it is explicitly deleted using the
[TryDelete](#delete) operation or is removed during the [maintenance
job](#maintenanceperiodinmilliseconds) upon the expiration of its
[retention](#retentionperiodinmilliseconds) period.

## Troubleshooting

This component uses an
[EventSource](https://docs.microsoft.com/dotnet/api/system.diagnostics.tracing.eventsource)
with the name "OpenTelemetry-PersistentStorage-FileSystem" for its internal
logging. Please follow the [troubleshooting guide for OpenTelemetry
.NET](https://github.com/open-telemetry/opentelemetry-dotnet/tree/main/src/OpenTelemetry#troubleshooting)
for instructions on how to capture the logs.
