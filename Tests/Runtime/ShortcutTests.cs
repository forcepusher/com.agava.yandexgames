using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Agava.YandexGames.Tests
{
    public class ShortcutTests
    {
        [UnitySetUp]
        public IEnumerator InitializeSdk()
        {
            if (!YandexGamesSdk.IsInitialized)
                yield return YandexGamesSdk.Initialize(SdkTests.TrackSuccessCallback);
        }

        [Test]
        public void CanSuggestShouldNotThrow()
        {
            Assert.DoesNotThrow(() => Shortcut.CanSuggest(result => {}));
        }

        [Test]
        public void SuggestShouldNotThrow()
        {
            Assert.DoesNotThrow(() => Shortcut.Suggest());
        }
    }
}
