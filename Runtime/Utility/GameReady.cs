using System.Runtime.InteropServices;

namespace Agava.YandexGames
{
    public static class GameReady
    {
        public static void NotifyLoadingCompleted()
        {
            GameReadyLoadingCompleted();
        }

        [DllImport("__Internal")]
        private static extern void GameReadyLoadingCompleted();
    }
}


