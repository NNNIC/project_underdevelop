using UnityEngine;
using System.Collections;
using System;

namespace slagtool
{
    public class util {

        #region テキストソース
        /// <summary>
        /// テキストソース実行
        /// </summary>
        public static void ExeSrc(string src)
        {
            process.Run(src);
        }
        /// <summary>
        /// テキストソース格納
        /// </summary>
        public static void LoadSrc(string src)
        {
            process.Run(src,true);
        }
        #endregion

        #region バイナリソース
        /// <summary>
        /// バイナリソース実行
        /// </summary>
        public static void ExeBin(byte[] bin)
        {
            YSAVELOAD.Load(bin);
            process.Run_from_savefile();
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
            process.Run_from_savefile();
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

        #region 汎用実行
        /// <summary>
        /// 格納ソース実行
        /// </summary>
        public static void Run()
        {
            process.Run_from_savefile();
        }
        #endregion

        #region ログ設定
        public static void SetLogFunc(Action<string> write, Action<string> writeline )
        {
            sys.m_conWrite = write;
            sys.m_conWriteLine = writeline;
            if (!sys.DEBUGMODE) sys.DEBUGMODE = true;
        }
        #endregion
    }
}