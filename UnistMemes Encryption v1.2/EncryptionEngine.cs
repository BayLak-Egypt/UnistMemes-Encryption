using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection; // تأكد من وجود هذا السطر في الأعلى
using System.Text;
using System.Windows.Forms;
using UnistMemes_Encryption_v1._2;
using UnistMemes_Encryption_v1._2.Properties;
using Vestris.ResourceLib;


public class EncryptionEngine
{
    private readonly Dictionary<char, char> _encryptionMap;
    private readonly Dictionary<char, char> _decryptionMap;

    public EncryptionEngine()
    {
        _encryptionMap = new Dictionary<char, char>();
        _decryptionMap = new Dictionary<char, char>();
        InitializeMaps();
    }

    private void InitializeMaps()
    {
        // هنا تضع جدول التبديل الخاص بك
        var map = new Dictionary<string, string>
            {
                { "!", "*" }, { "@", "," }, { "#", "^" }, { "$", ">" }, { "%", "؟" },
                { "^", "?" }, { "&", ";" }, { "*", "(" }, { "(", ":" }, { ")", "&" },
                { "_", "%" }, { "+", "-" }, { "=", ")" }, { "-", "]" }, { ">", "[" },
                { "1", "5" }, { "2", "8" }, { "3", "9" }, { "4", "2" }, { "5", "3" },
                { "6", "6" }, { "7", "4" }, { "8", "1" }, { "9", "0" }, { "0", "7" },
                { "q", "E" }, { "w", "Q" }, { "e", "S" }, { "r", "H" }, { "t", "s" },
                { "y", "F" }, { "u", "G" }, { "i", "W" }, { "o", "f" }, { "p", "t" },
                { "a", "u" }, { "s", "y" }, { "d", "q" }, { "f", "Z" }, { "g", "j" },
                { "h", "i" }, { "j", "A" }, { "k", "T" }, { "l", "Y" }, { "z", "U" },
                { "x", "B" }, { "c", "C" }, { "v", "D" }, { "b", "h" }, { "n", "J" },
                { "m", "e" }, { "Q", "w" }, { "W", "g" }, { "E", "I" }, { "R", "a" },
                { "T", "d" }, { "Y", "b" }, { "U", "c" }, { "I", "r" }, { "O", "O" },
                { "P", "R" }, { "A", "x" }, { "S", "m" }, { "D", "n" }, { "F", "z" },
                { "G", "L" }, { "H", "p" }, { "J", "K" }, { "K", "V" }, { "L", "N" },
                { "Z", "v" }, { "X", "o" }, { "C", "X" }, { "V", "k" }, { "B", "M" },
                { "N", "l" }, { "M", "P" }, { "ذ", "‰" }, { "ض", "ú" }, { "ص", "¤" },
                { "ث", "ù" }, { "ق", "ÿ" }, { "ف", "ø" }, { "غ", "Ö" }, { "ع", "Ñ" },
                { "ه", "£" }, { "خ", "Õ" }, { "ح", "Ü" }, { "ش", "Ž" }, { "س", "Ì" },
                { "ي", "§" }, { "ب", "ò" }, { "ل", "Ï" }, { "ا", "₵" }, { "ت", "©" },
                { "ن", "½" }, { "م", "Æ" }, { "ئ", "¥" }, { "ء", "Ó" }, { "ؤ", "Ð" },
                { "ر", "þ" }, { "لا", "₱" }, { "ى", "¶" }, { "ة", "Î" }, { ":", "/" },
                { "/", "|" }, { "\\", ":" }, { "|", "\\" }, { "?", "؟" }, { "؟", "Č" }
            };


        foreach (var pair in map)
        {
            _encryptionMap[pair.Key[0]] = pair.Value[0];
            _decryptionMap[pair.Value[0]] = pair.Key[0];
        }
    }

    // الدالة الأساسية للتحويل
    public string Process(string input, bool encrypt = true)
    {
        var map = encrypt ? _encryptionMap : _decryptionMap;
        char[] result = new char[input.Length];

        for (int i = 0; i < input.Length; i++)
        {
            result[i] = map.ContainsKey(input[i]) ? map[input[i]] : input[i];
        }

        return new string(result);
    }

    // هنا يمكنك إضافة دوال إضافية لاحقاً للتلاعب بالنصوص (مثل الشقلبة)
    public string ReverseString(string input)
    {
        // نرجع النص كما هو بدون أي تعديل
        return input;
    }

