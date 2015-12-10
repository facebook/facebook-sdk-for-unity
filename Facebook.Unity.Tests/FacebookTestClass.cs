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

namespace Facebook.Unity.Tests
{
    using System;
    using Facebook.Unity.Canvas;
    using Facebook.Unity.Mobile.Android;
    using Facebook.Unity.Mobile.IOS;
    using Facebook.Unity.Tests.Canvas;
    using Facebook.Unity.Tests.Mobile.Android;
    using Facebook.Unity.Tests.Mobile.IOS;
    using NUnit.Framework;

    public abstract class FacebookTestClass
    {
        internal MockWrapper Mock { get; set; }

        [SetUp]
        public void Init()
        {
            FacebookLogger.Instance = new FacebookTestLogger();
            Type type = this.GetType();
            if (Attribute.GetCustomAttribute(type, typeof(AndroidTestAttribute)) != null)
            {
                var callbackManager = new CallbackManager();
                var mockWrapper = new MockAndroid();
                Constants.CurrentPlatform = FacebookUnityPlatform.Android;
                var facebook = new AndroidFacebook(mockWrapper, callbackManager);
                this.Mock = mockWrapper;
                this.Mock.Facebook = facebook;
                FB.FacebookImpl = facebook;
            }
            else if (Attribute.GetCustomAttribute(type, typeof(IOSTestAttribute)) != null)
            {
                var callbackManager = new CallbackManager();
                var mockWrapper = new MockIOS();
                Constants.CurrentPlatform = FacebookUnityPlatform.IOS;
                var facebook = new IOSFacebook(mockWrapper, callbackManager);
                this.Mock = mockWrapper;
                this.Mock.Facebook = facebook;
                FB.FacebookImpl = facebook;
            }
            else if (Attribute.GetCustomAttribute(type, typeof(CanvasTestAttribute)) != null)
            {
                var callbackManager = new CallbackManager();
                var mockWrapper = new MockCanvas();
                Constants.CurrentPlatform = FacebookUnityPlatform.WebGL;
                var facebook = new CanvasFacebook(mockWrapper, callbackManager);
                this.Mock = mockWrapper;
                this.Mock.Facebook = facebook;
                FB.FacebookImpl = facebook;
            }
            else
            {
                throw new Exception("No platform specified on test class");
            }

            this.OnInit();
        }

        protected virtual void OnInit()
        {
        }

        private class FacebookTestLogger : IFacebookLogger
        {
            public void Log(string msg)
            {
                Console.WriteLine(msg);
            }

            public void Info(string msg)
            {
                Console.WriteLine(msg);
            }

            public void Warn(string msg)
            {
                Console.WriteLine(msg);
            }

            public void Error(string msg)
            {
                Console.WriteLine(msg);
            }
        }
    }
}
