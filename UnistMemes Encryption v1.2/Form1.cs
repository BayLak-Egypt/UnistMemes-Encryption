using System;
using System.Collections.Generic;
using System.Windows.Forms;
using UnistMemes_Encryption_v1._2;

namespace EncryptionDecryptionApp
{
    public partial class MainForm : Form
    {

        private EncryptionEngine _engine = new EncryptionEngine();
        private bool isKeyEnabled = false; // حالة المفتاح
        private string secretKey = "";     // كلمة السر (يجب أن تدخلها في textBox خاص بالمفتاح)
        private int layersCount = 1; // القيمة الافتراضية
        private string selectedFilePath = ""; // متغير لتخزين مسار الملف المختار
        public MainForm()
        {
            InitializeComponent();

          

        }


        // --- زر التشفير ---
        private void btnEncrypt_Click(object sender, EventArgs e)
        {
            if (panel2.Visible) // نص
            {
                string text = textBox1.Text;
                if (string.IsNullOrEmpty(text)) return;
                string result = text;
                for (int i = 0; i < layersCount; i++)
                {
                    result = _engine.Process(result, true);
                    if (isKeyEnabled) result = _engine.ShuffleByPassword(result, secretKey, false);
                    result = _engine.ReverseString(result);
                }
                txtOutput.Text = result;
            }
            else if (panel3.Visible) // ملف
            {
                PerformFileAction(true);
            }
            else if (panel4.Visible) // عملية تشفير EXE
            {
                if (string.IsNullOrEmpty(textBox3.Text)) return;

                CryptoSettings settings = new CryptoSettings();
                settings.SourcePath = textBox3.Text;
                settings.OutputPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(textBox3.Text), textBox2.Text + "_Locked.exe");

                // التحديث هنا: جلب القيم مباشرة من الـ UI لضمان أنها أحدث القيم
                int.TryParse(Convert.ToString(layersCount), out int layers);
                settings.Layers = layers;
                settings.Key = string.IsNullOrEmpty(secretKey) ? "DEFAULT_KEY" : secretKey;
                settings.UseKey = !string.IsNullOrEmpty(secretKey);

                settings.IsEncrypt = true;
                settings.IsExeOperation = true;

                label8.Text = "Status: Encrypting...";
                progressBar1.Value = 20; // تحديث الـ Progress ليعطي شعوراً بالعملية
                settings.CustomIconPath = checkBox1.Checked ? textBox4.Text : null;
                // استدعاء الجوكر
                _engine.MasterProcess(settings);

                progressBar1.Value = 100;
                label8.Text = "Status: Finished!";
            }
        }

        // --- زر فك التشفير ---
        private void btnDecrypt_Click(object sender, EventArgs e)
        {
            if (panel2.Visible) // نص
            {
                string text = textBox1.Text;
                if (string.IsNullOrEmpty(text)) return;
                string result = text;
                for (int i = 0; i < layersCount; i++)
                {
                    result = _engine.ReverseString(result);
                    if (isKeyEnabled) result = _engine.ShuffleByPassword(result, secretKey, true);
                    result = _engine.Process(result, false);
                }
                txtOutput.Text = result;
            }
            else if (panel3.Visible) // ملف
            {
                PerformFileAction(false);
            }
        }

        // --- دالة موحدة للتعامل مع الملفات (تطلب مكان الحفظ) ---
        private void PerformFileAction(bool isEncrypt)
        {
            if (string.IsNullOrEmpty(selectedFilePath))
            {
                MessageBox.Show("Please select a file first using the Select File button!");
                return;
            }

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.FileName = isEncrypt ? selectedFilePath + ".locked" : selectedFilePath.Replace(".locked", "");

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // محاولة تنفيذ التشفير أو فك التشفير
                    _engine.ProcessFile(selectedFilePath, sfd.FileName, isEncrypt, layersCount, isKeyEnabled, secretKey);

                    // إذا تمت العملية بنجاح، أظهر رسالة النجاح
                    MessageBox.Show(isEncrypt ? "File encrypted successfully!" : "File decrypted successfully!");

                    // تحديث الواجهة عند النجاح فقط
                    label7.Text = isEncrypt ? "Status: Encrypted" : "Status: Decrypted";
                    label7.ForeColor = isEncrypt ? System.Drawing.Color.Red : System.Drawing.Color.Green;

                    txtFileName.Text = System.IO.Path.GetFileName(sfd.FileName);
                    txtFileName.ForeColor = label7.ForeColor;
                }
                catch (Exception ex)
                {
                    // إذا حدث أي خطأ (سواء من الدالة الخاصة بك أو من النظام)، 
                    // سيتم التقاطه هنا ولن يغلق البرنامج!
                    MessageBox.Show(ex.Message, "Process Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void button6_Click(object sender, EventArgs e) // Set/Unset Key
        {
            // هذا الزر يعمل بنفس المنطق لكل الـ Panels
            if (!isKeyEnabled)
            {
                string input = Microsoft.VisualBasic.Interaction.InputBox("Enter the encryption key:", "Key Setup", ""); if (!string.IsNullOrEmpty(input))
                {
                    secretKey = input;
                    isKeyEnabled = true;
                    button6.Text = "Unset Key";
                }
            }
            else
            {
                secretKey = "";
                isKeyEnabled = false;
                button6.Text = "Set Key";
            }
        }

        private void button5_Click(object sender, EventArgs e) // Layers
        {
            string input = Microsoft.VisualBasic.Interaction.InputBox("Enter the number of layers:", "Layers Count", layersCount.ToString()); if (int.TryParse(input, out int newCount) && newCount > 0)
            {
                layersCount = newCount;
                button5.Text = "Layers " + layersCount.ToString();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            panel2.Visible = true;
            panel3.Visible = false;
            panel4.Visible = false;
            btnDecrypt.Visible = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            panel3.Visible = true;
            panel2.Visible = false;
            panel4.Visible = false;
            btnDecrypt.Visible = true;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                selectedFilePath = ofd.FileName;
                string fileName = System.IO.Path.GetFileName(selectedFilePath);

                label7.Visible = true;
                if (fileName.EndsWith(".locked"))
                {
                    label7.Text = "Needs Decryption";
                    label7.ForeColor = System.Drawing.Color.Red;
                }
                else
                {
                    label7.Text = "Ready to Encrypt";
                    label7.ForeColor = System.Drawing.Color.Yellow;
                }

                txtFilePath.Text = selectedFilePath;
                txtFileName.Text = fileName;
                txtFileName.ForeColor = label7.ForeColor;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            panel4.Visible = true;
            panel3.Visible = false;
            panel2.Visible = false;
            btnDecrypt.Visible = false;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            bool isCustomIcon = checkBox1.Checked;

            // تفعيل/تعطيل عناصر الأيقونة بناءً على حالة الـ CheckBox
            button9.Enabled = isCustomIcon; // Browse Icon
            textBox4.Enabled = isCustomIcon; // Dir Icon

            if (!isCustomIcon)
            {
                textBox4.Clear(); // مسح المسار إذا تم إلغاء التفعيل
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog { Filter = "Executable files (*.exe)|*.exe" };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                textBox3.Text = ofd.FileName; // Dir File
                textBox2.Text = System.IO.Path.GetFileNameWithoutExtension(ofd.FileName); // Name File
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog { Filter = "Icon files (*.ico)|*.ico" };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                textBox4.Text = ofd.FileName; // Dir Icon
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form ab = new about();
            ab.ShowDialog();

        }
    }
}
