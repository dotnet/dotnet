<#
.SYNOPSIS
    CI script used for setting up Strawberry Perl on Windows

.DESCRIPTION
    Script used in the build pipeline to set up Strawberry Perl which is used to 
    install openssl on Windows and set the necessary environment variables.
    openssl is a requirement for building mcr device on Windows.
    Can be used in local development environment as well.
#>
Param(
    [string]$perl_setup_url = 'https://github.com/StrawberryPerl/Perl-Dist-Strawberry/releases/download/SP_5380_5361/strawberry-perl-5.38.0.1-64bit.msi',
    # SHA256 hash of the Strawberry Perl MSI obtained using Get-Filehash -Path <path-to-msi> -Algorithm SHA256
    [string]$perl_setup_sha256 = 'A9B44E50424DCC7E40B8F67D906C76A15469AF3D5998E04635FA8465A0A56877',
    [string]$ci_path = '',
    # Optional debug flag to print additional information
    [switch]$debug 
)

if (Test-Path $ci_path) {
    $root_dir = $ci_path
}
else {
    $root_dir = 'C:\Strawberry'
    New-Item -Path $root_dir -ItemType Directory -Force
}
$destination = "C:\Strawberry"

$ErrorActionPreference = "Stop"

# Check if Strawberry Perl is already installed anywhere on the machine
try {
    $perlVersion = & perl -v
    if ($null -ne $perlVersion) {
        $perlLocation = (Get-Command perl).Source
        Write-Output "Perl is installed."
        Write-Output "Version: $perlVersion"
        Write-Output "Location: $perlLocation"
        # Set environment variables
        Write-Output "##vso[task.setvariable variable=PERL;]$perlLocation"
        Write-Output "##vso[task.setvariable variable=OPENSSL_SRC_PERL;]$perlLocation"
        [Environment]::SetEnvironmentVariable("PERL", $perlLocation, "Machine")
        [Environment]::SetEnvironmentVariable("OPENSSL_SRC_PERL", $perlLocation, "Machine")
        # Check if Locale::Maketext::Simple is installed as it is needed for openssl compilation
        & perl -MLocale::Maketext::Simple -e '1'
        if ($LASTEXITCODE -eq 0) {
            Write-Output "Locale::Maketext::Simple is installed."
        }
        else {
            Write-Output "Locale::Maketext::Simple is not installed. Installing it."
            & cpan "Locale::Maketext::Simple"
        }
        if ($debug) {
            $perlOutput = (perl -e "print join(';', @INC)").split(';')
            foreach ($path in $perlOutput) {
                Get-ChildItem -Recurse -Path $path
            }
        }
        exit 0
    }
}
catch {
    Write-Output "Perl is not installed. Proceeding with installation."
}

# Download Strawberry Perl
Invoke-WebRequest -Uri $perl_setup_url -OutFile $root_dir\perl_setup.msi
# Verify downloaded MSI exists
if (-not (Test-Path $root_dir\perl_setup.msi)) {
    Write-Error "Strawberry Perl MSI not found at $root_dir\perl_setup.msi"
}
# Verify hash of downloaded MSI
$hash = Get-FileHash -Path $root_dir\perl_setup.msi -Algorithm SHA256
if ($hash.Hash -ne $perl_setup_sha256) {
    Write-Error "Strawberry Perl MSI hash mismatch"
}
# Install Strawberry Perl
Start-Process -FilePath msiexec -ArgumentList "/i $root_dir\perl_setup.msi /quiet /norestart /log $root_dir\perl_setup_log.txt" -Wait
# Output install logs
Get-Content $root_dir\perl_setup_log.txt
# Verify installation
if (-not (Test-Path $destination\perl)) {
    Write-Error "Strawberry Perl installation failed. See logs for more details."
}
# Set environment variable
[Environment]::SetEnvironmentVariable("PERL", "$destination\perl\bin\perl.exe", "Machine")
[Environment]::SetEnvironmentVariable("OPENSSL_SRC_PERL", "$destination\perl\bin\perl.exe", "Machine")
# Update PATH
$existingPath = [Environment]::GetEnvironmentVariable("PATH", [System.EnvironmentVariableTarget]::Machine)
if ($existingPath -notlike "*$pathUpdate*") {
    [Environment]::SetEnvironmentVariable("PATH", "$existingPath;$destination\perl\bin", "Machine")
}
Write-Host "[Info] Strawberry Perl installed successfully"
