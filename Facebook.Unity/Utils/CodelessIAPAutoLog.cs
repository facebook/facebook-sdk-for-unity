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
    using System.Reflection;
    using UnityEngine;
    using UnityEngine.Events;
    using System.Collections.Generic;

    internal class CodelessIAPAutoLog
    {
        internal static void handlePurchaseCompleted(System.Object data) {
            try {
                if (!FB.Mobile.IsImplicitPurchaseLoggingEnabled()) {
                    return;
                }
                object metadata =  CodelessIAPAutoLog.GetProperty(data, "metadata");
                object productDefinition = CodelessIAPAutoLog.GetProperty(data, "definition");
                if (metadata == null || productDefinition == null) {
                    return;
                }
                Decimal price = (Decimal)CodelessIAPAutoLog.GetProperty(metadata, "localizedPrice");
                String currency = (String)CodelessIAPAutoLog.GetProperty(metadata, "isoCurrencyCode");
                String id = (String)CodelessIAPAutoLog.GetProperty(productDefinition, "id");
                FB.LogAppEvent(
                    AppEventName.Purchased,
                    (float)price,
                    new Dictionary<string, object>()
                    {
                        { "_implicitlyLogged", "1" },
                        { AppEventParameterName.Currency, currency },
                        { AppEventParameterName.ContentID, id }
                    });
            }
            catch (System.Exception e)
            {
                FacebookLogger.Log("Failed to automatically handle Purchase Completed: " + e.Message);
            }
        }

        internal static void addListenerToIAPButtons(object listenerObject) {
            UnityEngine.Object[] iapButtons = FindObjectsOfTypeByName("IAPButton", "UnityEngine.Purchasing");
            if (iapButtons == null) {
                return;
            }
            foreach (UnityEngine.Object btn in iapButtons) {
                addListenerToGameObject(btn, listenerObject);
            }
        }

        internal static void addListenerToGameObject(UnityEngine.Object gameObject, object listenerObject) {
            // Code stripping will work here when developer uses Unity IAP service the UnityEngine.Purchasing will be linked
            // If it's not enabled, this will return null and it's expected and handled in null check
            Type productType = FindTypeInAssemblies("Product", "UnityEngine.Purchasing");
            if (productType == null) {
                return;
            }
            Type eventGeneric = typeof(UnityEvent<>);
            Type actionGeneric = typeof(UnityAction<>);

            Type[] typeArgs = { productType };
            Type unityEventProduct = eventGeneric.MakeGenericType(typeArgs);
            Type unityActionProduct = actionGeneric.MakeGenericType(typeArgs);

            UnityEventBase onPurchaseComplete = (UnityEventBase)GetField(gameObject, "onPurchaseComplete");
            MethodInfo addListener = unityEventProduct.GetMethod("AddListener");
            MethodInfo removeListener = unityEventProduct.GetMethod("RemoveListener");

            MethodInfo method = listenerObject.GetType().GetMethod(
                "onPurchaseCompleteHandler",
                BindingFlags.Instance | BindingFlags.Public
            );

            removeListener.Invoke (onPurchaseComplete, new object[] {
                System.Delegate.CreateDelegate(unityActionProduct, listenerObject, method)
            });
            addListener.Invoke (onPurchaseComplete, new object[] {
                System.Delegate.CreateDelegate(unityActionProduct, listenerObject, method)
            });
        }

        private static System.Type FindTypeInAssemblies(string typeName, string nameSpace)
        {
            var assemblies = System.AppDomain.CurrentDomain.GetAssemblies();
            for(int i = 0; i < assemblies.Length; i++)
            {
                var types = assemblies[i].GetTypes();
                for(int n = 0; n < types.Length; n++)
                {
                    if (typeName == types[n].Name && nameSpace == types[n].Namespace) {
                        return types[n];
                    }
                }
            }
            return null;
        }

        private static UnityEngine.Object[] FindObjectsOfTypeByName(string typeName, string nameSpace)
        {
            // Code stripping will work here when developer uses Unity IAP service the UnityEngine.Purchasing will be linked
            // If it's not enabled, this will return null and it's expected and handled in null check
            Type type = FindTypeInAssemblies(typeName, nameSpace);
            if (type == null) {
                return null;
            }
            return UnityEngine.Object.FindObjectsOfType(type);
        }

        private static object GetField(object inObj, string fieldName)
        {
            object ret = null;
            FieldInfo info = inObj.GetType().GetField(fieldName);
            if (info != null)
                ret = info.GetValue(inObj);
            return ret;
        }

        private static object GetProperty(object inObj, string propertyName)
        {
            object ret = null;
            PropertyInfo info = inObj.GetType().GetProperty(propertyName);
            if (info != null)
                ret = info.GetValue(inObj, null);
            return ret;
        }
    }
}
