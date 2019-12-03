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

# shellcheck disable=SC2039

. "$(dirname "$0")/common.sh"

BUILD_TYPE="Release"
localSettings=false

while [ $# -gt 0 ]; do
	opt="$1"
	shift
	case $opt in
		--debug)
			BUILD_TYPE=Debug
			;;
		--release)
			;;
		--local)
			localSettings=true
			;;
		*)
			die "Invalid arguments"
			;;
	esac
done

# Load settings
source "$PROJECT_ROOT/scripts/build.properties"
LOCAL_PROPS=$PROJECT_ROOT/scripts/local.properties
if [ "$localSettings" = true ]; then
  if [ -f "$LOCAL_PROPS" ]; then
    source "$LOCAL_PROPS"
  else
    echo "No properties file found at $LOCAL_PROPS"
  fi
fi

PROJECT_SLN=$PROJECT_ROOT/Facebook.sln
UNITY_CSPROJ=$UNITY_PACKAGE_ROOT/Assembly-CSharp.csproj

ANDROID_ROOT=$PROJECT_ROOT/Facebook.Unity.Android
ANDROID_DLL=$ANDROID_ROOT/bin/Release/Facebook.Unity.Android.dll

CANVAS_ROOT=$PROJECT_ROOT/Facebook.Unity.Canvas
CANVAS_DLL=$CANVAS_ROOT/bin/Release/Facebook.Unity.Canvas.dll
CANVAS_JSLIB=$CANVAS_ROOT/bin/Release/CanvasJSSDKBindings.jslib

GAMEROOM_ROOT=$PROJECT_ROOT/Facebook.Unity.Gameroom
GAMEROOM_DLL=$GAMEROOM_ROOT/bin/Release/Facebook.Unity.Gameroom.dll
GAMEROOM_NAMED_PIPE_DLL=$GAMEROOM_ROOT/bin/Release/FacebookNamedPipeClient.dll

EDITOR_ROOT=$PROJECT_ROOT/Facebook.Unity.Editor
EDITOR_DLL=$EDITOR_ROOT/bin/Release/Facebook.Unity.Editor.dll

IOS_ROOT=$PROJECT_ROOT/Facebook.Unity.IOS
IOS_DLL=$IOS_ROOT/bin/Release/Facebook.Unity.IOS.dll

SETTINGS_ROOT=$PROJECT_ROOT/Facebook.Unity.Settings
SETTINGS_DLL=$SETTINGS_ROOT/bin/Release/Facebook.Unity.Settings.dll

IOS_STRIPPING_HACK_ROOT=$PROJECT_ROOT/Facebook.Unity.IOS.StrippingHack
IOS_STRIPPING_HACK_DLL=$IOS_STRIPPING_HACK_ROOT/bin/Release/Facebook.Unity.IOS.dll
ANDROID_STRIPPING_HACK_ROOT=$PROJECT_ROOT/Facebook.Unity.Android.StrippingHack
ANDROID_STRIPPING_HACK_DLL=$ANDROID_STRIPPING_HACK_ROOT/bin/Release/Facebook.Unity.Android.dll

CORE_DLL=$CORE_ROOT/bin/Release/Facebook.Unity.dll

###############################################################################
# UPDATE BUILD VERSION
###############################################################################
sed -i "" -e "s/[0-9]\.[0-9][0-9]\.[0-9]/$UNITY_SDK_BUILD_VERSION/g" "$PROJECT_ROOT/Facebook.Unity/FacebookSdkVersion.cs" || die "Failed to update the version"
sed -i "" -e "s/AssemblyVersion(\"[0-9]\.[0-9][0-9]\.[0-9]\")/AssemblyVersion(\"$UNITY_SDK_BUILD_VERSION\")/g" \
"$PROJECT_ROOT/Facebook.Unity/Properties/AssemblyInfo.cs" \
"$PROJECT_ROOT/Facebook.Unity.Canvas/Properties/AssemblyInfo.cs" \
"$PROJECT_ROOT/Facebook.Unity.Gameroom/Properties/AssemblyInfo.cs" \
"$PROJECT_ROOT/Facebook.Unity.Editor/Properties/AssemblyInfo.cs" \
"$PROJECT_ROOT/Facebook.Unity.IOS/Properties/AssemblyInfo.cs" \
"$PROJECT_ROOT/Facebook.Unity.IOS.StrippingHack/Properties/AssemblyInfo.cs" \
"$PROJECT_ROOT/Facebook.Unity.Android/Properties/AssemblyInfo.cs" \
"$PROJECT_ROOT/Facebook.Unity.Android.StrippingHack/Properties/AssemblyInfo.cs" \
"$PROJECT_ROOT/Facebook.Unity.Settings/Properties/AssemblyInfo.cs" \
"$PROJECT_ROOT/Facebook.Unity.Tests/Properties/AssemblyInfo.cs" || die "Failed to update the DLL versions"

