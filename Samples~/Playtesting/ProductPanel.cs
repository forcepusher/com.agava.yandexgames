using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Agava.YandexGames.Samples
{
    public class ProductPanel : MonoBehaviour
    {
        [SerializeField]
        private RawImage _productImage;
        [SerializeField]
        private Text _productIdText;

        private CatalogProduct _product;

        public CatalogProduct Product
        {
            set
            {
                _product = value;

                _productIdText.text = value.id;
                
                if (Uri.IsWellFormedUriString(value.imageURI, UriKind.Absolute))
                    StartCoroutine(DownloadAndSetProductImage(value.imageURI));
            }
        }

        private IEnumerator DownloadAndSetProductImage(string imageUrl)
        {
            var remoteImage = new RemoteImage(imageUrl);
            remoteImage.Download();

            while (!remoteImage.IsDownloadFinished)
                yield return null;

            if (remoteImage.IsDownloadSuccessful)
                _productImage.texture = remoteImage.Texture;
        }

        public void OnPurchaseButtonClick()
        {
            Billing.PurchaseProduct(_product.id, (purchaseProductResponse) =>
            {
                Debug.Log($"Purchased {purchaseProductResponse.purchaseData.productID}");
            });
        }

        public void OnPurchaseAndConsumeButtonClick()
        {
            Billing.PurchaseProduct(_product.id, (purchaseProductResponse) =>
            {
                Debug.Log($"Purchased {purchaseProductResponse.purchaseData.productID}");

                Billing.ConsumeProduct(purchaseProductResponse.purchaseData.purchaseToken, () =>
                {
                    Debug.Log($"Consumed {purchaseProductResponse.purchaseData.productID}");
                });
            });
        }
    }
}
