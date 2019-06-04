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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Facebook.Unity.Editor.iOS.Xcode.PBX;

namespace Facebook.Unity.Editor.iOS.Xcode
{
    using PBXBuildFileSection           = KnownSectionBase<PBXBuildFileData>;
    using PBXFileReferenceSection       = KnownSectionBase<PBXFileReferenceData>;
    using PBXGroupSection               = KnownSectionBase<PBXGroupData>;
    using PBXContainerItemProxySection  = KnownSectionBase<PBXContainerItemProxyData>;
    using PBXReferenceProxySection      = KnownSectionBase<PBXReferenceProxyData>;
    using PBXSourcesBuildPhaseSection   = KnownSectionBase<PBXSourcesBuildPhaseData>;
    using PBXFrameworksBuildPhaseSection= KnownSectionBase<PBXFrameworksBuildPhaseData>;
    using PBXResourcesBuildPhaseSection = KnownSectionBase<PBXResourcesBuildPhaseData>;
    using PBXCopyFilesBuildPhaseSection = KnownSectionBase<PBXCopyFilesBuildPhaseData>;
    using PBXShellScriptBuildPhaseSection = KnownSectionBase<PBXShellScriptBuildPhaseData>;
    using PBXVariantGroupSection        = KnownSectionBase<PBXVariantGroupData>;
    using PBXNativeTargetSection        = KnownSectionBase<PBXNativeTargetData>;
    using PBXTargetDependencySection    = KnownSectionBase<PBXTargetDependencyData>;
    using XCBuildConfigurationSection   = KnownSectionBase<XCBuildConfigurationData>;
    using XCConfigurationListSection    = KnownSectionBase<XCConfigurationListData>;
    using UnknownSection                = KnownSectionBase<PBXObjectData>;

    // Determines the tree the given path is relative to
    public enum PBXSourceTree
    {
        Absolute,   // The path is absolute
        Source,     // The path is relative to the source folder
        Group,      // The path is relative to the folder it's in. This enum is used only internally,
        // do not use it as function parameter
        Build,      // The path is relative to the build products folder
        Developer,  // The path is relative to the developer folder
        Sdk         // The path is relative to the sdk folder
    }

    public class PBXProject
    {
        PBXProjectData m_Data = new PBXProjectData();

        // convenience accessors for public members of data. This is temporary; will be fixed by an interface change
        // of PBXProjectData
        internal PBXContainerItemProxySection containerItems { get { return m_Data.containerItems; } }
        internal PBXReferenceProxySection references         { get { return m_Data.references; } }
        internal PBXSourcesBuildPhaseSection sources         { get { return m_Data.sources; } }
        internal PBXFrameworksBuildPhaseSection frameworks   { get { return m_Data.frameworks; } }
        internal PBXResourcesBuildPhaseSection resources     { get { return m_Data.resources; } }
        internal PBXCopyFilesBuildPhaseSection copyFiles     { get { return m_Data.copyFiles; } }
        internal PBXShellScriptBuildPhaseSection shellScripts { get { return m_Data.shellScripts; } }
        internal PBXNativeTargetSection nativeTargets        { get { return m_Data.nativeTargets; } }
        internal PBXTargetDependencySection targetDependencies { get { return m_Data.targetDependencies; } }
        internal PBXVariantGroupSection variantGroups        { get { return m_Data.variantGroups; } }
        internal XCBuildConfigurationSection buildConfigs    { get { return m_Data.buildConfigs; } }
        internal XCConfigurationListSection buildConfigLists { get { return m_Data.buildConfigLists; } }
        internal PBXProjectSection project                   { get { return m_Data.project; } }

        internal PBXBuildFileData BuildFilesGet(string guid) { return m_Data.BuildFilesGet(guid); }
        internal void BuildFilesAdd(string targetGuid, PBXBuildFileData buildFile) { m_Data.BuildFilesAdd(targetGuid, buildFile); }
        internal void BuildFilesRemove(string targetGuid, string fileGuid) { m_Data.BuildFilesRemove(targetGuid, fileGuid); }
        internal PBXBuildFileData BuildFilesGetForSourceFile(string targetGuid, string fileGuid) {
          return m_Data.BuildFilesGetForSourceFile(targetGuid, fileGuid);
        }
        internal IEnumerable<PBXBuildFileData> BuildFilesGetAll() { return m_Data.BuildFilesGetAll(); }
        internal void FileRefsAdd(string realPath, string projectPath, PBXGroupData parent, PBXFileReferenceData fileRef) {
          m_Data.FileRefsAdd(realPath, projectPath, parent, fileRef);
        }
        internal PBXFileReferenceData FileRefsGet(string guid) { return m_Data.FileRefsGet(guid); }
        internal PBXFileReferenceData FileRefsGetByRealPath(string path, PBXSourceTree sourceTree) {
          return m_Data.FileRefsGetByRealPath(path, sourceTree);
        }
        internal PBXFileReferenceData FileRefsGetByProjectPath(string path) { return m_Data.FileRefsGetByProjectPath(path); }
        internal void FileRefsRemove(string guid) { m_Data.FileRefsRemove(guid); }
        internal PBXGroupData GroupsGet(string guid) { return m_Data.GroupsGet(guid); }
        internal PBXGroupData GroupsGetByChild(string childGuid) { return m_Data.GroupsGetByChild(childGuid); }
        internal PBXGroupData GroupsGetMainGroup() { return m_Data.GroupsGetMainGroup(); }
        internal PBXGroupData GroupsGetByProjectPath(string sourceGroup) { return m_Data.GroupsGetByProjectPath(sourceGroup); }
        internal void GroupsAdd(string projectPath, PBXGroupData parent, PBXGroupData gr) { m_Data.GroupsAdd(projectPath, parent, gr); }
        internal void GroupsAddDuplicate(PBXGroupData gr) { m_Data.GroupsAddDuplicate(gr); }
        internal void GroupsRemove(string guid) { m_Data.GroupsRemove(guid); }
        internal FileGUIDListBase BuildSectionAny(PBXNativeTargetData target, string path, bool isFolderRef) {
          return m_Data.BuildSectionAny(target, path, isFolderRef);
        }
        internal FileGUIDListBase BuildSectionAny(string sectionGuid) { return m_Data.BuildSectionAny(sectionGuid); }

        /// <summary>
        /// Returns the path to PBX project in the given Unity build path. This function can only
        /// be used in Unity-generated projects
        /// </summary>
        /// <param name="buildPath">The project build path</param>
        /// <returns>The path to the PBX project file that can later be opened via ReadFromFile function</returns>
        public static string GetPBXProjectPath(string buildPath)
        {
            return PBXPath.Combine(buildPath, "Unity-iPhone.xcodeproj/project.pbxproj");
        }

        /// <summary>
        /// Returns the default main target name in Unity project.
        /// The returned target name can then be used to retrieve the GUID of the target via TargetGuidByName
        /// function. This function can only be used in Unity-generated projects.
        /// </summary>
        /// <returns>The default main target name.</returns>
        public static string GetUnityTargetName()
        {
            return "Unity-iPhone";
        }

        /// <summary>
        /// Returns the default test target name in Unity project.
        /// The returned target name can then be used to retrieve the GUID of the target via TargetGuidByName
        /// function. This function can only be used in Unity-generated projects.
        /// </summary>
        /// <returns>The default test target name.</returns>
        public static string GetUnityTestTargetName()
        {
            return "Unity-iPhone Tests";
        }

        /// <summary>
        /// Returns the GUID of the project. The project GUID identifies a project-wide native target which
        /// is used to set project-wide properties. This GUID can be passed to any functions that accepts
        /// target GUIDs as parameters.
        /// </summary>
        /// <returns>The GUID of the project.</returns>
        public string ProjectGuid()
        {
            return project.project.guid;
        }

