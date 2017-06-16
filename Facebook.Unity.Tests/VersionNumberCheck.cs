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

namespace Facebook.Unity.Tests
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using NUnit.Framework;

    [TestFixture]
    public class VersionNumberCheck
    {
        private const string UnityPluginSubPath = "UnitySDK/Assets/FacebookSDK/Plugins/";

        private static string unityRepoPath = Directory
            .GetParent(TestContext.CurrentContext.TestDirectory)
            .Parent
            .Parent
            .FullName;

        private static string unityPluginPath = Path.Combine(unityRepoPath, UnityPluginSubPath);
        private static string coreDLLSubPath = Path.Combine(unityPluginPath, "Facebook.Unity.dll");
        private static string gameroomDLLSubPath = Path.Combine(unityPluginPath, "Gameroom/Facebook.Unity.Gameroom.dll");
        private static string editorDLLSubPath = Path.Combine(unityPluginPath, "Editor/Facebook.Unity.Editor.dll");

        [Test]
        public void ValidateDLLVersions()
        {
            VersionNumberCheck.CheckVersionOfDll(VersionNumberCheck.coreDLLSubPath);
            VersionNumberCheck.CheckVersionOfDll(VersionNumberCheck.gameroomDLLSubPath);
            VersionNumberCheck.CheckVersionOfDll(VersionNumberCheck.editorDLLSubPath);
       }

        private static void CheckVersionOfDll(string dllPath)
        {
            FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(dllPath);

            // We only worry about version numbers x.y.z but c# appends a 4th build number for local
            // builds that we don't use.
            Assert.AreEqual(FacebookSdkVersion.Build + ".0", fileVersionInfo.FileVersion, dllPath);
        }
    }
}
