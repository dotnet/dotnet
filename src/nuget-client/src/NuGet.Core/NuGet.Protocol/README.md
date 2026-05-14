# NuGet.Protocol

NuGet.Protocol is a NuGet client SDK library that provides a set of APIs for interacting with NuGet feeds. It provides a way for developers to query NuGet feeds to discover packages and their dependencies, and also to download packages and their associated assets.

## Usage

At the center of this library are the PackageSource and SourceRepository types, which represent a NuGet source that may be a file source or an http based source implementing the V2 or [V3](https://learn.microsoft.com/nuget/api/overview#versioning) protocol.

```
PackageSource localSource = new PackageSource(@"D:\LocalSource");
SourceRepository localRepository = Repository.Factory.GetCoreV3(localSource);

SourceRepository repository = Repository.Factory.GetCoreV3("https://api.nuget.org/v3/index.json");
```

The SourceRepository then has a GetResourceAsync method that you can use to acquire implementations of INuGetResource that often are [V3](https://learn.microsoft.com/nuget/api/overview#versioning) resources.

```
FindPackageByIdResource resource = await repository.GetResourceAsync<FindPackageByIdResource>(); 
```

## Examples
### Search packages

Search for "json" packages using the [NuGet V3 Search API](https://learn.microsoft.com/nuget/api/search-query-service-resource):

```c#
PackageSearchResource resource = await repository.GetResourceAsync<PackageSearchResource>();
SearchFilter searchFilter = new SearchFilter(includePrerelease: true);

IEnumerable<IPackageSearchMetadata> results = await resource.SearchAsync(
    "json",
    searchFilter,
    skip: 0,
    take: 20,
    NullLogger.Instance,
    CancellationToken.None);
```

### Download a package

Download Newtonsoft.Json v12.0.1 using the [NuGet V3 Package Content API](https://learn.microsoft.com/nuget/api/package-base-address-resource):

```c#
FindPackageByIdResource resource = await repository.GetResourceAsync<FindPackageByIdResource>();

string packageId = "Newtonsoft.Json";
NuGetVersion packageVersion = new NuGetVersion("12.0.1");
using MemoryStream packageStream = new MemoryStream();

await resource.CopyNupkgToStreamAsync(
    packageId,
    packageVersion,
    packageStream,
    new SourceCacheContext(),
    NullLogger.Instance,
    CancellationToken.None);
```

### Push a package

Push a package using the [NuGet V3 Push and Delete API](https://learn.microsoft.com/nuget/api/package-publish-resource):

```c#
PackageUpdateResource resource = await repository.GetResourceAsync<PackageUpdateResource>();

await resource.Push(
    "MyPackage.nupkg",
    symbolSource: null,
    timeoutInSecond: 5 * 60,
    disableBuffering: false,
    getApiKey: packageSource => "my-api-key",
    getSymbolApiKey: packageSource => null,
    noServiceEndpoint: false,
    skipDuplicate: false,
    symbolPackageUpdateResource: null,
    NullLogger.Instance);
```

## Additional documentation

More information about the NuGet.Protocol library can be found on the [official Microsoft documentation page](https://learn.microsoft.com/nuget/reference/nuget-client-sdk#nugetprotocol) and [NuGet API docs](https://learn.microsoft.com/nuget/api/overview).