#!/usr/bin/env bash
#set -x

# Stop script if command returns non-zero exit code.
set -e

DEFAULT_TFM=net6.0

source="$(readlink -f "${BASH_SOURCE[0]}")"
scriptroot="$( cd -P "$( dirname "$source" )" && pwd )"

RED='\033[0;31m'
NC='\033[0m'


usage() {
    echo "usage: $0 [options]"
    echo ""
    echo "  Generates a reference package or a text-only package from the specified packages and versions. The"
    echo "  type of the generated package is controlled via the --type option."
    echo ""
    echo "  Reference package generation will restore reference package(s) and dependencies and generate cs files"
    echo "  and with accompanying projects into the specified destination ('./src/referencePackages/' by default)."
    echo "  Text-only package generation will restore the specified package and copy the source-build-usable content"
    echo "  into the provided directory ('./src/textOnlyPackages/' by default)."
    echo ""
    echo "  Either --pkg or --pkgCsv must be specified"
    echo ""
    echo "options:"
    echo "  --pkg <packageName,version[,tfm]>  A package and version, no spaces, separated by comma.  If the optional"
    echo "                                       tfm is specified, it will be used in the project that restores the"
    echo "                                       package."
    echo "                                       Example: System.Text.Json,4.7.0"
    echo "  --pkgCsv <pathToCSV>               A path to a csv file of packages to generate. Format is the same as the --pkg"
    echo "                                       option above, one per line.  If specified, the --pkg option is ignored."
    echo "  --dest <pathToDestRepo>            A path to the root of the repo to copy source into."
    echo "  --type <packageType>               Type of the package to generate. Accepted values: ref (default) | text."
    echo "  --feeds <nugetFeeds>               A semicolon-separated list of additional NuGet feeds to use during restore."
    echo "  --genapi-backend                   The GenAPI backend used to generate reference assemblies. Accepted values: cci (default) | roslyn."
    echo ""
}

backend="cci"
packageVersion=
defaultPathToCSV="$scriptroot/artifacts/targetPackages.csv"
pathToCSV=
pathToDestRepo=
packageType="ref"

if [[ $# -le 0 ]]; then
	usage
	exit 0
fi

while [ $# -gt 0 ]; do
    case "$1" in
        "-?"|-h|--help)
            usage
            exit 0
            ;;
        --pkgcsv)
            if [ ! -f "$2" ]; then
                echo -e "${RED}ERROR: CSV file not found - $2${NC}"
                exit 1
            fi
            pathToCSV="$(cd "$(dirname "$2")"; pwd)/$(basename "$2")"
            shift
            ;;
        --dest)
            if [ ! -d "$2" ]; then
                echo -e "${RED}ERROR: dest not found - $2${NC}"
                exit 1
            fi
            pathToDestRepo="$(cd -P "$2" && pwd)"
            shift
            ;;
        --type)
            packageType="$2"
            if [[ ! "$packageType" =~ ^(text|ref)$ ]]; then
                echo -e "${RED}ERROR: unknown package type - $2${NC}"
                usage
                exit 1
            fi
            shift
            ;;
        --pkg)
            packageVersion="$2"
            shift
            ;;
        --feeds)
            feeds="$2"
            shift
            ;;
        --genapi-backend)
            backend="$2"
            if [[ ! "$backend" =~ ^(cci|roslyn)$ ]]; then
                echo -e "${RED}ERROR: unknown backend type - $2${NC}"
                usage
                exit 1
            fi
            shift
            ;;
        *)
            echo -e "${RED}Unrecognized argument '$1'${NC}"
            usage
            exit 1
            ;;
    esac

    shift
done

if [ -z "$packageVersion" ] && [ -z "$pathToCSV" ]; then
	echo -e "${RED}ERROR: Either --pkg or --pkgCsv must be specified!${NC}"
    usage
    exit 1
fi

if [ "$packageVersion" != "" ]; then
    mkdir -p "$(dirname "$defaultPathToCSV")"
    echo "$packageVersion" > "$defaultPathToCSV"
fi

if [ "$pathToCSV" == "" ]; then
    pathToCSV="$defaultPathToCSV"
fi

if [[ -z "$pathToDestRepo" ]]; then
    if [ "$packageType" == "ref" ]; then
        pathToDestRepo="$scriptroot/src/referencePackages/src"
    else
        pathToDestRepo="$scriptroot/src/textOnlyPackages/src/"
    fi
fi

# Generate restore projects for each target package
# Packges are restored in their own project to eliminate any conflicts
# between different versions of the same package.
INPUT="$pathToCSV"
OLDIFS=$IFS
IFS=,

restoreProjectsDir="$scriptroot/artifacts/targetRestoreProjects"
targetPackageTemplate="$scriptroot/src/referencePackageSourceGenerator/GenerateSource/targetPackage.csproj.template"
if [ -d "$restoreProjectsDir" ]; then
  rm -rf "$restoreProjectsDir"
fi
mkdir -p "$restoreProjectsDir"
[ ! -f "$INPUT" ] && { echo "$INPUT file not found"; exit 99; }
while read -r pkgName pkgVersion tfm
do
    sed "s/##PackageName##/${pkgName}/g" "$targetPackageTemplate" > "$restoreProjectsDir/$pkgName.$pkgVersion.csproj"
    sed -i "s/##PackageVersion##/${pkgVersion}/g" "$restoreProjectsDir/$pkgName.$pkgVersion.csproj"
    if [ "$tfm" != "" ]; then
        sed -i "s/##Tfm##/${tfm}/g" "$restoreProjectsDir/$pkgName.$pkgVersion.csproj"
    else
        sed -i "s/##Tfm##/${DEFAULT_TFM}/g" "$restoreProjectsDir/$pkgName.$pkgVersion.csproj"
    fi
done < "$INPUT"
IFS=$OLDIFS

if [[ "$packageType" == "ref" ]]; then
    pathToRepoSrc="$pathToDestRepo"

    # Build the projects to generate source and projects
    "$scriptroot/eng/common/build.sh" --build --restore -bl /p:GeneratePackageSource=true /p:GenAPIBackend="\"$backend\"" /p:GeneratorVersion=2 /p:PathToRepoSrc="$pathToRepoSrc" /p:RestoreAdditionalProjectSources="\"$feeds\"" "$@"
else
    # -p:RestoreAdditionalProjectSources -> specifies additional Nuget package sources, including feeds: https://docs.microsoft.com/en-us/nuget/reference/msbuild-targets#restore-properties
    # -p:RestoreUsingNuGetTargets -> switch that controls who will restore the project; if set to `false`, Arcade's Build.proj will execute the Restore target of the project itself.
    #	Otherwise, Build.proj will trigger its own instance of Nuget.targets.Restore and delegate the restore of all applicable projects to it. In this case, the Restore target is
    #	outside the project and we cannot hook our post-processing logic to it. For more info see:
    #	https://github.com/dotnet/arcade/blob/5b838a3ed7f8e53c3082724605e5237fa614a43c/src/Microsoft.DotNet.Arcade.Sdk/tools/Build.proj#L227

    "$scriptroot/eng/common/build.sh" -bl --restore --projects "$scriptroot/src/textOnlyPackageGenerator/PackageGenerator.proj" /p:RestoreUsingNuGetTargets=false /p:TextOnlyPackageDestination="$pathToDestRepo" /p:PathToCsv="$pathToCSV" /p:RestoreAdditionalProjectSources="\"$feeds\""
fi
