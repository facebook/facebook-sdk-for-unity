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
    /// <summary>
    /// Contains the names used for standard App Events.
    /// </summary>
    public static class AppEventName
    {
        /// <summary>
        /// App Event for achieved level.
        /// </summary>
        public const string AchievedLevel = "fb_mobile_level_achieved";

        /// <summary>
        /// App Event for  activated app.
        /// </summary>
        public const string ActivatedApp = "fb_mobile_activate_app";

        /// <summary>
        /// App Event for  added payment info.
        /// </summary>
        public const string AddedPaymentInfo = "fb_mobile_add_payment_info";

        /// <summary>
        /// App Event for  added to cart.
        /// </summary>
        public const string AddedToCart = "fb_mobile_add_to_cart";

        /// <summary>
        /// App Event for  added to wishlist.
        /// </summary>
        public const string AddedToWishlist = "fb_mobile_add_to_wishlist";

        /// <summary>
        /// App Event for  completed registration.
        /// </summary>
        public const string CompletedRegistration = "fb_mobile_complete_registration";

        /// <summary>
        /// App Event for  completed tutorial.
        /// </summary>
        public const string CompletedTutorial = "fb_mobile_tutorial_completion";

        /// <summary>
        /// App Event for  initiated checkout.
        /// </summary>
        public const string InitiatedCheckout = "fb_mobile_initiated_checkout";

        /// <summary>
        /// App Event for  purchased.
        /// </summary>
        public const string Purchased = "fb_mobile_purchase";

        /// <summary>
        /// App Event for  rated.
        /// </summary>
        public const string Rated = "fb_mobile_rate";

        /// <summary>
        /// App Event for  searched.
        /// </summary>
        public const string Searched = "fb_mobile_search";

        /// <summary>
        /// App Event for  spent credits.
        /// </summary>
        public const string SpentCredits = "fb_mobile_spent_credits";

        /// <summary>
        /// App Event for  unlocked achievement.
        /// </summary>
        public const string UnlockedAchievement = "fb_mobile_achievement_unlocked";

        /// <summary>
        /// App Event for  content of the viewed.
        /// </summary>
        public const string ViewedContent = "fb_mobile_content_view";
    }
}
