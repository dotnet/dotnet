Write-Host "Regenerating all generated files from all .tt files."
Write-Host "(requires t4.exe from 'dotnet tool install --global dotnet-t4' or right click .tt files in VS and 'run custom tool')"
Write-Host ""
$ttFiles = Get-ChildItem -Filter *.tt -Recurse -File -Name
foreach ($ttFile in $ttFiles)
{
    if (-not $ttFile.endswith("AssemblySourceFileGenerator.tt"))
    {
        Write-Host t4.exe $ttFile
        t4.exe "$ttFile"
    }
}
