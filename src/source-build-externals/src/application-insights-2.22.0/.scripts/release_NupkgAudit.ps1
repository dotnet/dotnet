
# This script checks the Authoring Requirements listed here: 
# Full Requirements: https://microsoft.sharepoint.com/teams/NuGet/MicrosoftWiki/MicrosoftPackages.aspx
# Authoring Requirements: https://microsoft.sharepoint.com/teams/NuGet/MicrosoftWiki/AuthoringRequirements.aspx
# Signing Requirements: https://microsoft.sharepoint.com/teams/NuGet/MicrosoftWiki/SigningMicrosoftPackages.aspx
Param(
    
    [Parameter(Mandatory=$true,HelpMessage="Path to Nupkg files:")]
    [string]
    $nupkgPath,
    
    [Parameter(Mandatory=$true,HelpMessage="Path to working directory:")]
    [string]
    $workingDir,

    [Parameter(Mandatory=$true,HelpMessage="Full Log?:")] #Include Pass messages with output?
    [bool]
    $verboseLog,

    [Parameter(Mandatory=$false,HelpMessage="Enable or disable signing verification:")] 
    [bool]
    $verifySigning = $true,

    [Parameter(Mandatory=$false,HelpMessage="Enable or disable signing verification:")] 
    [string]
    $expectedCertHash = ""
) 


$requiredCopyright = "$([char]0x00A9) Microsoft Corporation. All rights reserved.";#"© Microsoft Corporation. All rights reserved.";
$expectedProjectUrl = "https://go.microsoft.com/fwlink/?LinkId=392727"; # Application Insights Project Url
$expectedLicense = "MIT"; # MIT License SPDX ID
$expectedOwner = "AppInsightsSdk"; # Application Insights Nuget Account
$expectedTags = @("Azure","Monitoring");

$nugetExePath = "$PSScriptRoot\Nuget.exe";

$sb = [System.Text.StringBuilder]::new();

$script:isValid = $true;


# Get the latest Nuget.exe from here:
if (!(Test-Path $nugetExePath)) {

    Write-Host "Nuget.exe not found. Attempting download...";
    Write-Host "Start time:" (Get-Date -Format G);
    $downloadNugetUrl = "https://dist.nuget.org/win-x86-commandline/latest/nuget.exe";
    (New-Object System.Net.WebClient).DownloadFile($downloadNugetUrl, $nugetExePath);
    Write-Host "Finish time:" (Get-Date -Format G);
    
    if (!(Test-Path $nugetExePath)) {
        throw "Error: Nuget.exe not found! Please download latest from: https://www.nuget.org/downloads";
    }
}

function Write-Break() {
    $displayMessage = "========== ========== ========== ========== ==========";
    Write-Host "";
    Write-Host $displayMessage;
    $null = $sb.AppendLine();
    $null = $sb.AppendLine($displayMessage);
}

function Write-Name([string]$message) {
    Write-Host $message;
    $null = $sb.AppendLine($message);
}

function Write-IsValid ([string]$message) {
    if ($verboseLog) {
        $displayMessage = "`tPass:`t$message";
        Write-Host $displayMessage -ForegroundColor Green;
        $null = $sb.AppendLine($displayMessage);
    }
}

function Write-SemiValid ([string]$message, [string]$recommendedDescription) {
    $displayMessage = "`tReview:`t$message [Recommended: $recommendedDescription]";
    Write-Host $displayMessage -ForegroundColor Yellow;
    $null = $sb.AppendLine($displayMessage);
}

function Write-NotValid ([string]$message, [string]$requiredDescription) {
    $displayMessage = "`tFail:`t$message [Required: $requiredDescription]";
    Write-Host $displayMessage -ForegroundColor Red;
    $null = $sb.AppendLine($displayMessage);
	$script:isValid = $false;
}

function Test-Condition ([bool]$condition, [string]$message, [string]$requiredDescription) {
    if ($condition) {
        Write-IsValid $message
    } else {
        Write-NotValid $message $requiredDescription
    }
}

function Test-MultiCondition ([bool]$requiredCondition, [bool]$recommendedCondition, [string]$message, [string]$requiredDescription, [string]$recommendedDescription) {
    if ($requiredCondition) {
        if($recommendedCondition) {
            Write-IsValid $message
        } else {
            Write-SemiValid $message $recommendedDescription
        }
    } else {
        Write-NotValid $message $requiredDescription
    }
}

function Get-IsPackageSigned([string]$nupkgPath) {
    $verifyOutput = "";
    $null = & $nugetExePath verify -signature -CertificateFingerprint $expectedCertHash $nupkgPath -verbosity detailed 2>&1 | Tee-Object -Variable verifyOutput
    
	#TEST OUTPUT
	Write-Host $verifyOutput

    $output = $verifyOutput[$verifyOutput.Length-1]

    $success = ($output -like "Successfully verified*");

    if (!$success){
        $verifyOutput | Where-Object { $_ -like "NU*" -and $_ -notlike "NUGET*"} | ForEach-Object {
            $output = "$output $_" -replace "`n","" -replace "`r","";
        }
    }
    
    $message = "Is Signed: $output";
    $requirement = "Must be signed."

    Test-Condition ($success) $message $requirement;
}


