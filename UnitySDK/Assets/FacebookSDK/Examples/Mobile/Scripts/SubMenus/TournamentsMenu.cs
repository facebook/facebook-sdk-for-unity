
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

namespace Facebook.Unity.Example
{
    using System;
    using System.Collections;
    using UnityEngine;

    internal class TournamentsMenu : MenuBase
    {
        private string score = string.Empty;
        private string tournamentID = string.Empty;

        protected override void GetGui()
        {
            bool enabled = GUI.enabled;
            GUI.enabled = enabled && FB.IsLoggedIn;

            if (this.Button("Get Tournaments"))
            {
                FB.Mobile.GetTournaments(this.HandleResult);
            }

            GUILayout.Space(24);
            this.LabelAndTextField("Score:", ref this.score);
            this.LabelAndTextField("TournamentID:", ref this.tournamentID);
            if (this.Button("Post Score to Tournament"))
            {
                FB.Mobile.UpdateTournament(tournamentID, int.Parse(score), this.HandleResult);
            }

            if (this.Button("Update Tournament and Share"))
            {
                FB.Mobile.UpdateAndShareTournament(tournamentID, int.Parse(score), this.HandleResult);
            }

            if (this.Button("Create Tournament and Share"))
            {
                FB.Mobile.CreateAndShareTournament(
                    int.Parse(score),
                    "Unity Tournament",
                    TournamentSortOrder.HigherIsBetter,
                    TournamentScoreFormat.Numeric,
                    DateTime.UtcNow.AddHours(2),
                    "Unity SDK Tournament",
                    this.HandleResult
                );

            }

            GUI.enabled = enabled;
        }
    }
}
