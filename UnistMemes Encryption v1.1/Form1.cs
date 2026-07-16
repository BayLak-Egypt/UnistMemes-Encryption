using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace EncryptionDecryptionApp
{
    public partial class MainForm : Form
    {
        private readonly Dictionary<string, string> encryptionMap;
        private readonly Dictionary<string, string> decryptionMap;

        public MainForm()
        {
            InitializeComponent();

            // جدول التشفير
            encryptionMap = new Dictionary<string, string>
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

            // إنشاء جدول فك التشفير
            decryptionMap = new Dictionary<string, string>();
            foreach (var pair in encryptionMap)
            {
                decryptionMap[pair.Value] = pair.Key;
            }
        }

        private string TransformText(string input, Dictionary<string, string> map)
        {
            var result = string.Empty;
            foreach (char c in input)
            {
                string charString = c.ToString();
                result += map.ContainsKey(charString) ? map[charString] : charString;
            }
            return result;
        }

        private void btnEncrypt_Click(object sender, EventArgs e)
        {
            string inputText = txtInput.Text;
            string encryptedText = TransformText(inputText, encryptionMap);
            txtOutput.Text = encryptedText;
        }

        private void btnDecrypt_Click(object sender, EventArgs e)
        {
            string inputText = txtInput.Text;
            string decryptedText = TransformText(inputText, decryptionMap);
            txtOutput.Text = decryptedText;
        }
    }
}
