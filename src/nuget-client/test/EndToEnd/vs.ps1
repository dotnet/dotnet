param([parameter(Mandatory = $true)]
      [string]$OutputPath,
      [parameter(Mandatory = $true)]
      [string]$TemplatePath)

# Make sure we stop on exceptions
$ErrorActionPreference = "Stop"
$FileKind = "{6BB5F8EE-4483-11D3-8BCF-00C04F8EC28C}"

function Get-VSVersion
{
    $version = [API.Test.VSHelper]::GetVSVersion()
    return $version
}

function New-UwpClassLibraryProjectJson
{
    param(
        [string]$ProjectName,
        [string]$SolutionFolder
    )

    if ((Get-VSVersion) -ge '15.0')
    {
        New-Project UwpClassLibraryProjectJson $ProjectName $SolutionFolder
    }
    else
    {
        throw "SKIP: $($_)"
    }
}

function New-UwpPackageRefClassLibrary
{
    param(
        [string]$ProjectName,
        [string]$SolutionFolder
    )

    if ((Get-VSVersion) -ge '15.0')
    {
        New-Project UwpPackageRefClassLibrary $ProjectName $SolutionFolder
    }
    else
    {
        throw "SKIP: $($_)"
    }
}

function New-BuildIntegratedProj
{
    param(
        [string]$ProjectName,
        [string]$SolutionFolder
    )

    if ((Get-VSVersion) -ge '15.0')
    {
        New-Project BuildIntegratedProj $ProjectName $SolutionFolder
    }
    else
    {
        throw "SKIP: $($_)"
    }
}

function Wait-OnNetCoreRestoreCompletion{
     param(
        [parameter(Mandatory = $true)]
        $Project,
        [int]$TimoutSeconds = 30
    )

    $NetCoreLockFilePath = Get-NetCoreLockFilePath $Project
    $timeout = New-Timespan -Seconds $TimoutSeconds
    $sw = [Diagnostics.Stopwatch]::StartNew()
    while (!(Test-Path $NetCoreLockFilePath)) {
        if ($sw.elapsed -ge $timeout) {
            throw "Time out while waiting for .Net Core project restore on create"
        }
        Start-Sleep -Seconds 1
    }
}

function Wait-ForSolutionLoad
{
    Write-Verbose "Wait-ForSolutionLoad function"

    [API.Test.VSSolutionHelper]::WaitForSolutionLoad()
}

function New-NetCoreConsoleApp
{
    param(
        [string]$ProjectName,
        [string]$SolutionFolder
    )

    if ((Get-VSVersion) -ge '15.0')
    {
        $project = New-Project NetCoreConsoleApp $ProjectName $SolutionFolder
        Wait-OnNetCoreRestoreCompletion $project
        return $project
    }
    else
    {
        throw "SKIP: $($_)"
    }
}

function New-NetCoreConsoleTargetFrameworksApp
{
    param(
        [string]$ProjectName,
        [string]$SolutionFolder
    )

    if ((Get-VSVersion) -ge '15.0')
    {
        $project = New-Project NetCoreConsoleTargetFrameworksApp $ProjectName $SolutionFolder
        Wait-OnNetCoreRestoreCompletion $project
        return $project
    }
    else
    {
        throw "SKIP: $($_)"
    }
}


function New-NetCoreConsoleMultipleTargetFrameworksApp
{
    param(
        [string]$ProjectName,
        [string]$SolutionFolder
    )

    if ((Get-VSVersion) -ge '15.0')
    {
        $project = New-Project NetCoreConsoleMultipleTargetFrameworksApp $ProjectName $SolutionFolder
        Wait-OnNetCoreRestoreCompletion $project
        return $project
    }
    else
    {
        throw "SKIP: $($_)"
    }
}

function New-NetCoreWebApp10
{
    param(
        [string]$ProjectName,
        [string]$SolutionFolder
    )

    if ((Get-VSVersion) -ge '15.0')
    {
        $project = New-Project NetCoreWebApplication1.0 $ProjectName $SolutionFolder
        Wait-OnNetCoreRestoreCompletion $project
        return $project
    }
    else
    {
        throw "SKIP: $($_)"
    }
}

