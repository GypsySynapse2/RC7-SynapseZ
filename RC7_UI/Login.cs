using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Net;
using Microsoft.Win32;
using System.Runtime.InteropServices;

namespace RC7_UI
{
    public partial class Login : Form
    {
        string _tempThemeDir = Application.StartupPath + "\\bin\\Themes\\temp\\";

        Image main;
        Image idle;
        Image side;
        Image hover;

        public Login()
        {
            InitializeComponent();
        }

        bool _Login(string user, string pass)
        {
            return true;
        }

        private void Login_Load(object sender, EventArgs e)
        {
            try
            {
                main = Image.FromFile(_tempThemeDir + "MainUi.bmp");
                idle = Image.FromFile(_tempThemeDir + "Button_Idle.bmp");
                side = Image.FromFile(_tempThemeDir + "Hide_Side.bmp");
                hover = Image.FromFile(_tempThemeDir + "Button_Hover.bmp");
            } catch (Exception)
            {
                MessageBox.Show("Unable to load theme correctly.\r\nPlease make sure all files are in the temp directory or reinstall RC7.", "Theme Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0);
            }

            Extensions.configReader cr = new Extensions.configReader(this);
            cr.readConfig();

            alphaBlendTextBox1.BorderStyle = BorderStyle.None;
            alphaBlendTextBox2.BorderStyle = BorderStyle.None;
            button1.FlatStyle = FlatStyle.Flat;

            /*Color invert = Color.FromArgb(alphaBlendTextBox1.ForeColor.ToArgb()^0xffffff);
            alphaBlendTextBox1.BackColor = invert;
            alphaBlendTextBox2.BackColor = invert;*/

            this.BackgroundImage = main;
            button1.BackgroundImage = idle;
            panel1.BackgroundImage = side;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string t1 = alphaBlendTextBox1.Text;
            string t2 = alphaBlendTextBox2.Text;
           // if (t1 == "mnoq" && t2 == "trollservice")
            if (true)
            {
                mainForm frm = new mainForm();
                frm.Show();
                this.BackgroundImage = null;
                foreach (Control c in this.Controls)
                {
                    c.BackgroundImage = null;
                }

                main.Dispose();
                idle.Dispose();
                side.Dispose();
                hover.Dispose();

                this.Hide();
            }
            else if (t1 == "cheatbuddy" && t2 == "realpassword123")
            {
                mainForm frm = new mainForm();
                frm.Show();
                this.BackgroundImage = null;
                foreach (Control c in this.Controls)
                {
                    c.BackgroundImage = null;
                }

                main.Dispose();
                idle.Dispose();
                side.Dispose();
                hover.Dispose();

                this.Hide();
            }
            else if (t1 == "a" && t2 == "a")
            {
                mainForm frm = new mainForm();
                frm.Show();
                this.BackgroundImage = null;
                foreach (Control c in this.Controls)
                {
                    c.BackgroundImage = null;
                }

                main.Dispose();
                idle.Dispose();
                side.Dispose();
                hover.Dispose();

                this.Hide();
            }
            else if (t1 == "officiallymelon" && t2 == "melon")
            {
                mainForm frm = new mainForm();
                frm.Show();
                this.BackgroundImage = null;
                foreach (Control c in this.Controls)
                {
                    c.BackgroundImage = null;
                }

                main.Dispose();
                idle.Dispose();
                side.Dispose();
                hover.Dispose();

                this.Hide();
            }
            else if (t1 == "sten" && t2 == "ilikemen")
            {
                mainForm frm = new mainForm();
                frm.Show();
                this.BackgroundImage = null;
                foreach (Control c in this.Controls)
                {
                    c.BackgroundImage = null;
                }

                main.Dispose();
                idle.Dispose();
                side.Dispose();
                hover.Dispose();

                this.Hide();
            }
            else if (t1 == "alex" && t2 == "sigma999")
            {
                mainForm frm = new mainForm();
                frm.Show();
                this.BackgroundImage = null;
                foreach (Control c in this.Controls)
                {
                    c.BackgroundImage = null;
                }

                main.Dispose();
                idle.Dispose();
                side.Dispose();
                hover.Dispose();

                this.Hide();
            }
            else
            {
                MessageBox.Show("Wrong password or username!");
            }
        }

        private void button1_MouseEnter(object sender, EventArgs e)
        {
            button1.BackgroundImage = hover;
        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
            button1.BackgroundImage = idle;
        }

    }
}
