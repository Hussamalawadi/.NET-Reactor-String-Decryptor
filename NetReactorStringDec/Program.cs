using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dnlib.DotNet.Emit;
using dnlib.DotNet;
using dnlib.DotNet.Writer;

namespace NetReactorStringDec
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.Title = ".NET Reactor String Decrypter";
                Console.WriteLine(".NET Reactor String Decrypter Coded by Robert\n");
                var filename = args[0];
                var module = ModuleDefMD.Load(filename); // load file
                Console.WriteLine("Module Loaded: " + filename + "\n");
                // execute unpackers
                Remover.Execute(module, filename);
                // save unpacked file
                var opts = new ModuleWriterOptions(module);
                opts.Logger = DummyLogger.NoThrowInstance;
                module.Write(filename.Replace(".exe", "_Cleaned.exe"), opts);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            Console.ReadLine();
        }

    }
}
