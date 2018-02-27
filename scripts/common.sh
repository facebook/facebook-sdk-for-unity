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

cd "$( dirname "${BASH_SOURCE[0]}" )/.."
PROJECT_ROOT=$(pwd)

SCRIPTS_DIR="$PROJECT_ROOT/scripts"

CORE_ROOT=$PROJECT_ROOT/Facebook.Unity

if [ -z ${UNITY_PATH+x} ];
  then UNITY_PATH="/Applications/Unity/Unity.app/Contents/MacOS/Unity";
fi
if [  -z ${UNITY_DLL_PATH+x} ];
  then UNITY_DLL_PATH="/Applications/Unity/Unity.app/Contents/Managed";
fi
UNITYDLLPATH=$UNITY_DLL_PATH
UNITY_PACKAGE_ROOT=$PROJECT_ROOT/UnitySDK
UNITY_PACKAGE_PLUGIN=$UNITY_PACKAGE_ROOT/Assets/FacebookSDK/Plugins/
UNITY_ANDROID_PLUGIN=$UNITY_PACKAGE_PLUGIN/Android/
UNITY_GAMEROOM_PLUGIN=$UNITY_PACKAGE_PLUGIN/Gameroom/
UNITY_EDITOR_PLUGIN=$UNITY_PACKAGE_PLUGIN/Editor/
UNITY_IOS_PLUGIN=$UNITY_PACKAGE_PLUGIN/iOS/
UNITY_SETTINGS_PLUGIN=$UNITY_PACKAGE_PLUGIN/Settings/

SCRIPTS_DIR="$PROJECT_ROOT/scripts"

RED='\033[0;31m'
NC='\033[0m'
CYAN='\033[0;36m'

# Extract the SDK version from FacebookSdkVersion.java
SDK_VERSION_RAW=$(sed -n 's/.*"\(.*\)\";/\1/p' "$CORE_ROOT/FacebookSdkVersion.cs")
SDK_VERSION_MAJOR=$(echo $SDK_VERSION_RAW | awk -F'.' '{print $1}')
SDK_VERSION_MAJOR=${SDK_VERSION_MAJOR:-0}
SDK_VERSION_MINOR=$(echo $SDK_VERSION_RAW | awk -F'.' '{print $2}')
SDK_VERSION_MINOR=${SDK_VERSION_MINOR:-0}
SDK_VERSION_REVISION=$(echo $SDK_VERSION_RAW | awk -F'.' '{print $3}')
SDK_VERSION_REVISION=${SDK_VERSION_REVISION:-0}
SDK_VERSION=$SDK_VERSION_MAJOR.$SDK_VERSION_MINOR.$SDK_VERSION_REVISION
SDK_VERSION_SHORT=$(echo $SDK_VERSION | sed 's/\.0$//')

OUT="$PROJECT_ROOT/out"
MAVEN_BASE_URL='http://repository.sonatype.org/service/local/artifact/maven/redirect?r=central-proxy&g=%s&a=%s&p=%s&v=%s'
FACEBOOK_BASE_URL='https://origincache.facebook.com/developers/resources/?id=%s-%s-unity.zip'
UNITY_JAR_RESOLVER_NAME='unity-jar-resolver'
UNITY_JAR_RESOLVER_PACKAGE_NAME='play-services-resolver'
UNITY_JAR_RESOLVER_BASE_URL="https://github.com/googlesamples/$UNITY_JAR_RESOLVER_NAME/raw/master/"
UNITY_JAR_RESOLVER_VERSION='1.2.61.0'
UNITY_JAR_RESOLVER_URL="$UNITY_JAR_RESOLVER_BASE_URL$UNITY_JAR_RESOLVER_PACKAGE_NAME-$UNITY_JAR_RESOLVER_VERSION.unitypackage"

FB_SDK_MODULES=(
  'facebook-applinks'
  'facebook-common'
  'facebook-core'
  'facebook-login'
  'facebook-messenger'
  'facebook-places'
  'facebook-share'
)

