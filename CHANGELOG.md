### 8.0.1  
Fixes:  
- Prevent crashes when Yandex SDK API throws an empty error (or error of an unexpected type).  
  
### 8.0.0  
Breaking changes:  
- Added Agava prefix in front of all namespaces to comply with MSDN C# standard.  
  
### 7.0.0  
Breaking changes:  
- WebBackgroundMute.Enable was replaced with WebApplication.InBackground property. Now you will have to manually set `AudioListener.pause = WebApplication.InBackground;` in an Update to achieve the same effect. Although now you can use it for anything rather than just muting the audio in the background.  
  
### 6.0.0  
Breaking changes:  
- PlayerAccount.Authorized is now PlayerAccount.IsAuthorized. Needed that to achieve full naming consistency across internal and external API.  
- Same goes for YandexGamesSdk.IsInitialized.  
  
### 5.0.1  
Fixes:  
- Fixed Leaderboard.GetPlayerEntry successCallback silently stopping execution when it's supposed to return null.  
  
### 5.0.0  
Breaking changes:  
- Leaderboard.GetPlayerEntry now returns null in successCallback instead of invoking errorCallback.  
  
New features:  
- Added WebBackgroundMute to mute audio when RunInBackground is enabled.  
  
### 4.0.0  
Breaking changes:  
*To make it clear that GetProfileData still returns some data if permissions were not granted:*  
- Renamed RequestProfileDataPermission to RequestPersonalProfileDataPermission.  
- Renamed HasProfileDataPermission to HasPersonalProfileDataPermission.
