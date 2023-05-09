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

var FBUnityLib = {
    $FBUnity: {
        init: function(connectFacebookUrl, locale, debug, initParams, status) {
            // make element for js sdk
            if (!document.getElementById('fb-root')) {
                var fbroot = document.createElement('div');
                fbroot.id = 'fb-root';
                var body = document.getElementsByTagName('body')[0];
                body.insertBefore(fbroot, body.children[0]);
            }

            // load js sdk
            var js, id = 'facebook-jssdk', ref = document.getElementsByTagName('script')[0];
            if (document.getElementById(id)) {return;}
            js = document.createElement('script'); js.id = id; js.async = true;
            js.src = connectFacebookUrl + '/' + locale + '/sdk' + (debug ? '/debug' : '') + '.js';
            ref.parentNode.insertBefore(js, ref);
            // once jssdk is loaded, init
            window.fbAsyncInit = function() {
                initParams = JSON.parse(initParams);
                initParams.hideFlashCallback = FBUnity.onHideUnity;
                FB.init(initParams);
                // send url to unity - needed for deep linking
                FBUnity.sendMessage('OnUrlResponse', location.href);
                if (status) {
                    FBUnity.onInitWithStatus();
                } else {
                    FBUnity.onInit();
                }
            };
        },

        initScreenPosition: function() {
            if (!screenPosition) {
                var body = document.getElementsByTagName('body')[0];
                var screenPosition = {omo : body.onmouseover || function(){}, iframeX: 0, iframeY: 0};
                body.onmouseover = function(e) {
                    // Distance from top of screen to top of client area
                    screenPosition.iframeX = e.screenX - e.clientX;
                    screenPosition.iframeY = e.screenY - e.clientY;

                    screenPosition.omo(e);
                }
            }
        },

        sendMessage: function(method, param) {
            SendMessage('FacebookJsBridge', method, param);
        },

        login: function(scope, callback_id) {
            FB.login(FBUnity.loginCallback.bind(null, callback_id), scope ? {scope: scope, auth_type: 'rerequest', return_scopes: true} : {return_scopes: true});
        },

        loginCallback: function(callback_id, response) {
            response = {'callback_id': callback_id, 'response': response};
            FBUnity.sendMessage('OnLoginComplete', JSON.stringify(response));
        },

        onInitWithStatus: function() {
            var timeoutHandler = setTimeout(function() { requestFailed(); }, 3000);

            function requestFailed() {
                FBUnity.onInit();
            }

            // try to get the login status right after init'ing
            FB.getLoginStatus(function(response) {
                clearTimeout(timeoutHandler);
                FBUnity.onInit(response);
            });
        },

        onInit: function(response) {
            var jsonResponse = '';
            if (response && response.authResponse) {
                jsonResponse = JSON.stringify(response);
            }

            FBUnity.sendMessage('OnInitComplete', jsonResponse);
            FB.Event.subscribe('auth.authResponseChange', function(r){ FBUnity.onAuthResponseChange(r) });

            FBUnity.logLoadingTime(response);
        },

        logLoadingTime: function(response) {
            FB.Canvas.setDoneLoading(
                function(result) {
                    // send implicitly event to log the time from the canvas pages load to facebook init being called.
                    FBUnity.logAppEvent('fb_canvas_time_till_init_complete', result.time_delta_ms / 1000, null);
                }
            );
        },

        onAuthResponseChange: function(response) {
            FBUnity.sendMessage('OnFacebookAuthResponseChange', response ? JSON.stringify(response) : '');
        },

        apiCallback: function(query, response) {
            response = {'query': query, 'response': response};
            FBUnity.sendMessage('OnFacebookAPIResponse', JSON.stringify(response));
        },

        api: function(query) {
            FB.api(query, FBUnity.apiCallback.bind(null, query));
        },

        activateApp: function() {
            FB.AppEvents.activateApp();
        },

        uiCallback: function(uid, callbackMethodName, response) {
            response = {'callback_id': uid, 'response': response};
            FBUnity.sendMessage(callbackMethodName, JSON.stringify(response));
        },

        logout: function() {
            FB.logout();
        },

        logAppEvent: function(eventName, valueToSum, parameters) {
            FB.AppEvents.logEvent(
                eventName,
                valueToSum,
                JSON.parse(parameters)
            );
        },

        logPurchase: function(purchaseAmount, currency, parameters) {
            FB.AppEvents.logPurchase(
                purchaseAmount,
                currency,
                JSON.parse(parameters)
            );
        },

        ui: function(x, uid, callbackMethodName) {
            x = JSON.parse(x);
            FB.ui(x, FBUnity.uiCallback.bind(null, uid, callbackMethodName));
        },


        hideUnity: function(direction) {
            direction = direction || 'hide';
            //TODO support this for webgl
            var unityDiv = jQuery(u.getUnity());

            if (direction == 'hide') {
                FBUnity.sendMessage('OnFacebookFocus', 'hide');
            } else /*show*/ {
                FBUnity.sendMessage('OnFacebookFocus', 'show');

                if (FBUnity.showScreenshotBackground.savedBackground) {
                    /*
                    if(fbShowScreenshotBackground.savedBackground == 'sentinel') {
                        jQuery('body').css('background', null);
                    } else {
                        jQuery('body').css('background', fbShowScreenshotBackground.savedBackground);
                    }
                    */
                }

                hideUnity.savedCSS = FBUnity.showScreenshotBackground.savedBackground = null;
            }
        },

        showScreenshotBackground: function(pngbytes) /*and hide unity*/ {
            // window.screenxX and window.screenY = browser position
            // window.screen.height and window.screen.width = screen size
            // findPos, above, locates the iframe within the browser
            /*
            if (!fbShowScreenshotBackground.savedBackground)
                fbShowScreenshotBackground.savedBackground = jQuery('body').css('background') || 'sentinel';

            jQuery('body').css('background-image', 'url(data:image/png;base64,'+pngbytes+')');
            jQuery('body').css(
                'background-position',
                -(screenPosition.iframeX)+'px '+
                -(screenPosition.iframeY)+'px'
            );
            jQuery('body').css('background-size', '100%');
            jquery('body').css('background-repeat', 'no-repeat');
            // TODO: Zoom detection
            */
        },

        onHideUnity: function(info) {
            if (info.state == 'opened') {
                FBUnity.sendMessage('OnFacebookFocus', 'hide');
            } else {
                FBUnity.sendMessage('OnFacebookFocus', 'show');
            }
        }
    },

    init: function(connectFacebookUrl, locale, debug, initParams, status) {
        var connectFacebookUrlString = UTF8ToString(connectFacebookUrl);
        var localeString = UTF8ToString(locale);
        var initParamsString = UTF8ToString(initParams);

        FBUnity.init(connectFacebookUrlString, localeString, debug, initParamsString, status);
    },

    initScreenPosition: function() {
        FBUnity.initScreenPosition();
    },

    login: function(scope, callback_id) {
        var scopeString = UTF8ToString(scope);
        var scopeArray = JSON.parse(scopeString);

        var callback_idString = UTF8ToString(callback_id);

        FBUnity.login(scopeArray, callback_idString);
    },

    activateApp: function() {
        FBUnity.activateApp();
    },

    logout: function() {
        FBUnity.logout();
    },

    logAppEvent: function(eventName, valueToSum, parameters) {
        var eventNameString = UTF8ToString(eventName);
        var parametersString = UTF8ToString(parameters);

        FBUnity.logAppEvent(eventNameString, valueToSum, parametersString);
    },

    logAppEventWithoutValue: function(eventName, parameters) {
        var eventNameString = UTF8ToString(eventName);
        var parametersString = UTF8ToString(parameters);

        FBUnity.logAppEvent(eventNameString, null, parametersString);
    },

    logPurchase: function(purchaseAmount, currency, parameters) {
        var currencyString = UTF8ToString(currency);
        var parametersString = UTF8ToString(parameters);

        FBUnity.logPurchase(purchaseAmount, currencyString, parametersString);
    },

    ui: function(x, uid, callbackMethodName) {
        var xString = UTF8ToString(x);
        var uidString = UTF8ToString(uid);
        var callbackMethodNameString = UTF8ToString(callbackMethodName);

        FBUnity.ui(xString, uidString, callbackMethodNameString);
    }
};

autoAddDeps(LibraryManager.library, '$FBUnity');
mergeInto(LibraryManager.library, FBUnityLib);
