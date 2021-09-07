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

clear

$TARGET_OS="NONE"
$TARGET_VERSION="NONE"
$SELECTED_VERSION=-1

function copyright
{
    echo "Copyright (c) 2014-present, Facebook, Inc. All rights reserved."
    echo ""
}

function help
{
    copyright

    echo "This script adjusts external references to Unity 3D for each of the Facebook SDK for Unity projects."
    echo ""
    echo "Usage:"
    echo "   .\configure.sh"
    echo ""
}

# Cheking working dir
if ( ![System.IO.File]::Exists("$PWD\Facebook.sln") ){
    echo "Please, execute configure.sh script from root project directory"
    exit 1
}

if ($args[0])
{
    echo "Using arguments not allowed."
    echo $args[0]

    exit 1
}
else
{
    $unity_versions = ""
    # Interactive process try to find versions
    help
    
    echo ""
    echo "Searching Unity versions ..."
    $unity_versions = Get-ChildItem -Path "c:\Program Files" -Filter "unity.exe" -Recurse | Select-Object Fullname | Format-List | Out-String

    clear

    echo ""
    echo "Processing Unity folders ..."
    $unity_array = $unity_versions.Split("`n", [StringSplitOptions]::RemoveEmptyEntries)

    $patterns = '^FullName\s:\s(.*)Unity.exe'
    $pattern_version = '^(.*)\\Editor\\(.*)\.(.*)\.(.*)\\Editor'

    $array_unity_path = @()
    $array_unity_version = @()

    foreach ($unity_path in $unity_array){

      if ($unity_path){
        
        $path_data = [regex]::Match($unity_path,$patterns).captures.groups

        if ($path_data)
        {
            #echo $path_data[1].value
            $array_unity_path = $array_unity_path + $path_data[1].value
             
            $version_number = [regex]::Match($path_data,$pattern_version).captures.groups

            if ($version_number)
            {
                #echo $version_number[2].value
                $array_unity_version = $array_unity_version + $version_number[2].value
            }
            else
            {
                #echo "Old Unity"
                $array_unity_version = $array_unity_version + "2017"
            }
        } 

      }

    }

    #Show selection menu
    do{
        clear

        echo ""
        echo "Interactive mode, the following versions of unity have been found on your system."
        echo ""

        $option = 1
        foreach ($unity_path in $array_unity_path){
            $ver = $array_unity_version[$option-1]
            echo $option" - "$ver" [ "$unity_path" ]"
            $option++
        }

        echo ""
        $Prompt = Read-host "Select unity version from the list (Ctrl+C to cancel): " 

        if ( ($Prompt -gt 0 -and $Prompt -lt $option) )
        {
          $SELECTED_VERSION = $Prompt-1
          $TARGET_VERSION = $array_unity_version[$SELECTED_VERSION]
          $TARGET_PATH = $array_unity_path[$SELECTED_VERSION]
          
          echo ""
          echo "You has been selected unity version: "$TARGET_VERSION
          echo ""

          Start-Sleep -Seconds 2.0
        }
        else
        {
          echo ""
          echo "Wrong Selection!"
          echo ""

          $SELECTED_VERSION = -1

          Start-Sleep -Seconds 1.0
        }

    } while($SELECTED_VERSION -eq -1)

    clear

    echo ""
    echo "Preparing Unity references..."
    echo ""

}

if ( $TARGET_VERSION -eq "NONE" )
{
    echo ""
    echo "!Unity family version not selected."
    echo""

    help
    
    exit 1
}

