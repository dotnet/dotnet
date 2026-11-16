param(
    [Parameter(Mandatory)]
    [string] $SignedCabPath
)

$ErrorActionPreference = 'Stop'
$PSNativeCommandUseErrorActionPreference = $false
if ($env:OS -ne 'Windows_NT') {
    throw 'This regression requires Windows Authenticode validation.'
}
$cab = Get-Item -LiteralPath $SignedCabPath
if ($cab.Extension -ne '.cab' -or (Get-AuthenticodeSignature -LiteralPath $cab.FullName).Status -ne 'Valid') {
    throw 'Provide the valid, signed IntelliTrace CAB used by the package build.'
}

$restore = $true
$ci = $false
$configuration = 'Release'
$msbuildEngine = 'dotnet'
$disableConfigureToolsetImport = $true
. $PSScriptRoot\common\tools.ps1
$dotnet = Join-Path (InitializeDotNetCli $true) (GetExecutableFileName 'dotnet')
$validationProject = GetSdkTaskProject 'SigningValidation'
[xml]$signingProperties = Get-Content -LiteralPath (Join-Path $PSScriptRoot 'Signing.props') -Raw
$configuredTaskVersion = $signingProperties.Project.PropertyGroup.MicrosoftDotNetSignCheckTaskVersion
if (-not $configuredTaskVersion) {
    throw 'The repository does not declare a SignCheck task version.'
}
$id = [guid]::NewGuid().ToString('N')
$root = Join-Path $TempDir "cab-signing-$id"
$reports = Join-Path $LogDir "cab-signing-$id"
$null = New-Item -ItemType Directory -Path $root, $reports

function Invoke-Validation([string] $Name, [string] $Configuration, [string] $InputDirectory, [string] $TaskVersion = '') {
    $reportDirectory = Join-Path $reports "$Configuration-$Name"
    $null = New-Item -ItemType Directory -Path $reportDirectory
    $arguments = @(
        'msbuild', $validationProject, '-nologo', '-nr:false', '-verbosity:minimal',
        "/p:RepoRoot=$RepoRoot", "/p:Configuration=$Configuration",
        "/p:BaseIntermediateOutputPath=$root\$Configuration-$Name\",
        "/p:PackageBasePath=$InputDirectory",
        '/p:SignCheckExclusionsFile=',
        "/p:SignCheckLog=$reportDirectory\signcheck.log",
        "/p:SignCheckErrorLog=$reportDirectory\signcheck.errors.log",
        "/p:SignCheckResultsXmlFile=$reportDirectory\signcheck.xml"
    )
    if ($TaskVersion) {
        $arguments += "/p:MicrosoftDotNetSignCheckTaskVersion=$TaskVersion"
    }
    $expectedVersion = if ($TaskVersion) { $TaskVersion } else { $configuredTaskVersion }
    $evaluatedVersion = (& $dotnet @arguments -getProperty:MicrosoftDotNetSignCheckTaskVersion) -join ''
    if ($LASTEXITCODE -ne 0 -or $evaluatedVersion.Trim() -ne $expectedVersion) {
        throw "SigningValidation did not select the expected task version $expectedVersion."
    }
    & $dotnet @arguments -t:Restore | Out-Host
    if ($LASTEXITCODE -ne 0) {
        throw "Could not restore $Configuration/$Name."
    }
    $output = (& $dotnet @arguments -t:Execute 2>&1) -join "`n"
    $exitCode = $LASTEXITCODE
    [IO.File]::WriteAllText((Join-Path $reportDirectory 'execute.log'), $output)
    [xml]$report = Get-Content -LiteralPath (Join-Path $reportDirectory 'signcheck.xml') -Raw
    return [pscustomobject]@{
        ExitCode = $exitCode
        Files = @($report.SelectNodes('//File'))
        ErrorLog = Get-Content -LiteralPath (Join-Path $reportDirectory 'signcheck.errors.log') -Raw
    }
}

