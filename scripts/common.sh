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
UNITY_PACKAGE_ROOT=$PROJECT_ROOT/Facebook.Unity
SCRIPTS_DIR="$PROJECT_ROOT/scripts"

RED='\033[0;31m'
NC='\033[0m'
CYAN='\033[0;36m'

# Load settings
source $PROJECT_ROOT/scripts/build.properties
LOCAL_PROPS=$PROJECT_ROOT/scripts/local.properties
if [ -f "$LOCAL_PROPS" ]; then
  source $PROJECT_ROOT/scripts/local.properties
else
  echo "No properties file found at $LOCAL_PROPS"
fi

# Extract the SDK version from FacebookSdkVersion.java
SDK_VERSION_RAW=$(sed -n 's/.*"\(.*\)\";/\1/p' "$UNITY_PACKAGE_ROOT/Assets/Facebook/Scripts/FacebookSdkVersion.cs")
SDK_VERSION_MAJOR=$(echo $SDK_VERSION_RAW | awk -F'.' '{print $1}')
SDK_VERSION_MAJOR=${SDK_VERSION_MAJOR:-0}
SDK_VERSION_MINOR=$(echo $SDK_VERSION_RAW | awk -F'.' '{print $2}')
SDK_VERSION_MINOR=${SDK_VERSION_MINOR:-0}
SDK_VERSION_REVISION=$(echo $SDK_VERSION_RAW | awk -F'.' '{print $3}')
SDK_VERSION_REVISION=${SDK_VERSION_REVISION:-0}
SDK_VERSION=$SDK_VERSION_MAJOR.$SDK_VERSION_MINOR.$SDK_VERSION_REVISION
SDK_VERSION_SHORT=$(echo $SDK_VERSION | sed 's/\.0$//')

OUT="$PROJECT_ROOT/out"
DEV_SERVER=$(whoami).sb.facebook.com
MAVEN_BASE_URL='http://repository.sonatype.org/service/local/artifact/maven/redirect?r=central-proxy&g=%s&a=%s&p=%s&v=%s'

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

function add_resource() {
  RES=$1
  scp $OUT/$RES ${DEV_SERVER}:/tmp/$RES \
    || die "Error copying $RES to ${DEV_SERVER}:/tmp/$RES"
  ssh $DEV_SERVER '~/www/scripts/developer/resource_admin' add /tmp/$RES >/tmp/$RES.handle.txt \
    || die "Error running resource_admin add $RES"
  echo "$RES handle: $(cat /tmp/$RES.handle.txt)"
}
