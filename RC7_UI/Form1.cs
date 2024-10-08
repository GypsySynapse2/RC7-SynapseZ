﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.IO.Compression;
using System.Diagnostics;
using System.Net;
using mshtml;
using Microsoft.Web.WebView2.WinForms;
using Newtonsoft.Json;
using SynapseZAPI;

namespace RC7_UI
{
    public partial class mainForm : Form
    {
        public SynapseZAPI.SynapseZAPI synapseZAPI = new SynapseZAPI.SynapseZAPI();
        string _themeDir = Application.StartupPath + "\\bin\\Themes\\";
        string _tempThemeDir = Application.StartupPath + "\\bin\\Themes\\temp\\";

        string binLocation = Application.StartupPath + "\\bin";
        string defPath = Application.StartupPath + "\\bin\\def";
        string scriptPath = Application.StartupPath + "\\bin\\scripts\\";

        string _comOUT = "RC7_SCRIPT";


        Image side;
        Image back;
        Image save;
        Image WordWrapB;
        Image AutoB;
        Image DownloadU;
        Image Krystal;
        Image Mute;
        Image buttonIdle;
        Image buttonHover;

        public mainForm()
        {
            InitializeComponent();
        }

        void loadTheme()
        {
            try
            {
                //images
                side = Image.FromFile(_tempThemeDir + "Hide_Side.bmp");
                back = Image.FromFile(_tempThemeDir + "MainUi.bmp");
                save = Image.FromFile(_tempThemeDir + "Save_In.bmp");
                WordWrapB = Image.FromFile(_tempThemeDir + "WordWrap_In.bmp");
                AutoB = Image.FromFile(_tempThemeDir + "Auto_In.bmp");
                DownloadU = Image.FromFile(_tempThemeDir + "Google_Drive_In.bmp");
                Krystal = Image.FromFile(_tempThemeDir + "Krystal_In.bmp");
                Mute = Image.FromFile(_tempThemeDir + "Wofly_In.bmp");
                buttonIdle = Image.FromFile(_tempThemeDir + "Button_Idle.bmp");
                buttonHover = Image.FromFile(_tempThemeDir + "Button_Hover.bmp");

                //main form elements
                rightPanel.BackgroundImage = side;
                this.BackgroundImage = back;

                //side buttons
                saveButton.BackgroundImage = save;
                WButton.BackgroundImage = WordWrapB;
                AButton.BackgroundImage = AutoB;
                downloadButton.BackgroundImage = DownloadU;
                roexploitButton.BackgroundImage = Krystal;
                muteButton.BackgroundImage = Mute;

                //main gui buttons
                openButton.BackgroundImage = buttonIdle;
                executeButton.BackgroundImage = buttonIdle;
                clearButton.BackgroundImage = buttonIdle;
            }
            catch (Exception)
            {
                MessageBox.Show("Unable to load theme correctly.\r\nPlease make sure all files are in the temp directory or reinstall RC7.", "Theme Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0);
            }
        }

        void initToolTips()
        {
            ToolTip _saveTT = new ToolTip();
            _saveTT.ShowAlways = true;
            _saveTT.SetToolTip(saveButton, "Save");
            _saveTT.SetToolTip(WButton, "Wordwrap");
            _saveTT.SetToolTip(AButton, "Autorun");
            _saveTT.SetToolTip(downloadButton, "Download");
            _saveTT.SetToolTip(roexploitButton, "Ro-Xploit");
            _saveTT.SetToolTip(muteButton, "Mute");
        }

        void setDefPos()
        {
            Screen rightmost = Screen.AllScreens[0];
            foreach (Screen screen in Screen.AllScreens)
            {
                if (screen.WorkingArea.Right > rightmost.WorkingArea.Right)
                    rightmost = screen;
            }

            this.Left = (rightmost.WorkingArea.Right + 10) - this.Width;
            this.Top = ((rightmost.WorkingArea.Bottom / 2) + 200) - this.Height;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Output outx = new Output(this);
            outx.Show();
            outx.Visible = false;
            Extensions.ScriptIDE ide = new Extensions.ScriptIDE(tabControl1.TabPages[0]);
            tabControl1.TabPages.Add("+");
            ide.makeIDE();
            initToolTips();
            setDefPos();
            loadTheme();
            Extensions.configReader cr = new Extensions.configReader(this);
            cr.readConfig();
        }

        //open
        private void openButton_MouseEnter(object sender, EventArgs e)
        {
            openButton.BackgroundImage = buttonHover;
        }

        private void openButton_MouseLeave(object sender, EventArgs e)
        {
            openButton.BackgroundImage = buttonIdle;
        }

        //execute
        private void executeButton_MouseEnter(object sender, EventArgs e)
        {
            executeButton.BackgroundImage = buttonHover;
        }

        private void executeButton_MouseLeave(object sender, EventArgs e)
        {
            executeButton.BackgroundImage = buttonIdle;
        }

        //clear
        private void clearButton_MouseEnter(object sender, EventArgs e)
        {
            clearButton.BackgroundImage = buttonHover;
        }

        private void clearButton_MouseLeave(object sender, EventArgs e)
        {
            clearButton.BackgroundImage = buttonIdle;
        }

        private void showOutputToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormCollection fc = Application.OpenForms;

            foreach (Form frm in fc)
            {
                if (frm.Name == "Output" && frm.Visible == false)
                {
                    frm.Visible = true;
                }
            }
        }

