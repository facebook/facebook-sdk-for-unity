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

﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Facebook.Unity
{
    public class CodelessUIInteractEvent : MonoBehaviour
    {

        private FBSDKEventBindingManager eventBindingManager { get; set; }

        void Awake ()
        {
            EventSystem sceneEventSystem = FindObjectOfType<EventSystem> ();
            if (sceneEventSystem == null) {
                GameObject eventSystem = new GameObject ("EventSystem");
                eventSystem.AddComponent<EventSystem> ();
#if ENABLE_INPUT_SYSTEM
                eventSystem.AddComponent<UnityEngine.InputSystem.UI.InputSystemUIInputModule>();
#else
                eventSystem.AddComponent<StandaloneInputModule> ();
#endif
                DontDestroyOnLoad (eventSystem);
            }
            switch (Constants.CurrentPlatform) {
            case FacebookUnityPlatform.Android:
                SetLoggerInitAndroid ();
                break;
            case FacebookUnityPlatform.IOS:
                SetLoggerInitIos ();
                break;
            default:
                break;
            }
        }

        private static void SetLoggerInitAndroid ()
        {
            AndroidJavaObject fetchedAppSettingsManager = new AndroidJavaClass ("com.facebook.internal.FetchedAppSettingsManager");
            fetchedAppSettingsManager.CallStatic ("setIsUnityInit", true);
        }

        private static void SetLoggerInitIos ()
        {
            //PLACEHOLDER for IOS
        }

        // Update is called once per frame
        void Update ()
        {
            bool leftMouseButtonDown;
            bool primaryTouchDown;
            int touchCount;
            int primaryTouchFingerId;
            
#if ENABLE_INPUT_SYSTEM
            leftMouseButtonDown = UnityEngine.InputSystem.Mouse.current.leftButton.wasPressedThisFrame;
            primaryTouchDown = UnityEngine.InputSystem.Touchscreen.current.primaryTouch.press.wasPressedThisFrame;
            touchCount = UnityEngine.InputSystem.Touchscreen.current.touches.Count;
            primaryTouchFingerId = UnityEngine.InputSystem.Touchscreen.current.primaryTouch.touchId.ReadValue();
#else 
            leftMouseButtonDown = Input.GetMouseButtonDown (0);
            touchCount = Input.touchCount;
            primaryTouchDown = touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Began;
            primaryTouchId = Input.touches[0].fingerId;
#endif
            
            if (leftMouseButtonDown || (primaryTouchDown)) {
                try {
                    if (EventSystem.current.IsPointerOverGameObject () ||
                        (touchCount > 0 && EventSystem.current.IsPointerOverGameObject (primaryTouchFingerId))
                        ) {
                        if (null != EventSystem.current.currentSelectedGameObject) {
                            string name = EventSystem.current.currentSelectedGameObject.name;
                            GameObject go = EventSystem.current.currentSelectedGameObject;
                            if (null != go.GetComponent<UnityEngine.UI.Button> () &&
                                null != eventBindingManager) {

                                var eventBindings = eventBindingManager.eventBindings;
                                FBSDKEventBinding matchedBinding = null;
                                if (null != eventBindings) {
                                  foreach(var eventBinding in eventBindings) {
                                      if (FBSDKViewHiearchy.CheckGameObjectMatchPath(go, eventBinding.path)) {
                                          matchedBinding = eventBinding;
                                          break;
                                      }
                                  }
                                }

                                if (null != matchedBinding) {
                                    FB.LogAppEvent(matchedBinding.eventName);
                                }
                            }
                        }
                    }
                } catch (Exception ex) {
                    Debug.Log (ex);
                }
            }
        }

        public void OnReceiveMapping (string message)
        {
            var dict = MiniJSON.Json.Deserialize(message) as List<System.Object>;
            this.eventBindingManager = new FBSDKEventBindingManager(dict);
        }

    }
}
