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
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Facebook.Unity
{
    public class FBSDKViewHiearchy
    {
        public static bool CheckGameObjectMatchPath(GameObject go, List<FBSDKCodelessPathComponent> path)
        {
            var goPath = GetPath (go);
            return CheckPathMatchPath (goPath, path);
        }

        public static bool CheckPathMatchPath(List<FBSDKCodelessPathComponent> goPath, List<FBSDKCodelessPathComponent> path)
        {
            for (int i = 0; i < System.Math.Min(goPath.Count, path.Count); i++) {
                var idxGoPath = goPath.Count - i - 1;
                var idxPath = path.Count - i - 1;

                var goPathComponent = goPath [idxGoPath];
                var pathComponent = path [idxPath];

                // TODO: add more attributes comparison beyond class names
                if (String.Compare (goPathComponent.className, pathComponent.className) != 0) {
                    return false;
                }
            }
            return true;
        }

        public static List<FBSDKCodelessPathComponent> GetPath(GameObject go)
        {
            return GetPath (go, Constants.MaxPathDepth);
        }

        public static List<FBSDKCodelessPathComponent> GetPath(GameObject go, int limit)
        {
            if (go == null || limit <= 0) {
                return null;
            }

            var path = new List<FBSDKCodelessPathComponent> ();
            var parent = GetParent (go);
            if (parent != null) {
                var parentPath = GetPath (parent, limit - 1);
                path = parentPath;
            } else {
                // pAdd the scene first
                var componentInfo1 = new Dictionary<string, System.Object>();
                componentInfo1.Add (Constants.EventBindingKeysClassName, SceneManager.GetActiveScene ().name);
                var pathComponent1 = new FBSDKCodelessPathComponent (componentInfo1);
                path.Add (pathComponent1);
            }

            var componentInfo = GetAttribute(go, parent);
            var pathComponent = new FBSDKCodelessPathComponent (componentInfo);
            path.Add (pathComponent);

            return path;
        }

        public static GameObject GetParent(GameObject go)
        {
            var parentTransform = go.transform.parent;
            if (parentTransform != null) {
                return parentTransform.gameObject;
            }
            return null;
        }

        public static Dictionary<string, System.Object> GetAttribute(GameObject obj, GameObject parent)
        {
            var result = new Dictionary<string, System.Object> ();
            result.Add (Constants.EventBindingKeysClassName, obj.name);
            if (parent != null) {
                result.Add (Constants.EventBindingKeysIndex, Convert.ToInt64(obj.transform.GetSiblingIndex ()));
            } else {
                result.Add (Constants.EventBindingKeysIndex, Convert.ToInt64(0));
            }
            return result;
        }
    }
}
