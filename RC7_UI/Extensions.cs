using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Diagnostics;
using System.Net;
using mshtml;
using Microsoft.Web.WebView2.WinForms;

namespace RC7_UI
{
    public static class RichTextBoxExtensions
    {
        public static void AppendText(this RichTextBox box, string text, Color color)
        {
            box.SelectionStart = box.TextLength;
            box.SelectionLength = 0;

            box.SelectionColor = color;
            box.AppendText(text);
            box.SelectionColor = box.ForeColor;
            box.SelectionStart = box.TextLength;
            box.ScrollToCaret();
        }
    }

    public class Extensions
    {
        public class configReader
        {
            string _tempThemeDir = Application.StartupPath + "\\bin\\Themes\\temp\\";

            Form _form;
            public configReader(Form frm)
            {
                _form = frm;
            }

            public void readConfig()
            {
                try
                {
                    string[] lines = File.ReadAllLines(_tempThemeDir + "Config.txt");
                    foreach (string xline in lines)
                    {
                        string line = xline;
                        line = line.Replace(";", "");
                        string[] obj = line.Split(':');
                        switch (obj[0])
                        {
                            case "FontColor":
                                int R = Convert.ToInt32(obj[1].Split(',')[0]);
                                int G = Convert.ToInt32(obj[1].Split(',')[1]);
                                int B = Convert.ToInt32(obj[1].Split(',')[2]);
                                foreach (Control c in _form.Controls)
                                {
                                    if (c.Name != "ToolBar")
                                    {
                                        c.ForeColor = Color.FromArgb(R, G, B);
                                    }
                                }
                                break;
                        }
                    }
                }
                catch (Exception ex)
                { MessageBox.Show(ex.Message, "Config load error"); }
            }
        }

        public class ScriptIDE
        {
            Control parent;
            WebView2 curBrows;
            string binLocation = Application.StartupPath + "\\bin";
            string defPath = Application.StartupPath + "\\bin\\def";

            public ScriptIDE(Control targ)
            {
                parent = targ;
            }

            async void runJS(string js, WebView2 bs)
            {
                await bs.ExecuteScriptAsync(js);
            }

            void addIntel(string label, string kind, string detail, string insertText, WebView2 bs)
            {
                string label1 = @"""" + label + @"""";
                string kind1 = @"""" + kind + @"""";
                string detail1 = @"""" + detail + @"""";
                string insertText1 = @"""" + insertText + @"""";
                string built = "AddIntellisense(" + label1 + "," + kind1 + "," + detail1 + "," + insertText1 + ")";
                //MessageBox.Show(built);
                runJS(built, bs);
            }

            void addGlobalF(WebView2 bs)
            {
                string[] lines = File.ReadAllLines(defPath + "//globalf.txt");
                foreach (string line in lines)
                {
                    if (line.Contains(':'))
                    {
                        addIntel(line, "Function", line, line.Substring(1), bs);
                    }
                    else
                    {
                        addIntel(line, "Function", line, line, bs);
                    }
                }
            }

            void addGlobalV(WebView2 bs)
            {
                foreach (string line in File.ReadLines(defPath + "//globalv.txt"))
                {
                    addIntel(line, "Variable", line, line, bs);
                }
            }

            void addGlobalNS(WebView2 bs)
            {
                foreach (string line in File.ReadLines(defPath + "//globalns.txt"))
                {
                    addIntel(line, "Class", line, line, bs);
                }
            }

            void addMath(WebView2 bs)
            {
                foreach (string line in File.ReadLines(defPath + "//classfunc.txt"))
                {
                    addIntel(line, "Method", line, line, bs);
                }
            }

            void addBase(WebView2 bs)
            {
                foreach (string line in File.ReadLines(defPath + "//base.txt"))
                {
                    addIntel(line, "Keyword", line, line, bs);
                }
            }

            void initScript()
            {
                runJS("SwitchFontSize(11)", curBrows);
                addBase(curBrows);
                addGlobalF(curBrows);
                addGlobalV(curBrows);
                addGlobalNS(curBrows);
                addMath(curBrows);
            }

            Timer curtm;

            void tmTick(object sender, EventArgs e)
            {
                initScript();
                curtm.Stop();
            }

            void timerhandle(object sender, EventArgs e)
            {
                Timer tm = new Timer();
                tm.Interval = 10;
                tm.Tick += tmTick;
                curtm = tm;
                curtm.Start();
            }

            public void makeIDE()
            {
                WebView2 browser = new WebView2();
                browser.Parent = parent;
                string path = Path.Combine(Directory.GetCurrentDirectory(), "bin", "Monaco.html");
                browser.Source = new Uri(path);
                browser.Dock = DockStyle.Fill;
                curBrows = browser;
                browser.NavigationCompleted += timerhandle;
            }
        }
    }
}