        /// <summary>
        /// Returns the GUID of the native target with the given name.
        /// In projects produced by Unity the main target can be retrieved via GetUnityTargetName function,
        /// whereas the test target name can be retrieved by GetUnityTestTargetName function.
        /// </summary>
        /// <returns>The name of the native target.</returns>
        /// <param name="name">The GUID identifying the native target.</param>
        public string TargetGuidByName(string name)
        {
            foreach (var entry in nativeTargets.GetEntries())
                if (entry.Value.name == name)
                    return entry.Key;
            return null;
        }

        /// <summary>
        /// Checks if files with the given extension are known to PBXProject.
        /// </summary>
        /// <returns>Returns <c>true</c> if is the extension is known, <c>false</c> otherwise.</returns>
        /// <param name="ext">The file extension (leading dot is not necessary, but accepted).</param>
        public static bool IsKnownExtension(string ext)
        {
            return FileTypeUtils.IsKnownExtension(ext);
        }

        /// <summary>
        /// Checks if files with the given extension are known to PBXProject.
        /// Returns <c>true</c> if the extension is not known by PBXProject.
        /// </summary>
        /// <returns>Returns <c>true</c> if is the extension is known, <c>false</c> otherwise.</returns>
        /// <param name="ext">The file extension (leading dot is not necessary, but accepted).</param>
        public static bool IsBuildable(string ext)
        {
            return FileTypeUtils.IsBuildableFile(ext);
        }

        // The same file can be referred to by more than one project path.
        private string AddFileImpl(string path, string projectPath, PBXSourceTree tree, bool isFolderReference)
        {
            path = PBXPath.FixSlashes(path);
            projectPath = PBXPath.FixSlashes(projectPath);

            if (!isFolderReference && Path.GetExtension(path) != Path.GetExtension(projectPath))
                throw new Exception("Project and real path extensions do not match");

            string guid = FindFileGuidByProjectPath(projectPath);
            if (guid == null)
                guid = FindFileGuidByRealPath(path);
            if (guid == null)
            {
                PBXFileReferenceData fileRef;
                if (isFolderReference)
                    fileRef = PBXFileReferenceData.CreateFromFolderReference(path, PBXPath.GetFilename(projectPath), tree);
                else
                    fileRef = PBXFileReferenceData.CreateFromFile(path, PBXPath.GetFilename(projectPath), tree);
                PBXGroupData parent = CreateSourceGroup(PBXPath.GetDirectory(projectPath));
                parent.children.AddGUID(fileRef.guid);
                FileRefsAdd(path, projectPath, parent, fileRef);
                guid = fileRef.guid;
            }
            return guid;
        }

        /// <summary>
        /// Adds a new file reference to the list of known files.
        /// The group structure is automatically created to correspond to the project path.
        /// To add the file to the list of files to build, pass the returned value to [[AddFileToBuild]].
        /// </summary>
        /// <returns>The GUID of the added file. It can later be used to add the file for building to targets, etc.</returns>
        /// <param name="path">The physical path to the file on the filesystem.</param>
        /// <param name="projectPath">The project path to the file.</param>
        /// <param name="sourceTree">The source tree the path is relative to. By default it's [[PBXSourceTree.Source]].
        /// The [[PBXSourceTree.Group]] tree is not supported.</param>
        public string AddFile(string path, string projectPath, PBXSourceTree sourceTree = PBXSourceTree.Source)
        {
            if (sourceTree == PBXSourceTree.Group)
                throw new Exception("sourceTree must not be PBXSourceTree.Group");
            return AddFileImpl(path, projectPath, sourceTree, false);
        }

        /// <summary>
        /// Adds a new folder reference to the list of known files.
        /// The group structure is automatically created to correspond to the project path.
        /// To add the folder reference to the list of files to build, pass the returned value to [[AddFileToBuild]].
        /// </summary>
        /// <returns>The GUID of the added folder reference. It can later be used to add the file for building to targets, etc.</returns>
        /// <param name="path">The physical path to the folder on the filesystem.</param>
        /// <param name="projectPath">The project path to the folder.</param>
        /// <param name="sourceTree">The source tree the path is relative to. By default it's [[PBXSourceTree.Source]].
        /// The [[PBXSourceTree.Group]] tree is not supported.</param>
        public string AddFolderReference(string path, string projectPath, PBXSourceTree sourceTree = PBXSourceTree.Source)
        {
            if (sourceTree == PBXSourceTree.Group)
                throw new Exception("sourceTree must not be PBXSourceTree.Group");
            return AddFileImpl(path, projectPath, sourceTree, true);
        }

        private void AddBuildFileImpl(string targetGuid, string fileGuid, bool weak, string compileFlags)
        {
            PBXNativeTargetData target = nativeTargets[targetGuid];
            PBXFileReferenceData fileRef = FileRefsGet(fileGuid);

            string ext = Path.GetExtension(fileRef.path);

            if (FileTypeUtils.IsBuildable(ext, fileRef.isFolderReference) &&
                BuildFilesGetForSourceFile(targetGuid, fileGuid) == null)
            {
                PBXBuildFileData buildFile = PBXBuildFileData.CreateFromFile(fileGuid, weak, compileFlags);
                BuildFilesAdd(targetGuid, buildFile);
                BuildSectionAny(target, ext, fileRef.isFolderReference).files.AddGUID(buildFile.guid);
            }
        }

        /// <summary>
        /// Configures file for building for the given native target.
        /// A projects containing multiple native targets, a single file or folder reference can be
        /// configured to be built in all, some or none of the targets. The file or folder reference is
        /// added to appropriate build section depending on the file extension.
        /// </summary>
        /// <param name="targetGuid">The GUID of the target as returned by [[TargetGuidByName()]].</param>
        /// <param name="fileGuid">The file guid returned by [[AddFile]] or [[AddFolderReference]].</param>
        public void AddFileToBuild(string targetGuid, string fileGuid)
        {
            AddBuildFileImpl(targetGuid, fileGuid, false, null);
        }

        /// <summary>
        /// Configures file for building for the given native target with specific compiler flags.
        /// The function is equivalent to [[AddFileToBuild()]] except that compile flags are specified.
        /// A projects containing multiple native targets, a single file or folder reference can be
        /// configured to be built in all, some or none of the targets. The file or folder reference is
        /// added to appropriate build section depending on the file extension.
        /// </summary>
        /// <param name="targetGuid">The GUID of the target as returned by [[TargetGuidByName()]].</param>
        /// <param name="fileGuid">The file guid returned by [[AddFile]] or [[AddFolderReference]].</param>
        /// <param name="compileFlags">Compile flags to use.</param>
        public void AddFileToBuildWithFlags(string targetGuid, string fileGuid, string compileFlags)
        {
            AddBuildFileImpl(targetGuid, fileGuid, false, compileFlags);
        }

        /// <summary>
        /// Configures file for building for the given native target on specific build section.
        /// The function is equivalent to [[AddFileToBuild()]] except that specific build section is specified.
        /// A projects containing multiple native targets, a single file or folder reference can be
        /// configured to be built in all, some or none of the targets. The file or folder reference is
        /// added to appropriate build section depending on the file extension.
        /// </summary>
        /// <param name="targetGuid">The GUID of the target as returned by [[TargetGuidByName()]].</param>
        /// <param name="sectionGuid">The GUID of the section to add the file to.</param>
        /// <param name="fileGuid">The file guid returned by [[AddFile]] or [[AddFolderReference]].</param>
        public void AddFileToBuildSection(string targetGuid, string sectionGuid, string fileGuid)
        {
            PBXBuildFileData buildFile = PBXBuildFileData.CreateFromFile(fileGuid, false, null);
            BuildFilesAdd(targetGuid, buildFile);
            BuildSectionAny(sectionGuid).files.AddGUID(buildFile.guid);
        }

