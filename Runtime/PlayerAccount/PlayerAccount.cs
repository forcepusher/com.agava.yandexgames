using System;
using System.Runtime.InteropServices;
using AOT;
using UnityEngine;

namespace Agava.YandexGames
{
    /// <summary>
    /// Proxy for player-related methods in YandexGames SDK.
    /// </summary>
    public static class PlayerAccount
    {
        public static event Action AuthorizedInBackground;

        private static Action s_onStartAuthorizationPollingSuccessCallback;
        private static Action s_onStartAuthorizationPollingErrorCallback;

        private static Action s_onAuthorizeSuccessCallback;
        private static Action<string> s_onAuthorizeErrorCallback;

        private static Action s_onRequestPersonalProfileDataPermissionSuccessCallback;
        private static Action<string> s_onRequestPersonalProfileDataPermissionErrorCallback;

        private static Action<PlayerAccountProfileDataResponse> s_onGetProfileDataSuccessCallback;
        private static Action<string> s_onGetProfileDataErrorCallback;

        private static Action s_onSetCloudSaveDataSuccessCallback;
        private static Action<string> s_onSetCloudSaveDataErrorCallback;

        private static Action<string> s_onGetCloudSaveDataSuccessCallback;
        private static Action<string> s_onGetCloudSaveDataErrorCallback;

        /// <summary>
        /// Use this before calling SDK methods that require authorization.
        /// </summary>
        public static bool IsAuthorized => GetPlayerAccountIsAuthorized();

        [DllImport("__Internal")]
        private static extern bool GetPlayerAccountIsAuthorized();

        /// <summary>
        /// Permission to use name and profile picture from the Yandex account.
        /// </summary>
        /// <remarks>
        /// Requires authorization. Use <see cref="IsAuthorized"/> and <see cref="Authorize"/>.
        /// </remarks>
        public static bool HasPersonalProfileDataPermission => GetPlayerAccountHasPersonalProfileDataPermission();

        [DllImport("__Internal")]
        private static extern bool GetPlayerAccountHasPersonalProfileDataPermission();

        #region Authorize
        /// <summary>
        /// Calls a scary authorization window upon the user. Be very afraid.
        /// </summary>
        public static void Authorize(Action onSuccessCallback = null, Action<string> onErrorCallback = null)
        {
            s_onAuthorizeSuccessCallback = onSuccessCallback;
            s_onAuthorizeErrorCallback = onErrorCallback;

            PlayerAccountAuthorize(OnAuthorizeSuccessCallback, OnAuthorizeErrorCallback);
        }

        [DllImport("__Internal")]
        private static extern void PlayerAccountAuthorize(Action successCallback, Action<string> errorCallback);

        [MonoPInvokeCallback(typeof(Action))]
        private static void OnAuthorizeSuccessCallback()
        {
            if (YandexGamesSdk.CallbackLogging)
                Debug.Log($"{nameof(PlayerAccount)}.{nameof(OnAuthorizeSuccessCallback)} invoked");

            s_onAuthorizeSuccessCallback?.Invoke();
        }

        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void OnAuthorizeErrorCallback(string errorMessage)
        {
            if (YandexGamesSdk.CallbackLogging)
                Debug.Log($"{nameof(PlayerAccount)}.{nameof(OnAuthorizeErrorCallback)} invoked, {nameof(errorMessage)} = {errorMessage}");

            s_onAuthorizeErrorCallback?.Invoke(errorMessage);
        }

        public static void StartAuthorizationPolling(int delay, Action successCallback = null, Action errorCallback = null)
        {
            s_onStartAuthorizationPollingSuccessCallback = successCallback;
            s_onStartAuthorizationPollingErrorCallback = errorCallback;

            PlayerAccountStartAuthorizationPolling(delay, OnStartAuthorizationPollingSuccessCallback, OnStartAuthorizationPollingErrorCallback);
        }

        [DllImport("__Internal")]
        private static extern void PlayerAccountStartAuthorizationPolling(int cooldown, Action successCallback, Action errorCallback);

