﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System;

#if NUMBERISFLOAT
using number = System.Single;
#else
using number = System.Double;
#endif

using slagtool.runtime;
using slagtool.runtime.builtin;

namespace slagtool
{
    public class slag
    {
        public   Guid          m_guid;
        public   string        m_id;

        public   string        m_filename;
            
        private  List<YVALUE>  m_exelist;
        private  StateBuffer   m_statebuf;

        public slag()
        {
            m_guid = System.Guid.NewGuid();
        }

        public string ID
        {
            get{ return m_id!=null ? m_id : m_guid.ToString(); }
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
            if (id==null) m_id = Path.GetFileNameWithoutExtension(filename);
            m_filename = filename;

            var ext = Path.GetExtension(filename);
            switch(ext.ToUpper())
            {
                case ".JS":
                case ".TXT":
                    {
                        var src = File.ReadAllText(filename);
                        m_exelist = util_sub.Compile(src);
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
        /// テキストソースロード
        /// id はデバッグ時の認識に利用
        /// </summary>
        public void LoadSrc(string src, string id=null)
        {
            m_id      =id;
            m_exelist = util_sub.Compile(src);
        }
        public void LoadBin(byte[] bin)
        {
            var d     = deserialize(bin);
            m_guid    = d.guid;
            m_id      = d.id;
            m_exelist = d.list;
        }
        public void LoadBase64(string base64str)
        {
            var bin    = Convert.FromBase64String(base64str);
            LoadBin(bin);
        }
        public void SaveBin(string filename)
        {
            var bytes = GetBin();
            File.WriteAllBytes(filename,bytes);
        }
        public byte[] GetBin()
        {
            return serialize(m_exelist,m_guid,m_id);
        }
        public string GetBase64()
        {
            var bytes = GetBin();
            return Convert.ToBase64String(bytes);
        }
        #endregion
        public void Run(bool bStepIn=false)
        {
            m_statebuf = new StateBuffer();
            runtime.builtin.builtin_func.Init();
            run_script.run(m_exelist[0],m_statebuf);
        }
        public void Step(bool bStepInOrOut=false)
        {

        }
        public void Pause(bool bStopOrResume=true)
        {

        }
        public void Terminate()
        {

        }
        public void Breakpoint(int linenum, bool bSetOrClear=true)
        {

        }
        #region 関数関連
        public bool ExistFunc(string funcname)
        {
            return builtin_func.IsFunc(funcname);
        }
        public object CallFunc(string funcname,object[] param=null)
        {
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
            public Guid   guid;
            public string id;
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
        private byte[] serialize(List<YVALUE> list, Guid guid, string id)
        {
            var d = new SaveFormat();
            d.guid = guid;
            d.id   = id;
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
        #endregion


        #region ログ設定
        public static void SetLogFunc(Action<string> write, Action<string> writeline, int debugLevel=1 )
        {
            sys.m_conWrite = write;
            sys.m_conWriteLine = writeline;
            sys.DEBUGLEVEL = debugLevel;
        }
        #endregion

        public static void SetDebugMode(int n) { sys.DEBUGLEVEL = n;    }
        public static int  GetDebugMode()      { return sys.DEBUGLEVEL; }
    }

    internal class util_sub
    {
        internal static List<YVALUE> Compile(string src)
        {
            var engine = new yengine();

            // 終末記号に分類
            var lex_output = engine.Lex(src);

            //スペース・コメント削除。"文字列"以外大文字化。
            engine.Normalize(ref lex_output);                             sys.logline("\n*lex_output");           YDEF_DEBUG.DumpList(lex_output, true);

            //１行化
            var one_line = engine.Make_one_line(lex_output);

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

        internal static void Run(List<YVALUE> executable_value_list)
        {
            runtime.builtin.builtin_func.Init();
            runtime.run_script.Run(executable_value_list[0]);
        }
    }
}