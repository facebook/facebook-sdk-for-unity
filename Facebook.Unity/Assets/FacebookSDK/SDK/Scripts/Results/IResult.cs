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
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// A result.
    /// </summary>
    public interface IResult
    {
        /// <summary>
        /// Gets the error.
        /// </summary>
        /// <value>The error string from the result. If no error occured value is null or empty.</value>
        string Error { get; }

        /// <summary>
        /// Gets the result.
        /// </summary>
        /// <value>A collection of key values pairs that are parsed from the result</value>
        IDictionary<string, object> ResultDictionary { get; }

        /// <summary>
        /// Gets the raw result.
        /// </summary>
        /// <value>The raw result string.</value>
        string RawResult { get; }

        /// <summary>
        /// Gets a value indicating whether this instance cancelled.
        /// </summary>
        /// <value><c>true</c> if this instance cancelled; otherwise, <c>false</c>.</value>b
        bool Cancelled { get; }
    }
}
