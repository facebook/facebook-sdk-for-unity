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

namespace Facebook.Unity
{
    using System.Collections.Generic;
    using System.IO;
    #if UNITY_EDITOR
    using UnityEditor;
    #endif
    using UnityEngine;

    #if UNITY_EDITOR
    [InitializeOnLoad]
    #endif

    /// <summary>
    /// Facebook settings.
    /// </summary>
    public class FacebookSettings : ScriptableObject
    {
        private const string FacebookSettingsAssetName = "FacebookSettings";
        private const string FacebookSettingsPath = "FacebookSDK/SDK/Resources";
        private const string FacebookSettingsAssetExtension = ".asset";

        private static FacebookSettings instance;

        [SerializeField]
        private int selectedAppIndex = 0;
        [SerializeField]
        private List<string> appIds = new List<string> { "0" };
        [SerializeField]
        private List<string> appLabels = new List<string> { "App Name" };
        [SerializeField]
        private bool cookie = true;
        [SerializeField]
        private bool logging = true;
        [SerializeField]
        private bool status = true;
        [SerializeField]
        private bool xfbml = false;
        [SerializeField]
        private bool frictionlessRequests = true;
        [SerializeField]
        private string iosURLSuffix = string.Empty;
        [SerializeField]
        private List<UrlSchemes> appLinkSchemes = new List<UrlSchemes>() { new UrlSchemes() };

        /// <summary>
        /// Gets or sets the index of the selected app.
        /// </summary>
        /// <value>The index of the selected app.</value>
        public static int SelectedAppIndex
        {
            get
            {
                return Instance.selectedAppIndex;
            }

            set
            {
                if (Instance.selectedAppIndex != value)
                {
                    Instance.selectedAppIndex = value;
                    FacebookSettings.DirtyEditor();
                }
            }
        }

        /// <summary>
        /// Gets or sets the app identifiers.
        /// </summary>
        /// <value>The app identifiers.</value>
        public static List<string> AppIds
        {
            get
            {
                return Instance.appIds;
            }

            set
            {
                if (Instance.appIds != value)
                {
                    Instance.appIds = value;
                    DirtyEditor();
                }
            }
        }

        /// <summary>
        /// Gets or sets the app labels.
        /// </summary>
        /// <value>The app labels.</value>
        public static List<string> AppLabels
        {
            get
            {
                return Instance.appLabels;
            }

            set
            {
                if (Instance.appLabels != value)
                {
                    Instance.appLabels = value;
                    DirtyEditor();
                }
            }
        }

        /// <summary>
        /// Gets the app identifier.
        /// </summary>
        /// <value>The app identifier.</value>
        public static string AppId
        {
            get
            {
                return AppIds[SelectedAppIndex];
            }
        }

