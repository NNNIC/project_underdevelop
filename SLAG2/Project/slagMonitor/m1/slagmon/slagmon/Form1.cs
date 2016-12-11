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
#if enable_resize
            textBox1_log.Width = this.Width / 2 - 10;
            textBox2_src.Width = this.Width / 2 - 20;
            var loc = textBox2_src.Location;
            loc.X = this.Width / 2;
            textBox2_src.Location = loc;

            loc= label2_source.Location;
            loc.X = textBox2_src.Location.X;
            label2_source.Location = loc;
#endif
        }

        private void WriteLog(string s)
        {
            textBox1_log.AppendText(s + System.Environment.NewLine);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            m_pipe.Update();
            for(var loop= 0; loop<5000; loop++)
            { 
                var s = m_pipe.Read();
                if (s!=null)
                { 
                    WriteLog(s);
                }
                else
                {
                    break;
                }
            }
        }
        private void textBox3_input_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                string cmd = null;

                var topindex = textBox3_input.GetFirstCharIndexOfCurrentLine();
                var buf = textBox3_input.Text.Substring(topindex);
                if (string.IsNullOrWhiteSpace(buf)) return;
                var endindex = buf.IndexOf('\xa');
                if (endindex < 0)
                {
                    cmd = buf.TrimStart('>').TrimEnd();
                }
                else
                {
                    cmd = buf.Substring(0, endindex).TrimStart('>').TrimEnd();
                    e.KeyChar= '\x00';
                }

                if (!string.IsNullOrWhiteSpace(cmd))
                {
                    textBox1_log.AppendText("Send Command : " + cmd + Environment.NewLine);
                    m_pipe.Write(cmd, "unity");

                    _loadScriptWhenCmdHas(cmd);
                }

            }

        }

        private void textBox3_input_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Enter)
            //{
            //    string cmd = null;

            //    var topindex = textBox3_input.GetFirstCharIndexOfCurrentLine();
            //    var buf = textBox3_input.Text.Substring(topindex);
            //    if (string.IsNullOrWhiteSpace(buf)) return;
            //    var endindex = buf.IndexOf('\xa');
            //    if (endindex < 0)
            //    {
            //        cmd = buf.TrimStart('>').TrimEnd();
            //    }
            //    else
            //    {
            //        cmd = buf.Substring(0, endindex).TrimStart('>').TrimEnd();
            //    }

            //    if (!string.IsNullOrWhiteSpace(cmd))
            //    {
            //        textBox1_log.AppendText("Send Command : " + cmd + Environment.NewLine);
            //        m_pipe.Write(cmd, "unity");
            //    }

            //}


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

        private void button1clear_Click(object sender, EventArgs e)
        {
            textBox1_log.Clear();
        }

        //---
        private void _loadScriptWhenCmdHas(string cmd)
        {
            if (string.IsNullOrWhiteSpace(cmd)) return;
            var tokens = cmd.Split(' ');
            if (tokens[0].Trim().ToUpper()=="LOAD" && tokens.Length>=2)
            {
                var file = @"N:\Project\test\" + tokens[1].Trim();
                if (File.Exists(file))
                {
                    textBox2_src.Text = File.ReadAllText(file);
                }              
            }
        }

    }
}
