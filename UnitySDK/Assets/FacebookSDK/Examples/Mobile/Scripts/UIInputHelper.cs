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
