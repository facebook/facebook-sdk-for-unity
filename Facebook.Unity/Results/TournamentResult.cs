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
    using System.Collections.Generic;
    using System.Text;

    internal class TournamentResult : ResultBase, ITournamentResult
    {
        internal TournamentResult(ResultContainer resultContainer) : base(resultContainer)
        {
            if (this.ResultDictionary != null)
            {
                string tournamentId;
                if (this.ResultDictionary.TryGetValue("tournament_id", out tournamentId))
                {
                    this.TournamentId = tournamentId;
                }

                string contextId;
                if (this.ResultDictionary.TryGetValue("context_id", out contextId))
                {
                    this.ContextId = contextId;
                }

                int endTime;
                if (this.ResultDictionary.TryGetValue("end_time", out endTime))
                {
                    this.EndTime = endTime;
                }

                string tournamentTitle;
                if (this.ResultDictionary.TryGetValue("tournament_title", out tournamentTitle))
                {
                    this.TournamentTitle = tournamentTitle;
                }

                IDictionary<string, string> payload;
                if (this.ResultDictionary.TryGetValue("payload", out payload))
                {
                    this.Payload = payload;
                }
            }
        }

        public string TournamentId { get; private set; }

        public string ContextId { get; private set; }

        public int EndTime { get; private set; }

        public string TournamentTitle { get; private set; }

        public IDictionary<string, string> Payload { get; private set; }

        public override string ToString()
        {

            StringBuilder sb = new StringBuilder();
            foreach (var kvp in this.Payload)
            {
                sb.AppendFormat("\n\t{0}: {1}", kvp.Key, kvp.Value);

            }

            return Utilities.FormatToString(
                base.ToString(),
                this.GetType().Name,
                new Dictionary<string, string>()
                {
                    { "TournamentId", this.TournamentId },
                    { "ContextId", this.ContextId},
                    { "EntTime", this.EndTime.ToString() },
                    { "TournamentTitle", this.TournamentTitle },
                    { "Payload", sb.ToString()},
                });
        }
    }
}