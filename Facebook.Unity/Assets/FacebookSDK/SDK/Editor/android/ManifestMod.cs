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
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using Facebook.Unity;
    using UnityEditor;
    using UnityEngine;

    public class ManifestMod
    {
        public const string AppLinkActivityName = "com.facebook.unity.FBUnityAppLinkActivity";
        public const string DeepLinkingActivityName = "com.facebook.unity.FBUnityDeepLinkingActivity";
        public const string LoginActivityName = "com.facebook.LoginActivity";
        public const string UnityLoginActivityName = "com.facebook.unity.FBUnityLoginActivity";
        public const string UnityDialogsActivityName = "com.facebook.unity.FBUnityDialogsActivity";
        public const string UnityGameRequestActivityName = "com.facebook.unity.FBUnityGameRequestActivity";
        public const string UnityGameGroupCreateActivityName = "com.facebook.unity.FBUnityCreateGameGroupActivity";
        public const string UnityGameGroupJoinActivityName = "com.facebook.unity.FBUnityJoinGameGroupActivity";
        public const string UnityAppInviteDialogActivityName = "com.facebook.unity.AppInviteDialogActivity";
        public const string ApplicationIdMetaDataName = "com.facebook.sdk.ApplicationId";
        public const string FacebookContentProviderName = "com.facebook.FacebookContentProvider";
        public const string FacebookContentProviderAuthFormat = "com.facebook.app.FacebookContentProvider{0}";
        public const string FacebookActivityName = "com.facebook.FacebookActivity";
        public const string AndroidManifestPath = "Plugins/Android/AndroidManifest.xml";
        public const string AndroidManifestName = "AndroidManifest.xml";
        public const string FacebookDefaultAndroidManifestPath = "FacebookSDK/SDK/Editor/android/DefaultAndroidManifest.xml";

        public static void GenerateManifest()
        {
            var outputFile = Path.Combine(Application.dataPath, ManifestMod.AndroidManifestPath);
            // Create containing directory if it does not exist
            Directory.CreateDirectory(Path.GetDirectoryName(outputFile));

            // only copy over a fresh copy of the AndroidManifest if one does not exist
            if (!File.Exists(outputFile))
            {
                ManifestMod.CreateDefaultAndroidManifest(outputFile);
            }

            UpdateManifest(outputFile);
        }

        public static bool CheckManifest()
        {
            bool result = true;
            var outputFile = Path.Combine(Application.dataPath, ManifestMod.AndroidManifestPath);
            if (!File.Exists(outputFile))
            {
                Debug.LogError("An android manifest must be generated for the Facebook SDK to work.  Go to Facebook->Edit Settings and press \"Regenerate Android Manifest\"");
                return false;
            }

            XmlDocument doc = new XmlDocument();
            doc.Load(outputFile);

            if (doc == null)
            {
                Debug.LogError("Couldn't load " + outputFile);
                return false;
            }

            XmlNode manNode = FindChildNode(doc, "manifest");
            XmlNode dict = FindChildNode(manNode, "application");

            if (dict == null)
            {
                Debug.LogError("Error parsing " + outputFile);
                return false;
            }

            XmlElement loginElement;
            if (!ManifestMod.TryFindElementWithAndroidName(dict, UnityLoginActivityName, out loginElement))
            {
                Debug.LogError(string.Format("{0} is missing from your android manifest.  Go to Facebook->Edit Settings and press \"Regenerate Android Manifest\"", LoginActivityName));
                result = false;
            }

            var deprecatedMainActivityName = "com.facebook.unity.FBUnityPlayerActivity";
            XmlElement deprecatedElement;
            if (ManifestMod.TryFindElementWithAndroidName(dict, deprecatedMainActivityName, out deprecatedElement))
            {
                Debug.LogWarning(string.Format("{0} is deprecated and no longer needed for the Facebook SDK.  Feel free to use your own main activity or use the default \"com.unity3d.player.UnityPlayerNativeActivity\"", deprecatedMainActivityName));
            }

            return result;
        }

        public static void UpdateManifest(string fullPath)
        {
            string appId = FacebookSettings.AppId;

            if (!FacebookSettings.IsValidAppId)
            {
                Debug.LogError("You didn't specify a Facebook app ID.  Please add one using the Facebook menu in the main Unity editor.");
                return;
            }

            XmlDocument doc = new XmlDocument();
            doc.Load(fullPath);

            if (doc == null)
            {
                Debug.LogError("Couldn't load " + fullPath);
                return;
            }

            XmlNode manNode = FindChildNode(doc, "manifest");
            XmlNode dict = FindChildNode(manNode, "application");

            if (dict == null)
            {
                Debug.LogError("Error parsing " + fullPath);
                return;
            }

            string ns = dict.GetNamespaceOfPrefix("android");

            // add the unity login activity
            XmlElement unityLoginElement = CreateUnityOverlayElement(doc, ns, UnityLoginActivityName);
            ManifestMod.SetOrReplaceXmlElement(dict, unityLoginElement);

            // add the unity dialogs activity
            XmlElement unityDialogsElement = CreateUnityOverlayElement(doc, ns, UnityDialogsActivityName);
            ManifestMod.SetOrReplaceXmlElement(dict, unityDialogsElement);

            // add the login activity
            XmlElement loginElement = CreateLoginElement(doc, ns);
            ManifestMod.SetOrReplaceXmlElement(dict, loginElement);

            ManifestMod.AddAppLinkingActivity(doc, dict, ns, FacebookSettings.AppLinkSchemes[FacebookSettings.SelectedAppIndex].Schemes);

            ManifestMod.AddSimpleActivity(doc, dict, ns, DeepLinkingActivityName, true);
            ManifestMod.AddSimpleActivity(doc, dict, ns, UnityGameRequestActivityName);
            ManifestMod.AddSimpleActivity(doc, dict, ns, UnityGameGroupCreateActivityName);
            ManifestMod.AddSimpleActivity(doc, dict, ns, UnityGameGroupJoinActivityName);
            ManifestMod.AddSimpleActivity(doc, dict, ns, UnityAppInviteDialogActivityName);

            // add the app id
            // <meta-data android:name="com.facebook.sdk.ApplicationId" android:value="\ fb<APPID>" />
            XmlElement appIdElement = doc.CreateElement("meta-data");
            appIdElement.SetAttribute("name", ns, ApplicationIdMetaDataName);
            appIdElement.SetAttribute("value", ns, "fb" + appId);
            ManifestMod.SetOrReplaceXmlElement(dict, appIdElement);

            // Add the facebook content provider
            // <provider
            //   android:name="com.facebook.FacebookContentProvider"
            //   android:authorities="com.facebook.app.FacebookContentProvider<APPID>"
            //   android:exported="true" />
            XmlElement contentProviderElement = CreateContentProviderElement(doc, ns, appId);
            ManifestMod.SetOrReplaceXmlElement(dict, contentProviderElement);

            // Add the facebook activity
            // <activity
            //   android:name="com.facebook.FacebookActivity"
            //   android:configChanges="keyboard|keyboardHidden|screenLayout|screenSize|orientation"
            //   android:label="@string/app_name"
            //   android:theme="@android:style/Theme.Translucent.NoTitleBar" />
            XmlElement facebookElement = CreateFacebookElement(doc, ns);
            ManifestMod.SetOrReplaceXmlElement(dict, facebookElement);

            // Save the document formatted
            XmlWriterSettings settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "  ",
                NewLineChars = "\r\n",
                NewLineHandling = NewLineHandling.Replace
            };

            using (XmlWriter xmlWriter = XmlWriter.Create(fullPath, settings))
            {
                doc.Save(xmlWriter);
            }
        }

        private static XmlNode FindChildNode(XmlNode parent, string name)
        {
            XmlNode curr = parent.FirstChild;
            while (curr != null)
            {
                if (curr.Name.Equals(name))
                {
                    return curr;
                }

                curr = curr.NextSibling;
            }

            return null;
        }

        private static void SetOrReplaceXmlElement(
            XmlNode parent,
            XmlElement newElement)
        {
            string attrNameValue = newElement.GetAttribute("name");
            string elementType = newElement.Name;

            XmlElement existingElment;
            if (TryFindElementWithAndroidName(parent, attrNameValue, out existingElment, elementType))
            {
                parent.ReplaceChild(newElement, existingElment);
            }
            else
            {
                parent.AppendChild(newElement);
            }
        }

        private static bool TryFindElementWithAndroidName(
            XmlNode parent,
            string attrNameValue,
            out XmlElement element,
            string elementType = "activity")
        {
            string ns = parent.GetNamespaceOfPrefix("android");
            var curr = parent.FirstChild;
            while (curr != null)
            {
                var currXmlElement = curr as XmlElement;
                if (currXmlElement != null &&
                    currXmlElement.Name == elementType &&
                    currXmlElement.GetAttribute("name", ns) == attrNameValue)
                {
                    element = currXmlElement;
                    return true;
                }

                curr = curr.NextSibling;
            }

            element = null;
            return false;
        }

        private static void AddSimpleActivity(XmlDocument doc, XmlNode xmlNode, string ns, string className, bool export = false)
        {
            XmlElement element = CreateActivityElement(doc, ns, className, export);
            ManifestMod.SetOrReplaceXmlElement(xmlNode, element);
        }

        private static XmlElement CreateLoginElement(XmlDocument doc, string ns)
        {
            // <activity android:name="com.facebook.LoginActivity" android:configChanges="keyboardHidden|orientation" android:theme="@android:style/Theme.Translucent.NoTitleBar.Fullscreen">
            // </activity>
            XmlElement activityElement = ManifestMod.CreateActivityElement(doc, ns, LoginActivityName);
            activityElement.SetAttribute("configChanges", ns, "keyboardHidden|orientation");
            activityElement.SetAttribute("theme", ns, "@android:style/Theme.Translucent.NoTitleBar.Fullscreen");
            return activityElement;
        }

        private static XmlElement CreateUnityOverlayElement(XmlDocument doc, string ns, string activityName)
        {
            // <activity android:name="activityName" android:configChanges="all|of|them" android:theme="@android:style/Theme.Translucent.NoTitleBar.Fullscreen">
            // </activity>
            XmlElement activityElement = ManifestMod.CreateActivityElement(doc, ns, activityName);
            activityElement.SetAttribute("configChanges", ns, "fontScale|keyboard|keyboardHidden|locale|mnc|mcc|navigation|orientation|screenLayout|screenSize|smallestScreenSize|uiMode|touchscreen");
            activityElement.SetAttribute("theme", ns, "@android:style/Theme.Translucent.NoTitleBar.Fullscreen");
            return activityElement;
        }

        private static XmlElement CreateFacebookElement(XmlDocument doc, string ns)
        {
            // <activity android:name="com.facebook.unity.FBUnityGameRequestActivity" android:exported="true">
            // </activity>
            XmlElement activityElement = ManifestMod.CreateActivityElement(doc, ns, FacebookActivityName);
            activityElement.SetAttribute("configChanges", ns, "keyboard|keyboardHidden|screenLayout|screenSize|orientation");
            activityElement.SetAttribute("label", ns, "@string/app_name");
            activityElement.SetAttribute("theme", ns, "@android:style/Theme.Translucent.NoTitleBar");
            return activityElement;
        }

        private static XmlElement CreateContentProviderElement(XmlDocument doc, string ns, string appId)
        {
            XmlElement provierElement = doc.CreateElement("provider");
            provierElement.SetAttribute("name", ns, FacebookContentProviderName);
            string authorities = string.Format(CultureInfo.InvariantCulture, FacebookContentProviderAuthFormat, appId);
            provierElement.SetAttribute("authorities", ns, authorities);
            provierElement.SetAttribute("exported", ns, "true");
            return provierElement;
        }

        private static XmlElement CreateActivityElement(XmlDocument doc, string ns, string activityName, bool exported = false)
        {
            // <activity android:name="activityName" android:exported="true">
            // </activity>
            XmlElement activityElement = doc.CreateElement("activity");
            activityElement.SetAttribute("name", ns, activityName);
            if (exported)
            {
                activityElement.SetAttribute("exported", ns, "true");
            }

            return activityElement;
        }

        private static void AddAppLinkingActivity(XmlDocument doc, XmlNode xmlNode, string ns, List<string> schemes)
        {
            XmlElement element = ManifestMod.CreateActivityElement(doc, ns, AppLinkActivityName, true);
            foreach (var scheme in schemes)
            {
                // We have to create an intent filter for each scheme since an intent filter
                // can have only one data element.
                XmlElement intentFilter = doc.CreateElement("intent-filter");

                var action = doc.CreateElement("action");
                action.SetAttribute("name", ns, "android.intent.action.VIEW");
                intentFilter.AppendChild(action);

                var category = doc.CreateElement("category");
                category.SetAttribute("name", ns, "android.intent.category.DEFAULT");
                intentFilter.AppendChild(category);

                XmlElement dataElement = doc.CreateElement("data");
                dataElement.SetAttribute("scheme", ns, scheme);
                intentFilter.AppendChild(dataElement);
                element.AppendChild(intentFilter);
            }

            ManifestMod.SetOrReplaceXmlElement(xmlNode, element);
        }

        private static void CreateDefaultAndroidManifest(string outputFile)
        {
            var inputFile = Path.Combine(
                EditorApplication.applicationContentsPath,
                "PlaybackEngines/androidplayer/AndroidManifest.xml");
            if (!File.Exists(inputFile))
            {
                // Unity moved this file. Try to get it at its new location
                inputFile = Path.Combine(
                    EditorApplication.applicationContentsPath,
                    "PlaybackEngines/AndroidPlayer/Apk/AndroidManifest.xml");

                if (!File.Exists(inputFile))
                {
                    // On Unity 5.3+ we don't have default manifest so use our own
                    // manifest and warn the user that they may need to modify it manually
                    inputFile = Path.Combine(Application.dataPath, FacebookDefaultAndroidManifestPath);
                    Debug.LogWarning(
                        string.Format(
                            "No existing android manifest found at '{0}'. Creating a default manifest file",
                            outputFile));
                }
            }

            File.Copy(inputFile, outputFile);
        }
    }
}
