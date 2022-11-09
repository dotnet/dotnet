# Generate Contract Assemblies, for each Target Framework, which will be used as reference assemblies during ApiCompat validation
# Directory structure under contractAssemblies will be as following:
# $contractAssembliesPath\{TargetFramework}\{ReferenceAssembly.dll}

$implementationAssembliesRootPath = "$PSScriptRoot\src"
$contractAssembliesPath = "$PSScriptRoot\Tools\apiCompat\contractAssemblies"

Write-Host "============================ `n"
Write-Host "implementationAssembliesRootPath: $implementationAssembliesRootPath"
Write-Host "contractAssembliesPath:           $contractAssembliesPath `n"

# remove existing contract assemblies
if (Test-Path $contractAssembliesPath)
{
    Write-Host ">>> Remove-Item  -Recurse -Force $contractAssembliesPath"
    Remove-Item  -Recurse -Force $contractAssembliesPath
}

# create contractAssembliesDir
Write-Host ">>> mkdir $contractAssembliesPath"
mkdir $contractAssembliesPath | Out-Null

# recursively iterate implAssembliesRootPath and include DLLs whose name contain 'IdentityModel'
Get-ChildItem $implementationAssembliesRootPath -Recurse -Include '*IdentityModel*.dll' | Foreach-Object `
{
    # get partialAssemblyName - remove text before the first dot e.g. System.IdentityModel.Tokens.Jwt -> IdentityModel.Tokens.Jwt
    # this hack is in place as there are cases when a project is producing an assembly with a name different than its name (special builds)
    $null = $_.Name -match "^*\.(?<partialName>.*).dll$"
    $partialAssemblyName = $matches["partialName"]

    # continue if source path [string] of current item (assembly) doesn't contain partialAssemblyName
    # we don't want assemblies that don't belong to IdentityModel-extensions solution in contractAssemblies e.g. 'Microsoft.IdentityModel.Clients.ActiveDirectory.dll'
    if (!($_.Directory -match $partialAssemblyName))
    {
        # think continue;
        return
    }

    # resolve target framework, destination directory and destination assembly file path
    $targetFramework = Split-Path $_.Directory -leaf
    $destDir = Join-Path -Path $contractAssembliesPath -ChildPath $targetFramework
    $destAssemblyFilePath = Join-Path -Path $destDir -ChildPath $_.Name

    # create directory if it doesn't exist already
    if (!(Test-Path $destDir))
    {
        Write-Host ">>> New-Item -ItemType directory $destDir | Out-Null"
        New-Item -ItemType directory $destDir | Out-Null
    }

    # if an assembly with the same name as the current item (assembly) already exists in destination dir
    # overwrite it only if curent item's LastWriteTime is greater than LastWriteTime of an existing assembly
    if (Test-Path $destAssemblyFilePath)
    {
        $destAssemblyFile = Get-Item $destAssemblyFilePath

        if ($_.LastWriteTime -gt $destAssemblyFile.LastWriteTime)
        {
            Write-Host ">>> Copy-Item $_ -Destination $destDir"
            Copy-Item $_ -Destination $destDir
        }
    }
    else # copy assembly to destination dir
    {
        Write-Host ">>> Copy-Item $_ -Destination $destDir"
        Copy-Item $_ -Destination $destDir
    }
}

Write-Host "`nDone!`n"