try {
    $signed = Join-Path $root 'signed'
    $unsigned = Join-Path $root 'unsigned'
    $fixture = Join-Path $root 'fixture'
    $null = New-Item -ItemType Directory -Path $signed, $unsigned, $fixture
    Copy-Item -LiteralPath $cab.FullName -Destination (Join-Path $signed $cab.Name)
    foreach ($file in @('Directory.Build.props', 'Directory.Build.targets')) {
        [IO.File]::WriteAllText((Join-Path $fixture $file), '<Project />')
    }
    $framework = 'net' + ([version]($GlobalJson.tools.dotnet.Split('-')[0])).Major + '.0'
    $project = Join-Path $fixture 'UnsignedFixture.csproj'
    [IO.File]::WriteAllText($project, @"
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>$framework</TargetFramework>
    <PackageId>UnsignedFixture</PackageId>
    <Version>1.0.0</Version>
  </PropertyGroup>
</Project>
"@)
    [IO.File]::WriteAllText((Join-Path $fixture 'Fixture.cs'), 'namespace SigningValidationFixture { public sealed class UnsignedFixture { } }')
    & $dotnet pack $project -nologo -verbosity:quiet -o $unsigned
    if ($LASTEXITCODE -ne 0 -or -not (Test-Path -LiteralPath (Join-Path $unsigned 'UnsignedFixture.1.0.0.nupkg'))) {
        throw 'Could not build the valid unsigned package fixture.'
    }

    foreach ($buildConfiguration in @('Debug', 'Release')) {
        $old = Invoke-Validation 'original-cab-reader' $buildConfiguration $signed '11.0.0-beta.26177.6'
        $oldCab = @($old.Files | Where-Object { $_.GetAttribute('Name') -like "*$($cab.Name)" })
        if ($old.ExitCode -eq 0 -or $oldCab.Count -ne 1 -or
            $oldCab[0].GetAttribute('Outcome') -ne 'Unsigned' -or $oldCab[0].GetAttribute('Error') -ne 'Unknown file format.') {
            throw "The original CAB-reader failure was not reproduced in $buildConfiguration."
        }
        Write-Host "$buildConfiguration : original task incorrectly rejects the Authenticode-valid CAB."

        $fixed = Invoke-Validation 'signed-cab' $buildConfiguration $signed
        $fixedCab = @($fixed.Files | Where-Object { $_.GetAttribute('Name') -like "*$($cab.Name)" })
        if ($fixed.ExitCode -ne 0 -or $fixedCab.Count -ne 1 -or $fixedCab[0].GetAttribute('Outcome') -ne 'Signed' -or
            $fixed.ErrorLog -or @($fixed.Files | Where-Object {
                $_.GetAttribute('Outcome') -in @('Unsigned', 'Excluded', 'NotUnpacked') -or $_.HasAttribute('Error')
            }).Count -ne 0) {
            throw "The configured task did not validate the signed CAB without exclusions in $buildConfiguration."
        }
        Write-Host "$buildConfiguration : configured task accepts the signed CAB; inspected $($fixed.Files.Count) file(s)."

        $rejected = Invoke-Validation 'unsigned-package' $buildConfiguration $unsigned
        if ($rejected.ExitCode -eq 0 -or -not $rejected.ErrorLog) {
            throw "The configured task accepted the unsigned fixture in $buildConfiguration."
        }
        foreach ($name in @('UnsignedFixture.1.0.0.nupkg', 'UnsignedFixture.dll')) {
            $matches = @($rejected.Files | Where-Object {
                $_.GetAttribute('Name') -like "*$name" -and $_.GetAttribute('Outcome') -eq 'Unsigned'
            })
            if ($matches.Count -ne 1 -or $matches[0].HasAttribute('Error')) {
                throw "The real checker did not classify the valid $name as unsigned in $buildConfiguration."
            }
        }
        Write-Host "$buildConfiguration : configured task rejects the valid unsigned package and DLL; inspected $($rejected.Files.Count) files."
    }
    Write-Host "Signing reports: $reports"
}
finally {
    Remove-Item -LiteralPath $root -Recurse -Force
}
