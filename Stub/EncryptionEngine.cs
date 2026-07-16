using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Stub
{


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

}