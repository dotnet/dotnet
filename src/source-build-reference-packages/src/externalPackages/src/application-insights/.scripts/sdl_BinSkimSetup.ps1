[CmdletBinding()]
Param(
    # C:\Repos\bin\Debug\Src
    [string]$buildDirectory = (Join-Path -Path (Split-Path -parent (Split-Path -parent (Split-Path -parent $PSCommandPath))) -ChildPath "bin\Debug\Src") ,
    
    # C:\Repos\binSkim
    [string]$binSkimDirectory = (Join-Path -Path (Split-Path -parent (Split-Path -parent (Split-Path -parent $PSCommandPath))) -ChildPath "binSkim")
)

# SUMMARY
# This script will collect and filter all output DLLs to a specified directory for use by the BinSkim SDL tool.


# these are dlls that end up in the bin, but do not belong to us and don't need to be scanned.
$excludedFiles = @(
    "KernelTraceControl.dll", 
    "msdia140.dll",
    "Microsoft.Web.Infrastructure.dll"
    )

Write-Host "`nPARAMETERS:";
Write-Host "`tbuildDirectory:" $buildDirectory;
Write-Host "`tbinSkimDirectory:" $binSkimDirectory;

# don't need to clean folder on build server, but is needed for local dev
Write-Host "`nCreate BinSkim Directory...";
if (Test-Path $binSkimDirectory) { Remove-Item $binSkimDirectory -Recurse; }

# copy all
Write-Host "`nCopy all files...";
Copy-Item -Path $buildDirectory -Filter "*.dll" -Destination $binSkimDirectory -Recurse;

# delete test directories
Write-Host "`nDelete any 'Test' directories...";
Get-ChildItem -Path $binSkimDirectory -Recurse -Directory | 
    Where-Object {$_ -match "Test"} |
    Remove-Item -Recurse;

# delete excluded files
if ($excludedFiles.Count -gt 0) {
    Write-Host "`nDelete excluded files...";
    Get-ChildItem -Path $binSkimDirectory -Recurse -File | 
        ForEach-Object { 
            if ($excludedFiles.Contains($_.Name)) {
                Write-Host "Excluded File:" $_.FullName;
                Remove-Item $_.FullName;
            }
        } 
}

# summary for log output (file list and count)
Write-Host "`nCopied Files:";

$count = 0;
Get-ChildItem -Path $binSkimDirectory -Recurse -File | 
    ForEach-Object { 
        Write-Host "`t"$_.FullName; 
        $count++;
    } 

Write-Host "`nTOTAL FILES:" $count;