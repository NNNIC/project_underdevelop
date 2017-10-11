
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

public partial class Define
{
    public static  float POINT_PER_MILI=2.84f;

    public static float OUT_WIDTH   { get { return 62f * POINT_PER_MILI; } } //外枠幅
    public static float GAP         { get { return 1f  * POINT_PER_MILI; } } //枠のギャップ
    public static float IN_WIDTH    { get { return 58f * POINT_PER_MILI; } } //内枠の幅
    public static float IN_HEIGHT   { get { return 9f  * POINT_PER_MILI; } } //内枠の高さ

    public static float OUTLINE_SIZE = 2f;

    public static float CIRCLE_DIAMETER { get { return 4f * POINT_PER_MILI; } } //丸の直径

    public static Color  OUTLINE_COLOR     = Color.FromArgb(237,125,49);
    public static Color  OUTFILL_COLOR     = Color.FromArgb(175,171,171);

    public static Color  STATELINE_COLOR   = Color.FromArgb(0,0,0);
    public static Color  STATEFILL_COLOR   = Color.FromArgb(175,171,171);
    public static Color  STATETEXT_COLOR   = Color.White;

    public static Color  CONTENTLINE_COLOR = Color.FromArgb(65,113,156);
    public static Color  CONTENTFILL_COLOR = Color.FromArgb(91,155,213);
    public static Color  CONTENTTEXT_COLOR = Color.White;

    public static Color  BRANCHLINE_COLOR  = Color.FromArgb(65,113,156);
    public static Color  BRANCHFILL_COLOR  = Color.FromArgb(255,255,255);
    public static Color  BRANCTEXT_COLOR   = Color.Black;

    public static Color  POINT_LINE_COLOR  = Color.White;
    public static Color  POINT_IN_COLOR    = Color.Green;
    public static Color  POINT_OUT_COLOR   = Color.Red;

    public static Color  POINT_OUT_BRANCHES_COLOR(int i) {  return Color.FromArgb(235-20*i,0,0); }

    public static string FONTNAME          = "メイリオ";
    public static float  FONTSIZE          = 11;

    public static Color  ARROW_COLOR       = Color.White;
    public static int    ARROW_SIZE        = 4;
    public static LineType ARROW_LINETYPE  = LineType.BEZIR;
}
