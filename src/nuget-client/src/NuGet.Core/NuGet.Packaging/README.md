# Nuget.Packaging

NuGet.Packaging is a NuGet client SDK library that provides a set of APIs to interact with `.nupkg` and `.nuspec` files from a stream. It provides a way for developers to create and read packages and work with the package metadata.

## Usage

> It is strongly recommended that NuGet packages are created using the official NuGet tooling and instead of this low-level API. There are a variety of characteristics important for a well-formed package and the latest version of tooling helps incorporate these best practices.

> For more information about creating NuGet packages, see the overview of the [package creation workflow](https://learn.microsoft.com/nuget/create-packages/overview-and-workflow) and the documentation for official pack tooling (for example, [using the dotnet CLI](https://learn.microsoft.com/nuget/create-packages/creating-a-package-dotnet-cli)).
## Examples

### Create a package

Create a package, set metadata, and add dependencies.

```c#
PackageBuilder builder = new PackageBuilder();
builder.Id = "MyPackage";
builder.Version = new NuGetVersion("1.0.0-beta");
builder.Description = "My package created from the API.";
builder.Authors.Add("Sample author");
builder.DependencyGroups.Add(new PackageDependencyGroup(
    targetFramework: NuGetFramework.Parse("netstandard1.4"),
    packages: new[]
    {
        new PackageDependency("Newtonsoft.Json", VersionRange.Parse("10.0.1"))
    }));

using FileStream outputStream = new FileStream("MyPackage.nupkg", FileMode.Create);
builder.Save(outputStream);
Console.WriteLine($"Saved a package to {outputStream.Name}");
```

### Read a package

Read a package from a file.

```c#
using FileStream inputStream = new FileStream("MyPackage.nupkg", FileMode.Open);
using PackageArchiveReader reader = new PackageArchiveReader(inputStream);
NuspecReader nuspec = reader.NuspecReader;
Console.WriteLine($"ID: {nuspec.GetId()}");
Console.WriteLine($"Version: {nuspec.GetVersion()}");
Console.WriteLine($"Description: {nuspec.GetDescription()}");
Console.WriteLine($"Authors: {nuspec.GetAuthors()}");

Console.WriteLine("Dependencies:");
foreach (PackageDependencyGroup dependencyGroup in nuspec.GetDependencyGroups())
{
    Console.WriteLine($" - {dependencyGroup.TargetFramework.GetShortFolderName()}");
    foreach (var dependency in dependencyGroup.Packages)
    {
        Console.WriteLine($"   > {dependency.Id} {dependency.VersionRange}");
    }
}
```

## Additional documentation

More information about the NuGet.Packaging library can be found on the [official Microsoft documentation page](https://learn.microsoft.com/nuget/reference/nuget-client-sdk#nugetpackaging) and [NuGet API docs](https://learn.microsoft.com/nuget/reference/nuget-client-sdk).