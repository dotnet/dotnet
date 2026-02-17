#!/usr/bin/env bash

# Stop script if unbound variable found (use ${var:-} if intentional)
set -u

# Stop script if command returns non-zero exit code.
# Prevents hidden errors caused by missing error code propagation.
set -e

usage()
{
  echo "Common settings:"
  echo "  --binaryLog                       Create MSBuild binary log (short: -bl)"
  echo "  --configuration <value>           Build configuration: 'Debug' or 'Release' (short: -c)"
  echo "  --rid, --target-rid <value>       Overrides the rid that is produced by the build. e.g. alpine.3.18-arm64, fedora.37-x64, freebsd.13-arm64, ubuntu.19.10-x64"
  echo "  --os, --target-os <value>         Target operating system: e.g. linux, osx, freebsd. Note: this is the base OS name, not the distro"
  echo "  --arch, --target-arch <value>     Target architecture: e.g. x64, x86, arm64, arm, riscv64"
  echo "  --branding                        Specify the branding suffix for shipping packages/assets. By default uses VMR branding defaults (RepoDotNetFinalVersionKind property in eng/Versions.props)"
  echo "                                    for release branch builds or otherwise repo branding defaults."
  echo "    <repodefault>                   'repodefault' uses the repo branding defaults."
  echo "    <unstable>                      'unstable' produces assets with the repo specified branding suffix."
  echo "    <preview>                       'preview' produces assets with a '.final' branding suffix."
  echo "    <release>                       'release' produces assets without a branding suffix."
  echo "  --verbosity <value>               Msbuild verbosity: q[uiet], m[inimal], n[ormal], d[etailed], and diag[nostic] (short: -v)"
  echo "  --with-system-libs <libs>         Use system versions of these libraries. Combine with a plus. 'all' will use all supported libraries. e.g. brotli+libunwind+rapidjson+zlib+zstd"
  echo "  --official-build-id <YYYYMMDD.X>  Official build ID to use for the build. This is used to set the OfficialBuildId MSBuild property."

  echo ""

  echo "Actions:"
  echo "  --clean                           Clean the solution"
  echo "  --help                            Print help and exit (short: -h)"
  echo "  --test                            Run tests (short: -t)"
  echo "  --sign                            Sign the build."
  echo ""

  echo "Source-only settings:"
  echo "  --source-only, --source-build     Source-build the solution (short: -so, -sb)"
  echo "  --online                          Build using online sources"
  echo "  --poison                          Build with poisoning checks"
  echo "  --release-manifest <FILE>         A JSON file, an alternative source of Source Link metadata"
  echo "  --source-repository <URL>         Source Link repository URL, required when building from tarball"
  echo "  --source-version <SHA>            Source Link revision, required when building from tarball"
  echo "  --with-packages <DIR>             Use the specified directory of previously-built packages"
  echo "  --with-shared-components <DIR>    Use the specified shared components artifacts (e.g. from a build of a 1xx branch)"
  echo "  --with-sdk <DIR>                  Use the SDK in the specified directory for bootstrapping"
  echo "  --prep                            Run prep-source-build.sh to download bootstrap binaries before building"
  echo ""

  echo "Advanced settings:"
  echo "  --buildCheck <value>            Sets /check msbuild parameter"
  echo "  --build-repo-tests              Build repository tests"
  echo "  --ci                            Set when running on CI server"
  echo "  --clean-while-building          Cleans each repo after building (reduces disk space usage, short: -cwb)"
  echo "  --excludeCIBinarylog            Don't output binary log (short: -nobl)"
  echo "  --nodeReuse <value>             Sets nodereuse msbuild parameter ('true' or 'false')"
  echo "  --prepareMachine                Prepare machine for CI run, clean up processes after build"
  echo "  --projects <value>              Project or solution file to build"
  echo "  --use-mono-runtime              Output uses the mono runtime"
  echo "  --warnAsError <value>           Sets warnaserror msbuild parameter ('true' or 'false')"

  echo ""
  echo "Command line arguments not listed above are passed thru to msbuild."
  echo "Arguments can also be passed in with a single hyphen."
}

