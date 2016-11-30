using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace slagmon
{
    public partial class Form1 : Form
    {
        FilePipe m_pipe;

        public Form1()
        {
            InitializeComponent();

            m_pipe = new FilePipe("mon");
            m_pipe.Start(s=>WriteLog(s));
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            textBox1_log.Width = this.Width / 2 - 10;
            textBox2_src.Width = this.Width / 2 - 20;
            var loc = textBox2_src.Location;
            loc.X = this.Width / 2;
            textBox2_src.Location = loc;

            loc= label2_source.Location;
            loc.X = textBox2_src.Location.X;
            label2_source.Location = loc;
        }

        private void WriteLog(string s)
        {
            textBox1_log.AppendText(s + System.Environment.NewLine);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            m_pipe.Update();
            var s = m_pipe.Read();
            if (s!=null) WriteLog(s);
        }

        private void textBox3_input_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;

            var l = textBox3_input.Text.Split('\xa');
            if (l.Length>0)
            {
                var cmd = l[l.Length-1].Trim('>');
                m_pipe.Write(cmd,"unity");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox3_input.AppendText(">");
        }

        private void textBox3_input_TextChanged(object sender, EventArgs e)
        {
            var txt = textBox3_input.Text;
            if (txt.Length>1)
            {
                if (txt[txt.Length-1]=='\x0a')
                {
                    textBox3_input.AppendText(">");
                }
            }
            
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            m_pipe.Tenminate();
        }
    }
}
