using System.Collections;
using Agava.YandexGames;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace YandexGames.Tests
{
    public class DeviceTests
    {
        [UnitySetUp]
        public IEnumerator WaitForSdkInitialization()
        {
            yield return YandexGamesSdk.WaitForInitialization();
        }

        [Test]
        public void ShouldReturnDesktopDeviceType()
        {
            Assert.AreEqual(Device.Type, DeviceType.Desktop);
        }
    }
}
