using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace test3
{
    public partial class Form1 : Form
    {
        work m_work;

        public Form1()
        {
            m_work = new work();
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            m_work.Update();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
