using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace EgeUnblockedReader
{
    public partial class Main : Form
    {
        string tempPath = Path.GetTempPath();
        string version = "v1.0";
        WebClient wc = new WebClient();
        public Main()
        {
            InitializeComponent();
        }

        private void btnOpenSelected_Click(object sender, EventArgs e)
        {
            OpenSite();
        }

        void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index = this.listBox1.IndexFromPoint(e.Location);
            if (index != System.Windows.Forms.ListBox.NoMatches)
            {
                OpenSite();
            }
        }
        private void OpenSite()
        {
            if (listBox1.SelectedItem != null)
            {
                string selItm = listBox1.SelectedItem.ToString();
                if (selItm.StartsWith("https://"))
                {
                    Process.Start(selItm);
                }
            }
            else
            {
                MessageBox.Show("Lütfen bir site seçiniz...", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
        private void DownloadList()
        {
            listBox1.Items.Clear();
            wc.DownloadFile("https://imagedeliverynetwork--mimarselimbey.repl.co/unblock/unblocked-sites.txt", Path.Combine(tempPath, "egeunblock.txt"));
            string[] read = File.ReadAllLines(Path.Combine(tempPath, "egeunblock.txt"));
            foreach (var readLine in read)
            {
                listBox1.Items.Add(readLine);
            }
        }

        private void VersionCheck()
        {

            wc.DownloadFile("https://imagedeliverynetwork--mimarselimbey.repl.co/unblock/unblocked-client-ver.txt", Path.Combine(tempPath, "egeunblock-ver.txt"));
            string latestVer = File.ReadAllText(Path.Combine(tempPath, "egeunblock-ver.txt"));
            if (latestVer[0] == version[0])
            {
                if (latestVer != version)
                {
                    if (MessageBox.Show("Engellenmemiş Site Okuyucusu Sürümü: " + latestVer + " Yayınlanmış,\nYeni sürümü indirmek ister misiniz?", "Güncelleme", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                    {
                        wc.DownloadFile("https://imagedeliverynetwork--mimarselimbey.repl.co/unblock/ESIupdater.exe", Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "ESIupdater.exe"));
                        ProcessStartInfo startInfo = new ProcessStartInfo();
                        startInfo.FileName = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "ESIupdater.exe");
                        startInfo.Arguments = Path.GetFileName(Assembly.GetEntryAssembly().Location);
                        Process.Start(startInfo);
                        Application.Exit();

                    }
                }
            }
            File.Delete(Path.Combine(tempPath, "egeunblock-ver.txt"));
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            DownloadList();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (File.Exists(Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "ESIupdater.exe")))
            {
                File.Delete(Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "ESIupdater.exe"));
            }
            VersionCheck();
            listBox1.Items.Clear();
            if (!File.Exists(Path.Combine(tempPath, "egeunblock.txt")))
            {
                DownloadList();
            }

            string[] read = File.ReadAllLines(Path.Combine(tempPath, "egeunblock.txt"));
            foreach (var readLine in read)
            {
                listBox1.Items.Add(readLine);
            }
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            About aboutFrm = new About();
            aboutFrm.ShowDialog();

        }

    }
}
