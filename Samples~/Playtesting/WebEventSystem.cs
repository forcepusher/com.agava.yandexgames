using UnityEngine.EventSystems;

namespace BananaParty.YandexGames.Samples
{
    /// <summary>
    /// Fixes unresponsive UI controls after alt-tabbing on mobile Google Chrome.
    /// </summary>
    /// <remarks>
    /// Workaround for <see href="https://trello.com/c/PjW4j3st"/>
    /// </remarks>
    public class WebEventSystem : EventSystem
    {
        protected override void OnApplicationFocus(bool hasFocus) => base.OnApplicationFocus(true);
    }
}