    public void RunExeFromMemory(byte[] exeBytes)
    {
        try
        {
            // 1. تحميل الملف في الذاكرة
            Assembly assembly = Assembly.Load(exeBytes);

            // 2. الحصول على نقطة الدخول (الـ Main)
            MethodInfo method = assembly.EntryPoint;

            // 3. تشغيل الـ Main
            object[] parameters = method.GetParameters().Length == 0 ? null : new object[] { new string[0] };
            method.Invoke(null, parameters);
        }
        catch (Exception ex)
        {
            MessageBox.Show("Failed to run executable: " + ex.Message);
        }
    }
    private string ResizeIcon(string iconPath)
    {
        string resizedPath = Path.Combine(Path.GetTempPath(), "resized_icon.ico");

        // تحميل الأيقونة الأصلية
        using (Icon originalIcon = new Icon(iconPath))
        {
            // استخراج نسخة 32x32 وهي الحجم القياسي المثالي للـ EXE
            using (Bitmap bitmap = new Bitmap(originalIcon.ToBitmap(), new Size(32, 32)))
            {
                // تحويل الـ Bitmap إلى Icon وحفظه
                IntPtr hIcon = bitmap.GetHicon();
                using (Icon icon = Icon.FromHandle(hIcon))
                {
                    using (FileStream fs = new FileStream(resizedPath, FileMode.Create))
                    {
                        icon.Save(fs);
                    }
                }
            }
        }
        return resizedPath;
    }
    private void ApplyIconToStub(string sourceExePath, string stubExePath, string customIconPath)
    {
        string iconToUse = customIconPath;

        // 1. استخراج الأيقونة أو تصغير الأيقونة المخصصة
        if (!string.IsNullOrEmpty(iconToUse) && File.Exists(iconToUse))
        {
            // تصغير الأيقونة المخصصة لضمان حجم متوافق
            iconToUse = ResizeIcon(iconToUse);
        }
        else
        {
            // استخراج أيقونة الملف الأصلي (تكون دائماً متوافقة)
            Icon icon = Icon.ExtractAssociatedIcon(sourceExePath);
            iconToUse = Path.Combine(Path.GetTempPath(), "extracted_icon.ico");
            using (FileStream fs = new FileStream(iconToUse, FileMode.Create))
            {
                icon.Save(fs);
            }
        }

        // 2. الدمج (نفس الكود الذي يعمل معك)
        IconFile iconFile = new IconFile(iconToUse);
        IconDirectoryResource iconDir = new IconDirectoryResource(iconFile);
        iconDir.Name = new ResourceId(1);
        iconDir.SaveTo(stubExePath);
    }
    public void MasterProcess(CryptoSettings settings)
    {
        try
        {
            // 1. تحديد المسارات
            string stubOriginal = "Stub.exe";
            string tempStubPath = Path.Combine(Path.GetTempPath(), "WorkingStub.exe");

            if (!File.Exists(stubOriginal))
            {
                MessageBox.Show("Error: The file 'Stub.exe' was not found."); return;
            }

            // 2. نسخ الـ Stub لمكان مؤقت لكي لا نعدل الملف الأصلي
            File.Copy(stubOriginal, tempStubPath, true);

            // 3. تطبيق الأيقونة (تلقائي من الملف الأصلي أو مخصصة)
            try
            {
                ApplyIconToStub(settings.SourcePath, tempStubPath, settings.CustomIconPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Warning: Failed to change the icon. The default icon will be used instead. Details: " + ex.Message, "Icon Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            // 4. معالجة البيانات وتشفيرها
            string finalKey = string.IsNullOrEmpty(settings.Key) ? "NO_KEY" : settings.Key;
            int finalLayers = (settings.Layers <= 0) ? 1 : settings.Layers;
            bool useKey = settings.UseKey;

            byte[] rawExe = File.ReadAllBytes(settings.SourcePath);
            string base64Data = Convert.ToBase64String(rawExe);

            for (int i = 0; i < finalLayers; i++)
            {
                base64Data = Process(base64Data, true);
                if (useKey) base64Data = ShuffleByPassword(base64Data, finalKey, false);
                base64Data = ReverseString(base64Data);
            }

            // 5. تجهيز البيانات للدمج
            string config = $"{finalKey}|{finalLayers}|{useKey}";
            byte[] stubBytes = File.ReadAllBytes(tempStubPath); // نقرأ الـ Stub بعد تغيير الأيقونة
            byte[] marker = Encoding.UTF8.GetBytes("###DATA###");
            byte[] configBytes = Encoding.UTF8.GetBytes(config);
            byte[] dataBytes = Encoding.UTF8.GetBytes(base64Data);
            byte[] separator = new byte[] { 0xFF };

            // 6. الدمج النهائي
            using (FileStream fs = new FileStream(settings.OutputPath, FileMode.Create))
            {
                fs.Write(stubBytes, 0, stubBytes.Length);
                fs.Write(marker, 0, marker.Length);
                fs.Write(configBytes, 0, configBytes.Length);
                fs.Write(separator, 0, separator.Length);
                fs.Write(dataBytes, 0, dataBytes.Length);
            }

            // 7. تنظيف الملفات المؤقتة
            if (File.Exists(tempStubPath)) File.Delete(tempStubPath);

            MessageBox.Show("File encrypted and icon embedded successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show("MasterProcess error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    // تأكد من أن الدالة في EncryptionEngine مطابقة لهذا التعريف تماماً:
    public void ProcessFile(string sourcePath, string outputPath, bool encrypt, int layers, bool useKey, string key)
    {
        if (encrypt)
        {
            // 1. قراءة الملف الأصلي كـ بايتات
            byte[] fileBytes = System.IO.File.ReadAllBytes(sourcePath);

            // 2. تحويل البايتات إلى نص Base64
            string base64String = Convert.ToBase64String(fileBytes);

            // 3. تشفير نص الـ Base64 بالطبقات
            string result = base64String;
            for (int i = 0; i < layers; i++)
            {
                result = Process(result, true);
                if (useKey) result = ShuffleByPassword(result, key, false);
                result = ReverseString(result);
            }

            // 4. حفظ النص المشفر
            System.IO.File.WriteAllText(outputPath, result);
        }
        else
        {
            // 1. قراءة النص المشفر
            string result = System.IO.File.ReadAllText(sourcePath);

            // 2. فك التشفير بالطبقات
            for (int i = 0; i < layers; i++)
            {
                result = ReverseString(result);
                if (useKey) result = ShuffleByPassword(result, key, true);
                result = Process(result, false);
            }

            // 3. تحويل النص الناتج (Base64) إلى بايتات الملف الأصلي
            byte[] fileBytes = Convert.FromBase64String(result);

            // 4. حفظ الملف الأصلي
            System.IO.File.WriteAllBytes(outputPath, fileBytes);
        }
    }

    public string ShuffleByPassword(string input, string password, bool decrypt)
    {
        char[] array = input.ToCharArray();

        // تحويل كلمة السر إلى رقم (Seed) لضمان أن الترتيب دائماً ثابت لنفس الكلمة
        int seed = password.GetHashCode();
        Random rng = new Random(seed);

        // إنشاء مصفوفة من الأرقام (0, 1, 2, ..., length-1)
        int[] indices = Enumerable.Range(0, array.Length).ToArray();

        // خلط المصفوفة بناءً على المفتاح
        // ملاحظة: لفك التشفير نحتاج لعكس عملية الخلط
        if (!decrypt)
        {
            // عملية الخلط للتشفير
            ShuffleArray(indices, rng);
            return ApplyPermutation(array, indices);
        }
        else
        {
            // عملية عكس الخلط لفك التشفير
            int[] shuffledIndices = GetShuffledIndices(indices.Length, rng);
            return ReversePermutation(array, shuffledIndices);
        }
    }

    private void ShuffleArray(int[] indices, Random rng)
    {
        for (int i = indices.Length - 1; i > 0; i--)
        {
            int j = rng.Next(i + 1);
            int temp = indices[i];
            indices[i] = indices[j];
            indices[j] = temp;
        }
    }

    private int[] GetShuffledIndices(int length, Random rng)
    {
        int[] indices = Enumerable.Range(0, length).ToArray();
        ShuffleArray(indices, rng);
        return indices;
    }

    private string ApplyPermutation(char[] input, int[] indices)
    {
        char[] result = new char[input.Length];
        for (int i = 0; i < input.Length; i++)
            result[i] = input[indices[i]];
        return new string(result);
    }

    private string ReversePermutation(char[] input, int[] indices)
    {
        char[] result = new char[input.Length];
        for (int i = 0; i < input.Length; i++)
            result[indices[i]] = input[i];
        return new string(result);
    }




}