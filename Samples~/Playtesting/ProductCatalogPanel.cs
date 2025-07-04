using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BananaParty.YandexGames.Samples
{
    public class ProductCatalogPanel : MonoBehaviour
    {
        [SerializeField]
        private ProductPanel _productPanelTemplate;
        [SerializeField]
        private LayoutGroup _productCatalogLayoutGroup;

        private readonly List<ProductPanel> _productPanels = new List<ProductPanel>();

        private void Awake()
        {
            _productPanelTemplate.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
#if UNITY_EDITOR
            string sampleResponseJson = "{\"products\":[{\"id\":\"TestProduct\",\"title\":\"Òåñòëîë\",\"description\":\"\",\"imageURI\":\"/default256x256\",\"price\":\"1 YAN\",\"priceValue\":\"1\",\"priceCurrencyCode\":\"YAN\"},{\"id\":\"AnotherTestProduct\",\"title\":\"Æåëåøå÷êà\",\"description\":\"\",\"imageURI\":\"https://avatars.mds.yandex.net/get-games/2977039/2a0000018627c05340c1234f5ceb18517812//default256x256\",\"price\":\"4 YAN\",\"priceValue\":\"4\",\"priceCurrencyCode\":\"YAN\"}]}";
            UpdateProductCatalog(JsonUtility.FromJson<GetProductCatalogResponse>(sampleResponseJson).products);
#else
            Billing.GetProductCatalog(productCatalogReponse => UpdateProductCatalog(productCatalogReponse.products));
#endif
        }

        private void UpdateProductCatalog(CatalogProduct[] products)
        {
            foreach (ProductPanel productPanel in _productPanels)
                Destroy(productPanel.gameObject);

            _productPanels.Clear();

            foreach (CatalogProduct product in products)
            {
                ProductPanel productPanel = Instantiate(_productPanelTemplate, _productCatalogLayoutGroup.transform);
                _productPanels.Add(productPanel);

                productPanel.gameObject.SetActive(true);
                productPanel.Product = product;
            }
        }
    }
}
