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

. $(dirname $0)/common.sh

CSPROJ_ERROR="To generate csproj files open this project in unity at least once"
SDK_CSPROJ=$UNITY_PACKAGE_ROOT/Assembly-CSharp.csproj
SDK_EDITOR_CSPROJ=$UNITY_PACKAGE_ROOT/Assembly-CSharp-Editor.csproj
TEST_CSPROJ=$TEST_ROOT/Facebook.Unity.Tests.csproj

validate_file_exists $UNITY_PACKAGE_ROOT/Assembly-CSharp.csproj $CSPROJERROR
validate_file_exists $UNITY_PACKAGE_ROOT/Assembly-CSharp-Editor.csproj $CSPROJERROR

which mono &>/dev/null || die "mono command not found. Please install mono."

// Build the core sdk
xbuild /p:Configuration=Release $SDK_CSPROJ || die "Failed to build SDK DLL"

// Buidl the editor support
xbuild /p:Configuration=Release $SDK_EDITOR_CSPROJ || die "Failed to build SDK Edior DLL"

// Build unit test proejct
xbuild /p:Configuration=Release $TEST_CSPROJ || die "Failed to build test DLL"
