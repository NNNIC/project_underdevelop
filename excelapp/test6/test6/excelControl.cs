using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Reflection;

using Microsoft.Office.Interop.Excel;
using Microsoft.VisualBasic;

using System.Runtime.InteropServices;
using ExcelApp = Microsoft.Office.Interop.Excel.Application;

// エクセルの起動と終了  -- 正しく終了する
// http://qiita.com/midori44/items/acab9106e6dad9653e73
// https://social.msdn.microsoft.com/Forums/ja-JP/0f210f52-3667-4e66-9dd6-4480eede48de/c-excel-exe?forum=csharpgeneralja
// https://teratail.com/questions/41794
// http://c-sharp-guide.com/?p=185  

namespace excelwork
{
    public class ExcelControl : IDisposable
    {
        public string m_path;

        public Microsoft.Office.Interop.Excel.Application m_app;
        public Microsoft.Office.Interop.Excel.Workbooks   m_wbs;
        public Microsoft.Office.Interop.Excel.Workbook    m_wb;

        public Worksheet   m_ws
        {
            get { return __ws;                                     }
            set { __ws = value;  m_sheetnames = null;              }
        }
        private Worksheet   __ws;
        public  string[]    m_sheetnames;

        public celldata     m_cache;

        public void Dispose() //ref http://ufcpp.net/study/csharp/rm_disposable.html
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        } 

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // 管理（managed）リソースの破棄処理をここに記述します。 
            }
            try {
                if (m_ws!=null)
                {
                   Marshal.ReleaseComObject(m_ws);
                   m_ws = null;
                }
            } catch { m_ws = null; }
            try {
                if (m_wb!=null) {
                    try { m_wb.Close(false); } catch { }
                    Marshal.ReleaseComObject(m_wb);
                    m_wb = null;
                }
            } catch {  m_wb = null; }
            try {
                if (m_wbs!=null)
                {
                    Marshal.ReleaseComObject(m_wbs);
                    m_wbs = null;
                }
            } catch { m_wbs = null;   }

            try {
                if (m_app!=null)
                {
                    try {  m_app.Quit(); } catch { }
                    Marshal.ReleaseComObject(m_app);
                    m_app = null;
                }
            } catch { m_app = null; }            
        }

        ~ExcelControl()
        {
            Dispose(false);
        }

        // アクセスAPI
        public void     SetVisible(bool bVisible)                    { _SetVisible(this,bVisible);       }
        public void     Save()                                       { _Flush(this);  _Save(this);       }
        public void     Close()                                      { _Close(this);                     }
        public object   GetObject(string row, string col)            { return _GetObject(this,row,col);  }
        public object   GetObject(int row, int col)                  { return _GetObject(this,row,col);  }
                                                                                                                     
        public void     SetObject(string row, string col,object val) { _SetObject(this,row,col,val);     }
        public void     SetObject(int row, int col, object val)      { _SetObject(this,row,col,val);     }
        
        /// <summary>
        /// 画面反映
        /// </summary>
        public void     Flush(bool bForce=false)                     { _Flush(this,bForce);              }    
        
        public string[] GetAllSheets()                               { return _GetAllSheets(this);       }
        public void     SetSheet(string name)                        { _SetSheet(this,name);             }
        public void     SetSheet(int num)                            { _SetSheet(this,num);              }
        public string   GetSheetName()                               { return _GetSheetName(this);       }
        public int      GetSheetIndex()                              { return _GetSheetIndex(this);      }
        public string   GetActiveSheetName()                         { return _GetActiveSheetName(this); }
        public int      GetActiveSheetIndex()                        { return _GetActiveSheetIndex(this);}
        public bool     ExistSheet(string name)                      { return _ExistSheet(this,name);    }
        public void     CreateSheet(string name)                     { _CreateSheet(this,name);          }


        //
        #region 便利セルアイテム  ※高速アクセス用
        public class celldata
        {
            public ExcelControl m_wk;
            public class item
            {
                public int   row;
                public int   col;
                public object value;
                public bool  bModified;
            }
            private Dictionary<ulong, item> m_celllist;
            private ulong _makekey(int row, int col) { return (ulong)row * 100000L + (ulong)col; } //辞書に使うキー

            private int m_maxrow;
            private int m_maxcol;

            public celldata(ExcelControl wk)
            {
                m_wk = wk;
                _init();
            }

            private void _init()
            {
                m_celllist = new Dictionary<ulong, item>();
                m_maxrow = 0;
                m_maxcol = 0;
            }

            public object this[string row, string col] //エクセルrow(ベース１)とカラム指定  
            {
                set
                {
                    var row_b1 = util._toNum_nb1(row);
                    var col_b1 = util._toNum_nb1(col);
                    this[row_b1 - 1, col_b1 - 1] = value;
                }
                get
                {
                    var row_b1 = util._toNum_nb1(row);
                    var col_b1 = util._toNum_nb1(col);
                    return this[row_b1 - 1, col_b1 - 1];
                }
            }

            public object this[int row, int col]
            {
                set
                {
                    var key = _makekey(row, col);
                    object org  = null;
                    if (!m_celllist.ContainsKey(key))
                    {
                        _setdummy(row,col);
                    }
                    org = m_celllist[key].value;
                    if (org!=null && org.GetType()==typeof(Range))
                    {
                        var rorg = (Range)org;
                        if (value!=null && value.GetType() == typeof(Range))
                        {
                            util.CopyRange((Range)value,rorg);
                        }
                        else
                        {
                            rorg.Value = value;
                        }
                        m_celllist[key].value = rorg;
                    }
                    else
                    {
                        m_celllist[key].value = value;
                    }
                    m_celllist[key].bModified = true;
                }
                get
                {
                    var key = _makekey(row, col);
                    if (!m_celllist.ContainsKey(key))
                    {
                        _setdummy(row,col);
                    }
                    return m_celllist[key].value;
                }
            }
            private void _setdummy(int row, int col)
            {
                var key = _makekey(row, col);
                if (!m_celllist.ContainsKey(key))
                {
                    var item = new item();
                    item.row = row;
                    item.col = col;
                    item.value = null;
                    m_celllist.Add(key, item);
                    m_maxrow = Math.Max(m_maxrow, row);
                    m_maxcol = Math.Max(m_maxcol, col);
                }
            }
            private void _read(int row, int col)
            {
                var key = _makekey(row, col);
                if (!m_celllist.ContainsKey(key))
                {
                    var item = new item();
                    item.row = row;
                    item.col = col;
                    item.value = m_wk.m_ws.Cells[row+1,col+1]; 
                    m_celllist.Add(key, item);
                    m_maxrow = Math.Max(m_maxrow, row);
                    m_maxcol = Math.Max(m_maxcol, col);
                }
            }

            public void ReadAll()
            {
                _init();

                object[,] rangeArray;
                Range usedRange = null;
                try
                {
                    usedRange = m_wk.m_ws.UsedRange;
                    rangeArray = usedRange.Value;
                    // 二次元配列に対してループを回す
                    var lastrow = rangeArray.GetLength(0);
                    var lastcol = rangeArray.GetLength(1);
                    for (int r = 0; r < lastrow; r++) for (int c = 0; c < lastcol; c++)
                    {
                        this[r, c] = rangeArray[r, c];
                    }
                }
                catch
                {
                    this[0,0] = m_wk.m_ws.Cells[1,1];
                }
                finally { Marshal.ReleaseComObject(usedRange); }
            }

            public void Flush(bool bForce = false)
            {

                var cell1 = m_wk.m_ws.Cells[1,1];
                var cell2 = m_wk.m_ws.Cells[m_maxrow+1,m_maxcol+1];
                //object[,] orgrange =(object[,])((Range)m_wk.m_ws.Range[cell1,cell2]).Value;
                //var r = (Range)m_wk.m_ws.Range["A1","Z100"];
                //var r2 = r.GetEnumerator();

                var rgn = m_wk.m_ws.Range["A1","Z100"];
                rgn.Font.Size=10;
                rgn.Font.Name="メイリオ";
                rgn.NumberFormatLocal = "@";
                rgn.Value2 = "temp";

                object[,] orgrange =(object[,])rgn.Value;
                var len1 = orgrange.GetLength(0);
                var len2 = orgrange.GetLength(1);
                for(var x = 1; x<=len1; x++) for(var y = 1;y<=len2;y++)
                    {
                        orgrange[x,y] = x.ToString() +":" + y.ToString();
                    }
                rgn.Value2 = orgrange;




                //object[,] orgrange =(object[,])((Range)m_wk.m_ws.Range["A1","Z100"]).Value2;
                //var lastrow = orgrange.GetLength(0);
                //var lastcol = orgrange.GetLength(1);
                //foreach (var p in m_celllist)
                //{
                //    var i = p.Value;
                //    if (bForce || i.bModified)
                //    {
                //        var range = (Range)orgrange[i.row+1, i.col+1];
                        
                //        if (range==null)
                //        {
                //            var cellname =util._toAlphabet_nb1(i.col+1) + (i.row+1).ToString();
                //            range = m_wk.m_ws.Range[cellname];
                //        }
                //        if (i.value!=null && i.value.GetType()==typeof(Range))
                //        {
                //            var src = (Range)i.value;
                //            util.CopyRange(src,range);
                //        }
                //        else
                //        {
                //            range.Value = i.value;
                //        }
                //    }
                //}
                //m_wk.m_ws.Range[cell1,cell2] = orgrange;
#if x
                foreach (var p in m_celllist)
                {
                    var i = p.Value;
                    if (bForce || i.bModified)
                    {
                        var src = (Range)i.value;

                        var range = (Range)m_wk.m_ws.Cells[i.row + 1, i.col + 1];
                        util.CopyRange(src,range);
                    }
                }
#else

#endif
            }
        }
