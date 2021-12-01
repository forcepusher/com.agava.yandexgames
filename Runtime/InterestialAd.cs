using System;
using System.Runtime.InteropServices;
using AOT;
using UnityEngine;
using YandexGames.Utility;

namespace YandexGames
{
    /// <summary>
    /// Proxy for ysdk.adv.showFullscreenAdv() method in YandexGames SDK.
    /// </summary>
    public static class InterestialAd
    {
        // Mutable static fields - absolutely disgusting.
        private static Action s_onOpenCallback;
        private static Action<bool> s_onCloseCallback;
        private static Action<string> s_onErrorCallback;
        private static Action s_onOfflineCallback;

        public static void Show(Action onOpenCallback = null, Action<bool> onCloseCallback = null,
            Action<string> onErrorCallback = null, Action onOfflineCallback = null)
        {
            // Let's pretend you didn't see this.
            s_onOpenCallback = onOpenCallback;
            s_onCloseCallback = onCloseCallback;
            s_onErrorCallback = onErrorCallback;
            s_onOfflineCallback = onOfflineCallback;

            ShowInterestialAd(OnOpenCallback, OnCloseCallback, OnErrorCallback, OnOfflineCallback);
        }

        [DllImport("__Internal")]
        private static extern bool ShowInterestialAd(Action openCallback, Action<bool> closeCallback, Action<IntPtr, int> errorCallback, Action offlineCallback);

        [MonoPInvokeCallback(typeof(Action))]
        private static void OnOpenCallback()
        {
            if (YandexGamesSdk.CallbackLogging)
                Debug.Log($"{nameof(InterestialAd)}.{nameof(OnOpenCallback)} invoked");

            s_onOpenCallback?.Invoke();
        }

        [MonoPInvokeCallback(typeof(Action<bool>))]
        private static void OnCloseCallback(bool wasShown)
        {
            if (YandexGamesSdk.CallbackLogging)
                Debug.Log($"{nameof(InterestialAd)}.{nameof(OnCloseCallback)} invoked, wasShown = {wasShown}");

            s_onCloseCallback?.Invoke(wasShown);
        }

        [MonoPInvokeCallback(typeof(Action<IntPtr, int>))]
        private static void OnErrorCallback(IntPtr errorMessageBufferPtr, int errorMessageBufferLength)
        {
            string errorMessage = new StringBuffer(errorMessageBufferPtr, errorMessageBufferLength).ToString();

            if (YandexGamesSdk.CallbackLogging)
                Debug.Log($"{nameof(InterestialAd)}.{nameof(OnErrorCallback)} invoked, errorMessage = {errorMessage}");

            s_onErrorCallback?.Invoke(errorMessage);
        }

        [MonoPInvokeCallback(typeof(Action))]
        private static void OnOfflineCallback()
        {
            if (YandexGamesSdk.CallbackLogging)
                Debug.Log($"{nameof(InterestialAd)}.{nameof(OnOfflineCallback)} invoked");

            s_onOfflineCallback?.Invoke();
        }
    }
}
