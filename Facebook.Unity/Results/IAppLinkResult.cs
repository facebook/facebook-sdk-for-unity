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

namespace Facebook.Unity
{
    using System.Collections.Generic;

    /// <summary>
    /// A result containing an app link.
    /// </summary>
    public interface IAppLinkResult : IResult
    {
        /// <summary>
        /// Gets the URL. This is the url that was used to open the app on iOS
        /// or on Android the intent's data string. When handling deffered app
        /// links on Android this may not be available.
        /// </summary>
        /// <value>The link url.</value>
        string Url { get; }

        /// <summary>
        /// Gets the target URI.
        /// </summary>
        /// <value>The target uri for this App Link.</value>
        string TargetUrl { get; }

        /// <summary>
        /// Gets the ref.
        /// </summary>
        /// <value> Returns the ref for this App Link.
        /// The referer data associated with the app link.
        /// This will contain Facebook specific information like fb_access_token, fb_expires_in, and fb_ref.
        /// </value>
        string Ref { get; }

        /// <summary>
        /// Gets the extras.
        /// </summary>
        /// <value>
        /// The full set of arguments for this app link. Properties like target uri &amp; ref are typically
        /// picked out of this set of arguments.
        /// </value>
        IDictionary<string, object> Extras { get; }
    }
}
