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

UNITY_CSPROJ=$UNITY_PACKAGE_ROOT/Assets/ExternalDependencyManager/Editor

fix_meta(){
  cp "$1" "$UNITY_CSPROJ/meta.txt"
  sed s/_v1.2.166././g "$UNITY_CSPROJ/meta.txt" > "$1"
  rm "$UNITY_CSPROJ/meta.txt"
}

set_name(){
  if [ ! -f "$1" ]; then
    echo "Missing DLL"
    echo "$1"
    exit 1
  fi
  mv "$1" "$2"
  mv "$1.meta" "$2.meta"
  fix_meta "$2.meta"
}

set_name $UNITY_CSPROJ/Google.IOSResolver*.dll "$UNITY_CSPROJ/Google.IOSResolver.dll"
set_name $UNITY_CSPROJ/Google.IOSResolver*.dll.mdb "$UNITY_CSPROJ/Google.IOSResolver.dll.mdb"
set_name $UNITY_CSPROJ/Google.JarResolver*.dll "$UNITY_CSPROJ/Google.JarResolver.dll"
set_name $UNITY_CSPROJ/Google.JarResolver*.dll.mdb "$UNITY_CSPROJ/Google.JarResolver.dll.mdb"
set_name $UNITY_CSPROJ/Google.PackageManagerResolver*.dll "$UNITY_CSPROJ/Google.PackageManagerResolver.dll"
set_name $UNITY_CSPROJ/Google.PackageManagerResolver*.dll.mdb "$UNITY_CSPROJ/Google.PackageManagerResolver.dll.mdb"
set_name $UNITY_CSPROJ/Google.VersionHandler*.dll "$UNITY_CSPROJ/Google.VersionHandler.dll"
set_name $UNITY_CSPROJ/Google.VersionHandler*.dll.mdb "$UNITY_CSPROJ/Google.VersionHandler.dll.mdb"
set_name $UNITY_CSPROJ/Google.VersionHandlerImpl*.dll "$UNITY_CSPROJ/Google.VersionHandlerImpl.dll"
set_name $UNITY_CSPROJ/Google.VersionHandlerImpl*.dll.mdb "$UNITY_CSPROJ/Google.VersionHandlerImpl.dll.mdb"

cp "$UNITY_CSPROJ/external-dependency-manager_version-1.2.166_manifest.txt" $UNITY_CSPROJ/tmp.txt
## TO DO: obtain the dependency version dinamically
sed s/_v1.2.166././g "$UNITY_CSPROJ/tmp.txt" > "$UNITY_CSPROJ/external-dependency-manager_version-1.2.166_manifest.txt"
rm "$UNITY_CSPROJ/tmp.txt"
