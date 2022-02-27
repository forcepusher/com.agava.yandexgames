using System.Threading.Tasks;
using UnityEngine;

namespace Agava.YandexGames.Utility
{
    /// <summary>
    /// Workaround class for <see href="https://trello.com/c/PjW4j3st"/>
    /// </summary>
    public static class WebApplication
    {
        /// <summary>
        /// Threshold for <see cref="Time.unscaledDeltaTime"/> to assume that app is running in the background.
        /// </summary>
        /// <remarks>
        /// This works because most browsers throttle background page update frequency to 1 per second.
        /// </remarks>
        public static float BackgroundDetectionDeltaTimeThreshold = 0.75f;
        /// <summary>
        /// Number of subsequent updates with <see cref="Time.unscaledDeltaTime"/> above <see cref="BackgroundDetectionDeltaTimeThreshold"/>
        /// will switch <see cref="InBackground"/> to true.<br/>
        /// </summary>
        /// <remarks>
        /// Increase this value for laggy apps that are getting false-positives.
        /// </remarks>
        public static int BackgroundDetectionTicksThreshold = 2;

        /// <summary>
        /// Detects when app is in the background while <see cref="Application.runInBackground"/> is set to true.
        /// </summary>
        /// <remarks>
        /// This can't be a static event because we can't trust developers to reliably unsubscribe from it.
        /// </remarks>
        public static bool InBackground { get; private set; }

        private static int s_ticksAboveThreshold = 0;

#if UNITY_WEBGL && !UNITY_EDITOR
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
#endif
        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Unity InitializeOnLoadMethod")]
        private static async void UpdateLoopAsync()
        {
            while (true)
            {
                if (Time.unscaledDeltaTime > BackgroundDetectionDeltaTimeThreshold)
                    s_ticksAboveThreshold += 1;
                else
                    s_ticksAboveThreshold = 0;

                InBackground = s_ticksAboveThreshold >= BackgroundDetectionTicksThreshold;

                await Task.Yield();
            }
        }
    }
}
