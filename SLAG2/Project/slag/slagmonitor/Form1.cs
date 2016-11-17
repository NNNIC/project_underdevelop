using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace slagmonitor
{
    public partial class Form1 : Form
    {
        TcpPipe m_pipe;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            m_pipe = new TcpPipe("127.0.0.1",2002);
            m_pipe.Start(s=>textBox1.AppendText(s));
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            m_pipe.Update();
            var msg = m_pipe.Read();
            if (msg!=null)
            {
                textBox1.AppendText(msg + Environment.NewLine);
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
        
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                var s = textBox2.Text;
                if (!string.IsNullOrEmpty(s))
                { 
                    m_pipe.Write("127.0.0.1",2001,s);
                }
                textBox2.Text = null;
            }
        }
    }
}
