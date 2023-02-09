using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Agava.YandexGames.Tests
{
    public class LeaderboardTests
    {
        [UnitySetUp]
        public IEnumerator InitializeSdk()
        {
            if (!YandexGamesSdk.IsInitialized)
                yield return YandexGamesSdk.Initialize(SdkTests.TrackSuccessCallback);
        }

        [Test]
        public void SetScoreShouldNotThrow()
        {
            Assert.DoesNotThrow(() => Leaderboard.SetScore("NonExistingBoard", 228));
            Assert.DoesNotThrow(() => Leaderboard.SetScore("NonExistingBoard", 0, extraData: "henlo"));
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
            Leaderboard.GetEntries("NonExistingBoard", null, onErrorCallback: (message) =>
            {
                callbackInvoked = true;
            });

            yield return new WaitForSecondsRealtime(1);

            Assert.IsTrue(callbackInvoked);
        }

        [UnityTest]
        public IEnumerator GetPlayerEntryShouldInvokeErrorCallback()
        {
            bool callbackInvoked = false;
            Leaderboard.GetPlayerEntry("NonExistingBoard", null, onErrorCallback: (message) =>
            {
                callbackInvoked = true;
            });

            yield return new WaitForSecondsRealtime(1);

            Assert.IsTrue(callbackInvoked);
        }

        [Test]
        public void GetEntriesResponseParsingShouldNotThrow()
        {
            // {"entries":[{"score":99,"extraData":"","rank":1,"player":{"lang":"en","publicName":"","scopePermissions":{"avatar":"forbid","public_name":"forbid"},"uniqueID":"PYI9P2xFf4Z0+w0j/DU+p7wREIi3cwEcJGflNCrSlsY="},"formattedScore":"99"},{"score":95,"extraData":"","rank":2,"player":{"lang":"ru","publicName":"Dpyr C.","scopePermissions":{"avatar":"allow","public_name":"allow"},"uniqueID":"XhSPasVSsFoZ1P85fpN/YJMfTzRVWPdLtbn8PIzvo5U="},"formattedScore":"95"},{"score":74,"extraData":"","rank":3,"player":{"lang":"ru","publicName":"Петр Мустаев","scopePermissions":{"avatar":"allow","public_name":"allow"},"uniqueID":"ybDY/9WafCq6ChPUO6SyXfnZY9gxO0LPsd/mF1KzgBo="},"formattedScore":"74"},{"score":72,"extraData":"","rank":4,"player":{"lang":"ru","publicName":"","scopePermissions":{"avatar":"not_set","public_name":"not_set"},"uniqueID":"/cYcBitvABSVWKHEohLL3kFEfJ3lbTZq8AhrhDWu39c="},"formattedScore":"72"},{"score":48,"extraData":"","rank":5,"player":{"lang":"ru","publicName":"Рома А.","scopePermissions":{"avatar":"allow","public_name":"allow"},"uniqueID":"EWfli0tKXO5ag7dx53G2QtTSp35okhbTyJHLFxuRqBw="},"formattedScore":"48"},{"score":12,"extraData":"","rank":6,"player":{"lang":"ru","publicName":"Павел Д.","scopePermissions":{"avatar":"allow","public_name":"allow"},"uniqueID":"bq5R0Ks1tDBlT2nhV7XZr6pU0hHkruKbQAuaRf8e0qI="},"formattedScore":"12"}],"leaderboard":{"appID":179973,"name":"PlaytestBoard","default":true,"description":{"invert_sort_order":false,"score_format":{"type":"numeric","options":{"decimal_offset":0}}},"title":{"ru":""}},"ranges":[{"start":0,"size":9}],"userRank":4}
            string responseSampleJson = @"{""entries"":[{""score"":99,""extraData"":"""",""rank"":1,""player"":{""lang"":""en"",""publicName"":"""",""scopePermissions"":{""avatar"":""forbid"",""public_name"":""forbid""},""uniqueID"":""PYI9P2xFf4Z0+w0j/DU+p7wREIi3cwEcJGflNCrSlsY=""},""formattedScore"":""99""},{ ""score"":95,""extraData"":"""",""rank"":2,""player"":{ ""lang"":""ru"",""publicName"":""Dpyr C."",""scopePermissions"":{ ""avatar"":""allow"",""public_name"":""allow""},""uniqueID"":""XhSPasVSsFoZ1P85fpN/YJMfTzRVWPdLtbn8PIzvo5U=""},""formattedScore"":""95""},{ ""score"":74,""extraData"":"""",""rank"":3,""player"":{ ""lang"":""ru"",""publicName"":""Петр Мустаев"",""scopePermissions"":{ ""avatar"":""allow"",""public_name"":""allow""},""uniqueID"":""ybDY/9WafCq6ChPUO6SyXfnZY9gxO0LPsd/mF1KzgBo=""},""formattedScore"":""74""},{ ""score"":72,""extraData"":"""",""rank"":4,""player"":{ ""lang"":""ru"",""publicName"":"""",""scopePermissions"":{ ""avatar"":""not_set"",""public_name"":""not_set""},""uniqueID"":""/cYcBitvABSVWKHEohLL3kFEfJ3lbTZq8AhrhDWu39c=""},""formattedScore"":""72""},{ ""score"":48,""extraData"":"""",""rank"":5,""player"":{ ""lang"":""ru"",""publicName"":""Рома А."",""scopePermissions"":{ ""avatar"":""allow"",""public_name"":""allow""},""uniqueID"":""EWfli0tKXO5ag7dx53G2QtTSp35okhbTyJHLFxuRqBw=""},""formattedScore"":""48""},{ ""score"":12,""extraData"":"""",""rank"":6,""player"":{ ""lang"":""ru"",""publicName"":""Павел Д."",""scopePermissions"":{ ""avatar"":""allow"",""public_name"":""allow""},""uniqueID"":""bq5R0Ks1tDBlT2nhV7XZr6pU0hHkruKbQAuaRf8e0qI=""},""formattedScore"":""12""}],""leaderboard"":{ ""appID"":179973,""name"":""PlaytestBoard"",""default"":true,""description"":{ ""invert_sort_order"":false,""score_format"":{ ""type"":""numeric"",""options"":{ ""decimal_offset"":0} } },""title"":{ ""ru"":""""} },""ranges"":[{""start"":0,""size"":9}],""userRank"":4}";

            LeaderboardGetEntriesResponse response = JsonUtility.FromJson<LeaderboardGetEntriesResponse>(responseSampleJson);
            Assert.IsNotEmpty(response.entries);
        }
    }
}
