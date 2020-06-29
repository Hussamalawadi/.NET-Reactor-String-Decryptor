using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using System.Reflection;

namespace NetReactorStringDec
{
    public class Remover
    {
        public static void Execute(ModuleDefMD module, string filename)
        {
            var second_module = System.Reflection.Assembly.LoadFrom(filename);
            int decypted = 0;
            foreach (var type_ in module.GetTypes())
            {
                foreach (var method in type_.Methods)
                {
                    try
                    {
                        for (int i = 0; i < method.Body.Instructions.Count; i++)
                        {

                            try
                            {
                                MethodInfo methodx = null;
                                object returnedstring = null;
                                if (method.Body.Instructions[i - 1].IsLdcI4() && method.Body.Instructions[i].OpCode == OpCodes.Call)
                                {
                                    {
                                        try
                                        {
                                            if (method.Body.Instructions[i].Operand.GetType().ToString().Contains("MethodDef"))
                                            {
                                                methodx = GetStringMethod((int)((MethodDef)method.Body.Instructions[i].Operand).MDToken.Raw, second_module);
                                            }
                                            else
                                            {
                                                methodx = GetStringMethod((int)((MethodSpec)method.Body.Instructions[i].Operand).Method.MDToken.Raw, second_module);
                                            }
                                            int intergerevalue1 = (int)method.Body.Instructions[i - 0x1].GetLdcI4Value();
                                            returnedstring = methodx.Invoke(null, new object[] { intergerevalue1 });
                                        }
                                        catch (Exception EX)
                                        {

                                        }

                                    }

                                }
                                if (returnedstring != null)
                                {
                                    method.Body.Instructions[i - 1].OpCode = OpCodes.Nop;
                                    method.Body.Instructions[i].OpCode = OpCodes.Ldstr;
                                    method.Body.Instructions[i].Operand = (string)returnedstring;
                                    decypted++;
                                }
                            }

                            catch
                            {
                            }

                        }
                    }
                    catch
                    {
                    }

                }

            }
            Console.WriteLine("Decrypted Strings: " + decypted.ToString());

        }
        private static MethodInfo GetStringMethod(int MethodToken, Assembly asm)
        {
            MethodInfo info = (MethodInfo)asm.ManifestModule.ResolveMethod(MethodToken);
            if (info.IsGenericMethodDefinition || info.IsGenericMethod)
            {
                return info.MakeGenericMethod(new Type[] { typeof(string) });
            }
            return info;
        }
    }

}
