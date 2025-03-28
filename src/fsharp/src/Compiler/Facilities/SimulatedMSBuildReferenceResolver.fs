// Copyright (c) Microsoft Corporation.  All Rights Reserved.  See License.txt in the project root for license information.

module internal FSharp.Compiler.CodeAnalysis.SimulatedMSBuildReferenceResolver

open System
open System.IO
open System.Reflection
open Internal.Utilities.Library
open FSharp.Compiler.IO

// ATTENTION!: the following code needs to be updated every time we are switching to the new MSBuild version because new .NET framework version was released
// 1. List of frameworks
// 2. DeriveTargetFrameworkDirectoriesFor45Plus
// 3. HighestInstalledRefAssembliesOrDotNETFramework
// 4. GetPathToDotNetFrameworkImplementationAssemblies
[<Literal>]
let private Net45 = "v4.5"

[<Literal>]
let private Net451 = "v4.5.1"

[<Literal>]
let private Net452 = "v4.5.2" // not available in Dev15 MSBuild version

[<Literal>]
let private Net46 = "v4.6"

[<Literal>]
let private Net461 = "v4.6.1"

[<Literal>]
let private Net462 = "v4.6.2"

[<Literal>]
let private Net47 = "v4.7"

[<Literal>]
let private Net471 = "v4.7.1"

[<Literal>]
let private Net472 = "v4.7.2"

[<Literal>]
let private Net48 = "v4.8"

let SupportedDesktopFrameworkVersions =
    [ Net48; Net472; Net471; Net47; Net462; Net461; Net46; Net452; Net451; Net45 ]

let private SimulatedMSBuildResolver =

    /// Get the path to the .NET Framework implementation assemblies by using ToolLocationHelper.GetPathToDotNetFramework
    /// This is only used to specify the "last resort" path for assembly resolution.
    let GetPathToDotNetFrameworkImplementationAssemblies _ =
        let isDesktop = typeof<int>.Assembly.GetName().Name = "mscorlib"

        if isDesktop then
            match (System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory(): string MaybeNull) with
            | null -> []
            | x -> [ x ]
        else
            []

    let GetPathToDotNetFrameworkReferenceAssemblies version =
        ignore version
        let r: string list = []
        r

    { new ILegacyReferenceResolver with
        member x.HighestInstalledNetFrameworkVersion() =

            let root = x.DotNetFrameworkReferenceAssembliesRootDirectory

            let fwOpt =
                SupportedDesktopFrameworkVersions
                |> Seq.tryFind (fun fw -> FileSystem.DirectoryExistsShim(Path.Combine(root, fw)))

            match fwOpt with
            | Some fw -> fw
            | None -> Net45

        member _.DotNetFrameworkReferenceAssembliesRootDirectory =
            if Environment.OSVersion.Platform = PlatformID.Win32NT then
                let PF =
                    match Environment.GetEnvironmentVariable("ProgramFiles(x86)") with
                    | null -> !!Environment.GetEnvironmentVariable("ProgramFiles") // if PFx86 is null, then we are 32-bit and just get PF
                    | s -> s

                PF + @"\Reference Assemblies\Microsoft\Framework\.NETFramework"
            else
                ""

        member _.Resolve
            (
                resolutionEnvironment,
                references,
                targetFrameworkVersion,
                targetFrameworkDirectories,
                targetProcessorArchitecture,
                fsharpCoreDir,
                explicitIncludeDirs,
                implicitIncludeDir,
                logMessage,
                logWarningOrError
            ) =

            let results = ResizeArray()

            let searchPaths =
                [
                    yield! targetFrameworkDirectories
                    yield! explicitIncludeDirs
                    yield fsharpCoreDir
                    yield implicitIncludeDir
                    yield! GetPathToDotNetFrameworkReferenceAssemblies targetFrameworkVersion
                    yield! GetPathToDotNetFrameworkImplementationAssemblies targetFrameworkVersion
                ]

            for r, baggage in references do
                //printfn "resolving %s" r
                let mutable found = false

                let success path =
                    if not found then
                        //printfn "resolved %s --> %s" r path
                        found <- true

                        results.Add
                            {
                                itemSpec = path
                                prepareToolTip = snd
                                baggage = baggage
                            }

                try
                    if not found && FileSystem.IsPathRootedShim r then
                        if FileSystem.FileExistsShim r then
                            success r
                with e ->
                    logWarningOrError false "SR001" (e.ToString())

                // For this one we need to get the version search exactly right, without doing a load
                try
                    if
                        not found
                        && r.StartsWithOrdinal("FSharp.Core, Version=")
                        && Environment.OSVersion.Platform = PlatformID.Win32NT
                    then
                        let n = AssemblyName r

                        let fscoreDir0 =
                            let PF =
                                match Environment.GetEnvironmentVariable("ProgramFiles(x86)") with
                                | null -> !!Environment.GetEnvironmentVariable("ProgramFiles")
                                | s -> s

                            PF
                            + @"\Reference Assemblies\Microsoft\FSharp\.NETFramework\v4.0\"
                            + (!!n.Version).ToString()

                        let trialPath = Path.Combine(fscoreDir0, !!n.Name + ".dll")

                        if FileSystem.FileExistsShim trialPath then
                            success trialPath
                with e ->
                    logWarningOrError false "SR001" (e.ToString())

                let isFileName =
                    r.EndsWith("dll", StringComparison.OrdinalIgnoreCase)
                    || r.EndsWith("exe", StringComparison.OrdinalIgnoreCase)

                let qual =
                    if isFileName then
                        r
                    else
                        try
                            !!AssemblyName(r).Name + ".dll"
                        with _ ->
                            r + ".dll"

                for searchPath in searchPaths do
                    try
                        if not found then
                            let trialPath = Path.Combine(searchPath, qual)

                            if FileSystem.FileExistsShim trialPath then
                                success trialPath
                    with e ->
                        logWarningOrError false "SR001" (e.ToString())

                try
                    // Search the GAC on Windows
                    if
                        not found
                        && not isFileName
                        && Environment.OSVersion.Platform = PlatformID.Win32NT
                    then
                        let n = AssemblyName r
                        let netFx = System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory()

                        let gac =
                            Path.Combine(!!Path.GetDirectoryName(Path.GetDirectoryName(netFx.TrimEnd('\\'))), "assembly")

                        match n.Version, n.GetPublicKeyToken() with
                        | null, _
                        | _, null ->
                            let options =
                                [
                                    if FileSystem.DirectoryExistsShim gac then
                                        for gacDir in FileSystem.EnumerateDirectoriesShim gac do
                                            let assemblyDir = Path.Combine(gacDir, !!n.Name)

                                            if FileSystem.DirectoryExistsShim assemblyDir then
                                                for tdir in FileSystem.EnumerateDirectoriesShim assemblyDir do
                                                    let trialPath = Path.Combine(tdir, qual)

                                                    if FileSystem.FileExistsShim trialPath then
                                                        yield trialPath
                                ]
                            //printfn "sorting GAC paths: %A" options
                            options
                            |> List.sort // puts latest version last
                            |> List.tryLast
                            |> function
                                | None -> ()
                                | Some p -> success p

                        | v, tok ->
                            if FileSystem.DirectoryExistsShim gac then
                                for gacDir in Directory.EnumerateDirectories gac do
                                    //printfn "searching GAC directory: %s" gacDir
                                    let assemblyDir = Path.Combine(gacDir, !!n.Name)

                                    if FileSystem.DirectoryExistsShim assemblyDir then
                                        //printfn "searching GAC directory: %s" assemblyDir

                                        let tokText = String.concat "" [| for b in tok -> sprintf "%02x" b |]
                                        let verDir = Path.Combine(assemblyDir, "v4.0_" + v.ToString() + "__" + tokText)
                                        //printfn "searching GAC directory: %s" verDir

                                        if FileSystem.DirectoryExistsShim verDir then
                                            let trialPath = Path.Combine(verDir, qual)
                                            //printfn "searching GAC: %s" trialPath
                                            if FileSystem.FileExistsShim trialPath then
                                                success trialPath
                with e ->
                    logWarningOrError false "SR001" (e.ToString())

            results.ToArray()
    }
    |> LegacyReferenceResolver

