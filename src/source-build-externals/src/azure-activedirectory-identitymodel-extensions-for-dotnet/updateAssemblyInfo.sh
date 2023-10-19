#!/bin/bash
set -euo pipefail

scriptroot=$(cd -P "$(dirname -- "${BASH_SOURCE[0]}")" && pwd)

packageType=${1:-preview}

date=$(date '+%y%m%d%H%M%S')
# Formats the date by replacing the 4-digit year with a 2-digit value and then subtract 19
dateTimeStamp=$(echo $((10#${date:0:2}-19)))${date:2}

commitSha=$(git rev-parse HEAD)

assemblyVersion=$(sed -n 's/.*<assemblyVersion>\([^<]*\)<.*/\1/p' ${scriptroot}/buildConfiguration.xml)
assemblyFileVersion="$assemblyVersion.${dateTimeStamp::$((${#dateTimeStamp} - 6))}" # Trim minutes/seconds
assemblyInformationalVersion="$assemblyVersion.$dateTimeStamp.$commitSha"

echo "assemblyVersion: $assemblyVersion"
echo "assemblyFileVersion: $assemblyFileVersion"
echo "assemblyInformationalVersion: $assemblyInformationalVersion"

nugetSuffix=$(sed -n 's/.*<nugetSuffix>\([^<]*\)<.*/\1/p' ${scriptroot}/buildConfiguration.xml)
if [ "$packageType" = "release" ]
then
    versionSuffix=""
else
    versionSuffix="$nugetSuffix-$dateTimeStamp"
fi

echo "nugetSuffix: $nugetSuffix"

versionPath="${scriptroot}/build/version.props"
version=$(cat $versionPath)
version=$(echo "$version" | sed "s|<VersionPrefix>.*</VersionPrefix>|<VersionPrefix>$assemblyVersion</VersionPrefix>|")
version=$(echo "$version" | sed "s|<VersionSuffix>.*</VersionSuffix>|<VersionSuffix>$versionSuffix</VersionSuffix>|")
echo "$version" > $versionPath

projects=$(sed -n '/<src>/,/<\/src>/p' ${scriptroot}/buildConfiguration.xml | sed -n 's/.*name="\([^"]*\)".*/\1/p')

for project in $projects; do
    name="$project"
    assemblyInfoPath="${scriptroot}/src/$name/Properties/AssemblyInfo.cs"
    echo "assemblyInfoPath: $assemblyInfoPath"

    assemblyInfo=$(cat $assemblyInfoPath)
    assemblyInfo=$(echo "$assemblyInfo" | sed "s|AssemblyVersion.*|AssemblyVersion(\"$assemblyVersion\")]|")
    assemblyInfo=$(echo "$assemblyInfo" | sed "s|AssemblyFileVersion.*|AssemblyFileVersion(\"$assemblyFileVersion\")]|")
    assemblyInfo=$(echo "$assemblyInfo" | sed "s|AssemblyInformationalVersion.*|AssemblyInformationalVersion(\"$assemblyInformationalVersion\")]|")
    echo "$assemblyInfo" > $assemblyInfoPath
done
