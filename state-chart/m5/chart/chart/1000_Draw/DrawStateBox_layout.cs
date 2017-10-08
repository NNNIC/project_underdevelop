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

        public Rectangle   circle_in; 
        public Rectangle   circle_out;
        
        public Point       point_in  { get { return PointUtil.Add_XY(circle_in.Location,circle_in.Width/ 2 , circle_in.Height/2 ); } }
        public Point       point_out { get { return PointUtil.Add_XY(circle_out.Location,circle_out.Width/2, circle_out.Height/2); } } 

        public Rectangle[]     circle_out_branches;
        
        public Point           point_out_branches(int i)
        {
            if (circle_out_branches!=null && i<circle_out_branches.Length)
            {
                var rect = circle_out_branches[i];
                return PointUtil.Add_XY(rect.Location,rect.Width / 2, rect.Height / 2);
            }
            return default(Point);
        }

        // Offset変換の取得用
        public Point     offset;

        public Rectangle  offset_Frame     { get { var rect = Frame;     rect.Offset(offset); return rect; } }
        public Rectangle  offset_State     { get { var rect = State; rect.Offset(offset); return rect;     } }
        public Rectangle? offset_Content   { get { if (Content==null) return null; var rect = (Rectangle)Content; rect.Offset(offset); return rect; } }
        public Rectangle  offset_Branches(int i) {
            if (Branches!=null && i < Branches.Length){
                var rect = Branches[i];
                rect.Offset(offset);
                return rect;
            }
            return default(Rectangle);
        }

        public Rectangle offset_circle_in  { get { var rect = circle_in;  rect.Offset(offset); return rect; } }
        public Rectangle offset_circle_out { get { var rect = circle_out; rect.Offset(offset); return rect; } }
        public Rectangle offset_circle_out_branches(int i)
        {
            if (circle_out_branches!=null && i < circle_out_branches.Length)
            {
                var rect = circle_out_branches[i];
                rect.Offset(offset);
                return rect;
            }
            return default(Rectangle);
        }
        public Point     offset_point_in  { get { var pos = point_in;  pos.Offset(offset); return pos; } }
        public Point     offset_point_out { get { var pos = point_out; pos.Offset(offset); return pos; } }   
        public Point     offset_point_out_branches(int i)
        {
            var rect= point_out_branches(i);
            rect.Offset(offset);
            return rect;
        }
    }
}
