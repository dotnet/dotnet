# Copyright (c) Microsoft. All rights reserved.
# Build script for Test Platform.

[CmdletBinding()]
Param(
    [Parameter(Mandatory=$false)]
    [ValidateSet("Debug", "Release")]
    [Alias("c")]
    [System.String] $Configuration = "Debug",

    [Parameter(Mandatory=$true)]
    [Alias("cert")]
    [System.String] $Certificate
)

$ErrorActionPreference = "Continue"

#
# Variables
#
Write-Verbose "Setup environment variables."
$env:TP_ROOT_DIR = (Get-Item (Split-Path $MyInvocation.MyCommand.Path)).Parent.FullName
$env:TP_OUT_DIR = Join-Path $env:TP_ROOT_DIR "artifacts"
$env:TP_TOOLS_DIR = Join-Path $env:TP_ROOT_DIR "tools"
$script:ErrorCount = 0

#
# Signing configuration
#
# Authenticode signature details
Write-Verbose "Setup build configuration."
$TPB_SignCertificate = $Certificate
$TPB_Configuration = $Configuration
$TPB_AssembliesPattern = @(
    "*test*.dll", "*qualitytools*.dll", "*test*.exe", "*datacollector*.dll", "*datacollector*.exe",
    "QTAgent*.exe", "Microsoft.VisualStudio*.dll", "Microsoft.TestPlatform.Build.dll", "Microsoft.DiaSymReader.dll",
    "Microsoft.IntelliTrace*.dll", "concrt140.dll", "msvcp140.dll", "vccorlib140.dll", "vcruntime140.dll", "codecoveragemessages.dll",
    "covrun32.dll", "msdia140.dll", "covrun64.dll", "covrunarm64.dll", "IntelliTrace.exe", "ProcessSnapshotCleanup.exe", "TDEnvCleanup.exe",
    "CodeCoverage.exe", "Microsoft.ShDocVw.dll", "UIAComwrapper.dll", "Interop.UIAutomationClient.dll", "SettingsMigrator.exe",
    "Newtonsoft.Json.dll", "DumpMinitool*.exe"
)

function Verify-Assemblies
{
    Write-Debug "Verify-Assemblies: Start"
    $artifactsDirectory = Join-Path $env:TP_OUT_DIR $TPB_Configuration
    foreach ($pattern in $TPB_AssembliesPattern) {
        Write-Debug "Pattern: $pattern"
        Get-ChildItem -Recurse -Include $pattern $artifactsDirectory | Where-Object { (!$_.PSIsContainer) -and !($($_.FullName).Contains('VSIX\obj')) -and !($($_.FullName).Contains('publishTemp')) -and !($($_.FullName).Contains('sign_temp'))} | % {
            $Path = $_.FullName
            $signature = Get-AuthenticodeSignature -FilePath $Path

            if ($signature.Status -eq "Valid") {
                if ($signature.SignerCertificate.Subject -eq "CN=Microsoft Corporation, O=Microsoft Corporation, L=Redmond, S=Washington, C=US") {
                    Write-Debug "Valid ($($signature.SignerCertificate.Thumbprint)): $Path"
                }
                elseif ($signature.SignerCertificate.Subject -eq "CN=Microsoft 3rd Party Application Component, O=Microsoft Corporation, L=Redmond, S=Washington, C=US") {
                    Write-Debug "Valid ($($signature.SignerCertificate.Thumbprint)): $Path [3rd Party]"
                }
                else {
                    # For legacy components, sign certificate is always "prod" signature. Skip such binaries.
                    if ($signature.SignerCertificate.Thumbprint -eq "98ED99A67886D020C564923B7DF25E9AC019DF26") {
                        Write-Debug "Valid ($($signature.SignerCertificate.Thumbprint)): $Path [Prod Signed]"
                    }
                    # For some dlls e.g. "Microsoft.DiaSymReader.dll", sign certificate is different signature. Skip such binaries.
                    elseif ($signature.SignerCertificate.Thumbprint -eq "5EAD300DC7E4D637948ECB0ED829A072BD152E17") {
                        Write-Debug "Valid ($($signature.SignerCertificate.Thumbprint)): $Path [Prod Signed]"
                    }
                    # For some dlls e.g. "Interop.UIAutomationClient.dll", sign certificate is different signature. Skip such binaries.
                    elseif ($signature.SignerCertificate.Thumbprint -eq "67B1757863E3EFF760EA9EBB02849AF07D3A8080") {
                        Write-Debug "Valid ($($signature.SignerCertificate.Thumbprint)): $Path [Prod Signed]"
                    }
                    # For some dlls e.g. "Microsoft.VisualStudio.ArchitectureTools.PEReader.dll", sign certificate is different signature. Skip such binaries.
                    elseif ($signature.SignerCertificate.Thumbprint -eq "9DC17888B5CFAD98B3CB35C1994E96227F061675") {
                        Write-Debug "Valid ($($signature.SignerCertificate.Thumbprint)): $Path [Prod Signed]"
                    }
                    # For some dlls sign certificate is different signature. Skip such binaries.
                    elseif ($signature.SignerCertificate.Thumbprint -eq "62009AAABDAE749FD47D19150958329BF6FF4B34") {
                        Write-Debug "Valid ($($signature.SignerCertificate.Thumbprint)): $Path [Prod Signed]"
                    }
                    # Microsoft 3rd Party Authenticode Signature
                    elseif ($signature.SignerCertificate.Thumbprint -eq "899FA016DEE8E665FF2A315A1151C43FB96C430B") {
                        Write-Debug "Valid ($($signature.SignerCertificate.Thumbprint)): $Path [Prod Signed]"
                    }
                    # Microsoft 3rd Party Application Component
                    elseif ($signature.SignerCertificate.Thumbprint -eq "709133ECC53CBF386F4A5ECB782AEEF499F0F8CA") {
                        Write-Debug "Valid ($($signature.SignerCertificate.Thumbprint)): $Path [3rd Party Signed]"
                    }
                    # Microsoft 3rd Party Application Component
                    elseif ($signature.SignerCertificate.Thumbprint -eq "912357a68d29b8fe17168ef8c44d6830d1d42801") {
                        Write-Debug "Valid ($($signature.SignerCertificate.Thumbprint)): $Path [Prod Signed]"
                    }
                    # For some dlls sign certificate is different signature, which already come as signed from nuget packages. Skip such binaries.
                    elseif ($signature.SignerCertificate.Thumbprint -eq "81C25099511180D15B858DC2B7EC4C057B1CE4BF") {
                        Write-Debug "Valid ($($signature.SignerCertificate.Thumbprint)): $Path [Prod Signed]"
                    }
                    # .NET Foundation, Newtonsoft.Json.dll 13.0.1
                    elseif ($signature.SignerCertificate.Thumbprint -eq "4CFB89FAA49539A58968D81960B3C1258E8F6A34") {
                        Write-Debug "Valid ($($signature.SignerCertificate.Thumbprint)): $Path [.NET Foundation Signed]"
                    }
                    else {
                        Write-FailLog "Invalid ($($signature.SignerCertificate.Thumbprint)). File: $Path. [$($signature.SignerCertificate.Subject)]"
                    }
                }
            }
            else {
                Write-FailLog "Not signed. File: $Path."
            }
        }
    }

    Write-Debug "Verify-Assemblies: Complete"
}