        /// <summary>
        /// Returns compile flags set for the specific file.
        /// Null is returned if the file has no configured compile flags or the file is not configured for
        /// building on the given target.
        /// </summary>
        /// <returns>The compile flags for the specified file.</returns>
        /// <param name="targetGuid">The GUID of the target as returned by [[TargetGuidByName()]].</param>
        /// <param name="fileGuid">The GUID of the file.</param>
        public List<string> GetCompileFlagsForFile(string targetGuid, string fileGuid)
        {
            var buildFile = BuildFilesGetForSourceFile(targetGuid, fileGuid);
            if (buildFile == null)
                return null;
            if (buildFile.compileFlags == null)
                return new List<string>();
            return new List<string>(buildFile.compileFlags.Split(new char[]{' '}, StringSplitOptions.RemoveEmptyEntries));
        }

        /// <summary>
        /// Sets the compilation flags for the given file in the given target.
        /// </summary>
        /// <param name="targetGuid">The GUID of the target as returned by [[TargetGuidByName()]].</param>
        /// <param name="fileGuid">The GUID of the file.</param>
        /// <param name="compileFlags">The list of compile flags or null if the flags should be unset.</param>
        public void SetCompileFlagsForFile(string targetGuid, string fileGuid, List<string> compileFlags)
        {
            var buildFile = BuildFilesGetForSourceFile(targetGuid, fileGuid);
            if (buildFile == null)
                return;
            if (compileFlags == null)
                buildFile.compileFlags = null;
            else
                buildFile.compileFlags = string.Join(" ", compileFlags.ToArray());
        }

        /// <summary>
        /// Adds an asset tag for the given file.
        /// The asset tags identify resources that will be downloaded via On Demand Resources functionality.
        /// A request for specific tag will initiate download of all files, configured for that tag.
        /// </summary>
        /// <param name="targetGuid">The GUID of the target as returned by [[TargetGuidByName()]].</param>
        /// <param name="fileGuid">The GUID of the file.</param>
        /// <param name="tag">The name of the asset tag.</param>
        public void AddAssetTagForFile(string targetGuid, string fileGuid, string tag)
        {
            var buildFile = BuildFilesGetForSourceFile(targetGuid, fileGuid);
            if (buildFile == null)
                return;
            if (!buildFile.assetTags.Contains(tag))
                buildFile.assetTags.Add(tag);
            if (!project.project.knownAssetTags.Contains(tag))
                project.project.knownAssetTags.Add(tag);
        }

        /// <summary>
        /// Removes an asset tag for the given file.
        /// The function does nothing if the file is not configured for building in the given target or if
        /// the asset tag is not present in the list of asset tags configured for file. If the file was the
        /// last file referring to the given tag across the Xcode project, then the tag is removed from the
        /// list of known tags.
        /// </summary>
        /// <param name="targetGuid">The GUID of the target as returned by [[TargetGuidByName()]].</param>
        /// <param name="fileGuid">The GUID of the file.</param>
        /// <param name="tag">The name of the asset tag.</param>
        public void RemoveAssetTagForFile(string targetGuid, string fileGuid, string tag)
        {
            var buildFile = BuildFilesGetForSourceFile(targetGuid, fileGuid);
            if (buildFile == null)
                return;
            buildFile.assetTags.Remove(tag);
            // remove from known tags if this was the last one
            foreach (var buildFile2 in BuildFilesGetAll())
            {
                if (buildFile2.assetTags.Contains(tag))
                    return;
            }
            project.project.knownAssetTags.Remove(tag);
        }

        /// <summary>
        /// Adds the asset tag to the list of tags to download during initial installation.
        /// The function does nothing if there are no files that use the given asset tag across the project.
        /// </summary>
        /// <param name="targetGuid">The GUID of the target as returned by [[TargetGuidByName()]].</param>
        /// <param name="tag">The name of the asset tag.</param>
        public void AddAssetTagToDefaultInstall(string targetGuid, string tag)
        {
            if (!project.project.knownAssetTags.Contains(tag))
                return;
            AddBuildProperty(targetGuid, "ON_DEMAND_RESOURCES_INITIAL_INSTALL_TAGS", tag);
        }

        /// <summary>
        /// Removes the asset tag from the list of tags to download during initial installation.
        /// The function does nothing if the tag is not already configured for downloading during
        /// initial installation.
        /// </summary>
        /// <param name="targetGuid">The GUID of the target as returned by [[TargetGuidByName()]].</param>
        /// <param name="tag">The name of the asset tag.</param>
        public void RemoveAssetTagFromDefaultInstall(string targetGuid, string tag)
        {
            UpdateBuildProperty(targetGuid, "ON_DEMAND_RESOURCES_INITIAL_INSTALL_TAGS", null, new[]{tag});
        }

        /// <summary>
        /// Removes an asset tag.
        /// Removes the given asset tag from the list of configured asset tags for all files on all targets,
        /// the list of asset tags configured for initial installation and the list of known asset tags in
        /// the Xcode project.
        /// </summary>
        /// <param name="tag">The name of the asset tag.</param>
        public void RemoveAssetTag(string tag)
        {
            foreach (var buildFile in BuildFilesGetAll())
                buildFile.assetTags.Remove(tag);
            foreach (var targetGuid in nativeTargets.GetGuids())
                RemoveAssetTagFromDefaultInstall(targetGuid, tag);
            project.project.knownAssetTags.Remove(tag);
        }

        /// <summary>
        /// Checks if the project contains a file with the given physical path.
        /// The search is performed across all absolute source trees.
        /// </summary>
        /// <returns>Returns <c>true</c> if the project contains the file, <c>false</c> otherwise.</returns>
        /// <param name="path">The physical path of the file.</param>
        public bool ContainsFileByRealPath(string path)
        {
            return FindFileGuidByRealPath(path) != null;
        }

        /// <summary>
        /// Checks if the project contains a file with the given physical path.
        /// </summary>
        /// <returns>Returns <c>true</c> if the project contains the file, <c>false</c> otherwise.</returns>
        /// <param name="path">The physical path of the file.</param>
        /// <param name="sourceTree">The source tree path is relative to. The [[PBXSourceTree.Group]] tree is not supported.</param>
        public bool ContainsFileByRealPath(string path, PBXSourceTree sourceTree)
        {
            if (sourceTree == PBXSourceTree.Group)
                throw new Exception("sourceTree must not be PBXSourceTree.Group");
            return FindFileGuidByRealPath(path, sourceTree) != null;
        }

        /// <summary>
        /// Checks if the project contains a file with the given project path.
        /// </summary>
        /// <returns>Returns <c>true</c> if the project contains the file, <c>false</c> otherwise.</returns>
        /// <param name="path">The project path of the file.</param>
        public bool ContainsFileByProjectPath(string path)
        {
            return FindFileGuidByProjectPath(path) != null;
        }

        /// <summary>
        /// Checks whether the given system framework is a dependency of a target.
        /// The function assumes system frameworks are located in System/Library/Frameworks folder in the SDK source tree.
        /// </summary>
        /// <returns>Returns <c>true</c> if the given framework is a dependency of the given target,
        /// <c>false</c> otherwise.</returns>
        /// <param name="targetGuid">The GUID of the target as returned by [[TargetGuidByName()]].</param>
        /// <param name="framework">The name of the framework. The extension of the filename must be ".framework".</param>
        public bool ContainsFramework(string targetGuid, string framework)
        {
            var fileGuid = FindFileGuidByRealPath("System/Library/Frameworks/" + framework, PBXSourceTree.Sdk);
            if (fileGuid == null)
                return false;

            var buildFile = BuildFilesGetForSourceFile(targetGuid, fileGuid);
            return (buildFile != null);
        }

