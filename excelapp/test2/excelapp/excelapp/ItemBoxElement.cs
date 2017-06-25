
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace excelapp
{
    public class ItemBoxElement
    {
        /*
           box up/down
        */
        public const int knob_button_width    = 20;
        public const int knob_button_height   = 20;
        public const int updown_button_width  = 20;
        public const int updown_button_height = 20;
        public const int row_box_width        = 40;
        public const int row_box_height       = 20;
        public const int text_box_width       = 200;
        public const int text_box_height      = 20;

        private MainForm   m_form;

        public string  m_id       { get; private set; }

        private int     __x;
        private int     __y;

        private List<Control> m_localSaveControls = new List<Control>();
        private int    m_knobLabel_index;
        private int    m_upButton_index;
        private int    m_dwnButton_index;
        private int    m_rowBox_index;
        private int    m_textBox_index;
        public Label   m_knobLabel{ get { return (Label)  m_localSaveControls[m_knobLabel_index];} }
        public Button  m_upButton { get { return (Button) m_localSaveControls[m_upButton_index]; } }
        public Button  m_dwnButton{ get { return (Button) m_localSaveControls[m_dwnButton_index];} }
        public TextBox m_rowBox   { get { return (TextBox)m_localSaveControls[m_rowBox_index];   } }
        public TextBox m_textBox  { get { return (TextBox)m_localSaveControls[m_textBox_index];  } }

        public void SetLocation(int x, int y)  { __x = x; __y = y; _update_location();   }
        public Point LocalCursor { get {
                return new Point(
                    Cursor.Position.X -  m_form.Location.X - knob_button_width * 2 / 3,
                    Cursor.Position.Y -  m_form.Location.Y - SystemInformation.CaptionHeight - knob_button_height / 2
                );
            } }


        public void SetText(string text)       { m_textBox.Text = text; }
        public void SetRowText(string text)    { m_rowBox.Text = text;  }
        private ItemBoxElement() { }

        private void _update_location()
        {
            m_knobLabel.Location  = new Point(__x                                             , __y);
            m_upButton.Location   = new Point(__x + _total_with_in_localSaveControls(0)       , __y-3);
            m_dwnButton.Location  = new Point(__x + _total_with_in_localSaveControls(1)       , __y-3);
            m_rowBox.Location     = new Point(__x + _total_with_in_localSaveControls(2)       , __y-3);
            m_textBox.Location    = new Point(__x + _total_with_in_localSaveControls(3)       , __y-3);
        }

        public static ItemBoxElement Create(string id, MainForm form,int x, int y)
        {
            var ib = new ItemBoxElement();
            ib.m_form = form;
            ib.m_id = id;
            ib.__x = x;
            ib.__y = y;

            ib.create_knob_label();
            ib.create_updown_button(true);
            ib.create_updown_button(false);
            ib.create_row_box();
            ib.create_text_box();

            ib._update_location();

            return ib;
        }
        public void Destroy()
        {
            foreach(var c in m_localSaveControls)
            {
                m_form.Controls.Remove(c);
            }
        }

        //----
#region knob操作
        private void create_knob_label()
        {
            var lb = new Label();

            lb.Name = m_id + "_knob";

            lb.Width = knob_button_width;
            lb.Height = knob_button_height;
            lb.Size = new Size(knob_button_width,knob_button_height);
            lb.Text = "●";
            lb.MouseDown += Lb_MouseDown;
            lb.MouseMove += Lb_MouseMove;
            m_form.Controls.Add(lb);

            m_knobLabel_index = m_localSaveControls.Count;
            m_localSaveControls.Add(lb);

        }

        private void Lb_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (m_form.m_draggingItemBox==null)
                {
                    m_form.m_draggingItemBox = this;
                    System.Diagnostics.Debug.WriteLine("{0},{1}",__x,__y);
                }
            }
        }
        private void Lb_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if ( m_form.m_draggingItemBox!=null)
                {
                    SetLocation(LocalCursor.X, LocalCursor.Y);
                    System.Diagnostics.Debug.WriteLine("{0},{1}",__x,__y);
                }
            }
        }
#endregion

        private void create_text_box()
        {
            var tb       = new TextBox();
            tb.Name      = m_id + "_textbox";
            tb.Width     = text_box_width;
            tb.Height    = text_box_height;
            tb.Multiline = true;
            tb.Size      = new Size(text_box_width,text_box_height);
            tb.ReadOnly  = true;
            tb.Text      = "-";

            tb.Click += textbox_Click;

            m_form.Controls.Add(tb);

            m_textBox_index = m_localSaveControls.Count;
            m_localSaveControls.Add(tb);
        }

        private void textbox_Click(object sender, EventArgs e)
        {
            var row = ParseUtil.IntParse(m_rowBox.Text);
            if (row!=null) {
                m_form.Update_EditForm((int)row);
            }
        }

        private void create_row_box()
        {
            var tb       = new TextBox();
            tb.Name      = m_id + "_textbox";
            tb.Width     = row_box_width;
            tb.Height    = row_box_height;
            tb.Multiline = true;
            tb.Size      = new Size(row_box_width,row_box_height);
            tb.ReadOnly  = true;
            tb.Text      = "-";

            m_form.Controls.Add(tb);

            m_rowBox_index     = m_localSaveControls.Count;
            m_localSaveControls.Add(tb);

        }
        private void create_updown_button(bool bUpOrDown)
        {
            var bt      = new Button();
            bt.Name     = m_id + (bUpOrDown ? "_up" : "_down") + "_button";
            bt.Width    = updown_button_width;
            bt.Height   = updown_button_height;
            bt.Size     = new Size(updown_button_width, updown_button_height);
            bt.Text     = bUpOrDown ? "↑" : "↓";
            bt.Font     = new Font("メイリオ", 5);

            m_form.Controls.Add(bt);


            if (bUpOrDown)
            {
                m_upButton_index = m_localSaveControls.Count;
            }
            else
            {
                m_dwnButton_index = m_localSaveControls.Count;
            }
            m_localSaveControls.Add(bt);
        }
        private int _get_width_in_localSaveControls(int n)
        {
            var cnt = m_localSaveControls[n];
            return cnt.Size.Width;
        }
        private int _total_with_in_localSaveControls(int n)
        {
            var total = 0;
            for(var i = 0; i<=n;i++) total += _get_width_in_localSaveControls(i);
            return total;
        }
    }
}
