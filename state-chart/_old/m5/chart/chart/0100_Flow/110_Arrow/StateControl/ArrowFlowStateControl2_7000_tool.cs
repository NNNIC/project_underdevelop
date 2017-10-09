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
using LineType=DrawUtil.LineType;
using D=Define;
//>>>

public partial class ArrowFlowStateControl2
{
    bool IsHit_statebox(Point a, Point b)
    {
        var rect = _create_rect(a,b);

        foreach(var st in m_stateData)
        {
            if (st!=null && st.m_layout!=null && st.m_layout.offset_Frame!=null)
            {
                if (st.m_layout.offset_Frame.IntersectsWith(rect))
                {
                    //var rectx = st.m_layout.offset_Frame;
                    //rectx.Intersect(rect);
                    //if (!rectx.IsEmpty)
                    //{
                    //    return true;
                    //}
                    return true;
                }
            }
        }
        return false;
    }
    bool IsHit_HorizontalLine(Point a, Point b) //aとbの領域が 他の水平ラインと重なるか？
    {
        var rect = _create_rect(a,b);

        foreach(var st in m_stateData)
        {
            if (st!=null)
            {
                if (st.m_ArrowLine_toNext!=null && _isHit_Line(rect,st.m_ArrowLine_toNext,true))
                {
                    return true;
                }
                if (st.m_ArrowLine_branches!=null && st.m_ArrowLine_branches.Length>0)
                {
                    foreach(var plist in st.m_ArrowLine_branches)
                    {
                        if (_isHit_Line(rect,plist,true))
                        {
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }
    bool IsHit_startLine(StateData st,Point a, Point b) //aとbの領域がステートから出るNextへの開始線およびbranchからの開始線に重ならないかの確認
    {
        if (st==null) return false;
        var rect = _create_rect(a,b);

        Func<List<Point>,bool> _isHit = (plist) => {
            if (plist==null) return false;
            if (plist.Count<=2) return false;
            var rect2 = _create_rect(plist[0],plist[2]);
            if (rect2.IntersectsWith(rect))
            {
                if (b.X == plist[2].X)
                {
                    return true;
                }
            }
            return false;
        };

        if (_isHit(st.m_ArrowLine_toNext))
        {
            return true;
        }

        if (st.m_ArrowLine_branches==null)
        {
            return false;
        }
        foreach(var plist in st.m_ArrowLine_branches)
        {
            if (_isHit(plist))
            {
                return true;
            }
        }
        return false;
    }
    bool IsHit_goalLine(StateData st,Point a, Point b) //stはgoal先,bがgoal
    {
        if (st==null) return false;
        var rect = _create_rect(a,b);

        Func<List<Point>,bool> _isHit = (plist)=> {
            if (plist==null) return false;
            if (plist.Count<=2) return false;
            var rect2 = _create_rect(plist[plist.Count-1],plist[plist.Count-2]);
            if (rect2.IntersectsWith(rect))
            {
                if (b.X == plist[2].X)
                {
                    return true;
                }
            }
            return false;
        };

        foreach(var st2 in m_stateData)
        {
            if (st2==null) continue;
            if (st2.m_dist_nextstate!=null)
            {
                if (st2.m_dist_nextstate == st)
                {
                    if (_isHit(st2.m_ArrowLine_toNext))
                    {
                        return true;
                    }
                }
            }
            if (st2.m_dist_branches!=null)
            {
                for(var i = 0;  i<st2.m_dist_branches.Length; i++)
                {
                    if (st2.m_dist_branches[i] == st)
                    {
                        if (st2.m_ArrowLine_branches!=null && i < st2.m_ArrowLine_branches.Length)
                        {
                            if (_isHit(st2.m_ArrowLine_branches[i]))
                            {
                                return true;
                            }
                        }
                    }
                }
            }
        }
        return false;
    }

    bool _isHit_Line(Rectangle rect, List<Point> plist, bool HorV)
    {
        if (plist==null)       return false;
        if (plist.Count < 2)   return false;
        for(var i = 0; i<plist.Count-1; i++)
        {
            var a = plist[i];
            var b = plist[i+1];
            if (HorV)
            {
                if (a.Y - b.Y == 0)
                {
                    var abrect = _create_rect(a,b);
                    if (abrect.IntersectsWith(rect))
                        return true;
                }
            }
            else
            {
                if (a.X - b.X == 0)
                {
                    var abrect = _create_rect(a,b);
                    if (abrect.IntersectsWith(rect))
                        return true;
                }
            }
        }
        return false;
    }

    Rectangle _create_rect(Point a, Point b)
    {
        var width = Math.Abs(a.X-b.X);
        var height= Math.Abs(a.Y-b.Y);
        if (width==0 ) width = 1;
        if (height==0) height =1;
        var x = Math.Min(a.X,b.X);
        var y = Math.Min(a.Y,b.Y);

        return new Rectangle(x,y,width,height);
    }
}
