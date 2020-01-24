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

cd "$( dirname "${BASH_SOURCE[0]}" )/.."
PROJECT_ROOT=$(pwd)

export SCRIPTS_DIR="$PROJECT_ROOT/scripts"

CORE_ROOT=$PROJECT_ROOT/Facebook.Unity

UNITY_PATH="/Applications/Unity/Unity.app/Contents/MacOS/Unity"
UNITY_PACKAGE_ROOT=$PROJECT_ROOT/UnitySDK
UNITY_PACKAGE_PLUGIN=$UNITY_PACKAGE_ROOT/Assets/FacebookSDK/Plugins/
export UNITY_ANDROID_PLUGIN=$UNITY_PACKAGE_PLUGIN/Android/
export UNITY_CANVAS_PLUGIN=$UNITY_PACKAGE_PLUGIN/Canvas/
export UNITY_GAMEROOM_PLUGIN=$UNITY_PACKAGE_PLUGIN/Gameroom/
export UNITY_EDITOR_PLUGIN=$UNITY_PACKAGE_PLUGIN/Editor/
export UNITY_IOS_PLUGIN=$UNITY_PACKAGE_PLUGIN/iOS/
export UNITY_SETTINGS_PLUGIN=$UNITY_PACKAGE_PLUGIN/Settings/

RED='\033[0;31m'
NC='\033[0m'
CYAN='\033[0;36m'

# Extract the SDK version from FacebookSdkVersion.java
SDK_VERSION_RAW=$(sed -n 's/.*"\(.*\)\";/\1/p' "$CORE_ROOT/FacebookSdkVersion.cs")
SDK_VERSION_MAJOR=$(echo "$SDK_VERSION_RAW" | awk -F'.' '{print $1}')
SDK_VERSION_MAJOR=${SDK_VERSION_MAJOR:-0}
SDK_VERSION_MINOR=$(echo "$SDK_VERSION_RAW" | awk -F'.' '{print $2}')
SDK_VERSION_MINOR=${SDK_VERSION_MINOR:-0}
SDK_VERSION_REVISION=$(echo "$SDK_VERSION_RAW" | awk -F'.' '{print $3}')
SDK_VERSION_REVISION=${SDK_VERSION_REVISION:-0}
export SDK_VERSION=$SDK_VERSION_MAJOR.$SDK_VERSION_MINOR.$SDK_VERSION_REVISION

UNITY_JAR_RESOLVER_NAME='unity-jar-resolver'
UNITY_JAR_RESOLVER_PACKAGE_NAME='play-services-resolver'
UNITY_JAR_RESOLVER_BASE_URL="https://github.com/googlesamples/$UNITY_JAR_RESOLVER_NAME/archive/v"
UNITY_JAR_RESOLVER_VERSION='1.2.135'
UNITY_JAR_RESOLVER_ZIP_URL="$UNITY_JAR_RESOLVER_BASE_URL$UNITY_JAR_RESOLVER_VERSION.zip"

export OUT="$PROJECT_ROOT/out"

die() {
  echo ""
  echo "${RED}FATAL: $* ${NC}" >&2
  exit 1
}

# Echoes a progress message to stderr
progress_message() {
    echo "$@" >&2
}

info() {
  echo "${CYAN}$* ${NC}" >&2
}

downloadUnityJarResolverFromGithub() {
  UNITY_JAR_RESOLVER_PACKAGE="$UNITY_JAR_RESOLVER_PACKAGE_NAME.unitypackage"

  pushd $PROJECT_ROOT > /dev/null
  info "Downloading unity-jar-resolver..."
  curl -L "$UNITY_JAR_RESOLVER_ZIP_URL" > $UNITY_JAR_RESOLVER_NAME.zip || die "Failed to download $UNITY_JAR_RESOLVER_URL"
  unzip -o -j -q $UNITY_JAR_RESOLVER_NAME.zip -d $UNITY_JAR_RESOLVER_NAME
  mv $UNITY_JAR_RESOLVER_NAME/$UNITY_JAR_RESOLVER_PACKAGE_NAME-$UNITY_JAR_RESOLVER_VERSION.0.unitypackage $PROJECT_ROOT/$UNITY_JAR_RESOLVER_PACKAGE
  rm -rf $UNITY_JAR_RESOLVER_NAME.zip $UNITY_JAR_RESOLVER_NAME
  info "Importing unity-jar-resolver to UnitySDK project..."

  UNITY_PACKAGE_PATH="$PROJECT_ROOT/$UNITY_JAR_RESOLVER_PACKAGE"

  $UNITY_PATH -quit -batchmode -logFile -nographics -projectPath="$PROJECT_ROOT/UnitySDK" \
   -importPackage "$UNITY_PACKAGE_PATH" || die "Failed to import $UNITY_PACKAGE_PATH"
  info "Cleaning up..."
  rm "$UNITY_PACKAGE_PATH"
  popd > /dev/null
}

validate_file_exists() {
  if [ ! -f "$1" ]; then
    echo "${RED}FATAL: File not found $1 ${NC}" >&2
    die "$2"
  fi
}

validate_any_file_exists() {
  FILE_COUNT=$(find "$1" -name "$2" | wc -l)

  if [[ $FILE_COUNT -eq 0 ]]; then
    echo "${RED}FATAL: File not found $2 ${NC}" >&2
    die "$3"
  fi
}
