using System;
using System.Runtime.InteropServices;
using AOT;
using UnityEngine;

namespace Agava.YandexGames
{
    public static class Shortcut
    {
        private static Action<bool> s_onCanSuggestCallback;
        private static Action s_onSuggestionAcceptCallback;
        private static Action s_onSuggestionDeclineCallback;

        public static void Suggest(Action onAcceptCallback = null, Action onDeclineCallback = null)
        {
            s_onSuggestionAcceptCallback = onAcceptCallback;
            s_onSuggestionDeclineCallback = onDeclineCallback;

            ShortcutSuggestShortcut(SuggestShortcutAcceptCallback, SuggestShortcutDeclineCallback);
        }

        public static void CanSuggest(Action<bool> onResultCallback)
        {
            s_onCanSuggestCallback = onResultCallback;

            ShortcutCanSuggestShortcut(CanSuggestShortcutCallback);
        }

        [DllImport("__Internal")]
        private static extern void ShortcutCanSuggestShortcut(Action<bool> onResultCallback);

        [DllImport("__Internal")]
        private static extern void ShortcutSuggestShortcut(Action onAcceptCallback, Action onDeclineCallback);

        [MonoPInvokeCallback(typeof(Action<bool>))]
        private static void CanSuggestShortcutCallback(bool canSuggest)
        {
            if (YandexGamesSdk.CallbackLogging)
                Debug.Log($"{nameof(Shortcut)}.{nameof(CanSuggestShortcutCallback)} called. {nameof(canSuggest)}={canSuggest}");

            s_onCanSuggestCallback.Invoke(canSuggest);
        }

        [MonoPInvokeCallback(typeof(Action))]
        private static void SuggestShortcutAcceptCallback()
        {
            if (YandexGamesSdk.CallbackLogging)
                Debug.Log($"{nameof(Shortcut)}.{nameof(SuggestShortcutAcceptCallback)} called");

            s_onSuggestionAcceptCallback?.Invoke();
        }

        [MonoPInvokeCallback(typeof(Action))]
        private static void SuggestShortcutDeclineCallback()
        {
            if (YandexGamesSdk.CallbackLogging)
                Debug.Log($"{nameof(Shortcut)}.{nameof(SuggestShortcutDeclineCallback)} called");

            s_onSuggestionDeclineCallback?.Invoke();
        }
    }
}