        /// <summary>
        /// Adds a system framework dependency for the specified target.
        /// The function assumes system frameworks are located in System/Library/Frameworks folder in the SDK source tree.
        /// The framework is added to Frameworks logical folder in the project.
        /// </summary>
        /// <param name="targetGuid">The GUID of the target as returned by [[TargetGuidByName()]].</param>
        /// <param name="framework">The name of the framework. The extension of the filename must be ".framework".</param>
        /// <param name="weak"><c>true</c> if the framework is optional (i.e. weakly linked) required,
        /// <c>false</c> if the framework is required.</param>
        public void AddFrameworkToProject(string targetGuid, string framework, bool weak)
        {
            string fileGuid = AddFile("System/Library/Frameworks/" + framework, "Frameworks/" + framework, PBXSourceTree.Sdk);
            AddBuildFileImpl(targetGuid, fileGuid, weak, null);
        }

        /// <summary>
        /// Removes a system framework dependency for the specified target.
        /// The function assumes system frameworks are located in System/Library/Frameworks folder in the SDK source tree.
        /// </summary>
        /// <param name="targetGuid">The GUID of the target as returned by [[TargetGuidByName()]].</param>
        /// <param name="framework">The name of the framework. The extension of the filename must be ".framework".</param>
        public void RemoveFrameworkFromProject(string targetGuid, string framework)
        {
            var fileGuid = FindFileGuidByRealPath("System/Library/Frameworks/" + framework, PBXSourceTree.Sdk);
            if (fileGuid == null)
                return;

            BuildFilesRemove(targetGuid, fileGuid);
        }

        // Allow user to add a Capability
        public bool AddCapability(
          string targetGuid,
          PBXCapabilityType capability,
          string entitlementsFilePath = null,
          bool addOptionalFramework = false
        )
        {
            // If the capability requires entitlements then you have to provide the name of it or we don't add the capability.
            if (capability.requiresEntitlements && entitlementsFilePath == "")
            {
                throw new Exception(
                  "Couldn't add the Xcode Capability "
                  + capability.id
                  + " to the PBXProject file because this capability requires an entitlement file."
                );
            }
            var p = project.project;

            // If an entitlement with a different name was added for another capability
            // we don't add this capacity.
            if (p.entitlementsFile != null && entitlementsFilePath != null && p.entitlementsFile != entitlementsFilePath)
            {
                if (p.capabilities.Count > 0)
                    throw new WarningException(
                      "Attention, it seems that you have multiple entitlements file. Only one will be added the Project : "
                      + p.entitlementsFile
                    );

                return false;
            }

            // Add the capability only if it doesn't already exist.
            if (p.capabilities.Contains(new PBXCapabilityType.TargetCapabilityPair(targetGuid, capability)))
            {
                throw new WarningException("This capability has already been added. Method ignored");
            }

            p.capabilities.Add(new PBXCapabilityType.TargetCapabilityPair(targetGuid, capability));

            // Add the required framework.
            if (capability.framework != "" && !capability.optionalFramework ||
               (capability.framework != "" && capability.optionalFramework && addOptionalFramework))
            {
                AddFrameworkToProject(targetGuid, capability.framework, false);
            }

            // Finally add the entitlement code signing if it wasn't added before.
            if (entitlementsFilePath != null && p.entitlementsFile == null)
            {
                p.entitlementsFile = entitlementsFilePath;
                AddFileImpl(entitlementsFilePath,  entitlementsFilePath, PBXSourceTree.Source, false);
                SetBuildProperty(targetGuid, "CODE_SIGN_ENTITLEMENTS", PBXPath.FixSlashes(entitlementsFilePath));
            }
            return true;
        }

        // The Xcode project needs a team set to be able to complete code signing or to add some capabilities.
        public void SetTeamId(string targetGuid, string teamId)
        {
            SetBuildProperty(targetGuid, "DEVELOPMENT_TEAM", teamId);
            project.project.teamIDs.Add(targetGuid, teamId);
        }

        /// <summary>
        /// Finds a file with the given physical path in the project, if any.
        /// </summary>
        /// <returns>The GUID of the file if the search succeeded, null otherwise.</returns>
        /// <param name="path">The physical path of the file.</param>
        /// <param name="sourceTree">The source tree path is relative to. The [[PBXSourceTree.Group]] tree is not supported.</param>
        public string FindFileGuidByRealPath(string path, PBXSourceTree sourceTree)
        {
            if (sourceTree == PBXSourceTree.Group)
                throw new Exception("sourceTree must not be PBXSourceTree.Group");
            path = PBXPath.FixSlashes(path);
            var fileRef = FileRefsGetByRealPath(path, sourceTree);
            if (fileRef != null)
                return fileRef.guid;
            return null;
        }

        /// <summary>
        /// Finds a file with the given physical path in the project, if any.
        /// The search is performed across all absolute source trees.
        /// </summary>
        /// <returns>The GUID of the file if the search succeeded, null otherwise.</returns>
        /// <param name="path">The physical path of the file.</param>
        public string FindFileGuidByRealPath(string path)
        {
            path = PBXPath.FixSlashes(path);

            foreach (var tree in FileTypeUtils.AllAbsoluteSourceTrees())
            {
                string res = FindFileGuidByRealPath(path, tree);
                if (res != null)
                    return res;
            }
            return null;
        }

        /// <summary>
        /// Finds a file with the given project path in the project, if any.
        /// </summary>
        /// <returns>The GUID of the file if the search succeeded, null otherwise.</returns>
        /// <param name="path">The project path of the file.</param>
        public string FindFileGuidByProjectPath(string path)
        {
            path = PBXPath.FixSlashes(path);
            var fileRef = FileRefsGetByProjectPath(path);
            if (fileRef != null)
                return fileRef.guid;
            return null;
        }

        /// <summary>
        /// Removes given file from the list of files to build for the given target.
        /// </summary>
        /// <param name="targetGuid">The GUID of the target as returned by [[TargetGuidByName()]].</param>
        /// <param name="fileGuid">The GUID of the file or folder reference.</param>
        public void RemoveFileFromBuild(string targetGuid, string fileGuid)
        {
            var buildFile = BuildFilesGetForSourceFile(targetGuid, fileGuid);
            if (buildFile == null)
                return;
            BuildFilesRemove(targetGuid, fileGuid);

            string buildGuid = buildFile.guid;
            if (buildGuid != null)
            {
                foreach (var section in sources.GetEntries())
                    section.Value.files.RemoveGUID(buildGuid);
                foreach (var section in resources.GetEntries())
                    section.Value.files.RemoveGUID(buildGuid);
                foreach (var section in copyFiles.GetEntries())
                    section.Value.files.RemoveGUID(buildGuid);
                foreach (var section in frameworks.GetEntries())
                    section.Value.files.RemoveGUID(buildGuid);
            }
        }

        /// <summary>
        /// Removes the given file from project.
        /// The file is removed from the list of files to build for each native target and also removed
        /// from the list of known files.
        /// </summary>
        /// <param name="fileGuid">The GUID of the file or folder reference.</param>
        public void RemoveFile(string fileGuid)
        {
            if (fileGuid == null)
                return;

            // remove from parent
            PBXGroupData parent = GroupsGetByChild(fileGuid);
            if (parent != null)
                parent.children.RemoveGUID(fileGuid);
            RemoveGroupIfEmpty(parent);

            // remove actual file
            foreach (var target in nativeTargets.GetEntries())
                RemoveFileFromBuild(target.Value.guid, fileGuid);
            FileRefsRemove(fileGuid);
        }

