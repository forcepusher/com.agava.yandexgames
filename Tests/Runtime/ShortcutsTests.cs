using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Agava.YandexGames.Tests
{
    public class ShortcutsTests
    {
        [UnitySetUp]
        public IEnumerator InitializeSdk()
        {
            if (!YandexGamesSdk.IsInitialized)
                yield return YandexGamesSdk.Initialize(SdkTests.TrackSuccessCallback);
        }

        [Test]
        public void IsCanSuggestShouldNotThrow()
        {
            Assert.DoesNotThrow(() => Shortcuts.CanSuggest(result => {}));
        }

        [Test]
        public void SuggestShouldNotThrow()
        {
            Assert.DoesNotThrow(() => Shortcuts.Suggest());
        }
    }
}
