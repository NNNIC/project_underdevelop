using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Threading;

namespace winpipe_sample
{
    //ref http://tomosoft.jp/design/?p=4607
    class winpipe
    {
        [DllImport("winpipe.dll",CallingConvention = CallingConvention.Cdecl,EntryPoint = "create")]    /*public*/ static extern void create(IntPtr readpipename,IntPtr writepipename);
        [DllImport("winpipe.dll",CallingConvention = CallingConvention.Cdecl,EntryPoint = "destroy")]   /*public*/ static extern void destroy();
        [DllImport("winpipe.dll",CallingConvention = CallingConvention.Cdecl,EntryPoint = "read")]      /*public*/ static extern int read();
        [DllImport("winpipe.dll",CallingConvention = CallingConvention.Cdecl,EntryPoint = "write")]     /*public*/ static extern void write(IntPtr msg);
        [DllImport("winpipe.dll",CallingConvention = CallingConvention.Cdecl,EntryPoint = "get_buf")]   /*public*/ static extern IntPtr get_buf();

        //tool
        public static IntPtr ConvertToPtr(string s)
        {
            return (IntPtr)Marshal.StringToHGlobalAnsi(s);
        }
        public static string ConvertToString(IntPtr p)
        {
            return (string)Marshal.PtrToStringAnsi(p);
        }
        public static void Create(string readpipename, string writepipename)
        {
            Console.WriteLine("READ :" + readpipename);
            Console.WriteLine("WRITE:" + writepipename);

            var ptr_readpipename = ConvertToPtr(readpipename);
            var ptr_writepipename= ConvertToPtr(writepipename);

            create(ptr_readpipename,ptr_writepipename);

            Marshal.FreeHGlobal(ptr_readpipename);
            Marshal.FreeHGlobal(ptr_writepipename);
        }
        public static void Destroy()
        {
            destroy();
        }
        public static string Read()
        {
            var size = read();
            if (size==0) return null;

            var ptr_buf = get_buf();

            return ConvertToString(ptr_buf);
        }
        public static void Write(string msg)
        {
            var ptr_msg = ConvertToPtr(msg);
            write(ptr_msg);
            Marshal.FreeHGlobal(ptr_msg);
        }
    }

    class Program
    {
        const string PIPENAME_PASS1_A =	"\\\\.\\pipe\\testpipe_1A";
        const string PIPENAME_PASS1_B =	"\\\\.\\pipe\\testpipe_1B";
        //const string PIPENAME_PASS2_A =	"\\\\.\\pipe\\testpipe_2A";
        //const string PIPENAME_PASS2_B =	"\\\\.\\pipe\\testpipe_2B";
        //const string PIPENAME_PASS3_A =	"\\\\.\\pipe\\testpipe_3A";
        //const string PIPENAME_PASS3_B =	"\\\\.\\pipe\\testpipe_3B";

        static void Main(string[] args)
        { 
            if (args!=null && args.Length>0) //引数がある場合 -- プロセス１
            {
                winpipe.Create(PIPENAME_PASS1_A,PIPENAME_PASS1_B);
            }
            else //引数がない場合　--- プロセス２
            {
                winpipe.Create(PIPENAME_PASS1_B,PIPENAME_PASS1_A);
            }

            Console.WriteLine("※ 'I'キー押下でメッセージ入力");

            while (true)
	        {
                Thread.Sleep(100);

		        while (true)
		        {
			        string s = winpipe.Read(); // * s = g_pipe->read_msg();
			        if (s != null)
			        {
				        Console.WriteLine(s); //    printf(s->c_str());
			        }
			        else
			        {
				        break;
			        }
		        }

                if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.I)
                {
                    Console.Write(">");
                    var l = Console.ReadLine();
                    winpipe.Write(l);
                }
	        }
        }
    }
}
