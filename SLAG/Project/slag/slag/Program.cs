using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

/*
    ● 　実行概略

    Program.Main -> process.Run -> yengine.Lex        …  字句分解
                                   :
                                   yengine.Interpret　…  構文解析 and リスト作成
                                   :
                                   runtime.run_script …  実行
   
    ●　ファイル概要‘
    
    Program.cs         -- 本ファイル。実行開始箇所
    lextool/process.cs -- 処理本体
    lextool/sys.cs     -- ログ・エラーAPI

    lextool/y/syntax/lex.cs  -- 字句分解API
    lextool/y/syntax/ydef.cs -- 構文定義
    lextool/y/yanalyzer.cs   -- 構文解析
    lextool/y/ydef_api.cs    -- 構文解析用API
    lextool/y/ydef_debug.cd  -- デバッグ用
    lextool/y/ydef_do.cs     -- 構文ツリー作成API ydef.cs内で使用
    lextool/y/yengine.cs     -- 構文解析メイン
    
    lextool/runtime/embedded_func.cs -- 実行用組込関数
    lextool/runtime/run_sxript.cs    -- スクリプト実行

    ●　スクリプト文法概略

    JavaScript - likeな言語　多数の制限あり。

    主な制限

    ・大文字・小文字区別なし
    ・var宣言必須
    ・配列あり
    ・ループは"for文"と"while文"のみ。
    ・if/for/whileの実行ブロックは 波カッコのブロック定義必須
      例） if (a==1) print("yes");      --- エラー
           if (a==1) { print("yes"); }  --- ＯＫ
    ・未サポート演算子 : ３項演算子、インクリメント(++)、デクリメント(--)、自己代入(+=等) 
    ・エラー箇所断定は曖昧(「おそらくこのあたり」程度）)
*/
namespace slag
{
    class Program
    {
        static void Main(string[] args)
        {
            string NL = System.Environment.NewLine;
            string help = "jawascript [file [-c output]] | [-i] " + NL +
                          "file : filename          " + NL +
                          "       .js  --- source   " + NL +
                          "       .bin --- binary   " + NL +
                          "-c output : compile only"  + NL +
                          "            'output' is  output binary file." + NL +
                          "-i   : interactive mode"   + NL +
                          "";

            if (args.Length == 0)
            {
                Console.Write(help);
                Environment.Exit(1);
            }

            bool isInteractive = args[0].StartsWith("-i");
            if (!isInteractive)
            {
                var file = args[0];
                if (!File.Exists(file))
                {
                    Console.WriteLine("File not found : " + file);
                    Environment.Exit(1);
                }
                if (Path.GetExtension(file).ToLower()==".txt" || Path.GetExtension(file).ToLower()==".js")
                {
                    bool bCompile = false;
                    string binfile = null;
                    if (args.Length >= 3)
                    {
                        if (args[1]=="-c")
                        {
                            bCompile = true;
                            binfile = args[2];
                            if (string.IsNullOrWhiteSpace(Path.GetExtension(binfile)))
                            {
                                binfile += ".bin";
                            }
                        }
                    }

                    var src = File.ReadAllText(args[0]);

                    try { 
                        if (bCompile)
                        {
                                slagtool.process.Run(src,true,binfile);
                        }
                        else
                        { 
                            slagtool.process.Run(src);
                        }
                    }
                    catch ( SystemException e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    return;
                }
                else if (Path.GetExtension(file).ToLower()==".bin")
                {
                    try { 
                        slagtool.process.Run_from_savefile(file);
                    }
                    catch( SystemException e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    return;
                }
                else
                {
                    Console.WriteLine("No action");
                }
            }
            else {    // Interactive mode
                interactive_mode.interactive();
            }
        }

    }
}
