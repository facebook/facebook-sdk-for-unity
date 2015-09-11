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
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml;
    using System.Xml.Linq;

    public class PListDict : Dictionary<string, object>
    {
        public PListDict()
        {
        }

        public PListDict(PListDict dict) : base(dict)
        {
        }

        public PListDict(XElement dict)
        {
            this.Load(dict);
        }

        public void Load(XElement dict)
        {
            var dictElements = dict.Elements();
            this.ParseDictForLoad(this, dictElements);
        }

        public void Save(string fileName, XDeclaration declaration, XDocumentType docType)
        {
            XElement plistNode = new XElement("plist", this.ParseDictForSave(this));
            plistNode.SetAttributeValue("version", "1.0");
            XDocument file = new XDocument(declaration, docType);
            file.Add(plistNode);
            file.Save(fileName);
        }

        public XElement ParseValueForSave(object node)
        {
            if (node is string)
            {
                return new XElement("string", node);
            }
            else if (node is bool)
            {
                return new XElement(node.ToString().ToLower());
            }
            else if (node is int)
            {
                return new XElement("integer", node);
            }
            else if (node is float)
            {
                return new XElement("real", node);
            }
            else if (node is IList<object>)
            {
                return this.ParseArrayForSave(node);
            }
            else if (node is PListDict)
            {
                return this.ParseDictForSave((PListDict)node);
            }
            else if (node == null)
            {
                return null;
            }

            throw new NotSupportedException("Unexpected type: " + node.GetType().FullName);
        }

        private void ParseDictForLoad(PListDict dict, IEnumerable<XElement> elements)
        {
            for (int i = 0; i < elements.Count(); i += 2)
            {
                XElement key = elements.ElementAt(i);
                XElement val = elements.ElementAt(i + 1);
                dict[key.Value] = this.ParseValueForLoad(val);
            }
        }

        private IList<object> ParseArrayForLoad(IEnumerable<XElement> elements)
        {
            var list = new List<object>();
            foreach (XElement e in elements)
            {
                object one = this.ParseValueForLoad(e);
                list.Add(one);
            }

            return list;
        }

        private object ParseValueForLoad(XElement val)
        {
            switch (val.Name.ToString())
            {
                case "string":
                    return val.Value;
                case "integer":
                    return int.Parse(val.Value);
                case "real":
                    return float.Parse(val.Value);
                case "true":
                    return true;
                case "false":
                    return false;
                case "dict":
                    PListDict plist = new PListDict();
                    this.ParseDictForLoad(plist, val.Elements());
                    return plist;
                case "array":
                    return this.ParseArrayForLoad(val.Elements());
                default:
                    throw new ArgumentException("Format unsupported, Parser update needed");
            }
        }

        private XElement ParseDictForSave(PListDict dict)
        {
            XElement dictNode = new XElement("dict");
            foreach (string key in dict.Keys)
            {
                dictNode.Add(new XElement("key", key));
                dictNode.Add(this.ParseValueForSave(dict[key]));
            }

            return dictNode;
        }

        private XElement ParseArrayForSave(object node)
        {
            XElement arrayNode = new XElement("array");
            var array = (IList<object>)node;
            for (int i = 0; i < array.Count; i++)
            {
                arrayNode.Add(this.ParseValueForSave(array[i]));
            }

            return arrayNode;
        }
    }
}
