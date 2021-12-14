using System.Threading.Tasks;
using UnityEngine;

namespace YandexGames
{
    /// <summary>
    /// Mutes audio in background while "Run In Background" option is set to true.
    /// Workaround for https://trello.com/c/PjW4j3st
    /// </summary>
    public static class WebBackgroundMute
    {
        public static bool Enabled = false;

#if UNITY_WEBGL && !UNITY_EDITOR
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
#endif
        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Fuck off. TYVM.")]
        private static async void MuteUnmuteLoop()
        {
            while (true)
            {
                AudioListener.pause = Enabled && Time.unscaledDeltaTime > Time.maximumDeltaTime;
                await Task.Yield();
            }
        }
    }
}
