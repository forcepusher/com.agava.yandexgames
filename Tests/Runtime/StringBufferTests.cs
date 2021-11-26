using System;
using System.Runtime.InteropServices;
using System.Text;
using NUnit.Framework;

namespace YandexGames.Utility.Tests
{
    public class StringBufferTests
    {
        [Test]
        public void ShouldConvertString()
        {
            string testString =
                "henlo programmer\n" +
                "helllo you STINKY PROGRAMMER\n" +
                "go code in javascript ugly\n";

            byte[] testStringBytes = Encoding.UTF8.GetBytes(testString);
            IntPtr stringBufferPointer = Marshal.AllocHGlobal(testStringBytes.Length);
            Marshal.Copy(testStringBytes, 0, stringBufferPointer, testStringBytes.Length);

            var stringBuffer = new StringBuffer(stringBufferPointer, testStringBytes.Length);
            Assert.AreEqual(testString, stringBuffer.ToString());

            Marshal.FreeHGlobal(stringBufferPointer);
        }
    }
}
