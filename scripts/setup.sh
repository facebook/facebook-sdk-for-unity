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

command -v nuget >/dev/null 2>&1 || die "nuget command not found. Please install nuget."
echo "checking packages..."
nuget restore "$PROJECT_ROOT"
echo "checking packages done."

"$SCRIPTS_DIR/setup_ios_unity_plugin.sh" "$@" || die "Failed to setup the ios sdk plugin"
"$SCRIPTS_DIR/setup_android_unity_plugin.sh" "$@" || die "Failed to build the android sdk plugin"

cd "$SCRIPTS_DIR" || die "$SCRIPTS_DIR not found"
./build.sh
