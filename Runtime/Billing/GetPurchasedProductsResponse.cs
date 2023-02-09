using System;
using UnityEngine.Scripting;

namespace Agava.YandexGames
{
    [Serializable]
    public class GetPurchasedProductsResponse
    {
        [field: Preserve]
        public PurchasedProduct[] purchasedProducts;
        [field: Preserve]
        public string signature;
    }
}
