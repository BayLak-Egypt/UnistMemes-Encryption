using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UnistMemes_Encryption_v1._2
{
    public partial class about : Form
    {
        public about()
        {
            InitializeComponent();
            LoadSocialLinks();
        }


private void LoadSocialLinks()
    {
        try
        {
            using (System.Net.WebClient client = new System.Net.WebClient())
            {
                string content = client.DownloadString("https://raw.githubusercontent.com/BayLak-Egypt/baylak-egypt.github.io/refs/heads/main/mysocial.txt");

                // دالة Regex لاكتشاف الروابط التي تفتقر إلى https
                // ستبحث عن أي رابط يبدأ بـ (t.me, tiktok.com, instagram.com, إلخ) وتضيف له https://
                string pattern = @"(?<=\s|^)(?!https?://)(www\.|t\.me|tiktok\.com|instagram\.com|github\.com|wa\.me)([^\s]+)";
                string formatted = Regex.Replace(content, pattern, "https://$1$2");

                // استبدال الأسماء بأيقونات
                formatted = formatted.Replace("youtube=", "📺 YouTube: ")
                                     .Replace("github=", "💻 GitHub: ")
                                     .Replace("telegram=", "✈️ Telegram: ")
                                     .Replace("tiktok=", "🎵 TikTok: ")
                                     .Replace("instagram=", "📸 Instagram: ")
                                     .Replace("discord=", "🎮 Discord: ")
                                     .Replace("whatsapp=", "💬 WhatsApp: ");

                richTextBox1.Text = formatted;
                richTextBox1.DetectUrls = true;
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error loading data: " + ex.Message);
        }
    }

        private void richTextBox1_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.LinkText);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string link1 = "https://baylak-egypt.blogspot.com/p/donate.html";
            string link2 = "https://baylak-egypt.github.io/donate.html";

            try
            {
                // محاولة فتح الرابط الأول
                OpenLink(link1);
            }
            catch
            {
                try
                {
                    // إذا فشل الأول، محاولة فتح الرابط الثاني
                    OpenLink(link2);
                }
                catch (Exception ex)
                {
                    // إذا فشل كلاهما، إظهار رسالة الخطأ
                    MessageBox.Show("Error opening the link: " + ex.Message);
                }
            }
        }

        // دالة مساعدة لفتح الرابط لتجنب تكرار الكود
        private void OpenLink(string url)
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            });
        }
    }
}
