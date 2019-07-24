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

# This file performs the custom build logic to setup the Plugins
# in the unity project

. "$(dirname "$0")/common.sh"

UNITY_PATH="/Applications/Unity/Unity.app/Contents/MacOS/Unity"
$UNITY_PATH -batchmode -nographics -projectPath "$UNITY_PACKAGE_ROOT" -executeMethod FacebookConsoleEndpoint.ExportPackage -quit \
|| die "Failed to export package. Make sure the Facebook.Unity project is not open in Unity"

info "Unity Package exported to $PROJECT_ROOT/out/facebook-unity-sdk-$SDK_VERSION.unitypackage"
