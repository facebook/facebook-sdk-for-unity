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

$CPP_PROJECT_SOLUTION = "C:\open\fbsource\fbcode\games\services\windows\pcsdk\FBGamingServicesSDK.sln"

function copyright {
    Write-Output "Copyright (c) 2014-present, Facebook, Inc. All rights reserved."
    Write-Output ""
}

copyright

# Check solution project
if ( ![System.IO.File]::Exists($CPP_PROJECT_SOLUTION) ) {
    Write-Output "$CPP_PROJECT_SOLUTION not found."
    exit 1
}

$msbuild_path = (&"${env:ProgramFiles(x86)}\Microsoft Visual Studio\Installer\vswhere.exe" -latest -prerelease -products * -requires Microsoft.Component.MSBuild -find MSBuild\**\Bin\MSBuild.exe)
$add_x86 = "/property:Configuration=Release;Platform=x86"
$add_x64 = "/property:Configuration=Release;Platform=x64"

$rebuild = "/t:Rebuild"

# Call to compile X86
& $msbuild_path $CPP_PROJECT_SOLUTION $add_x86 $rebuild

# Call to compile X64
& $msbuild_path $CPP_PROJECT_SOLUTION $add_x64 $rebuild

Write-Output "Copy Managed DLL"
Copy-Item C:\open\build\FBGamingServicesSDK\LibFBGManaged\bin\x64\Release\LibFBGManaged.dll C:\open\fbsource\xplat\unity-sdk\UnitySDK\Assets\FacebookSDK\Plugins\Windows\LibFBGManaged.dll -Force
Copy-Item C:\open\build\FBGamingServicesSDK\LibFBGManaged\bin\x64\Release\LibFBGManaged.dll C:\open\fbsource\xplat\unity-sdk\Facebook.Unity.Windows\Plugins\LibFBGManaged.dll -Force

Write-Output "Copy x86 DLLs"
Copy-Item C:\open\build\FBGamingServicesSDK\LibFBGPlatform\bin\x86\Release\*.dll C:\open\fbsource\xplat\unity-sdk\UnitySDK\Assets\FacebookSDK\Plugins\Windows\x86\ -Force
Copy-Item C:\open\build\FBGamingServicesSDK\LibFBGPlatform\bin\x86\Release\*.dll C:\open\fbsource\xplat\unity-sdk\Facebook.Unity.Windows\Plugins\x86\ -Force

Write-Output "Copy x64 DLLs"
Copy-Item C:\open\build\FBGamingServicesSDK\LibFBGPlatform\bin\x64\Release\*.dll C:\open\fbsource\xplat\unity-sdk\UnitySDK\Assets\FacebookSDK\Plugins\Windows\x64\ -Force
Copy-Item C:\open\build\FBGamingServicesSDK\LibFBGPlatform\bin\x64\Release\*.dll C:\open\fbsource\xplat\unity-sdk\Facebook.Unity.Windows\Plugins\x64\ -Force

Write-Output "Process completed successfully."
