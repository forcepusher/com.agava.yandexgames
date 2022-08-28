using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Agava.YandexGames.Tests
{
    public class SdkTests
    {
        private static bool s_isInitializationSuccessCallbackReceived = false;

        [UnitySetUp]
        public IEnumerator InitializeSdk()
        {
            if (!YandexGamesSdk.IsInitialized)
                yield return YandexGamesSdk.Initialize(TrackSuccessCallback);
        }

        [Test]
        public void ShouldReturnEnvironment()
        {
            Assert.IsNotEmpty(YandexGamesSdk.Environment.browser.lang);
        }

        [Test]
        public void ShouldReceiveInitializationSuccessCallback()
        {
            Assert.IsTrue(s_isInitializationSuccessCallbackReceived);
        }

        public static void TrackSuccessCallback()
        {
            s_isInitializationSuccessCallbackReceived = true;
        }
    }
}
