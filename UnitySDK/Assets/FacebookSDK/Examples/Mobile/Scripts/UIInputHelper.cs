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

using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Facebook.Unity;

public class UIInputHelper : MonoBehaviour
{
    private FBSDKEventBindingManager eventBindingManager { get; set; }
    private Boolean isOldEventSystem { get; set; } = false;

    private void Start()
    {
        EventSystem sceneEventSystem = FindObjectOfType<EventSystem>();
        isOldEventSystem = !(sceneEventSystem == null);
        if (!isOldEventSystem)
        {
            var eventSystem = new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
            DontDestroyOnLoad(eventSystem);
        }
    }

    void Update()
    {
        if (isOldEventSystem)
        {
            if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
            {
                try
                {
                    if (EventSystem.current.IsPointerOverGameObject() ||
                        (Input.touchCount > 0 && EventSystem.current.IsPointerOverGameObject(Input.touches[0].fingerId))
                        )
                    {
                        if (null != EventSystem.current.currentSelectedGameObject)
                        {
                            string name = EventSystem.current.currentSelectedGameObject.name;
                            GameObject go = EventSystem.current.currentSelectedGameObject;
                            if (null != go.GetComponent<UnityEngine.UI.Button>() &&
                                null != eventBindingManager)
                            {

                                var eventBindings = eventBindingManager.eventBindings;
                                FBSDKEventBinding matchedBinding = null;
                                if (null != eventBindings)
                                {
                                    foreach (var eventBinding in eventBindings)
                                    {
                                        if (FBSDKViewHiearchy.CheckGameObjectMatchPath(go, eventBinding.path))
                                        {
                                            matchedBinding = eventBinding;
                                            break;
                                        }
                                    }
                                }

                                if (null != matchedBinding)
                                {
                                    FB.LogAppEvent(matchedBinding.eventName);
                                }
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    return;
                }
            }
        }
    }
}
