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

$ErrorActionPreference = "SilentlyContinue"
$TARGET_VERSION = "NONE"
$SELECTED_VERSION = -1

function copyright {
    Write-Output "Copyright (c) 2014-present, Facebook, Inc. All rights reserved."
    Write-Output ""
}

function help {
    copyright
    Write-Output "This script adjusts external references to Unity 3D for each of the Facebook SDK for Unity projects."
    Write-Output ""
    Write-Output "Usage:"
    Write-Output "   .\configure.sh"
    Write-Output ""
}

# Cheking working dir
if ( ![System.IO.File]::Exists("$PWD\Facebook.sln") ) {
    Write-Output "Please, execute configure.sh script from root project directory"
    exit 1
}

if ($args[0]) {
    Write-Output "Using arguments not allowed."
    Write-Output $args[0]
    exit 1
}
else {
    $unity_versions = ""
    # Interactive process try to find versions
    help

    Write-Output ""
    Write-Output "Searching Unity versions ..."
    $unity_versions = Get-ChildItem -Path "c:\Program Files" -Filter "unity.exe" -Recurse | Select-Object Fullname | Format-List | Out-String

    Write-Output ""
    Write-Output "Processing Unity folders ..."
    $unity_array = $unity_versions.Split("`n", [StringSplitOptions]::RemoveEmptyEntries)

    $patterns = '^FullName\s:\s(.*)Unity.exe'
    $pattern_version = '^(.*)\\Editor\\(.*)\.(.*)\.(.*)\\Editor'

    $array_unity_path = @()
    $array_unity_version = @()

    foreach ($unity_path in $unity_array) {
        if ($unity_path) {
            $path_data = [regex]::Match($unity_path, $patterns).captures.groups
            if ($path_data) {
                #Write-Output $path_data[1].value
                $array_unity_path = $array_unity_path + $path_data[1].value
                $version_number = [regex]::Match($path_data, $pattern_version).captures.groups
                if ($version_number) {
                    #Write-Output $version_number[2].value
                    $array_unity_version = $array_unity_version + $version_number[2].value
                }
                else {
                    #Write-Output "Old Unity"
                    $array_unity_version = $array_unity_version + "2017"
                }
            }
        }
    }

    #Show selection menu
    do {
        Write-Output ""
        Write-Output "Interactive mode, the following versions of unity have been found on your system."
        Write-Output ""
        $option = 1
        foreach ($unity_path in $array_unity_path) {
            $ver = $array_unity_version[$option - 1]
            Write-Output $option" - "$ver" [ "$unity_path" ]"
            $option++
        }
        Write-Output ""
        $Prompt = Read-host "Select unity version from the list (Ctrl+C to cancel) "
        if ( ($Prompt -gt 0 -and $Prompt -lt $option) ) {
            $SELECTED_VERSION = $Prompt - 1
            $TARGET_VERSION = $array_unity_version[$SELECTED_VERSION]
            $TARGET_PATH = $array_unity_path[$SELECTED_VERSION]
            Write-Output ""
            Write-Output "You has been selected unity version: "$TARGET_VERSION
            Write-Output ""
        }
        else {
            Write-Output ""
            Write-Output "Wrong Selection!"
            Write-Output ""
            $SELECTED_VERSION = -1
        }

    } while ($SELECTED_VERSION -eq -1)

    Write-Output ""
    Write-Output "Preparing Unity references..."
    Write-Output ""

}

if ( $TARGET_VERSION -eq "NONE" ) {
    Write-Output ""
    Write-Output "!Unity family version not selected."
    Write-Output""
    help
    exit 1
}

#define full paths for unity version
switch ($TARGET_VERSION) {
    2017 {
        $UNITY_MANAGED_DIR = "$TARGET_PATH\Data\Managed\"
        $UNITY_UI_DIR = "$TARGET_PATH\Data\UnityExtensions\Unity\GUISystem\"
        $UNITY_ENGINE_DIR = "$TARGET_PATH\Data\Managed\"
        $UNITY_EXTENSIONS_DIR = "$TARGET_PATH\Data\UnityExtensions\Unity\"
    }
    2018 {
        $UNITY_MANAGED_DIR = "$TARGET_PATH\Data\Managed\"
        $UNITY_UI_DIR = "$TARGET_PATH\Data\UnityExtensions\Unity\GUISystem\"
        $UNITY_ENGINE_DIR = "$TARGET_PATH\Data\Managed\UnityEngine\"
        $UNITY_EXTENSIONS_DIR = "$TARGET_PATH\Data\UnityExtensions\Unity\"
    }
    2019 {
        #fix find template version
        $PRJ_TEMPLATE = "$TARGET_PATH\Data\Resources\PackageManager\ProjectTemplates\libcache\"
        $tmp = Get-ChildItem -Path $PRJ_TEMPLATE -Directory | Select-Object Name -Unique | Where-Object Name -Match universal | Select-Object Name -First 1
        $TEMPLATE_VERSION = $tmp.Name

        $UNITY_MANAGED_DIR = "$TARGET_PATH\Data\Managed\"
        $UNITY_UI_DIR = "$TARGET_PATH\Data\Resources\PackageManager\ProjectTemplates\libcache\$TEMPLATE_VERSION\ScriptAssemblies\"
        $UNITY_ENGINE_DIR = "$TARGET_PATH\Data\Managed\UnityEngine\"
        $UNITY_EXTENSIONS_DIR = "$TARGET_PATH\Data\UnityExtensions\Unity\"
    }
    2020 {
        #fix find template version
        $PRJ_TEMPLATE = "$TARGET_PATH\Data\Resources\PackageManager\ProjectTemplates\libcache\"
        $tmp = Get-ChildItem -Path $PRJ_TEMPLATE -Directory | Select-Object Name -Unique | Where-Object Name -Match universal | Select-Object Name -First 1
        $TEMPLATE_VERSION = $tmp.Name

        $UNITY_MANAGED_DIR = "$TARGET_PATH\Data\Managed\"
        $UNITY_UI_DIR = "$TARGET_PATH\Data\esources\PackageManager\ProjectTemplates\libcache\$TEMPLATE_VERSION\ScriptAssemblies\"
        $UNITY_ENGINE_DIR = "$TARGET_PATH\Data\Managed\UnityEngine\"
        $UNITY_EXTENSIONS_DIR = "NONE"
    }
    2021 {
        #fix find template version
        $PRJ_TEMPLATE = "$TARGET_PATH\Data\Resources\PackageManager\ProjectTemplates\libcache\"
        $tmp = Get-ChildItem -Path $PRJ_TEMPLATE -Directory | Select-Object Name -Unique | Where-Object Name -Match universal | Select-Object Name -First 1
        $TEMPLATE_VERSION = $tmp.Name

        $UNITY_MANAGED_DIR = "$TARGET_PATH\Data\Managed\"
        $UNITY_UI_DIR = "$TARGET_PATH\Data\esources\PackageManager\ProjectTemplates\libcache\$TEMPLATE_VERSION\ScriptAssemblies\"
        $UNITY_ENGINE_DIR = "$TARGET_PATH\Data\Managed\UnityEngine\"
        $UNITY_EXTENSIONS_DIR = "NONE"
    }
    default {
        Write-Output ""
        Write-Output "Unknown version!"
        Write-Output ""
        exit 1
    }
}

