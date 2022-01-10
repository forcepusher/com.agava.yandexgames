using System;
using UnityEngine.Scripting;

namespace Agava.YandexGames
{
    [Serializable]
    public class LeaderboardGetEntriesResponse
    {
        [field: Preserve]
        public LeaderboardDescriptionResponse leaderboard;
        [field: Preserve]
        public Range[] ranges;
        [field: Preserve]
        public int userRank;
        [field: Preserve]
        public LeaderboardEntryResponse[] entries;


        [Serializable]
        public class Range
        {
            [field: Preserve]
            public int start;
            [field: Preserve]
            public int size;
        }
    }
}
