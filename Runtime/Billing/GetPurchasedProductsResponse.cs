using System;
using UnityEngine.Scripting;

namespace BananaParty.YandexGames
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
