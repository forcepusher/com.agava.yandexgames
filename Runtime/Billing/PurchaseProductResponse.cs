using System;
using UnityEngine.Scripting;

namespace Agava.YandexGames
{
    [Serializable]
    public class PurchaseProductResponse
    {
        [field: Preserve]
        public PurchasedProduct purchaseData;
        [field: Preserve]
        public string signature;
    }
}
