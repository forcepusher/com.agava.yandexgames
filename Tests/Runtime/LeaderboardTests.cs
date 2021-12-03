using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace YandexGames.Tests
{
    public class LeaderboardTests
    {
        [UnitySetUp]
        public IEnumerator WaitForInitialization()
        {
            yield return Leaderboard.WaitForInitialization();
        }

        [Test]
        public void SetScoreShouldNotThrow()
        {
            Assert.DoesNotThrow(() => Leaderboard.SetScore("NonExistingBoard", 228));
            Assert.DoesNotThrow(() => Leaderboard.SetScore("NonExistingBoard", 0, additionalData: "henlo"));
        }

        [UnityTest]
        public IEnumerator SetScoreShouldInvokeErrorCallback()
        {
            bool callbackInvoked = false;
            Leaderboard.SetScore("NonExistingBoard", 228, onErrorCallback: (message) =>
            {
                callbackInvoked = true;
            });

            yield return new WaitForSecondsRealtime(1);

            Assert.IsTrue(callbackInvoked);
        }

        [UnityTest]
        public IEnumerator GetEntriesShouldInvokeErrorCallback()
        {
            bool callbackInvoked = false;
            Leaderboard.GetEntries("NonExistingBoard", onErrorCallback: (message) =>
            {
                callbackInvoked = true;
            });

            yield return new WaitForSecondsRealtime(1);

            Assert.IsTrue(callbackInvoked);
        }
    }
}
