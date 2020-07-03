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
    using System.Diagnostics;
    using System.Text;
    using UnityEditor;
    using UnityEngine;

    public class FacebookAndroidUtil
    {
        private static string keyHash;

        public static bool SetupProperly
        {
            get
            {
                return KeyHash != null;
            }
        }

        public static string KeyHash
        {
            get
            {
                if (keyHash == null)
                {
                    if (!HasAndroidSDK)
                    {
                        SetupError = "You don't have the Android SDK setup!  Go to " + (Application.platform == RuntimePlatform.OSXEditor ? "Unity" : "Edit") + "->Preferences... and set your Android SDK Location under External Tools";
                        return null;
                    }

                    if (!HasAndroidKeystoreFile)
                    {
                        SetupError = "Your android keystore file is missing! You can create new one by creating and building empty Android project in Android Studio.";
                        return null;
                    }

                    if (!DoesCommandExist("echo \"xxx\" | openssl base64"))
                    {
                        SetupError = "OpenSSL not found. Make sure that OpenSSL is installed, and that it is in your path.";
                        return null;
                    }

                    if (!DoesCommandExist("keytool"))
                    {
                        SetupError = "Keytool not found. Make sure that Java is installed, and that Java tools are in your path.";
                        return null;
                    }

                    keyHash = GetKeyHash();
                }

                return keyHash;
            }
        }

        public static string SetupError { get; private set; }

        private static string DebugKeyStorePath
        {
            get
            {
                return (Application.platform == RuntimePlatform.WindowsEditor) ?
                    System.Environment.GetEnvironmentVariable("HOMEDRIVE") + System.Environment.GetEnvironmentVariable("HOMEPATH") + @"\.android\debug.keystore" :
                    System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal) + @"/.android/debug.keystore";
            }
        }

        public static bool HasAndroidSDK
        {
            get
            {
                return EditorPrefs.HasKey("AndroidSdkRoot") && System.IO.Directory.Exists(EditorPrefs.GetString("AndroidSdkRoot"));
            }
        }

        private static bool UseDebugKeystore
        {
            get
            {
                // If the keystore name or alias is empty, Unity will use the debug keystore.
                return string.IsNullOrEmpty(PlayerSettings.Android.keystoreName) ||
                       string.IsNullOrEmpty(PlayerSettings.Android.keyaliasName);
            }
        }

        private static string KeystorePath
        {
            get
            {
                return UseDebugKeystore
                    ? DebugKeyStorePath
                    : PlayerSettings.Android.keystoreName;
            }
        }

        private static bool HasAndroidKeystoreFile
        {
            get
            {
                return System.IO.File.Exists(KeystorePath);
            }
        }

        private static string GetKeyHash()
        {
            var alias = UseDebugKeystore ? "androiddebugkey" : PlayerSettings.Android.keyaliasName;
            var keystorePassword = UseDebugKeystore ? "android" : PlayerSettings.Android.keystorePass;
            var aliasPassword = UseDebugKeystore ? "android" : PlayerSettings.Android.keyaliasPass;

            var proc = new Process();
            var arguments = @"""keytool -storepass {0} -keypass {1} -exportcert -alias {2} -keystore {3} | openssl sha1 -binary | openssl base64""";
            if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                proc.StartInfo.FileName = "cmd";
                arguments = @"/C " + arguments;
            }
            else
            {
                proc.StartInfo.FileName = "bash";
                arguments = @"-c " + arguments;
            }

            proc.StartInfo.Arguments = string.Format(arguments, keystorePassword, aliasPassword, alias, KeystorePath);
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.CreateNoWindow = true;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.Start();
            var keyHash = new StringBuilder();
            while (!proc.HasExited)
            {
                keyHash.Append(proc.StandardOutput.ReadToEnd());
            }

            switch (proc.ExitCode)
            {
                case 255:
                    SetupError = "Unknown error while getting Debug Android Key Hash.";
                    return null;
            }

            return keyHash.ToString().TrimEnd('\n');
        }

        private static bool DoesCommandExist(string command)
        {
            var proc = new Process();
            if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                proc.StartInfo.FileName = "cmd";
                proc.StartInfo.Arguments = @"/C" + command;
            }
            else
            {
                proc.StartInfo.FileName = "bash";
                proc.StartInfo.Arguments = @"-c " + command;
            }

            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.CreateNoWindow = true;
            proc.Start();
            proc.WaitForExit();
            if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                return proc.ExitCode == 0;
            }
            else
            {
                return proc.ExitCode != 127;
            }
        }
    }
}
