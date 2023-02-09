using System;
using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Agava.YandexGames.Tests
{
    public class BillingTests
    {
        [UnitySetUp]
        public IEnumerator InitializeSdk()
        {
            if (!YandexGamesSdk.IsInitialized)
                yield return YandexGamesSdk.Initialize(SdkTests.TrackSuccessCallback);
        }

        [UnityTest]
        public IEnumerator PurchaseShouldInvokeErrorCallback()
        {
            bool callbackInvoked = false;
            Billing.PurchaseProduct("adsfjoisadjfojds", onErrorCallback: (message) =>
            {
                callbackInvoked = true;
            });

            yield return new WaitForSecondsRealtime(1);

            Assert.IsTrue(callbackInvoked);
        }

        [UnityTest]
        public IEnumerator ConsumeShouldInvokeErrorCallback()
        {
            bool callbackInvoked = false;
            Billing.ConsumeProduct("adsfjoisadjfojds", onErrorCallback: (message) =>
            {
                callbackInvoked = true;
            });

            yield return new WaitForSecondsRealtime(1);

            Assert.IsTrue(callbackInvoked);
        }

        [UnityTest]
        public IEnumerator GetProductCatalogShouldInvokeErrorCallback()
        {
            bool callbackInvoked = false;
            Billing.GetProductCatalog(onErrorCallback: (message) =>
            {
                callbackInvoked = true;
            });

            yield return new WaitForSecondsRealtime(1);

            Assert.IsTrue(callbackInvoked);
        }

        [UnityTest]
        public IEnumerator GetPurchasedProductsShouldInvokeErrorCallback()
        {
            bool callbackInvoked = false;
            Billing.GetPurchasedProducts(onErrorCallback: (message) =>
            {
                callbackInvoked = true;
            });

            yield return new WaitForSecondsRealtime(1);

            Assert.IsTrue(callbackInvoked);
        }
    }
}