        void RemoveGroupIfEmpty(PBXGroupData gr)
        {
            if (gr.children.Count == 0 && gr != GroupsGetMainGroup())
            {
                // remove from parent
                PBXGroupData parent = GroupsGetByChild(gr.guid);
                parent.children.RemoveGUID(gr.guid);
                RemoveGroupIfEmpty(parent);

                // remove actual group
                GroupsRemove(gr.guid);
            }
        }

        private void RemoveGroupChildrenRecursive(PBXGroupData parent)
        {
            List<string> children = new List<string>(parent.children);
            parent.children.Clear();
            foreach (string guid in children)
            {
                PBXFileReferenceData file = FileRefsGet(guid);
                if (file != null)
                {
                    foreach (var target in nativeTargets.GetEntries())
                        RemoveFileFromBuild(target.Value.guid, guid);
                    FileRefsRemove(guid);
                    continue;
                }

                PBXGroupData gr = GroupsGet(guid);
                if (gr != null)
                {
                    RemoveGroupChildrenRecursive(gr);
                    GroupsRemove(gr.guid);
                    continue;
                }
            }
        }

        internal void RemoveFilesByProjectPathRecursive(string projectPath)
        {
            projectPath = PBXPath.FixSlashes(projectPath);
            PBXGroupData gr = GroupsGetByProjectPath(projectPath);
            if (gr == null)
                return;
            RemoveGroupChildrenRecursive(gr);
            RemoveGroupIfEmpty(gr);
        }

        // Returns null on error
        internal List<string> GetGroupChildrenFiles(string projectPath)
        {
            projectPath = PBXPath.FixSlashes(projectPath);
            PBXGroupData gr = GroupsGetByProjectPath(projectPath);
            if (gr == null)
                return null;
            var res = new List<string>();
            foreach (var guid in gr.children)
            {
                PBXFileReferenceData fileRef = FileRefsGet(guid);
                if (fileRef != null)
                    res.Add(fileRef.name);
            }
            return res;
        }

        // Returns an empty dictionary if no group or files are found
        internal HashSet<string> GetGroupChildrenFilesRefs(string projectPath)
        {
            projectPath = PBXPath.FixSlashes(projectPath);
            PBXGroupData gr = GroupsGetByProjectPath(projectPath);
            if (gr == null)
                return new HashSet<string>();
            HashSet<string> res = new HashSet<string>();
            foreach (var guid in gr.children)
            {
                PBXFileReferenceData fileRef = FileRefsGet(guid);
                if (fileRef != null)
                    res.Add(fileRef.path);
            }
            return res == null ? new HashSet<string> () : res;
        }

        internal HashSet<string> GetFileRefsByProjectPaths(IEnumerable<string> paths)
        {
            HashSet<string> ret = new HashSet<string>();
            foreach (string path in paths)
            {
                string fixedPath = PBXPath.FixSlashes(path);
                var fileRef = FileRefsGetByProjectPath(fixedPath);
                if (fileRef != null)
                    ret.Add(fileRef.path);
            }
            return ret;
        }

        private PBXGroupData GetPBXGroupChildByName(PBXGroupData group, string name)
        {
            foreach (string guid in group.children)
            {
                var gr = GroupsGet(guid);
                if (gr != null && gr.name == name)
                    return gr;
            }
            return null;
        }

        /// Creates source group identified by sourceGroup, if needed, and returns it.
        /// If sourceGroup is empty or null, root group is returned
        internal PBXGroupData CreateSourceGroup(string sourceGroup)
        {
            sourceGroup = PBXPath.FixSlashes(sourceGroup);

            if (sourceGroup == null || sourceGroup == "")
                return GroupsGetMainGroup();

            PBXGroupData gr = GroupsGetByProjectPath(sourceGroup);
            if (gr != null)
                return gr;

            // the group does not exist -- create new
            gr = GroupsGetMainGroup();

            var elements = PBXPath.Split(sourceGroup);
            string projectPath = null;
            foreach (string pathEl in elements)
            {
                if (projectPath == null)
                    projectPath = pathEl;
                else
                    projectPath += "/" + pathEl;

                PBXGroupData child = GetPBXGroupChildByName(gr, pathEl);
                if (child != null)
                    gr = child;
                else
                {
                    PBXGroupData newGroup = PBXGroupData.Create(pathEl, pathEl, PBXSourceTree.Group);
                    gr.children.AddGUID(newGroup.guid);
                    GroupsAdd(projectPath, gr, newGroup);
                    gr = newGroup;
                }
            }
            return gr;
        }

        /// <summary>
        /// Creates a new native target.
        /// Target-specific build configurations are automatically created for each known build configuration name.
        /// Note, that this is a requirement that follows from the structure of Xcode projects, not an implementation
        /// detail of this function. The function creates a product file reference in the "Products" project folder
        /// which refers to the target artifact that is built via this target.
        /// </summary>
        /// <returns>The GUID of the new target.</returns>
        /// <param name="name">The name of the new target.</param>
        /// <param name="ext">The file extension of the target artifact (leading dot is not necessary, but accepted).</param>
        /// <param name="type">The type of the target. For example:
        /// "com.apple.product-type.app-extension" - App extension,
        /// "com.apple.product-type.application.watchapp2" - WatchKit 2 application</param>
        public string AddTarget(string name, string ext, string type)
        {
            var buildConfigList = XCConfigurationListData.Create();
            buildConfigLists.AddEntry(buildConfigList);

            // create build file reference
            string fullName = name + "." + FileTypeUtils.TrimExtension(ext);
            var productFileRef = AddFile(fullName, "Products/" + fullName, PBXSourceTree.Build);
            var newTarget = PBXNativeTargetData.Create(name, productFileRef, type, buildConfigList.guid);
            nativeTargets.AddEntry(newTarget);
            project.project.targets.Add(newTarget.guid);

            foreach (var buildConfigName in BuildConfigNames())
                AddBuildConfigForTarget(newTarget.guid, buildConfigName);

            return newTarget.guid;
        }

        private IEnumerable<string> GetAllTargetGuids()
        {
            var targets = new List<string>();

            targets.Add(project.project.guid);
            targets.AddRange(nativeTargets.GetGuids());

            return targets;
        }

        /// <summary>
        /// Returns the file reference of the artifact created by building target.
        /// </summary>
        /// <returns>The file reference of the artifact created by building target.</returns>
        /// <param name="targetGuid">The GUID of the target as returned by [[TargetGuidByName()]].</param>
        public string GetTargetProductFileRef(string targetGuid)
        {
            return nativeTargets[targetGuid].productReference;
        }

        /// <summary>
        /// Sets up a dependency between two targets.
        /// </summary>
        /// <param name="targetGuid">The GUID of the target that is depending on the dependency.</param>
        /// <param name="targetDependencyGuid">The GUID of the dependency target</param>
        internal void AddTargetDependency(string targetGuid, string targetDependencyGuid)
        {
            string dependencyName = nativeTargets[targetDependencyGuid].name;
            var containerProxy = PBXContainerItemProxyData.Create(project.project.guid, "1", targetDependencyGuid, dependencyName);
            containerItems.AddEntry(containerProxy);

            var targetDependency = PBXTargetDependencyData.Create(targetDependencyGuid, containerProxy.guid);
            targetDependencies.AddEntry(targetDependency);

            nativeTargets[targetGuid].dependencies.AddGUID(targetDependency.guid);
        }

