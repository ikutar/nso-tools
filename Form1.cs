using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace NSO_TOOL
{
    public partial class Form1 : Form
    {
        List<Process> processes = new List<Process>();
        List<string> accounts = new List<string>();

        string javaPath = "";
        string jarPath = "";

        public Form1()
        {
            InitializeComponent();
        }

        private void btnSelectJava_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Java (*.exe)|*.exe";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                javaPath = ofd.FileName;
                txtJava.Text = javaPath;
            }
        }

        private void btnSelectJar_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Jar (*.jar)|*.jar";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                jarPath = ofd.FileName;
                txtJar.Text = jarPath;
            }
        }

        private void btnAddAcc_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtAcc.Text))
            {
                accounts.Add(txtAcc.Text);
                listAcc.Items.Add(txtAcc.Text);
                txtAcc.Clear();
            }
        }

        private void btnRemoveAcc_Click(object sender, EventArgs e)
        {
            if (listAcc.SelectedIndex >= 0)
            {
                accounts.RemoveAt(listAcc.SelectedIndex);
                listAcc.Items.RemoveAt(listAcc.SelectedIndex);
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (javaPath == "" || jarPath == "")
            {
                MessageBox.Show("Chọn Java và file .jar trước!");
                return;
            }

            int max = Math.Min(accounts.Count, 15);

            for (int i = 0; i < max; i++)
            {
                Process p = new Process();
                p.StartInfo.FileName = javaPath;

                // truyền tài khoản vào nếu game hỗ trợ
                p.StartInfo.Arguments = $"-jar \"{jarPath}\" {accounts[i]}";

                p.StartInfo.CreateNoWindow = false;
                p.StartInfo.UseShellExecute = true;

                p.Start();

                processes.Add(p);
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            foreach (var p in processes)
            {
                try
                {
                    if (!p.HasExited)
                        p.Kill();
                }
                catch { }
            }

            processes.Clear();
        }
    }
}
