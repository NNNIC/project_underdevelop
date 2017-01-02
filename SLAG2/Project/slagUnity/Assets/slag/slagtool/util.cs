using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System;

using number = System.Double;
using slagtool.runtime;
using slagtool.runtime.builtin;

namespace slagtool
{
    public class slag
    {
        public static slag       m_curslag;

        public   Guid            m_guid;
        public   string[]        m_idlist;

        //public   string        m_filename;
            
        private  List<YVALUE>  m_exelist;
        private  StateBuffer   m_statebuf;

        public slag()
        {
            m_curslag = this;
            m_guid    = System.Guid.NewGuid();
        }
            
        #region ロード&セーブ
        /// <summary>
        /// ファイルロード
        /// xx.js or xx.txt     : ソースとしてロード
        /// xx.bin or xx.base64 : バイナリとしてロード
        /// 
        /// id : デバッグ時の認識に利用。null時はファイル名となる
        /// </summary>
        public void LoadFile(string filename,string id=null)
        {
            m_curslag = this;
            var ext = Path.GetExtension(filename);
            switch(ext.ToUpper())
            {
                case ".JS":
                    {
                        LoadJSFiles(new string[1] {filename });
                    }
                    break;
                case ".BASE64":
                    {
                        var b64txt = File.ReadAllText(filename);
                        LoadBase64(b64txt);
                    }
                    break;
                case ".bin":
                    {
                        var bin = File.ReadAllBytes(filename);
                        LoadBin(bin);
                    }
                    break;
                default:
                    throw new SystemException("unexpected");
            }
        }
        /// <summary>
        /// 拡張子JSのファイル（複数）をロード
        /// </summary>
        /// <param name="filenames"></param>
        public void LoadJSFiles(string[] filenames)
        {
            m_curslag = this;
            //m_id = Path.GetFileNameWithoutExtension(filenames[0]);
            //m_filename = filenames[0];
            var ids = new List<string>();
            var sources = new List<string>();
            Array.ForEach(filenames,f=> {
                sources.Add(File.ReadAllText(f));
                ids.Add(Path.GetFileNameWithoutExtension(f));
            });

            m_idlist = ids.ToArray();
            m_exelist = util_sub.Compile(sources);
        }
        /// <summary>
        /// テキストソースロード
        /// id はデバッグ時の認識に利用
        /// </summary>
        public void LoadSrc(string src,string id = null)
        {
            m_curslag = this;
            m_idlist = id!=null ? new string[] { id} : null;
            m_exelist = util_sub.Compile(src);
        }
        /// <summary>
        /// バイナリロード
        /// </summary>
        public void LoadBin(byte[] bin)
        {
            m_curslag = this;
            var d     = deserialize(bin);
            m_guid    = d.guid;
            m_idlist  = d.ids;
            m_exelist = d.list;
        }
        /// <summary>
        /// Base64ロード
        /// </summary>
        public void LoadBase64(string base64str)
        {
            m_curslag = this;
            var bin    = Convert.FromBase64String(base64str);
            LoadBin(bin);
        }
        public void SaveBin(string filename)
        {
            m_curslag = this;
            var bytes = GetBin();
            File.WriteAllBytes(filename,bytes);
        }
        public byte[] GetBin()
        {
            m_curslag = this;
            return serialize(m_exelist,m_guid,m_idlist);
        }
        public string GetBase64()
        {
            m_curslag = this;
            var bytes = GetBin();
            return Convert.ToBase64String(bytes);
        }
        #endregion
        public void Run()
        {
            m_curslag = this;
            m_statebuf = new StateBuffer();
            runtime.builtin.builtin_func.Init();

            var save = Time.realtimeSinceStartup;
            run_script.run(m_exelist[0],m_statebuf);
            Debug.Log(Time.realtimeSinceStartup- save);
        }
        #region 関数関連
        public bool ExistFunc(string funcname)
        {
            m_curslag = this;
            return builtin_func.IsFunc(funcname);
        }
        public object CallFunc(string funcname,object[] param=null)
        {
            m_curslag = this;
            var fv = (YVALUE)m_statebuf.get_func(funcname);
            if (fv==null)
            {
                if (builtin_func.IsFunc(funcname))
                {
                    return builtin_func.Run(funcname,param,m_statebuf);
                }
                throw new SystemException("CallFunc : Not Found Function : " + funcname);
            }
            if (fv!=null)
            {
                var ol = param;
                var numofparam= ol!=null ? ol.Length : 0;
                m_statebuf.set_funcwork();
                {
                    var fvbk = slagtool.runtime.util.normalize_func_bracket(fv.list_at(1).list_at(1)); //ファンクション定義部の引数部分
                    if (   fvbk.list.Count != numofparam)
                    {
                        slagtool.runtime.util._error("number of arguments in valid.");
                    }
                    int n = 0;
                    if (fvbk!=null) for(int i = 0; i<fvbk.list.Count; i+=2)
                    {
                        var varname = fvbk.list_at(i).GetString();//定義側の変数名
                        object o = ol!=null && n < ol.Length ? ol[n] : null;
                        m_statebuf.define(varname, o);
                        n++;
                    }
                    m_statebuf = run_script.run(fv.list_at(2),m_statebuf);
                    m_statebuf.breaknone();
                }
                m_statebuf.reset_funcwork();

                return m_statebuf.m_cur;
            }

            throw new SystemException("CallFunc : Not Found The Function : " + funcname);
        }
        #endregion

