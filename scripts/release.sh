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
  SDK_MAIN_VERSION_FILE="scripts/build.properties"
  SDK_CURRENT_VERSION=$(grep -Eo "UNITY_SDK_BUILD_VERSION='.*'" "$SDK_MAIN_VERSION_FILE" | awk -F "'" '{print $2}')

  SDK_GIT_REMOTE="https://github.com/facebook/facebook-sdk-for-unity"

  local command_type=${1:-}
  if [ -n "$command_type" ]; then shift; fi

  case "$command_type" in
  "bump-changelog") bump_changelog "$@" ;;
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

# --------------
# Main Script
# --------------

main "$@"