function SetOfficialBuildId()
{
  local officialBuildIdValue="$1"
  if [[ ! "$officialBuildIdValue" =~ ^[0-9]{8}\.[0-9]{1,3}$ ]]; then
    echo "ERROR: Invalid official-build-id format. Expected format: YYYYYMMDD.X"
    exit 1
  fi
  properties+=( "/p:OfficialBuildId=$officialBuildIdValue" )
}

source="${BASH_SOURCE[0]}"

# resolve $source until the file is no longer a symlink
while [[ -h "$source" ]]; do
  scriptroot="$( cd -P "$( dirname "$source" )" && pwd )"
  source="$(readlink "$source")"
  # if $source was a relative symlink, we need to resolve it relative to the path where the
  # symlink file was located
  [[ $source != /* ]] && source="$scriptroot/$source"
done
scriptroot="$( cd -P "$( dirname "$source" )" && pwd )"

# Common settings
binary_log=''
configuration='Release'
verbosity='minimal'
officialBuildId=''

# Actions
clean=false
test=false

# Source-only settings
prep=false
sourceOnly=false
releaseManifest=''
sourceRepository=''
sourceVersion=''
customPackagesDir=''
customSdkDir=''
packagesDir="$scriptroot/prereqs/packages/"
packagesArchiveDir="${packagesDir}archive/"
packagesPreviouslySourceBuiltDir="${packagesDir}previously-source-built/"

# Advanced settings
build_check=false
ci=''
exclude_ci_binary_log=''
node_reuse=''
prepare_machine=''
warn_as_error=''
targetOS=''
targetArch=''

properties=()
while [[ $# > 0 ]]; do
  opt="$(echo "${1/#--/-}" | tr "[:upper:]" "[:lower:]")"
  case "$opt" in
    # Common settings
    -binarylog|-bl)
      binary_log=true
      ;;
    -buildcheck)
      build_check=true
      ;;
    -configuration|-c)
      configuration=$2
      shift
      ;;
    -rid|-target-rid)
      properties+=( "/p:TargetRid=$2" )
      shift
      ;;
    -os|-target-os)
      properties+=( "/p:TargetOS=$2" )
      targetOS="$2"
      shift
      ;;
    -arch|-target-arch)
      properties+=( "/p:TargetArchitecture=$2" )
      targetArch="$2"
      shift
      ;;
    -branding)
      if [ -z ${2+x} ]; then
        echo "No branding type supplied. See help (--help) for supported values." 1>&2
        exit 1
      fi
      passedBrandingType="$(echo "$2" | tr "[:upper:]" "[:lower:]")"
      case "$passedBrandingType" in
        repodefault|unstable|preview|release)
          ;;
        *)
          echo "Unsupported branding type '$2'."
          echo "The allowed values are repodefault, unstable, preview or release."
          echo ""
          echo "IMPORTANT: If you previously passed in the '-branding rtm' argument for .NET 10, remove it entirely. The default is now set correctly (VMR branding defaults for release branch builds or otherwise repo branding defaults)."
          exit 1
          ;;
      esac
      properties+=( "/p:RepoDotNetFinalVersionKind=$passedBrandingType" )
      shift
      ;;
    -with-system-libs)
      value="$2"
      if [[ "$value" != +* ]]; then
        # Ensure the value is prepended with a '+'
        value="+$value"
      fi
      properties+=( "/p:UseSystemLibs=$value" )
      shift
      ;;
    -official-build-id)
      officialBuildId="$2"
      SetOfficialBuildId "$officialBuildId"
      shift
      ;;
    -verbosity|-v)
      verbosity=$2
      shift
      ;;

    # Actions
    -clean)
      clean=true
      ;;
    -help|-h|-\?|/?)
      usage
      exit 0
      ;;
    -test|-t)
      test=true
      ;;
    -sign)
      properties+=( "/p:DotNetBuildSign=true" )
      ;;

    # Source-only settings
    -source-only|-source-build|-so|-sb)
      sourceOnly=true
      properties+=( "/p:DotNetBuildSourceOnly=true" )
      ;;
    -online)
      properties+=( "/p:DotNetBuildWithOnlineFeeds=true" )
      ;;
    -poison)
      properties+=( "/p:EnablePoison=true" )
      ;;
    -release-manifest)
      releaseManifest="$2"
      shift
      ;;
    -source-repository)
      sourceRepository="$2"
      shift
      ;;
    -source-version)
      sourceVersion="$2"
      shift
      ;;
    -with-packages)
      customPackagesDir="$(cd -P "$2" && pwd)"
      if [ ! -d "$customPackagesDir" ]; then
          echo "Custom previously built packages directory '$customPackagesDir' does not exist"
          exit 1
      fi
      shift
      ;;
    -with-shared-components)
      sharedComponentsDir="$2"
      if [ ! -d "$sharedComponentsDir" ]; then
          echo "Shared components directory '$sharedComponentsDir' does not exist"
          exit 1
      fi
      properties+=( "/p:CustomSharedComponentsArtifactsPath=$sharedComponentsDir" )
      shift
      ;;
    -with-sdk)
      customSdkDir="$(cd -P "$2" && pwd)"
      shift
      ;;
    -prep)
      prep=true
      ;;
    # Advanced settings
    -build-repo-tests)
      properties+=( "/p:DotNetBuildTests=true" )
      ;;
    -projects)
      properties+=( "/p:Projects=$2" )
      shift
      ;;
    -ci)
      ci=true
      ;;
    -clean-while-building|-cwb)
      properties+=( "/p:CleanWhileBuilding=true" )
      ;;
    -excludecibinarylog|-nobl)
      exclude_ci_binary_log=true
      ;;
    -nodereuse)
      node_reuse=$2
      shift
      ;;
    -preparemachine)
      prepare_machine=true
      ;;
    -use-mono-runtime)
      properties+=( "/p:DotNetBuildUseMonoRuntime=true" )
      ;;
    -warnaserror)
      warn_as_error=$2
      shift
      ;;
    *)
      properties+=( "$1" )
      ;;
  esac

  shift
done

if [[ "$ci" == true ]]; then
  if [[ "$exclude_ci_binary_log" == false ]]; then
    binary_log=true
  fi
fi

source "$scriptroot/eng/common/tools.sh"
source "$scriptroot/eng/source-build-toolset-init.sh"

# Default properties
properties+=( "/p:RepoRoot=$repo_root" )
properties+=( "/p:Configuration=$configuration" )

actions=( "/p:Restore=true" "/p:Build=true" "/p:Publish=true")

if [[ "$test" == true ]]; then
  actions=( "/p:Restore=true" "/p:Build=true" "/p:Test=true" )
  properties+=( "/p:IsTestRun=true" )

  # Workaround for vstest hangs: https://github.com/microsoft/vstest/issues/10760
  export MSBUILDENSURESTDOUTFORTASKPROCESSES=1
  # Ensure all test projects share stdout (https://github.com/dotnet/source-build/issues/4635#issuecomment-2397464519)
  export MSBUILDDISABLENODEREUSE=1
fi

function Build {

  local bl=""
  if [[ "$binary_log" == true ]]; then
    bl="/bl:\"$log_dir/Build.binlog\""
  fi

  local check=""
  if [[ "$build_check" == true ]]; then
    check="/check"
  fi

  InitializeToolset

  MSBuild $_InitializeToolset \
    $bl \
    $check \
    "-tl:off" \
    "${actions[@]}" \
    "${properties[@]}"

  ExitWithExitCode 0
}

if [[ "$clean" == true ]]; then
  if [ -d "$artifacts_dir" ]; then
    rm -rf $artifacts_dir
    echo "Artifacts directory deleted."
  fi
  exit 0
fi

# Initialize __DistroRid and __PortableTargetOS
source $scriptroot/eng/common/native/init-os-and-arch.sh
source $scriptroot/eng/common/native/init-distro-rid.sh

initDistroRidGlobal "$os" "$arch" ""

if [[ -n "$__DistroRid" ]]; then
  properties+=( "/p:BuildRid=$__DistroRid" )
fi

# if targetOS and targetArch were provided, recompute __PortableTargetOS
if [[ -n "$targetOS" && -n "$targetArch" ]]; then
  unset __PortableTargetOS
  initDistroRidGlobal "$targetOS" "$targetArch" ""
fi

properties+=( "/p:PortableTargetOS=$__PortableTargetOS" )

# Source-only settings
if [[ "$sourceOnly" == "true" ]]; then
  # Run prep if requested
  if [[ "$prep" == true ]]; then
    "$scriptroot/prep-source-build.sh" --configuration "$configuration"
  fi

  # For build purposes, we need to make sure we have all the SourceLink information
  if [ "$test" != "true" ]; then
    get_property() {
      local json_file_path="$1"
      local property_name="$2"
      grep -oP '(?<="'$property_name'": ")[^"]*' "$json_file_path"
    }

    GIT_DIR="$scriptroot/.git"
    # Check if exist .git file
    if [ -f "$GIT_DIR" ]; then
      if ! grep -iq '^gitdir: ' "$GIT_DIR"; then
        echo "ERROR: $GIT_DIR exist, but does not contain a valid 'gitdir:' pointer."
        exit 1
      else
        GIT_DIR=$(grep '^gitdir: ' "$GIT_DIR" | awk -F'gitdir: ' '{ print $2 }')
      fi
    fi
    if [ -f "$GIT_DIR/config" ] && [ -f "$GIT_DIR/HEAD" ]; then # We check for config/HEAD because if outside of git, we create config and HEAD manually
      if [ -n "$sourceRepository" ] || [ -n "$sourceVersion" ] || [ -n "$releaseManifest" ]; then
        echo "ERROR: Source Link arguments cannot be used in a git repository"
        exit 1
      fi
    else
      if [ -z "$releaseManifest" ]; then
        if [ -z "$sourceRepository" ] || [ -z "$sourceVersion" ]; then
          echo "ERROR: $scriptroot is not a git repository, either --release-manifest or --source-repository and --source-version must be specified"
          exit 1
        fi
      else
        if [ -n "$sourceRepository" ] || [ -n "$sourceVersion" ]; then
          echo "ERROR: --release-manifest cannot be specified together with --source-repository and --source-version"
          exit 1
        fi

        sourceRepository=$(get_property "$releaseManifest" sourceRepository) \
          || (echo "ERROR: Failed to find sourceRepository in $releaseManifest" && exit 1)
        sourceVersion=$(get_property "$releaseManifest" sourceVersion) \
          || (echo "ERROR: Failed to find sourceVersion in $releaseManifest" && exit 1)

        if [ -z "$sourceRepository" ] || [ -z "$sourceVersion" ]; then
          echo "ERROR: sourceRepository and sourceVersion must be specified in $releaseManifest"
          exit 1
        fi
      fi

      # We need to add "fake" .git/ files when not building from a git repository
      mkdir -p "$GIT_DIR"
      echo '[remote "origin"]' > "$GIT_DIR/config"
      echo "url=\"$sourceRepository\"" >> "$GIT_DIR/config"
      echo "$sourceVersion" > "$GIT_DIR/HEAD"
    fi

    # If the release manifest is provided
    if [ -n "$releaseManifest" ] ; then
      # If OfficialBuildId was not provided, extract it from the release manifest
      if [ "$officialBuildId" == "" ]; then
        officialBuildId=$(get_property "$releaseManifest" officialBuildId) \
            || (echo "ERROR: Failed to find officialBuildId in $releaseManifest" && exit 1)
        SetOfficialBuildId "$officialBuildId"
      fi
    fi
  # else, running tests
  else
    properties+=( "/p:DisableSharedComponentValidation=true" )
  fi

  # Support custom source built package locations
  if [ "$customPackagesDir" != "" ]; then
    properties+=( "/p:CustomPreviouslySourceBuiltPackagesPath=$customPackagesDir" )
  fi

  if [ ! -e "$scriptroot/.git" ]; then
    echo "ERROR: $scriptroot is not a git repository/worktree."
    exit 1
  fi

  # Initialize source-only toolset (includes custom SDK setup, MSBuild resolver, and source-built resolver)
  source_only_toolset_init "$customSdkDir" "$customPackagesDir" "$binary_log" "$test" "${properties[@]}"
fi

Build
