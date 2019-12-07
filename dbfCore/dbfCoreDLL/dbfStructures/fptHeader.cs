using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace dbfCoreDLL.dbfStructures
{
    /// The header of a FPT file
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    internal struct FPTHeader
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        private byte[] _nextBlockID;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] reserved;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        private byte[] _blockLength;

        public Int32 nextBlockID
        {
            get
            {
                return BitConverter.ToInt32(_nextBlockID.Reverse().ToArray(), 0);
            }
        }

        public Int16 blockLength
        {
            get
            {
                return BitConverter.ToInt16(_blockLength.Reverse().ToArray(), 0);
            }
        }
    }
}
