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
