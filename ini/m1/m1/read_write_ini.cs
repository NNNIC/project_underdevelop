using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

/*

    READ WRITE INI

    iniファイルの書込みと読込みを行う

    - 書込みの際にコメント保持
    - 文字列を取得
    - 文字列を設定
    - カテゴリとキー値を設定すると、既存時は上書き、新規時はカテゴリの最後に挿入
    - 同一カテゴリが複数ある場合は対応しない
    - 同一キーが複数ある場合は対応しない
    - エスケープ処理はない

    フォーマット

    [カテゴリ名]　　－－　カテゴリ名はカギ括弧で括る

    key = 値　　　　  　　　
    key = "値"　　　－－　スペース等を指定したい場合
    key = "　　　　 －－　改行ができる
      値１
      値２
    "
    key = <<<END    －－　ヒアドキュメント　
      値１
      値２
    END

    ;               －－　セミコロン以降はコメント

*/

namespace read_write_ini
{
    public enum ItemType
    {
        COMMENT,   //コメント行
        CATEGORY,  //カテゴリ名
        KEYVALUE,  //キーと値
    }
    public enum ValueType
    {
        NONE,
        STRING,
        MULTISTRINGS,
        HEREDOC,
        SIMPLE
    }

    public class Item
    {
        public int       lineno_at_read;      //Read時の行番号 ベース0
        public ItemType  type;    
           
        public string    category;

        public string    key;
        public ValueType vtype;
        public string    val;

        public string    comment;
        public string    heredocmark;
    }
    public class Data 
    {
        public string     m_filepath;
        public List<Item> m_list      = new List<Item>();

        public string Get(string Category,string key)
        {
            return null;
        }
        public void Set(string Category, string key, string value, string comment = null)
        {
        }
        public void CreateCategory(string categorynsmr)
        {
        }
    }
    public class IO
    {
        public static Data Read(string file)
        {
            var str = File.ReadAllText(file,Encoding.UTF8);
            Data d = new Data();
            d.m_filepath = Path.GetFullPath(file);

            return Decode.Deserialize(ref d,str);
        }
        public static void Write(Data d, string file=null)
        {
            var str = Encode.Serialize(d);
            if (file==null) file = d.m_filepath;
            File.WriteAllText(file,str,Encoding.UTF8);
        }
    }

