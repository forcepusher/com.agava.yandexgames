using System;
using System.Collections;
using System.Runtime.InteropServices;
using AOT;
using UnityEngine;

namespace Agava.YandexGames
{
    public static class YandexGamesSdk
    {
        private static Action s_onInitializeSuccessCallback;
        private static bool s_notifyLoadingCompletedOnInitialize;

        /// <summary>
        /// Enable it to log SDK callbacks in the console.
        /// </summary>
        public static bool CallbackLogging = false;

        /// <summary>
        /// SDK is initialized automatically on load.
        /// If either something fails or called way too early, this will return false.
        /// </summary>
        public static bool IsInitialized => GetYandexGamesSdkIsInitialized();

        [DllImport("__Internal")]
        private static extern bool GetYandexGamesSdkIsInitialized();

        public static YandexGamesEnvironment Environment
        {
            get
            {
                string environmentJson = GetYandexGamesSdkEnvironment();
                return JsonUtility.FromJson<YandexGamesEnvironment>(environmentJson);
            }
        }

        [DllImport("__Internal")]
        private static extern string GetYandexGamesSdkEnvironment();

        /// <summary>
        /// Invoke this and wait for coroutine to finish before using any SDK methods.<br/>
        /// Downloads Yandex SDK script and inserts it into the HTML page.
        /// </summary>
        /// <returns>Coroutine waiting for <see cref="IsInitialized"/> to return true.</returns>
        public static IEnumerator Initialize(Action onSuccessCallback = null, bool callReadyOnCompletion = true)
        {
            s_onInitializeSuccessCallback = onSuccessCallback;
            s_notifyLoadingCompletedOnInitialize = callReadyOnCompletion;

            YandexGamesSdkInitialize(OnInitializeSuccessCallback);

            while (!IsInitialized)
                yield return null;
        }

        [DllImport("__Internal")]
        private static extern void YandexGamesSdkInitialize(Action successCallback);

        [MonoPInvokeCallback(typeof(Action))]
        private static void OnInitializeSuccessCallback()
        {
            if (CallbackLogging)
                Debug.Log($"{nameof(YandexGamesSdk)}.{nameof(OnInitializeSuccessCallback)} invoked");

            if (s_notifyLoadingCompletedOnInitialize)
                GameReady.NotifyLoadingCompleted();

            s_onInitializeSuccessCallback?.Invoke();
        }
    }
}
