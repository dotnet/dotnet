param (
    [Parameter(Mandatory=$true)]
    [string]$NuGetRoot,
    [Parameter(Mandatory=$true)]
    [string]$DropLocation)

$Artifacts = Join-Path $DropLocation "artifacts"
$Nupkgs = Join-Path $DropLocation "nupkgs"
$Symbols = Join-Path $DropLocation "symbols"

# Force make directory does not fail if the directory is already present
# and, does not delete existing directory.
mkdir $Artifacts -Force
mkdir $Nupkgs -Force
mkdir $Symbols -Force

Write-Host "Copying binaries and packages from $NuGetRoot to $DropLocation..."

copy $NuGetRoot\nupkgs\*.nupkg $Nupkgs
copy $NuGetRoot\artifacts\*.vsix $Artifacts
copy $NuGetRoot\artifacts\NuGet.exe $Artifacts
ls $NuGetRoot\artifacts\*.pdb -Recurse | %{ copy $_.FullName $DropLocation\symbols\ }

Write-Host "Binaries and packages have been copied successfully."