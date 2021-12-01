using System;
using System.Runtime.InteropServices;
using System.Text;
using NUnit.Framework;

namespace YandexGames.Utility.Tests
{
    public class StringBufferTests
    {
        [Test]
        public void ToStringShouldNotGiveCorruptResult()
        {
            string testString =
                "henlo developer\n" +
                "helllo you STINKY DEVELOPER\n" +
                "go code in javascript ugly\n";

            byte[] testStringBytes = Encoding.UTF8.GetBytes(testString);
            IntPtr testStringBufferPtr = Marshal.AllocHGlobal(testStringBytes.Length);
            Marshal.Copy(testStringBytes, 0, testStringBufferPtr, testStringBytes.Length);

            var stringBuffer = new StringBuffer(testStringBufferPtr, testStringBytes.Length);
            Assert.AreEqual(testString, stringBuffer.ToString());

            Marshal.FreeHGlobal(testStringBufferPtr);
        }
    }
}
