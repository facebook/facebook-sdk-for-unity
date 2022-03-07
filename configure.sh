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

TARGET_OS=NONE
TARGET_VERSION=NONE
SELECTED_VERSION=-1

copyright()
{
cat <<EOT
Copyright (c) 2014-present, Facebook, Inc. All rights reserved.

EOT
}

help()
{
copyright
cat <<EOT
This script adjusts external references to Unity 3D for each of the Facebook SDK for Unity projects.

Usage:
./configure.sh
or
./configure.sh [Unity version]
Unity version: Exactly full definition as it appears. Ex: 2017.4.40f1, 2018.4.36f1, 2021.1.17f1

Examples:
./configure.sh                     <- Interactive
./configure.sh 2021.1.17f1         <- Use 2021.1.17f1 version
./configure.sh 2018.4.36f1         <- Use 2018.4.36f1 version

EOT
}

os_not_supported()
{
copyright
cat <<EOT
At this time this script does not support the operating system it is running on.
You will need to set the references to Unity by hand.
We are working to support this system, sorry for the inconvenience.

EOT
}

# Cheking working dir
if [ ! -f Facebook.sln ]; then
    echo "Please, execute configure.sh script from root project directory"
    exit 1
fi

unameOut="$(uname -s)"
case "${unameOut}" in
    Linux*)
        os_not_supported
        exit 1
    ;;
    Darwin*)
        TARGET_OS=MAC
    ;;
    CYGWIN*)
        os_not_supported
        exit 1
    ;;
    MINGW*)
        os_not_supported
        exit 1
    ;;
    *)
        os_not_supported
        exit 1
esac

if [[ -z "$1" ]]
    then
        declare -a unity_versions
        # Interactive process try to find versions
        help
        echo "Interactive mode, the following versions of unity have been found on your system."

        # Old version
        if [ -d "/Applications/Unity/Unity.app" ]; then
            # We assume 2017 version
            unity_versions+=("2017.x.x")
        fi

        # New versions Unity Hub
        files=($(ls /Applications/Unity/Hub/Editor | tr '\n' '\0' | xargs -0 -n 1 basename))
        for file in "${files[@]}"; do
            if [[ $file == *"2017"* || $file == *"2018"* || $file == *"2019"* || $file == *"2020"* || $file == *"2021"* ]]; then
                unity_versions+=("$file")
            fi
        done

        while [[ $SELECTED_VERSION == -1 ]]
        do
            option=1
            for version in "${unity_versions[@]}"; do
                echo "$option- $version"
                ((option = option + 1))
            done
            read -r -p "Select unity version (Ctrl+C to cancel): " SELECTED_VERSION
            if ! [[ $SELECTED_VERSION =~ ^[0-9]+$ ]] ; then
                echo "Wrong input"
                SELECTED_VERSION=-1
            fi
            if [[ "$SELECTED_VERSION" -ge "$option" ]] ; then
                echo "Wrong option"
                SELECTED_VERSION=-1
            fi
        done

        ((SELECTED_VERSION = SELECTED_VERSION - 1))
        echo "You have selected ${unity_versions[SELECTED_VERSION]} version"
        IFS='.' read -ra INPUT_VERSION <<< "${unity_versions[SELECTED_VERSION]}"
        TARGET_VERSION=${INPUT_VERSION[0]}
        FULL_VERSION=${unity_versions[SELECTED_VERSION]}
    else
        # No interactive process
        IFS='.' read -ra INPUT_VERSION <<< "$1"
        TARGET_VERSION=${INPUT_VERSION[0]}
        FULL_VERSION=$1
        case "$TARGET_VERSION" in
            '2017'|'2018'|'2019'|'2020'|'2021')
                echo "Full version name $FULL_VERSION selected."
                echo "Family version $TARGET_VERSION selected."
            ;;
            *)
                echo "!Incorrect family version $TARGET_VERSION or not supported."
                help
                exit 1
            ;;
        esac
fi


if [ $TARGET_OS = "NONE" ];
then
    printf "!Target OS not selected.\n"
    help
    exit 1
fi

if [[ $TARGET_VERSION = "NONE" ]];
then
    printf "!Unity family version not selected.\n"
    help
    exit 1
fi

TEMP_FILE=$(mktemp)
TEMP_FILE2=$(mktemp)

printf "\n"

