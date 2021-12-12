using UnityEngine.EventSystems;

namespace YandexGames
{
    /// <summary>
    /// Workaround for EventSystem bug https://trello.com/c/PjW4j3st
    /// </summary>
    public class WebEventSystem : EventSystem
    {
        protected override void OnApplicationFocus(bool hasFocus) => base.OnApplicationFocus(true);
    }
}
