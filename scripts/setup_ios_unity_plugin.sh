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

cd $(dirname $0)/..
PROJECT_ROOT=$(pwd)
PROPS_PATH="$PROJECT_ROOT/scripts/build.properties"
source $PROPS_PATH

rm -rf tempIosBuild
mkdir tempIosBuild
cd tempIosBuild

UNITY_PLUGIN_FACEBOOK="$UNITY_PACKAGE_ROOT/Assets/Plugins/iOS/Facebook"
mkdir -p "$UNITY_PLUGIN_FACEBOOK"
rm -rf "$UNITY_PLUGIN_FACEBOOK/*"

if [[ $* == *--local* ]]; then
    info "Local build selected"
    if [ -z "$FB_IOS_SDK_PATH" ]; then
        die "Please set the FB_IOS_SDK_PATH variable in $LOCAL_PROPS"
    fi
    sdkFolder="$FB_IOS_SDK_PATH"
else
    if [ -z "$FB_IOS_SDK_VERSION" ]; then
      echo "${RED}Error: 'FB_IOS_SDK_VERSION' not defined in $PROPS_PATH ${NC}"
      exit 1
    fi

    if [ -z "$FB_IOS_RESOURCE_NAME" ]; then
      echo "${RED}Error: 'FB_IOS_RESOURCE_NAME' not defined in $PROPS_PATH ${NC}"
      exit 1
    fi
    packageName="facebook-ios-sdk-$FB_IOS_SDK_VERSION"
    curl -L "http://fb.me/$FB_IOS_RESOURCE_NAME" -o "$packageName.zip"

    unzip -q "$packageName.zip" -d $packageName
    sdkFolder="$PROJECT_ROOT/tempIosBuild/$packageName"
fi

for FRAMEWORK in FBSDKCoreKit.framework FBSDKLoginKit.framework FBSDKShareKit.framework Bolts.framework; do
    cp -r -f "$sdkFolder/$FRAMEWORK" "$UNITY_PLUGIN_FACEBOOK" || die "failed to copy $FRAMEWORK"
done

cd $PROJECT_ROOT
rm -r -f tempIosBuild