#endregion

#region 生成と終了
        /// <summary>
        /// 新規生成
        /// </summary>
        public static ExcelControl Create(string path)
        {
            var wk = new ExcelControl();

            try {
                wk.m_app  = new ExcelApp();
                wk.m_app.DisplayAlerts = false;
                wk.m_path = path;
                wk.m_wbs  = wk.m_app.Workbooks;
                wk.m_wb   = wk.m_wbs.Add();
            }
            catch
            {
                wk.Dispose();
                return null;
            }
            return wk;
        }
        /// <summary>
        /// 既存があれば開く
        /// </summary>
        public static ExcelControl Open(string path)
        {
            ExcelApp app = null;
            app = Find(path);
            if (app ==null)
            {
                return null;
            }

            var work = new ExcelControl();
            work.m_app = app;

            try {
                work.m_wbs = app.Workbooks;
                work.m_app.Workbooks.Open(path);
            }
            catch
            {
                work.Dispose();
                return null;
            }
            return work;
        }
        private static ExcelApp Find(string path)
        {
            ExcelApp app = null;
            try { app = (ExcelApp)Interaction.GetObject(Path.GetFullPath(path),"Excel.Application"); }catch { app = null;}
            return app;
        }    

        private static void _SetVisible(ExcelControl wk, bool bVisible)
        {
            if (wk==null || wk.m_wb==null) return;
            var wb = wk.m_wb;
            wb.Application.Visible = bVisible;
        }
        private static void _Save(ExcelControl wk)
        {
            if (wk==null || wk.m_wb==null) return;
            var wb = wk.m_wb;
            
            try {
                wb.SaveAs(wk.m_path);
            } catch
            {
                MessageBox.Show("Saveに失敗");             
            }
        }
        private static void _Close(ExcelControl wk)
        {
            var bOk = false;
            if (wk==null || wk.m_wb==null)
            {
                bOk = false;
            }
            else
            {
                var wb = wk.m_wb;
                try {
                    wb.Close(true);
                    wk.m_app.Quit();
                    bOk = true;
                }
                catch
                {
                    bOk = false;
                }
            }
            if (!bOk)
            {
                MessageBox.Show("Closeに失敗");             
            }
        }
