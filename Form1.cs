using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace NSO_TOOL
{
    public class Form1 : Form
    {
        ListBox list;
        TextBox txt;
        Button btnAdd, btnStart, btnStop;

        List<string> accs = new List<string>();
        List<Process> ps = new List<Process>();

        public Form1()
        {
            this.Text = "NSO TOOL";
            this.Width = 400;
            this.Height = 400;

            txt = new TextBox() { Top = 10, Left = 10, Width = 200 };
            btnAdd = new Button() { Text = "Add", Top = 10, Left = 220 };

            list = new ListBox() { Top = 50, Left = 10, Width = 360, Height = 200 };

            btnStart = new Button() { Text = "Start", Top = 270, Left = 10 };
            btnStop = new Button() { Text = "Stop", Top = 270, Left = 100 };

            this.Controls.Add(txt);
            this.Controls.Add(btnAdd);
            this.Controls.Add(list);
            this.Controls.Add(btnStart);
            this.Controls.Add(btnStop);

            btnAdd.Click += (s, e) =>
            {
                accs.Add(txt.Text);
                list.Items.Add(txt.Text);
                txt.Clear();
            };

            btnStart.Click += (s, e) =>
            {
                int max = Math.Min(accs.Count, 15);

                for (int i = 0; i < max; i++)
                {
                    Process p = new Process();
                    p.StartInfo.FileName = "java";
                    p.StartInfo.Arguments = "-jar game.jar";
                    p.Start();

                    ps.Add(p);
                }
            };

            btnStop.Click += (s, e) =>
            {
                foreach (var p in ps)
                {
                    try { if (!p.HasExited) p.Kill(); } catch { }
                }
                ps.Clear();
            };
        }
    }
}
