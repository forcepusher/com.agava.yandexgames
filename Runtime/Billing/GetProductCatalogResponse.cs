using System;
using UnityEngine.Scripting;

namespace BananaParty.YandexGames
{
    [Serializable]
    public class GetProductCatalogResponse
    {
        [field: Preserve]
        public CatalogProduct[] products;
    }
}
