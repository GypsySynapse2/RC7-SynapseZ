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
using System.IO.Pipes;

namespace RC7_UI
{
    public partial class Output : Form
    {
        Form mainform;
        string _comIN = "RC7_LOG";

        public Output(Form frm)
        {
            InitializeComponent();
            mainform = frm;
        }

        void sendClient(string text)
        {
            richTextBox1.AppendText("[");
            richTextBox1.AppendText("CLIENT", Color.Blue);
            richTextBox1.AppendText("] ");
            richTextBox1.AppendText(text);
            richTextBox1.AppendText(Environment.NewLine);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }

    }
}