#define full paths for unity version
case "$TARGET_VERSION" in
    2017)
        UNITY_MANAGED_DIR="/Applications/Unity/Unity.app/Contents/Managed/"
        UNITY_UI_DIR="/Applications/Unity/Unity.app/Contents/UnityExtensions/Unity/GUISystem/"
        UNITY_ENGINE_DIR="/Applications/Unity/Unity.app/Contents/Managed/"
        UNITY_EXTENSIONS_DIR="/Applications/Unity/Unity.app/Contents/UnityExtensions/Unity/"
    ;;
    2018)
        UNITY_MANAGED_DIR="/Applications/Unity/hub/Editor/$FULL_VERSION/Unity.app/Contents/Managed/"
        UNITY_UI_DIR="/Applications/Unity/Hub/Editor/$FULL_VERSION/Unity.app/Contents/UnityExtensions/Unity/GUISystem/"
        UNITY_ENGINE_DIR="/Applications/Unity/Hub/Editor/$FULL_VERSION/Unity.app/Contents/Managed/UnityEngine/"
        UNITY_EXTENSIONS_DIR="/Applications/Unity/Hub/Editor/$FULL_VERSION/Unity.app/Contents/UnityExtensions/Unity/"
    ;;
    2019)
        #fix find template version
        PRJ_TEMPLATE="/Applications/Unity/Hub/Editor/$FULL_VERSION/Unity.app/Contents/Resources/PackageManager/ProjectTemplates/libcache/"
        TEMPLATE_VERSION=$(ls "$PRJ_TEMPLATE" | tr '\n' '\0' | xargs -0 -n 1 basename | grep universal)


        UNITY_MANAGED_DIR="/Applications/Unity/hub/Editor/$FULL_VERSION/Unity.app/Contents/Managed/"
        UNITY_UI_DIR="/Applications/Unity/Hub/Editor/$FULL_VERSION/Unity.app/Contents/Resources/PackageManager/ProjectTemplates/libcache/$TEMPLATE_VERSION/ScriptAssemblies/"
        UNITY_ENGINE_DIR="/Applications/Unity/Hub/Editor/$FULL_VERSION/Unity.app/Contents/Managed/UnityEngine/"
        UNITY_EXTENSIONS_DIR="/Applications/Unity/Hub/Editor/$FULL_VERSION/Unity.app/Contents/UnityExtensions/Unity/"
    ;;
    2020)
        #fix find template version
        PRJ_TEMPLATE="/Applications/Unity/Hub/Editor/$FULL_VERSION/Unity.app/Contents/Resources/PackageManager/ProjectTemplates/libcache/"
        TEMPLATE_VERSION=$(ls "$PRJ_TEMPLATE" | tr '\n' '\0' | xargs -0 -n 1 basename | grep universal)

        UNITY_MANAGED_DIR="/Applications/Unity/hub/Editor/$FULL_VERSION/Unity.app/Contents/Managed/"
        UNITY_UI_DIR="/Applications/Unity/Hub/Editor/$FULL_VERSION/Unity.app/Contents/Resources/PackageManager/ProjectTemplates/libcache/$TEMPLATE_VERSION/ScriptAssemblies/"
        UNITY_ENGINE_DIR="/Applications/Unity/Hub/Editor/$FULL_VERSION/Unity.app/Contents/Managed/UnityEngine/"
        UNITY_EXTENSIONS_DIR="NONE"
    ;;
    2021)
        #fix find template version
        PRJ_TEMPLATE="/Applications/Unity/Hub/Editor/$FULL_VERSION/Unity.app/Contents/Resources/PackageManager/ProjectTemplates/libcache/"
        TEMPLATE_VERSION=$(ls "$PRJ_TEMPLATE" | tr '\n' '\0' | xargs -0 -n 1 basename | grep universal)

        UNITY_MANAGED_DIR="/Applications/Unity/hub/Editor/$FULL_VERSION/Unity.app/Contents/Managed/"
        UNITY_UI_DIR="/Applications/Unity/Hub/Editor/$FULL_VERSION/Unity.app/Contents/Resources/PackageManager/ProjectTemplates/libcache/$TEMPLATE_VERSION/ScriptAssemblies/"
        UNITY_ENGINE_DIR="/Applications/Unity/Hub/Editor/$FULL_VERSION/Unity.app/Contents/Managed/UnityEngine/"
        UNITY_EXTENSIONS_DIR="NONE"
    ;;
    *)
        printf "!Unknown version.\n"
        exit 1
esac

# Set unity version for projects
sed -n "/\<PropertyGroup Condition=\"\'\$(UNITY_VERSION)\' == \'$TARGET_VERSION\'\"\>/,/<\/PropertyGroup>/p" UnityReferences.xml | grep -v PropertyGroup > "$TEMP_FILE"

# Set full paths for unity version
sed "s/\<UNITY_MANAGED_DIR\>.*\<\/UNITY_MANAGED_DIR\>/\<UNITY_MANAGED_DIR\>${UNITY_MANAGED_DIR//\//\\/}<\/UNITY_MANAGED_DIR\>/g" "$TEMP_FILE" > "$TEMP_FILE2"
sed "s/\<UNITY_UI_DIR\>.*\<\/UNITY_UI_DIR\>/\<UNITY_UI_DIR\>${UNITY_UI_DIR//\//\\/}<\/UNITY_UI_DIR\>/g" "$TEMP_FILE2" > "$TEMP_FILE"
sed "s/\<UNITY_ENGINE_DIR\>.*\<\/UNITY_ENGINE_DIR\>/\<UNITY_ENGINE_DIR\>${UNITY_ENGINE_DIR//\//\\/}<\/UNITY_ENGINE_DIR\>/g" "$TEMP_FILE" > "$TEMP_FILE2"
sed "s/\<UNITY_EXTENSIONS_DIR\>.*\<\/UNITY_EXTENSIONS_DIR\>/\<UNITY_EXTENSIONS_DIR\>${UNITY_EXTENSIONS_DIR//\//\\/}<\/UNITY_EXTENSIONS_DIR\>/g" "$TEMP_FILE2" > "$TEMP_FILE"