function New-NetStandardClassLibrary
{
    param(
        [string]$ProjectName,
        [string]$SolutionFolder
    )

    if ((Get-VSVersion) -ge '15.0')
    {
        $project = New-Project NetStandardClassLibrary $ProjectName $SolutionFolder
        Wait-OnNetCoreRestoreCompletion $project
        return $project
    }
    else
    {
        throw "SKIP: $($_)"
    }
}

function New-NetCoreConsoleAppWithCustomRestorePackagesPath
{
    param(
        [string]$ProjectName,
        [string]$SolutionFolder
    )

    if ((Get-VSVersion) -ge '15.0')
    {
        $project = New-Project NetCoreConsoleAppWithCustomRestorePackagesPath $ProjectName $SolutionFolder
        Wait-OnNetCoreRestoreCompletion $project
        return $project
    }
    else
    {
        throw "SKIP: $($_)"
    }
}


function New-CpsApp
{
    param(
        [string]$ProjectName,
        [string]$SolutionFolder
    )

    if ($version -ge '15.0')
    {
        New-Project CpsApp $ProjectName $SolutionFolder
    }
    else
    {
        throw "SKIP: $($_)"
    }
}

function Get-SolutionDir {
    Write-Verbose "Get-SolutionDir function"

    $solutionFullName = Get-SolutionFullName
    return Split-Path $solutionFullName
}

function Get-SolutionFullName {
    Write-Verbose "Get-SolutionFullName function"

    return [API.Test.VSSolutionHelper]::GetSolutionFullName()
}

function Close-Solution
{
    Write-Verbose "Close-Solution function"

    [API.Test.VSSolutionHelper]::CloseSolution()
}

function Open-Solution
{
    param
    (
        [string]
        [parameter(Mandatory = $true)]
        $Path
    )
    Write-Verbose "Open-Solution function"

    [API.Test.VSSolutionHelper]::OpenSolution($Path)
}

function SaveAs-Solution
{
    param
    (
        [string]
        [parameter(Mandatory = $true)]
        $Path
    )
    Write-Verbose "SaveAs-Solution function"

    [API.Test.VSSolutionHelper]::SaveAsSolution($Path)
}

function Ensure-Dir {
    param(
        [string]
        [parameter(Mandatory = $true)]
        $Path
    )
    if(!(Test-Path $Path)) {
        mkdir $Path | Out-Null
    }
}

function New-Solution {
    param(
        [string]$solutionName
    )

    Write-Host "New-Solution function"
    if ($solutionName)
    {
        [API.Test.VSSolutionHelper]::CreateNewSolution($OutputPath, $solutionName)
    }
    else
    {
        [API.Test.VSSolutionHelper]::CreateNewSolution($OutputPath)
    }

    Write-Host "New-Solution function: End"
}

function New-Project {
    param(
         [parameter(Mandatory = $true)]
         [string]$TemplateName,
         [string]$ProjectName,
         [string]$SolutionFolder
    )

    Write-Verbose "New-Project function"
    $p = [API.Test.VSProjectHelper]::NewProject($TemplatePath, $OutputPath, $TemplateName, $ProjectName, $SolutionFolder)
    Write-Verbose "New-Project function end"

    return $p
}

function New-SolutionFolder {
    param(
        [string]$FolderPath
    )

    Write-Verbose "New-SolutionFolder function"
    [API.Test.VSSolutionHelper]::NewSolutionFolder($OutputPath, $FolderPath)
    Write-Verbose "New-SolutionFolder function: End"
}

function Rename-SolutionFolder {
    param(
        [string]$FolderPath,
        [string]$NewName
    )

    Write-Verbose "Rename-SolutionFolder function"
    [API.Test.VSSolutionHelper]::RenameSolutionFolder($FolderPath, $NewName)
    Write-Verbose "Rename-SolutionFolder function: End"
}

function New-ClassLibrary {
    param(
        [string]$ProjectName,
        [string]$SolutionFolderName
    )

    New-Project ClassLibrary $ProjectName $SolutionFolderName
}

function New-ClassLibraryNET46 {
    param(
        [string]$ProjectName,
        [string]$SolutionFolderName
    )

    New-Project ClassLibrary46 $ProjectName $SolutionFolderName
}

