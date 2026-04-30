using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace NSO_TOOL
{
    public class Form1 : Form
    {
        TextBox txtAcc;
        Button btnAdd, btnStart, btnStop;
        ListBox list;

        List<string> accs = new List<string>();
        List<Process> processes = new List<Process>();

        string jarPath = "game.jar";

        [DllImport("user32.dll")]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter,
            int X, int Y, int cx, int cy, uint uFlags);

        public Form1()
        {
            this.Text = "NSO PRO MAX";
            this.Width = 400;
            this.Height = 450;

            txtAcc = new TextBox() { Top = 10, Left = 10, Width = 200 };
            btnAdd = new Button() { Text = "Add (user|pass)", Top = 10, Left = 220 };

            list = new ListBox() { Top = 50, Left = 10, Width = 360, Height = 250 };

            btnStart = new Button() { Text = "Start", Top = 320, Left = 10 };
            btnStop = new Button() { Text = "Stop", Top = 320, Left = 100 };

            this.Controls.Add(txtAcc);
            this.Controls.Add(btnAdd);
            this.Controls.Add(list);
            this.Controls.Add(btnStart);
            this.Controls.Add(btnStop);

            btnAdd.Click += (s, e) =>
            {
                accs.Add(txtAcc.Text);
                list.Items.Add(txtAcc.Text);
                txtAcc.Clear();
            };

            btnStart.Click += (s, e) =>
            {
                new Thread(() =>
                {
                    int index = 0;

                    foreach (var acc in accs)
                    {
                        Thread t = new Thread(() => RunAccount(acc, index));
                        t.Start();

                        index++;
                        Thread.Sleep(2000);
                    }
                }).Start();
            };

            btnStop.Click += (s, e) =>
            {
                foreach (var p in processes)
                {
                    try { if (!p.HasExited) p.Kill(); } catch { }
                }
                processes.Clear();
            };
        }

        void RunAccount(string acc, int index)
        {
            var parts = acc.Split('|');
            if (parts.Length < 2) return;

            string user = parts[0];
            string pass = parts[1];

            Process p = new Process();
            p.StartInfo.FileName = "java";
            p.StartInfo.Arguments = "-Xms32m -Xmx96m -jar " + jarPath;
            p.Start();

            processes.Add(p);

            Thread.Sleep(5000);

            IntPtr h = p.MainWindowHandle;

            // xếp cửa sổ dạng grid
            int cols = 3;
            int w = 300;
            int hSize = 250;

            int x = (index % cols) * w;
            int y = (index / cols) * hSize;

            SetWindowPos(h, IntPtr.Zero, x, y, w, hSize, 0);

            // login
            for (int i = 0; i < 2; i++)
            {
                if (p.MainWindowHandle != IntPtr.Zero)
                {
                    SendKeys.SendWait(user);
                    SendKeys.SendWait("{TAB}");
                    SendKeys.SendWait(pass);
                    SendKeys.SendWait("{ENTER}");
                    break;
                }

                Thread.Sleep(2000);
            }
        }
    }
}