###############################################################################
# BUILD SDK
###############################################################################
command -v mono >/dev/null 2>&1 || die "mono command not found. Please install mono."
msbuild /p:Configuration=$BUILD_TYPE "$PROJECT_SLN" || die "Facebook.sln Build Failed"

###############################################################################
# COPY PLUGINS
###############################################################################
if [ ! -d "$UNITY_PACKAGE_PLUGIN" ]; then
  mkdir -p "$UNITY_PACKAGE_PLUGIN" || die "Failed to create core plugins folder"
fi
cp "$CORE_DLL" "$UNITY_PACKAGE_PLUGIN" || die "Failed to copy core DLL"

if [ ! -d "$UNITY_CANVAS_PLUGIN" ]; then
  mkdir -p "$UNITY_CANVAS_PLUGIN" || die "Failed to create Canvas plugins folder"
fi
cp "$CANVAS_DLL" "$UNITY_CANVAS_PLUGIN" || die "Failed to copy Canvas DLL"
cp "$CANVAS_JSLIB" "$UNITY_CANVAS_PLUGIN" || die "Failed to copy Canvas JSLIB"

if [ ! -d "$UNITY_ANDROID_PLUGIN" ]; then
  mkdir -p "$UNITY_ANDROID_PLUGIN" || die "Failed to create Android plugins folder"
fi
cp "$ANDROID_DLL" "$UNITY_ANDROID_PLUGIN" || die "Failed to copy Android DLL"

if [ ! -d "$UNITY_GAMEROOM_PLUGIN" ]; then
  mkdir -p "$UNITY_GAMEROOM_PLUGIN" || die "Failed to create Gameroom plugins folder"
fi
cp "$GAMEROOM_DLL" "$UNITY_GAMEROOM_PLUGIN" || die "Failed to copy Gameroom DLL"
cp "$GAMEROOM_NAMED_PIPE_DLL" "$UNITY_GAMEROOM_PLUGIN" || die "Failed to copy FacebookNamedPipeClient DLL"

if [ ! -d "$UNITY_EDITOR_PLUGIN" ]; then
  mkdir -p "$UNITY_EDITOR_PLUGIN" || die "Failed to create Editor plugins folder"
fi
cp "$EDITOR_DLL" "$UNITY_EDITOR_PLUGIN" || die "Failed to copy Editor DLL"

if [ ! -d "$UNITY_IOS_PLUGIN" ]; then
  mkdir -p "$UNITY_IOS_PLUGIN" || die "Failed to create IOS plugins folder"
fi
cp "$IOS_DLL" "$UNITY_IOS_PLUGIN" || die "Failed to copy IOS DLL"

if [ ! -d "$UNITY_SETTINGS_PLUGIN" ]; then
  mkdir -p "$UNITY_SETTINGS_PLUGIN" || die "Failed to create Settings plugins folder"
fi
cp "$SETTINGS_DLL" "$UNITY_SETTINGS_PLUGIN" || die "Failed to copy Settings DLL"

###############################################################################
# BUILD EXAMPLE
###############################################################################
validate_file_exists "$UNITY_PACKAGE_ROOT/Assembly-CSharp.csproj" "To generate csproj files open this project in unity at least once"
msbuild /p:Configuration=$BUILD_TYPE "$UNITY_CSPROJ" || die "Failed to build SDK DLL"
