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
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

// Base classes for section handling

namespace Facebook.Unity.Editor.iOS.Xcode.PBX
{

    // common base
    internal abstract class SectionBase
    {
        public abstract void AddObject(string key, PBXElementDict value);
        public abstract void WriteSection(StringBuilder sb, GUIDToCommentMap comments);
    }

    // known section: contains objects that we care about
    internal class KnownSectionBase<T> : SectionBase where T : PBXObjectData, new()
    {
        private Dictionary<string, T> m_Entries = new Dictionary<string, T>();

        private string m_Name;

        public KnownSectionBase(string sectionName)
        {
            m_Name = sectionName;
        }

        public IEnumerable<KeyValuePair<string, T>> GetEntries()
        {
            return m_Entries;
        }

        public IEnumerable<string> GetGuids()
        {
            return m_Entries.Keys;
        }

        public IEnumerable<T> GetObjects()
        {
            return m_Entries.Values;
        }

        public override void AddObject(string key, PBXElementDict value)
        {
            T obj = new T();
            obj.guid = key;
            obj.SetPropertiesWhenSerializing(value);
            obj.UpdateVars();
            m_Entries[obj.guid] = obj;
        }

        public override void WriteSection(StringBuilder sb, GUIDToCommentMap comments)
        {
            if (m_Entries.Count == 0)
                return;            // do not write empty sections

            sb.AppendFormat("\n\n/* Begin {0} section */", m_Name);
            var keys = new List<string>(m_Entries.Keys);
            keys.Sort(StringComparer.Ordinal);
            foreach (string key in keys)
            {
                T obj = m_Entries[key];
                obj.UpdateProps();
                sb.Append("\n\t\t");
                comments.WriteStringBuilder(sb, obj.guid);
                sb.Append(" = ");
                Serializer.WriteDict(sb, obj.GetPropertiesWhenSerializing(), 2,
                                     obj.shouldCompact, obj.checker, comments);
                sb.Append(";");
            }
            sb.AppendFormat("\n/* End {0} section */", m_Name);
        }

        // returns null if not found
        public T this[string guid]
        {
            get {
                if (m_Entries.ContainsKey(guid))
                    return m_Entries[guid];
                return null;
            }
        }

        public bool HasEntry(string guid)
        {
            return m_Entries.ContainsKey(guid);
        }

        public void AddEntry(T obj)
        {
            m_Entries[obj.guid] = obj;
        }

        public void RemoveEntry(string guid)
        {
            if (m_Entries.ContainsKey(guid))
                m_Entries.Remove(guid);
        }
    }

    // we assume there is only one PBXProject entry
    internal class PBXProjectSection : KnownSectionBase<PBXProjectObjectData>
    {
        public PBXProjectSection() : base("PBXProject")
        {
        }

        public PBXProjectObjectData project
        {
            get {
                foreach (var kv in GetEntries())
                    return kv.Value;
                return null;
            }
        }
    }

} // UnityEditor.iOS.Xcode
