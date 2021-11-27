using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Scripting;

[assembly: AlwaysLinkAssembly]
namespace YandexGames
{
    public static class YandexGamesSdk
    {
        /// <summary>
        /// Enable it to log SDK callbacks in the console.
        /// </summary>
        public static bool CallbackLogging = false;

        /// <summary>
        /// Think of this as a static constructor.
        /// </summary>
#if UNITY_WEBGL && !UNITY_EDITOR
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
#endif
        [DllImport("__Internal")]
        private static extern bool Initialize();

        /// <summary>
        /// SDK is initialized automatically on load. If something fails, this will return false.
        /// </summary>
        [DllImport("__Internal")]
        public static extern bool VerifyInitialization();
    }
}
