using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnistMemes_Encryption_v1._2
{
    public class CryptoSettings
    {
        public string SourcePath;
        public string OutputPath;
        public bool IsEncrypt;
        public int Layers;
        public bool UseKey;
        public string Key;
        public bool IsExeOperation;    // لتمييز عملية الـ EXE
        public string CustomIconPath;  // مسار الأيقونة
    }
}
