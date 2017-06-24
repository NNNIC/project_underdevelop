using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace excelapp
{
    public partial class MainForm : Form
    {
        public Config         m_config = new Config();
        public LoadExcel      m_loadexcel;
        public ItemBoxUtil    m_itemBoxUtil;

        public List<ItemBoxElement> m_itemBoxElementList = new List<ItemBoxElement>();
        public ItemBoxElement m_draggingItemBox = null;

        public MainForm()
        {
            InitializeComponent();
            m_itemBoxUtil = new ItemBoxUtil(this);
        }

        private void testToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_loadexcel = LoadExcel.Load(@"C:\Users\gea01\autoplay\0000_Smoke\Scene.xlsx","MyPageScene");
            m_itemBoxUtil.Draw();
            
        }
    }
}
