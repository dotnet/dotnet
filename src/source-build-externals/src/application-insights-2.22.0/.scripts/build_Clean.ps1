param(
    [Parameter(Mandatory=$True)]
    [System.String]
    $Directory,

    [Parameter(Mandatory=$False)]
    [System.Boolean]
    $CleanBinAndObj
)


# SUMMARY
# This script will delete the BIN and OBJ directories, if they exist.
# This is used in our builds to ensure that there are no left over artifacts from a previous build.


function Clean ([string]$dir) {
    
    Write-Host "`nDirectory: $($dir)";

    if (Test-Path $dir) 
    {
        PrintFileCount $dir;
        Remove-Item $dir -Recurse -Force; 
        Write-Host " removed";
        PrintFileCount $dir;
    }
    else 
    { 
        Write-Host " directory not found"
    }
}

function PrintFileCount ([string]$dir) {
    $count = ( Get-ChildItem $dir -Recurse | Measure-Object ).Count;
    Write-Host " File count: $($count)";
}


if ($CleanBinAndObj) {
    Clean "$($Directory)\bin"
    Clean "$($Directory)\obj"
} else {
    Clean "$($Directory)"
}
