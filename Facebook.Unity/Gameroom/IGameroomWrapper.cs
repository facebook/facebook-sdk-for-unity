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

namespace Facebook.Unity.Gameroom
{
    using System.Collections.Generic;

    internal interface IGameroomWrapper
    {
        IDictionary<string, object> PipeResponse { get; set; }

        void Init(GameroomFacebook.OnComplete completeDelegate);

        void DoLoginRequest(
            string appID,
            string permissions,
            string callbackID,
            GameroomFacebook.OnComplete completeDelegate);

        void DoPayRequest(
            string appId,
            string method,
            string action,
            string product,
            string productId,
            string quantity,
            string quantityMin,
            string quantityMax,
            string requestId,
            string pricepointId,
            string testCurrency,
            string developerPayload,
            string callbackID,
            GameroomFacebook.OnComplete completeDelegate);

        void DoFeedShareRequest(
            string appId,
            string toId,
            string link,
            string linkName,
            string linkCaption,
            string linkDescription,
            string pictureLink,
            string mediaSource,
            string callbackID,
            GameroomFacebook.OnComplete completeDelegate);

        void DoAppRequestRequest(
            string appId,
            string message,
            string actionType,
            string objectId,
            string to,
            string filters,
            string excludeIDs,
            string maxRecipients,
            string data,
            string title,
            string callbackID,
            GameroomFacebook.OnComplete completeDelegate);

        void DoPayPremiumRequest(
            string appId,
            string callbackID,
            GameroomFacebook.OnComplete completeDelegate);

        void DoHasLicenseRequest(
            string appId,
            string callbackID,
            GameroomFacebook.OnComplete completeDelegate);
    }
}