        // Returns the GUID of the new configuration
        // targetGuid can be either native target or the project target.
        private string AddBuildConfigForTarget(string targetGuid, string name)
        {
            if (BuildConfigByName(targetGuid, name) != null)
            {
                throw new Exception(String.Format("A build configuration by name {0} already exists for target {1}",
                                                  targetGuid, name));
            }
            var buildConfig = XCBuildConfigurationData.Create(name);
            buildConfigs.AddEntry(buildConfig);

            buildConfigLists[GetConfigListForTarget(targetGuid)].buildConfigs.AddGUID(buildConfig.guid);
            return buildConfig.guid;
        }

        private void RemoveBuildConfigForTarget(string targetGuid, string name)
        {
            var buildConfigGuid = BuildConfigByName(targetGuid, name);
            if (buildConfigGuid == null)
                return;
            buildConfigs.RemoveEntry(buildConfigGuid);
            buildConfigLists[GetConfigListForTarget(targetGuid)].buildConfigs.RemoveGUID(buildConfigGuid);
        }

        /// <summary>
        /// Returns the GUID of build configuration with the given name for the specific target.
        /// Null is returned if such configuration does not exist.
        /// </summary>
        /// <returns>The GUID of the build configuration or null if it does not exist.</returns>
        /// <param name="targetGuid">The GUID of the target as returned by [[TargetGuidByName()]].</param>
        /// <param name="name">The name of the build configuration.</param>
        public string BuildConfigByName(string targetGuid, string name)
        {
            foreach (string guid in buildConfigLists[GetConfigListForTarget(targetGuid)].buildConfigs)
            {
                var buildConfig = buildConfigs[guid];
                if (buildConfig != null && buildConfig.name == name)
                    return buildConfig.guid;
            }
            return null;
        }

        /// <summary>
        /// Returns the names of the build configurations available in the project.
        /// The number and names of the build configurations is a project-wide setting. Each target has the
        /// same number of build configurations and the names of these build configurations is the same.
        /// In other words, [[BuildConfigByName()]] will succeed for all targets in the project and all
        /// build configuration names returned by this function.
        /// </summary>
        /// <returns>An array of build config names.</returns>
        public IEnumerable<string> BuildConfigNames()
        {
            var names = new List<string>();
            // We use the project target to fetch the build configs
            foreach (var guid in buildConfigLists[project.project.buildConfigList].buildConfigs)
                names.Add(buildConfigs[guid].name);

            return names;
        }

        /// <summary>
        /// Creates a new set of build configurations for all targets in the project.
        /// The number and names of the build configurations is a project-wide setting. Each target has the
        /// same number of build configurations and the names of these build configurations is the same.
        /// The created configurations are initially empty. Care must be taken to fill them with reasonable
        /// defaults.
        /// The function throws an exception if a build configuration with the given name already exists.
        /// </summary>
        /// <param name="name">The name of the build configuration.</param>
        public void AddBuildConfig(string name)
        {
            foreach (var targetGuid in GetAllTargetGuids())
                AddBuildConfigForTarget(targetGuid, name);
        }

        /// <summary>
        /// Removes all build configurations with the given name from all targets in the project.
        /// The number and names of the build configurations is a project-wide setting. Each target has the
        /// same number of build configurations and the names of these build configurations is the same.
        /// The function does nothing if the build configuration with the specified name does not exist.
        /// </summary>
        /// <param name="name">The name of the build configuration.</param>
        public void RemoveBuildConfig(string name)
        {
            foreach (var targetGuid in GetAllTargetGuids())
                RemoveBuildConfigForTarget(targetGuid, name);
        }

        /// <summary>
        /// Returns the GUID of sources build phase for the given target.
        /// </summary>
        /// <returns>Returns the GUID of the existing phase or null if it does not exist.</returns>
        /// <param name="targetGuid">The GUID of the target as returned by [[TargetGuidByName()]].</param>
        public string GetSourcesBuildPhaseByTarget(string targetGuid)
        {
            var target = nativeTargets[targetGuid];
            foreach (var phaseGuid in target.phases) {
                var phaseAny = BuildSectionAny(phaseGuid);
                if (phaseAny is PBXSourcesBuildPhaseData)
                    return phaseGuid;
            }
            return null;
        }

        /// <summary>
        /// Creates a new sources build phase for given target.
        /// If the target already has sources build phase configured for it, the function returns the
        /// existing phase. The new phase is placed at the end of the list of build phases configured
        /// for the target.
        /// </summary>
        /// <returns>Returns the GUID of the new phase.</returns>
        /// <param name="targetGuid">The GUID of the target as returned by [[TargetGuidByName()]].</param>
        public string AddSourcesBuildPhase(string targetGuid)
        {
            var phaseGuid = GetSourcesBuildPhaseByTarget(targetGuid);
            if (phaseGuid != null)
                return phaseGuid;

            var phase = PBXSourcesBuildPhaseData.Create();
            sources.AddEntry(phase);
            nativeTargets[targetGuid].phases.AddGUID(phase.guid);
            return phase.guid;
        }

        /// <summary>
        /// Returns the GUID of resources build phase for the given target.
        /// </summary>
        /// <returns>Returns the GUID of the existing phase or null if it does not exist.</returns>
        /// <param name="targetGuid">The GUID of the target as returned by [[TargetGuidByName()]].</param>
        public string GetResourcesBuildPhaseByTarget(string targetGuid)
        {
            var target = nativeTargets[targetGuid];
            foreach (var phaseGuid in target.phases) {
                var phaseAny = BuildSectionAny(phaseGuid);
                if (phaseAny is PBXResourcesBuildPhaseData)
                    return phaseGuid;
            }
            return null;
        }

        /// <summary>
        /// Creates a new resources build phase for given target.
        /// If the target already has resources build phase configured for it, the function returns the
        /// existing phase. The new phase is placed at the end of the list of build phases configured
        /// for the target.
        /// </summary>
        /// <returns>Returns the GUID of the new phase.</returns>
        /// <param name="targetGuid">The GUID of the target as returned by [[TargetGuidByName()]].</param>
        public string AddResourcesBuildPhase(string targetGuid)
        {
            var phaseGuid = GetResourcesBuildPhaseByTarget(targetGuid);
            if (phaseGuid != null)
                return phaseGuid;

            var phase = PBXResourcesBuildPhaseData.Create();
            resources.AddEntry(phase);
            nativeTargets[targetGuid].phases.AddGUID(phase.guid);
            return phase.guid;
        }

        /// <summary>
        /// Returns the GUID of frameworks build phase for the given target.
        /// </summary>
        /// <returns>Returns the GUID of the existing phase or null if it does not exist.</returns>
        /// <param name="targetGuid">The GUID of the target as returned by [[TargetGuidByName()]].</param>
        public string GetFrameworksBuildPhaseByTarget(string targetGuid)
        {
            var target = nativeTargets[targetGuid];
            foreach (var phaseGuid in target.phases) {
                var phaseAny = BuildSectionAny(phaseGuid);
                if (phaseAny is PBXFrameworksBuildPhaseData)
                    return phaseGuid;
            }
            return null;
        }

        /// <summary>
        /// Creates a new frameworks build phase for given target.
        /// If the target already has frameworks build phase configured for it, the function returns the
        /// existing phase. The new phase is placed at the end of the list of build phases configured
        /// for the target.
        /// </summary>
        /// <returns>Returns the GUID of the new phase.</returns>
        /// <param name="targetGuid">The GUID of the target as returned by [[TargetGuidByName()]].</param>
        public string AddFrameworksBuildPhase(string targetGuid)
        {
            var phaseGuid = GetFrameworksBuildPhaseByTarget(targetGuid);
            if (phaseGuid != null)
                return phaseGuid;

            var phase = PBXFrameworksBuildPhaseData.Create();
            frameworks.AddEntry(phase);
            nativeTargets[targetGuid].phases.AddGUID(phase.guid);
            return phase.guid;
        }

