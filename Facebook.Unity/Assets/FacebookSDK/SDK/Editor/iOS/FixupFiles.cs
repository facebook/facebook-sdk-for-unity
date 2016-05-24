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
    using System.Text.RegularExpressions;
    using UnityEngine;

    public class FixupFiles
    {
        private static string didFinishLaunchingWithOptions =
@"(?x)                                  # Verbose mode
  (didFinishLaunchingWithOptions.+      # Find this function...
    (?:.*\n)+?                          # Match as few lines as possible until...
    \s*return\ )NO(\;\n                 #   return NO;
  \})                                   # }";

        public static void FixSimulator(string path)
        {
            string fullPath = Path.Combine(path, Path.Combine("Libraries", "RegisterMonoModules.cpp"));
            string data = Load(fullPath);

            data = Regex.Replace(data, @"\s+void\s+mono_dl_register_symbol\s+\(const\s+char\*\s+name,\s+void\s+\*addr\);", string.Empty);
            data = Regex.Replace(data, "typedef int gboolean;", "typedef int gboolean;\n\tvoid mono_dl_register_symbol (const char* name, void *addr);");

            // this only need to be done for unity 4, unity 5 declares user functions correctly
            if (GetUnityVersionNumber() < 500)
            {
                data = Regex.Replace(
                    data,
                    @"#endif\s+//\s*!\s*\(\s*TARGET_IPHONE_SIMULATOR\s*\)\s*}\s*void RegisterAllStrippedInternalCalls\s*\(\s*\)",
                    "}\n\nvoid RegisterAllStrippedInternalCalls()");
                data = Regex.Replace(
                    data,
                    @"mono_aot_register_module\(mono_aot_module_mscorlib_info\);",
                    "mono_aot_register_module(mono_aot_module_mscorlib_info);\n#endif // !(TARGET_IPHONE_SIMULATOR)");
            }

            Save(fullPath, data);
        }

        public static void AddVersionDefine(string path)
        {
            int versionNumber = GetUnityVersionNumber();

            string fullPath = Path.Combine(path, Path.Combine("Libraries", "RegisterMonoModules.h"));
            string data = Load(fullPath);

            if (versionNumber >= 430)
            {
                data += "\n#define HAS_UNITY_VERSION_DEF 1\n";
            }
            else
            {
                data += "\n#define UNITY_VERSION ";
                data += versionNumber;
                data += "\n";
            }

            Save(fullPath, data);
        }

        public static void FixColdStart(string path)
        {
            string fullPath = Path.Combine(path, Path.Combine("Classes", "UnityAppController.mm"));
            string data = Load(fullPath);

            data = Regex.Replace(
                data,
                didFinishLaunchingWithOptions,
                "$1YES$2");

            Save(fullPath, data);
        }

        protected static string Load(string fullPath)
        {
            string data;
            FileInfo projectFileInfo = new FileInfo(fullPath);
            StreamReader fs = projectFileInfo.OpenText();
            data = fs.ReadToEnd();
            fs.Close();

            return data;
        }

        protected static void Save(string fullPath, string data)
        {
            System.IO.StreamWriter writer = new System.IO.StreamWriter(fullPath, false);
            writer.Write(data);
            writer.Close();
        }

        private static int GetUnityVersionNumber()
        {
            string version = Application.unityVersion;
            string[] versionComponents = version.Split('.');

            int majorVersion = 0;
            int minorVersion = 0;

            try
            {
                if (versionComponents != null && versionComponents.Length > 0 && versionComponents[0] != null)
                {
                    majorVersion = Convert.ToInt32(versionComponents[0]);
                }

                if (versionComponents != null && versionComponents.Length > 1 && versionComponents[1] != null)
                {
                    minorVersion = Convert.ToInt32(versionComponents[1]);
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError("Error parsing Unity version number: " + e);
            }

            return (majorVersion * 100) + (minorVersion * 10);
        }
    }
}
