using System;
using System.Runtime.InteropServices;
using AOT;
using UnityEngine;

namespace Agava.YandexGames
{
    /// <summary>
    /// Proxy for ysdk.adv.showRewardedVideo() method in YandexGames SDK.
    /// </summary>
    public static class VideoAd
    {
        // More statics to the god of statics.
        private static Action s_onOpenCallback;
        private static Action s_onRewardedCallback;
        private static Action s_onCloseCallback;
        private static Action<string> s_onErrorCallback;

        /// <summary>
        /// Shows the rewarded video ad.
        /// </summary>
        /// <remarks>
        /// Doesn't seem to have any call-per-minute limit (at the time of writing).
        /// </remarks>
        public static void Show(Action onOpenCallback = null, Action onRewardedCallback = null,
            Action onCloseCallback = null, Action<string> onErrorCallback = null)
        {
            // And this is where static fields backfire. Instant Karma.
            s_onOpenCallback = onOpenCallback;
            s_onRewardedCallback = onRewardedCallback;
            s_onCloseCallback = onCloseCallback;
            s_onErrorCallback = onErrorCallback;

            VideoAdShow(OnOpenCallback, OnRewardedCallback, OnCloseCallback, OnErrorCallback);
        }

        [DllImport("__Internal")]
        private static extern bool VideoAdShow(Action openCallback, Action rewardedCallback, Action closeCallback, Action<string> errorCallback);

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

        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void OnErrorCallback(string errorMessage)
        {
            if (YandexGamesSdk.CallbackLogging)
                Debug.Log($"{nameof(VideoAd)}.{nameof(OnErrorCallback)} invoked, {nameof(errorMessage)} = {errorMessage}");

            s_onErrorCallback?.Invoke(errorMessage);
        }
    }
}
