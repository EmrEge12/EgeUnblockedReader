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
        string version = "v1.01";
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
                MessageBox.Show("Please select a website from the list...", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
        private void DownloadList()
        {
            listBox1.Items.Clear();
            wc.DownloadFile("https://raw.githubusercontent.com/EmrEge12/EgeUnblockedReader/master/data/unblocked-sites.txt", Path.Combine(tempPath, "egeunblock.txt"));
            string[] read = File.ReadAllLines(Path.Combine(tempPath, "egeunblock.txt"));
            listBox1.Items.Clear();
            foreach (var readLine in read)
            {
                listBox1.Items.Add(readLine);
            }
        }

        private void VersionCheck()
        {

            wc.DownloadFile("https://raw.githubusercontent.com/EmrEge12/EgeUnblockedReader/master/data/unblocked-client-ver.txt", Path.Combine(tempPath, "egeunblock-ver.txt"));
            string latestVer = File.ReadAllText(Path.Combine(tempPath, "egeunblock-ver.txt"));
            if (latestVer[0] == version[0])
            {
                if (latestVer != version)
                {
                    if (MessageBox.Show("New Unblocked Reader version " + latestVer + " is released,\nDo you want to update your installation?", "Update", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                    {
                        wc.DownloadFile("https://raw.githubusercontent.com/EmrEge12/EgeUnblockedReader/master/data/EUBupdater.exe", Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "EUBupdater.exe"));
                        ProcessStartInfo startInfo = new ProcessStartInfo();
                        startInfo.FileName = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "EUBupdater.exe");
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
            wc.Headers.Add("User-Agent", "Mozilla/5.0 (Linux; U; Linux i563 ) AppleWebKit/601.48 (KHTML, like Gecko) Chrome/51.0.3629.220 Safari/537");
            if (File.Exists(Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "EUBupdater.exe")))
            {
                File.Delete(Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "EUBupdater.exe"));
            }
            VersionCheck();
            listBox1.Items.Clear();
            DownloadList();

        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            About aboutFrm = new About();
            aboutFrm.ShowDialog();

        }

    }
}
