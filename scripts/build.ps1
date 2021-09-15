#
# Copyright (c) 2014-present, Facebook, Inc. All rights reserved.
#
# You are hereby granted a non-exclusive, worldwide, royalty-free license to use,
# copy, modify, and distribute this software in source code or binary form for use
# in connection with the web services and APIs provided by Facebook.
#
# As with any software that integrates with the Facebook platform, your use of
# this software is subject to the Facebook Developer Principles and Policies
# [http:\\developers.facebook.com\policy\]. This copyright notice shall be
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
function copyright {
    Write-Output "Copyright (c) 2014-present, Facebook, Inc. All rights reserved."
    Write-Output ""
}

copyright

# Cheking working dir
if ( ![System.IO.File]::Exists("$PWD\Facebook.sln") ) {
    Write-Output "Please, execute build.ps1 script from root project directory"
    exit 1
}

if ( ![System.IO.File]::Exists("$PWD\scripts\build.properties") ) {
    Write-Output "Build properties file not found"
    exit 1
}

# Get build version
$properties_file = Get-Content ".\scripts\build.properties"
$UNITY_SDK_BUILD_VERSION = [regex]::Match($properties_file, "UNITY_SDK_BUILD_VERSION='(.*?)'").Groups[1].Value
$UNITY_SDK_BUILD_VERSION = ($UNITY_SDK_BUILD_VERSION).trim()

# Update Version
$project_file = Get-Content ".\Facebook.Unity\FacebookSdkVersion.cs"
$newContent = $project_file -replace 'return (.*)"', "$('return "')$($UNITY_SDK_BUILD_VERSION)$('"')"
Set-Content -Path ".\Facebook.Unity\FacebookSdkVersion.cs" -Value $newContent

$projects_array = @("Facebook.Unity", "Facebook.Unity.Canvas", "Facebook.Unity.Windows", "Facebook.Unity.Editor", "Facebook.Unity.IOS", "Facebook.Unity.IOS.StrippingHack", "Facebook.Unity.Android", "Facebook.Unity.Android.StrippingHack", "Facebook.Unity.Settings", "Facebook.Unity.Tests")
foreach ($PROJECT in $projects_array) {
    # Update Version
    Write-Output "Update $UNITY_SDK_BUILD_VERSION to project $PROJECT"
    $fileProjectPath = ".\" + $PROJECT + "\Properties\AssemblyInfo.cs"
    $AssemblyInfo = Get-Content $fileProjectPath
    $newValue = '[assembly: AssemblyVersion("' + $UNITY_SDK_BUILD_VERSION + '")]'
    $newContent = $AssemblyInfo -replace '\[assembly: AssemblyVersion(.*)\]', $newValue
    Set-Content -Path ".\$PROJECT\Properties\AssemblyInfo.cs" -Value $newContent
}

$msbuild_path = (&"${env:ProgramFiles(x86)}\Microsoft Visual Studio\Installer\vswhere.exe" -latest -prerelease -products * -requires Microsoft.Component.MSBuild -find MSBuild\**\Bin\MSBuild.exe)
$parameters = "$PWD\Facebook.sln"
$add = "/property:Configuration=Release"

& $msbuild_path $parameters $add

$UNITY_PACKAGE_ROOT = "$PWD\UnitySDK"
$UNITY_PACKAGE_PLUGIN = "$UNITY_PACKAGE_ROOT\Assets\FacebookSDK\Plugins"

$UNITY_ANDROID_PLUGIN = "$UNITY_PACKAGE_PLUGIN\Android\"
$UNITY_CANVAS_PLUGIN = "$UNITY_PACKAGE_PLUGIN\Canvas\"
$UNITY_EDITOR_PLUGIN = "$UNITY_PACKAGE_PLUGIN\Editor\"
$UNITY_IOS_PLUGIN = "$UNITY_PACKAGE_PLUGIN\iOS\"
$UNITY_SETTINGS_PLUGIN = "$UNITY_PACKAGE_PLUGIN\Settings\"
$UNITY_WINDOWS_PLUGIN = "$UNITY_PACKAGE_PLUGIN\Windows\"

