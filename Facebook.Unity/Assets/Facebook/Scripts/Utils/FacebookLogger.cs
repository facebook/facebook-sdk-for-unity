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
    using Facebook.Unity.Mobile.Android;
    using UnityEngine;

    internal static class FacebookLogger
    {
        private const string UnityAndroidTag = "Facebook.Unity.FBDebug";

        static FacebookLogger()
        {
            FacebookLogger.Instance = new CustomLogger();
        }

        internal static IFacebookLogger Instance { private get; set; }

        public static void Log(string msg)
        {
            FacebookLogger.Instance.Log(msg);
        }

        public static void Log(string format, params string[] args)
        {
            FacebookLogger.Log(string.Format(format, args));
        }

        public static void Info(string msg)
        {
            FacebookLogger.Instance.Info(msg);
        }

        public static void Info(string format, params string[] args)
        {
            FacebookLogger.Info(string.Format(format, args));
        }

        public static void Warn(string msg)
        {
            FacebookLogger.Instance.Warn(msg);
        }

        public static void Warn(string format, params string[] args)
        {
            FacebookLogger.Warn(string.Format(format, args));
        }

        public static void Error(string msg)
        {
            FacebookLogger.Instance.Error(msg);
        }

        public static void Error(string format, params string[] args)
        {
            FacebookLogger.Error(string.Format(format, args));
        }

        private class CustomLogger : IFacebookLogger
        {
            private IFacebookLogger logger;

            public CustomLogger()
            {
#if UNITY_EDITOR
                this.logger = new EditorLogger();
#elif UNITY_ANDROID
                this.logger = new AndroidLogger();
#elif UNITY_IOS
                this.logger = new IOSLogger();
#else
                this.logger = new CanvasLogger();
#endif
            }

            public void Log(string msg)
            {
                if (Debug.isDebugBuild)
                {
                    Debug.Log(msg);
                    this.logger.Log(msg);
                }
            }

            public void Info(string msg)
            {
                Debug.Log(msg);
                this.logger.Info(msg);
            }

            public void Warn(string msg)
            {
                Debug.LogWarning(msg);
                this.logger.Warn(msg);
            }

            public void Error(string msg)
            {
                Debug.LogError(msg);
                this.logger.Error(msg);
            }
        }

#if UNITY_EDITOR
        private class EditorLogger : IFacebookLogger
        {
            public void Log(string msg)
            {
            }

            public void Info(string msg)
            {
            }

            public void Warn(string msg)
            {
            }

            public void Error(string msg)
            {
            }
        }

#elif UNITY_ANDROID
        private class AndroidLogger : IFacebookLogger
        {
            public void Log(string msg)
            {
                using (AndroidJavaClass androidLogger = new AndroidJavaClass("android.util.Log"))
                {
                    androidLogger.CallStatic<int>("v", UnityAndroidTag, msg);
                }
            }

            public void Info(string msg)
            {
                using (AndroidJavaClass androidLogger = new AndroidJavaClass("android.util.Log"))
                {
                    androidLogger.CallStatic<int>("i", UnityAndroidTag, msg);
                }
            }

            public void Warn(string msg)
            {
                using (AndroidJavaClass androidLogger = new AndroidJavaClass("android.util.Log"))
                {
                    androidLogger.CallStatic<int>("w", UnityAndroidTag, msg);
                }
            }

            public void Error(string msg)
            {
                using (AndroidJavaClass androidLogger = new AndroidJavaClass("android.util.Log"))
                {
                    androidLogger.CallStatic<int>("e", UnityAndroidTag, msg);
                }
            }
        }
#elif UNITY_IOS
        private class IOSLogger: IFacebookLogger
        {
            public void Log(string msg)
            {
                // TODO
            }

            public void Info(string msg)
            {
                // TODO
            }

            public void Warn(string msg)
            {
                // TODO
            }

            public void Error(string msg)
            {
                // TODO
            }
        }
#else
        private class CanvasLogger : IFacebookLogger
        {
            public void Log(string msg)
            {
                Application.ExternalCall("console.log", msg);
            }

            public void Info(string msg)
            {
                Application.ExternalCall("console.info", msg);
            }

            public void Warn(string msg)
            {
                Application.ExternalCall("console.warn", msg);
            }

            public void Error(string msg)
            {
                Application.ExternalCall("console.error", msg);
            }
        }
#endif
    }
}