        /// <summary>
        /// Gets a value indicating whether the app id is valid app identifier.
        /// </summary>
        /// <value><c>true</c> if is valid app identifier; otherwise, <c>false</c>.</value>
        public static bool IsValidAppId
        {
            get
            {
                return FacebookSettings.AppId != null
                    && FacebookSettings.AppId.Length > 0
                    && !FacebookSettings.AppId.Equals("0");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Facebook.Unity.FacebookSettings"/> is cookie.
        /// </summary>
        /// <value><c>true</c> if cookie; otherwise, <c>false</c>.</value>
        public static bool Cookie
        {
            get
            {
                return Instance.cookie;
            }

            set
            {
                if (Instance.cookie != value)
                {
                    Instance.cookie = value;
                    DirtyEditor();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Facebook.Unity.FacebookSettings"/> is logging.
        /// </summary>
        /// <value><c>true</c> if logging; otherwise, <c>false</c>.</value>
        public static bool Logging
        {
            get
            {
                return Instance.logging;
            }

            set
            {
                if (Instance.logging != value)
                {
                    Instance.logging = value;
                    DirtyEditor();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Facebook.Unity.FacebookSettings"/> is status.
        /// </summary>
        /// <value><c>true</c> if status; otherwise, <c>false</c>.</value>
        public static bool Status
        {
            get
            {
                return Instance.status;
            }

            set
            {
                if (Instance.status != value)
                {
                    Instance.status = value;
                    DirtyEditor();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Facebook.Unity.FacebookSettings"/> is xfbml.
        /// </summary>
        /// <value><c>true</c> if xfbml; otherwise, <c>false</c>.</value>
        public static bool Xfbml
        {
            get
            {
                return Instance.xfbml;
            }

            set
            {
                if (Instance.xfbml != value)
                {
                    Instance.xfbml = value;
                    DirtyEditor();
                }
            }
        }

        /// <summary>
        /// Gets or sets the ios URL suffix.
        /// </summary>
        /// <value>The ios URL suffix.</value>
        public static string IosURLSuffix
        {
            get
            {
                return Instance.iosURLSuffix;
            }

            set
            {
                if (Instance.iosURLSuffix != value)
                {
                    Instance.iosURLSuffix = value;
                    DirtyEditor();
                }
            }
        }

        /// <summary>
        /// Gets the channel URL.
        /// </summary>
        /// <value>The channel URL.</value>
        public static string ChannelUrl
        {
            get { return "/channel.html"; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Facebook.Unity.FacebookSettings"/> frictionless requests.
        /// </summary>
        /// <value><c>true</c> if frictionless requests; otherwise, <c>false</c>.</value>
        public static bool FrictionlessRequests
        {
            get
            {
                return Instance.frictionlessRequests;
            }

            set
            {
                if (Instance.frictionlessRequests != value)
                {
                    Instance.frictionlessRequests = value;
                    DirtyEditor();
                }
            }
        }

        /// <summary>
        /// Gets or sets the app link schemes.
        /// </summary>
        /// <value>A list of app link schemese for each app.</value>
        public static List<UrlSchemes> AppLinkSchemes
        {
            get
            {
                return Instance.appLinkSchemes;
            }

            set
            {
                if (Instance.appLinkSchemes != value)
                {
                    Instance.appLinkSchemes = value;
                    DirtyEditor();
                }
            }
        }

#if UNITY_EDITOR

        private string testFacebookId = "";
        private string testAccessToken = "";

        public static string TestFacebookId
        {
            get { return Instance.testFacebookId; }
            set
            {
                if (Instance.testFacebookId != value)
                {
                    Instance.testFacebookId = value;
                    DirtyEditor();
                }
            }
        }

        public static string TestAccessToken
        {
            get { return Instance.testAccessToken; }
            set
            {
                if (Instance.testAccessToken != value)
                {
                    Instance.testAccessToken = value;
                    DirtyEditor();
                }
            }
        }
#endif

        private static FacebookSettings Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = Resources.Load(FacebookSettingsAssetName) as FacebookSettings;
                    if (instance == null)
                    {
                        // If not found, autocreate the asset object.
                        instance = ScriptableObject.CreateInstance<FacebookSettings>();
                        #if UNITY_EDITOR
                        string properPath = Path.Combine(Application.dataPath, FacebookSettingsPath);
                        if (!Directory.Exists(properPath))
                        {
                            Directory.CreateDirectory(properPath);
                        }

                        string fullPath = Path.Combine(
                            Path.Combine("Assets", FacebookSettingsPath),
                            FacebookSettingsAssetName + FacebookSettingsAssetExtension);
                        AssetDatabase.CreateAsset(instance, fullPath);
                        #endif
                    }
                }

                return instance;
            }
        }

        /// <summary>
        /// Settingses the changed.
        /// </summary>
        public static void SettingsChanged()
        {
            FacebookSettings.DirtyEditor();
        }

        #if UNITY_EDITOR
        [MenuItem("Facebook/Edit Settings")]
        public static void Edit()
        {
            Selection.activeObject = Instance;
        }

        [MenuItem("Facebook/Developers Page")]
        public static void OpenAppPage()
        {
            string url = "https://developers.facebook.com/apps/";
            if (FacebookSettings.AppIds[FacebookSettings.SelectedAppIndex] != "0")
                url += FacebookSettings.AppIds[FacebookSettings.SelectedAppIndex];
            Application.OpenURL(url);
        }

        [MenuItem("Facebook/SDK Documentation")]
        public static void OpenDocumentation()
        {
            string url = "https://developers.facebook.com/docs/unity/";
            Application.OpenURL(url);
        }

        [MenuItem("Facebook/SDK Facebook Group")]
        public static void OpenFacebookGroup()
        {
            string url = "https://www.facebook.com/groups/491736600899443/";
            Application.OpenURL(url);
        }

        [MenuItem("Facebook/Report a SDK Bug")]
        public static void ReportABug()
        {
            string url = "https://developers.facebook.com/bugs";
            Application.OpenURL(url);
        }
        #endif

        private static void DirtyEditor()
        {
#if UNITY_EDITOR
            EditorUtility.SetDirty(Instance);
#endif
        }

        /// <summary>
        /// Unity doesn't seralize lists of lists so create a serializable type to wrapp the list for use.
        /// </summary>
        [System.Serializable]
        public class UrlSchemes
        {
            [SerializeField]
            private List<string> list;

            /// <summary>
            /// Initializes a new instance of the <see cref="UrlSchemes"/> class.
            /// </summary>
            /// <param name="schemes">Url schemes.</param>
            public UrlSchemes(List<string> schemes = null)
            {
                this.list = schemes == null ? new List<string>() : schemes;
            }

            /// <summary>
            /// Gets or sets the schemes.
            /// </summary>
            /// <value>The schemes.</value>
            public List<string> Schemes
            {
                get
                {
                    return this.list;
                }

                set
                {
                    this.list = value;
                }
            }
        }
    }
}
