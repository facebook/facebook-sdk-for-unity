using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.Collections;
using System.IO;

public static class UWPFacebookPostprocess
{
    private const string FacebookUWPPath = "Assets/UWPFacebook/";

    [PostProcessBuild(100)]
    public static void OnPostProcessBuild(BuildTarget target, string path)
    {
        //We copy all files in the directory "../Plugins/UWP" because the winsdkfb requires the .windmd, .dll and .pri file along with the files inside the "Facebook" folder
        string fromDestination = Application.dataPath + "/Plugins/UWP";
        string toDestination = path + "/" + Application.productName;

        DirectoryCopy(fromDestination, toDestination, true);
    }


    public static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
    {
        // Get the subdirectories for the specified directory.
        DirectoryInfo dir = new DirectoryInfo(sourceDirName);
        DirectoryInfo[] dirs = dir.GetDirectories();

        if (!dir.Exists)
        {
            throw new DirectoryNotFoundException(
                "Source directory does not exist or could not be found: "
                + sourceDirName);
        }

        // If the destination directory doesn't exist, create it. 
        if (!Directory.Exists(destDirName))
        {
            Directory.CreateDirectory(destDirName);
        }

        // Get the files in the directory and copy them to the new location.
        FileInfo[] files = dir.GetFiles();
        foreach (FileInfo file in files)
        {
            string temppath = Path.Combine(destDirName, file.Name);
            file.CopyTo(temppath, true);
        }

        // If copying subdirectories, copy them and their contents to new location. 
        if (copySubDirs)
        {
            foreach (DirectoryInfo subdir in dirs)
            {
                string temppath = Path.Combine(destDirName, subdir.Name);
                DirectoryCopy(subdir.FullName, temppath, copySubDirs);
            }
        }
    }
}