function Verify-NugetPackages
{
    Write-Debug "Verify-NugetPackages: Start"

    # Move acquiring nuget.exe to external dependencies once Nuget.Commandline for 4.6.1 is available.
    $nugetInstallDir = Join-Path $env:TP_TOOLS_DIR "nuget"
    $nugetInstallPath = Join-Path $nugetInstallDir "nuget.exe"

    if(![System.IO.File]::Exists($nugetInstallPath))
    {
        # Create the directory for nuget.exe if it does not exist
        New-Item -ItemType Directory -Force -Path $nugetInstallDir
        Invoke-WebRequest https://dist.nuget.org/win-x86-commandline/v4.6.1/nuget.exe -OutFile $nugetInstallPath
    }

    Write-Debug "Using nuget.exe installed at $nugetInstallPath"

    $artifactsDirectory = Join-Path $env:TP_OUT_DIR $TPB_Configuration
    $packagesDirectory = Join-Path $artifactsDirectory "packages"

    Get-ChildItem -Filter *.nupkg  $packagesDirectory | % {
        & $nugetInstallPath verify -signature -CertificateFingerprint "3F9001EA83C560D712C24CF213C3D312CB3BFF51EE89435D3430BD06B5D0EECE;AA12DA22A49BCE7D5C1AE64CC1F3D892F150DA76140F210ABD2CBFFCA2C18A27;" $_.FullName
        if($LASTEXITCODE -eq 1) {
            $script:ErrorCount = $script:ErrorCount + 1
        }
    }

    Write-Debug "Verify-NugetPackages: Complete"
}

function Write-FailLog ([string] $message)
{
    $script:ErrorCount = $script:ErrorCount + 1
    Write-ToCI -message $message -type "task.logissue type=error"
}

function Write-Debug ([string] $message)
{
    Write-ToCI -message $message -type "debug"
}

function Write-ToCI ([string] $message, [string]$type, [switch]$vso)
{
    if ($message -or $vso -or $type)
    {
        $prefix = ""
        if ($vso) {
            $prefix = "vso"
        }

        $color = if($type -eq "error") { "Red" } else { $Host.UI.RawUI.ForegroundColor }
        Write-Host "##$prefix[$type]$message" -ForegroundColor $color
    }
}

Write-Debug "Variables used: "
Get-ChildItem variable:TPB_*
Write-Host ""
Write-Host ""

Verify-Assemblies
Verify-NugetPackages

if ($script:ErrorCount -gt 0) {
    # Write-ToCI -message "Verification failed, $($script:ErrorCount) errors found!" -type "task.logissue type=error" -vso
    Write-ToCI -message "Verification failed, $($script:ErrorCount) errors found!" -type "task.complete result=Failed;" -vso
}