#define full paths for unity version
switch ($TARGET_VERSION)
{
    2017{
            $UNITY_MANAGED_DIR="$TARGET_PATH\Data\Managed\"
            $UNITY_UI_DIR="$TARGET_PATH\Data\UnityExtensions\Unity\GUISystem\"
            $UNITY_ENGINE_DIR="$TARGET_PATH\Data\Managed\"
            $UNITY_EXTENSIONS_DIR="$TARGET_PATH\Data\UnityExtensions\Unity\"
        }
    2018{
            $UNITY_MANAGED_DIR="$TARGET_PATH\Data\Managed\"
            $UNITY_UI_DIR="$TARGET_PATH\Data\UnityExtensions\Unity\GUISystem\"
            $UNITY_ENGINE_DIR="$TARGET_PATH\Data\Managed\UnityEngine\"
            $UNITY_EXTENSIONS_DIR="$TARGET_PATH\Data\UnityExtensions\Unity\"
        }
    2019
        {
            #fix find template version
            $PRJ_TEMPLATE="$TARGET_PATH\Data\Resources\PackageManager\ProjectTemplates\libcache\"
            $tmp=Get-ChildItem -Path $PRJ_TEMPLATE -Directory | Select-Object Name -Unique | Where-Object Name -Match universal | Select-Object Name -First 1
            $TEMPLATE_VERSION = $tmp.Name

            $UNITY_MANAGED_DIR="$TARGET_PATH\Data\Managed\"
            $UNITY_UI_DIR="$TARGET_PATH\Data\Resources\PackageManager\ProjectTemplates\libcache\$TEMPLATE_VERSION\ScriptAssemblies\"
            $UNITY_ENGINE_DIR="$TARGET_PATH\Data\Managed\UnityEngine\"
            $UNITY_EXTENSIONS_DIR="$TARGET_PATH\Data\UnityExtensions\Unity\"
        }
    2020{
            #fix find template version
            $PRJ_TEMPLATE="$TARGET_PATH\Data\Resources\PackageManager\ProjectTemplates\libcache\"
            $tmp=Get-ChildItem -Path $PRJ_TEMPLATE -Directory | Select-Object Name -Unique | Where-Object Name -Match universal | Select-Object Name -First 1
            $TEMPLATE_VERSION = $tmp.Name

            $UNITY_MANAGED_DIR="$TARGET_PATH\Data\Managed\"
            $UNITY_UI_DIR="$TARGET_PATH\Data\esources\PackageManager\ProjectTemplates\libcache\$TEMPLATE_VERSION\ScriptAssemblies\"
            $UNITY_ENGINE_DIR="$TARGET_PATH\Data\Managed\UnityEngine\"
            $UNITY_EXTENSIONS_DIR="NONE"
        }
    2021{
            #fix find template version
            $PRJ_TEMPLATE="$TARGET_PATH\Data\Resources\PackageManager\ProjectTemplates\libcache\"
            $tmp=Get-ChildItem -Path $PRJ_TEMPLATE -Directory | Select-Object Name -Unique | Where-Object Name -Match universal | Select-Object Name -First 1
            $TEMPLATE_VERSION = $tmp.Name

            $UNITY_MANAGED_DIR="$TARGET_PATH\Data\Managed\"
            $UNITY_UI_DIR="$TARGET_PATH\Data\esources\PackageManager\ProjectTemplates\libcache\$TEMPLATE_VERSION\ScriptAssemblies\"
            $UNITY_ENGINE_DIR="$TARGET_PATH\Data\Managed\UnityEngine\"
            $UNITY_EXTENSIONS_DIR="NONE"
        }
    default{
            echo ""
            echo "Unknown version!"
            echo ""

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
foreach ($property_group in $xml_data.Project.PropertyGroup){
  if ($property_group.Attributes["Condition"])
  {
     $version_group = $property_group.Attributes["Condition"].Value

     if ($version_group.Contains($TARGET_VERSION) )
     {
        $property_group.UNITY_MANAGED_DIR = $UNITY_MANAGED_DIR
        $property_group.UNITY_UI_DIR = $UNITY_UI_DIR
        $property_group.UNITY_ENGINE_DIR = $UNITY_ENGINE_DIR
        $property_group.UNITY_EXTENSIONS_DIR = $UNITY_EXTENSIONS_DIR
        $error_version = false
     }
  }
}

if ($error_version)
{
     clear
     echo ""
     echo "Unity version is not in UnityReferences.xml. Please add it or use an exiting Unity version."
     echo ""
     exit 1
}
else
{
    $xml_data.Save($unity_references_file)
}

# Fix projects files
clear
echo ""
echo "Preparing Visual Studio Projects..."
echo ""

$projects_array = @("Facebook.Unity","Facebook.Unity.Settings","Facebook.Unity.Gameroom","Facebook.Unity.Android","Facebook.Unity.Canvas","Facebook.Unity.Editor","Facebook.Unity.Tests","UnitySDK","Facebook.Unity.Windows")
foreach ($PROJECT in $projects_array)
{
   echo ""
   echo "Project: "$PROJECT
   echo ""

   if ($PROJECT -eq "UnitySDK")
   {
        $PROJECT_FILE_NAME="Assembly-CSharp"}
   else
   {
        $PROJECT_FILE_NAME=$PROJECT
   }

   $project_path = "$PWD\"+$PROJECT+"\"+$PROJECT_FILE_NAME+".csproj"
   $project_ref = "$PWD\"+$PROJECT+"\UnityReferences\"+$TARGET_VERSION+".xml"
   
   # Cheking project file
   if ( ![System.IO.File]::Exists($project_path) ){
    clear
    echo ""
    echo "Project doesn't exist. Please review project list."
    echo ""
    echo "Error path: " $project_path
    echo "" 
    exit 1
   }
   else
   {

      # Cheking project ref file
      if ( ![System.IO.File]::Exists($project_ref) ){
        clear
        echo ""
        echo "Project references doesn't exist. Please review project references folder."
        echo ""
        echo "Error path: " $project_ref
        echo "" 
        exit 1
      }
      else
      {
        [xml] $xml_project_ref = Get-Content $project_ref -Raw
        $ref_nodes = $xml_project_ref.ItemGroup.Reference

        [xml] $xml_project_data = Get-Content $project_path -Raw
        foreach ($ref in $ref_nodes){
          foreach ($item in $xml_project_data.Project.ItemGroup){
            if ($item.Reference)
            {
                $item.AppendChild($xml_project_data.ImportNode($ref,$true))
            }
          }
        }
        
        $xml_project_data.Save($project_path)
    
        echo " -> $PROJECT/$PROJECT_FILE_NAME.csproj Updated!"
      }
   }
}

Start-Sleep -Seconds 2.0

#finish script
clear
echo ""
echo "The script finished successfully."
echo ""
exit 0


