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

. "$(dirname "$0")/common.sh"

cd "$(dirname "$0")/.." || die "$(dirname "$0")/.. not found"
PROJECT_ROOT=$(pwd)
. "$PROJECT_ROOT/scripts/build.properties"

rm -rf tempIosBuild
mkdir tempIosBuild
cd tempIosBuild || die "Directory tempIosBuild not found"

UNITY_PLUGIN_FACEBOOK="$UNITY_PACKAGE_ROOT/Assets/FacebookSDK/Plugins/iOS"
mkdir -p "$UNITY_PLUGIN_FACEBOOK"
rm -rf "$UNITY_PLUGIN_FACEBOOK/*.framework"

cd "$PROJECT_ROOT" || die "$PROJECT_ROOT not found"
rm -rf tempIosBuild
