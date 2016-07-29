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

PROJECT_SLN=$PROJECT_ROOT/Facebook.sln
UNITY_CSPROJ=$UNITY_PACKAGE_ROOT/Assembly-CSharp.csproj

ARCADE_ROOT=$PROJECT_ROOT/Facebook.Unity.Arcade
ARCADE_DLL=$ARCADE_ROOT/bin/Release/Facebook.Unity.Arcade.dll
ARCADE_NAMED_PIPE_DLL=$ARCADE_ROOT/bin/Release/FacebookNamedPipeClient.dll

EDITOR_ROOT=$PROJECT_ROOT/Facebook.Unity.Editor
EDITOR_DLL=$EDITOR_ROOT/bin/Release/Facebook.Unity.Editor.dll

CORE_ROOT=$PROJECT_ROOT/Facebook.Unity
CORE_DLL=$CORE_ROOT/bin/Release/Facebook.Unity.dll

TEST_ROOT=$PROJECT_ROOT/Facebook.Unity.Tests

###############################################################################
# UPDATE BUILD VERSION
###############################################################################
sed -i "" -e "s/[0-9]\.[0-9]\.[0-9]/$UNITY_SDK_BUILD_VERSION/g" "$PROJECT_ROOT/Facebook.Unity/FacebookSdkVersion.cs" || die "Failed to update the version"
sed -i "" -e "s/AssemblyVersion(\"[0-9]\.[0-9]\.[0-9]\")/AssemblyVersion(\"$UNITY_SDK_BUILD_VERSION\")/g" \
"$PROJECT_ROOT/Facebook.Unity/Properties/AssemblyInfo.cs" \
"$PROJECT_ROOT/Facebook.Unity.Arcade/Properties/AssemblyInfo.cs" \
"$PROJECT_ROOT/Facebook.Unity.Editor/Properties/AssemblyInfo.cs" \
"$PROJECT_ROOT/Facebook.Unity.Tests/Properties/AssemblyInfo.cs" || die "Failed to update the DLL versions"

###############################################################################
# BUILD SDK
###############################################################################
which mono &>/dev/null || die "mono command not found. Please install mono."
xbuild /p:Configuration=Release $PROJECT_SLN || die "Facebook.sln Build Failed"

###############################################################################
# COPY DLLS
###############################################################################
if [ ! -d "$UNITY_PACKAGE_PLUGIN" ]; then
  mkdir -p $UNITY_PACKAGE_PLUGIN || die "Failed to create core plugins folder"
fi
cp $CORE_DLL $UNITY_PACKAGE_PLUGIN || die "Failed to copy core DLL"

if [ ! -d "$UNITY_ARCADE_PLUGIN" ]; then
  mkdir -p $UNITY_ARCADE_PLUGIN || die "Failed to create Arcade plugins folder"
fi
cp $ARCADE_DLL $UNITY_ARCADE_PLUGIN || die "Failed to copy Arcade DLL"

cp $ARCADE_NAMED_PIPE_DLL $UNITY_ARCADE_PLUGIN || die "Failed to copy FacebookNamedPipeClient DLL"

if [ ! -d "$UNITY_EDITOR_PLUGIN" ]; then
  mkdir -p $UNITY_EDITOR_PLUGIN || die "Failed to create Editor plugins folder"
fi
cp $EDITOR_DLL $UNITY_EDITOR_PLUGIN || die "Failed to copy Editor DLL"

###############################################################################
# BUILD EXAMPLE
###############################################################################
validate_file_exists $UNITY_PACKAGE_ROOT/Assembly-CSharp.csproj $CSPROJERROR
xbuild /p:Configuration=Release $UNITY_CSPROJ || die "Failed to build SDK DLL"