function New-NativeWinStoreApplication
{
    param(
        [string]$ProjectName,
        [string]$SolutionFolder
    )

    try
    {
        New-Project CppWinStoreApplication $ProjectName $SolutionFolder
    }
    catch {
        # If we're unable to create the project that means we probably don't have some SDK installed
        # Signal to the runner that we want to skip this test
        throw "SKIP: $($_)"
    }
}

function New-ConsoleApplication {
    param(
        [string]$ProjectName,
        [string]$SolutionFolder
    )

    New-Project ConsoleApplication $ProjectName $SolutionFolder
}

function New-WebApplication {
    param(
        [string]$ProjectName,
        [string]$SolutionFolder
    )

    New-Project EmptyWebApplicationProject40 $ProjectName $SolutionFolder
}

function New-VBConsoleApplication {
    param(
        [string]$ProjectName,
        [string]$SolutionFolder
    )

    New-Project VBConsoleApplication $ProjectName $SolutionFolder
}

function New-MvcApplication {
    param(
        [string]$ProjectName,
        [string]$SolutionFolder
    )

    New-Project EmptyMvcWebApplicationProjectTemplatev4.0.csaspx $ProjectName $SolutionFolder
}

function New-MvcWebSite {
    param(
        [string]$ProjectName,
        [string]$SolutionFolder
    )

    New-Project WebApplication45WebSite $ProjectName $SolutionFolder
}

function New-WebSite {
    param(
        [string]$ProjectName,
        [string]$SolutionFolder
    )

    New-Project EmptyWeb $ProjectName $SolutionFolder
}

function New-FSharpLibrary {
    param(
        [string]$ProjectName,
        [string]$SolutionFolder
    )

    $project = New-Project FSharpLibrary $ProjectName $SolutionFolder
    Wait-OnNetCoreRestoreCompletion $project
    return $project
}

function New-FSharpConsoleApplication {
    param(
        [string]$ProjectName,
        [string]$SolutionFolder
    )

    $project = New-Project FSharpConsoleApplication $ProjectName $SolutionFolder
    Wait-OnNetCoreRestoreCompletion $project
    return $project
}

function New-WPFApplication {
    param(
        [string]$ProjectName,
        [string]$SolutionFolder
    )

    New-Project WPFApplication $ProjectName $SolutionFolder
}

function New-SilverlightClassLibrary {
    param(
        [string]$ProjectName,
        [string]$SolutionFolder
    )

    try
    {
        New-Project SilverlightClassLibrary $ProjectName $SolutionFolder
    }
    catch {
        # If we're unable to create the project that means we probably don't have some SDK installed
        # Signal to the runner that we want to skip this test
        throw "SKIP: $($_)"
    }
}

function New-SilverlightApplication {
    param(
        [string]$ProjectName,
        [string]$SolutionFolder
    )

    try
    {
        New-Project SilverlightProject $ProjectName $SolutionFolder
    }
    catch {
        # If we're unable to create the project that means we probably don't have some SDK installed
        # Signal to the runner that we want to skip this test
        throw "SKIP: $($_)"
    }
}

function New-WindowsPhoneClassLibrary {
    param(
        [string]$ProjectName,
        [string]$SolutionFolder
    )

    try {
        New-Project WindowsPhoneClassLibrary $ProjectName $SolutionFolder
    }
    catch {
        # If we're unable to create the project that means we probably don't have some SDK installed
        # Signal to the runner that we want to skip this test
        throw "SKIP: $($_)"
    }
}

function New-DNXClassLibrary
{
    param(
        [string]$ProjectName,
        [string]$SolutionFolder
    )

    try
    {
        New-Project DNXClassLibrary $ProjectName $SolutionFolder
    }
    catch {
        # If we're unable to create the project that means we probably don't have some SDK installed
        # Signal to the runner that we want to skip this test
        throw "SKIP: $($_)"
    }
}

function New-DNXConsoleApp
{
    param(
        [string]$ProjectName,
        [string]$SolutionFolder
    )

    try
    {
        New-Project DNXConsoleApp $ProjectName $SolutionFolder
    }
    catch {
        # If we're unable to create the project that means we probably don't have some SDK installed
        # Signal to the runner that we want to skip this test
        throw "SKIP: $($_)"
    }
}

