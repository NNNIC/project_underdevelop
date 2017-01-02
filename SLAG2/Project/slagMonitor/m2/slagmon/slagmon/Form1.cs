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

namespace slagmon
{
    public partial class Form1 : Form
    {
        public static string m_work_path = @"N:\Project\test";


        Queue<string> m_log;
        FilePipe m_pipe;

        public Form1()
        {
            InitializeComponent();

            m_log = new Queue<string>();

            FilePipe.Log = (s) => {
                lock(m_log)
                {
                    m_log.Enqueue(s);
                }
            };

            m_pipe = new FilePipe("mon");
            m_pipe.Start();
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
        private void WriteVar(string s)
        {
            if (s.ToUpper().StartsWith("@STOP"))
            {
                textBoxVar.Text="";
            }

            textBoxVar.Text += s + Environment.NewLine;
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            for(var loop= 0; loop<5000; loop++)
            { 
                var s = m_pipe.Read();
                if (s!=null&&!string.IsNullOrEmpty(s))
                { 
                    if (s[0]=='@')
                    {
                        WriteVar(s);
                    }   
                    else
                    {                   
                        WriteLog(s);
                    }
                }
                else
                {
                    break;
                }
            }

            lock(m_log)
            {
                while(m_log.Count>0)
                {
                    var s = m_log.Dequeue();
                    textBox1_log.AppendText(s + Environment.NewLine);
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

                    string[] cmdlist = null;
                    bool bBatch = _get_cmdlist_if_batch(cmd, out cmdlist);
                    if (bBatch)
                    {
                        textBox1_log.AppendText("--- [Start Batch] ---" +  Environment.NewLine);
                    }
                    
                    foreach(var i in cmdlist)
                    { 
                        if (!string.IsNullOrWhiteSpace(i))
                        { 
                            textBox1_log.AppendText("Send Command : " + i + Environment.NewLine);

                            m_pipe.Write(i, "unity");

                            _loadScriptWhenCmdHas(i);
                        }
                    }

                    if (bBatch)
                    {
                        textBox1_log.AppendText("---------------------" +  Environment.NewLine);
                    }
                }
            }
        }
        private bool _get_cmdlist_if_batch(string cmd, out string[] cmdlist)
        {
            cmdlist= new string[1] {cmd};
            var tokens = cmd.Split(' ');
            if (tokens==null || tokens.Length<2 || tokens[0].ToLower()!="batch") return false;
            try { 
                cmdlist = File.ReadAllLines(Path.Combine(m_work_path, tokens[1]),Encoding.UTF8);
            } catch { return false; }
            return true;
        }


        private void textBox3_input_KeyDown(object sender, KeyEventArgs e)
        {
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
            m_pipe.Terminate();
        }

        private void button1clear_Click(object sender, EventArgs e)
        {
            textBox1_log.Clear();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            try { 
                var filename = comboBoxFiles.Items[comboBoxFiles.SelectedIndex].ToString().Substring(3);
                util.StartEditor(Path.Combine(m_work_path , filename));
            }
            catch
            {
                ;
            }
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            this.textBox3_input.Focus();
        }

        private void buttonConfig_Click(object sender, EventArgs e)
        {
            var fm = new FormConfig();
            fm.ShowDialog();
        }

        private void buttonFolder_Click(object sender, EventArgs e)
        {
            util.OpenFolder(@"N:\Project\test");
        }

        private void comboBoxFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            _SetSource(comboBoxFiles.SelectedIndex);
        }


        //---
        private void _loadScriptWhenCmdHas(string cmd)
        {
            if (string.IsNullOrWhiteSpace(cmd)) return;
                        var tokens = cmd.Split(' ');
            if (tokens[0].Trim().ToUpper()=="LOAD" && tokens.Length>=2)
            {
                comboBoxFiles.Items.Clear();
                for(int i = 0; i<tokens.Length-1;i++)
                {
                    var tok = tokens[i+1]; 
                    var filename = tok.Trim();
                    comboBoxFiles.Items.Add((i+1).ToString("00") + " " +  filename);
                }
                _SetSource(0);
            }
        }
        private void _SetSource(int index)
        {
            try { 
                if (comboBoxFiles.Items.Count>index)
                {
                    var filename = comboBoxFiles.Items[index].ToString().Substring(3);
                    var path = @"N:\Project\test\" + filename;
                    if (File.Exists(path))
                    {
                        textBox2_src.Text = null;
                        var lines = File.ReadAllLines(path,Encoding.UTF8);
                        for(int i = 0; i<lines.Length; i++)
                        {
                            if (textBox2_src.Text!=null) textBox2_src.Text += Environment.NewLine;
                            textBox2_src.Text += (i+1).ToString("0000") + " : " + lines[i];
                        }

                        //textBox2_src.Text = File.ReadAllText(path,Encoding.UTF8);
                        comboBoxFiles.SelectedIndex = index;
                    }
                }
            }
            catch { }
        }

    }
}
