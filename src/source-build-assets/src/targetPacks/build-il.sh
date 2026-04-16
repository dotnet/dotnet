#!/usr/bin/env bash

IlasmDir=$1
IlFile=$2
OutputFile=$3

set -o pipefail

# Note: Use awk as hack below to not fill up build logs. Ilasm produces warning
# on validly disassembled il src. The awk expression eats just that warning.
"${IlasmDir}ilasm" "${IlFile}" -dll -quiet -nologo "-output=${OutputFile}" 2>&1 | awk '!/warning : Method has no body/'
