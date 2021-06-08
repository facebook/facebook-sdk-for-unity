# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

## [11.0.0] - 2021-06-05

### Added

- Added tournaments APIs for cloud game
- Added Limited Login support for `user_hometown`, `user_location`, `user_gender` and `user_link` permissions under public beta.

### Changed

- Bumped native SDK version to 11.0.0
- Bumped Graph API version to 11.0

### Deprecated

- Deprecated UpdateUserProperties API

## [9.2.0] - 2021-04-25

### Added

- Added AEM (Aggregated Events Measurement) support under public beta.

### Fixed

- Fix `IOSFBEnableProfileUpdatesOnAccessTokenChange` build issue

## [9.1.0] - 2021-04-06

### Added

- Added Limited Login support for `user_friends`, `user_birthday` and `user_age_range` permissions under public beta.
- Shared `Profile` instance will be populated with `FriendIDs`, `Birthday` and `AgeRange` fields using the claims from the AuthenticationToken. (NOTE: `FriendIDs`, `Birthday` and `AgeRange` fields are in public beta mode)
- Added `EnableProfileUpdatesOnAccessTokenChange` as part of fixing a bug where upgrading from limited to regular login would fail to fetch the profile using the newly available access token.

## [9.0.0] - 2021-01-10

### Added

- Limited Login. See the [docs](https://developers.facebook.com/docs/facebook-login/ios/limited-login/) for a general overview and implementation details.

## [8.1.1] - 2020-10-25

### Fixed

- Add use_frameworks! to Podfile
- Avoid adding swift standard libs

## [8.1.0] - 2020-10-14

### Fixed

- Fixed AndroidSdk path detection in Unity 2019 and above

## [8.0.0] - 2020-09-27

### Added

- Added method `SetAdvertiserTrackingEnabled` to overwrite the `advertiser_tracking_enabled` flag

### Changed

- Bumped native SDK version to 8.0.0
- Bumped Graph API version to 8.0

### Fixed

- Fixed lock up issue in Facebook Login

## [7.21.2] - 2020-08-14

### Added

- Support customized Android Keystore path

### Fixed

- Fix iOS build issue for swift libraries
- Deprecate old GUI.ModalWindow API

## [7.21.1] - 2020-06-25

### Changed

- Bump native SDK version to 7.1.1

## [7.21.0] - 2020-06-20

### Added

- Introduce DataProcessingOptions

### Deprecated

- Remove UserProperties API

## [7.20.0] - 2020-06-15

### Changed

- Bump native SDK version to 7.0.1

### Added

- Add Express Login

## [7.19.2] - 2020-04-08

### Fixed

- Fix FeedShare issue: https://github.com/facebook/facebook-sdk-for-unity/issues/284
- Fix crash when AccessToken has no graph domain

## [7.19.1] - 2020-03-22

### Fixed

- Fix Login issue in Editor mode

## [7.19.0] - 2020-03-11

### Added

- Support for Gaming Video Uploads
- Support for Gaming Image Uploads
- Support for Gaming Friend Finder

## [7.18.1] - 2020-02-03

### Fixed

- Fix exported iOS project build issue
- Fix Android install referrer issue
- Fix GetUserID issue
- Remove AndroidJNIHelper log

### Changed

- Bump Google Playservices Resolver to v1.2.135

## [7.18.0] - 2019-10-11

### Fixed

- Fix Android logout issue
- Fix the issue of multiple AppRequest instances when changing screen orientation
- Fix GetUserID issue

### Changed

- Remove Bolts framework

## [7.17.2] - 2019-07-18

### Fixed

- Various bug fixes

## [7.17.1] - 2019-07-05

### Other

- Facebook Developer Docs: [Changelog v7.x](https://developers.facebook.com/docs/unity/change-log)

<!-- Links -->

[Unreleased]: https://github.com/facebook/facebook-sdk-for-unity/compare/sdk-version-11.0.0...HEAD
[11.0.0]: https://github.com/facebook/facebook-sdk-for-unity/compare/sdk-version-9.2.0...sdk-version-11.0.0
[9.2.0]: https://github.com/facebook/facebook-sdk-for-unity/compare/sdk-version-9.1.0...sdk-version-9.2.0
[9.1.0]: https://github.com/facebook/facebook-sdk-for-unity/compare/sdk-version-9.0.0...sdk-version-9.1.0
[9.0.0]: https://github.com/facebook/facebook-sdk-for-unity/compare/sdk-version-8.1.1...sdk-version-9.0.0
[8.1.1]: https://github.com/facebook/facebook-sdk-for-unity/compare/sdk-version-8.1.0...sdk-version-8.1.1
[8.1.0]: https://github.com/facebook/facebook-sdk-for-unity/compare/sdk-version-8.0.0...sdk-version-8.1.0
[8.0.0]: https://github.com/facebook/facebook-sdk-for-unity/compare/sdk-version-7.21.2...sdk-version-8.0.0
[7.21.2]: https://github.com/facebook/facebook-sdk-for-unity/compare/sdk-version-7.21.1...sdk-version-7.21.2
[7.21.1]: https://github.com/facebook/facebook-sdk-for-unity/compare/sdk-version-7.21.0...sdk-version-7.21.1
[7.21.0]: https://github.com/facebook/facebook-sdk-for-unity/compare/sdk-version-7.20.0...sdk-version-7.21.0
[7.20.0]: https://github.com/facebook/facebook-sdk-for-unity/compare/sdk-version-7.19.2...sdk-version-7.20.0
[7.19.2]: https://github.com/facebook/facebook-sdk-for-unity/compare/sdk-version-7.19.1...sdk-version-7.19.2
[7.19.1]: https://github.com/facebook/facebook-sdk-for-unity/compare/sdk-version-7.19.0...sdk-version-7.19.1
[7.19.0]: https://github.com/facebook/facebook-sdk-for-unity/compare/sdk-version-7.18.1...sdk-version-7.19.0
[7.18.1]: https://github.com/facebook/facebook-sdk-for-unity/compare/sdk-version-7.18.0...sdk-version-7.18.1
[7.18.0]: https://github.com/facebook/facebook-sdk-for-unity/compare/sdk-version-7.17.2...sdk-version-7.18.0
[7.17.2]: https://github.com/facebook/facebook-sdk-for-unity/compare/sdk-version-7.17.1...sdk-version-7.17.2
[7.17.1]: https://github.com/facebook/facebook-sdk-for-unity/compare/sdk-version-7.1.0...sdk-version-7.17.1
