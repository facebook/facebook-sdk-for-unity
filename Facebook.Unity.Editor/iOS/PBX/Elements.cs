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

using System.Collections.Generic;
using System.Collections;
using System;

namespace Facebook.Unity.Editor.iOS.Xcode.PBX
{

    class PBXElement
    {
        protected PBXElement() {}

        // convenience methods
        public string AsString() { return ((PBXElementString)this).value; }
        public PBXElementArray AsArray() { return (PBXElementArray)this; }
        public PBXElementDict AsDict()   { return (PBXElementDict)this; }

        public PBXElement this[string key]
        {
            get { return AsDict()[key]; }
            set { AsDict()[key] = value; }
        }
    }

    class PBXElementString : PBXElement
    {
        public PBXElementString(string v) { value = v; }

        public string value;
    }

    class PBXElementDict : PBXElement
    {
        public PBXElementDict() : base() {}

        private Dictionary<string, PBXElement> m_PrivateValue = new Dictionary<string, PBXElement>();
        public IDictionary<string, PBXElement> values { get { return m_PrivateValue; }}

        new public PBXElement this[string key]
        {
            get {
                if (values.ContainsKey(key))
                    return values[key];
                return null;
            }
            set { this.values[key] = value; }
        }

        public bool Contains(string key)
        {
            return values.ContainsKey(key);
        }

        public void Remove(string key)
        {
            values.Remove(key);
        }

        public void SetString(string key, string val)
        {
            values[key] = new PBXElementString(val);
        }

        public PBXElementArray CreateArray(string key)
        {
            var v = new PBXElementArray();
            values[key] = v;
            return v;
        }

        public PBXElementDict CreateDict(string key)
        {
            var v = new PBXElementDict();
            values[key] = v;
            return v;
        }
    }

    class PBXElementArray : PBXElement
    {
        public PBXElementArray() : base() {}
        public List<PBXElement> values = new List<PBXElement>();

        // convenience methods
        public void AddString(string val)
        {
            values.Add(new PBXElementString(val));
        }

        public PBXElementArray AddArray()
        {
            var v = new PBXElementArray();
            values.Add(v);
            return v;
        }

        public PBXElementDict AddDict()
        {
            var v = new PBXElementDict();
            values.Add(v);
            return v;
        }
    }

} // namespace UnityEditor.iOS.Xcode
