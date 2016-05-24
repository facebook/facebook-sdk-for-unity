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

namespace Facebook.Unity.Example
{
    using System;

    internal class AppInvites : MenuBase
    {
        protected override void GetGui()
        {
            if (this.Button("Android Invite"))
            {
                this.Status = "Logged FB.AppEvent";
                FB.Mobile.AppInvite(new Uri("https://fb.me/892708710750483"), callback: this.HandleResult);
            }

            if (this.Button("Android Invite With Custom Image"))
            {
                this.Status = "Logged FB.AppEvent";
                FB.Mobile.AppInvite(new Uri("https://fb.me/892708710750483"), new Uri("http://i.imgur.com/zkYlB.jpg"), this.HandleResult);
            }

            if (this.Button("iOS Invite"))
            {
                this.Status = "Logged FB.AppEvent";
                FB.Mobile.AppInvite(new Uri("https://fb.me/810530068992919"), callback: this.HandleResult);
            }

            if (this.Button("iOS Invite With Custom Image"))
            {
                this.Status = "Logged FB.AppEvent";
                FB.Mobile.AppInvite(new Uri("https://fb.me/810530068992919"), new Uri("http://i.imgur.com/zkYlB.jpg"), this.HandleResult);
            }
        }
    }
}
