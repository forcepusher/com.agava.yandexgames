using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace BananaParty.YandexGames.Tests
{
    public class ReviewPopupTests
    {
        [UnitySetUp]
        public IEnumerator InitializeSdk()
        {
            if (!YandexGamesSdk.IsInitialized)
                yield return YandexGamesSdk.Initialize(SdkTests.TrackSuccessCallback);
        }

        [Test]
        public void CanReviewShouldNotThrow()
        {
            Assert.DoesNotThrow(() => ReviewPopup.CanOpen((result, reason) => { }));
        }

        [Test]
        public void ReviewShouldNotThrow()
        {
            Assert.DoesNotThrow(() => ReviewPopup.Open());
        }
    }
}
