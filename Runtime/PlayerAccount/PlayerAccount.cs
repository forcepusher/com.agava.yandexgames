using System;
using System.Runtime.InteropServices;
using AOT;
using UnityEngine;
using YandexGames.Utility;

namespace YandexGames
{
    /// <summary>
    /// Proxy for player-related methods in YandexGames SDK.
    /// </summary>
    public static class PlayerAccount
    {
        private static Action s_onAuthorizeSuccessCallback;
        private static Action<string> s_onAuthorizeErrorCallback;
        private static Action s_onRequestPersonalProfileDataPermissionSuccessCallback;
        private static Action<string> s_onRequestPersonalProfileDataPermissionErrorCallback;
        private static Action<PlayerAccountProfileDataResponse> s_onGetProfileDataSuccessCallback;
        private static Action<string> s_onGetProfileDataErrorCallback;

        /// <summary>
        /// Use this before calling SDK methods that require authorization.
        /// </summary>
        public static bool Authorized => CheckAuthorization();

        [DllImport("__Internal")]
        private static extern bool CheckAuthorization();

        /// <summary>
        /// Permission to use name and profile picture from the Yandex account.
        /// </summary>
        /// <remarks>
        /// Requires authorization. Use <see cref="Authorized"/> and <see cref="Authorize"/>.
        /// </remarks>
        public static bool HasPersonalProfileDataPermission => CheckPersonalProfileDataPermission();

        [DllImport("__Internal")]
        private static extern bool CheckPersonalProfileDataPermission();

        #region Authorize
        /// <summary>
        /// Calls a scary authorization window upon the user. Be very afraid.
        /// </summary>
        public static void Authorize(Action onSuccessCallback = null, Action<string> onErrorCallback = null)
        {
            s_onAuthorizeSuccessCallback = onSuccessCallback;
            s_onAuthorizeErrorCallback = onErrorCallback;

            Authorize(OnAuthorizeSuccessCallback, OnAuthorizeErrorCallback);
        }

        [DllImport("__Internal")]
        private static extern void Authorize(Action successCallback, Action<IntPtr, int> errorCallback);

        [MonoPInvokeCallback(typeof(Action))]
        private static void OnAuthorizeSuccessCallback()
        {
            if (YandexGamesSdk.CallbackLogging)
                Debug.Log($"{nameof(PlayerAccount)}.{nameof(OnAuthorizeSuccessCallback)} invoked");

            s_onAuthorizeSuccessCallback?.Invoke();
        }

        [MonoPInvokeCallback(typeof(Action<IntPtr, int>))]
        private static void OnAuthorizeErrorCallback(IntPtr errorMessageBufferPtr, int errorMessageBufferLength)
        {
            string errorMessage = new UnmanagedString(errorMessageBufferPtr, errorMessageBufferLength).ToString();

            if (YandexGamesSdk.CallbackLogging)
                Debug.Log($"{nameof(PlayerAccount)}.{nameof(OnAuthorizeErrorCallback)} invoked, {nameof(errorMessage)} = {errorMessage}");

            s_onAuthorizeErrorCallback?.Invoke(errorMessage);
        }
        #endregion

        #region RequestPersonalProfileDataPermission
        /// <summary>
        /// Request the permission to get Yandex account name and profile picture when calling <see cref="GetProfileData"/>.
        /// </summary>
        /// <remarks>
        /// Be aware, if user rejects the request - it's permanent. The request window will never open again.<br/>
        /// Requires authorization. Use <see cref="Authorized"/> and <see cref="Authorize"/>.
        /// </remarks>
        public static void RequestPersonalProfileDataPermission(Action onSuccessCallback = null, Action<string> onErrorCallback = null)
        {
            s_onRequestPersonalProfileDataPermissionSuccessCallback = onSuccessCallback;
            s_onRequestPersonalProfileDataPermissionErrorCallback = onErrorCallback;

            RequestPersonalProfileDataPermission(OnRequestPersonalProfileDataPermissionSuccessCallback, OnRequestPersonalProfileDataPermissionErrorCallback);
        }

        [DllImport("__Internal")]
        private static extern void RequestPersonalProfileDataPermission(Action successCallback, Action<IntPtr, int> errorCallback);

        [MonoPInvokeCallback(typeof(Action))]
        private static void OnRequestPersonalProfileDataPermissionSuccessCallback()
        {
            if (YandexGamesSdk.CallbackLogging)
                Debug.Log($"{nameof(PlayerAccount)}.{nameof(OnRequestPersonalProfileDataPermissionSuccessCallback)} invoked");

            s_onRequestPersonalProfileDataPermissionSuccessCallback?.Invoke();
        }

        [MonoPInvokeCallback(typeof(Action<IntPtr, int>))]
        private static void OnRequestPersonalProfileDataPermissionErrorCallback(IntPtr errorMessageBufferPtr, int errorMessageBufferLength)
        {
            string errorMessage = new UnmanagedString(errorMessageBufferPtr, errorMessageBufferLength).ToString();

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
        /// Requires authorization. Use <see cref="Authorized"/> and <see cref="Authorize"/>.
        /// </remarks>
        public static void GetProfileData(Action<PlayerAccountProfileDataResponse> onSuccessCallback, Action<string> onErrorCallback = null)
        {
            s_onGetProfileDataSuccessCallback = onSuccessCallback;
            s_onGetProfileDataErrorCallback = onErrorCallback;

            GetProfileData(OnGetProfileDataSuccessCallback, OnGetProfileDataErrorCallback);
        }

        [DllImport("__Internal")]
        private static extern void GetProfileData(Action<IntPtr, int> successCallback, Action<IntPtr, int> errorCallback);

        [MonoPInvokeCallback(typeof(Action<IntPtr, int>))]
        private static void OnGetProfileDataSuccessCallback(IntPtr entryMessageBufferPtr, int entryMessageBufferLength)
        {
            string profileDataResponseJson = new UnmanagedString(entryMessageBufferPtr, entryMessageBufferLength).ToString();

            if (YandexGamesSdk.CallbackLogging)
                Debug.Log($"{nameof(PlayerAccount)}.{nameof(OnGetProfileDataSuccessCallback)} invoked, {nameof(profileDataResponseJson)} = {profileDataResponseJson}");

            PlayerAccountProfileDataResponse profileDataResponse = JsonUtility.FromJson<PlayerAccountProfileDataResponse>(profileDataResponseJson);

            s_onGetProfileDataSuccessCallback?.Invoke(profileDataResponse);
        }

        [MonoPInvokeCallback(typeof(Action<IntPtr, int>))]
        private static void OnGetProfileDataErrorCallback(IntPtr errorMessageBufferPtr, int errorMessageBufferLength)
        {
            string errorMessage = new UnmanagedString(errorMessageBufferPtr, errorMessageBufferLength).ToString();

            if (YandexGamesSdk.CallbackLogging)
                Debug.Log($"{nameof(PlayerAccount)}.{nameof(OnGetProfileDataErrorCallback)} invoked, {nameof(errorMessage)} = {errorMessage}");

            s_onGetProfileDataErrorCallback?.Invoke(errorMessage);
        }
        #endregion
    }
}
