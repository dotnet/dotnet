param(
    [Parameter(Mandatory=$True)]
    [System.String]
    $Directory
)

# SUMMARY
# This script will print all DLLs and PDBs in a given directory.
# This is useful when debugging a hosted build and inspecting if and where expceted artifacts were built.

Write-Host "`nDirectory:" $Directory;

$files = Get-ChildItem -Path $Directory -Recurse -File -Include *.dll, *.pdb, *.nupkg, *.snupkg;
$files | foreach-object { Write-Host "`t"$_.FullName };
Write-Host "`nCount:" $files.Count;