    public class Decode
    {
        public static Data Deserialize(ref Data data,string str)
        {
            var lines = str.Split('\x0d','\x0a');
            var save_category = "-unknown-";
            for(var no = 0; no<lines.Length; no++)
            {
                var r = lines[no];
                if (!string.IsNullOrEmpty(r))
                {
                    var s = r.Trim();
                    if (!string.IsNullOrEmpty(s))
                    {
                        var sc = s[0];
                        if (sc == '[')
                        {
                            var close_index = s.IndexOf(']',1);
                            if (close_index < 0) _error("Not find a close bracket",no);           
                            var cat = s.Substring(1,close_index-1);                             //カテゴリ名
                            var cmt = s.Substring(close_index+1);                               //コメント部分

                            _addcategory(data,cat,cmt,no);

                            save_category = cat;
                            continue;
                        }

                        // key = value
                        if (s.IndexOf('=') >= 0)
                        {
                            _interpret_keyvalue(data, save_category, s,lines,ref no);
                            continue;
                        }
                    }
                }
                //その他はコメントとして処理
                _addcomment(data,r,no);
            }
            return data;
        }
        private static void _interpret_keyvalue(Data data,string category, string s, string[] lines, ref int no)
        {
            var tokens = s.Split('=');
            if (tokens==null || tokens.Length<=1) _error("Cannot understand the line #2",no);
            var key = tokens[0].Trim();
            if (string.IsNullOrEmpty(key))  _error("Cannot understand the line #3",no);
            var val = tokens[1].Trim();
            if (string.IsNullOrEmpty(val)) _error("Cannot understand the value",no);

            //ヒアドキュメント
            string eot = null;
            if (val.StartsWith("<<<"))
            {
                eot = val.Substring(3);
                if (eot==null || string.IsNullOrEmpty(eot)) _error("Here doc mark cannot be understand",no);
                var saveno = no;
                no++;
                val = __extruct_heredoc(eot,lines,ref no);
                _addkeyvalue_heredoc(data, category, key, val, saveno,eot);
                return;
            }
            
            //文字列
            if (val[0]=='\"')
            {
                var close_dq_index = val.IndexOf('\"',1);
                if (close_dq_index < 0) //複数行
                {
                    var saveno = no;
                    no++;
                    val = val.Substring(1) + Environment.NewLine;
                    val += __extruct_multilinestring(lines, ref no);
                    _addkeyvalue_multistrings(data, category, key, val, saveno);
                    return;
                }
                //１行
                val     = val.Substring(1,close_dq_index-1);
                var cmt = val.Substring(close_dq_index+1);
                _addkeyvalue_string(data,category,key,val,cmt,no);
                return;
            }

            //以外
            {
                string cmt = null;
                var sp_index  = val.IndexOfAny(new char[2] {' ',';' });
                if (sp_index >= 0)
                {
                    cmt = val.Substring(sp_index);
                    val = val.Substring(0,sp_index);
                }
                _addkeyvalue_simple(data,category,key,val,cmt,no);
                return;
            }
        }
        private static string __extruct_heredoc(string eot,string[] lines,ref int no)
        {
            string val = null;
            for(;no<lines.Length; no++)
            {
                var r = lines[no];
                if (!string.IsNullOrEmpty(r))
                {
                    var s = r.Trim();
                    if (!string.IsNullOrEmpty(s))
                    {
                        if (s==eot)
                        {
                            return val;
                        }
                    }
                }
                val += r + Environment.NewLine;
            }
            return val;
        }
        private static string __extruct_multilinestring(string[] lines, ref int no)
        {
            string val = null;
            for(;no<lines.Length; no++)
            {
                if (val!=null) val+=Environment.NewLine;
                var s = lines[no];
                var closedq_index = s.IndexOf('\"');
                if (closedq_index>=0)
                {
                    val += s.Substring(closedq_index);
                    return val;
                }
                val += s;
            }
            _error("Cannot find close double quote",no);

            return null;
        }
        private static void _addcomment(Data data, string cmt,int line_no)
        {
            var i = new Item();
            i.lineno_at_read = line_no;
            i.type = ItemType.COMMENT;
            i.comment = cmt;
            data.m_list.Add(i);
        }
        private static void _addcategory(Data data, string category, string comment, int line_no)
        {
            var i = new Item();
            i.lineno_at_read = line_no;
            i.type = ItemType.CATEGORY;
            i.category = category;
            i.comment = comment;
            data.m_list.Add(i);
        }
        private static void _addkeyvalue_simple(Data data,string category, string key, string val, string comment, int line_no)
        {
            var i = new Item();
            i.lineno_at_read = line_no;
            i.type           = ItemType.KEYVALUE;
            i.category       = category;
            i.key            = key;
            i.val            = val;
            i.vtype          = ValueType.SIMPLE;
            i.comment        = comment;
            data.m_list.Add(i);
        }
        private static void _addkeyvalue_heredoc(Data data,string category, string key, string val,int line_no,string heredocmark)
        {
            var i = new Item();
            i.lineno_at_read = line_no;
            i.type           = ItemType.KEYVALUE;
            i.category       = category;
            i.key            = key;
            i.val            = val;
            i.vtype          = ValueType.HEREDOC;
            i.heredocmark    = heredocmark;
            data.m_list.Add(i);
        }
        private static void _addkeyvalue_string(Data data, string category, string key, string val, string comment, int line_no)
        {
            var i = new Item();
            i.lineno_at_read = line_no;
            i.type = ItemType.KEYVALUE;
            i.category = category;
            i.key      = key;
            i.val      = val;
            i.vtype    = ValueType.STRING;
            i.comment  = comment;
            data.m_list.Add(i);
        }
        private static void _addkeyvalue_multistrings(Data data, string category, string key, string val, int line_no)
        {
            var i            = new Item();
            i.lineno_at_read = line_no;
            i.type           = ItemType.KEYVALUE;
            i.category       = category;
            i.key            = key;
            i.val            = val;
            i.vtype          = ValueType.MULTISTRINGS;
            data.m_list.Add(i);
        }
        private static void _error(string s, int lineno)
        {
            var os = string.Format("Line {0}:{1}",(lineno+1).ToString("d4"),s);
            throw new Exception(os);
        }
    }

    public class Encode
    {
        public static string Serialize(Data d)
        {
            string s = null;
            foreach(var i in d.m_list)
            {
                if (s!=null) s+= Environment.NewLine;
                s += _comment(i);
                s += _category(i);
                s += _keyvalue_simple(i);
                s += _keyvalue_heredoc(i);
                s += _keyvalue_string(i);
                s += _keyvalue_mutistrings(i);
            }
            return s;
        }
        private static string _comment(Item item)
        {
            if (item.type != ItemType.COMMENT) return null;
            return item.comment;
        }
        private static string _category(Item item)
        {
            if (item.type != ItemType.CATEGORY) return null;
            return string.Format("[{0}]{1}", item.category, item.comment);
        }
        private static string _keyvalue_simple(Item item)
        {
            if (item.type != ItemType.KEYVALUE) return null;
            if (item.vtype!= ValueType.SIMPLE)  return null;

            return string.Format("{0}={1}{2}",item.key,item.val,item.comment);
        }
        private static string _keyvalue_heredoc(Item item)
        {
            if (item.type != ItemType.KEYVALUE) return null;
            if (item.vtype!= ValueType.HEREDOC) return null;

            string s = string.Format("{0}=<<<{1}",item.key,item.heredocmark) + Environment.NewLine;
            s+=item.val + Environment.NewLine;
            s+=item.heredocmark;          
            return s;
        }
        private static string _keyvalue_string(Item item)
        {
            if (item.type != ItemType.KEYVALUE) return null;
            if (item.vtype!= ValueType.STRING) return null;

            return string.Format("{0}=\"{1}\"{2}",item.key,item.val,item.comment);
        }
        private static string _keyvalue_mutistrings(Item item)
        {
            if (item.type != ItemType.KEYVALUE)      return null;
            if (item.vtype!= ValueType.MULTISTRINGS) return null;

            string s = string.Format("{0}=\"{1}", item.key, item.val) + Environment.NewLine + "\"";
            return s;
        }
    }
}
