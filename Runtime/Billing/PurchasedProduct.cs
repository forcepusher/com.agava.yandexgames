using System;
using UnityEngine.Scripting;

namespace Agava.YandexGames
{
    [Serializable]
    public class PurchasedProduct
    {
        [field: Preserve]
        public string developerPayload;
        [field: Preserve]
        public string productID;
        [field: Preserve]
        public string purchaseTime;
        [field: Preserve]
        public string purchaseToken;
    }
}
