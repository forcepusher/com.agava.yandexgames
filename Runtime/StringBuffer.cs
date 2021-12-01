using System;
using System.Runtime.InteropServices;
using System.Text;

namespace YandexGames.Utility
{
    public class StringBuffer
    {
        private readonly IntPtr _bufferPtr;
        private readonly int _bufferLength;

        public StringBuffer(IntPtr bufferPtr, int bufferLength)
        {
            _bufferPtr = bufferPtr;
            _bufferLength = bufferLength;
        }

        public override string ToString()
        {
            byte[] errorMessageBuffer = new byte[_bufferLength];
            Marshal.Copy(_bufferPtr, errorMessageBuffer, 0, _bufferLength);
            return Encoding.UTF8.GetString(errorMessageBuffer);
        }
    }
}
