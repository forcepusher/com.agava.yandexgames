using System;
using System.Runtime.InteropServices;
using AOT;
using UnityEngine;
using YandexGames.Utility;

namespace YandexGames
{
    /// <summary>
    /// Proxy for ysdk.adv.showRewardedVideo() method in YandexGames SDK.
    /// </summary>
    public static class VideoAd
    {
        // Mutable static fields - absolutely disgusting.
        private static Action s_onOpenCallback;
        private static Action s_onRewardedCallback;
        private static Action s_onCloseCallback;
        private static Action<string> s_onErrorCallback;

        public static void Show(Action onOpenCallback = null, Action onRewardedCallback = null,
            Action onCloseCallback = null, Action<string> onErrorCallback = null)
        {
            // Let's pretend you didn't see this.
            s_onOpenCallback = onOpenCallback;
            s_onRewardedCallback = onRewardedCallback;
            s_onCloseCallback = onCloseCallback;
            s_onErrorCallback = onErrorCallback;

            ShowVideoAd(OnOpenCallback, OnRewardedCallback, OnCloseCallback, OnErrorCallback);
        }

        [DllImport("__Internal")]
        private static extern bool ShowVideoAd(Action openCallback, Action rewardedCallback, Action closeCallback, Action<IntPtr, int> errorCallback);

        [MonoPInvokeCallback(typeof(Action))]
        private static void OnOpenCallback()
        {
            if (YandexGamesSdk.CallbackLogging)
                Debug.Log($"{nameof(VideoAd)}.{nameof(OnOpenCallback)} invoked");

            s_onOpenCallback?.Invoke();
        }

        [MonoPInvokeCallback(typeof(Action))]
        private static void OnRewardedCallback()
        {
            if (YandexGamesSdk.CallbackLogging)
                Debug.Log($"{nameof(VideoAd)}.{nameof(OnRewardedCallback)} invoked");

            s_onRewardedCallback?.Invoke();
        }

        [MonoPInvokeCallback(typeof(Action))]
        private static void OnCloseCallback()
        {
            if (YandexGamesSdk.CallbackLogging)
                Debug.Log($"{nameof(VideoAd)}.{nameof(OnCloseCallback)} invoked");

            s_onCloseCallback?.Invoke();
        }

        [MonoPInvokeCallback(typeof(Action<IntPtr, int>))]
        private static void OnErrorCallback(IntPtr errorMessageBufferPtr, int errorMessageBufferLength)
        {
            string errorMessage = new UnmanagedString(errorMessageBufferPtr, errorMessageBufferLength).ToString();

            if (YandexGamesSdk.CallbackLogging)
                Debug.Log($"{nameof(VideoAd)}.{nameof(OnErrorCallback)} invoked, {nameof(errorMessage)} = {errorMessage}");

            s_onErrorCallback?.Invoke(errorMessage);
        }
    }
}
