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

﻿namespace Facebook.Unity.Mobile.Android
{
    using System;
    using System.Linq;
    using System.Reflection;

    internal class AndroidWrapper : IAndroidWrapper
    {
        private const string FacebookJavaClassName = "com.facebook.unity.FB";
        private const string UnityEngineAssemblyName = "UnityEngine";
        private const string AndroidJavaClassName = "UnityEngine.AndroidJavaClass";
        private const string CallStaticMethodInfoName = "CallStatic";

        private static Type androidJavaClassType;
        private static object androidJavaClassObject;
        private static MethodInfo callStaticMethodInfo;
        private static MethodInfo callStaticMethodInfoGeneric;

        private static Type AndroidJavaClassType
        {
            get
            {
                if (androidJavaClassType != null)
                {
                    return AndroidWrapper.androidJavaClassType;
                }

                Assembly assembly = Assembly.Load(AndroidWrapper.UnityEngineAssemblyName);
                AndroidWrapper.androidJavaClassType = assembly.GetType(AndroidJavaClassName);

                if (AndroidWrapper.androidJavaClassType == null)
                {
                    throw new InvalidOperationException(
                        "Failed to load type: " + AndroidJavaClassName);
                }

                return AndroidWrapper.androidJavaClassType;
            }
        }

        private static object AndroidJavaClassObject
        {
            get
            {
                if (androidJavaClassObject != null)
                {
                    return AndroidWrapper.androidJavaClassObject;
                }

                AndroidWrapper.androidJavaClassObject = Activator.CreateInstance(
                    AndroidWrapper.AndroidJavaClassType,
                    FacebookJavaClassName);

                if (AndroidWrapper.androidJavaClassObject == null)
                {
                    throw new InvalidOperationException(
                        "Failed to institate object of type: " + AndroidWrapper.AndroidJavaClassType.FullName);
                }

                return AndroidWrapper.androidJavaClassObject;
            }
        }

        private static MethodInfo CallStaticMethodInfo
        {
            get
            {
                if (callStaticMethodInfo != null)
                {
                    return AndroidWrapper.callStaticMethodInfo;
                }

                AndroidWrapper.callStaticMethodInfo = AndroidJavaClassType.GetMethod(
                    AndroidWrapper.CallStaticMethodInfoName,
                    new Type[] { typeof(string), typeof(object[]) });

                if (AndroidWrapper.callStaticMethodInfo == null)
                {
                    throw new InvalidOperationException(
                        "Failed to locate method: " + AndroidWrapper.CallStaticMethodInfoName);
                }

                return AndroidWrapper.callStaticMethodInfo;
            }
        }

        private static MethodInfo CallStaticMethodInfoGeneric
        {
            get
            {
                if (callStaticMethodInfoGeneric != null)
                {
                    return AndroidWrapper.callStaticMethodInfoGeneric;
                }

                MethodInfo[] methods = AndroidJavaClassType.GetMethods();
                foreach (MethodInfo methodInfo in methods)
                {
                    if (methodInfo.Name != CallStaticMethodInfoName)
                    {
                        continue;
                    }

                    if (methodInfo.GetGenericArguments().Count() != 1)
                    {
                        continue;
                    }

                    ParameterInfo[] parameters = methodInfo.GetParameters();
                    if (parameters.Count() != 2)
                    {
                        continue;
                    }

                    if (!parameters[0].ParameterType.IsAssignableFrom(typeof(string)))
                    {
                        continue;
                    }

                    if (!parameters[1].ParameterType.IsAssignableFrom(typeof(object[])))
                    {
                        continue;
                    }

                    AndroidWrapper.callStaticMethodInfoGeneric = methodInfo;
                    break;
                }

                if (AndroidWrapper.callStaticMethodInfoGeneric == null)
                {
                    throw new InvalidOperationException(
                        "Failed to locate generic method: " + AndroidWrapper.CallStaticMethodInfoName);
                }

                return AndroidWrapper.callStaticMethodInfoGeneric;
            }
        }

        public void CallStatic(string methodName, params object[] args)
        {
            AndroidWrapper.CallStaticMethodInfo.Invoke(
                AndroidWrapper.AndroidJavaClassObject,
                new object[] { methodName, args });
        }

        public T CallStatic<T>(string methodName)
        {
            MethodInfo methodInfo = AndroidWrapper.CallStaticMethodInfoGeneric.MakeGenericMethod(typeof(T));
            if (methodInfo == null)
            {
                throw new InvalidOperationException("Failed to make generic method for calling static");
            }

            return (T)methodInfo.Invoke(
                AndroidWrapper.AndroidJavaClassObject,
                new object[] { methodName, new object[0] });
        }
    }
}
