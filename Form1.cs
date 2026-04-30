using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NSO_TOOL
{
    public class Form1 : Form
    {
        Button btnStart, btnStop, btnAdd;
        TextBox txtUser, txtPass, txtDelay;
        ListBox listBox;

        List<Account> accounts = new List<Account>();
        List<Process> processes = new List<Process>();

        string jarPath = "";
        int max = 15;

        class Account
        {
            public string u;
            public string p;
        }

        public Form1()
        {
            this.Text = "NSO TOOL";
            this.Width = 400;
            this.Height = 500;

            txtUser = new TextBox() { Top = 10, Left = 10, Width = 150 };
            txtPass = new TextBox() { Top = 40, Left = 10, Width = 150 };
            txtDelay = new TextBox() { Top = 70, Left = 10, Width = 150, Text = "1000" };

            btnAdd = new Button() { Text = "Add", Top = 100, Left = 10 };
            btnStart = new Button() { Text = "Start", Top = 130, Left = 10 };
            btnStop = new Button() { Text = "Stop", Top = 160, Left = 10 };

            listBox = new ListBox() { Top = 200, Left = 10, Width = 350, Height = 200 };

            this.Controls.Add(txtUser);
            this.Controls.Add(txtPass);
            this.Controls.Add(txtDelay);
            this.Controls.Add(btnAdd);
            this.Controls.Add(btnStart);
            this.Controls.Add(btnStop);
            this.Controls.Add(listBox);

            btnAdd.Click += (s, e) =>
            {
                accounts.Add(new Account { u = txtUser.Text, p = txtPass.Text });
                listBox.Items.Add(txtUser.Text);
            };

            btnStart.Click += async (s, e) =>
            {
                int delay = int.Parse(txtDelay.Text);

                foreach (var acc in accounts)
                {
                    if (processes.Count >= max) break;

                    StartGame(acc);
                    await Task.Delay(delay);
                }
            };

            btnStop.Click += (s, e) =>
            {
                foreach (var p in processes)
                    if (!p.HasExited) p.Kill();

                processes.Clear();
            };
        }

        void StartGame(Account acc)
        {
            Process p = new Process();
            p.StartInfo.FileName = "java";
            p.StartInfo.Arguments = "-Xms64m -Xmx128m -jar game.jar";
            p.StartInfo.UseShellExecute = false;
            p.Start();

            processes.Add(p);
        }
    }
}
