/**
 * Copyright (c) 2014-present, Facebook, Inc. All rights reserved.
 *
 * You are hereby granted a non-exclusive, worldwide, royalty-free license to use,
 * copy, modify, and distribute this software in source code or binary form for use
 * in connection with the web services and APIs provided by Facebook.
 *
 * As with any software that integrates with the Facebook platform, your use of
 * this software is subject to the Facebook Developer Principles and Policies
 * [http://developers.facebook.com/policy/]. This copyright notice shall be
 * included in all copies or substantial portions of the software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
 * FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
 * COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
 * IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
 * CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */

using UnityEngine;
using UnityEditor;
using Facebook.Unity.Settings;
using System.IO;

class UnitySDKWinBuild
{
    static void PerformBuild64()
    {
        // Prepare Facebook Unity SDK Settings file
        var instance = ScriptableObject.CreateInstance<FacebookSettings>();
        string properPath = Path.Combine(Application.dataPath, FacebookSettings.FacebookSettingsPath);
        Directory.CreateDirectory(properPath);
        string fullPath = Path.Combine("Assets", FacebookSettings.FacebookSettingsPath, FacebookSettings.FacebookSettingsAssetName + FacebookSettings.FacebookSettingsAssetExtension);
        AssetDatabase.CreateAsset(instance, fullPath);

        // Set example settings
        FacebookSettings.AppLabels[0]="Friend Smash Unity";
        FacebookSettings.AppIds[0]="523164037733626";
        FacebookSettings.ClientTokens[0] = "5252a149936a7040e498360b574c7f2a";
        FacebookSettings.AppLinkSchemes[0] = new FacebookSettings.UrlSchemes();

        // Save settings
        EditorUtility.SetDirty((FacebookSettings)instance);

        // Prepare building player options
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = new[] { "Assets/FacebookSDK/Examples/Windows/WindowsExample.unity" };
        buildPlayerOptions.locationPathName = "Build64/FriendSmashUnity.exe";
        buildPlayerOptions.target = BuildTarget.StandaloneWindows64;
        buildPlayerOptions.options = BuildOptions.None;

        // Build the example
        BuildPipeline.BuildPlayer(buildPlayerOptions);
    }

    static void PerformBuild86()
    {
        // Prepare Facebook Unity SDK Settings file
        var instance = ScriptableObject.CreateInstance<FacebookSettings>();
        string properPath = Path.Combine(Application.dataPath, FacebookSettings.FacebookSettingsPath);
        Directory.CreateDirectory(properPath);
        string fullPath = Path.Combine("Assets", FacebookSettings.FacebookSettingsPath, FacebookSettings.FacebookSettingsAssetName + FacebookSettings.FacebookSettingsAssetExtension);
        AssetDatabase.CreateAsset(instance, fullPath);

        // Set example settings
        FacebookSettings.AppLabels[0]="Friend Smash Unity";
        FacebookSettings.AppIds[0]="523164037733626";
        FacebookSettings.ClientTokens[0] = "5252a149936a7040e498360b574c7f2a";
        FacebookSettings.AppLinkSchemes[0] = new FacebookSettings.UrlSchemes();

        // Save settings
        EditorUtility.SetDirty((FacebookSettings)instance);

        // Prepare building player options
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = new[] { "Assets/FacebookSDK/Examples/Windows/WindowsExample.unity" };
        buildPlayerOptions.locationPathName = "Build86/FriendSmashUnity.exe";
        buildPlayerOptions.target = BuildTarget.StandaloneWindows;
        buildPlayerOptions.options = BuildOptions.None;

        // Build the example
        BuildPipeline.BuildPlayer(buildPlayerOptions);
    }
}
