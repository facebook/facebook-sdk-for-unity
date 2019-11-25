#!/bin/sh
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
# CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

# shellcheck disable=SC2039

# --------------
# Main Script
# --------------

CACHE="$(pwd)/cache"
LOGFILE="$(pwd)/tmp.log"

OSX_DOTNET_PACKAGE_URL="https://download.visualstudio.microsoft.com/download/pr/5c281f95-91c4-499d-baa2-31fec919047a/38c6964d72438ac30032bce516b655d9/dotnet-sdk-3.0.100-osx-x64.pkg"
OSX_UNITY_PACKAGE_URL="https://download.unity3d.com/download_unity/e80cc3114ac1/MacEditorInstaller/Unity-5.6.7f1.pkg"

install() {
  # Download Unity Package from URL if it doesn't exist in cache directory
  local url=${1:-}
  local filename=$(basename $url)

  if [ ! -f "$CACHE/$filename" ]; then
    echo "*** NOTE: unable to find $filename, download from $url."
    mkdir -p "$CACHE"
    curl -o "$CACHE/$filename" "$url"
  fi

  # Install Unity Package
  sudo installer -dumplog -package "$CACHE/$filename" -target /
}

brew tap caskroom/cask
brew cask install android-sdk  > "$LOGFILE"
brew install gradle  > "$LOGFILE"

echo yes | $ANDROID_HOME/tools/bin/sdkmanager "platforms;android-27" "build-tools;27.0.3" > "$LOGFILE"

install $OSX_DOTNET_PACKAGE_URL
install $OSX_UNITY_PACKAGE_URL
