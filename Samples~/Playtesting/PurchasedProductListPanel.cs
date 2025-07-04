using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BananaParty.YandexGames.Samples
{
    public class PurchasedProductListPanel : MonoBehaviour
    {
        [SerializeField]
        private PurchasedProductPanel _purchasedProductPanelTemplate;
        [SerializeField]
        private LayoutGroup _purchasedProductsLayoutGroup;

        private readonly List<PurchasedProductPanel> _purchasedProductPanels = new List<PurchasedProductPanel>();

        private void Awake()
        {
            _purchasedProductPanelTemplate.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
#if UNITY_EDITOR
            string sampleResponseJson = "{\"purchasedProducts\":[{\"productID\":\"AnotherTestProduct\",\"purchaseTime\":0,\"purchaseToken\":\"c7a6a276-bf77-483f-b657-ae7bfce3fd8f\"},{\"productID\":\"AnotherTestProduct\",\"purchaseTime\":0,\"purchaseToken\":\"89d0621d-7789-4330-9b45-c9294084490f\"},{\"productID\":\"AnotherTestProduct\",\"purchaseTime\":0,\"purchaseToken\":\"a39f433d-cbda-4fc7-8748-e1266d335251\"},{\"productID\":\"AnotherTestProduct\",\"purchaseTime\":0,\"purchaseToken\":\"aee6053e-3923-47f4-b703-3d21438274a4\"},{\"productID\":\"TestProduct\",\"purchaseTime\":0,\"purchaseToken\":\"a7afeaac-a694-4d39-a2eb-583f1bc19b1b\"},{\"productID\":\"TestProduct\",\"purchaseTime\":0,\"purchaseToken\":\"4b512664-a2f9-4906-ad26-66db76a69b82\"}],\"signature\":\"oQEbcvtMHUCBBipMUb8/vF0anwEqjoR6TZh+//jqIv0=.eyJhbGdvcml0aG0iOiJITUFDLVNIQTI1NiIsImlzc3VlZEF0IjoxNjc1OTE2MjQyLCJyZXF1ZXN0UGF5bG9hZCI6IiIsImRhdGEiOlt7InByb2R1Y3QiOnsiaWQiOiJBbm90aGVyVGVzdFByb2R1Y3QiLCJ0aXRsZSI6ItCW0LXQu9C10YjQtdGH0LrQsCIsImRlc2NyaXB0aW9uIjoiIiwicHJpY2UiOnsiY29kZSI6IllBTiIsInZhbHVlIjoiNCJ9LCJpbWFnZVByZWZpeCI6Imh0dHBzOi8vYXZhdGFycy5tZHMueWFuZGV4Lm5ldC9nZXQtZ2FtZXMvMjk3NzAzOS8yYTAwMDAwMTg2MjdjMDUzNDBjMTIzNGY1Y2ViMTg1MTc4MTIvIn0sInRva2VuIjoiYzdhNmEyNzYtYmY3Ny00ODNmLWI2NTctYWU3YmZjZTNmZDhmIiwiY3JlYXRlZCI6MTY3NTkwNDQ0Nn0seyJwcm9kdWN0Ijp7ImlkIjoiQW5vdGhlclRlc3RQcm9kdWN0IiwidGl0bGUiOiLQltC10LvQtdGI0LXRh9C60LAiLCJkZXNjcmlwdGlvbiI6IiIsInByaWNlIjp7ImNvZGUiOiJZQU4iLCJ2YWx1ZSI6IjQifSwiaW1hZ2VQcmVmaXgiOiJodHRwczovL2F2YXRhcnMubWRzLnlhbmRleC5uZXQvZ2V0LWdhbWVzLzI5NzcwMzkvMmEwMDAwMDE4NjI3YzA1MzQwYzEyMzRmNWNlYjE4NTE3ODEyLyJ9LCJ0b2tlbiI6Ijg5ZDA2MjFkLTc3ODktNDMzMC05YjQ1LWM5Mjk0MDg0NDkwZiIsImNyZWF0ZWQiOjE2NzU5MDI0MzF9LHsicHJvZHVjdCI6eyJpZCI6IkFub3RoZXJUZXN0UHJvZHVjdCIsInRpdGxlIjoi0JbQtdC70LXRiNC10YfQutCwIiwiZGVzY3JpcHRpb24iOiIiLCJwcmljZSI6eyJjb2RlIjoiWUFOIiwidmFsdWUiOiI0In0sImltYWdlUHJlZml4IjoiaHR0cHM6Ly9hdmF0YXJzLm1kcy55YW5kZXgubmV0L2dldC1nYW1lcy8yOTc3MDM5LzJhMDAwMDAxODYyN2MwNTM0MGMxMjM0ZjVjZWIxODUxNzgxMi8ifSwidG9rZW4iOiJhMzlmNDMzZC1jYmRhLTRmYzctODc0OC1lMTI2NmQzMzUyNTEiLCJjcmVhdGVkIjoxNjc1ODkxMDU3fSx7InByb2R1Y3QiOnsiaWQiOiJBbm90aGVyVGVzdFByb2R1Y3QiLCJ0aXRsZSI6ItCW0LXQu9C10YjQtdGH0LrQsCIsImRlc2NyaXB0aW9uIjoiIiwicHJpY2UiOnsiY29kZSI6IllBTiIsInZhbHVlIjoiNCJ9LCJpbWFnZVByZWZpeCI6Imh0dHBzOi8vYXZhdGFycy5tZHMueWFuZGV4Lm5ldC9nZXQtZ2FtZXMvMjk3NzAzOS8yYTAwMDAwMTg2MjdjMDUzNDBjMTIzNGY1Y2ViMTg1MTc4MTIvIn0sInRva2VuIjoiYWVlNjA1M2UtMzkyMy00N2Y0LWI3MDMtM2QyMTQzODI3NGE0IiwiY3JlYXRlZCI6MTY3NTg5MTAyN30seyJwcm9kdWN0Ijp7ImlkIjoiVGVzdFByb2R1Y3QiLCJ0aXRsZSI6ItCi0LXRgdGC0LvQvtC7IiwiZGVzY3JpcHRpb24iOiIiLCJwcmljZSI6eyJjb2RlIjoiWUFOIiwidmFsdWUiOiIxIn0sImltYWdlUHJlZml4IjoiIn0sInRva2VuIjoiYTdhZmVhYWMtYTY5NC00ZDM5LWEyZWItNTgzZjFiYzE5YjFiIiwiY3JlYXRlZCI6MTY3NDc2MzkyMH0seyJwcm9kdWN0Ijp7ImlkIjoiVGVzdFByb2R1Y3QiLCJ0aXRsZSI6ItCi0LXRgdGC0LvQvtC7IiwiZGVzY3JpcHRpb24iOiIiLCJwcmljZSI6eyJjb2RlIjoiWUFOIiwidmFsdWUiOiIxIn0sImltYWdlUHJlZml4IjoiIn0sInRva2VuIjoiNGI1MTI2NjQtYTJmOS00OTA2LWFkMjYtNjZkYjc2YTY5YjgyIiwiY3JlYXRlZCI6MTY3NDc2Mzc5M31dfQ==\"}";
            UpdatePurchasedProducts(JsonUtility.FromJson<GetPurchasedProductsResponse>(sampleResponseJson).purchasedProducts);
#else
            Billing.GetPurchasedProducts(purchasedProductsResponse => UpdatePurchasedProducts(purchasedProductsResponse.purchasedProducts));
#endif
        }

        private void UpdatePurchasedProducts(PurchasedProduct[] purchasedProducts)
        {
            foreach (PurchasedProductPanel purchasedProductPanel in _purchasedProductPanels)
                Destroy(purchasedProductPanel.gameObject);

            _purchasedProductPanels.Clear();

            foreach (PurchasedProduct purchasedProduct in purchasedProducts)
            {
                PurchasedProductPanel purchasedProductPanel = Instantiate(_purchasedProductPanelTemplate, _purchasedProductsLayoutGroup.transform);
                _purchasedProductPanels.Add(purchasedProductPanel);

                purchasedProductPanel.gameObject.SetActive(true);
                purchasedProductPanel.PurchasedProduct = purchasedProduct;
            }
        }

        public void RemovePurchasedProductPanel(PurchasedProductPanel purchasedProductPanel)
        {
            _purchasedProductPanels.Remove(purchasedProductPanel);

            Destroy(purchasedProductPanel.gameObject);
        }
    }
}
