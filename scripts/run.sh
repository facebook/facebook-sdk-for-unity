#!/bin/sh
#
# Copyright (c) 2014-present, Facebook, Inc. All rights reserved.
#
# You are hereby granted a non-exclusive, worldwide, royalty-free license to use,
# copy, modify, and distribute this software in source code or binary form for use
# in connection with the web services and APIs provided by Facebook.
#
# As with any software that integrates with the Facebook platform, your use of
# this software is subject to the Facebook Developer Principles and Policies
# [http://developers.facebook.com/policy/]. This copyright notice shall be
# included in all copies or substantial portions of the software.
#
# THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
# IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
# FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
# COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
# IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN

# shellcheck disable=SC2039

set -euo pipefail

main() {
  SDK_VERSION_FILES=(
    "Facebook.Unity.Android.StrippingHack/Properties/AssemblyInfo.cs"
    "Facebook.Unity.Android/Properties/AssemblyInfo.cs"
    "Facebook.Unity.Canvas/Properties/AssemblyInfo.cs"
    "Facebook.Unity.Editor/Properties/AssemblyInfo.cs"
    "Facebook.Unity.Gameroom/Properties/AssemblyInfo.cs"
    "Facebook.Unity.IOS.StrippingHack/Properties/AssemblyInfo.cs"
    "Facebook.Unity.IOS/Properties/AssemblyInfo.cs"
    "Facebook.Unity.Settings/Properties/AssemblyInfo.cs"
    "Facebook.Unity.Tests/Properties/AssemblyInfo.cs"
    "Facebook.Unity/Properties/AssemblyInfo.cs"
    "Facebook.Unity/FacebookSdkVersion.cs"
    "scripts/build.properties"
  )
  SDK_MAIN_VERSION_FILE="scripts/build.properties"

  SDK_CURRENT_VERSION=$(grep -Eo "UNITY_SDK_BUILD_VERSION='.*'" "$SDK_MAIN_VERSION_FILE" | awk -F"'" '{print $2}')

  local command_type=${1:-}
  if [ -n "$command_type" ]; then shift; fi

  case "$command_type" in
    "bump-version") bump_version "$@" ;;
    "--help" | "help") echo "Check main() for supported commands" ;;
  esac
}

bump_version() {
  local new_version=${1:-}

  if [ "$new_version" == "$SDK_CURRENT_VERSION" ]; then
    echo "This version is the same as the current version"
    false
    return
  fi

  if ! is_valid_semver "$new_version"; then
    echo "This version isn't a valid semantic versioning"
    false
    return
  fi

  echo "Changing from: $SDK_CURRENT_VERSION to: $new_version"

  local version_change_files=(
    "${SDK_VERSION_FILES[@]}"
  )

  # Replace the previous version to the new version
  for file_path in "${version_change_files[@]}"; do
    if [ ! -f "$file_path" ]; then
      echo "*** NOTE: unable to find $file_path."
      continue
    fi

    local temp_file="$file_path.tmp"
    sed -e "s/$SDK_CURRENT_VERSION/$new_version/g" "$file_path" >"$temp_file"
    if diff "$file_path" "$temp_file" >/dev/null; then
      echo "*** ERROR: unable to update $file_path"
      rm "$temp_file"
      continue
    fi

    mv "$temp_file" "$file_path"
  done
}

# Proper Semantic Version
is_valid_semver() {
  if ! [[ ${1:-} =~ ^([0-9]{1}|[1-9][0-9]+)\.([0-9]{1}|[1-9][0-9]+)\.([0-9]{1}|[1-9][0-9]+)($|[-+][0-9A-Za-z+.-]+$) ]]; then
    false
    return
  fi
}

main "$@"
