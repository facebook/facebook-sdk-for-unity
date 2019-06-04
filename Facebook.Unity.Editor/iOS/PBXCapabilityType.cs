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

namespace Facebook.Unity.Editor.iOS.Xcode
{
    /// <summary>
    /// List of all the capabilities available.
    /// </summary>
    public sealed class PBXCapabilityType
    {
        public static readonly PBXCapabilityType ApplePay = new PBXCapabilityType ("com.apple.ApplePay", true);
        public static readonly PBXCapabilityType AppGroups = new PBXCapabilityType ("com.apple.ApplicationGroups.iOS", true);
        public static readonly PBXCapabilityType AssociatedDomains = new PBXCapabilityType ("com.apple.SafariKeychain", true);
        public static readonly PBXCapabilityType BackgroundModes = new PBXCapabilityType ("com.apple.BackgroundModes", false);
        public static readonly PBXCapabilityType DataProtection = new PBXCapabilityType ("com.apple.DataProtection", true);
        public static readonly PBXCapabilityType GameCenter = new PBXCapabilityType ("com.apple.GameCenter", false, "GameKit.framework");
        public static readonly PBXCapabilityType HealthKit = new PBXCapabilityType ("com.apple.HealthKit", true, "HealthKit.framework");
        public static readonly PBXCapabilityType HomeKit = new PBXCapabilityType ("com.apple.HomeKit", true, "HomeKit.framework");
        public static readonly PBXCapabilityType iCloud = new PBXCapabilityType("com.apple.iCloud", true, "CloudKit.framework", true);
        public static readonly PBXCapabilityType InAppPurchase = new PBXCapabilityType ("com.apple.InAppPurchase", false);
        public static readonly PBXCapabilityType InterAppAudio =
          new PBXCapabilityType ("com.apple.InterAppAudio", true, "AudioToolbox.framework");
        public static readonly PBXCapabilityType KeychainSharing = new PBXCapabilityType ("com.apple.KeychainSharing", true);
        public static readonly PBXCapabilityType Maps = new PBXCapabilityType("com.apple.Maps.iOS", false, "MapKit.framework");
        public static readonly PBXCapabilityType PersonalVPN =
          new PBXCapabilityType("com.apple.VPNLite", true, "NetworkExtension.framework");
        public static readonly PBXCapabilityType PushNotifications = new PBXCapabilityType ("com.apple.Push", true);
        public static readonly PBXCapabilityType Siri = new PBXCapabilityType ("com.apple.Siri", true);
        public static readonly PBXCapabilityType Wallet = new PBXCapabilityType ("com.apple.Wallet", true, "PassKit.framework");
        public static readonly PBXCapabilityType WirelessAccessoryConfiguration =
          new PBXCapabilityType("com.apple.WAC", true, "ExternalAccessory.framework");

        private readonly string m_ID;
        private readonly bool m_RequiresEntitlements;
        private readonly string m_Framework;
        private readonly bool m_OptionalFramework;

        public bool optionalFramework
        {
            get { return m_OptionalFramework; }
        }

        public string framework
        {
            get { return m_Framework; }
        }

        public string id
        {
            get { return m_ID; }
        }

        public bool requiresEntitlements
        {
            get { return m_RequiresEntitlements; }
        }

        public struct TargetCapabilityPair
        {
            public string targetGuid;
            public PBXCapabilityType capability;

            public TargetCapabilityPair(string guid, PBXCapabilityType type)
            {
                targetGuid = guid;
                capability = type;
            }
        }

        /// <summary>
        /// This private object represents what a capability changes in the PBXProject file
        /// </summary>
        /// <param name="id">The string used in the PBXProject file to identify the capability and mark it as enabled.</param>
        /// <param name="requiresEntitlements">This capability requires an entitlements file
        /// therefore we need to add this entitlements file to the code signing entitlement.
        /// </param>
        /// <param name="framework">Specify which framework need to be added to the
        /// project for this capability, if "" no framework are added.
        /// </param>
        /// <param name="optionalFramework">Some capability (right now only iCloud)
        /// adds a framework, not all the time but just when some option are checked
        /// this parameter indicates if one of them is checked.</param>
        private PBXCapabilityType(string _id, bool _requiresEntitlements, string _framework = "", bool _optionalFramework = false)
        {
            m_ID = _id;
            m_RequiresEntitlements = _requiresEntitlements;
            m_Framework = _framework;
            m_OptionalFramework = _optionalFramework;
        }

        public static PBXCapabilityType StringToPBXCapabilityType(string cap)
        {
            switch (cap)
            {
                case "com.apple.ApplePay":
                    return ApplePay;
                case "com.apple.ApplicationGroups.iOS":
                    return AppGroups;
                case "com.apple.SafariKeychain":
                    return AssociatedDomains;
                case "com.apple.BackgroundModes":
                    return BackgroundModes;
                case "com.apple.DataProtection":
                    return DataProtection;
                case "com.apple.GameCenter":
                    return GameCenter;
                case "com.apple.HealthKit":
                    return HealthKit;
                case "com.apple.HomeKit":
                    return HomeKit;
                case "com.apple.iCloud":
                    return iCloud;
                case "com.apple.InAppPurchase":
                    return InAppPurchase;
                case "com.apple.InterAppAudio":
                    return InterAppAudio;
                case "com.apple.KeychainSharing":
                    return KeychainSharing;
                case "com.apple.Maps.iOS":
                    return Maps;
                case "com.apple.VPNLite":
                    return PersonalVPN;
                case "com.apple.Push":
                    return PushNotifications;
                case "com.apple.Siri":
                    return Siri;
                case "com.apple.Wallet":
                    return Wallet;
                case "WAC":
                    return WirelessAccessoryConfiguration;
                default:
                    return null;
            }
        }
    }
}
