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
    using System.Linq;

    /// <summary>
    /// Contains a Inviation from Friend Finder.
    /// </summary>
    public class FriendFinderInviation
    {
        internal FriendFinderInviation(
            string id,
            string applicationId,
            string applicationName,
            string fromId,
            string fromName,
            string toId,
            string toName,
            string message,
            string createdTime)
        {
            this.Id = sanityCheckParam(id, "id");
            this.ApplicationId = sanityCheckParam(applicationId, "applicationId");
            this.ApplicationName = sanityCheckParam(applicationName, "applicationName");
            this.FromId = sanityCheckParam(fromId, "fromId");
            this.FromName = sanityCheckParam(fromName, "fromName");
            this.ToId = sanityCheckParam(toId, "toId");
            this.ToName = sanityCheckParam(toName, "toName");
            this.Message = message;
            this.CreatedTime = sanityCheckParam(createdTime, "createdTime");
        }

        private string sanityCheckParam(string param, string errorMsg)
        {
            if (string.IsNullOrEmpty(param))
            {
                throw new ArgumentNullException(errorMsg);
            }
            return param;
        }

        public string Id { get; private set; }

        public string ApplicationId { get; private set; }

        public string ApplicationName { get; private set; }

        public string FromId { get; private set; }

        public string FromName { get; private set; }

        public string ToId { get; private set; }

        public string ToName { get; private set; }

        public string Message { get; private set; }

        public string CreatedTime { get; private set; }



    }
}
