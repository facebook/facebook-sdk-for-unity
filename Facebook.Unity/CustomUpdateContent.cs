/*
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
using System.Collections.Generic;
using Facebook.MiniJSON;

using UnityEngine;


namespace Facebook.Unity
{

    /// <summary>
    /// Static class to hold Custom Update Content for FBGamingServices.PerformCustomUpdate.
    /// </summary>
    public sealed class CustomUpdateContent
    {
        public const string CONTEXT_TOKEN_KEY = "context_token_id";
        public const string CTA_KEY = "cta";
        public const string DATA_KEY = "data";
        public const string DEFAULT_KEY = "default";
        public const string GIF_KEY = "gif";
        public const string IMAGE_KEY = "image";
        public const string LOCALIZATIONS_KEY = "localizations";
        public const string MEDIA_KEY = "media";
        public const string TEXT_KEY = "text";
        public const string URL_KEY = "url";
        public const string VIDEO_KEY = "video";

        private string _contextTokenId;
        private CustomUpdateLocalizedText _text;
        private CustomUpdateLocalizedText _cta;
        private string _image;
        private CustomUpdateMedia _media;
        private string _data;

        private CustomUpdateContent(
            string contextTokenId,
            CustomUpdateLocalizedText text,
            CustomUpdateLocalizedText cta,
            string image,
            CustomUpdateMedia media,
            string data)
        {
            _contextTokenId = contextTokenId;
            _text = text;
            _cta = cta;
            _image = image;
            _media = media;
            _data = data;
        }

        public IDictionary<string, string> toGraphAPIData()
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add(CONTEXT_TOKEN_KEY, _contextTokenId);
            data.Add(TEXT_KEY, _text.toJson());
            if (_cta != null)
            {
                data.Add(CTA_KEY, _cta.toJson());
            }
            if (_image != null)
            {
                data.Add(IMAGE_KEY, _image);
            }
            if (_media != null)
            {
                data.Add(MEDIA_KEY, _media.toJson());
            }
            if (_data != null)
            {
                data.Add(DATA_KEY, _data);
            }
            return data;
        }

        public class CustomUpdateContentBuilder
        {
            private string _contextTokenId;
            private CustomUpdateLocalizedText _text;
            private CustomUpdateLocalizedText _cta;
            private string _image;
            private CustomUpdateMedia _media;
            private string _data;

            /// <summary>
            /// Creates a CustomUpdateContent Builder
            /// </summary>
            /// <param name="contextTokenId">A valid GamingContext to send the update to</param>
            /// <param name="text">The text that will be included in the update</param>
            /// <param name="image">An image that will be included in the update</param>
            public CustomUpdateContentBuilder(
                string contextTokenId,
                CustomUpdateLocalizedText text,
                Texture2D image)
            {
                _contextTokenId = contextTokenId;
                _text = text;
                byte[] bytes = image.EncodeToPNG();
                _image = "data:image/png;base64," + Convert.ToBase64String(bytes);
            }

            /// <summary>
            /// Creates a CustomUpdateContent Builder
            /// </summary>
            /// <param name="contextTokenId">A valid GamingContext to send the update to</param>
            /// <param name="text">The text that will be included in the update</param>
            /// <param name="media">A gif or video that will be included in the update</param>
            public CustomUpdateContentBuilder(
                string contextTokenId,
                CustomUpdateLocalizedText text,
                CustomUpdateMedia media)
            {
                _contextTokenId = contextTokenId;
                _text = text;
                _media = media;
            }

            /// <summary>
            /// Sets the CTA (Call to Action) text in the update message
            /// </summary>
            /// <param name="cta">Custom CTA to use. If none is provided a localized version of 'play' is used.</param>
            public CustomUpdateContentBuilder setCTA(CustomUpdateLocalizedText cta) {
                _cta = cta;
                return this;
            }

            /// <summary>
            ///  Sets a Data that will be sent back to the game when a user clicks on the message. When the
            ///  game is launched from a Custom Update message the data here will be forwarded as a Payload.
            /// </summary>
            /// <param name="data">A String that will be sent back to the game</param>
            public CustomUpdateContentBuilder setData(string data) {
                _data = data;
                return this;
            }

            /// <summary>
            /// Returns a CustomUpdateContent with the values defined in this builder
            /// </summary>
            /// <returns>CustomUpdateContent instance to pass to FBGamingServices.PerformCustomUpdate()</returns>
            public CustomUpdateContent build() {
                return new CustomUpdateContent(
                    _contextTokenId,
                    _text,
                    _cta,
                    _image,
                    _media,
                    _data);
            }
        }

    }

    /// <summary>
    /// Represents a text string that can have different Locale values provided.
    /// </summary>
    public sealed class CustomUpdateLocalizedText {
        private string _default;
        private IDictionary<string, string> _localizations;

        /// <summary>
        /// Creates a CustomUpdateLocalizedText instance
        /// </summary>
        /// <param name="defaultText">Text that will be used if no matching locale is found</param>
        /// <param name="localizations">Optional key-value Dictionary of Locale_Code: Locale String Value for this text.
        /// For a list of valid locale codes see:
        /// https://lookaside.facebook.com/developers/resources/?id=FacebookLocales.xml </param>
        public CustomUpdateLocalizedText(string defaultText, IDictionary<string, string> localizations)
        {
            _default = defaultText;
            _localizations = localizations;
        }

        public string toJson()
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add(CustomUpdateContent.DEFAULT_KEY, _default);
            if (_localizations != null)
            {
                data.Add(CustomUpdateContent.LOCALIZATIONS_KEY, _localizations);
            }
            return Json.Serialize(data);
        }
    }

    /// <summary>
    /// Represents a media that will be included in a Custom Update Message
    /// </summary>
    public sealed class CustomUpdateMedia {
        private CustomUpdateMediaInfo _gif;
        private CustomUpdateMediaInfo _video;

        /// <summary>
        /// Creates a CustomUpdateMedia instance. Note that gif and video are mutually exclusive
        /// </summary>
        /// <param name="gif">Gif that will be included in the Update Message</param>
        /// <param name="video">Video that will be included in the Update Message. Currently this is not yet
        /// supported but will be in a server side update so it is already included in the SDK. This
        /// disclaimer will be removed as soon as it is.</param>
        public CustomUpdateMedia(CustomUpdateMediaInfo gif, CustomUpdateMediaInfo video)
        {
            _gif = gif;
            _video = video;
        }

        public string toJson()
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            if (_gif != null)
            {
                Dictionary<string, string> media = new Dictionary<string, string>();
                media.Add(CustomUpdateContent.URL_KEY, _gif.Url);
                data.Add(CustomUpdateContent.GIF_KEY, media);
            }
            if (_video != null)
            {
                Dictionary<string, string> media = new Dictionary<string, string>();
                media.Add(CustomUpdateContent.URL_KEY, _video.Url);
                data.Add(CustomUpdateContent.VIDEO_KEY, media);
            }
            return Json.Serialize(data);
        }

    }

    /// <summary>
    /// Stores Information about a Media that will be part of a Custom Update
    /// </summary>
    public sealed class CustomUpdateMediaInfo {
        private string _url;
        public string Url {
            get {
                return _url;
            }
        }
        public CustomUpdateMediaInfo(string url)
        {
            _url = url;
        }
    }
}
