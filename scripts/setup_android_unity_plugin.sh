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

. $(dirname $0)/common.sh

PROPS_PATH="$PROJECT_ROOT/scripts/build.properties"
source $PROPS_PATH

info "Starting build"
# Check for required settings
if [ -z "$ANDROID_HOME" ]; then
    echo "${RED}ERROR: ANDROID_HOME environment variable not set${NC}"
    echo "${RED}Please set the ANDROID_HOME environment variable to point to your android sdk${NC}"
    exit 1
fi

localBuild=false
if [[ $* == *--local* ]]; then
  localBuild=true
fi

# Copy the required libs
UNITY_PLUGIN_FACEBOOK="$UNITY_PACKAGE_ROOT/Assets/FacebookSDK/Plugins/Android/libs/"

FB_WRAPPER_PATH=$PROJECT_ROOT/facebook-android-wrapper
FB_WRAPPER_LIB_PATH=$FB_WRAPPER_PATH/libs
FB_ANDROID_SDK_WRAPPER_NAME="facebook-android-wrapper-release.aar"
FB_ANDROID_SDK_WRAPPER="$FB_WRAPPER_PATH/build/outputs/aar/$FB_ANDROID_SDK_WRAPPER_NAME"

FB_SDK_AAR_NAME="$FB_ANDROID_SDK_ARTIFACT_ID-$FB_ANDROID_SDK_VERSION.$FB_ANDROID_SDK_PACKAGING"
FB_SDK_AAR_PATH="$FB_WRAPPER_LIB_PATH/$FB_SDK_AAR_NAME"
FB_SDK_AAR_PATH_FOLDER="$FB_WRAPPER_LIB_PATH/"

BOLTS_SDK_JAR_NAME="$BOLTS_ARTIFACT_ID-$BOLTS_VERSION.jar"
BOLTS_JAR_PATH="$FB_WRAPPER_LIB_PATH/$BOLTS_SDK_JAR_NAME"
BOLTS_TASKS_SDK_JAR_NAME="$BOLTS_TASKS_ARTIFACT_ID-$BOLTS_VERSION.jar"
BOLTS_TASKS_JAR_PATH="$FB_WRAPPER_LIB_PATH/$BOLTS_TASKS_SDK_JAR_NAME"
BOLTS_APPLINKS_SDK_JAR_NAME="$BOLTS_APPLINKS_ARTIFACT_ID-$BOLTS_VERSION.jar"
BOLTS_APPLINKS_JAR_PATH="$FB_WRAPPER_LIB_PATH/$BOLTS_APPLINKS_SDK_JAR_NAME"

# Local build only properties
FB_ANDROID_SDK_AAR="$FB_ANDROID_SDK_PATH/facebook/build/outputs/aar/facebook-release.aar"

info "Step 1 - Cleaning wrapper libs folder"
if [ ! -d "$FB_WRAPPER_LIB_PATH" ]; then
  mkdir -p $FB_WRAPPER_LIB_PATH || die "Failed to create wrapper libs folder"
fi
pushd "$FB_WRAPPER_LIB_PATH" || die "Cannot navigate to directory $FB_WRAPPER_LIB_PATH"
# Only delete everything except the expected bolts and sdk versions
find . ! -name $BOLTS_SDK_JAR_NAME ! -name $FB_SDK_AAR_NAME -maxdepth 1 -type f -delete
popd

info "Step 2 - Get dependencies for android wrapper"
info "Step 2.1.0 - Download $BOLTS_SDK_JAR_NAME"
if [ ! -f "$BOLTS_JAR_PATH" ]; then
  downloadFromMaven $BOLTS_GROUP_ID $BOLTS_ARTIFACT_ID $BOLTS_PACKAGING $BOLTS_VERSION "$BOLTS_JAR_PATH" || die "Failed to download from maven"
else
  info "$BOLTS_SDK_JAR_NAME already exists. Skipping download."
fi

info "Step 2.1.1 - Download $BOLTS_TASKS_SDK_JAR_NAME"
if [ ! -f "$BOLTS_TASKS_JAR_PATH" ]; then
  downloadFromMaven $BOLTS_GROUP_ID $BOLTS_TASKS_ARTIFACT_ID $BOLTS_PACKAGING $BOLTS_VERSION "$BOLTS_TASKS_JAR_PATH"