function Get-IsDllSigned ([string]$dllPath) {
    $test = Get-AuthenticodeSignature $dllPath; # this is  equivalent to signtool.exe verify /pa
    $SignedStatus = $test.Status;
    $SignatureValid = [System.Management.Automation.SignatureStatus]::Valid;

    $message = "Is Signed: $SignedStatus";
    $requirement = "Must be signed."

    Test-Condition ($SignedStatus -eq $SignatureValid) $message $requirement;
}

function Get-DoesXmlDocExist ([string]$dllPath) {
    # CONFIRM .XML DOCUMENTATION FILE EXISTS WITH EACH DLL
    [string]$docFile = $dllPath -replace ".dll", ".xml";

    $message = "XML Documentation:";
    $requirement = "Must exist."
    Test-Condition (Test-Path $docFile) $message $requirement;
}

function Get-DoesXmlDocContainsLang ([string]$dllPath) {
    # CONFIRM .XML DOCUMENTATION FILE EXISTS WITH EACH DLL
    [string]$docFile = $dllPath -replace ".dll", ".xml";

    [bool]$result = $false;
    [string]$searchString = '<doc xml:lang="en">';

    if (Test-Path $docFile) {
        $result = select-string -path $docFile -Pattern $searchString -Quiet;
    }

    $message = "XML Documentation:";
    $requirement = "Must contain xml:lang='en' ";
    Test-Condition ($result) $message $requirement;
}

function Get-DoesPdbExist ([string]$dllPath) {
    # CONFIRM .PDB SYMBOLS EXIST WITH EACH DLL
    [string]$docFile = $dllPath -replace ".dll", ".pdb";

    $message = "Symbols in package:";
    $requirement = "Not a requirement, but made a decision to include these in packages."
    Test-Condition (Test-Path $docFile) $message $requirement;
}

function Get-DoesDllVersionsMatch ([string]$dllPath) {
    # CONFIRM Assembly version matches File version
    [string]$fileVersion = [System.Diagnostics.FileVersionInfo]::GetVersionInfo($dllPath).FileVersion;
    [string]$assemblyVersion = [Reflection.AssemblyName]::GetAssemblyName($dllPath).Version;

    $message = "File Version: '$fileVersion' Assembly Version: '$assemblyVersion";
    $requirement = "Versions should match."
    Test-Condition ([version]$fileVersion -eq [version]$assemblyVersion) $message $requirement;
}

function Get-IsValidPackageId([xml]$nuspecXml) {
    $id = $nuspecXml.package.metadata.id;

    $message = "Package Id: $id";
    $requirement = "Must begin with 'Microsoft.'"

    Test-Condition ($id.StartsWith("Microsoft.")) $message $requirement;
}

function Get-IsValidAuthors([xml]$nuspecXml) {
    $authors = $nuspecXml.package.metadata.authors;

    $message = "Authors: $authors";
    $requirement = "Microsoft must be the only author.";

    Test-Condition ($authors -eq "Microsoft") $message $requirement;
}

function Get-IsValidOwners([xml]$nuspecXml) {
    $owners = $nuspecXml.package.metadata.owners;
    $ownersList = $owners -split ',';

    $message = "Owners: $owners";
    $requirement = "Must include Microsoft."
    $recommendation = "Should include nuget owner."

    Test-MultiCondition ($ownersList -contains "Microsoft") ($ownersList -contains $expectedOwner -and $ownersList.Length -eq 2) $message $requirement $recommendation;
}

function Get-IsValidProjectUrl([xml]$nuspecXml) {
    $projectUrl = $nuspecXml.package.metadata.projectUrl;

    $message = "Project Url: $projectUrl";
    $requirement = "Must match expected."

    Test-Condition ($projectUrl -eq $expectedProjectUrl) $message $requirement;
}

function Get-IsValidLicense([xml]$nuspecXml) {
    $license = $nuspecXml.package.metadata.license.InnerText;

    $message = "License Url: $license";
    $requirement = "Must match expected."

    Test-Condition ($license -eq $expectedLicense) $message $requirement;
}

function Get-IsValidLicenseAcceptance([xml]$nuspecXml) {
    $requireLicenseAcceptance = $nuspecXml.package.metadata.requireLicenseAcceptance;

    $message = "Require License Acceptance: $requireLicenseAcceptance";
    $requirement = "Not mandatory requirement."
    $recommendation = "Should require.";

    Test-MultiCondition ($true) ($requireLicenseAcceptance -eq $true) $message $requirement $recommendation;
}

function Get-IsValidCopyright([xml]$nuspecXml) {
    $copyright = $nuspecXml.package.metadata.copyright;
    
    $message = "Copyright: $copyright";
    $requirement = "Must match '$requiredCopyright'";

    Test-Condition ($copyright -eq $requiredCopyright) $message $requirement;
}

