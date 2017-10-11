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
    public static void DrawLayout(Graphics g, Layout lo)
    {
        if (lo==null) return;

        //外枠
        var rect0 = lo.offset_Frame;
        DrawUtil.DrawBox_LineAndFill(g,rect0.Location,rect0.Width,rect0.Height,OUTLINE_SIZE,OUTLINE_COLOR,OUTFILL_COLOR);

        //ステート
        {
            //var rect = lo.State;
            var rect1 = lo.offset_State;
            //rect.Offset(pointi);

            DrawUtil.DrawBoxText_LineAndFill(g,lo.text_state,FONTNAME,STATETEXT_COLOR, FONTSIZE,rect1,OUTLINE_SIZE,STATELINE_COLOR,STATEFILL_COLOR);

            //ポイント
            if (lo.circle_in!=null)
            {
                //var rect2 = lo.circle_in;
                //rect2.Offset(pointi);
                var rect2 = lo.offset_circle_in;
                DrawUtil.DrawCircle_LinaAndFill(g,rect2, POINT_LINE_COLOR,POINT_IN_COLOR);
            }
            if (lo.circle_out!=null)
            {
                var rect2 = lo.offset_circle_out;
                DrawUtil.DrawCircle_LinaAndFill(g,rect2, POINT_LINE_COLOR,POINT_OUT_COLOR);
            }
        }
        //コンテンツ
        if (lo.Content!=null)
        {
            var rect = (Rectangle)lo.offset_Content;

            DrawUtil.DrawBoxText_LineAndFill(g,lo.text_content,FONTNAME,CONTENTTEXT_COLOR, FONTSIZE,rect,OUTLINE_SIZE,CONTENTLINE_COLOR,CONTENTFILL_COLOR);
        }
        //ブランチ
        if (lo.Branches!=null && lo.Branches.Length>0)
        {
            for(var i = 0; i<lo.Branches.Length; i++)
            {
                var rect = lo.offset_Branches(i);

                DrawUtil.DrawBoxText_LineAndFill(g,lo.text_branches[i],FONTNAME,BRANCTEXT_COLOR, FONTSIZE,rect,OUTLINE_SIZE,BRANCHLINE_COLOR,BRANCHFILL_COLOR);

                //ポイント
                if (lo.circle_out_branches!=null && i <lo.circle_out_branches.Length)
                {
                    var rect2 = lo.offset_circle_out_branches(i);
                    DrawUtil.DrawCircle_LinaAndFill(g,rect2, POINT_LINE_COLOR,POINT_OUT_BRANCHES_COLOR(i));
                }
            }
        }
    }
    public static void DrawLayout_for_select(Graphics g, Layout lo)
    {
        if (lo==null) return;

        //外枠
        var rect0 = lo.Frame;
        DrawUtil.DrawBox_LineAndFill(g,rect0.Location,rect0.Width,rect0.Height,OUTLINE_SIZE,OUTLINE_COLOR,OUTFILL_COLOR);

        //ステート
        {
            var rect1 = lo.State;

            DrawUtil.DrawBoxText_LineAndFill(g,lo.text_state,FONTNAME,STATETEXT_COLOR, FONTSIZE,rect1,OUTLINE_SIZE,STATELINE_COLOR,STATEFILL_COLOR);

            //ポイント
            if (lo.circle_in!=null)
            {
                var rect2 = lo.circle_in;
                DrawUtil.DrawCircle_LinaAndFill(g,rect2, POINT_LINE_COLOR,POINT_IN_COLOR);
            }
            if (lo.circle_out!=null)
            {
                var rect2 = lo.circle_out;
                DrawUtil.DrawCircle_LinaAndFill(g,rect2, POINT_LINE_COLOR,POINT_OUT_COLOR);
            }
        }
        //コンテンツ
        if (lo.Content!=null)
        {
            var rect = (Rectangle)lo.Content;

            DrawUtil.DrawBoxText_LineAndFill(g,lo.text_content,FONTNAME,CONTENTTEXT_COLOR, FONTSIZE,rect,OUTLINE_SIZE,CONTENTLINE_COLOR,CONTENTFILL_COLOR);
        }
        //ブランチ
        if (lo.Branches!=null && lo.Branches.Length>0)
        {
            for(var i = 0; i<lo.Branches.Length; i++)
            {
                var rect = lo.Branches[i];

                DrawUtil.DrawBoxText_LineAndFill(g,lo.text_branches[i],FONTNAME,BRANCTEXT_COLOR, FONTSIZE,rect,OUTLINE_SIZE,BRANCHLINE_COLOR,BRANCHFILL_COLOR);

                //ポイント
                if (lo.circle_out_branches!=null && i <lo.circle_out_branches.Length)
                {
                    var rect2 = lo.offset_circle_out_branches(i);
                    DrawUtil.DrawCircle_LinaAndFill(g,rect2, POINT_LINE_COLOR,POINT_OUT_BRANCHES_COLOR(i));
                }
            }
        }
    }

}