cat "$TEMP_FILE"

# Checking paths
printf "\n"
declare -a directories=("UNITY_MANAGED_DIR" "UNITY_UI_DIR" "UNITY_ENGINE_DIR" "UNITY_EXTENSIONS_DIR")
for TAG in "${directories[@]}"
do
    DIRECTORY=$(awk "/\<$TAG\>/, /\<\/$TAG\>/" "$TEMP_FILE" | sed -e "s/\<$TAG\>\(.*\)\<\/$TAG\>/\1/" | xargs)
    if [ "$DIRECTORY" != 'NONE' ]; then
        if [ ! -d "$DIRECTORY" ]; then
            echo "Path $DIRECTORY not found."
            exit 1
        else
            echo "Check $DIRECTORY Ok!"
        fi
    fi
done

line_add_from=$(grep -n '<PropertyGroup Condition="'\''$(UNITY_VERSION)'\'' == '\'''"$TARGET_VERSION"''\''">' UnityReferences.xml | awk '{print $1}' FS=":")
line_add_to_array=($(grep -n '<\/PropertyGroup>'  UnityReferences.xml | awk '{print $1}' FS=":"))

#find next close
for close_tag_position in "${line_add_to_array[@]}"; do
    if [[ "$close_tag_position" -ge "$line_add_from" ]] ; then
        line_add_to=$close_tag_position
        break
    fi
done

line_add_from=$((line_add_from + 1))
line_add_to=$((line_add_to - 1))

if [ "$line_add_to" -ge "$line_add_from" ];
then
    sed "$line_add_from,$line_add_to d" UnityReferences.xml > "$TEMP_FILE2"
    mv "$TEMP_FILE2" UnityReferences.xml
fi

awk -v line_add_from=$line_add_from 'FNR==line_add_from{system("cat '"$TEMP_FILE"'")} 1' UnityReferences.xml > "$TEMP_FILE2"
awk '{ gsub(/\xef\xbb\xbf/,""); print }' "$TEMP_FILE2" > "$TEMP_FILE"
mv "$TEMP_FILE" UnityReferences.xml
echo " -> UnityReferences.xml Updated!"

# Set version
sed "s/\<UNITY_VERSION\>.*\<\/UNITY_VERSION\>/\<UNITY_VERSION\>$TARGET_VERSION<\/UNITY_VERSION\>/g" UnityReferences.xml > "$TEMP_FILE"
mv "$TEMP_FILE" UnityReferences.xml

# Fix projects files
printf "\n"
declare -a projects=("Facebook.Unity" "Facebook.Unity.Settings" "Facebook.Unity.Android" "Facebook.Unity.Canvas" "Facebook.Unity.Editor" "Facebook.Unity.Tests" "UnitySDK" "Facebook.Unity.Windows")
for PROJECT in "${projects[@]}"
do
    printf "Project: %s \n" "$PROJECT"
    if [[ $PROJECT = "UnitySDK" ]];
    then
        PROJECT_FILE_NAME="Assembly-CSharp"
    else
        PROJECT_FILE_NAME=$PROJECT
    fi
    line_add_from=$(grep -n '<!-- BEGIN_UNITY_REFERENCES -->' "$PROJECT"/"$PROJECT_FILE_NAME".csproj | awk '{print $1}' FS=":")
    line_add_to=$(grep -n '<!-- END_UNITY_REFERENCES -->' "$PROJECT"/"$PROJECT_FILE_NAME".csproj | awk '{print $1}' FS=":")
    line_add_from=$((line_add_from + 1))
    line_add_to=$((line_add_to - 1))
    if [ "$line_add_to" -ge "$line_add_from" ];
    then
        sed "$line_add_from,$line_add_to d" "$PROJECT"/"$PROJECT_FILE_NAME".csproj > "$TEMP_FILE"
        mv "$TEMP_FILE" "$PROJECT"/"$PROJECT_FILE_NAME".csproj
    fi
    awk -v line_add_from=$line_add_from 'FNR==line_add_from{system("cat '"$PROJECT"'/UnityReferences/'"$TARGET_VERSION"'.xml")} 1' "$PROJECT"/"$PROJECT_FILE_NAME".csproj > "$TEMP_FILE"
    mv "$TEMP_FILE" "$PROJECT"/"$PROJECT_FILE_NAME".csproj
    echo " -> $PROJECT/$PROJECT_FILE_NAME.csproj Updated!"
done

printf "\nThe script finished successfully."
exit 0
