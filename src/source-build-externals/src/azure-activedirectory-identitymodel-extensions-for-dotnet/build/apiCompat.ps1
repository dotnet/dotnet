param(
  [string]$feedLocation = "ADO", # or 'ADO' or 'NuGet'
  [string]$packageNames="", # comma-separated list of packages
  [string]$packageVersion="latest", # or the exact version e.g. "5.4.0-preview"
  [string]$adoFeedSource="", # required only for ADO feeds
  [string]$adoFeedName="", # required only for ADO feeds
  [string]$adoFeedUsername="VssSessionToken", # required only for ADO feeds
  [string]$adoFeedPat="", # required only for ADO feeds
  [string]$apiCompatRoot="$PSScriptRoot\tools\ApiCompat",
  [string]$nugetPackageProviderVersion="2.8.5.208",
  [string]$adoAllowPrerelease ="false" # uses the latest release (including preview releases) if packageVersion is 'latest'
)

################################################# Functions ############################################################

function CreateBasicAuthHeader {
Param(
  [string]$Username,
  [string]$PAT
)

$Auth = '{0}:{1}' -f $Username, $PAT
$Auth = [System.Text.Encoding]::UTF8.GetBytes($Auth)
$Auth = [System.Convert]::ToBase64String($Auth)
$Header = @{Authorization=("Basic {0}" -f $Auth)} 
$Header
}

function CreatePsCredential {
Param(
  [string]$Username,
  [string]$PAT
)

  $password = ConvertTo-SecureString $PAT -AsPlainText -Force
  $pSCredential = New-Object System.Management.Automation.PSCredential $Username, $password
  $pSCredential
}

function CreateFolder {
Param(
  [string]$Folder
)
  if (Test-Path($folder))
  {
      Write-Host ">>> $folder already exists!"
  }
  else  
  {
      Write-Host ">>> New-Item -ItemType directory $Folder | Out-Null"
      New-Item -ItemType directory $Folder | Out-Null
  }
}

function DownloadPackage {
Param(
  [string]$PackageDownloadUrl,
  [string]$OutFile,
  [System.Collections.IDictionary]$Headers,
  [string]$apiCompatRoot
)
  Write-Host ">>> Invoke-WebRequest -Uri $PackageDownloadUrl -OutFile $OutFile -Headers $Headers"
  Invoke-WebRequest -Uri $PackageDownloadUrl -OutFile $OutFile -Headers $Headers
}

function FormAdoPackageDownloadUrl {
Param(
  [string]$FeedSource,
  [string]$FeedName,
  [string]$PackageName,
  [string]$PackageVersion
)

  $scheme = ([System.Uri]$FeedSource).Scheme
  $baseUrl = ([System.Uri]$FeedSource).Host
  $packageDownloadUrl = "$scheme`://$baseUrl/_apis/packaging/feeds/$FeedName/nuget/packages/$PackageName/versions/$PackageVersion/content"
  Write-Host ">>> Formed ADO package download URL: $packageDownloadUrl"
  $packageDownloadUrl
}

function FormNugetPackageDownloadUrl {
  Param(
      [string]$PackageName,
      [string]$PackageVersion
  )

  if ($PackageVersion -Eq 'latest') {
      $packageDownloadUrl = "https://www.nuget.org/api/v2/package/$PackageName"
  } else {
      $packageDownloadUrl = "https://www.nuget.org/api/v2/package/$PackageName/$PackageVersion"
  }

  Write-Host ">>> Formed NuGet package download URL: $packageDownloadUrl"
  $packageDownloadUrl
}