        /// <summary>
        /// Returns the GUID of matching copy files build phase for the given target.
        /// The parameters of existing copy files build phase are matched to the arguments of this
        /// function and the GUID of the phase is returned only if a matching build phase is found.
        /// </summary>
        /// <returns>Returns the GUID of the matching phase or null if it does not exist.</returns>
        /// <param name="targetGuid">The GUID of the target as returned by [[TargetGuidByName()]].</param>
        /// <param name="name">The name of the phase.</param>
        /// <param name="dstPath">The destination path.</param>
        /// <param name="subfolderSpec">The "subfolder spec". The following usages are known:
        /// "10" for embedding frameworks;
        /// "13" for embedding app extension content;
        /// "16" for embedding watch content</param>
        public string GetCopyFilesBuildPhaseByTarget(string targetGuid, string name, string dstPath, string subfolderSpec)
        {
            var target = nativeTargets[targetGuid];
            foreach (var phaseGuid in target.phases) {
                var phaseAny = BuildSectionAny(phaseGuid);
                if (phaseAny is PBXCopyFilesBuildPhaseData)
                {
                    var copyPhase = (PBXCopyFilesBuildPhaseData) phaseAny;
                    if (copyPhase.name == name && copyPhase.dstPath == dstPath &&
                        copyPhase.dstSubfolderSpec == subfolderSpec)
                    {
                        return phaseGuid;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Creates a new copy files build phase for given target.
        /// If the target already has copy files build phase with the same name, dstPath and subfolderSpec
        /// configured for it, the function returns the existing phase.
        /// The new phase is placed at the end of the list of build phases configured for the target.
        /// </summary>
        /// <returns>Returns the GUID of the new phase.</returns>
        /// <param name="targetGuid">The GUID of the target as returned by [[TargetGuidByName()]].</param>
        /// <param name="name">The name of the phase.</param>
        /// <param name="dstPath">The destination path.</param>
        /// <param name="subfolderSpec">The "subfolder spec". The following usages are known:
        /// "10" for embedding frameworks;
        /// "13" for embedding app extension content;
        /// "16" for embedding watch content</param>
        public string AddCopyFilesBuildPhase(string targetGuid, string name, string dstPath, string subfolderSpec)
        {
            var phaseGuid = GetCopyFilesBuildPhaseByTarget(targetGuid, name, dstPath, subfolderSpec);
            if (phaseGuid != null)
                return phaseGuid;

            var phase = PBXCopyFilesBuildPhaseData.Create(name, dstPath, subfolderSpec);
            copyFiles.AddEntry(phase);
            nativeTargets[targetGuid].phases.AddGUID(phase.guid);
            return phase.guid;
        }

        internal string GetConfigListForTarget(string targetGuid)
        {
            if (targetGuid == project.project.guid)
                return project.project.buildConfigList;
            else
            return nativeTargets[targetGuid].buildConfigList;
        }

        // Sets the baseConfigurationReference key for a XCBuildConfiguration.
        // If the argument is null, the base configuration is removed.
        internal void SetBaseReferenceForConfig(string configGuid, string baseReference)
        {
            buildConfigs[configGuid].baseConfigurationReference = baseReference;
        }

        internal PBXBuildFileData FindFrameworkByFileGuid(PBXCopyFilesBuildPhaseData phase, string fileGuid)
        {
            foreach (string buildFileDataGuid in phase.files)
            {
                var buildFile = BuildFilesGet(buildFileDataGuid);
                if (buildFile.fileRef == fileGuid)
                    return buildFile;
            }
            return null;
        }

        /// <summary>
        /// Adds a value to build property list in all build configurations for the specified target.
        /// Duplicate build properties are ignored. Values for names "LIBRARY_SEARCH_PATHS" and
        /// "FRAMEWORK_SEARCH_PATHS" are quoted if they contain spaces.
        /// </summary>
        /// <param name="targetGuid">The GUID of the target as returned by [[TargetGuidByName()]].</param>
        /// <param name="name">The name of the build property.</param>
        /// <param name="value">The value of the build property.</param>
        public void AddBuildProperty(string targetGuid, string name, string value)
        {
            foreach (string guid in buildConfigLists[GetConfigListForTarget(targetGuid)].buildConfigs)
                AddBuildPropertyForConfig(guid, name, value);
        }

        /// <summary>
        /// Adds a value to build property list in all build configurations for the specified targets.
        /// Duplicate build properties are ignored. Values for names "LIBRARY_SEARCH_PATHS" and
        /// "FRAMEWORK_SEARCH_PATHS" are quoted if they contain spaces.
        /// </summary>
        /// <param name="targetGuids">The GUIDs of the target as returned by [[TargetGuidByName()]].</param>
        /// <param name="name">The name of the build property.</param>
        /// <param name="value">The value of the build property.</param>
        public void AddBuildProperty(IEnumerable<string> targetGuids, string name, string value)
        {
            foreach (string t in targetGuids)
                AddBuildProperty(t, name, value);
        }

        /// <summary>
        /// Adds a value to build property list of the given build configuration
        /// Duplicate build properties are ignored. Values for names "LIBRARY_SEARCH_PATHS" and
        /// "FRAMEWORK_SEARCH_PATHS" are quoted if they contain spaces.
        /// </summary>
        /// <param name="configGuid">The GUID of the build configuration as returned by [[BuildConfigByName()]].</param>
        /// <param name="name">The name of the build property.</param>
        /// <param name="value">The value of the build property.</param>
        public void AddBuildPropertyForConfig(string configGuid, string name, string value)
        {
            buildConfigs[configGuid].AddProperty(name, value);
        }

        /// <summary>
        /// Adds a value to build property list of the given build configurations
        /// Duplicate build properties are ignored. Values for names "LIBRARY_SEARCH_PATHS" and
        /// "FRAMEWORK_SEARCH_PATHS" are quoted if they contain spaces.
        /// </summary>
        /// <param name="configGuids">The GUIDs of the build configurations as returned by [[BuildConfigByName()]].</param>
        /// <param name="name">The name of the build property.</param>
        /// <param name="value">The value of the build property.</param>
        public void AddBuildPropertyForConfig(IEnumerable<string> configGuids, string name, string value)
        {
            foreach (string guid in configGuids)
                AddBuildPropertyForConfig(guid, name, value);
        }

        /// <summary>
        /// Adds a value to build property list in all build configurations for the specified target.
        /// Duplicate build properties are ignored. Values for names "LIBRARY_SEARCH_PATHS" and
        /// "FRAMEWORK_SEARCH_PATHS" are quoted if they contain spaces.
        /// </summary>
        /// <param name="targetGuid">The GUID of the target as returned by [[TargetGuidByName()]].</param>
        /// <param name="name">The name of the build property.</param>
        /// <param name="value">The value of the build property.</param>
        public void SetBuildProperty(string targetGuid, string name, string value)
        {
            foreach (string guid in buildConfigLists[GetConfigListForTarget(targetGuid)].buildConfigs)
                SetBuildPropertyForConfig(guid, name, value);
        }

        /// <summary>
        /// Adds a value to build property list in all build configurations for the specified targets.
        /// Duplicate build properties are ignored. Values for names "LIBRARY_SEARCH_PATHS" and
        /// "FRAMEWORK_SEARCH_PATHS" are quoted if they contain spaces.
        /// </summary>
        /// <param name="targetGuids">The GUIDs of the target as returned by [[TargetGuidByName()]].</param>
        /// <param name="name">The name of the build property.</param>
        /// <param name="value">The value of the build property.</param>
        public void SetBuildProperty(IEnumerable<string> targetGuids, string name, string value)
        {
            foreach (string t in targetGuids)
                SetBuildProperty(t, name, value);
        }
        public void SetBuildPropertyForConfig(string configGuid, string name, string value)
        {
            buildConfigs[configGuid].SetProperty(name, value);
        }
        public void SetBuildPropertyForConfig(IEnumerable<string> configGuids, string name, string value)
        {
            foreach (string guid in configGuids)
                SetBuildPropertyForConfig(guid, name, value);
        }

        internal void RemoveBuildProperty(string targetGuid, string name)
        {
            foreach (string guid in buildConfigLists[GetConfigListForTarget(targetGuid)].buildConfigs)
                RemoveBuildPropertyForConfig(guid, name);
        }
        internal void RemoveBuildProperty(IEnumerable<string> targetGuids, string name)
        {
            foreach (string t in targetGuids)
                RemoveBuildProperty(t, name);
        }
        internal void RemoveBuildPropertyForConfig(string configGuid, string name)
        {
            buildConfigs[configGuid].RemoveProperty(name);
        }
        internal void RemoveBuildPropertyForConfig(IEnumerable<string> configGuids, string name)
        {
            foreach (string guid in configGuids)
                RemoveBuildPropertyForConfig(guid, name);
        }

        internal void RemoveBuildPropertyValueList(string targetGuid, string name, IEnumerable<string> valueList)
        {
            foreach (string guid in buildConfigLists[GetConfigListForTarget(targetGuid)].buildConfigs)
                RemoveBuildPropertyValueListForConfig(guid, name, valueList);
        }
        internal void RemoveBuildPropertyValueList(IEnumerable<string> targetGuids, string name, IEnumerable<string> valueList)
        {
            foreach (string t in targetGuids)
                RemoveBuildPropertyValueList(t, name, valueList);
        }
        internal void RemoveBuildPropertyValueListForConfig(string configGuid, string name, IEnumerable<string> valueList)
        {
            buildConfigs[configGuid].RemovePropertyValueList(name, valueList);
        }
        internal void RemoveBuildPropertyValueListForConfig(IEnumerable<string> configGuids, string name, IEnumerable<string> valueList)
        {
            foreach (string guid in configGuids)
                RemoveBuildPropertyValueListForConfig(guid, name, valueList);
        }

        /// Interprets the value of the given property as a set of space-delimited strings, then
        /// removes strings equal to items to removeValues and adds strings in addValues.
        public void UpdateBuildProperty(string targetGuid, string name,
                                        IEnumerable<string> addValues, IEnumerable<string> removeValues)
        {
            foreach (string guid in buildConfigLists[GetConfigListForTarget(targetGuid)].buildConfigs)
                UpdateBuildPropertyForConfig(guid, name, addValues, removeValues);
        }
        public void UpdateBuildProperty(IEnumerable<string> targetGuids, string name,
                                        IEnumerable<string> addValues, IEnumerable<string> removeValues)
        {
            foreach (string t in targetGuids)
                UpdateBuildProperty(t, name, addValues, removeValues);
        }
        public void UpdateBuildPropertyForConfig(string configGuid, string name,
                                                 IEnumerable<string> addValues, IEnumerable<string> removeValues)
        {
            var config = buildConfigs[configGuid];
            if (config != null)
            {
                if (removeValues != null)
                    foreach (var v in removeValues)
                        config.RemovePropertyValue(name, v);
                if (addValues != null)
                    foreach (var v in addValues)
                        config.AddProperty(name, v);
            }
        }
        public void UpdateBuildPropertyForConfig(IEnumerable<string> configGuids, string name,
                                                 IEnumerable<string> addValues, IEnumerable<string> removeValues)
        {
            foreach (string guid in configGuids)
                UpdateBuildProperty(guid, name, addValues, removeValues);
        }

        internal string ShellScriptByName(string targetGuid, string name)
        {
            foreach (var phase in nativeTargets[targetGuid].phases)
            {
                var script = shellScripts[phase];
                if (script != null && script.name == name)
                    return script.guid;
            }
            return null;
        }

        internal void AppendShellScriptBuildPhase(string targetGuid, string name, string shellPath, string shellScript)
        {
            PBXShellScriptBuildPhaseData shellScriptPhase = PBXShellScriptBuildPhaseData.Create(name, shellPath, shellScript);

            shellScripts.AddEntry(shellScriptPhase);
            nativeTargets[targetGuid].phases.AddGUID(shellScriptPhase.guid);
        }

        internal void AppendShellScriptBuildPhase(IEnumerable<string> targetGuids, string name, string shellPath, string shellScript)
        {
            PBXShellScriptBuildPhaseData shellScriptPhase = PBXShellScriptBuildPhaseData.Create(name, shellPath, shellScript);

            shellScripts.AddEntry(shellScriptPhase);
            foreach (string guid in targetGuids)
            {
                nativeTargets[guid].phases.AddGUID(shellScriptPhase.guid);
            }
        }

        public void ReadFromFile(string path)
        {
            ReadFromString(File.ReadAllText(path));
        }

        public void ReadFromString(string src)
        {
            TextReader sr = new StringReader(src);
            ReadFromStream(sr);
        }

        public void ReadFromStream(TextReader sr)
        {
            m_Data.ReadFromStream(sr);
        }

        public void WriteToFile(string path)
        {
            File.WriteAllText(path, WriteToString());
        }

        public void WriteToStream(TextWriter sw)
        {
            sw.Write(WriteToString());
        }

        public string WriteToString()
        {
            return m_Data.WriteToString();
        }

        internal PBXProjectObjectData GetProjectInternal()
        {
            return project.project;
        }

        /*
         * Allows the setting of target attributes in the project section such as Provisioning Style and Team ID for each target
         *
         * The Target Attributes are structured like so:
         * attributes = {
         *      TargetAttributes = {
         *          1D6058900D05DD3D006BFB54 = {
         *              DevelopmentTeam = Z6SFPV59E3;
         *              ProvisioningStyle = Manual;
         *          };
         *          5623C57217FDCB0800090B9E = {
         *              DevelopmentTeam = Z6SFPV59E3;
         *              ProvisioningStyle = Manual;
         *              TestTargetID = 1D6058900D05DD3D006BFB54;
         *          };
         *      };
         *  };
         */
        internal void SetTargetAttributes(string key, string value)
        {
            PBXElementDict properties = project.project.GetPropertiesRaw();
            PBXElementDict attributes;
            PBXElementDict targetAttributes;
            if (properties.Contains("attributes"))
            {
                attributes = properties["attributes"] as PBXElementDict;
            }
            else
            {
                attributes = properties.CreateDict("attributes");
            }

            if (attributes.Contains("TargetAttributes"))
            {
                targetAttributes = attributes["TargetAttributes"] as PBXElementDict;
            }
            else
            {
                targetAttributes = attributes.CreateDict("TargetAttributes");
            }

            foreach (KeyValuePair<string, PBXNativeTargetData> target in nativeTargets.GetEntries()) {
                PBXElementDict targetAttributesRaw;
                if (targetAttributes.Contains(target.Key))
                {
                    targetAttributesRaw = targetAttributes[target.Key].AsDict();
                }
                else
                {
                    targetAttributesRaw = targetAttributes.CreateDict(target.Key);
                }
                targetAttributesRaw.SetString(key, value);
            }
            project.project.UpdateVars();

        }
    }
} // namespace UnityEditor.iOS.Xcode
