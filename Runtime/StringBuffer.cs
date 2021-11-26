using System;
using System.Runtime.InteropServices;
using System.Text;

namespace YandexGames.Utility
{
    public class StringBuffer
    {
        private readonly IntPtr _pointer;
        private readonly int _length;

        public StringBuffer(IntPtr pointer, int length)
        {
            _pointer = pointer;
            _length = length;
        }

        public override string ToString()
        {
            byte[] errorMessageBuffer = new byte[_length];
            Marshal.Copy(_pointer, errorMessageBuffer, 0, _length);
            return Encoding.UTF8.GetString(errorMessageBuffer);
        }
    }
}