        #region 変数関連
        public bool ExistVal(string name)
        {
            name = name.ToUpper();
            if (m_statebuf!=null&&m_statebuf.m_root_dic!=null)
            {
                return m_statebuf.m_root_dic.ContainsKey(name);
            }
            return false;
        }
        public object GetVal(string name)
        {
            var ret = _getval(name);
            if (ret==null) throw new SystemException("GetVal : Not Found Valriable : " + name);
            return ret;
        }
        private object _getval(string name)
        {
            name = name.ToUpper();
            if (m_statebuf!=null&&m_statebuf.m_root_dic!=null)
            {
                var dic = m_statebuf.m_root_dic;
                if (dic.ContainsKey(name))
                {
                    return dic[name];
                }
            }
            return null;
        }
        public number GetNumVal(string name)
        {
            var ret = _getval(name);
            if (ret==null)                       throw new SystemException("GetNumVal : Not Found Valriable : "     + name);
            if (ret.GetType() != typeof(number)) throw new SystemException("GetNumVal : Valriable is not Number : " + name);
                
            return (number)ret;
        
        }
        public string GetStrVal(string name)
        {
            var ret = _getval(name);
            if (ret==null)                       throw new SystemException("GetStrVal : Not Found Valriable : "     + name);
            if (ret.GetType() != typeof(string)) throw new SystemException("GetStrVal : Valriable is not String : " + name);
                
            return (string)ret;
        }
        public void SetVal(string name, object val,bool bCreateIfNotExist=true)
        {            
            if (!_setval(name,val,bCreateIfNotExist))
            {
                throw new SystemException("SetVal : Fail to Set ; " + name);
            }
        }
        public bool _setval(string name, object val, bool bCreateIfNotExist)
        {
            name = name.ToUpper();
            if (ExistVal(name))
            {
                m_statebuf.m_root_dic[name] = val;
                return true;
            }
            if (bCreateIfNotExist)
            {
                m_statebuf.m_root_dic[name] = val;
                return true;
            }
            return false;
        }
        public void SetNumVal(string name, number val,bool bCreateIfNotExist=true)
        {
            if (!_setval(name,val,bCreateIfNotExist))
            {
                throw new SystemException("SetNumVal : Fail to Set ; " + name);
            }
        }
        public void SetStrVal(string name, string val,bool bCreateIfNotExist=true)
        {
            if (!_setval(name,val,bCreateIfNotExist))
            {
                throw new SystemException("SetStrVal : Fail to Set ; " + name);
            }
        }
        #endregion

        [System.Serializable]
        public class SaveFormat
        {
            public Guid         guid;
            public string[]     ids;
            public List<YVALUE> list;
        }

        private SaveFormat deserialize(byte[] bin)
        {
            var bf = new BinaryFormatter();
            using (var ms = new MemoryStream(bin))
            {
                return (SaveFormat)bf.Deserialize(ms);
            }
        }
        private byte[] serialize(List<YVALUE> list, Guid guid, string[] ids)
        {
            var d = new SaveFormat();
            d.guid = guid;
            d.ids   = ids;
            d.list = list;
            var bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms,d);
                return ms.ToArray();
            }
        }
    }
    public class util {

        #region 組込関数設定
        public static void SetBuitIn(Type type,string name=null)
        {
            var catname = !string.IsNullOrEmpty(name) ? name : type.ToString();
            runtime.builtin.builtin_func.Subscribe(type,catname);
        }
        public static void SetCalcOp(Func<object,object,string,object> user_calc_op)
        {
            runtime.util.User_Calc_op = user_calc_op;
        }
        #endregion


        #region ログ設定
        public static void SetLogFunc(Action<string> write, Action<string> writeline, int debugLevel=1 )
        {
            sys.m_conWrite = write;
            sys.m_conWriteLine = writeline;
            sys.DEBUGLEVEL = debugLevel;
        }
        #endregion

        public static void Error(string msg)
        {
            slagtool.runtime.util._error(msg);
        }
        public static void SetDebugMode(int n) { sys.DEBUGLEVEL = n;    }
        public static int  GetDebugMode()      { return sys.DEBUGLEVEL; }
    }

    internal class util_sub
    {
        internal static List<YVALUE> Compile(List<string> multiple_srces)
        {
            var engine = new yengine();

            // 終末記号に分類
            var lex_output = new List<List<YVALUE>>();
            for(int i = 0; i<multiple_srces.Count; i++)
            { 
                lex_output.AddRange( engine.Lex(multiple_srces[i],i));
            }

            //スペース・コメント削除。"文字列"以外大文字化。
            engine.Normalize(ref lex_output);
            sys.logline("\n*lex_output");
            YDEF_DEBUG.DumpList(lex_output, true);

            //改行を無くしBOFとEOFを追加
            var one_line = engine.Del_LF_And_Add_BOF_EOF(lex_output);

            //実行用リスト作成(解析)
            var analyzed = engine.Interpret(one_line);       
            var executable_value_list = analyzed[0];
            

            //ダンプ
            sys.logline("\n[executable_value_list]\n");

            YDEF_DEBUG.PrintListValue(executable_value_list);

            sys.logline("\n");

            //リストの整合性テスト
            List<int> errorline;
            if (YDEF_DEBUG.IsExecutable(executable_value_list,out errorline))
            {
                sys.logline("This script is ready to excute.");
            }
            else
            {
                string s = null;
                errorline.ForEach(i=> {
                    if (s!=null) s+=",";
                    s += (i+1);
                });
                sys.error("This script is not executable. Check syntax at " + s);
            }
            return executable_value_list;
        }
        internal static List<YVALUE> Compile(string src)
        {
            var multiple_srces = new List<string>();
            multiple_srces.Add(src);

            return Compile(multiple_srces);
        }

        //internal static void Run(List<YVALUE> executable_value_list)
        //{
        //    runtime.builtin.builtin_func.Init();
        //    runtime.run_script.Run(executable_value_list[0]);
        //}
    }
}