        [MonoPInvokeCallback(typeof(Action))]
        private static void OnStartAuthorizationPollingSuccessCallback()
        {
            if (YandexGamesSdk.CallbackLogging)
                Debug.Log($"{nameof(PlayerAccount)}.{nameof(OnStartAuthorizationPollingSuccessCallback)} invoked");

            AuthorizedInBackground?.Invoke();
            s_onStartAuthorizationPollingSuccessCallback?.Invoke();
        }

        [MonoPInvokeCallback(typeof(Action))]
        private static void OnStartAuthorizationPollingErrorCallback()
        {
            if (YandexGamesSdk.CallbackLogging)
                Debug.Log($"{nameof(PlayerAccount)}.{nameof(OnStartAuthorizationPollingErrorCallback)} invoked");

            s_onStartAuthorizationPollingErrorCallback?.Invoke();
        }
        #endregion

        #region RequestPersonalProfileDataPermission
        /// <summary>
        /// Request the permission to get Yandex account name and profile picture when calling <see cref="GetProfileData"/>.
        /// </summary>
        /// <remarks>
        /// Be aware, if user rejects the request - it's permanent. The request window will never open again.<br/>
        /// Requires authorization. Use <see cref="IsAuthorized"/> and <see cref="Authorize"/>.
        /// </remarks>
        public static void RequestPersonalProfileDataPermission(Action onSuccessCallback = null, Action<string> onErrorCallback = null)
        {
            s_onRequestPersonalProfileDataPermissionSuccessCallback = onSuccessCallback;
            s_onRequestPersonalProfileDataPermissionErrorCallback = onErrorCallback;

            PlayerAccountRequestPersonalProfileDataPermission(OnRequestPersonalProfileDataPermissionSuccessCallback, OnRequestPersonalProfileDataPermissionErrorCallback);
        }

        [DllImport("__Internal")]
        private static extern void PlayerAccountRequestPersonalProfileDataPermission(Action successCallback, Action<string> errorCallback);

        [MonoPInvokeCallback(typeof(Action))]
        private static void OnRequestPersonalProfileDataPermissionSuccessCallback()
        {
            if (YandexGamesSdk.CallbackLogging)
                Debug.Log($"{nameof(PlayerAccount)}.{nameof(OnRequestPersonalProfileDataPermissionSuccessCallback)} invoked");

            s_onRequestPersonalProfileDataPermissionSuccessCallback?.Invoke();
        }

        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void OnRequestPersonalProfileDataPermissionErrorCallback(string errorMessage)
        {
            if (YandexGamesSdk.CallbackLogging)
                Debug.Log($"{nameof(PlayerAccount)}.{nameof(OnRequestPersonalProfileDataPermissionErrorCallback)} invoked, {nameof(errorMessage)} = {errorMessage}");

            s_onRequestPersonalProfileDataPermissionErrorCallback?.Invoke(errorMessage);
        }
        #endregion

        #region GetProfileData
        /// <summary>
        /// Will only return <see cref="PlayerAccountProfileDataResponse.uniqueID"/> unless <see cref="HasPersonalProfileDataPermission"/>.
        /// </summary>
        /// <remarks>
        /// Requires authorization. Use <see cref="IsAuthorized"/> and <see cref="Authorize"/>.
        /// </remarks>
        public static void GetProfileData(Action<PlayerAccountProfileDataResponse> onSuccessCallback, Action<string> onErrorCallback = null, ProfilePictureSize pictureSize = ProfilePictureSize.medium)
        {
            s_onGetProfileDataSuccessCallback = onSuccessCallback;
            s_onGetProfileDataErrorCallback = onErrorCallback;

            PlayerAccountGetProfileData(OnGetProfileDataSuccessCallback, OnGetProfileDataErrorCallback, pictureSize.ToString());
        }

        [DllImport("__Internal")]
        private static extern void PlayerAccountGetProfileData(Action<string> successCallback, Action<string> errorCallback, string pictureSize);

        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void OnGetProfileDataSuccessCallback(string profileDataResponseJson)
        {
            if (YandexGamesSdk.CallbackLogging)
                Debug.Log($"{nameof(PlayerAccount)}.{nameof(OnGetProfileDataSuccessCallback)} invoked, {nameof(profileDataResponseJson)} = {profileDataResponseJson}");

            PlayerAccountProfileDataResponse profileDataResponse = JsonUtility.FromJson<PlayerAccountProfileDataResponse>(profileDataResponseJson);

            s_onGetProfileDataSuccessCallback?.Invoke(profileDataResponse);
        }

        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void OnGetProfileDataErrorCallback(string errorMessage)
        {
            if (YandexGamesSdk.CallbackLogging)
                Debug.Log($"{nameof(PlayerAccount)}.{nameof(OnGetProfileDataErrorCallback)} invoked, {nameof(errorMessage)} = {errorMessage}");

            s_onGetProfileDataErrorCallback?.Invoke(errorMessage);
        }
        #endregion

