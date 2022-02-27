#pragma warning disable

using System.Collections;
using Agava.YandexGames;
using Agava.YandexGames;
using Agava.YandexGames.Samples;
using Agava.YandexGames.Samples;
using Agava.YandexGames.Utility;
using Agava.YandexGames.Utility;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Agava.YandexGames.Samples
{
    public class PlaytestingCanvas : MonoBehaviour
    {
        [SerializeField]
        private Text _authorizationStatusText;

        [SerializeField]
        private Text _personalProfileDataPermissionStatusText;

        [SerializeField]
        private InputField _playerDataTextField;

        private void Awake()
        {
            YandexGamesSdk.CallbackLogging = true;
        }

        private IEnumerator Start()
        {
#if !UNITY_WEBGL || UNITY_EDITOR
            yield break;
#endif

            // Always wait for it if invoking something immediately in the first scene.
            yield return YandexGamesSdk.WaitForInitialization();

            while (true)
            {
                _authorizationStatusText.color = PlayerAccount.IsAuthorized ? Color.green : Color.red;

                if (PlayerAccount.IsAuthorized)
                    _personalProfileDataPermissionStatusText.color = PlayerAccount.HasPersonalProfileDataPermission ? Color.green : Color.red;

                yield return new WaitForSecondsRealtime(0.25f);
            }
        }

        private void Update()
        {
            // Mute sounds when app is running in the background.
            AudioListener.pause = WebApplication.InBackground;
        }

        public void OnShowInterestialButtonClick()
        {
            InterestialAd.Show();
        }

        public void OnShowVideoButtonClick()
        {
            VideoAd.Show();
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

        public void OnSetPlayerDataButtonClick()
        {
            PlayerAccount.SetPlayerData(_playerDataTextField.text);
        }

        public void OnGetPlayerDataButtonClick()
        {
            PlayerAccount.GetPlayerData((data) => _playerDataTextField.text = data);
        }

        public void OnGetDeviceTypeButtonClick()
        {
            Debug.Log($"DeviceType = {Device.Type}");
        }
    }
}
