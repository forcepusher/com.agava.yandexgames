using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Agava.YandexGames.Tests
{
    public class PlayerAccountTests
    {
        [UnitySetUp]
        public IEnumerator InitializeSdk()
        {
            if (!YandexGamesSdk.IsInitialized)
                yield return YandexGamesSdk.Initialize(SdkTests.TrackSuccessCallback);
        }

        [Test]
        public void ShouldNotBeAuthorizedOnStart()
        {
            Assert.IsFalse(PlayerAccount.IsAuthorized);
        }

        [UnityTest]
        public IEnumerator AuthorizeShouldInvokeErrorCallback()
        {
            bool callbackInvoked = false;
            PlayerAccount.Authorize(onErrorCallback: (message) =>
            {
                callbackInvoked = true;
            });

            yield return new WaitForSecondsRealtime(1);

            Assert.IsTrue(callbackInvoked);
        }
    }
}
