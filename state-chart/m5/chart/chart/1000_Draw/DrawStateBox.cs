﻿//<<<include=using_text.txt
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
    const  float POINT_PER_MILI=2.84f;

    static float OUT_WIDTH   { get { return 62f * POINT_PER_MILI; } } //外枠幅
    static float GAP         { get { return 1f  * POINT_PER_MILI; } } //枠のギャップ
    static float IN_WIDTH    { get { return 58f * POINT_PER_MILI; } } //内枠の幅
    static float IN_HEIGHT   { get { return 9f  * POINT_PER_MILI; } } //内枠の高さ

    static float OUTLINE_SIZE = 2f;

    static float CIRCLE_DIAMETER { get { return 4f * POINT_PER_MILI; } } //丸の直径

    static Color  OUTLINE_COLOR     = Color.FromArgb(237,125,49);
    static Color  OUTFILL_COLOR     = Color.FromArgb(175,171,171);

    static Color  STATELINE_COLOR   = Color.FromArgb(0,0,0);
    static Color  STATEFILL_COLOR   = Color.FromArgb(175,171,171);
    static Color  STATETEXT_COLOR   = Color.White;

    static Color  CONTENTLINE_COLOR = Color.FromArgb(65,113,156);
    static Color  CONTENTFILL_COLOR = Color.FromArgb(91,155,213);
    static Color  CONTENTTEXT_COLOR = Color.White;

    static Color  BRANCHLINE_COLOR  = Color.FromArgb(65,113,156);
    static Color  BRANCHFILL_COLOR  = Color.FromArgb(255,255,255);
    static Color  BRANCTEXT_COLOR   = Color.Black;

    static Color  POINT_LINE_COLOR  = Color.White;
    static Color  POINT_IN_COLOR    = Color.Green;
    static Color  POINT_OUT_COLOR   = Color.Red;

    static Color  POINT_OUT_BRANCHES_COLOR(int i) {  return Color.FromArgb(235-20*i,0,0); }

    static string FONTNAME          = "メイリオ";
    static float  FONTSIZE          = 11;

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

    public static void DrawLayout(Graphics g,PointF point, Layout lo)
    {
        if (lo==null) return;
        var pointi = new Point((int)point.X,(int)point.Y);

        //外枠
        DrawUtil.DrawBox_LineAndFill(g,point,lo.Frame.Width,lo.Frame.Height,OUTLINE_SIZE,OUTLINE_COLOR,OUTFILL_COLOR);

        //ステート
        {
            var rect = lo.State;
            rect.Offset(pointi);

            DrawUtil.DrawBoxText_LineAndFill(g,lo.text_state,FONTNAME,STATETEXT_COLOR, FONTSIZE,rect,OUTLINE_SIZE,STATELINE_COLOR,STATEFILL_COLOR);

            //ポイント
            if (lo.circle_in!=null)
            {
                var rect2 = lo.circle_in;
                rect2.Offset(pointi);
                DrawUtil.DrawCircle_LinaAndFill(g,rect2, POINT_LINE_COLOR,POINT_IN_COLOR);  
            }
            if (lo.circle_out!=null)
            {
                var rect2 = lo.circle_out;
                rect2.Offset(pointi);
                DrawUtil.DrawCircle_LinaAndFill(g,rect2, POINT_LINE_COLOR,POINT_OUT_COLOR);  
            }
        }
        //コンテンツ
        if (lo.Content!=null)
        {
            var rect = (Rectangle)lo.Content;
            rect.Offset(pointi);

            DrawUtil.DrawBoxText_LineAndFill(g,lo.text_content,FONTNAME,CONTENTTEXT_COLOR, FONTSIZE,rect,OUTLINE_SIZE,CONTENTLINE_COLOR,CONTENTFILL_COLOR);
        }
        //ブランチ
        if (lo.Branches!=null && lo.Branches.Length>0)
        {
            for(var i = 0; i<lo.Branches.Length; i++)
            {
                var rect = (Rectangle)lo.Branches[i];
                rect.Offset(pointi);

                DrawUtil.DrawBoxText_LineAndFill(g,lo.text_branches[i],FONTNAME,BRANCTEXT_COLOR, FONTSIZE,rect,OUTLINE_SIZE,BRANCHLINE_COLOR,BRANCHFILL_COLOR);

                //ポイント
                if (lo.circle_out_branches!=null && i <lo.circle_out_branches.Length) 
                {
                    var rect2 = lo.circle_out_branches[i];
                    rect2.Offset(pointi);
                    DrawUtil.DrawCircle_LinaAndFill(g,rect2, POINT_LINE_COLOR,POINT_OUT_BRANCHES_COLOR(i));  
                }
            }

        }
    }
}