#Replace Unity path in UnityReferences.xml
$unity_references_file = "$PWD\UnityReferences.xml"
[xml] $xml_data = Get-Content $unity_references_file -Raw

# Set unity version for projects
$xml_data.Project.PropertyGroup[0].UNITY_VERSION = $TARGET_VERSION

# Set full paths for unity version
$error_version = true
foreach ($property_group in $xml_data.Project.PropertyGroup) {
    if ($property_group.Attributes["Condition"]) {
        $version_group = $property_group.Attributes["Condition"].Value
        if ($version_group.Contains($TARGET_VERSION) ) {
            $property_group.UNITY_MANAGED_DIR = $UNITY_MANAGED_DIR
            $property_group.UNITY_UI_DIR = $UNITY_UI_DIR
            $property_group.UNITY_ENGINE_DIR = $UNITY_ENGINE_DIR
            $property_group.UNITY_EXTENSIONS_DIR = $UNITY_EXTENSIONS_DIR
            $error_version = false
        }
    }
}

if ($error_version) {
    Write-Output ""
    Write-Output "Unity version is not in UnityReferences.xml. Please add it or use an exiting Unity version."
    Write-Output ""
    exit 1
}
else {
    $xml_data.Save($unity_references_file)
}

# Fix projects files
Write-Output ""
Write-Output "Preparing Visual Studio Projects..."
Write-Output ""

$projects_array = @("Facebook.Unity", "Facebook.Unity.Settings", "Facebook.Unity.Android", "Facebook.Unity.Canvas", "Facebook.Unity.Editor", "Facebook.Unity.Tests", "UnitySDK", "Facebook.Unity.Windows")
foreach ($PROJECT in $projects_array) {
    Write-Output ""
    Write-Output "Project: $PROJECT"
    Write-Output ""

    if ($PROJECT -eq "UnitySDK") {
        $PROJECT_FILE_NAME = "Assembly-CSharp"
    }
    else {
        $PROJECT_FILE_NAME = $PROJECT
    }

    $project_path = "$PWD\" + $PROJECT + "\" + $PROJECT_FILE_NAME + ".csproj"
    $project_ref = "$PWD\" + $PROJECT + "\UnityReferences\" + $TARGET_VERSION + ".xml"

    # Cheking project file
    if ( ![System.IO.File]::Exists($project_path) ) {
        Write-Output ""
        Write-Output "Project doesn't exist. Please review project list."
        Write-Output ""
        Write-Output "Error path: " $project_path
        Write-Output ""
        exit 1
    }
    else {
        # Cheking project ref file
        if ( ![System.IO.File]::Exists($project_ref) ) {
            Write-Output ""
            Write-Output "Project references doesn't exist. Please review project references folder."
            Write-Output ""
            Write-Output "Error path: " $project_ref
            Write-Output ""
            exit 1
        }
        else {
            $xml_project_ref = Get-Content $project_ref -raw
            $result=Select-String -Path  $project_path -Pattern "<!-- BEGIN_UNITY_REFERENCES -->" | Select-Object LineNumber -First 1
            $line_start=$result[0].LineNumber
            $result=Select-String -Path  $project_path -Pattern "<!-- END_UNITY_REFERENCES -->" | Select-Object LineNumber -First 1
            $line_end=$result[0].LineNumber

            $fileContent = Get-Content $project_path
            if( $line_end -gt ($line_start+1) ){
                $prev=$fileContent[0..($line_start-1)]
                $final=$fileContent[($line_end-1)..($fileContent.length-1)]
                $total=$prev+$final
                Set-Content $project_path $total
            }

            $fileContent = Get-Content $project_path
            $fileContent[$line_start-1] += "`n" + $xml_project_ref.trimend()
            Set-Content $project_path $fileContent
            Write-Output " -> $PROJECT/$PROJECT_FILE_NAME.csproj Updated!"
        }
    }
}

#finish script
Write-Output ""
Write-Output "The script finished successfully."
Write-Output ""
exit 0
