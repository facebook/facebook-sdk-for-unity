using System.IO;
using UnityEngine;
using UnityEditor;
#if UNITY_IOS
using UnityEditor.Build.Reporting;
using UnityEditor.iOS.Xcode;
#endif
using UnityEditor.Callbacks;


namespace Facebook.Unity.PostProcess
{
    /// <summary>
    /// Automatically disables Bitcode on iOS builds
    /// </summary>
    public static class DisableBitcode
    {
        [PostProcessBuildAttribute(999)]
        public static void OnPostProcessBuild(BuildTarget buildTarget, string pathToBuildProject)
        {
#if UNITY_IOS
            if (buildTarget != BuildTarget.iOS) return;
            string projectPath = pathToBuildProject + "/Unity-iPhone.xcodeproj/project.pbxproj";
            PBXProject pbxProject = new PBXProject();
            pbxProject.ReadFromFile(projectPath);

            //Disabling Bitcode on all targets
            //Main
            string target = pbxProject.GetUnityMainTargetGuid();
            pbxProject.SetBuildProperty(target, "ENABLE_BITCODE", "NO");
            //Unity Tests
            target = pbxProject.TargetGuidByName(PBXProject.GetUnityTestTargetName());
            pbxProject.SetBuildProperty(target, "ENABLE_BITCODE", "NO");
            //Unity Framework
            target = pbxProject.GetUnityFrameworkTargetGuid();
            pbxProject.SetBuildProperty(target, "ENABLE_BITCODE", "NO");

            pbxProject.WriteToFile(projectPath);
#endif
        }
    }
}
