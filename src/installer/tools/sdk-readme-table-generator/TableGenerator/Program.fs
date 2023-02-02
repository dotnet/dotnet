module TableGenerator.App

open System
open TableGenerator.Shared
open TableGenerator.Reference
open TableGenerator.Table

let inputBranches =
        [ { GitBranchName = "main"
            DisplayName = "main<br>(8.0.x&nbsp;Runtime)"
            AkaMsChannel = Some("8.0.1xx/daily") }
          { GitBranchName = "release/8.0.1xx-preview1"
            DisplayName = "8.0.1xx-preview1<br>(8.0-preview1&nbsp;Runtime)"
            AkaMsChannel = Some("8.0.1xx-preview1/daily") }
          { GitBranchName = "release/7.0.3xx"
            DisplayName = "Release/7.0.3xx<br>(7.0.x&nbsp;Runtime)"
            AkaMsChannel = Some("7.0.3xx/daily") }]


let referentNotes = """Reference notes:
> **1**: Our Debian packages are put together slightly differently than the other OS specific installers. Instead of combining everything, we have separate component packages that depend on each other. If you're installing the SDK from the .deb file (via dpkg or similar), then you'll need to install the corresponding dependencies first:
> * [Host, Host FX Resolver, and Shared Framework](https://github.com/dotnet/runtime/blob/main/docs/project/dogfooding.md#nightly-builds-table)
> * [ASP.NET Core Shared Framework](https://github.com/aspnet/AspNetCore/blob/main/docs/DailyBuilds.md)

.NET Core SDK 2.x downloads can be found at [.NET Core SDK 2.x Installers and Binaries](Downloads2.x.md) but they are [out of support](https://dotnet.microsoft.com/platform/support/policy/dotnet-core)."""

let sdksha2 =
    "[sdk-shas-2.2.1XX]: https://github.com/dotnet/versions/tree/master/build-info/dotnet/product/cli/release/2.2#built-repositories"

let wholeTable branches =
    String.Join
        (Environment.NewLine + Environment.NewLine,
         [ table branches
           referentNotes
           referenceList branches
           sdksha2 ])

[<EntryPoint>]
let main argv =
    Console.WriteLine(wholeTable inputBranches)
    0
