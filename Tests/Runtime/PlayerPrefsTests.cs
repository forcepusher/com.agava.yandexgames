using NUnit.Framework;

namespace Agava.YandexGames.Tests
{
    public class PlayerPrefsTests
    {
        [Test]
        public void ShouldNotThrowOnEmptyData()
        {
            Assert.DoesNotThrow(() => PlayerPrefs.ParseAndApplyData(""));
        }

        [Test]
        public void ShouldNotCorruptData()
        {
            // {"abc":"1232","s":"5","somestring":"herpthederp"}
            PlayerPrefs.ParseAndApplyData("{\"abc\":\"1232\",\"s\":\"5\",\"somestring\":\"herpthederp\"}");
            Assert.AreEqual(PlayerPrefs.GetInt("abc"), 1232);
            Assert.AreEqual(PlayerPrefs.GetFloat("s"), 5f);
            Assert.AreEqual(PlayerPrefs.GetString("somestring"), "herpthederp");
        }
    }
}
