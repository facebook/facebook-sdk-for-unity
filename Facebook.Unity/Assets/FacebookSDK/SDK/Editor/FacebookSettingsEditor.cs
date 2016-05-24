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

namespace Facebook.Unity.Editor
{
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEditor.FacebookEditor;
    using UnityEngine;

    [CustomEditor(typeof(FacebookSettings))]
    public class FacebookSettingsEditor : UnityEditor.Editor
    {
        private bool showFacebookInitSettings = false;
        private bool showAndroidUtils = EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android;
        private bool showIOSSettings = EditorUserBuildSettings.activeBuildTarget.ToString() == "iOS";
        private bool showAppLinksSettings = false;
        private bool showAboutSection = false;

        private GUIContent appNameLabel = new GUIContent("App Name [?]:", "For your own use and organization.\n(ex. 'dev', 'qa', 'prod')");
        private GUIContent appIdLabel = new GUIContent("App Id [?]:", "Facebook App Ids can be found at https://developers.facebook.com/apps");

        private GUIContent urlSuffixLabel = new GUIContent("URL Scheme Suffix [?]", "Use this to share Facebook APP ID's across multiple iOS apps.  https://developers.facebook.com/docs/ios/share-appid-across-multiple-apps-ios-sdk/");

        private GUIContent cookieLabel = new GUIContent("Cookie [?]", "Sets a cookie which your server-side code can use to validate a user's Facebook session");
        private GUIContent loggingLabel = new GUIContent("Logging [?]", "(Web Player only) If true, outputs a verbose log to the Javascript console to facilitate debugging.");
        private GUIContent statusLabel = new GUIContent("Status [?]", "If 'true', attempts to initialize the Facebook object with valid session data.");
        private GUIContent xfbmlLabel = new GUIContent("Xfbml [?]", "(Web Player only If true) Facebook will immediately parse any XFBML elements on the Facebook Canvas page hosting the app");
        private GUIContent frictionlessLabel = new GUIContent("Frictionless Requests [?]", "Use frictionless app requests, as described in their own documentation.");

        private GUIContent packageNameLabel = new GUIContent("Package Name [?]", "aka: the bundle identifier");
        private GUIContent classNameLabel = new GUIContent("Class Name [?]", "aka: the activity name");
        private GUIContent debugAndroidKeyLabel = new GUIContent("Debug Android Key Hash [?]", "Copy this key to the Facebook Settings in order to test a Facebook Android app");

        private GUIContent sdkVersion = new GUIContent("SDK Version [?]", "This Unity Facebook SDK version.  If you have problems or compliments please include this so we know exactly what version to look out for.");

        public override void OnInspectorGUI()
        {
            EditorGUILayout.Separator();
            this.AppIdGUI();
            EditorGUILayout.Separator();
            this.FBParamsInitGUI();
            EditorGUILayout.Separator();
            this.AndroidUtilGUI();
            EditorGUILayout.Separator();
            this.IOSUtilGUI();
            EditorGUILayout.Separator();
            this.AppLinksUtilGUI();
            EditorGUILayout.Separator();
            this.AboutGUI();
            EditorGUILayout.Separator();
            this.BuildGUI();
        }

