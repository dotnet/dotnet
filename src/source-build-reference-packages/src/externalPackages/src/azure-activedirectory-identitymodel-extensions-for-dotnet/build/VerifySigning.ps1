param([string]$root, [string]$buildType="Debug")

if(($root -eq $null) -or ($root -eq [System.String]::Empty))
{
    $root = $PSScriptRoot + "\.."
}

$srcPath = $root + "\src"

$snTool = $null
if([System.IO.File]::Exists("C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.8 Tools\sn.exe"))
{
    $snTool = Get-ChildItem "C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.8 Tools\sn.exe"
}

$snToolx64 = $null
if([System.IO.File]::Exists("C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.8 Tools\x64\sn.exe"))
{
    $snToolx64 = Get-ChildItem "C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.8 Tools\x64\sn.exe"
}

if (($snTool -eq $null) -or ($snToolx64 -eq $null))
{
    Write-Host ">>> Searching for sn.exe..."
    $snTools = Get-ChildItem ${env:ProgramFiles(x86)}\sn.exe -recurse -ErrorAction Ignore | Sort-Object LastWriteTime -descending
    foreach ($tool in $snTools)
    {
        if ($tool.DirectoryName.Contains("x64") -and ($snToolx64 -eq $null))
        {
            $snToolx64 = $tool
        }
        elseif ((-not $tool.DirectoryName.Contains("x64")) -and ($snTool -eq $null))
        {
            $snTool = $tool
        }
    
        if (($snTool -ne $null) -and ($snToolx64 -ne $null))
        {
            break
        }
    }
    
    if (($snTool -eq $null) -and ($snToolx64 -eq $null))
    {
        Write-Error ">>> Can not find strong name tool..."
        exit $LASTEXITCODE
    }
}


$signTool = $null
if([System.IO.File]::Exists("C:\Program Files (x86)\Windows Kits\10\bin\10.0.17763.0\x86\signtool.exe"))
{
    $signTool = Get-ChildItem "C:\Program Files (x86)\Windows Kits\10\bin\10.0.17763.0\x86\signtool.exe"
}

if ($signTool -eq $null)
{
    Write-Error ">>> Searching for signtool.exe..."
    $signTools = Get-ChildItem ${env:ProgramFiles(x86)}\signtool.exe -recurse -ErrorAction Ignore | Sort-Object LastWriteTime -descending

    foreach ($tool in $signTools)
    {
        if ($tool.DirectoryName.Contains("x64"))
        {
            $signTool = $tool
            break
        }
    }

    if ($signTool -eq $null)
    {
        Write-Error ">>> Can not find signtool.exe..."
        exit $LASTEXITCODE
    }
}

Write-Host "Verify Signing..."

$snParams = @("-vf", "")
$signParams = @("verify", "/pa", "")

[xml]$buildConfiguration = Get-Content $root\buildConfiguration.xml
$projects = $buildConfiguration.SelectNodes("root/projects/src/project")
$runtimes = $buildConfiguration.root.runtimes.Split(",")
$exitCode = 0

foreach ($project in $projects)
{
    foreach ($runtime in $runtimes)
    {
        $name = $project.name
        $file = Get-ChildItem $srcPath\$name\bin\$buildType\$runtime\$name.dll 2>&1

        if (-not $?)
        {
            Write-Warning ("Assembly not found: " + $name + "(" + $runtime + ")")
            Continue
        }

        Write-Host ("Verifing: " +  $file.FullName)
        if ( $? )
        {
            $snParams[1] = $file
            $unSigned = $false
            if ($snTool -ne $null)
            {
                $x = & "$snTool" $snParams 2>&1
                if (-not $?)
                {
                    $unSigned = $true
                    $exitCode += $LASTEXITCODE
                }
            }
            if ($snToolx64 -ne $null)
            {
                $x = & "$snToolx64" $snParams 2>&1
                if (-not $?)
                {
                    $unSigned = $true
                    $exitCode += $LASTEXITCODE
                }
            }

            if ($unSigned)
            {
                Write-Error ($file.FullName + " is not correctly strong-named signed.")
            }

            $signParams[2] = $file
            $x = & "$signTool" $signParams 2>&1
            if (-not $?)
            {
                Write-Error ($file.FullName + " is not Authentication signed.")
            }
        }
    }
}

Write-Host "Verify Signing - Done."

exit $exitCode
