#!/usr/bin/env bash
set -euo pipefail

eng_root="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

check_case() (
  local name="$1"
  local original="$2"
  local orchestrated="$3"
  local expected="$4"
  local expected_switch="$5"

  warn_not_as_error="$original"
  properties=("/p:Restore=true")
  if [[ "$orchestrated" == true ]]; then
    properties+=("/p:DotNetBuildOrchestrator=true")
  fi
  local original_count="${#properties[@]}"

  . "$eng_root/restore-toolset.sh"

  if [[ "$warn_not_as_error" != "$expected" ]]; then
    echo "$name: warning codes changed unexpectedly: $warn_not_as_error" >&2
    exit 1
  fi

  if [[ -n "$expected_switch" ]]; then
    local last_property_index=$((${#properties[@]} - 1))
    local inner_property="${warn_not_as_error//%25/%}"
    if [[ "${#properties[@]}" -ne $((original_count + 1)) ||
          "${properties[last_property_index]}" != "$expected_switch" ||
          "$inner_property" != "${original//;/%3B}" ||
          "$inner_property" == *';'* ]]; then
      echo "$name: the warning switch or nested shell property is not safe." >&2
      exit 1
    fi
  elif [[ "${#properties[@]}" -ne "$original_count" ]]; then
    echo "$name: extra MSBuild warning property outside the VMR source build." >&2
    exit 1
  fi
)

check_empty_case() (
  warn_not_as_error='NU1901;NU1902;NU1903;NU1904'
  properties=()

  . "$eng_root/restore-toolset.sh"

  if [[ "$warn_not_as_error" != 'NU1901;NU1902;NU1903;NU1904' ||
        -n "${properties[*]-}" ]]; then
    echo "empty: standalone build changed warning codes or added a property." >&2
    exit 1
  fi
)

check_empty_case
check_case standalone 'NU1901;NU1902;NU1903;NU1904' false \
  'NU1901;NU1902;NU1903;NU1904' ''
check_case vmr 'NU1901;NU1902;NU1903;NU1904' true \
  'NU1901%253BNU1902%253BNU1903%253BNU1904' \
  '/warnnotaserror:NU1901,NU1902,NU1903,NU1904'
check_case custom 'CA1000;CA1001' true \
  'CA1000%253BCA1001' '/warnnotaserror:CA1000,CA1001'
check_case single 'NU1901' true 'NU1901' ''
