//<<<include=using_text.txt
using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using ChartViewer = chart.ChartViewer;
using Detail=DrawStateBox.Detail;
using LineType=DrawUtil.LineType;
using D=Define;
//>>>

public partial class DrawStateBox
{
    static float POINT_PER_MILI     { get { return D.POINT_PER_MILI; } }

    static float OUT_WIDTH          { get { return D.OUT_WIDTH;} }//{ get { return 62f * POINT_PER_MILI; } } //外枠幅
    static float GAP                { get { return D.GAP;      } }      //{ get { return 1f  * POINT_PER_MILI; } } //枠のギャップ
    static float IN_WIDTH           { get { return D.IN_WIDTH; } }//{ get { return 58f * POINT_PER_MILI; } } //内枠の幅
    static float IN_HEIGHT          { get { return D.IN_HEIGHT;} }//{ get { return 9f  * POINT_PER_MILI; } } //内枠の高さ

    static float OUTLINE_SIZE       { get { return D.OUTLINE_SIZE; } }//= 2f;

    static float CIRCLE_DIAMETER    { get { return D.CIRCLE_DIAMETER;} }//{ get { return 4f * POINT_PER_MILI; } } //丸の直径

    static Color  OUTFILL_COLOR     { get { return D.OUTFILL_COLOR ; }}//= Color.FromArgb(175,171,171);
    static Color  OUTLINE_COLOR     { get { return D.OUTLINE_COLOR ; }}//    = Color.FromArgb(237,125,49);

    static Color  STATELINE_COLOR   { get { return D.STATELINE_COLOR ; }}//= Color.FromArgb(0,0,0);
    static Color  STATEFILL_COLOR   { get { return D.STATEFILL_COLOR ; }}//= Color.FromArgb(175,171,171);
    static Color  STATETEXT_COLOR   { get { return D.STATETEXT_COLOR ; }}// = Color.White;

    static Color  CONTENTLINE_COLOR { get { return D.CONTENTLINE_COLOR ; }}//= Color.FromArgb(65,113,156);
    static Color  CONTENTFILL_COLOR { get { return D.CONTENTFILL_COLOR ; }}//= Color.FromArgb(91,155,213);
    static Color  CONTENTTEXT_COLOR { get { return D.CONTENTTEXT_COLOR ; }}//= Color.White;

    static Color  BRANCHLINE_COLOR  { get { return D.BRANCHLINE_COLOR ;}}//= Color.FromArgb(65,113,156);
    static Color  BRANCHFILL_COLOR  { get { return D.BRANCHFILL_COLOR ;}}//= Color.FromArgb(255,255,255);
    static Color  BRANCTEXT_COLOR   { get { return D.BRANCTEXT_COLOR  ;}}//= Color.Black;

    static Color  POINT_LINE_COLOR  { get { return D.POINT_LINE_COLOR ; }}//= Color.White;
    static Color  POINT_IN_COLOR    { get { return D.POINT_IN_COLOR  ; }}//= Color.Green;
    static Color  POINT_OUT_COLOR   { get { return D.POINT_OUT_COLOR ; }}//= Color.Red;

    static Color  POINT_OUT_BRANCHES_COLOR(int i) { return D.POINT_OUT_BRANCHES_COLOR(i); }  //{  return Color.FromArgb(235-20*i,0,0); }

    static string FONTNAME          { get { return D.FONTNAME ; }}//= "メイリオ";
    static float  FONTSIZE          { get { return D.FONTSIZE ; }}//= 11;

    static Color  ARROW_COLOR       { get { return D.ARROW_COLOR ;    }} //= Color.White;
    static int    ARROW_SIZE        { get { return D.ARROW_SIZE ;     }} //= 4;
    static LineType ARROW_LINETYPE  { get { return D.ARROW_LINETYPE ; }} //= LineType.STRAIGHT;

    public static Layout CreateLayout(Graphics g,  StateData data,  Detail detail)
    {
        var lo = new Layout();

        var gh = 0f;

        //ステート枠
        gh = GAP;
        {
            var text =     data.State;

            var box_x     = (OUT_WIDTH - IN_WIDTH) / 2;
            var box_size  = DrawUtil.GetTextBoxSize(g,data.State,FONTNAME,FONTSIZE,IN_WIDTH);

            lo.State      = new Rectangle((int)box_x,(int)gh,(int)box_size.Width,(int)box_size.Height);
            lo.text_state = data.State;

            gh  = lo.State.Y + lo.State.Height;
            gh += GAP;
        }
        //コンテンツ
        if (detail == Detail.Detailed)
        {
            var text     = data.GetContent();
            if (!string.IsNullOrEmpty(text))
            {
                var box_x    = (OUT_WIDTH - IN_WIDTH) / 2;
                var box_size = DrawUtil.GetTextBoxSize(g,text,FONTNAME,FONTSIZE,IN_WIDTH);

                lo.Content   = new Rectangle((int)box_x,(int)gh,(int)box_size.Width,(int)box_size.Height);
                lo.text_content = text;

                gh  = ((Rectangle)lo.Content).Y + ((Rectangle)lo.Content).Height;
                gh += GAP;
            }
        }
        //分岐
        var branch_num   = data.GetBranchCount();
        lo.Branches      = new Rectangle[branch_num];
        lo.text_branches = new string[branch_num];
        for(var i = 0; i<branch_num; i++)
        {
            var text     = data.GetBranchApi(i);
            var box_x    = (OUT_WIDTH - IN_WIDTH) / 2;
            var box_size = DrawUtil.GetTextBoxSize(g,text,FONTNAME,FONTSIZE,IN_WIDTH);

            lo.Branches[i]      = new Rectangle((int)box_x,(int)gh,(int)box_size.Width,(int)box_size.Height);
            lo.text_branches[i] = text;

            gh  = lo.Branches[i].Y + lo.Branches[i].Height;
            gh += GAP;
        }

        lo.Frame = new Rectangle(0,0,(int)OUT_WIDTH,(int)gh);

        //ポイント
        {
            var r = (int)(CIRCLE_DIAMETER / 2);
            var d = r * 2;

            Func<int,int,Rectangle> _createPointRect = (x,y)=> {
                return new Rectangle(x-r,y-r,d,d);
            };

            lo.circle_in  = _createPointRect(0-r,              lo.State.Top + lo.State.Height / 2);
            lo.circle_out = _createPointRect(lo.Frame.Right+r, lo.State.Top + lo.State.Height / 2);
            lo.circle_out_branches = new Rectangle[branch_num];
            for(var i =0; i<branch_num; i++)
            {
                lo.circle_out_branches[i] = _createPointRect(lo.Frame.Right+r, lo.Branches[i].Top + lo.Branches[i].Height / 2);
            }
        }
        return lo;
    }

    public static void DrawArrowLine(Graphics g, StateData st)
    {
        if (st==null) return;

        Action<List<Point>> _draw = (plist)=> {
            DrawUtil.DrawLine(g,plist,ARROW_COLOR,ARROW_SIZE,ARROW_LINETYPE);
        };

        //next
        _draw(st.m_ArrowLine_toNext);

        //branches
        if (st.m_ArrowLine_branches!=null)
        {
            for(var i = 0; i<st.m_ArrowLine_branches.Length; i++)
            {
                _draw(st.m_ArrowLine_branches[i]);
            }
        }
    }
}
