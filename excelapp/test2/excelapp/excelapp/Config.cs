using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace excelapp
{
    public class Config
    {
        public string HeaderRow = "3";  //ヘッダ行
        public string BottomRow = "100";//最終行

        public string MainCol   = "P";  //メインで表示するカラム
        public string Range     = "B-P";//表示範囲

    }
}
