using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Agava.YandexGames.Tests
{
    public class VideoAdTests
    {
        [UnitySetUp]
        public IEnumerator InitializeSdk()
        {
            if (!YandexGamesSdk.IsInitialized)
                yield return YandexGamesSdk.Initialize(SdkTests.TrackSuccessCallback);
        }

        [UnityTest]
        public IEnumerator ShowShouldInvokeErrorCallback()
        {
            bool callbackInvoked = false;
            VideoAd.Show(onErrorCallback: (message) =>
            {
                callbackInvoked = true;
            });

            yield return new WaitForSecondsRealtime(1);

            Assert.IsTrue(callbackInvoked);
        }
    }
}
