using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace dbfCoreDLL.dbfStructures
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    internal struct MemoHeader
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        private byte[] _recordType;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        private byte[] _recordLength;

        // Проверка, что попали на реальный блок. Как выяснилось, в DBF есть ссылки на "битые" блоки FPT.
        // Вероятно, это произошло, когда делали "обрезку" файла FPT.
        public bool correctHeader
        {
            get
            {
                // "Фишка" TFO, позволяет проверить, что попали в начало записи. 
                // Значение второго байта заголовка = значение первого байта заголовка * 10.
                if (_recordType[0] != 0x00 && _recordType[1] != 0x00 && _recordType[2] == 0x00 && _recordType[3] == 0x00)
                {
                    return _recordType[1] == _recordType[0] * 10;
                }
                // Но эта "фишка" не всегда работает, поэтому проверяем по-другому: 3-й и 4-й байты типа записи = 0x00
                // и первый байт длины записи = 0x00. Иначе можем получить фигню.
                else
                {
                    return _recordType[2] == 0x00 && _recordType[3] == 0x00 && _recordLength[0] == 0x00;
                }
            }
        }

        public Int32 recordLength
        {
            get
            {
                return BitConverter.ToInt32(_recordLength.Reverse().ToArray(), 0);
            }
        }
    }
}
