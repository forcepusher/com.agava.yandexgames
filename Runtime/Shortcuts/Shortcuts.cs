using System;
using System.Runtime.InteropServices;
using AOT;
using UnityEngine;

namespace Agava.YandexGames
{
    public static class Shortcuts
    {
        private static Action<bool> s_onCanSuggestCallback;
        private static Action s_onSuggestionAcceptCallback;
        private static Action s_onSuggestionDeclineCallback;

        public static void Suggest(Action onAccept = null, Action onDecline = null)
        {
            s_onSuggestionAcceptCallback = onAccept;
            s_onSuggestionDeclineCallback = onDecline;

            ShortcutsSuggestShortcut(SuggestShortcutAcceptCallback, SuggestShortcutDeclineCallback);
        }

        public static void CanSuggest(Action<bool> onSuccessCallback)
        {
            s_onCanSuggestCallback = onSuccessCallback;

            ShortcutsCanSuggestShortcut(CanSuggestShortcutCallback);
        }

        [DllImport("__Internal")]
        private static extern void ShortcutsCanSuggestShortcut(Action<int> onSuccess);

        [DllImport("__Internal")]
        private static extern void ShortcutsSuggestShortcut(Action onAccept, Action onDecline);

        [MonoPInvokeCallback(typeof(Action<bool>))]
        private static void CanSuggestShortcutCallback(int isCan)
        {
            if (YandexGamesSdk.CallbackLogging)
                Debug.Log($"{nameof(Shortcuts)}.{nameof(CanSuggestShortcutCallback)} called. {nameof(isCan)}={isCan}");

            s_onCanSuggestCallback.Invoke(isCan == 1);
        }

        [MonoPInvokeCallback(typeof(Action))]
        private static void SuggestShortcutAcceptCallback()
        {
            if (YandexGamesSdk.CallbackLogging)
                Debug.Log($"{nameof(Shortcuts)}.{nameof(SuggestShortcutAcceptCallback)} called");

            s_onSuggestionAcceptCallback?.Invoke();
        }

        [MonoPInvokeCallback(typeof(Action))]
        private static void SuggestShortcutDeclineCallback()
        {
            if (YandexGamesSdk.CallbackLogging)
                Debug.Log($"{nameof(Shortcuts)}.{nameof(SuggestShortcutDeclineCallback)} called");

            s_onSuggestionDeclineCallback?.Invoke();
        }
    }
}