function PlaceContractAssemblies([String] $apiCompatRoot) {
  Get-ChildItem "$apiCompatRoot\unzippedPackages" -Recurse -Include '*.dll' | Foreach-Object `
  {
      # resolve target framework, destination directory and destination assembly file path
      $targetFramework = Split-Path $_.Directory -leaf
      $contractAssembliesPath = "$apiCompatRoot\contractAssemblies"
      $destDir = Join-Path -Path $contractAssembliesPath -ChildPath $targetFramework

      CreateFolder($destDir)

      Write-Host ">>> Copy-Item $_ -Destination $destDir"
      Copy-Item $_ -Destination $destDir
  }
}

function RemoveFolder {
Param(
  [string]$Folder
)
  if (Test-Path($Folder)) {
      
      Write-Host ">>> Remove-Item -Recurse -Force $Folder -Confirm:$false"   
      Remove-Item -Recurse -Force $Folder -Confirm:$false 
  } else {
      Write-Host ">>> $Folder doesn't exist!"
  }
}

function UzipPackage {
Param(
  [string]$Package,
  [string]$DestinationPath
)
  Write-Host ">>> Expand-Archive -Path $Package -DestinationPath $DestinationPath -Force"
  Expand-Archive -Path $Package -DestinationPath $DestinationPath -Force
}

################################################# Functions ############################################################

Write-Host (">>> Start ApiCompat - Prepare contract assemblies (v2) - parameters");
Write-Host "feedLocation:                   " $feedLocation;
Write-Host "adoFeedSource:                  " $adoFeedSource;
Write-Host "adoFeedName:                    " $adoFeedName;
Write-Host "adoFeedUsername:                " $adoFeedUsername;
Write-Host "packageNames:                   " $packageNames;
Write-Host "packageVersion:                 " $packageVersion;
Write-Host "apiCompatRoot:                  " $apiCompatRoot;
Write-Host "nugetPackageProviderVersion:    " $nugetPackageProviderVersion;
Write-Host "adoAllowPrerelease:             " $adoAllowPrerelease;
Write-Host (">>> End ApiCompat - Prepare contract assemblies (v2) - parameters");

if ($feedLocation -Eq 'ADO' -And ($adoFeedPat -Eq '' -Or $adoFeedSource -Eq '' -Or $adoFeedName -Eq '' -Or $adoFeedUsername -Eq '')) {
  throw ">>> adoFeedPat, adoFeedSource, adoFeedName, and adoFeedUsername are required when feed location is set to 'ADO'. Run the script again and set the required values."
}

if ($packageNames -Eq '') {
  throw ">>> List of packageNames is empty. Run the script again and set the packageNames."
}

$packageNamesArray = $packageNames.split(" ")

if ($packageVersion -Eq 'latest') {
  $useLatestVersion = 'true'
} else {
  $useLatestVersion = 'false'
}

# determine the latest package version from an ADO feed
if ($packageVersion -Eq 'latest' -And $feedLocation -Eq "ADO") {
  $nugetPackageProviderResult = Get-PackageProvider -Name Nuget -ErrorAction SilentlyContinue
  if ($null -Eq $nugetPackageProviderResult -Or $nugetPackageProviderResult.Version -ne [System.Version]$nugetPackageProviderVersion) {
      Write-Host ">>> Install-PackageProvider Nuget -RequiredVersion $nugetPackageProviderVersion -Force -Scope CurrentUser | Out-Null"
      Install-PackageProvider Nuget -RequiredVersion $nugetPackageProviderVersion -Force -Scope CurrentUser | Out-Null
  } else {
      Write-Host (">>> Nuget package provider (" + $nugetPackageProviderResult.Version + ") is already installed.")
  }
  
  $credential = CreatePsCredential -Username $adoFeedUsername -PAT $adoFeedPat

  $getFeedResult = Get-PSRepository -Name $adoFeedName -ErrorAction SilentlyContinue
  if ($null -Eq $getFeedResult) {
      Write-Host ">>> Register-PSRepository -Name $adoFeedName -SourceLocation $adoFeedSource -InstallationPolicy Trusted -Credential $credential"
      Register-PSRepository -Name $adoFeedName -SourceLocation $adoFeedSource -InstallationPolicy Trusted -Credential $credential
  } else
  {
      Write-Host ">>> Feed $adoFeedName is already registered."
  }
}

# prepare directories
RemoveFolder -Folder "$apiCompatRoot\contractAssemblies"
CreateFolder -Folder "$apiCompatRoot\contractAssemblies"
CreateFolder("$apiCompatRoot\downloadedPackages")

# download and unzip packages
foreach($packageName in $packageNamesArray) {
  $packageName = $packageName.trim()
  $outFile = "$apiCompatRoot\downloadedPackages\$packageName.zip"
  $unzippedDir = "$apiCompatRoot\unzippedPackages\$packageName"

  if ($useLatestVersion -Eq 'true' -And $feedLocation -Eq "ADO") {
      if ($adoAllowPrerelease -Eq 'true') {
          $packageVersion = (Find-Module -Name $packageName -Repository $adoFeedName -Credential $credential -AllowPrerelease)[0].Version
      } else {
          $packageVersion = (Find-Module -Name $packageName -Repository $adoFeedName -Credential $credential)[0].Version
      }
  }

  Write-Host (">>> Latest " + $packageName + " version: $packageVersion")

  if ($feedLocation -eq 'ADO') {
      $header = CreateBasicAuthHeader -Username $adoFeedUsername -PAT $adoFeedPat
      $downloadPackageUrl = FormAdoPackageDownloadUrl -FeedSource $adoFeedSource -FeedName $adoFeedName -PackageName $packageName -PackageVersion $PackageVersion
      DownloadPackage -packageDownloadUrl $downloadPackageUrl -header $header -outFile $outFile -apiCompatRoot $apiCompatRoot
  } else {
      $downloadPackageUrl = FormNugetPackageDownloadUrl -PackageName $packageName -PackageVersion $PackageVersion
      DownloadPackage -packageDownloadUrl $downloadPackageUrl -outFile $outFile -apiCompatRoot $apiCompatRoot
  }

  UzipPackage -package $outFile -destinationPath $unzippedDir
}

# place the contract assemblies and clean-up
PlaceContractAssemblies($apiCompatRoot)
RemoveFolder -Folder "$apiCompatRoot\downloadedPackages"
RemoveFolder -Folder "$apiCompatRoot\unzippedPackages"

Write-Host ">>> Done - ApiCompat - Prepare contract assemblies (v2)."