else
  info "$BOLTS_TASKS_SDK_JAR_NAME already exists. Skipping download."
fi

info "Step 2.1.2 - Download $BOLTS_APPLINKS_SDK_JAR_NAME"
if [ ! -f "$BOLTS_APPLINKS_JAR_PATH" ]; then
  downloadFromMaven $BOLTS_GROUP_ID $BOLTS_APPLINKS_ARTIFACT_ID $BOLTS_PACKAGING $BOLTS_VERSION "$BOLTS_APPLINKS_JAR_PATH"
else
  info "$BOLTS_APPLINKS_SDK_JAR_NAME already exists. Skipping download."
fi

# Get Unity Jar Resolver
info "Step 2.1.3 - Download $UNITY_JAR_RESOLVER_NAME"
downloadUnityJarResolverFromGithub

# Get the android sdk
if [ "$localBuild" = true ]; then
  info "Step 2.2.0 - Build local android sdk at '$FB_ANDROID_SDK_PATH'"
  pushd $FB_ANDROID_SDK_PATH
  ./gradlew :facebook:assemble || die "Failed to build facebook sdk"
  popd
  info "Step 2.2.1 - Copy FB_ANDROID_SDK_PATH to lib folder"
  cp $FB_ANDROID_SDK_AAR $FB_SDK_AAR_PATH || die "Failed to copy sdk to wrapper libs folder"
else
  info "Step 2.2 - Download $FB_SDK_AAR_NAME"
  downloadFromFacebook $FB_ANDROID_SDK_ARTIFACT_ID $FB_ANDROID_SDK_VERSION "$FB_SDK_AAR_PATH_FOLDER" || die "failed to download sdk from maven"
fi

info "Step 3 - Build android wrapper"
pushd $FB_WRAPPER_PATH
if [ "$localBuild" = true ]; then
  ./gradlew clean -PlocalRepo=libs -PsdkVersion=$FB_ANDROID_SDK_VERSION || die "Failed to perform gradle clean"
  ./gradlew assemble -PlocalRepo=libs -PsdkVersion=$FB_ANDROID_SDK_VERSION || die "Failed to build facebook android wrapper"
else
  ./gradlew clean -PsdkVersion=$FB_ANDROID_SDK_VERSION || die "Failed to perform gradle clean"
  ./gradlew assemble -PsdkVersion=$FB_ANDROID_SDK_VERSION || die "Failed to build facebook android wrapper"
fi
popd

info "Step 4 - Copy libs to unity plugin folder"
if [ ! -d "$UNITY_PLUGIN_FACEBOOK" ]; then
  mkdir -p $UNITY_PLUGIN_FACEBOOK || die "Failed to make unity plugin lib folder"
fi
# clean the unity lib folder
rm -r -f $UNITY_PLUGIN_FACEBOOK/*.jar
rm -r -f $UNITY_PLUGIN_FACEBOOK/*.aar
# Copy aars
cp "${FB_SDK_AAR_PATH_FOLDER}facebook-"* $UNITY_PLUGIN_FACEBOOK || die 'Failed to copy fb sdk to unity plugin folders'
cp $FB_ANDROID_SDK_WRAPPER  $UNITY_PLUGIN_FACEBOOK || die 'Failed to copy wrapper to unity plugin folder'
# Rename wrapper to include sdk version
mv $UNITY_PLUGIN_FACEBOOK/$FB_ANDROID_SDK_WRAPPER_NAME "$UNITY_PLUGIN_FACEBOOK/facebook-android-wrapper-$SDK_VERSION.aar"

if [ "$localBuild" = false ]; then
  # For local builds the jars are included in the wrapper for maven builds we have to copy them over
  cp $BOLTS_JAR_PATH $UNITY_PLUGIN_FACEBOOK || die 'Failed to copy bolts aar to unity plugin folder'
  cp $BOLTS_TASKS_JAR_PATH $UNITY_PLUGIN_FACEBOOK || die 'Failed to copy bolts tasks jar to unity plugin folder'
  cp $BOLTS_APPLINKS_JAR_PATH $UNITY_PLUGIN_FACEBOOK || die 'Failed to copy bolts app links jar to unity plugin folder'
else
  cp $FB_WRAPPER_LIB_PATH/* $UNITY_PLUGIN_FACEBOOK || die 'Failed to copy wrapper lib files'
fi

info "Done!"
