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
    public partial class WorkForm : Form
    {
        List<ItemBoxElement>   m_itemBoxElementList;
        public ItemBoxElement  m_draggingItemBox;

        public WorkForm()
        {
            InitializeComponent();
        }

        private void WorkForm_Load(object sender, EventArgs e)
        {
            m_itemBoxElementList = new List<ItemBoxElement>();
            for(var i = 0; i<10; i++)
            {
                var ibe = ItemBoxElement.Create(i.ToString("000"),this,100,(i+1)*40);
            }
                  
        }

        private void M_label_Click(object sender, EventArgs e)
        {
            MessageBox.Show("clicked");
        }

        private void WorkForm_MouseDown(object sender, MouseEventArgs e)
        {
        }

        private void WorkForm_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (m_draggingItemBox!=null)
                {
                    m_draggingItemBox = null;
                }
            }
          
        }



        private void WorkForm_MouseMove(object sender, MouseEventArgs e)
        {
            label1.Text = string.Format("abs :{0},{1}", Cursor.Position.X, Cursor.Position.Y);
            label2.Text = string.Format("form:{0},{1}", Cursor.Position.X - this.Location.X, Cursor.Position.Y - this.Location.Y);

            //if (e.Button == MouseButtons.Left)
            //{
            //    if (m_draggingItemBox!=null)
            //    {
            //        m_draggingItemBox.SetLocation(e.X,e.Y);
            //    }
            //}
        }
    }
}
