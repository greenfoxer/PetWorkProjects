﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace dbfCoreDLL.dbfStructures
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    internal struct DBFHeader
    {
        public byte version;
        public byte updateYear;
        public byte updateMonth;
        public byte updateDay;
        public Int32 numRecords;
        public Int16 headerLen;
        public Int16 recordLen;
        public Int16 reserved1;
        public byte incompleteTrans;
        public byte encryptionFlag;
        public Int32 reserved2;
        public Int64 reserved3;
        public byte MDX;
        public byte language;
        public Int16 reserved4;
    }
}