function New-TextFile {
    Write-Verbose "New-TextFile method"

    [API.Test.VSHelper]::NewTextFile()
}

function Clean-Solution {
    Write-Verbose "Clean the project and wait for it to complete"

    [API.Test.VSSolutionHelper]::CleanSolution()
}

function Build-Solution {
    Write-Verbose "Build and wait for it to complete"

    [API.Test.VSSolutionHelper]::BuildSolution()
}

function Rebuild-Solution {
    Write-Verbose "Rebuild and wait for it to complete"

    [API.Test.VSSolutionHelper]::RebuildSolution()
}

function Get-AssemblyReference {
    param(
        [parameter(Mandatory = $true)]
        $Project,
        [parameter(Mandatory = $true)]
        [string]$Reference
    )
    try {
        return $Project.Object.References.Item($Reference)
    }
    catch {
    }
    return $null
}

function Get-PropertyValue {
    param(
        [parameter(Mandatory = $true)]
        $Project,
        [parameter(Mandatory = $true)]
        [string]$PropertyName
    )
    try {
        $property = $Project.Properties.Item($PropertyName)
        if($property) {
            return $property.Value
        }
    }
    catch {
    }
    return $null
}

function Get-MsBuildPropertyValue {
    param(
        [parameter(Mandatory = $true)]
        $Project,
        [parameter(Mandatory = $true)]
        [string]$PropertyName
    )

    $msBuildProject = Get-MsBuildProject $project
    return $msBuildProject.GetPropertyValue($PropertyName)

    return $null
}

function Get-MsBuildItems {
    param(
        [parameter(Mandatory = $true)]
        $Project,
        [parameter(Mandatory = $true)]
        [string]$ItemName
    )

    $msBuildProject = Get-MsBuildProject $project
    return $msBuildProject.GetItems($ItemName)

    return $null
}

function Get-MsBuildProject
{
    param(
        [parameter(Mandatory = $true)]
        $project
    )

    $projectCollection = [Microsoft.Build.Evaluation.ProjectCollection]::GlobalProjectCollection

    $loadedProjects = $projectCollection.GetLoadedProjects($project.FullName)
    if ($loadedProjects.Count -gt 0) {
        foreach ($p in $loadedProjects) {
            return $p
        }
    }

    $projectCollection.LoadProject($project.FullName)
}

function Get-ProjectDir {
    param(
        [parameter(Mandatory = $true)]
        $Project
    )

    # c++ project has ProjectDirectory
    $path = Get-PropertyValue $Project 'ProjectDirectory'
    if ($path)
    {
        return $path
    }

    $path = Get-PropertyValue $Project FullPath
    if ($path)
    {
        if ([System.IO.File]::Exists($path))
        {
            $path = Split-Path $path -Parent
        }
    }

    $path
}

function Get-ProjectName
{
    param(
        [parameter(Mandatory = $true)]
        $Project
    )

    $projectName = $Project.Name

    if ($project.Type -eq 'Web Site' -and $project.Properties.Item("WebSiteType").Value -eq "0")
    {
        # If this is a WebSite project and WebSiteType = 0, meaning it's configured to use Casini as opposed to IIS Express,
        # then $Project.Name will return the full path to the website directory. We don't want to use the full path, thus
        # we extract the directory name out of it.

        $projectName = Split-Path -Leaf $projectName
    }

    $projectName
}

function Get-OutputPath {
    param(
        [parameter(Mandatory = $true)]
        $Project
    )

    $outputPath = $Project.ConfigurationManager.ActiveConfiguration.Properties.Item("OutputPath").Value
    Join-Path (Get-ProjectDir) $outputPath
}

function Get-Errors {
    Write-Verbose "Get-Errors method"

    $errors = [API.Test.VSHelper]::GetErrors()
    return ,$errors
}

function Get-Warnings {
    Write-Verbose "Get-Warnings method"

    $warnings = [API.Test.VSHelper]::GetWarnings()
    return ,$warnings
}

function Get-ProjectItemPath {
    param(
        [parameter(Mandatory = $true)]
        $Project,
        [parameter(Mandatory = $true, ValueFromPipelineByPropertyName = $true)]
        [string]$Path
    )
    $item = Get-ProjectItem $Project $Path

    if($item) {
        return $item.Properties.Item("FullPath").Value
    }
}

