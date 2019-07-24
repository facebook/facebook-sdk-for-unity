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

# Run build script to ensure that test DLL is built
"$SCRIPTS_DIR/build.sh" || die "Build failed"

command -v mono >/dev/null 2>&1 || die "mono command not found. Please install mono."

NSUBSTITUTE_CONSOLE="$PROJECT_ROOT/packages/NSubstitute.2.0.3/lib"
NUNIT_CONSOLE="$PROJECT_ROOT/packages/NUnit.ConsoleRunner.3.10.0/tools/nunit3-console.exe"
TEST_DLL="$PROJECT_ROOT/Facebook.Unity.Tests/bin/Release/Facebook.Unity.Tests.dll"

validate_any_file_exists "$NSUBSTITUTE_CONSOLE" "NSubstitute.dll" "Make sure NSubstitute is installed at /packages"
validate_file_exists "$NUNIT_CONSOLE" "Make sure mono is installed at this path"
validate_file_exists "$TEST_DLL" "Make sure that the Unity Project successfully built"

mono "$NUNIT_CONSOLE" "$TEST_DLL" --workers=0 || die "Some tests failed"
