# Changelog  
All notable changes to this project will be documented in this file.  
  
The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),  
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).  
  
## [11.2.0] - 2023-02-09  
### Added  
- `Billing` class for In-App purchases.  
  
## [11.1.0] - 2023-01-20  
### Added  
- `StickyAd.Show()` and `StickyAd.Hide` methods for controlling StickyAd visibility at run time.  
  
## [11.0.0] - 2022-08-29  
### Removed  
- WebApplication and WebEventSystem are now split into a separate [WebUtility](https://github.com/forcepusher/com.agava.webutility) package.  
  
## [10.0.0] - 2022-08-10  
### Added  
- YandexGamesSdk.Initialize() now has onSuccessCallback and returns a coroutine waiting for initialization to finish.  
  
### Removed  
- YandexGamesSdk.WaitForInitialization() is gone. Yield on YandexGamesSdk.Initialize() coroutine instead.  
  
### Changed  
- The SDK is no longer initializing itself because developers often forget to use WaitForInitialization() coroutine.  
This was leading to errors that were difficult to reproduce and detect. Now you have to call YandexGamesSdk.Initialize() on your own.  
  
## [9.0.0] - 2022-07-22  
### Fixed  
- Interstitial misspelling.  
  
## [8.3.1] - 2022-06-08  
### Fixed  
- PlayerAccount.IsAuthorized was broken due to recent YandexGames API changes. This update solves it.  
  
## [8.3.0] - 2022-06-03  
### Added  
- YandexGamesSdk.Environment data structure that proxies `ysdk.environment`.  
  
## [8.2.0] - 2022-02-27  
### Added  
- Device.Type property that proxies `ysdk.deviceInfo.type`.  
  
## [8.1.0] - 2022-02-25  
### Added  
- PlayerAccount.GetPlayerData and SetPlayerData that implements Cloud Save functionality.  
  
## [8.0.1] - 2022-02-08  
### Fixed  
- Prevent crashes when Yandex SDK API throws an empty error (or error of an unexpected type).  
  
## [8.0.0] - 2022-01-24  
### Changed  
- Added Agava prefix in front of all namespaces to comply with MSDN C# standard.  
  
## [7.0.0] - 2021-12-24  
### Changed  
- WebBackgroundMute.Enable was replaced with WebApplication.InBackground property. Now you will have to manually set `AudioListener.pause = WebApplication.InBackground;` in an Update to achieve the same effect. Although now you can use it for anything rather than just muting the audio in the background.  
  
## [6.0.0] - 2021-12-15  
### Changed  
- PlayerAccount.Authorized is now PlayerAccount.IsAuthorized. Needed that to achieve full naming consistency across internal and external API.  
- Same goes for YandexGamesSdk.IsInitialized.  
  
## [5.0.1] - 2021-12-15  
### Fixed  
- Leaderboard.GetPlayerEntry successCallback silently stopping execution when it's supposed to return null.  
  
## [5.0.0] - 2021-12-14  
### Changed  
- Leaderboard.GetPlayerEntry now returns null in successCallback instead of invoking errorCallback.  
  
### Added  
- WebBackgroundMute to mute audio when RunInBackground is enabled.  
  
## [4.0.0] - 2021-12-13  
### Changed  
- Making it clear that GetProfileData still returns some data if permissions were not granted.  
- Renamed RequestProfileDataPermission to RequestPersonalProfileDataPermission.  
- Renamed HasProfileDataPermission to HasPersonalProfileDataPermission.  
  
[11.2.0] https://github.com/forcepusher/com.agava.yandexgames/compare/11.1.0...11.2.0  
[11.1.0] https://github.com/forcepusher/com.agava.yandexgames/compare/11.0.0...11.1.0  
[11.0.0] https://github.com/forcepusher/com.agava.yandexgames/compare/10.0.0...11.0.0  
[10.0.0] https://github.com/forcepusher/com.agava.yandexgames/compare/9.0.0...10.0.0  
[9.0.0] https://github.com/forcepusher/com.agava.yandexgames/compare/8.3.1...9.0.0  
[8.3.1] https://github.com/forcepusher/com.agava.yandexgames/compare/8.3.0...8.3.1  
[8.3.0] https://github.com/forcepusher/com.agava.yandexgames/compare/8.2.0...8.3.0  
[8.2.0] https://github.com/forcepusher/com.agava.yandexgames/compare/8.1.0...8.2.0  
[8.1.0] https://github.com/forcepusher/com.agava.yandexgames/compare/8.0.1...8.1.0  
[8.0.1] https://github.com/forcepusher/com.agava.yandexgames/compare/8.0.0...8.0.1  
[8.0.0] https://github.com/forcepusher/com.agava.yandexgames/compare/7.0.0...8.0.0  
[7.0.0] https://github.com/forcepusher/com.agava.yandexgames/compare/6.0.0...7.0.0  
[6.0.0] https://github.com/forcepusher/com.agava.yandexgames/compare/5.0.1...6.0.0  
[5.0.1] https://github.com/forcepusher/com.agava.yandexgames/compare/5.0.0...5.0.1  
[5.0.0] https://github.com/forcepusher/com.agava.yandexgames/compare/4.0.0...5.0.0  
[4.0.0] https://github.com/forcepusher/com.agava.yandexgames/compare/3.0.0...4.0.0