function Remove-ProjectItem {
    param(
        [parameter(Mandatory = $true)]
        $Project,
        [parameter(Mandatory = $true, ValueFromPipelineByPropertyName = $true)]
        [string]$Path
    )

    $item = Get-ProjectItem $Project $Path
    $path = Get-ProjectItemPath $Project $Path
    $item.Remove()
    Remove-Item $path
}

function Get-ProjectItem {
    param(
        [parameter(Mandatory = $true)]
        $Project,
        [parameter(Mandatory = $true, ValueFromPipelineByPropertyName = $true)]
        [string]$Path
    )
    Process {
        $pathParts = $Path.Split('\')
        $projectItems = $Project.ProjectItems

        foreach($part in $pathParts) {
            if(!$part -or $part -eq '') {
                continue
            }

            try {
                $subItem = $projectItems.Item($part)
            }
            catch {
                return $null
            }

            $projectItems = $subItem.ProjectItems
        }

        if($subItem.Kind -eq $FileKind) {
            return $subItem
        }

        # Force array
       return  ,$projectItems
    }
}

function Add-ProjectReference {
    param (
        [parameter(Mandatory = $true, ValueFromPipeline = $true)]
        $ProjectFrom,
        [parameter(Mandatory = $true)]
        $ProjectTo
    )

    if($ProjectFrom.Object.References.AddProject) {
        $ProjectFrom.Object.References.AddProject($ProjectTo) | Out-Null
    }
    elseif($ProjectFrom.Object.References.AddFromProject) {
        $ProjectFrom.Object.References.AddFromProject($ProjectTo) | Out-Null
    }
}

function Remove-Project {
    param (
        [parameter(Mandatory = $true)]
        $ProjectName
    )

    [NuGetConsole.Host.PowerShell.Implementation.ProjectExtensions]::RemoveProject($ProjectName)
}

function Enable-PackageRestore {
    if (!([API.Test.VSSolutionHelper]::IsSolutionAvailable()))
    {
        throw "No solution is available."
    }

    $componentService = Get-VSComponentModel

    # change active package source to "All"
    $packageSourceProvider = $componentService.GetService([NuGet.VisualStudio.IVsPackageSourceProvider])
    $packageSourceProvider.ActivePackageSource = [NuGet.VisualStudio.AggregatePackageSource]::Instance

    $packageRestoreManager = $componentService.GetService([NuGet.VisualStudio.IPackageRestoreManager])
    $packageRestoreManager.EnableCurrentSolutionForRestore($false)
}

function Check-NuGetConfig {
    # Create an empty NuGet.Config file if not exist. It will happen on machines with visual studio newly installed.
    $nuGetConfigPath = Join-Path $env:AppData 'NuGet\NuGet.Config'
    if (!(Test-Path $nuGetConfigPath))
    {
        $newFile = New-Item $nuGetConfigPath -ItemType File
        '<?xml version="1.0" encoding="utf-8"?>
        <configuration>
        </configuration>' > $newFile
    }
}

function Get-BuildOutput {
    return [API.Test.VSHelper]::GetBuildOutput()
}

function AdviseSolutionEvents {
    Write-Verbose "Advise for Solution build events"

    [API.Test.VSSolutionHelper]::AdviseSolutionEvents()
}

function UnadviseSolutionEvents {
    Write-Verbose "Unadvise for solution build events"

    [API.Test.VSSolutionHelper]::UnadviseSolutionEvents()
}

function WaitUntilRebuildCompleted {
    [API.Test.VSSolutionHelper]::WaitUntilRebuildCompleted()
}

function Get-VSFolderPath
{
    $vsappiddir = $env:VSAPPIDDIR # gets vspath\Common7\IDE
    $VSFolderPath = Join-Path $vsappiddir ".." -Resolve
    $VSFolderPath = Join-Path $VSFolderPath ".." -Resolve
    return $VSFolderPath
}

function Get-MSBuildExe {

    $MSBuildRoot = Get-VSFolderPath
    Join-Path $MSBuildRoot "MsBuild\Current\bin\msbuild.exe"
}
