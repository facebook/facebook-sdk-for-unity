/*
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 *
 * This source code is licensed under the license found in the
 * LICENSE file in the root directory of this source tree.
 */

namespace Facebook.Unity.Tests
{
    using Facebook.Unity.Tests.Editor;
    using Facebook.Unity.Tests.Mobile.Android;
    using Facebook.Unity.Tests.Mobile.IOS;
    using NUnit.Framework;

    [AndroidTest]
    [TestFixture]
    public class AndroidProductImprovement : FacebookTestClass
    {
        [TestCase(true)]
        [TestCase(false)]
        public void ForwardsProductImprovementSetting(bool enabled)
        {
            FB.Mobile.SetAutoLogProductImprovementEnabled(enabled);
            Assert.AreEqual(enabled.ToString(), ((MockAndroid)this.Mock).AutoLogProductImprovementEnabled);
            Assert.AreEqual(1, this.Mock.GetMethodCallCount("SetAutoLogProductImprovementEnabled"));
            Assert.AreEqual(0, this.Mock.GetMethodCallCount("SetAutoLogAppEventsEnabled"));
        }
    }

    [IOSTest]
    [TestFixture]
    public class IOSProductImprovement : FacebookTestClass
    {
        [TestCase(true)]
        [TestCase(false)]
        public void ForwardsProductImprovementSetting(bool enabled)
        {
            FB.Mobile.SetAutoLogProductImprovementEnabled(enabled);
            Assert.AreEqual(enabled, ((MockIOS)this.Mock).AutoLogProductImprovementEnabled);
            Assert.AreEqual(1, this.Mock.GetMethodCallCount("FBAutoLogProductImprovementEnabled"));
            Assert.AreEqual(0, this.Mock.GetMethodCallCount("FBAutoLogAppEventsEnabled"));
        }
    }

    [EditorTest]
    [TestFixture]
    public class EditorProductImprovement : FacebookTestClass
    {
        [TestCase(true)]
        [TestCase(false)]
        public void ProductImprovementSettingIsSafeInEditor(bool enabled)
        {
            Assert.DoesNotThrow(() => FB.Mobile.SetAutoLogProductImprovementEnabled(enabled));
            Assert.AreEqual(0, this.Mock.GetMethodCallCount("SetAutoLogProductImprovementEnabled"));
        }
    }
}
