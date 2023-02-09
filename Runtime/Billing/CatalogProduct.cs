using System;
using UnityEngine.Scripting;

namespace Agava.YandexGames
{
    [Serializable]
    public class CatalogProduct
    {
        [field: Preserve]
        public string description;
        [field: Preserve]
        public string id;
        [field: Preserve]
        public string imageURI;
        [field: Preserve]
        public string price;
        [field: Preserve]
        public string priceCurrencyCode;
        [field: Preserve]
        public string priceValue;
        [field: Preserve]
        public string title;
    }
}
