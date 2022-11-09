NuGet.exe pack A.nuspec
NuGet.exe pack B.nuspec
NuGet.exe pack C.nuspec
NuGet.exe pack D.nuspec
NuGet.exe pack E.nuspec
NuGet.exe pack F.nuspec

csc /target:library EmptyBinary.cs
NuGet.exe pack Signed.nuspec
