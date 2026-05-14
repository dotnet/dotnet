# This takes docs.md and helps create/update files for the dotnet docs repo.
# https://github.com/dotnet/docs/blob/master/CONTRIBUTING.md#process-for-contributing

param (
    [Parameter(Mandatory=$true)][string]$DotnetDocsRootDir
 )

$mdFile = "src\NuGet.Core\NuGet.CommandLine.XPlat\external\docs.md"
$outFile = "";
$todayShortDate = Get-Date -Format "MM/dd/yyyy"

foreach($line in [System.IO.File]::ReadLines($mdFile))
{
    if ($line.StartsWith("---file:"))
    {
        $relativeDestFilePath = $line.SubString(8)
        $outFile = "$DotnetDocsRootDir\$relativeDestFilePath"
        Remove-Item "$outFile"
        New-Item "$outFile" -ItemType file
    }
    elseif ($line.StartsWith("ms.date:"))
    {
        Add-Content "$outFile" "ms.date: $todayShortDate" -Encoding ASCII 
    }
    elseif ($line.StartsWith("***"))
    {
        #skip each line of the instructions at top of docs.md file
    }
    else
    {
        Add-Content "$outFile" $line -Encoding ASCII             
    }
}