let internal getResolver () = SimulatedMSBuildResolver

#if INTERACTIVE
// Some manual testing
SimulatedMSBuildResolver.DotNetFrameworkReferenceAssembliesRootDirectory
SimulatedMSBuildResolver.HighestInstalledNetFrameworkVersion()

let fscoreDir =
    if System.Environment.OSVersion.Platform = System.PlatformID.Win32NT then // file references only valid on Windows
        let PF =
            match Environment.GetEnvironmentVariable("ProgramFiles(x86)") with
            | null -> Environment.GetEnvironmentVariable("ProgramFiles") // if PFx86 is null, then we are 32-bit and just get PF
            | s -> s

        PF + @"\Reference Assemblies\Microsoft\FSharp\.NETFramework\v4.0\4.4.0.0"
    else
        System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory()

let resolve s =
    SimulatedMSBuildResolver.Resolve(
        ResolutionEnvironment.EditingOrCompilation,
        [| for a in s -> (a, "") |],
        "v4.5.1",
        [
            SimulatedMSBuildResolver.DotNetFrameworkReferenceAssembliesRootDirectory
            + @"\v4.5.1"
        ],
        "",
        "",
        fscoreDir,
        [],
        __SOURCE_DIRECTORY__,
        ignore,
        (fun _ _ -> ()),
        (fun _ _ -> ())
    )

// Resolve partial name to something on search path
resolve [ "FSharp.Core" ]

// Resolve DLL name to something on search path
resolve [ "FSharp.Core.dll" ]

// Resolve from reference assemblies
resolve [ "System"; "mscorlib"; "mscorlib.dll" ]

// Resolve from Registry AssemblyFolders
resolve [ "Microsoft.SqlServer.Dmf.dll"; "Microsoft.SqlServer.Dmf" ]

// Resolve exact version of FSharp.Core
resolve
    [
        "FSharp.Core, Version=4.4.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    ]

// Resolve from GAC:
resolve
    [
        "EventViewer, Version=6.3.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    ]

// Resolve from GAC:
resolve [ "EventViewer" ]

resolve
    [
        "Microsoft.SharePoint.Client, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c"
    ]

resolve
    [
        "Microsoft.SharePoint.Client, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c"
    ]
#endif
