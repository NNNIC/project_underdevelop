//<<<include=using_text.txt
using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Detail=DrawStateBox.Detail;
//>>>

public partial class DrawStateBox
{
    public class Layout
    {
        public Rectangle   Frame;

        public Rectangle   State;
        public Rectangle?  Content;
        public Rectangle[] Branches;

        public string      text_state;
        public string      text_content;
        public string[]    text_branches;
    }
}
