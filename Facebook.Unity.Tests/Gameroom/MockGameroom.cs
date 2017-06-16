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

namespace Facebook.Unity.Tests.Gameroom
{
  using System;
  using System.Collections.Generic;
  using Facebook.Unity.Gameroom;
  using NUnit.Framework;

  internal class MockGameroom : MockWrapper, IGameroomWrapper
  {
    private Dictionary<string, object> response = new Dictionary<string, object>();

    public IDictionary<string, object> PipeResponse
    {
        get
        {
            return this.response;
        }

        set
        {
            if (value == null)
            {
                return;
            }

            throw new NotSupportedException("Can only set pipe response to null");
        }
    }

    public void Init(GameroomFacebook.OnComplete completeDelegate)
    {
        // Handle testing of init returning access token. It would be nice
        // to not have init return the access token but this could be
        // a breaking change for people who read the raw result
        ResultContainer resultContainer;
        IDictionary<string, object> resultExtras = this.ResultExtras;
        if (resultExtras != null)
        {
            var result = MockResults.GetGenericResult(0, resultExtras);
            resultContainer = new ResultContainer(result);
        }
        else
        {
            resultContainer = new ResultContainer(string.Empty);
        }

        completeDelegate(resultContainer);
    }

    public void DoLoginRequest(
        string appID,
        string permissions,
        string callbackID,
        GameroomFacebook.OnComplete completeDelegate)
    {
        var result = MockResults.GetLoginResult(int.Parse(callbackID), permissions, this.ResultExtras);
        completeDelegate(new ResultContainer(result));
    }

    public void DoPayRequest(
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
        GameroomFacebook.OnComplete completeDelegate)
    {
        // Stub
    }

    public void DoFeedShareRequest(
        string appId,
        string toId,
        string link,
        string linkName,
        string linkCaption,
        string linkDescription,
        string pictureLink,
        string mediaSource,
        string callbackID,
        GameroomFacebook.OnComplete completeDelegate)
    {
        // Stub
    }

    public void DoAppRequestRequest(
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
        GameroomFacebook.OnComplete completeDelegate)
    {
        int cbid = Convert.ToInt32(callbackID);
        completeDelegate(new ResultContainer(MockResults.GetGenericResult(cbid, this.ResultExtras)));
    }

    public void DoPayPremiumRequest(
        string appId,
        string callbackID,
        GameroomFacebook.OnComplete completeDelegate)
    {
        // Stub
    }

    public void DoHasLicenseRequest(
        string appId,
        string callbackID,
        GameroomFacebook.OnComplete completeDelegate)
    {
        // Stub
    }
  }
}
