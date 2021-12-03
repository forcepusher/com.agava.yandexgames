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
        private static Action s_onAuthenticatedCallback;
        private static Action<string> s_onErrorCallback;

        /// <summary>
        /// Use this before calling SDK methods that require authorization,
        /// so you can avoid unexpected authorization window popups.
        /// </summary>
        public static bool IsAuthorized => VerifyPlayerAccountAuthorization();

        [DllImport("__Internal")]
        private static extern bool VerifyPlayerAccountAuthorization();

        /// <remarks>
        /// Use <see cref="IsAuthorized"/> to avoid automatic authorization window popup.
        /// </remarks>
        public static void Authenticate(bool requestPermissions, Action onAuthenticatedCallback = null,
            Action<string> onErrorCallback = null)
        {
            s_onAuthenticatedCallback = onAuthenticatedCallback;
            s_onErrorCallback = onErrorCallback;

            AuthenticatePlayerAccount(requestPermissions, OnAuthenticatedCallback, OnAuthenticationErrorCallback);
        }

        [DllImport("__Internal")]
        private static extern void AuthenticatePlayerAccount(bool requestPermissions, Action onAuthenticatedCallback, Action<IntPtr, int> errorCallback);

        [MonoPInvokeCallback(typeof(Action))]
        private static void OnAuthenticatedCallback()
        {
            if (YandexGamesSdk.CallbackLogging)
                Debug.Log($"{nameof(PlayerAccount)}.{nameof(OnAuthenticatedCallback)} invoked");

            s_onAuthenticatedCallback?.Invoke();
        }

        [MonoPInvokeCallback(typeof(Action<IntPtr, int>))]
        private static void OnAuthenticationErrorCallback(IntPtr errorMessageBufferPtr, int errorMessageBufferLength)
        {
            string errorMessage = new UnmanagedString(errorMessageBufferPtr, errorMessageBufferLength).ToString();

            if (YandexGamesSdk.CallbackLogging)
                Debug.Log($"{nameof(PlayerAccount)}.{nameof(OnAuthenticationErrorCallback)} invoked, {nameof(errorMessage)} = {errorMessage}");

            s_onErrorCallback?.Invoke(errorMessage);
        }
    }
}
