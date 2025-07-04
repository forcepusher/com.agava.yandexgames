#pragma warning disable

using System.Collections;
using BananaParty.YandexGames;
using BananaParty.YandexGames.Samples;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BananaParty.YandexGames.Samples
{
    public class PlaytestingCanvas : MonoBehaviour
    {
        [SerializeField]
        private Text _authorizationStatusText;

        [SerializeField]
        private Text _personalProfileDataPermissionStatusText;

        [SerializeField]
        private Text _isRunningOnYandexStatusText;

        [SerializeField]
        private InputField _cloudSaveDataInputField;

        private void Awake()
        {
            YandexGamesSdk.CallbackLogging = true;
            PlayerAccount.AuthorizedInBackground += OnAuthorizedInBackground;
        }

        private IEnumerator Start()
        {
            if (!YandexGamesSdk.IsRunningOnYandex)
                yield break;

            CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
            canvasGroup.interactable = false;
            // Always wait for it if invoking something immediately in the first scene.
            yield return YandexGamesSdk.Initialize();

            YandexGamesSdk.GameReady();

            canvasGroup.interactable = true;

            if (PlayerAccount.IsAuthorized == false)
                PlayerAccount.StartAuthorizationPolling(1500);

            _isRunningOnYandexStatusText.color = YandexGamesSdk.IsRunningOnYandex ? Color.green : Color.red;

            while (true)
            {
                _authorizationStatusText.color = PlayerAccount.IsAuthorized ? Color.green : Color.red;

                if (PlayerAccount.IsAuthorized)
                    _personalProfileDataPermissionStatusText.color = PlayerAccount.HasPersonalProfileDataPermission ? Color.green : Color.red;
                else
                    _personalProfileDataPermissionStatusText.color = Color.red;

                yield return new WaitForSecondsRealtime(0.25f);
            }
        }

        private void OnDestroy()
        {
            PlayerAccount.AuthorizedInBackground -= OnAuthorizedInBackground;
        }

        public void OnShowInterstitialButtonClick()
        {
            InterstitialAd.Show();
        }

        public void OnShowVideoButtonClick()
        {
            VideoAd.Show();
        }

        public void OnShowStickyAdButtonClick()
        {
            StickyAd.Show();
        }

        public void OnHideStickyAdButtonClick()
        {
            StickyAd.Hide();
        }

        public void OnAuthorizeButtonClick()
        {
            PlayerAccount.Authorize();
        }

        public void OnRequestPersonalProfileDataPermissionButtonClick()
        {
            PlayerAccount.RequestPersonalProfileDataPermission();
        }

        public void OnGetProfileDataButtonClick()
        {
            PlayerAccount.GetProfileData((result) =>
            {
                string name = result.publicName;
                if (string.IsNullOrEmpty(name))
                    name = "Anonymous";
                Debug.Log($"My id = {result.uniqueID}, name = {name}");
            });
        }

        public void OnSetLeaderboardScoreButtonClick()
        {
            Leaderboard.SetScore("PlaytestBoard", Random.Range(1, 100));
        }

        public void OnGetLeaderboardEntriesButtonClick()
        {
            Leaderboard.GetEntries("PlaytestBoard", (result) =>
            {
                Debug.Log($"My rank = {result.userRank}");
                foreach (var entry in result.entries)
                {
                    string name = entry.player.publicName;
                    if (string.IsNullOrEmpty(name))
                        name = "Anonymous";
                    Debug.Log(name + " " + entry.score);
                }
            });
        }

        public void OnGetLeaderboardPlayerEntryButtonClick()
        {
            Leaderboard.GetPlayerEntry("PlaytestBoard", (result) =>
            {
                if (result == null)
                    Debug.Log("Player is not present in the leaderboard.");
                else
                    Debug.Log($"My rank = {result.rank}, score = {result.score}");
            });
        }

        public void OnSetCloudSaveDataButtonClick()
        {
            PlayerAccount.SetCloudSaveData(_cloudSaveDataInputField.text);
        }

        public void OnGetCloudSaveDataButtonClick()
        {
            PlayerAccount.GetCloudSaveData((data) => _cloudSaveDataInputField.text = data);
        }

        public void OnGetEnvironmentButtonClick()
        {
            Debug.Log($"Environment = {JsonUtility.ToJson(YandexGamesSdk.Environment)}");
        }

        public void OnCallGameReadyButtonClick()
        {
            YandexGamesSdk.GameReady();
        }

        public void OnSuggestShortcutButtonClick()
        {
            Shortcut.Suggest();
        }

        public void OnRequestReviewButtonClick()
        {
            ReviewPopup.Open();
        }

        public void OnCanSuggestShortcutButtonClick()
        {
            Shortcut.CanSuggest(result => { });
        }

        public void OnCanRequestReviewButtonClick()
        {
            ReviewPopup.CanOpen((result, reason) => { });
        }

        private void OnAuthorizedInBackground()
        {
            Debug.Log($"{nameof(OnAuthorizedInBackground)} {PlayerAccount.IsAuthorized}");
        }
    }
}
