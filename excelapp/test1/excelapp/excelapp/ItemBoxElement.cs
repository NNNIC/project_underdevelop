
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
        public const int box_width = 200;
        public const int box_height = 20;
        public const int updown_button_width  = 20;
        public const int updown_button_height = 20;
        public const int knob_button_width    = 20;
        public const int knob_button_height   = 20;

        private WorkForm   m_form;

        public string  m_id       { get; private set; }

        private int     __x;
        private int     __y;
        //public  int     m_x        { get { return __x;} set { __x = value; _update_location(); } }
        //public  int     m_y        { get { return __y;} set { __y = value; _update_location(); } }

        public TextBox m_textBox  { get; private set; }
        public Button  m_upButton { get; private set; } 
        public Button  m_dwnButton{ get; private set; }
        public Label   m_knobLabel{ get; private set; }

        public void SetLocation(int x, int y)  { __x = x; __y = y; _update_location();   }
        public Point LocalCursor { get {
                return new Point(
                    Cursor.Position.X -  m_form.Location.X - knob_button_width * 2 / 3,
                    Cursor.Position.Y -  m_form.Location.Y - SystemInformation.CaptionHeight - knob_button_height / 2
                );
            } }


        public void SetText(string text)       { m_textBox.Text = text;                   }

        private ItemBoxElement() { }

        private void _update_location()
        {
            //System.Diagnostics.Debug.WriteLine("{0},{1}",__x,__y);
#if xx
            // knob
            m_knobLabel.Location = new Point(
                __x - updown_button_width * 2 - knob_button_width, 
                __y+3);
            // up/down button
            m_upButton.Location = new Point(
                __x - updown_button_width * 2,
                __y);
            m_dwnButton.Location = new Point(
                __x - updown_button_width * 1,
                __y);
            // textbox
            m_textBox.Location = new Point(
                __x,
                __y);
#else
            // knob
            m_knobLabel.Location = new Point(
                __x, 
                __y);
            // up/down button
            m_upButton.Location = new Point(
                __x + knob_button_width,
                __y-3);
            m_dwnButton.Location = new Point(
                __x +  knob_button_width + updown_button_width ,
                __y-3);
            // textbox
            m_textBox.Location = new Point(
                __x +  knob_button_width + updown_button_width * 2 ,
                __y-3);

            m_form.label3.Text = string.Format("knob:{0},{1}",m_knobLabel.Location.X, m_knobLabel.Location.Y);

#endif
        }

        public static ItemBoxElement Create(string id, WorkForm form,int x, int y)
        {
            var ib = new ItemBoxElement();
            ib.m_form = form;
            ib.m_id = id;
            ib.__x = x;
            ib.__y = y;
            ib.create_text_box(x,y);
            ib.create_updown_button(x - 2 * updown_button_width , y,true);
            ib.create_updown_button(x - 1 * updown_button_width , y,false);
            ib.create_knob_label(x - 2 * updown_button_width - knob_button_width, y+3);

            ib._update_location();

            return ib;
        }

        //----
#region knob操作
        private void create_knob_label(int x, int y)
        {
            var lb = new Label();

            lb.Name = m_id + "_knob";

            lb.Width = knob_button_width;
            lb.Height = knob_button_height;
            lb.Size = new Size(knob_button_width,knob_button_height);
            //lb.Location = new Point(x,y);
            lb.Text = "●";
            lb.MouseDown += Lb_MouseDown;
            lb.MouseMove += Lb_MouseMove;
            m_form.Controls.Add(lb);
            
            m_knobLabel = lb;

        }

        //int m_save_mouseX;  int m_save_thisX;
        //int m_save_mouseY;  int m_save_thisY;
        private void Lb_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (m_form.m_draggingItemBox==null)
                {
                    //m_save_mouseX = Cursor.Position.X;
                    //m_save_mouseY = Cursor.Position.Y;
                    //m_save_thisX  = __x;
                    //m_save_thisY  = __y;
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
                    //SetLocation(m_save_thisX + (Cursor.Position.X - m_save_mouseX)  , m_save_thisY + (Cursor.Position.Y - m_save_mouseY ));
                    SetLocation(LocalCursor.X, LocalCursor.Y);
                    System.Diagnostics.Debug.WriteLine("{0},{1}",__x,__y);
                }
            }

        }
#endregion

        private void create_text_box(int x, int y)
        {
            var tb       = new TextBox();
            tb.Name      = m_id + "_textbox";
            tb.Width     = box_width;
            tb.Height    = box_height;
            tb.Multiline = true;
            tb.Size      = new Size(box_width,box_height);
            //tb.Location  = new Point(x,y);
            tb.ReadOnly  = true;
            tb.Text      = "-";

            m_form.Controls.Add(tb);

            m_textBox = tb;
        }
        private void create_updown_button(int x, int y, bool bUpOrDown)
        {
            var bt      = new Button();
            bt.Name     = m_id + (bUpOrDown ? "_up" : "_down") + "_button";
            bt.Width    = updown_button_width;
            bt.Height   = updown_button_height;
            bt.Size     = new Size(updown_button_width, updown_button_height);
            //bt.Location = new Point(x,y);
            bt.Text     = bUpOrDown ? "↑" : "↓";
            bt.Font     = new Font("メイリオ", 5);

            m_form.Controls.Add(bt);

            if (bUpOrDown)
            {
                m_upButton = bt;
            }
            else
            {
                m_dwnButton = bt;
            }
        }


    }
}