function Get-IsValidDescription([xml]$nuspecXml) {
    $description = $nuspecXml.package.metadata.description;
    $hasDescription = !([System.String]::IsNullOrEmpty($description));

    $message = "Description: $description";
    $requirement = "Must have a description."

    Test-Condition $hasDescription $message $requirement;
}

function Get-IsValidTags([xml]$nuspecXml) {
    $tags = $nuspecXml.package.metadata.tags;
    $hasTags = !([System.String]::IsNullOrEmpty($tags));

    $message = "Tags: $tags";
        $requirement = "Must have tags."
        Test-Condition $hasTags $message $requirement;

    $tagsArray = @();
    if($hasTags) {
        $tagsArray = $tags -split " ";
    } 

    $expectedTags | ForEach-Object {
        $hasTag = $tagsArray.Contains($_);
        $requirement = "Must include tag: $_";
        Test-Condition $hasTag $message $requirement;
    }
}

function Get-IsValidLogo([xml]$nuspecXml, $path) {
    $logoValue = $nuspecXml.package.metadata.icon;
    $hasLogo = !([System.String]::IsNullOrEmpty($logoValue));

    try {
        $filePath = Join-Path $path $logoValue;
        $exists = [System.IO.File]::Exists($filePath)
    } catch [System.SystemException] {
        $_.Exception.Message;
    }

    $message1 = "Logo: $logoValue";
    $message2 = "Logo Exists: $exists";
    $requirement = "Must have a logo."
    
    Test-Condition ($hasLogo) $message1 $requirement;
    Test-Condition ($exists) $message2 $requirement;
}

function Invoke-UnZip([string]$zipfile, [string]$outpath) {
    Write-Verbose "Unzip - source: $zipfile"
    Write-Verbose "Unzip - target: $outpath"
    Add-Type -assembly "system.io.compression.filesystem"
    [System.IO.Compression.ZipFile]::ExtractToDirectory($zipfile, $outpath)
}

function Start-EvaluateNupkg ($nupkgPath) {
    Write-Break;
    Write-Host "Evaluate nupkg:"
    Write-Name $nupkgPath;

    if ($verifySigning){
        Get-IsPackageSigned $nupkgPath;
    }

    $unzipPath = $nupkgPath+"_unzip";
    Remove-Item $unzipPath -Recurse -ErrorAction Ignore
    $null = Invoke-UnZip $nupkgPath $unzipPath;

    # LOOK FOR ALL NUSPEC WITHIN NUPKG
    Get-ChildItem -Path $unzipPath -Recurse -Filter *.nuspec | ForEach-Object { 
        Write-Host "Evaluate nuspec:"
        Write-Name $_.FullName;
        [xml]$nuspecXml = Get-Content $_.FullName
        Get-IsValidPackageId $nuspecXml;
        Get-IsValidAuthors $nuspecXml;
        #Get-IsValidOwners $nuspecXml; dotnet stopped building this property. Disabling the check.
        Get-IsValidProjectUrl $nuspecXml;
        Get-IsValidLicense $nuspecXml;
        Get-IsValidLicenseAcceptance $nuspecXml;
        Get-IsValidCopyright $nuspecXml;
        Get-IsValidLogo $nuspecXml $unzipPath;
        Get-IsValidDescription $nuspecXml;
        Get-IsValidTags $nuspecXml;
        }
    
    # LOOK FOR ALL DLL WITHIN NUPKG
    Get-ChildItem -Path $unzipPath -Recurse -Filter *.dll | ForEach-Object {
        Write-Host "Evaluate dll:"
        Write-Name $_.FullName;

        if ($verifySigning) {
            Get-IsDllSigned $_.FullName;
        }

        Get-DoesDllVersionsMatch $_.FullName;
        
        Get-DoesPdbExist $_.FullName;

        Get-DoesXmlDocExist $_.FullName;
        Get-DoesXmlDocContainsLang $_.FullName;
        }
}

############################
# MAIN EXECUTION STARTS HERE
############################

# CLEAR WORKING DIRECTORY
Remove-Item $workingDir -Recurse -ErrorAction Ignore
New-Item -ItemType directory -Path $workingDir

# FIND ALL NUPKG AND COPY TO WORKING DIRECTORY
Get-ChildItem -Path $nupkgPath -Recurse -Filter *.nupkg -Exclude *.symbols.nupkg -File |
     Copy-Item -Destination $workingDir

# LIST ALL FILES IN WORKING DIRECTORY
Write-Host "NUPKGS to audit:"
$files = Get-ChildItem -Path $workingDir -Recurse -File;
$files | ForEach-Object { Write-Host "`t"$_.FullName };
Write-Host "`nCount:" $files.Count;

# RUN AUDIT
Get-ChildItem -Path $workingDir -Recurse -File -Include *.nupkg | 
    ForEach-Object { Start-EvaluateNupkg $_.FullName }

# LOG
$logPath = (Join-Path $workingDir "log.txt")
$sb.ToString() | Add-Content $logPath;
Write-Host "`nLog file created at $logPath"

# RESULT
if (!$script:isValid){
    Write-Host "`n"
    throw "NUPKG or DLL is not valid. Please review log...";
    }
