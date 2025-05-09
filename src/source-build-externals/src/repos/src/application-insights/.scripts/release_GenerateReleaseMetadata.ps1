Param(
    [Parameter(Mandatory=$true,HelpMessage="Path to Artifact files (nupkg):")]
    [string]
    $artifactsPath,
    [Parameter(Mandatory=$true,HelpMessage="Path to Source files (changelog):")]
    [string]
    $sourcePath,
    [Parameter(Mandatory=$true,HelpMessage="Path to save metadata:")]
    [string]
    $outPath
) 

# SUMMARY
# This script will inventory the nuget packages output by a build and create a metadata file (releaseMetaData.xml).
# This metadata file must be published with the NUPKGs when archived.
# This metadata file is used by the release infrastructure and informs it on how to publish NUPKGs.
# This script is included in Gated builds for troubleshooting.


class PackageInfo {
    [string]$Name
    [string]$NuspecVersion
    [string]$DllVersion
    [string]$MyGetUri

    PackageInfo([string]$name, [string]$nuspecVersion, [string]$dllVersion) {
        $this.Name = $name
        $this.NuspecVersion = $nuspecVersion
        $this.DllVersion = $dllVersion

        $mygetBasePath = "https://www.myget.org/feed/applicationinsights/package/nuget/{0}/{1}"
        $this.MyGetUri = $mygetBasePath -f $name, $nuspecVersion
    }
}

class ReleaseInfo {
    [string]$ReleaseName
    [string]$ReleaseVersion
    [string]$NuspecVersion
    [string]$FormattedReleaseName
    [bool]$IsPreRelease
    [string]$CommitId
    [string]$ChangeLog
    [PackageInfo[]]$Packages
}

Function Get-GitChangeset() {
    # if running localy, use git command. for VSTS, probably better to use Build.SourceVersion
    # Git command only works if this script executes in the repo's directory
    [string]$commit = ""
    try {
        $commit = $(Build.SourceVersion)
    } catch {
        try {
            $commit = git log -1 --format=%H
        } catch {
            $commit = "not found"
        }
    }

    Write-Host "Git Commit: $commit"
    return [string]$commit
}

function Get-NuspecVersionName ([string]$version) {
    # get everything up to word "-build" (ex: "1.2.3-beta1-build1234  returns: "1.2.3-beta1")
    # get everything (ex: "1.2.3-beta1  returns: "1.2.3-beta1")
    # get everything (ex: "1.2.3  returns: "1.2.3")
    $splitVersion = $version.split('-')
    if($splitVersion.Length -gt 2 ) {
        return $splitVersion[0]+"-"+$splitVersion[1]
    } else {
        return $version
    }
}

function Invoke-UnZip([string]$zipfile, [string]$outpath) {
    Write-Verbose "Unzip - source: $zipfile"
    Write-Verbose "Unzip - target: $outpath"
    Add-Type -assembly "system.io.compression.filesystem"
    [System.IO.Compression.ZipFile]::ExtractToDirectory($zipfile, $outpath)
}

function Get-PackageInfoFromNupkg([string]$nupkgPath) {
    $unzipPath = $nupkgPath+"_unzip"
    $null = Invoke-UnZip $nupkgPath $unzipPath

    $nuspecPath = Get-ChildItem -Path $unzipPath -Recurse -Filter *.nuspec | Select-Object -First 1
    Write-Verbose ("Found Nuspec: " + $nuspecPath.FullName)
    [xml]$nuspec = Get-Content $nuspecPath.FullName
    $name = $nuspec.package.metadata.id
    $nuspecVersion = $nuspec.package.metadata.version

    $dllPath = Get-ChildItem -Path $unzipPath -Recurse -Filter *.dll | Select-Object -First 1
    Write-Verbose ("Found Dll: " + $dllPath.FullName)
    $dllVersion = [System.Diagnostics.FileVersionInfo]::GetVersionInfo($dllPath.FullName).FileVersion

    #cleanup
    Remove-Item -Recurse -Force $unzipPath

    return [PackageInfo]::new($name, $nuspecVersion, $dllVersion)
}

