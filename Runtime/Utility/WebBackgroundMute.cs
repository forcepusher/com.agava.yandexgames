using System.Threading.Tasks;
using UnityEngine;

namespace YandexGames.Utility
{
    /// <summary>
    /// Mutes audio in background while "Run In Background" option is set to true.
    /// </summary>
    /// <remarks>
    /// Uses <see cref="AudioListener.pause"/> to achieve this effect.<br/>
    /// Workaround for <see href="https://trello.com/c/PjW4j3st"/>
    /// </remarks>
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
                if (Enabled)
                    AudioListener.pause = Time.unscaledDeltaTime > Time.maximumDeltaTime;

                await Task.Yield();
            }
        }
    }
}