        private void AppIdGUI()
        {
            EditorGUILayout.LabelField("Add the Facebook App Id(s) associated with this game");
            if (FacebookSettings.AppIds.Count == 0 || FacebookSettings.AppId == "0")
            {
                EditorGUILayout.HelpBox("Invalid App Id", MessageType.Error);
            }

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(this.appNameLabel);
            EditorGUILayout.LabelField(this.appIdLabel);
            EditorGUILayout.EndHorizontal();
            for (int i = 0; i < FacebookSettings.AppIds.Count; ++i)
            {
                EditorGUILayout.BeginHorizontal();
                FacebookSettings.AppLabels[i] = EditorGUILayout.TextField(FacebookSettings.AppLabels[i]);
                GUI.changed = false;
                FacebookSettings.AppIds[i] = EditorGUILayout.TextField(FacebookSettings.AppIds[i]);
                if (GUI.changed)
                {
                    FacebookSettings.SettingsChanged();
                    ManifestMod.GenerateManifest();
                }

                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Add Another App Id"))
            {
                FacebookSettings.AppLabels.Add("New App");
                FacebookSettings.AppIds.Add("0");
                FacebookSettings.AppLinkSchemes.Add(new FacebookSettings.UrlSchemes());
                FacebookSettings.SettingsChanged();
            }

            if (FacebookSettings.AppLabels.Count > 1)
            {
                if (GUILayout.Button("Remove Last App Id"))
                {
                    FacebookSettings.AppLabels.Pop();
                    FacebookSettings.AppIds.Pop();
                    FacebookSettings.AppLinkSchemes.Pop();
                    FacebookSettings.SettingsChanged();
                }
            }

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();
            if (FacebookSettings.AppIds.Count > 1)
            {
                EditorGUILayout.HelpBox("2) Select Facebook App Id to be compiled with this game", MessageType.None);
                GUI.changed = false;
                FacebookSettings.SelectedAppIndex = EditorGUILayout.Popup(
                    "Selected App Id",
                    FacebookSettings.SelectedAppIndex,
                    FacebookSettings.AppIds.ToArray());
                if (GUI.changed)
                {
                    ManifestMod.GenerateManifest();
                }

                EditorGUILayout.Space();
            }
            else
            {
                FacebookSettings.SelectedAppIndex = 0;
            }
        }

        private void FBParamsInitGUI()
        {
            this.showFacebookInitSettings = EditorGUILayout.Foldout(this.showFacebookInitSettings, "FB.Init() Parameters");
            if (this.showFacebookInitSettings)
            {
                FacebookSettings.Cookie = EditorGUILayout.Toggle(this.cookieLabel, FacebookSettings.Cookie);
                FacebookSettings.Logging = EditorGUILayout.Toggle(this.loggingLabel, FacebookSettings.Logging);
                FacebookSettings.Status = EditorGUILayout.Toggle(this.statusLabel, FacebookSettings.Status);
                FacebookSettings.Xfbml = EditorGUILayout.Toggle(this.xfbmlLabel, FacebookSettings.Xfbml);
                FacebookSettings.FrictionlessRequests = EditorGUILayout.Toggle(this.frictionlessLabel, FacebookSettings.FrictionlessRequests);
            }

            EditorGUILayout.Space();
        }

        private void IOSUtilGUI()
        {
            this.showIOSSettings = EditorGUILayout.Foldout(this.showIOSSettings, "iOS Build Settings");
            if (this.showIOSSettings)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(this.urlSuffixLabel, GUILayout.Width(135), GUILayout.Height(16));
                FacebookSettings.IosURLSuffix = EditorGUILayout.TextField(FacebookSettings.IosURLSuffix);
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.Space();
        }

        private void AndroidUtilGUI()
        {
            this.showAndroidUtils = EditorGUILayout.Foldout(this.showAndroidUtils, "Android Build Facebook Settings");
            if (this.showAndroidUtils)
            {
                if (!FacebookAndroidUtil.SetupProperly)
                {
                    var msg = "Your Android setup is not right. Check the documentation.";
                    switch (FacebookAndroidUtil.SetupError)
                    {
                        case FacebookAndroidUtil.ErrorNoSDK:
                            msg = "You don't have the Android SDK setup!  Go to " + (Application.platform == RuntimePlatform.OSXEditor ? "Unity" : "Edit") + "->Preferences... and set your Android SDK Location under External Tools";
                            break;
                        case FacebookAndroidUtil.ErrorNoKeystore:
                            msg = "Your android debug keystore file is missing! You can create new one by creating and building empty Android project in Ecplise.";
                            break;
                        case FacebookAndroidUtil.ErrorNoKeytool:
                            msg = "Keytool not found. Make sure that Java is installed, and that Java tools are in your path.";
                            break;
                        case FacebookAndroidUtil.ErrorNoOpenSSL:
                            msg = "OpenSSL not found. Make sure that OpenSSL is installed, and that it is in your path.";
                            break;
                        case FacebookAndroidUtil.ErrorKeytoolError:
                            msg = "Unkown error while getting Debug Android Key Hash.";
                            break;
                    }

                    EditorGUILayout.HelpBox(msg, MessageType.Warning);
                }

                EditorGUILayout.LabelField(
                    "Copy and Paste these into your \"Native Android App\" Settings on developers.facebook.com/apps");
                this.SelectableLabelField(this.packageNameLabel, PlayerSettings.bundleIdentifier);
                this.SelectableLabelField(this.classNameLabel, ManifestMod.DeepLinkingActivityName);
                this.SelectableLabelField(this.debugAndroidKeyLabel, FacebookAndroidUtil.DebugKeyHash);
                if (GUILayout.Button("Regenerate Android Manifest"))
                {
                    ManifestMod.GenerateManifest();
                }
            }

            EditorGUILayout.Space();
        }

        private void AppLinksUtilGUI()
        {
            this.showAppLinksSettings = EditorGUILayout.Foldout(this.showAppLinksSettings, "App Links Settings");
            if (this.showAppLinksSettings)
            {
                for (int i = 0; i < FacebookSettings.AppLinkSchemes.Count; ++i)
                {
                    EditorGUILayout.LabelField(string.Format("App Link Schemes for '{0}'", FacebookSettings.AppLabels[i]));
                    List<string> currentAppLinkSchemes = FacebookSettings.AppLinkSchemes[i].Schemes;
                    for (int j = 0; j < currentAppLinkSchemes.Count; ++j)
                    {
                        GUI.changed = false;
                        string scheme = EditorGUILayout.TextField(currentAppLinkSchemes[j]);
                        if (scheme != currentAppLinkSchemes[j])
                        {
                            currentAppLinkSchemes[j] = scheme;
                            FacebookSettings.SettingsChanged();
                        }

                        if (GUI.changed)
                        {
                            ManifestMod.GenerateManifest();
                        }
                    }

                    EditorGUILayout.BeginHorizontal();
                    if (GUILayout.Button("Add a Scheme"))
                    {
                        FacebookSettings.AppLinkSchemes[i].Schemes.Add(string.Empty);
                        FacebookSettings.SettingsChanged();
                    }

                    if (currentAppLinkSchemes.Count > 0)
                    {
                        if (GUILayout.Button("Remove Last Scheme"))
                        {
                            FacebookSettings.AppLinkSchemes[i].Schemes.Pop();
                        }
                    }

                    EditorGUILayout.EndHorizontal();
                }
            }
        }

        private void AboutGUI()
        {
            this.showAboutSection = EditorGUILayout.Foldout(this.showAboutSection, "About the Facebook SDK");
            if (this.showAboutSection)
            {
                this.SelectableLabelField(this.sdkVersion, FacebookSdkVersion.Build);
                EditorGUILayout.Space();
            }
        }

        private void SelectableLabelField(GUIContent label, string value)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(label, GUILayout.Width(180), GUILayout.Height(16));
            EditorGUILayout.SelectableLabel(value, GUILayout.Height(16));
            EditorGUILayout.EndHorizontal();
        }

        private void BuildGUI()
        {
            if (GUILayout.Button("Build SDK Package"))
            {
                try
                {
                    string outputPath = FacebookBuild.ExportPackage();
                    EditorUtility.DisplayDialog("Finished Exporting unityPackage", "Exported to: " + outputPath, "Okay");
                }
                catch (System.Exception e)
                {
                    EditorUtility.DisplayDialog("Error Exporting unityPackage", e.Message, "Okay");
                }
            }
        }
    }
}
