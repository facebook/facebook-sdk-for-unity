#!/bin/sh
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
# CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

# shellcheck disable=SC2039

set -euo pipefail

# --------------
# Functions
# --------------

# Main
main() {
  SDK_SCRIPTS_DIR=$(realpath "$(dirname "${BASH_SOURCE[0]}")")
  SDK_DIR="$(dirname "$SDK_SCRIPTS_DIR")"

  SDK_VERSION_FILES=(
    "scripts/build.properties"
    "Facebook.Unity/FacebookSdkVersion.cs"
    "Facebook.Unity/Properties/AssemblyInfo.cs"
    "Facebook.Unity.Canvas/Properties/AssemblyInfo.cs"
    "Facebook.Unity.Gameroom/Properties/AssemblyInfo.cs"
    "Facebook.Unity.Editor/Properties/AssemblyInfo.cs"
    "Facebook.Unity.IOS/Properties/AssemblyInfo.cs"
    "Facebook.Unity.IOS.StrippingHack/Properties/AssemblyInfo.cs"
    "Facebook.Unity.Android/Properties/AssemblyInfo.cs"
    "Facebook.Unity.Android.StrippingHack/Properties/AssemblyInfo.cs"
    "Facebook.Unity.Settings/Properties/AssemblyInfo.cs"
    "Facebook.Unity.Tests/Properties/AssemblyInfo.cs"
  )

  SDK_MAIN_VERSION_FILE="scripts/build.properties"
  SDK_CURRENT_VERSION=$(grep -Eo "UNITY_SDK_BUILD_VERSION='.*'" "$SDK_MAIN_VERSION_FILE" | awk -F "'" '{print $2}')

  SDK_GIT_REMOTE="https://github.com/facebook/facebook-sdk-for-unity"

  local command_type=${1:-}
  if [ -n "$command_type" ]; then shift; fi

  case "$command_type" in
  "bump-changelog") bump_changelog "$@" ;;
  "bump-version") bump_version "$@" ;;
  "tag-current-version") tag_current_version "$@" ;;
  "--help" | "help") echo "Check main() for supported commands" ;;
  esac
}

# Bump Changelog
bump_changelog() {
  local new_version=${1:-}

  # Edit Changelog
  updated_changelog=""

  while IFS= read -r line; do
    if [[ $line == "## [Unreleased]"* ]]; then
      current_date=$(date +%Y-%m-%d)
      updated_line=$line"\n"
      updated_line=$updated_line"\n"
      updated_line=$updated_line"## [$new_version] - $current_date\n"
    elif [[ $line == "[Unreleased]"* ]]; then
      updated_line="[Unreleased]: $SDK_GIT_REMOTE/compare/sdk-version-$new_version...HEAD\n"
      updated_line=$updated_line"[$new_version]: $SDK_GIT_REMOTE/compare/sdk-version-$SDK_CURRENT_VERSION...sdk-version-$new_version\n"
    else
      updated_line=$line"\n"
    fi
    updated_changelog=$updated_changelog$updated_line
  done <"CHANGELOG.md"

  echo "$updated_changelog" >CHANGELOG.md
}

# Bump Version
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

  # Replace the previous version to the new version in relative files
  for file_path in "${SDK_VERSION_FILES[@]}"; do
    local full_file_path="$SDK_DIR/$file_path"

    if [ ! -f "$full_file_path" ]; then
      echo "*** NOTE: unable to find $full_file_path."
      continue
    fi

    local temp_file="$full_file_path.tmp"
    sed -e "s/$SDK_CURRENT_VERSION/$new_version/g" "$full_file_path" >"$temp_file"
    if diff "$full_file_path" "$temp_file" >/dev/null; then
      echo "*** ERROR: unable to update $full_file_path"
      rm "$temp_file"
      continue
    fi

    mv "$temp_file" "$full_file_path"
  done

  bump_changelog "$new_version"
}

# Tag push current version
tag_current_version() {
  if ! is_valid_semver "$SDK_CURRENT_VERSION"; then
    exit 1
  fi

  if does_version_exist "$SDK_CURRENT_VERSION"; then
    echo "Version $SDK_CURRENT_VERSION already exists"
    false
    return
  fi

  git tag -a "sdk-version-$SDK_CURRENT_VERSION" -m "Version $SDK_CURRENT_VERSION"

  if [ "${1:-}" == "--push" ]; then
    git push origin "sdk-version-$SDK_CURRENT_VERSION"
  fi
}

# Proper Semantic Version
is_valid_semver() {
  if ! [[ ${1:-} =~ ^([0-9]{1}|[1-9][0-9]+)\.([0-9]{1}|[1-9][0-9]+)\.([0-9]{1}|[1-9][0-9]+)($|[-+][0-9A-Za-z+.-]+$) ]]; then
    false
    return
  fi
}

# Check Version Tag Exists
does_version_exist() {
  local version_to_check=${1:-}

  if [ "$version_to_check" == "" ]; then
    version_to_check=$SDK_CURRENT_VERSION
  fi

  if [ ! -d "$SDK_DIR"/.git ]; then
    echo "Not a Git Repository"
    return
  fi

  if git rev-parse "v$version_to_check" >/dev/null 2>&1; then
    return
  fi

  if git rev-parse "sdk-version-$version_to_check" >/dev/null 2>&1; then
    return
  fi

  false
}

# --------------
# Main Script
# --------------

main "$@"
