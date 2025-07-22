#!/usr/bin/env bash
#set -x

# Stop script if command returns non-zero exit code.
set -e

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
    echo "  Either --package or --csv must be specified"
    echo ""
    echo "options:"
    echo "  -p|--package <name,version[,tfms]>            A package and version, no spaces, separated by comma. If optional semicolon-separated"
    echo "                                                TFMs are specified, they will be used to filter the project's target frameworks."
    echo "                                                Examples: System.Collections,4.3.0"
    echo "                                                          System.Text.Json,4.7.0,netstandard2.0;net461"
    echo "  --csv                                         A path to a csv file of packages to generate. Format is the same as the --package"
    echo "                                                option above, one per line.  If specified, the --package option is ignored."
    echo "  -d|--destination                              A path to the root of the repo to copy source into."
    echo "  -t|--type                                     Type of the package to generate. Accepted values: ref (default) | text."
    echo "  -x|--excludeDependencies                      Determines if package dependencies should be excluded. Default is false."
    echo "  -f|--feeds                                    A semicolon-separated list of additional NuGet feeds to use during restore."
    echo "  -h|--help                                     Print help and exit."
}

if [[ $# -le 0 ]]; then
	usage
	exit 0
fi

arguments=''
extraArgs=''

while [[ $# > 0 ]]; do
    opt="$(echo "${1/#--/-}" | tr "[:upper:]" "[:lower:]")"
    case "$opt" in
        -p|-package)
            if [ -z ${2+x} ]; then
                echo -e "${RED}ERROR: No package information supplied.${NC}"
                exit 1
            fi

            IFS=, read -r packageName packageVersion packageTargetFrameworks <<< $2
            if [ -z ${packageVersion} ]; then
                echo -e "${RED}ERROR: No package version supplied.${NC}"
                exit 1
            fi

            arguments="$arguments /p:PackageName=$packageName /p:PackageVersion=$packageVersion"
            if [ -n "$packageTargetFrameworks" ]; then
                arguments="$arguments /p:PackageTargetFrameworks=\"$packageTargetFrameworks\""
            fi
            shift 2
            ;;
        -c|-csv)
            if [ ! -f "$2" ]; then
                echo -e "${RED}ERROR: CSV file not found: '$2'${NC}"
                exit 1
            fi
            packageCSV="$(cd "$(dirname "$2")"; pwd)/$(basename "$2")"
            arguments="$arguments /p:PackageCSV=\"$packageCSV\""
            shift 2
            ;;
        -d|-destination)
            if [ ! -d "$2" ]; then
                echo -e "${RED}ERROR: Destination not found: '$2'${NC}"
                exit 1
            fi
            packagesSrcDirectory="$(cd -P "$2" && pwd)"
            arguments="$arguments /p:PackagesSrcDirectory=\"$packagesSrcDirectory\""
            shift 2
            ;;
        -t|-type)
            type="$2"
            if [[ ! "$type" =~ ^(text|ref)$ ]]; then
                echo -e "${RED}ERROR: Unknown package type: '$type'${NC}"
                exit 1
            fi
            arguments="$arguments /p:PackageType=$type"
            shift 2
            ;;
        -x|-excludedependencies)
            arguments="$arguments /p:ExcludePackageDependencies=true"
            shift 1
            ;;
        -f|-feeds)
            if [ -z ${2+x} ]; then
                echo -e "${RED}ERROR: No feed supplied.${NC}"
                exit 1
            fi
            # -p:RestoreAdditionalProjectSources -> specifies additional Nuget package sources, including feeds: https://docs.microsoft.com/en-us/nuget/reference/msbuild-targets#restore-properties
            arguments="$arguments /p:RestoreAdditionalProjectSources=\"$2\""
            shift 2
            ;;
        "-?"|-h|-help)
            usage
            exit 0
            ;;
        *)
            extraArgs="$extraArgs $1"
            shift 1
            ;;
    esac
done

# Build the projects to generate text only or reference package source
"$scriptroot/eng/common/build.sh" --restore --build --warnaserror false /p:GeneratePackageSource=true $arguments $extraArgs
