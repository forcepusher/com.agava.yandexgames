using System;
using UnityEngine.Scripting;

namespace Agava.YandexGames
{
    [Serializable]
    public class GetProductCatalogResponse
    {
        [field: Preserve]
        public CatalogProduct[] products;
    }
}