#endregion

#region 値設定と取得
        /// <summary>
        /// 行とカラムを指定して、値を得る。（指定は文字列）
        /// </summary>
        private static object _GetObject(ExcelControl wk,string row, string col)
        {
            try {
                var col_b1 = util._toNum_nb1(col);
                var row_b1 = int.Parse(row) + 1;
                return _getObject_b0(wk,row_b1,col_b1);
            } catch
            {
                return null;
            }
        }
        /// <summary>
        /// 行とカラムを指定して、値を得る。（指定はInt値-０ベース）
        /// </summary>
        private static object _GetObject(ExcelControl wk,int row, int col)
        {
            return _getObject_b0(wk,row,col);
        }

        //http://qiita.com/kyama0922/items/f0c6449d8734889d1e83 C#でエクセルを読み込みメモ
        private static object _getObject_b1(ExcelControl wk, int row_b1, int col_b1)
        {
            try {
                _cache(wk);
                var obj = wk.m_cache[row_b1-1,col_b1-1];
                return obj;
            } catch {
                return null;
            }
        }
        private static object _getObject_b0(ExcelControl wk, int row_b0, int col_b0)
        {
            try {
                _cache(wk);
                var obj = wk.m_cache[row_b0,col_b0];
                return obj;
            } catch {
                return null;
            }
        }

        private static void _cache(ExcelControl wk)
        {
            if (wk.m_cache != null) return;

            wk.m_cache = new celldata(wk);
            wk.m_cache.ReadAll();
        }

        /// <summary>
        /// 行とカラムを指定して値を設定。（指定は文字列）
        /// </summary>
        private static void _SetObject(ExcelControl wk, string row, string col, object value)
        {
            _cache(wk);
            wk.m_cache[row,col] = value;
            //var row_b1 = util._toNum_nb1(row);
            //var col_b1 = util._toNum_nb1(col);
            //_set(wk, row_b1 - 1, col_b1 -1, value);    
        }
        /// <summary>
        /// 行とカラムを指定して値を設定。（指定は文字列）
        /// </summary>
        private static void _SetObject(ExcelControl wk, int row, int col,object value)
        {
            _cache(wk);
            wk.m_cache[row,col] = value;
            //_set(wk,row,col,value);
        }
        /// <summary>
        /// 書き込み
        /// </summary>
        private  static void _Flush(ExcelControl wk, bool bForce = false)
        {
            _cache(wk);
            wk.m_cache.Flush(bForce);
        }
