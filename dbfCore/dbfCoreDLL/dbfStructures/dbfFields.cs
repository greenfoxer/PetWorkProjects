using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace dbfCoreDLL.dbfStructures
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    internal struct FieldDescriptor
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string fieldName;
        public dBaseType fieldType;
        public Int32 address;
        public byte fieldLen;
        public byte decimalCount;
        public Int16 reserved1;
        public byte workArea;
        public Int16 reserved2;
        public byte flag;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
        public byte[] reserved3;
        public byte indexFlag;

        public override string ToString()
        {
            return "Field-Name: " + fieldName;
        }
    }
}
