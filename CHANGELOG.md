# Changelog  
All notable changes to this project will be documented in this file.  
  
The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),  
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).  
  
## [16.1.0] - 2024-07-13  
### Fixed  
- Fixed saving JSON fields via `PlayerPrefs` cloud save utility.  
- `YandexGamesSdk.IsRunningOnYandex` now works when used within european Yandex Games reskin [Playhop](https://playhop.com/).  
- Added error callback when trying to save invalid JSON via `PlayerAccount.SetCloudSaveData()`.  
  
### Added  
- Added `CatalogProduct.priceCurrencyImage` for passing an IAP moderation.  
  
## [16.0.0] - 2023-08-11  
### Changed  
- Moved PlayerPrefs utility to its own `Agava.YandexGames.Utility` namespace to prevent unintentional naming clashes.  
  
## [15.1.1] - 2023-11-01  
### Fixed  
- Silent failure when invoking `YandexGamesSdk.Environment` property while SDK is not initialized.  
  
## [15.1.0] - 2023-10-17  
### Added  
- `YandexGamesSdk.IsRunningOnYandex` property to check whether you're using Build and Run or running the game on Yandex.  
  
## [15.0.0] - 2023-10-07  
### Added  
- `ReviewPopup.Open` and `CanOpen` methods for requesting user to rate your game.  
    
### Changed  
- `Shortcut.Suggest` now has a single callback with boolean indicating success.  
  
## [14.0.0] - 2023-10-06  
### Changed  
- `Shortcuts` class renamed to singular `Shortcut`.  
- `yandexGames.sdk.features.LoadingAPI.ready` method is no longer called automatically to prevent game moderation rejection.  
- `GameReady` class contents are moved to `YandexGamesSdk` class.  
  
### Added  
- Added `Shortcut` and `GameReady` testing buttons to playtesting canvas.  
  
## [13.3.0] - 2023-09-27  
### Added  
- Added GameReady API that allows to manually call `ysdk.features.LoadingAPI.ready()` when the game has finished loading.  
- Added desktop shortcut placement API.  
  
## [13.2.0] - 2023-07-31  
### Added  
- `Agava.YandexGames.PlayerPrefs` utility script for migrating from PlayerPrefs-based saves to `PlayerAccount.SetCloudSaveData`.  
  
## [13.1.0] - 2023-07-24  
### Added  
- `PlayerAccount.StartAuthorizationPolling()` method and `AuthorizedInBackground` event to detect background user login after initialization.  
  
## [13.0.0] - 2023-04-25  
### Changed  
- Billing methods no longer require authorization.  
  
### Removed  
- Removed Device.Type since it does not detect an iPad as a tablet.  
Proper device type detection is implemented in [WebUtility](https://github.com/forcepusher/com.agava.webutility) package.  
  
## [12.0.1] - 2023-04-06  
### Fixed  
- Fixed an exploit where disabling internet connection causes rewarded video ads to immediately grant a reward without showing an ad.  
  
## [12.0.0] - 2023-04-06  
### Added  
- Profile pictures now will be fetched with `PlayerAccount.GetProfileData()`, `Leaderboard.GetPlayerEntry` and `GetEntries` methods.  
Find profile picture URLs in `PlayerAccountProfileDataResponse.profilePicture` and download it via `RemoteImage` class.  
  
### Changed  
- `PlayerAccount.SetPlayerData` and `GetPlayerData` are renamed to `PlayerAccount.SetCloudSaveData` and `GetCloudSaveData`.  
- Removed redundant authorization requirement for these methods: `PlayerAccount.HasPersonalProfileDataPermission()`,  `RequestPersonalProfileDataPermission()`, `GetProfileData()`, `GetPlayerData()`, `SetPlayerData()`.  
  
## [11.2.1] - 2023-03-17  
### Fixed  
- The SDK now calls `ysdk.features.LoadingAPI.ready` method when initialization is complete.  
  
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
  
[16.1.0] https://github.com/forcepusher/com.agava.yandexgames/compare/16.0.0...16.1.0  
[16.0.0] https://github.com/forcepusher/com.agava.yandexgames/compare/15.1.1...16.0.0  
[15.1.1] https://github.com/forcepusher/com.agava.yandexgames/compare/15.1.0...15.1.1  
[15.1.0] https://github.com/forcepusher/com.agava.yandexgames/compare/15.0.0...15.1.0  
[15.0.0] https://github.com/forcepusher/com.agava.yandexgames/compare/14.0.0...15.0.0  
[14.0.0] https://github.com/forcepusher/com.agava.yandexgames/compare/13.3.0...14.0.0  
[13.3.0] https://github.com/forcepusher/com.agava.yandexgames/compare/13.2.0...13.3.0  
[13.2.0] https://github.com/forcepusher/com.agava.yandexgames/compare/13.1.0...13.2.0  
[13.1.0] https://github.com/forcepusher/com.agava.yandexgames/compare/13.0.0...13.1.0  
[13.0.0] https://github.com/forcepusher/com.agava.yandexgames/compare/12.0.1...13.0.0  
[12.0.1] https://github.com/forcepusher/com.agava.yandexgames/compare/12.0.0...12.0.1  
[12.0.0] https://github.com/forcepusher/com.agava.yandexgames/compare/11.2.1...12.0.0  
[11.2.1] https://github.com/forcepusher/com.agava.yandexgames/compare/11.2.0...11.2.1  
[11.2.0] https://github.com/forcepusher/com.agava.yandexgames/compare/11.1.0...11.2.0  
[11.1.0] https://github.com/forcepusher/com.agava.yandexgames/compare/11.0.0...11.1.0  
[11.0.0] https://github.com/forcepusher/com.agava.yandexgames/compare/10.0.0...11.0.0  
[10.0.0] https://github.com/forcepusher/com.agava.yandexgames/compare/8.3.1...10.0.0  
[8.3.1] https://github.com/forcepusher/com.agava.yandexgames/compare/8.3.0...8.3.1  
[8.3.0] https://github.com/forcepusher/com.agava.yandexgames/compare/8.2.0...8.3.0  
[8.2.0] https://github.com/forcepusher/com.agava.yandexgames/compare/8.1.0...8.2.0  
[8.1.0] https://github.com/forcepusher/com.agava.yandexgames/compare/8.0.1...8.1.0  
[8.0.1] https://github.com/forcepusher/com.agava.yandexgames/compare/8.0.0...8.0.1  
[8.0.0] https://github.com/forcepusher/com.agava.yandexgames/compare/7.0.0...8.0.0
