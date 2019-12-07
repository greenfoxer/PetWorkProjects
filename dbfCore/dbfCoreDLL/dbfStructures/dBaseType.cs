using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dbfCoreDLL.dbfStructures
{
    public enum dBaseType : byte
    {
        // Binary
        B = 0x42,
        // String
        C = 0x43,
        // Date (YYYYMMDD)
        D = 0x44,
        // Number (Double)
        N = 0x4E,
        // Float
        F = 0x46,
        // Logical (Boolean) Byte
        L = 0x4C,
        // Memo
        M = 0x4D,
        // DateTime        
        T = 0x54
    }
}
