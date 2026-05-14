#!/bin/bash

# This script downgrades the .NET SDK bundled versions in Microsoft.NETCoreSdk.BundledVersions.props
# file to the versions 2 releases prior. SDK includes the latest servicing versions automatically,
# which may not be publicly available yet, causing test failures. This script creates a backup of
# the original props file before making changes.

set -euo pipefail

# Configuration
PROPS_FILE="/usr/share/dotnet/sdk/$(dotnet --version)/Microsoft.NETCoreSdk.BundledVersions.props"
BACKUP_SUFFIX=".backup.$(date +%Y%m%d_%H%M%S)"

# Color output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

echo -e "${YELLOW}Downgrading .NET SDK bundled versions...${NC}"

# Check if file exists
if [ ! -f "$PROPS_FILE" ]; then
    echo -e "${RED}Error: Props file not found at $PROPS_FILE${NC}"
    exit 1
fi

# Backup the original file
echo "Creating backup: ${PROPS_FILE}${BACKUP_SUFFIX}"
cp "$PROPS_FILE" "${PROPS_FILE}${BACKUP_SUFFIX}"

# Function to find latest version for a major.minor
find_latest_version() {
    local major_minor=$1
    grep -oP "${major_minor}\.\d+" "$PROPS_FILE" | sort -V | tail -n1
}

# Function to decrement patch version by 2
decrement_version() {
    local version=$1
    local major=$(echo "$version" | cut -d. -f1)
    local minor=$(echo "$version" | cut -d. -f2)
    local patch=$(echo "$version" | cut -d. -f3)

    if [ "$patch" -lt 2 ]; then
        echo -e "${RED}Error: Cannot decrement ${version} by 2 (patch is ${patch})${NC}"
        return 1
    fi

    local new_patch=$((patch - 2))
    echo "${major}.${minor}.${new_patch}"
}

# Function to replace version in file
replace_version() {
    local old_version=$1
    local new_version=$2

    local old_escaped=$(echo "$old_version" | sed 's/\./\\./g')
    local new_escaped=$(echo "$new_version" | sed 's/\./\\./g')

    local count=$(grep -c "$old_version" "$PROPS_FILE" || true)

    if [ "$count" -eq 0 ]; then
        echo -e "${YELLOW}  No instances of ${old_version} found${NC}"
        return
    fi

    sed -i "s/${old_escaped}/${new_escaped}/g" "$PROPS_FILE"

    echo -e "${GREEN}  Replaced ${count} instance(s) of ${old_version} → ${new_version}${NC}"
}

# Function to process a specific .NET version
process_dotnet_version() {
    local version=$1

    echo -e "\n${YELLOW}Processing .NET ${version}...${NC}"
    local latest_version=$(find_latest_version "$version")

    if [ -n "$latest_version" ]; then
        echo "Found latest version: $latest_version"
        local new_version=$(decrement_version "$latest_version")
        if [ $? -eq 0 ]; then
            replace_version "$latest_version" "$new_version"
        fi
    else
        echo -e "${YELLOW}No ${version}.x versions found${NC}"
    fi
}

# Process .NET versions
process_dotnet_version "9.0"
process_dotnet_version "8.0"

# Verify changes
echo -e "\n${YELLOW}Verification:${NC}"
echo "9.0 versions in file:"
grep -oP "9\.0\.\d+" "$PROPS_FILE" | sort -u
echo ""
echo "8.0 versions in file:"
grep -oP "8\.0\.\d+" "$PROPS_FILE" | sort -u

echo -e "\n${GREEN}Done! Backup saved at: ${PROPS_FILE}${BACKUP_SUFFIX}${NC}"
echo -e "${YELLOW}To restore: cp ${PROPS_FILE}${BACKUP_SUFFIX} ${PROPS_FILE}${NC}"
