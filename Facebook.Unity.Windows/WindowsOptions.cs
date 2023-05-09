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
namespace Facebook.Unity.Windows
{
    [Serializable]
    public class WindowsOptions
    {
        [Serializable]
        public class Browser
        {
            public int port = 0;
            public string scheme = "https";
            public string url = "www.facebook.com";
        }
        [Serializable]
        public class Curl
        {
            public uint backend = 1;
            public bool verify_peer = false;
            public uint verbose = 0;
            public string cainfo = "";
        }
        [Serializable]
        public class Graph
        {
            public string scheme = "https";
            public string url = "graph.fb.gg";
            public string query = "";
        }
        [Serializable]
        public class Logger
        {
            public string file = "fbg.log";
            public string filter = "(\\w+)";
            public int verbosity = 0;
        }
        [Serializable]
        public class Login
        {
            public string client_token = "";
            public string redirect_uri = "";
            public string permissions = "gaming_profile,user_friends";
        }

        public Browser browser = new Browser();
        public Curl curl = new Curl();
        public Graph graph = new Graph();
        public Logger logger = new Logger();
        public Login login = new Login();

        public WindowsOptions(string clientToken)
        {
            this.login.client_token = clientToken;
        }
    }
}
