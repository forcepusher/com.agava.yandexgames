using System;
using System.Runtime.InteropServices;
using AOT;
using UnityEngine;

namespace Agava.YandexGames
{
    public class ReviewPopup
    {
        private static Action<bool, string> s_onCanOpenCallback;
        private static Action<bool> s_onOpenCallback;

        public static void CanOpen(Action<bool, string> onResultCallback)
        {
            s_onCanOpenCallback = onResultCallback;

            ReviewPopupCanOpen(OnCanOpenCallback);
        }

        [DllImport("__Internal")]
        private static extern void ReviewPopupCanOpen(Action<bool, string> onResultCallback);

        [MonoPInvokeCallback(typeof(Action<bool, string>))]
        private static void OnCanOpenCallback(bool result, string reason)
        {
            if (YandexGamesSdk.CallbackLogging)
                Debug.Log($"{nameof(Shortcut)}.{nameof(OnCanOpenCallback)} called. {nameof(result)}={result} {nameof(reason)}={reason}");

            s_onCanOpenCallback?.Invoke(result, reason);
        }

        public static void Open(Action<bool> onResultCallback = null)
        {
            s_onOpenCallback = onResultCallback;

            ReviewPopupOpen(OnOpenCallback);
        }

        [DllImport("__Internal")]
        private static extern void ReviewPopupOpen(Action<bool> onResultCallback);

        [MonoPInvokeCallback(typeof(Action<bool>))]
        private static void OnOpenCallback(bool result)
        {
            if (YandexGamesSdk.CallbackLogging)
                Debug.Log($"{nameof(Shortcut)}.{nameof(OnOpenCallback)} called. {nameof(result)}={result}");

            s_onOpenCallback?.Invoke(result);
        }
    }
}