function Get-ReleaseMetaData ([string]$artifactsPath, [string]$sourcePath) {
    $object = [ReleaseInfo]::new()
    $object.CommitId = Get-GitChangeset
    $object.Packages = $()

    Get-ChildItem -Path $artifactsPath -Recurse -Filter *.nupkg -Exclude *.symbols.nupkg | 
        ForEach-Object { $object.Packages += Get-PackageInfoFromNupkg $_.FullName }

    $object.NuspecVersion = $object.Packages[0].NuspecVersion
    $object.ReleaseName = Get-NuspecVersionName($object.Packages[0].NuspecVersion)
    $object.ReleaseVersion = $object.Packages[0].DllVersion

    $object.FormattedReleaseName = "$($object.ReleaseName) ($($object.ReleaseVersion))"

    $object.IsPreRelease = [bool]($object.ReleaseName -like "*beta*")
    
    $object.ChangeLog = Get-ChangelogText $sourcePath $object.ReleaseName
    return $object
}

Function Get-ChangelogText ([string]$sourcePath, [string]$versionName) {
    $sb = [System.Text.StringBuilder]::new()
    $saveLines = $false
    $readFile = $true

    $changelogPath = Get-ChildItem -Path $sourcePath -Recurse -Filter changelog.md | Select-Object -First 1
    Write-Verbose "Changelog Found: $($changelogPath.FullName)"
	Write-Verbose "Searching for: $versionName";

    Get-Content -Path $changelogPath.FullName | ForEach-Object {
        
        if($readFile) {
        
            if($saveLines) {
                if($_ -like "##*") {
                    Write-Verbose "STOP at $_"
                    $readFile = $false
                }
                
                if($readFile) {
                    [void]$sb.AppendLine($_)
                }
            } else {
				if($_ -like "##*")  {
					Write-Verbose "Inspecting $_"
				
					if ($_ -match $versionName) {
						$saveLines = $true
						Write-Verbose "START at $_"
					}
				}
            }
        }
    
    }
    return $sb.ToString()
}

Function Save-ToXml([string]$outPath, [ReleaseInfo]$object) {
    
    if ( -not (Test-Path -Path $outPath -PathType Container) )
    {
        throw "Output directory does not exist: '$($outPath)'."
    }
    
    $outFilePath = Join-Path $outPath "releaseMetaData.xml"
    $xmlWriter = [System.XMl.XmlTextWriter]::new($outFilePath, $Null)
    $xmlWriter.Formatting = "Indented"
    $xmlWriter.Indentation = 1
    $XmlWriter.IndentChar = "`t"
     
    # write the header
    $xmlWriter.WriteStartDocument()
     
    # create root node:
    $xmlWriter.WriteStartElement("MetaData")
     
    $XmlWriter.WriteElementString("ReleaseName", $object.ReleaseName)
    $XmlWriter.WriteElementString("ReleaseVersion", $object.ReleaseVersion)
    $XmlWriter.WriteElementString("FormattedReleaseName", $object.FormattedReleaseName)
    $XmlWriter.WriteElementString("NuspecVersion", $object.NuspecVersion)
    $XmlWriter.WriteElementString("IsPreRelease", $object.IsPreRelease)
    $XmlWriter.WriteElementString("CommitId", $object.CommitId)
    
    $XmlWriter.WriteElementString("ChangeLog", $object.ChangeLog)
    
    $XmlWriter.WriteStartElement("Packages")
    $object.Packages | ForEach-Object {
        $XmlWriter.WriteStartElement("Package")
        $XmlWriter.WriteElementString("Name", $_.Name)
        $XmlWriter.WriteElementString("NuspecVersion", $_.NuspecVersion)
        $XmlWriter.WriteElementString("DllVersion", $_.DllVersion)
        $XmlWriter.WriteElementString("MyGetUri", $_.MyGetUri)
        $XmlWriter.WriteEndElement()
    }
    $XmlWriter.WriteEndElement()
     
    # close the root node:
    $xmlWriter.WriteEndElement()
     
    # finalize the document:
    $xmlWriter.WriteEndDocument()
    $xmlWriter.Flush()
    $xmlWriter.Close()
}

# 1) GET META DATA FROM ARTIFACTS
$metaData = Get-ReleaseMetaData $artifactsPath $sourcePath
Write-Output $metaData
$metaData.Packages | ForEach-Object { Write-Output $_ }

# 2) SAVE
Save-ToXml $outPath $metaData