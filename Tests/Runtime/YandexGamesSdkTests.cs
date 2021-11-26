using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace YandexGames.Tests
{
    public class YandexGamesSdkTests
    {
        [UnityTest]
        public IEnumerator SdkShouldInitializeAutomatically()
        {
            // Needs a second to download the script.
            yield return new WaitForSecondsRealtime(1f);
            Assert.IsTrue(YandexGamesSdk.VerifyInitialization());
        }
    }
}
