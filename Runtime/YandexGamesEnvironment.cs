using System;
using UnityEngine.Scripting;

namespace BananaParty.YandexGames
{
    [Serializable]
    public class YandexGamesEnvironment
    {
        [field: Preserve]
        public App app;
        [field: Preserve]
        public Browser browser;
        [field: Preserve]
        public Internationalization i18n;
        [field: Preserve]
        public string payload;

        [Serializable]
        public class App
        {
            [field: Preserve]
            public string id;
        }

        [Serializable]
        public class Browser
        {
            [field: Preserve]
            public string lang;
        }

        [Serializable]
        public class Internationalization
        {
            [field: Preserve]
            public string lang;
            [field: Preserve]
            public string tld;
        }
    }
}
