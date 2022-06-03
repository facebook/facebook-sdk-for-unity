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

import FBSDKCoreKit
import FBSDKGamingServicesKit
import Foundation

@objc
public protocol FBSDKShareTournamentDialogDelegate: AnyObject {
    func didComplete(dialog: FBSDKShareTournamentDialog, tournament: FBSDKTournament)
    func didFail(withError error: Error, dialog: FBSDKShareTournamentDialog)
    func didCancel(dialog: FBSDKShareTournamentDialog)
}


/// An internal wrapper for sharing tournaments via dialog
@objcMembers
public final class FBSDKShareTournamentDialog: NSObject, ShareTournamentDialogDelegate {
    weak var delegate: FBSDKShareTournamentDialogDelegate?
    private var dialog: ShareTournamentDialog?

    public init(delegate: FBSDKShareTournamentDialogDelegate) {
        self.delegate = delegate
        super.init()
        dialog = ShareTournamentDialog(delegate: self)
    }

    public func show(score: Int, tournamentID: String) throws {
        try dialog?.show(score: score, tournamentID: tournamentID)
    }

    public func show(
        initialScore: Int,
        title: String? = nil,
        endTime: Date? = nil,
        scoreType: Int,
        sortOrder: Int,
        payload: String? = nil
    ) throws {
        let currentConfig = TournamentConfig(
            title: title,
            endTime: endTime,
            scoreType: scoreType == 0 ? .numeric : .time,
            sortOrder: sortOrder == 0 ? .higherIsBetter : .lowerIsBetter,
            image: nil,
            payload: payload
        )
        try dialog?.show(initialScore: initialScore, config: currentConfig)
    }

    // MARK: FBSDKShareTournamentDialogDelegate
    public func didComplete(dialog: ShareTournamentDialog, tournament: Tournament) {
        delegate?.didComplete(dialog: self, tournament: FBSDKTournament(tournament: tournament))
    }

    public func didFail(withError error: Error, dialog: ShareTournamentDialog) {
        delegate?.didFail(withError: error, dialog: self)
    }

    public func didCancel(dialog: ShareTournamentDialog) {
        delegate?.didCancel(dialog: self)
    }
}