#endregion;

#region シート関連
        /// <summary>
        /// 全シート名取得
        /// </summary>
        private static string[] _GetAllSheets(ExcelControl wk)
        {
            try {
                if (wk.m_sheetnames!=null) return wk.m_sheetnames;
                var list = new List<string>();
                foreach(var i in wk.m_wb.Sheets)
                {
                    var s = (Worksheet)i;
                    list.Add(s.Name);
                }   
                wk.m_sheetnames = list.ToArray();
            }
            catch
            {
                if (wk!=null) { wk.m_sheetnames = null;}
                return null;
            }
            return wk.m_sheetnames;
        }
        /// <summary>
        /// シート名指定
        /// </summary>
        private static void _SetSheet(ExcelControl wk,string name)
        {
            var index = _getSheetIndex(wk,name);
            if (index >= 0)
            {
                try {
                    wk.m_ws = wk.m_wb.Sheets[index+1];
                } catch { MessageBox.Show("シート設定に失敗");   }
            }
        }
        /// <summary>
        /// シートを番号で指定  ベース0
        /// </summary>
        private static void _SetSheet(ExcelControl wk, int num)
        {
            try {
                    wk.m_ws = wk.m_wb.Sheets[num+1];
            } catch { MessageBox.Show("シート設定に失敗");   }
        }
        /// <summary>
        /// 現在のシート名取得
        /// </summary>
        private static string _GetSheetName(ExcelControl wk)
        {
            try {
                return wk.m_ws.Name;
            }
            catch {
                MessageBox.Show("シート名取得に失敗");
                return null;
            }
        }

        /// <summary>
        /// 現在のシートのインデックス取得 ベース０
        /// </summary>
        private static int _GetSheetIndex(ExcelControl wk)
        {
            try {
                var index =  _getSheetIndex(wk,wk.m_ws.Name);
                return index;
            }
            catch
            {
                MessageBox.Show("シートインデックス取得に失敗");
                return -1;
            }
        }

        /// <summary>
        /// アクティブシート名取得
        /// </summary>
        private static string _GetActiveSheetName(ExcelControl wk)
        {
            try {
                var s =  (Worksheet)wk.m_app.ActiveSheet;
                return s.Name;
            } catch {
                MessageBox.Show("アクティブシート名取得に失敗");
                return null;
            }
        }

        /// <summary>
        /// アクティブシートのインデックス取得 ベース０
        /// </summary>
        private static int _GetActiveSheetIndex(ExcelControl wk)
        {
            try {
                var s =  (Worksheet)wk.m_app.ActiveSheet;
                return _getSheetIndex(wk,s.Name);
            } catch
            {
                MessageBox.Show("アクティブシート取得に失敗");
                return -1;
            }
        }

        /// <summary>
        /// シート名存在確認
        /// </summary>
        private static bool _ExistSheet(ExcelControl wk,string name)
        {
            try {
                return _getSheetIndex(wk,name)>=0;
            }
            catch { return false;}
        }

        /// <summary>
        /// シートの作成
        /// </summary>
        private static void _CreateSheet(ExcelControl wk, string name)
        {
            try {
                wk.m_ws = wk.m_wb.Worksheets.Add();
                wk.m_ws.Name = name;
            }catch { MessageBox.Show("シート生成失敗"); }
        }

        private static int _getSheetIndex(ExcelControl wk, string name)
        {
            try {
                var sheets = _GetAllSheets(wk);
                var index = Array.FindIndex(sheets,i=>i == name);
                return index;
            }
            catch { return -1; }
        }
