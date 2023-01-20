using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Agava.YandexGames.Tests
{
    public class StickyAdTests
    {
        [UnitySetUp]
        public IEnumerator InitializeSdk()
        {
            if (!YandexGamesSdk.IsInitialized)
                yield return YandexGamesSdk.Initialize(SdkTests.TrackSuccessCallback);
        }

        [Test]
        public void ShowShouldNotThrow()
        {
            Assert.DoesNotThrow(() => StickyAd.Show());
        }

        [Test]
        public void HideShouldNotThrow()
        {
            Assert.DoesNotThrow(() => StickyAd.Hide());
        }
    }
}
