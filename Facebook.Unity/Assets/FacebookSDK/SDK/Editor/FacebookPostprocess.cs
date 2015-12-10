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

namespace UnityEditor.FacebookEditor
{
    using System;
    using System.IO;
    using Facebook.Unity;
    using UnityEditor;
    using UnityEditor.Callbacks;
    using UnityEditor.FacebookEditor;
    using UnityEngine;

    public static class XCodePostProcess
    {
        [PostProcessBuild(100)]
        public static void OnPostProcessBuild(BuildTarget target, string path)
        {
            // If integrating with facebook on any platform, throw a warning if the app id is invalid
            if (!Facebook.Unity.FacebookSettings.IsValidAppId)
            {
                Debug.LogWarning("You didn't specify a Facebook app ID.  Please add one using the Facebook menu in the main Unity editor.");
            }

            // Unity renamed build target from iPhone to iOS in Unity 5, this keeps both versions happy
            if (target.ToString() == "iOS" || target.ToString() == "iPhone")
            {
                UpdatePlist(path);
                FixupFiles.FixSimulator(path);
                FixupFiles.AddVersionDefine(path);
                FixupFiles.FixColdStart(path);
            }

            if (target == BuildTarget.Android)
            {
                // The default Bundle Identifier for Unity does magical things that causes bad stuff to happen
                if (PlayerSettings.bundleIdentifier == "com.Company.ProductName")
                {
                    Debug.LogError("The default Unity Bundle Identifier (com.Company.ProductName) will not work correctly.");
                }

                if (!FacebookAndroidUtil.SetupProperly)
                {
                    Debug.LogError("Your Android setup is not correct. See Settings in Facebook menu.");
                }

                if (!ManifestMod.CheckManifest())
                {
                    // If something is wrong with the Android Manifest, try to regenerate it to fix it for the next build.
                    ManifestMod.GenerateManifest();
                }
            }
        }

        public static void UpdatePlist(string path)
        {
            const string FileName = "Info.plist";
            string appId = FacebookSettings.AppId;
            string fullPath = Path.Combine(path, FileName);

            if (string.IsNullOrEmpty(appId) || appId.Equals("0"))
            {
                Debug.LogError("You didn't specify a Facebook app ID.  Please add one using the Facebook menu in the main Unity editor.");
                return;
            }

            var facebookParser = new PListParser(fullPath);
            facebookParser.UpdateFBSettings(
                appId,
                FacebookSettings.IosURLSuffix,
                FacebookSettings.AppLinkSchemes[FacebookSettings.SelectedAppIndex].Schemes);
            facebookParser.WriteToFile();
        }
    }
}
