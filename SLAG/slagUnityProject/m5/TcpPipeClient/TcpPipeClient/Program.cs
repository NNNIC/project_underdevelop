using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace TcpPipeClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var pipe = new TcpPipe("127.0.0.1",2002);
            pipe.Start(s=>Console.WriteLine(s));

            while(true)
            {
                string readmsg =null;
                while(true)
                {
                    var s = pipe.Read();
                    if (s==null) break;
                    readmsg += s + Environment.NewLine;
                }
                Console.WriteLine(readmsg);

                pipe.Update();
                Console.Write("Input>");
                var msg = Console.ReadLine();
                if (string.IsNullOrEmpty(msg)) continue;
                pipe.Write("127.0.0.1",2001,msg);

                Thread.Sleep(100);
            }
        }
    }
}