        #region Cloud Save
        /// <summary>
        /// Stores Cloud Save data in the player account. Proxy for player.setData() with "flush" setting set to true.
        /// </summary>
        public static void SetCloudSaveData(string cloudSaveDataJson, Action onSuccessCallback = null, Action<string> onErrorCallback = null)
        {
            if (cloudSaveDataJson == null)
                throw new ArgumentNullException(nameof(cloudSaveDataJson));

            if (string.IsNullOrEmpty(cloudSaveDataJson))
                cloudSaveDataJson = "{}";

            s_onSetCloudSaveDataSuccessCallback = onSuccessCallback;
            s_onSetCloudSaveDataErrorCallback = onErrorCallback;

            PlayerAccountSetCloudSaveData(cloudSaveDataJson, OnSetCloudSaveDataSuccessCallback, OnSetCloudSaveDataErrorCallback);
        }

        [DllImport("__Internal")]
        private static extern void PlayerAccountSetCloudSaveData(string cloudSaveDataJson, Action successCallback, Action<string> errorCallback);

        [MonoPInvokeCallback(typeof(Action))]
        private static void OnSetCloudSaveDataSuccessCallback()
        {
            if (YandexGamesSdk.CallbackLogging)
                Debug.Log($"{nameof(PlayerAccount)}.{nameof(OnSetCloudSaveDataSuccessCallback)} invoked");

            s_onSetCloudSaveDataSuccessCallback?.Invoke();
        }

        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void OnSetCloudSaveDataErrorCallback(string errorMessage)
        {
            if (YandexGamesSdk.CallbackLogging)
                Debug.Log($"{nameof(PlayerAccount)}.{nameof(OnSetCloudSaveDataErrorCallback)} invoked, {nameof(errorMessage)} = {errorMessage}");

            s_onSetCloudSaveDataErrorCallback?.Invoke(errorMessage);
        }

        /// <summary>
        /// Loads cloud save data, proxy for player.getData().
        /// </summary>
        /// <param name="onSuccessCallback">
        /// Callback that returns unparsed JSON string.<br/>
        /// If player does not have any data saved, an empty JSON "{}" is returned.
        /// </param>
        public static void GetCloudSaveData(Action<string> onSuccessCallback = null, Action<string> onErrorCallback = null)
        {
            s_onGetCloudSaveDataSuccessCallback = onSuccessCallback;
            s_onGetCloudSaveDataErrorCallback = onErrorCallback;

            PlayerAccountGetCloudSaveData(OnGetCloudSaveDataSuccessCallback, OnGetCloudSaveDataErrorCallback);
        }

        [DllImport("__Internal")]
        private static extern void PlayerAccountGetCloudSaveData(Action<string> successCallback, Action<string> errorCallback);

        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void OnGetCloudSaveDataSuccessCallback(string playerDataJson)
        {
            if (YandexGamesSdk.CallbackLogging)
                Debug.Log($"{nameof(PlayerAccount)}.{nameof(OnGetCloudSaveDataSuccessCallback)} invoked, {nameof(playerDataJson)} = {playerDataJson}");

            s_onGetCloudSaveDataSuccessCallback?.Invoke(playerDataJson);
        }

        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void OnGetCloudSaveDataErrorCallback(string errorMessage)
        {
            if (YandexGamesSdk.CallbackLogging)
                Debug.Log($"{nameof(PlayerAccount)}.{nameof(OnGetCloudSaveDataErrorCallback)} invoked, {nameof(errorMessage)} = {errorMessage}");

            s_onGetCloudSaveDataErrorCallback?.Invoke(errorMessage);
        }
        #endregion
    }
}
