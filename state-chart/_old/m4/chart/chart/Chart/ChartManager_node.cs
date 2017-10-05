using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

public partial class ChartManager
{
    public class Node
    {
        public Rectangle     rect;
        public string        state;
        public string        nextstate;
        public List<string>  branches; 

        public List<Point>     arrow_next;              //Nextへの矢印ポイント
        public List<Point>[]   arrow_branch_list;       //branch用の矢印ポイント

        //矢印の終点
        public Point         dstpoint      { get { return new Point(rect.X, rect.Y+rect.Height / 2); } }
        public Point         buf_dstpoint  { get { return DrawUtil.Add_X(dstpoint,-ARROW_BUFFER);    } } //緩衝付

        //nextstateの始点
        public Point         srcpoint_next { get {
            var x = rect.X + rect.Width;
            var y = rect.Y+rect.Height / 2;
            return new Point(x,y);
        } }
        public Point         buf_srcpoint_next { get { return DrawUtil.Add_X(srcpoint_next,ARROW_BUFFER); } } //緩衝付     

        //branch時のテキスト
        private int          branch_height_size  {  get {return ((int)((double)NODE_CHARSIZE * 1.2d +0.99d)); }  }
        public Rectangle     get_branch_text_rect(int i)
        {
            var x = rect.X;
            var y = rect.Y + rect.Height + branch_height_size * i;
            return new Rectangle(x,y,NODE_WIDTH,branch_height_size);
        }

        //branch時の矢印始点
        public Point         srcpoint_branch(int i)
        {
            var trect = get_branch_text_rect(i);
            var x = trect.X + trect.Width;
            var y = trect.Y + trect.Height / 2;
            return new Point(x,y);
        }
        public Point         buf_srcpoint_branch(int i) { return DrawUtil.Add_X(srcpoint_branch(i),ARROW_BUFFER); } //緩衝付 

        //branchesの分解
        private void        branch_tokens(int i, out string cond, out string st)
        {
            cond = string.Empty;
            st   = string.Empty;
            if (branches==null || i >= branches.Count) return;
            var s = branches[i];
            if (string.IsNullOrEmpty(s)) return;
            s = s.Trim();
            if (string.IsNullOrEmpty(s)) return;

            var tokes = s.Split('(');
            if (tokes==null || tokes.Length <=1) return;

            cond = tokes[0].Trim();
            st   = tokes[1].Trim(')',';').Trim();
        }
        public string    branch_cond(int i)
        {
            string cond, st;
            branch_tokens(i, out cond, out st);
            if (string.IsNullOrEmpty(cond)) return null;
            return cond;
        }
        public string    branch_state(int i)
        {
            string cond, st;
            branch_tokens(i, out cond, out st);
            if (string.IsNullOrEmpty(cond)) return null;
            return st;
        }
        #region バンディングボックス
        private Rectangle? _bounding;
        public  Rectangle  bounding
        {
            get {
                if (_bounding==null)
                {
                    var newrect = rect;
                    if (branches!=null) for(var i = 0; i<branches.Count; i++)
                    {
                        newrect = RectUtil.Add(newrect, get_branch_text_rect(i));
                    }
                    _bounding = newrect;
                }
                return (Rectangle)_bounding;
            }
        }

        #endregion
    }
}