        private void mainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            //to stop application hanging in background
            Application.Exit();
            Environment.Exit(0);
        }

        private void scriptHubToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            FormCollection fc = Application.OpenForms;

            bool open = false;

            foreach (Form frm in fc)
            {
                if (frm.Name == "ScriptEditor")
                {
                    open = true;
                }
            }

            if (!open)
            {
                ScriptEditor se = new ScriptEditor();
                se.Show();
            }
        }


        private void closeRobloxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process[] processes = Process.GetProcessesByName("RobloxPlayerBeta");
            processes[0].Kill();
        }

        private void openDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(_themeDir);
        }

        private void loadThemeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            string _LoadFile = "";

            file.InitialDirectory = _themeDir;
            file.Filter = "Zip Files|*.zip;*.rar";
            file.Multiselect = false;

            if (file.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                _LoadFile = file.FileName;
            }

            if (_LoadFile == "" || _LoadFile == null)
            {
                MessageBox.Show("No file selected", "Unable to load theme", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                this.BackgroundImage = null;
                foreach (Control c in this.Controls)
                {
                    c.BackgroundImage = null;
                }

                foreach (Control c in rightPanel.Controls)
                {
                    c.BackgroundImage = null;
                }

                side.Dispose();
                back.Dispose();
                save.Dispose();
                WordWrapB.Dispose();
                AutoB.Dispose();
                DownloadU.Dispose();
                Krystal.Dispose();
                Mute.Dispose();
                buttonIdle.Dispose();
                buttonHover.Dispose();

                var files = Directory.GetFiles(_tempThemeDir);

                foreach (string f in files)
                {
                    File.Delete(f);
                }

                ZipFile.ExtractToDirectory(_LoadFile, _tempThemeDir);
                loadTheme();
                Extensions.configReader cr = new Extensions.configReader(this);
                cr.readConfig();
            }
        }

        private void restartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(AppDomain.CurrentDomain.FriendlyName);
            Environment.Exit(0);
        }

        private async void executeButton_Click(object sender, EventArgs e)
        {
            WebView2 bs = (WebView2)tabControl1.SelectedTab.Controls[0];
            string script = await getScript(bs);
            /**
 * Return values:
 * 0 - Execution successful
 * 1 - MainPath is not a valid Directory
 * 2 - Bin Folder not found
 * 3 - Scheduler Folder not found
 * 4 - No access to write file
*/

            synapseZAPI.Execute(Directory.GetCurrentDirectory() /*<- MainPath*/, script);
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            //clear script box
            WebView2 bs = (WebView2)tabControl1.SelectedTab.Controls[0];
            bs.CoreWebView2.ExecuteScriptAsync("SetText(\"\");");
        }

        private void muteButton_Click(object sender, EventArgs e)
        {
            synapseZAPI.Execute(Directory.GetCurrentDirectory() /*<- MainPath*/, "print('nuh uh')");
        }

        private void roexploitButton_Click(object sender, EventArgs e)
        {
            synapseZAPI.Execute(Directory.GetCurrentDirectory() /*<- MainPath*/, "loadstring(game:HttpGet('https://raw.githubusercontent.com/EdgeIY/infiniteyield/master/source'))()");
        }

        private void downloadButton_Click(object sender, EventArgs e)
        {
            synapseZAPI.Execute(Directory.GetCurrentDirectory() /*<- MainPath*/, "print('nuh uh')");
        }
        private void WButton_Click(object sender, EventArgs e)
        {
            synapseZAPI.Execute(Directory.GetCurrentDirectory() /*<- MainPath*/, "print('nuh uh')");
        }

        async Task<string> getScript(WebView2 bs)
        {
            string response = await bs.CoreWebView2.ExecuteScriptAsync("GetText()");
            string script = JsonConvert.DeserializeObject<string>(response);
            return script;
        }

        private void openButton_Click(object sender, EventArgs e)
        {
            WebView2 bs = (WebView2)tabControl1.SelectedTab.Controls[0];
            OpenFileDialog file = new OpenFileDialog();
            string _LoadFile = "";

            file.InitialDirectory = scriptPath;
            file.Filter = "Lua Files|*.lua;*.txt";
            file.Multiselect = false;

            if (file.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                _LoadFile = file.FileName;
            }

            try
            {
                StreamReader sr = new StreamReader(_LoadFile);
                string Script = sr.ReadToEnd();
                sr.Dispose();
                string ScriptFormatted = Script.Replace("\"", @"\""");
                string ScriptLined = ScriptFormatted.Replace(Environment.NewLine, @"\n");
                string Formatted = @"" + "SetText(\"" + ScriptLined + @"""" + ")";
                string Final = Formatted.Replace(@"\\", @"\");
                string Finala = Final.Replace(@"\\", @"\");
                bs.CoreWebView2.ExecuteScriptAsync($"SetText(\"{ScriptLined}\");");
            }
            catch (Exception) { }
        }

        private void creditsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Lead Coders :\r\n          CheatBuddy\r\n          mnoq\r\nGFX :\r\n          RealIndica", "Credits");
        }


        private void tabsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormCollection fc = Application.OpenForms;

            bool open = false;

            foreach (Form frm in fc)
            {
                if (frm.Name == "Tabs")
                {
                    open = true;
                }
            }

            if (!open)
            {
                Tabs outx = new Tabs(this);
                outx.Show();
            }
        }

        int tabCount = 2;

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.TabPages[tabControl1.SelectedIndex].Text == "+")
            {
                tabControl1.TabPages.Insert(tabControl1.TabPages.Count - 1, "(" + tabCount.ToString() + ").lua");
                tabControl1.SelectedIndex = tabControl1.TabPages.Count - 2;
                tabCount++;
                Extensions.ScriptIDE ide = new Extensions.ScriptIDE(tabControl1.TabPages[tabControl1.SelectedIndex]);
                ide.makeIDE();
            }
            else
            {

            }
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControl1.TabPages.Remove(tabControl1.SelectedTab);
        }

        private void tabControl1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                contextMenuStrip1.Show(this.PointToScreen(e.Location));
            }
        }

        private void AButton_Click(object sender, EventArgs e)
        {
            /**
* Return values:
* 0 - Injection successful
* 1 - MainPath is not a valid Directory
* 2 - Launcher not found in MainPath
* 3 - Couldn't start the launcher
*/
            synapseZAPI.Inject(Directory.GetCurrentDirectory() /*<- MainPath*/);
        }
    }
}
