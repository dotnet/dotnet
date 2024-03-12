Param(
    [Parameter(Mandatory=$true,HelpMessage="Path to releaseMetaData.xml")]
    [string]
    $metadataPath,

	[Parameter(Mandatory=$true,HelpMessage="Github Repo Name")]
    [string]
    $gitHubRepository,

	[Parameter(Mandatory=$true,HelpMessage="Github Username with Write permissions")]
    [string]
    $gitHubUsername,

	[Parameter(Mandatory=$true,HelpMessage="Github User's personal api token (https://github.com/blog/1509-personal-api-tokens)")]
    [string]
    $gitHubApiKey
) 

# SUMMARY
# This script will parse a metadata file (releaseMetaData.xml) and generate the release notes.
# The metadata contains a copy of the relevant changelog as well as the commit id.
# The commit id is required to create a release tag in github.

# DEVELOPER notes
# In the release definition, set metadataPath = "$(SYSTEM.ARTIFACTSDIRECTORY)";


function Push-Github {
    #https://developer.github.com/v3/repos/releases/#create-a-release
    Param (
        [string]$tagName,
        [string]$releaseName,
        # The Commit SHA for corresponding to this release
        [string]$commitId,
        # The notes to accompany this release, uses the commit message in this case
        [string]$releaseNotes,
        # The github username
        [string]$gitHubUsername,
        # The github repository name
        [string]$gitHubRepository,
        # The github API key (https://github.com/blog/1509-personal-api-tokens)
        [string]$gitHubApiKey,
        # Set to true to mark this as a pre-release version
        [bool]$preRelease = $TRUE,
        # Set to true to mark this as a draft release (not visible to users)
        [bool]$draft = $FALSE
    )

    $releaseData = @{
        tag_name = $tagName;
        target_commitish = $commitId;
        name = $releaseName;
        body = $releaseNotes;
        draft = $draft;
        prerelease = $preRelease;
    }

    $authPair = "$($gitHubUsername):$($gitHubApiKey)"
    $encodedCreds = [System.Convert]::ToBase64String([System.Text.Encoding]::ASCII.GetBytes($authPair));
    $basicAuthValue = "Basic $encodedCreds"

    $releaseCmd = @{
        Uri = "https://api.github.com/repos/$gitHubRepository/releases";
        Method = 'POST';
        Headers = @{
            Authorization = $basicAuthValue
        }
        ContentType = 'application/json';
        Body = (ConvertTo-Json $releaseData -Compress)
    }

    [Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12
    Invoke-RestMethod @releaseCmd 
}



if($metadataPath -notlike "releaseMetaData.xml") {
	Write-Verbose "'releaseMetaData.xml' not part of MetaDataPath: $metadataPath"
	Write-Verbose "Searching..."
    #assume this is Artifact Directory and find the required file
    $items = Get-ChildItem -Path $metadataPath -Recurse -Filter "releaseMetaData.xml"
	$items | ForEach-Object { Write-Verbose "Found: $($_.FullName)" }
    $metadataPath = $items[0].FullName
}
Write-Verbose "MetaDataPath: $metadataPath"

if (Test-Path $metadataPath) {
	
	[xml]$metaData = Get-Content $metadataPath

	$commitId = $metaData.MetaData.CommitId;
	$tagName = $metaData.MetaData.ReleaseName;
	$releaseName = $metaData.MetaData.FormattedReleaseName;
	$releaseNotes = $metaData.MetaData.ChangeLog;
	$isPreRelease = [System.Convert]::ToBoolean($metaData.MetaData.isPreRelease);

	Push-Github -tagName $tagName -releaseName $releaseName -commitId $commitId -releaseNotes $releaseNotes -gitHubUsername $gitHubUsername -gitHubRepository $gitHubRepository -gitHubApiKey $gitHubApiKey -preRelease $isPreRelease;

} else {
	Write-Error "MetaData Not Found."
}