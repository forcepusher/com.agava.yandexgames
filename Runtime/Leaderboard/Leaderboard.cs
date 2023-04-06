using System;
using System.Runtime.InteropServices;
using AOT;
using UnityEngine;

namespace Agava.YandexGames
{
    public static class Leaderboard
    {
        // This is what we deserve for using Unity.
        private static Action s_onSetScoreSuccessCallback;
        private static Action<string> s_onSetScoreErrorCallback;

        private static Action<LeaderboardGetEntriesResponse> s_onGetEntriesSuccessCallback;
        private static Action<string> s_onGetEntriesErrorCallback;

        private static Action<LeaderboardEntryResponse> s_onGetPlayerEntrySuccessCallback;
        private static Action<string> s_onGetPlayerEntryErrorCallback;

        // We shouldn't normally use regions, but my eyes hurt from statics.

        #region SetScore
        /// <remarks>
        /// <para />To prevent overwriting a better result, use <see cref="GetPlayerEntry"/>.
        /// <para />If user did not give <see cref="PlayerAccount.HasPersonalProfileDataPermission"/> or rejected it, the result will be posted anonymously.
        /// <para />Requires authorization. Use <see cref="PlayerAccount.IsAuthorized"/> and <see cref="PlayerAccount.Authorize"/>.
        /// </remarks>
        public static void SetScore(string leaderboardName, int score, Action onSuccessCallback = null, Action<string> onErrorCallback = null, string extraData = "")
        {
            if (leaderboardName == null)
                throw new ArgumentNullException(nameof(leaderboardName));

            if (extraData == null)
                extraData = string.Empty;

            s_onSetScoreSuccessCallback = onSuccessCallback;
            s_onSetScoreErrorCallback = onErrorCallback;

            LeaderboardSetScore(leaderboardName, score, OnSetScoreSuccessCallback, OnSetScoreErrorCallback, extraData);
        }

        [DllImport("__Internal")]
        private static extern void LeaderboardSetScore(string leaderboardName, int score, Action successCallback, Action<string> errorCallback, string extraData);

        [MonoPInvokeCallback(typeof(Action))]
        private static void OnSetScoreSuccessCallback()
        {
            if (YandexGamesSdk.CallbackLogging)
                Debug.Log($"{nameof(Leaderboard)}.{nameof(OnSetScoreSuccessCallback)} invoked");

            s_onSetScoreSuccessCallback?.Invoke();
        }

        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void OnSetScoreErrorCallback(string errorMessage)
        {
            if (YandexGamesSdk.CallbackLogging)
                Debug.Log($"{nameof(Leaderboard)}.{nameof(OnSetScoreErrorCallback)} invoked, {nameof(errorMessage)} = {errorMessage}");

            s_onSetScoreErrorCallback?.Invoke(errorMessage);
        }
        #endregion

        #region GetEntries
        /// <summary>
        /// Returns a fully parsed result object in onSuccessCallback.
        /// </summary>
        /// <remarks>
        /// Requires authorization. Use <see cref="PlayerAccount.IsAuthorized"/> and <see cref="PlayerAccount.Authorize"/>.
        /// </remarks>
        public static void GetEntries(string leaderboardName, Action<LeaderboardGetEntriesResponse> onSuccessCallback, Action<string> onErrorCallback = null, int topPlayersCount = 5, int competingPlayersCount = 5, bool includeSelf = true, ProfilePictureSize pictureSize = ProfilePictureSize.medium)
        {
            if (leaderboardName == null)
                throw new ArgumentNullException(nameof(leaderboardName));

            s_onGetEntriesSuccessCallback = onSuccessCallback;
            s_onGetEntriesErrorCallback = onErrorCallback;

            LeaderboardGetEntries(leaderboardName, OnGetEntriesSuccessCallback, OnGetEntriesErrorCallback, topPlayersCount, competingPlayersCount, includeSelf, pictureSize.ToString());
        }

        [DllImport("__Internal")]
        private static extern void LeaderboardGetEntries(string leaderboardName, Action<string> successCallback, Action<string> errorCallback, int topPlayersCount, int competingPlayersCount, bool includeSelf, string pictureSize);

        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void OnGetEntriesSuccessCallback(string entriesResponseJson)
        {
            if (YandexGamesSdk.CallbackLogging)
                Debug.Log($"{nameof(Leaderboard)}.{nameof(OnGetEntriesSuccessCallback)} invoked, {nameof(entriesResponseJson)} = {entriesResponseJson}");

            LeaderboardGetEntriesResponse entriesResponse = JsonUtility.FromJson<LeaderboardGetEntriesResponse>(entriesResponseJson);

            s_onGetEntriesSuccessCallback?.Invoke(entriesResponse);
        }

        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void OnGetEntriesErrorCallback(string errorMessage)
        {
            if (YandexGamesSdk.CallbackLogging)
                Debug.Log($"{nameof(Leaderboard)}.{nameof(OnGetEntriesErrorCallback)} invoked, {nameof(errorMessage)} = {errorMessage}");

            s_onGetEntriesErrorCallback?.Invoke(errorMessage);
        }
        #endregion

        #region GetPlayerEntry
        /// <summary>
        /// Returns a fully parsed result object in onSuccessCallback, or returns null if player is not present.
        /// </summary>
        /// <remarks>
        /// Requires authorization. Use <see cref="PlayerAccount.IsAuthorized"/> and <see cref="PlayerAccount.Authorize"/>.
        /// </remarks>
        public static void GetPlayerEntry(string leaderboardName, Action<LeaderboardEntryResponse> onSuccessCallback, Action<string> onErrorCallback = null, ProfilePictureSize pictureSize = ProfilePictureSize.medium)
        {
            if (leaderboardName == null)
                throw new ArgumentNullException(nameof(leaderboardName));

            s_onGetPlayerEntrySuccessCallback = onSuccessCallback;
            s_onGetPlayerEntryErrorCallback = onErrorCallback;

            LeaderboardGetPlayerEntry(leaderboardName, OnGetPlayerEntrySuccessCallback, OnGetPlayerEntryErrorCallback, pictureSize.ToString());
        }

        [DllImport("__Internal")]
        private static extern void LeaderboardGetPlayerEntry(string leaderboardName, Action<string> successCallback, Action<string> errorCallback, string pictureSize);

        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void OnGetPlayerEntrySuccessCallback(string entryResponseJson)
        {
            if (YandexGamesSdk.CallbackLogging)
                Debug.Log($"{nameof(Leaderboard)}.{nameof(OnGetPlayerEntrySuccessCallback)} invoked, {nameof(entryResponseJson)} = {entryResponseJson}");

            LeaderboardEntryResponse entryResponse = entryResponseJson == "null" ? null : JsonUtility.FromJson<LeaderboardEntryResponse>(entryResponseJson);

            s_onGetPlayerEntrySuccessCallback?.Invoke(entryResponse);
        }

        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void OnGetPlayerEntryErrorCallback(string errorMessage)
        {
            if (YandexGamesSdk.CallbackLogging)
                Debug.Log($"{nameof(Leaderboard)}.{nameof(OnGetPlayerEntryErrorCallback)} invoked, {nameof(errorMessage)} = {errorMessage}");

            s_onGetPlayerEntryErrorCallback?.Invoke(errorMessage);
        }
        #endregion
    }
}