$CORE_ROOT="$PWD\Facebook.Unity"
$CORE_DLL="$CORE_ROOT\bin\Release\Facebook.Unity.dll"

$CANVAS_ROOT="$PWD\Facebook.Unity.Canvas"
$CANVAS_DLL="$CANVAS_ROOT\bin\Release\Facebook.Unity.Canvas.dll"
$CANVAS_JSLIB="$CANVAS_ROOT\bin\Release\CanvasJSSDKBindings.jslib"

$ANDROID_ROOT="$PWD\Facebook.Unity.Android"
$ANDROID_DLL="$ANDROID_ROOT\bin\Release\Facebook.Unity.Android.dll"

$WINDOWS_ROOT="$PWD\Facebook.Unity.Windows"
$WINDOWS_DLL="$WINDOWS_ROOT\bin\Release\Facebook.Unity.Windows.dll"
$WINDOWS_SDK_DLL_DIR="$WINDOWS_ROOT\Plugin"

$EDITOR_ROOT="$PWD\Facebook.Unity.Editor"
$EDITOR_DLL="$EDITOR_ROOT\bin\Release\Facebook.Unity.Editor.dll"

$IOS_ROOT="$PWD\Facebook.Unity.IOS"
$IOS_DLL="$IOS_ROOT\bin\Release\Facebook.Unity.IOS.dll"

$SETTINGS_ROOT="$PWD\Facebook.Unity.Settings"
$SETTINGS_DLL="$SETTINGS_ROOT\bin\Release\Facebook.Unity.Settings.dll"

# Create folders
if (-Not (Test-Path -Path $UNITY_ANDROID_PLUGIN)) {
        New-Item -ItemType Directory -Path $UNITY_ANDROID_PLUGIN -Force
}

if (-Not(Test-Path -Path $UNITY_CANVAS_PLUGIN)) {
    New-Item -ItemType Directory -Path $UNITY_CANVAS_PLUGIN -Force
}

if (-Not(Test-Path -Path $UNITY_EDITOR_PLUGIN)) {
    New-Item -ItemType Directory -Path $UNITY_EDITOR_PLUGIN -Force
}

if (-Not(Test-Path -Path $UNITY_IOS_PLUGIN)) {
    New-Item -ItemType Directory -Path $UNITY_IOS_PLUGIN -Force
}

if (-Not(Test-Path -Path $UNITY_SETTINGS_PLUGIN)) {
    New-Item -ItemType Directory -Path $UNITY_SETTINGS_PLUGIN -Force
}

if (-Not(Test-Path -Path $UNITY_WINDOWS_PLUGIN)) {
    New-Item -ItemType Directory -Path $UNITY_WINDOWS_PLUGIN -Force
}

# Copy DLL
Copy-Item $CORE_DLL "$UNITY_PACKAGE_PLUGIN\" -Force
Copy-Item $CANVAS_DLL $UNITY_CANVAS_PLUGIN -Force
Copy-Item $CANVAS_JSLIB $UNITY_CANVAS_PLUGIN -Force
Copy-Item $ANDROID_DLL $UNITY_ANDROID_PLUGIN -Force
Copy-Item $WINDOWS_DLL $UNITY_WINDOWS_PLUGIN -Force
Copy-Item "$WINDOWS_SDK_DLL_DIR*" $UNITY_WINDOWS_PLUGIN -Recurse -Force
Copy-Item $EDITOR_DLL $UNITY_EDITOR_PLUGIN -Force
Copy-Item $IOS_DLL $UNITY_IOS_PLUGIN -Force
Copy-Item $SETTINGS_DLL $UNITY_SETTINGS_PLUGIN -Force

# Compile Unity Project
$UNITY_CSPROJ="$UNITY_PACKAGE_ROOT\Assembly-CSharp.csproj"
$add = "/property:Configuration=Release"
& $msbuild_path $UNITY_CSPROJ $add
