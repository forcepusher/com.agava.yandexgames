using System.Collections;
using Agava.YandexGames;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace YandexGames.Tests
{
    public class DeviceTests
    {
        [UnitySetUp]
        public IEnumerator InitializeSdk()
        {
            if (!YandexGamesSdk.IsInitialized)
                yield return YandexGamesSdk.Initialize(SdkTests.TrackSuccessCallback);
        }

        [Test]
        public void ShouldReturnDesktopDeviceType()
        {
            Assert.AreEqual(Device.Type, DeviceType.Desktop);
        }
    }
}