function die() {
  echo ""
  echo "${RED}FATAL: $* ${NC}" >&2
  exit 1
}

# Echoes a progress message to stderr
function progress_message() {
    echo "$@" >&2
}

function info() {
  echo "${CYAN}$* ${NC}" >&2
}

function downloadFromMaven() {
  GROUP_ID=$1
  ARTIFACT_ID=$2
  PACKAGING=$3
  VERSION=$4
  MAVEN_DOWNLOAD_URL=$(printf "$MAVEN_BASE_URL" "$GROUP_ID" "$ARTIFACT_ID" "$PACKAGING" "$VERSION")

  OUTPUT_PATH=$5
  curl -L "$MAVEN_DOWNLOAD_URL" -o "$OUTPUT_PATH" || die "Failed download $MAVEN_DOWNLOAD_URL"
}

function downloadFromFacebook() {
  ARTIFACT_ID=$1
  VERSION=$2
  OUTPUT_PATH=$3

  FACEBOOK_DOWNLOAD_URL=$(printf "$FACEBOOK_BASE_URL" "$ARTIFACT_ID" "$VERSION")
  PACKAGE_ZIP="package-$ARTIFACT_ID-$VERSION.zip"
  ARTIFACT_NAME=$(printf "%s-%s" "$ARTIFACT_ID" "$VERSION")
  FACEBOOK_AAR="$ARTIFACT_NAME/%s/%s.aar"

  if [ ! -f "$PACKAGE_ZIP" ]; then
    pushd $PROJECT_ROOT > /dev/null
    curl -L "$FACEBOOK_DOWNLOAD_URL" > $PACKAGE_ZIP
  else
    info "$PACKAGE_ZIP already exists. Skipping download"
  fi

  rm "${OUTPUT_PATH}facebook-"*
  unzip $PACKAGE_ZIP || die "Failed to unzip $FACEBOOK_DOWNLOAD_URL"
  for MODULE in "${FB_SDK_MODULES[@]}"
    do
      FACEBOOK_AAR_FILE=$(printf "$FACEBOOK_AAR" "$MODULE" "$MODULE")
      cp $FACEBOOK_AAR_FILE "$OUTPUT_PATH" || die "Failed to move $FACEBOOK_AAR_FILE to $OUTPUT_PATH"
    done
  rm $PACKAGE_ZIP
  rm -rf $ARTIFACT_NAME
}

function downloadUnityJarResolverFromGithub() {
  UNITY_JAR_RESOLVER_PACKAGE="$UNITY_JAR_RESOLVER_PACKAGE_NAME-$UNITY_JAR_RESOLVER_VERSION.unitypackage"

  pushd $PROJECT_ROOT > /dev/null
  info "Downloading unity-jar-resolver..."
  curl -L "$UNITY_JAR_RESOLVER_URL" > $UNITY_JAR_RESOLVER_PACKAGE || die "Failed to download $UNITY_JAR_RESOLVER_URL"
  info "Importing unity-jar-resolver to UnitySDK project..."

  UNITY_PACKAGE_PATH="$PROJECT_ROOT/$UNITY_JAR_RESOLVER_PACKAGE"

echo "emmet: $UNITY_PACKAGE_PATH"

  $UNITY_PATH -quit -batchmode -logFile -projectPath="$PROJECT_ROOT/UnitySDK" \
   -importPackage "$UNITY_PACKAGE_PATH" || die "Failed to import $UNITY_PACKAGE_PATH"
  info "Cleaning up..."
  rm $UNITY_PACKAGE_PATH
  popd > /dev/null
}

function validate_file_exists() {
  if [ ! -f "$1" ]; then
    echo "${RED}FATAL: File not found $1 ${NC}" >&2
    die $2
  fi
}

function validate_any_file_exists() {
  FILE_COUNT=$(find "$1" -name "$2" | wc -l)

  if [[ $FILE_COUNT -eq 0 ]]; then
    echo "${RED}FATAL: File not found $2 ${NC}" >&2
    die $3
  fi
}
