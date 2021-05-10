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
    using System.Collections.Generic;

    public class Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Profile"/> class.
        /// </summary>
        /// <param name="userID">User ID.</param>
        /// <param name="firstName">First Name.</param>
        /// <param name="middleName">Middle Name.</param>
        /// <param name="lastName">Last Name.</param>
        /// <param name="name">Name.</param>
        /// <param name="email">Email.</param>
        /// <param name="imageURL">Image URL.</param>
        /// <param name="linkURL">Link URL.</param>
        /// <param name="friendIDs">A list of identifiers for the user's friends.</param>
        /// <param name="birthday">User's birthday</param>
        /// <param name="ageRange">Age Range for the User</param>
        /// <param name="hometown">Home Town</param>
        /// <param name="location">Location</param>
        /// <param name="gender">Gender</param>

        internal Profile(
            string userID,
            string firstName,
            string middleName,
            string lastName,
            string name,
            string email,
            string imageURL,
            string linkURL,
            string[] friendIDs,
            string birthday,
            UserAgeRange ageRange,
            FBLocation hometown,
            FBLocation location,
            string gender)
        {
            this.UserID = userID;
            this.FirstName = firstName;
            this.MiddleName = middleName;
            this.LastName = lastName;
            this.Name = name;
            this.Email = email;
            this.ImageURL = imageURL;
            this.LinkURL = linkURL;
            this.FriendIDs = friendIDs ?? new string[] { };
            long birthdayTimestamp;
            if (long.TryParse(birthday, out birthdayTimestamp))
            {
                this.Birthday = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)
                    .AddMilliseconds(birthdayTimestamp * 1000)
                    .ToLocalTime();
            }
            this.AgeRange = ageRange;
            this.Hometown = hometown;
            this.Location = location;
            this.Gender = gender;
        }

        /// <summary>
        /// Gets the user ID.
        /// </summary>
        /// <value>The user ID.</value>
        public string UserID { get; private set; }

        /// <summary>
        /// Gets the fist name.
        /// </summary>
        /// <value>The fist name.</value>
        public string FirstName { get; private set; }

        /// <summary>
        /// Gets the middle name.
        /// </summary>
        /// <value>The middle name.</value>
        public string MiddleName { get; private set; }

        /// <summary>
        /// Gets the last name.
        /// </summary>
        /// <value>The last name.</value>
        public string LastName { get; private set; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the email.
        /// </summary>
        /// <value>The email.</value>
        public string Email { get; private set; }

        /// <summary>
        /// Gets the image URL.
        /// </summary>
        /// <value>The image URL.</value>
        public string ImageURL { get; private set; }

        /// <summary>
        /// Gets the link URL.
        /// </summary>
        /// <value>The link url.</value>
        public string LinkURL { get; private set; }

        /// <summary>
        /// Gets the list of identifiers for the user's friends.
        /// </summary>
        /// <value>The list of identifiers for the user's friends.</value>
        public string[] FriendIDs { get; private set; }

        /// <summary>
        /// Gets the user's birthday.
        /// </summary>
        /// <value>The user's birthday.</value>
        public DateTime? Birthday { get; private set; }

        /// <summary>
        /// Gets the user's age range.
        /// </summary>
        /// <value>The user's age range.</value>
        public UserAgeRange AgeRange { get; private set; }

        /// <summary>
        /// Gets the user's hometown
        /// </summary>
        /// <value>The user's hometown</value>
        public FBLocation Hometown { get; private set; }

        /// <summary>
        /// Gets the user's location
        /// </summary>
        /// <value>The user's location </value>
        public FBLocation Location { get; private set; }

        /// <summary>
        /// Gets the user's gender
        /// </summary>
        /// <value>The user's gender</value>
        public string Gender { get; private set; }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents the current <see cref="Facebook.Unity.Profile"/>.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents the current <see cref="Facebook.Unity.Profile"/>.</returns>
        public override string ToString()
        {
            return Utilities.FormatToString(
                null,
                this.GetType().Name,
                new Dictionary<string, string>()
                {
                    { "UserID", this.UserID},
                    { "FirstName", this.FirstName },
                    { "MiddleName", this.MiddleName},
                    { "LastName", this.LastName },
                    { "Name", this.Name},
                    { "Email", this.Email },
                    { "ImageURL", this.ImageURL},
                    { "LinkURL", this.LinkURL },
                    { "FriendIDs", String.Join(",", this.FriendIDs) },
                    { "Birthday", this.Birthday?.ToShortDateString()},
                    { "AgeRange", this.AgeRange?.ToString()},
                    { "Hometown", this.Hometown?.ToString() },
                    { "Location", this.Location?.ToString() },
                    { "Gender", this.Gender },
                });
        }
    }
}
