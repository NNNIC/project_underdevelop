using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace vt100art
{
    class Program
    {
        static void Main(string[] args)
        {
            //process.Run("SetVT();");
            //process.Run(args[0]);
            //builtin_func.RegisterFunctions(typeof(fnctions),"Applicaion");
            
            var asm = Assembly.LoadFrom(@"N:\Project\slag\slaglangtool\bin\Debug\slaglangtool.dll");

            foreach(var m in asm.GetModules())
            {
                Console.WriteLine(m.ToString());
            }

            var type = asm.GetType("slagtool.runtime.builtin.builtin_sysfunc");

            
                     
        }
    }
}
