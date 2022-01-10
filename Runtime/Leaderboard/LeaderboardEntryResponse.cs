using System;
using UnityEngine.Scripting;

namespace Agava.YandexGames
{
    [Serializable]
    public class LeaderboardEntryResponse
    {
        [field: Preserve]
        public int score;
        [field: Preserve]
        public string extraData;
        [field: Preserve]
        public int rank;
        [field: Preserve]
        public PlayerAccountProfileDataResponse player;
        [field: Preserve]
        public string formattedScore;
    }
}
