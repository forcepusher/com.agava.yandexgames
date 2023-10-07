using System;
using System.Runtime.InteropServices;
using AOT;
using UnityEngine;

namespace Agava.YandexGames
{
    public static class Shortcut
    {
        private static Action<bool> s_onCanSuggestCallback;
        private static Action<bool> s_onSuggestCallback;

        public static void CanSuggest(Action<bool> onResultCallback)
        {
            s_onCanSuggestCallback = onResultCallback;

            ShortcutCanSuggest(OnCanSuggestShortcutCallback);
        }

        [DllImport("__Internal")]
        private static extern void ShortcutCanSuggest(Action<bool> onResultCallback);

        [MonoPInvokeCallback(typeof(Action<bool>))]
        private static void OnCanSuggestShortcutCallback(bool result)
        {
            if (YandexGamesSdk.CallbackLogging)
                Debug.Log($"{nameof(Shortcut)}.{nameof(OnCanSuggestShortcutCallback)} called. {nameof(result)}={result}");

            s_onCanSuggestCallback?.Invoke(result);
        }

        public static void Suggest(Action<bool> onResultCallback = null)
        {
            s_onSuggestCallback = onResultCallback;

            ShortcutSuggest(OnSuggestShortcutCallback);
        }

        [DllImport("__Internal")]
        private static extern void ShortcutSuggest(Action<bool> onAcceptCallback);

        [MonoPInvokeCallback(typeof(Action<bool>))]
        private static void OnSuggestShortcutCallback(bool result)
        {
            if (YandexGamesSdk.CallbackLogging)
                Debug.Log($"{nameof(Shortcut)}.{nameof(OnSuggestShortcutCallback)} called. {nameof(result)}={result}");

            s_onSuggestCallback?.Invoke(result);
        }
    }
}
