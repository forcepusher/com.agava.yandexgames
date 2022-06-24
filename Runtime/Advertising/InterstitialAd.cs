using System;
using System.Runtime.InteropServices;
using AOT;
using UnityEngine;

namespace Agava.YandexGames
{
    /// <summary>
    /// Proxy for ysdk.adv.showFullscreenAdv() method in YandexGames SDK.
    /// </summary>
    public static class InterstitialAd
    {
        // Mutable static fields - absolutely disgusting.
        private static Action s_onOpenCallback;
        private static Action<bool> s_onCloseCallback;
        private static Action<string> s_onErrorCallback;
        private static Action s_onOfflineCallback;

        /// <summary>
        /// Shows the fullscreen ad banner.
        /// </summary>
        /// <remarks>
        /// Could be used only once every 2 minutes (at the time of writing).
        /// Otherwise calls onCloseCallback instantaneously.
        /// </remarks>
        public static void Show(Action onOpenCallback = null, Action<bool> onCloseCallback = null,
            Action<string> onErrorCallback = null, Action onOfflineCallback = null)
        {
            // Let's pretend you didn't see this.
            s_onOpenCallback = onOpenCallback;
            s_onCloseCallback = onCloseCallback;
            s_onErrorCallback = onErrorCallback;
            s_onOfflineCallback = onOfflineCallback;

            InterstitialAdShow(OnOpenCallback, OnCloseCallback, OnErrorCallback, OnOfflineCallback);
        }

        [DllImport("__Internal")]
        private static extern bool InterstitialAdShow(Action openCallback, Action<bool> closeCallback, Action<string> errorCallback, Action offlineCallback);

        [MonoPInvokeCallback(typeof(Action))]
        private static void OnOpenCallback()
        {
            if (YandexGamesSdk.CallbackLogging)
                Debug.Log($"{nameof(InterstitialAd)}.{nameof(OnOpenCallback)} invoked");

            s_onOpenCallback?.Invoke();
        }

        [MonoPInvokeCallback(typeof(Action<bool>))]
        private static void OnCloseCallback(bool wasShown)
        {
            if (YandexGamesSdk.CallbackLogging)
                Debug.Log($"{nameof(InterstitialAd)}.{nameof(OnCloseCallback)} invoked, {nameof(wasShown)} = {wasShown}");

            s_onCloseCallback?.Invoke(wasShown);
        }

        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void OnErrorCallback(string errorMessage)
        {
            if (YandexGamesSdk.CallbackLogging)
                Debug.Log($"{nameof(InterstitialAd)}.{nameof(OnErrorCallback)} invoked, {nameof(errorMessage)} = {errorMessage}");

            s_onErrorCallback?.Invoke(errorMessage);
        }

        [MonoPInvokeCallback(typeof(Action))]
        private static void OnOfflineCallback()
        {
            if (YandexGamesSdk.CallbackLogging)
                Debug.Log($"{nameof(InterstitialAd)}.{nameof(OnOfflineCallback)} invoked");

            s_onOfflineCallback?.Invoke();
        }
    }
}
