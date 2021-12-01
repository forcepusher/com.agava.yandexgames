using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace YandexGames.Tests
{
    public class PlayerAccountTests
    {
        [UnitySetUp]
        public IEnumerator WaitForSdkInitialization()
        {
            yield return YandexGamesSdk.WaitForInitialization();
        }

        [Test]
        public void ShouldNotBeAuthorizedOnStart()
        {
            Assert.IsFalse(PlayerAccount.IsAuthorized);
        }

        [UnityTest]
        public IEnumerator AuthenticateShouldInvokeErrorCallback()
        {
            bool callbackInvoked = false;
            PlayerAccount.Authenticate(true, onErrorCallback: (message) => {
                callbackInvoked = true;
            });

            yield return new WaitForSecondsRealtime(1);

            Assert.IsTrue(callbackInvoked);
        }
    }
}
