using System;
using UnityEngine.Scripting;

namespace Agava.YandexGames
{
    [Serializable]
    public class PlayerAccountProfileDataResponse
    {
        [field: Preserve]
        public string lang;
        [field: Preserve]
        public string publicName;
        [field: Preserve]
        public ScopePermissions scopePermissions;
        [field: Preserve]
        public string uniqueID;


        [Serializable]
        public class ScopePermissions
        {
            [field: Preserve]
            public string avatar;
            [field: Preserve]
            public string public_name;
        }
    }
}
