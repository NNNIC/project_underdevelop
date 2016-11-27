using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

#if NUMBERISFLOAT
using number = System.Single;
#else
using number = System.Double;
#endif

namespace slagtool
{
    public class util {

        #region テキストソース
        /// <summary>
        /// テキストソース実行
        /// </summary>
        public static void ExeSrc(string src)
        {
            util_sub.Run(src);
        }
        /// <summary>
        /// テキストソース格納
        /// </summary>
        public static void LoadSrc(string src)
        {
            util_sub.Run(src,true);
        }
        #endregion

        #region バイナリソース
        /// <summary>
        /// バイナリソース実行
        /// </summary>
        public static void ExeBin(byte[] bin)
        {
            YSAVELOAD.Load(bin);
            util_sub.Run_from_savefile();
        }
        /// <summary>
        /// バイナリソース格納
        /// </summary>
        public static void LoadBin(byte[] bin)
        {
            YSAVELOAD.Load(bin);
        }
        /// <summary>
        /// 格納ソースのバイナリ取得
        /// </summary>
        public static byte[] GetBin()
        {
            return YSAVELOAD.GetBin();
        }
        #endregion

        #region BASE64バイナリソース
        /// <summary>
        /// BASE64バイナリソースを実行
        /// </summary>
        public static void ExeBase64(string base64str)
        {
            var bin = Convert.FromBase64String(base64str);
            YSAVELOAD.Load(bin);
            util_sub.Run_from_savefile();
        }
        /// <summary>
        /// BASE64バイナリソースを格納
        /// </summary>
        public static void LoadBase64(string base64str)
        {
            var bin = Convert.FromBase64String(base64str);
            YSAVELOAD.Load(bin);
        }
        public static string GetBase64()
        {
            var bytes = YSAVELOAD.GetBin();
            if (bytes!=null)
            {
                return  Convert.ToBase64String(bytes);
            }
            return null;
        }
        #endregion

        #region 実行
        /// <summary>
        /// 格納ソース実行
        /// </summary>
        public static void Run()
        {
            util_sub.Run_from_savefile();
        }
        public static runtime.process CreateProcess()
        {
            return util_sub.CreateProcess();
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
        internal static void Run(string src, bool bCompileOnly=false,string outbinfile = null)
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

            //SAVE
            YSAVELOAD.Save(executable_value_list,slagtool.runtime.CFG.TMPBIN);
            if (outbinfile!=null)
            {
                YSAVELOAD.Save(executable_value_list,outbinfile);
            }

            if (!bCompileOnly)
            { 
                //LOAD
                executable_value_list = YSAVELOAD.Load(slagtool.runtime.CFG.TMPBIN);

                //実行
                sys.logline("\n\n*Execute! \n");

                runtime.builtin.builtin_func.Init();
                runtime.run_script.Run(executable_value_list[0]);
            }
            sys.logline("\n*end");
        }

        internal static void Run_from_savefile(string file = null)
        {
            if (file==null)
            {
                file = slagtool.runtime.CFG.TMPBIN;
            }

            //LOAD
            var executable_value_list = YSAVELOAD.Load(file);

            //実行
            sys.logline("\n\n*Execute! \n");

            runtime.builtin.builtin_func.Init();
            runtime.run_script.Run(executable_value_list[0]);

            sys.logline("\n*end");
        }

        internal static runtime.process CreateProcess(string file = null)
        {
            if (file == null)
            {
                file = slagtool.runtime.CFG.TMPBIN;
            }

            //LOAD
            var executable_value_list = YSAVELOAD.Load(file);


            var proc = new runtime.process();
            proc.Init(executable_value_list);

            return proc;
        }
    }
}