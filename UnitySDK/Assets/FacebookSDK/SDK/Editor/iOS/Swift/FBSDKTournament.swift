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

import FBSDKGamingServicesKit
import Foundation


/// An internal representation of tournament graph objects.
@objcMembers
public final class FBSDKTournament: NSObject {

    /// The unique ID that is associated with this tournament.
    public internal(set) var identifier: String

    /**
     Timestamp when the tournament ends.
     If the expiration is in the past, then the tournament is already finished and has expired.
     */
    public internal(set) var endTime: Date?

    /// Title of the tournament provided upon the creation of the tournament.
    public internal(set) var title: String?

    /// Payload of the tournament provided upon the creation of the tournament.
    public var payload: String?

    public convenience init(tournament: Tournament) {
        self.init(
            identifier: tournament.identifier,
            endTime: tournament.endTime,
            title: tournament.title,
            payload: tournament.payload
        )
    }

    public var toDictionary: [String: String] {
        var dictionary = ["tournament_id": identifier]
        if let endTime = endTime {
            dictionary["end_time"] = "\(endTime.timeIntervalSince1970)"
        }
        dictionary["tournament_title"] = title
        dictionary["payload"] = payload
        return dictionary
    }

    public init(
        identifier: String,
        endTime: Date? = nil,
        title: String? = nil,
        payload: String? = nil
    ) {
        self.identifier = identifier
        self.endTime = endTime
        self.title = title
        self.payload = payload
    }
}
