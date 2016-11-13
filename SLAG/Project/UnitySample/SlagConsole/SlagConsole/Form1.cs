using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SlagConsole
{
    public partial class Form1 : Form
    {
        static Form1 V;

        TcpPipe       m_pipe;
        Queue<string> m_cmds;

        public Form1()
        {
            V = this;
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            m_pipe = new TcpPipe("127.0.0.1",2002);
            m_pipe.Start(s=>V.WriteLog(s));

            m_cmds = new Queue<string>();
        }
        private void WriteLog(string s)
        {
            if (s!=null)
            { 
                var lines = s.Split('\x0a');
                foreach(var i in lines)
                {
                    var p= i;
                    if (checkBox1.Checked==false)
                    {
                        if (p.StartsWith("<slag>"))
                        {
                            p = p.Substring(6);
                        }
                        else
                        {
                            continue;
                        }
                    }
                    textBox1.AppendText(p + Environment.NewLine);
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            string readmsg =null;
            while(true)
            {
                var s = m_pipe.Read();
                if (s==null) break;

                readmsg += s + Environment.NewLine;
            }
            WriteLog(readmsg);

            m_pipe.Update();
            
            if (m_cmds.Count>0)
            { 
                var msg = m_cmds.Dequeue();
                if (!string.IsNullOrEmpty(msg))
                { 
                    m_pipe.Write("127.0.0.1",2001,msg);
                }
            }
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode== Keys.Enter)
            {
                var s = textBox2.Text.Trim();
                if (!string.IsNullOrEmpty(s))
                {
                    s="<slag>cmd:" + s;
                    m_cmds.Enqueue(s);
                }
                textBox2.Text=null;
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.Text!=null && textBox2.Text == Environment.NewLine)
            {
                textBox2.Text = null;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