#endregion

        private static object _get(ExcelControl wk,int row_b0, int col_b0)
        {
            var row = row_b0+1;
            var col = col_b0+1;
            
            return wk.m_ws.Cells[row,col];
        }
        private static void _set(ExcelControl wk, int row_b0, int col_b0, object val)
        {
            var row = row_b0+1;
            var col = col_b0+1;

            var dst   = (Range)wk.m_ws.Cells[row,col];
            var vtype = val.GetType();
            if (vtype == typeof(Range))
            {
                var src = (Range)val;
                util.Copy(src.Font,dst.Font);
                dst.Value = src.Value;
            }
            else
            {
                dst.Value = val;
                wk.m_ws.Cells[row,col] = dst;
            }
        }
    }


    static class util
    {
        //エクセルカラム(ベース１)をアルファベット文字へ
        // ref https://support.microsoft.com/ja-jp/help/833402/how-to-convert-excel-column-numbers-into-alphabetical-characters
        // ref https://memo-c-sharp.blogspot.jp/2015/09/excel.html
        public static string _toAlphabet_nb1(int i_num)
        {
            var num = i_num;
            var alphabet = "ZABCDEFGHIJKLMNOPQRSTUVWXY";
            var columstr = string.Empty;
            int m = 0;

            do {
                m %= 26;
                columstr = alphabet[m] + columstr;
                num /= 26;

            }while(num > 0 && m !=0);

            return columstr;
        }
        //エクセルカラム文字列を数字(ベース１)へ
        public static int _toNum_nb1(string str)
        {
            if (str.Length>0 &&  char.IsDigit(str[0]))
            {
                int x = 0;
                if (int.TryParse(str,out x))
                {
                    return x;
                }
                return 0;
            }
            var columnStr = str.ToUpper();
 
            double result = 0;
            for (int i = 0; i < columnStr.Length; i++)
            {
                int x = columnStr.Length - i - 1;
                int num = Convert.ToChar(columnStr[x]) - 64;
 
                result += num * Math.Pow(26, i);
            }
            return Convert.ToInt32(result);
        }
        public static void Copy<T>(T src, T dst)
        {
            var type = typeof(T);
            var members = type.GetMembers(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

            foreach(var mi in members)
            {
                try {
                    if (mi.MemberType.HasFlag(MemberTypes.Field))
                    {
                        var fi  = type.GetField(mi.Name);
                        var val = fi.GetValue(src);
                        fi.SetValue(src,val);
                    }
                    else if (mi.MemberType.HasFlag(MemberTypes.Property))
                    {
                        var pi  = type.GetProperty(mi.Name);
                        var val = pi.GetValue(src);
                    }
                }
                catch { }
            }
        }
        public static void CopyRange(Range src, Range dst)
        {
            Copy(src.Font,dst.Font);
            dst.Value = src.Value;
        }
    }

}
