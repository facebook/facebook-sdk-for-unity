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
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// The result of a graph api call.
    /// </summary>
    public interface IGraphResult : IResult
    {
        /// <summary>
        /// Gets the result.
        /// </summary>
        /// <value>A list parsed from the result</value>
        IList<object> ResultList { get; }

        /// <summary>
        /// Gets the Texture.
        /// </summary>
        /// <value>
        /// A texture downloaded from the graph endpoint if the graph api redirected to a image, otherwise null.
        /// </value>
        /// <remarks>
        ///     The Graph API does not return textures directly, but a few endpoints can
        ///     redirect to images when no 'redirect=false' parameter is specified. Ex: '/me/picture'
        /// </remarks>
        Texture2D Texture { get; }
    }
}
