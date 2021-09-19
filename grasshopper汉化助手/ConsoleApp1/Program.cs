using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using dnlib.DotNet.Writer;
using System.Xml;
namespace ConsoleApp1
{
    class Program
    {

        
        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        static void Main(string[] args)
        {
            System.IO.Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
           







            

            if (args.Length == 1)
                Do3(args[0]);
            else if (args.Length == 3)
            {
             
              
               

             }
            else {

                string[] lines = File.ReadAllLines("config.ini");
                string appid = lines[0].Split('=')[1];
                string appkey = lines[1].Split('=')[1];
                XmlDocument tDoc = new XmlDocument();
                XmlElement root;


                string Text = File.ReadAllText(("data.xml"));
                tDoc.LoadXml(Text);
                root = tDoc.DocumentElement;
                for (int i = 0; i < root.ChildNodes.Count; i++)
                {
                  
                        for (int ii = 0; ii < root.ChildNodes[i].ChildNodes.Count; ii++)
                    {
                        if (ii >=0)
                        {
                            string ttt = root.ChildNodes[i].ChildNodes[ii].ChildNodes[0].InnerText;
                            string ggg = "";
                            try
                            {
                                ggg = Class1.Main(ttt, appid, appkey);
                            }
                            catch
                            {
                            }
                            Thread.Sleep(1500);
                            root.ChildNodes[i].ChildNodes[ii].ChildNodes[1].InnerText = ggg;
                            Console.WriteLine(root.ChildNodes[i].ChildNodes.Count.ToString());
                            Console.WriteLine(ttt);
                            Console.WriteLine(ggg);
                        }

                    }


                }

                tDoc.Save("data2.xml");


            }
           
           ;
        }
        public void Do()
          { ModuleDef md = ModuleDefMD.Load("dump.exe");
        string[] attri = {
"AssemblyTitleAttribute" ,"AssemblyCompanyAttribute"};

            foreach (CustomAttribute attribute in md.Assembly.CustomAttributes)
            {


                if (attri.Any(attribute.AttributeType.Name.Contains))
                {

                    string encAttri = RandomString(10);
        attribute.ConstructorArguments[0] = new CAArgument(md.CorLibTypes.String, new UTF8String(encAttri));
                    Console.WriteLine(attribute.ConstructorArguments[0]);
                }

}

foreach (var type in md.GetTypes())
{
    type.Name = RandomString(10);
    type.Namespace = RandomString(10);
    foreach (MethodDef method in type.Methods)
    {
        if (!method.HasBody) continue;
        if (method.IsConstructor) continue;

        string encName = RandomString(10);
        method.Name = encName;
    }
}

foreach (var type in md.GetTypes())
{
    // methods in type
    foreach (MethodDef method in type.Methods)
    {
        // empty method check
        if (!method.HasBody) continue;
        // iterate over instructions of method

        for (int i = 0; i < method.Body.Instructions.Count(); i++)
        {

            if (method.Body.Instructions[i].OpCode == OpCodes.Ldstr)
            {
                Console.WriteLine(method.Body.Instructions[i].OpCode);
                // c# variable has for loop scope only
                String regString = method.Body.Instructions[i].Operand.ToString();
                String encString = Convert.ToBase64String(UTF8Encoding.UTF8.GetBytes(regString));
                Console.WriteLine($"{regString} -> {encString}");
                // methodology for adding code: write it in plain c#, compile, then view IL in dnspy
                method.Body.Instructions[i].OpCode = OpCodes.Nop; // errors occur if instruction not replaced with Nop
                method.Body.Instructions.Insert(i + 1, new Instruction(OpCodes.Call, md.Import(typeof(System.Text.Encoding).GetMethod("get_UTF8", new Type[] { })))); // Load string onto stack
                method.Body.Instructions.Insert(i + 2, new Instruction(OpCodes.Ldstr, encString)); // Load string onto stack
                method.Body.Instructions.Insert(i + 3, new Instruction(OpCodes.Call, md.Import(typeof(System.Convert).GetMethod("FromBase64String", new Type[] { typeof(string) })))); // call method FromBase64String with string parameter loaded from stack, returned value will be loaded onto stack
                method.Body.Instructions.Insert(i + 4, new Instruction(OpCodes.Callvirt, md.Import(typeof(System.Text.Encoding).GetMethod("GetString", new Type[] { typeof(byte[]) })))); // call method GetString with bytes parameter loaded from stack 
                i += 4; //skip the Instructions as to not recurse on them
            }
        }
    }


}

foreach (var type in md.GetTypes())
{
    foreach (MethodDef method in type.Methods)
    {
        // empty method check
        if (!method.HasBody) continue;

        method.Body.SimplifyBranches();
        method.Body.OptimizeBranches(); // negates simplifyBranches
                                        //method.Body.OptimizeMacros();
    }
}


md.Write("3.exe");





Console.Write(md);
Console.ReadKey();
        }


        public static void Do2()
        {
            ModuleDef md = ModuleDefMD.Load("3.exe");
            string[] attri = {
"AssemblyTitleAttribute" ,"AssemblyCompanyAttribute"};

            foreach (CustomAttribute attribute in md.Assembly.CustomAttributes)
            {


                if (attri.Any(attribute.AttributeType.Name.Contains))
                {

                    string encAttri = RandomString(10);
                    attribute.ConstructorArguments[0] = new CAArgument(md.CorLibTypes.String, new UTF8String(encAttri));
                    Console.WriteLine(attribute.ConstructorArguments[0]);
                }

            }

            int aa = 0;
            int bb = 0;
            foreach (var type in md.GetTypes())
            {
                type.Name = "class"+aa.ToString();
                type.Namespace = "space" + aa.ToString();
                aa++;
                foreach (MethodDef method in type.Methods)
                {
                    if (!method.HasBody) continue;
                    if (method.IsConstructor) continue;

                    string encName = RandomString(10);
                    method.Name = "method"+ bb.ToString();
                    bb++;
                }
            }

            foreach (var type in md.GetTypes())
            {
                // methods in type
                foreach (MethodDef method in type.Methods)
                {
                    // empty method check
                    if (!method.HasBody) continue;
                    // iterate over instructions of method

                    for (int i = 0; i < method.Body.Instructions.Count(); i++)
                    {

                        if (method.Body.Instructions[i].OpCode == OpCodes.Ldstr)
                        {
                            Console.WriteLine(method.Body.Instructions[i].OpCode);
                            // c# variable has for loop scope only
                            /*
                            String regString = method.Body.Instructions[i].Operand.ToString();
                            String encString = Convert.ToBase64String(UTF8Encoding.UTF8.GetBytes(regString));
                            Console.WriteLine($"{regString} -> {encString}");
                            // methodology for adding code: write it in plain c#, compile, then view IL in dnspy
                            method.Body.Instructions[i].OpCode = OpCodes.Nop; // errors occur if instruction not replaced with Nop
                            method.Body.Instructions.Insert(i + 1, new Instruction(OpCodes.Call, md.Import(typeof(System.Text.Encoding).GetMethod("get_UTF8", new Type[] { })))); // Load string onto stack
                            method.Body.Instructions.Insert(i + 2, new Instruction(OpCodes.Ldstr, encString)); // Load string onto stack
                            method.Body.Instructions.Insert(i + 3, new Instruction(OpCodes.Call, md.Import(typeof(System.Convert).GetMethod("FromBase64String", new Type[] { typeof(string) })))); // call method FromBase64String with string parameter loaded from stack, returned value will be loaded onto stack
                            method.Body.Instructions.Insert(i + 4, new Instruction(OpCodes.Callvirt, md.Import(typeof(System.Text.Encoding).GetMethod("GetString", new Type[] { typeof(byte[]) })))); // call method GetString with bytes parameter loaded from stack 
                            i += 4; //skip the Instructions as to not recurse on them
                            */
                            String regString = method.Body.Instructions[i].Operand.ToString();

                            byte[] encString = Convert.FromBase64String(regString);
                            string  hh= Encoding.UTF8.GetString(encString);
                            method.Body.Instructions[i-2].OpCode = OpCodes.Ldstr;
                            method.Body.Instructions[i - 2].Operand =hh;
                           method.Body.Instructions.RemoveAt(i+2);
                             method.Body.Instructions.RemoveAt(i+1);
                             method.Body.Instructions.RemoveAt(i);
                             method.Body.Instructions.RemoveAt(i-1);
                          
                           // method.Body.Instructions[i-1].OpCode = OpCodes.Nop;
                           // method.Body.Instructions[i].OpCode = OpCodes.Nop;
                           // method.Body.Instructions[i+1].OpCode = OpCodes.Nop;
                          //  method.Body.Instructions[i+2].OpCode = OpCodes.Nop;

                              i -= 2;

                        }
                    }
                }


            }

            foreach (var type in md.GetTypes())
            {
                foreach (MethodDef method in type.Methods)
                {
                    // empty method check
                    if (!method.HasBody) continue;

                    method.Body.SimplifyBranches();
                    method.Body.OptimizeBranches(); // negates simplifyBranches
                                                    //method.Body.OptimizeMacros();
                }
            }


            md.Write("4.exe");





            Console.Write(md);
            Console.ReadKey();
        }


        public static void Do22()
        {
            ModuleDef md = ModuleDefMD.Load("RhinoActivator.exe");
            string[] attri = {
"AssemblyTitleAttribute" ,"AssemblyCompanyAttribute"};

            foreach (CustomAttribute attribute in md.Assembly.CustomAttributes)
            {


                if (attri.Any(attribute.AttributeType.Name.Contains))
                {

                    string encAttri = RandomString(10);
                    attribute.ConstructorArguments[0] = new CAArgument(md.CorLibTypes.String, new UTF8String(encAttri));
                    Console.WriteLine(attribute.ConstructorArguments[0]);
                }

            }

            int aa = 0;
            int bb = 0;
            foreach (var type in md.GetTypes())
            {
                if(type.Namespace== "McNeel.License"||type.Namespace == "ZooCommon")continue;
                type.Name = "class" + aa.ToString();
                type.Namespace = "space" + aa.ToString();
                aa++;
                foreach (MethodDef method in type.Methods)
                {
                    if (!method.HasBody) continue;
                    if (method.IsConstructor) continue;

                    string encName = RandomString(10);
                    method.Name = "method" + bb.ToString();
                    bb++;
                }
            }

 

            foreach (var type in md.GetTypes())
            {
                foreach (MethodDef method in type.Methods)
                {
                    // empty method check
                    if (!method.HasBody) continue;

                    method.Body.SimplifyBranches();
                    method.Body.OptimizeBranches(); // negates simplifyBranches
                                                    //method.Body.OptimizeMacros();
                }
            }


            md.Write("RhinoActivator2-cleaned.exe");





            Console.Write(md);
            Console.ReadKey();
        }
        public static void Do3(string xx)
        {
            string jiedian = Path.GetFileNameWithoutExtension(xx);
            string houzi= Path.GetFileName(xx);
            houzi = houzi.Replace(jiedian, "");
            XmlDocument tDoc = new XmlDocument();
            XmlElement root;

            // 一些声明信息
            if (File.Exists("data.xml"))
            {
               string Text = File.ReadAllText(("data.xml"));
                tDoc.LoadXml(Text);
                root = tDoc.DocumentElement;
                for (int i = 0; i < root.ChildNodes.Count; i++)
                { if (jiedian == root.ChildNodes[i].Name) return; }
            }
            else
            {
                XmlDeclaration xmlDecl = tDoc.CreateXmlDeclaration("1.0", "utf-8", null);
                tDoc.AppendChild(xmlDecl);

                // 新建根节点
                root = tDoc.CreateElement("root");
                tDoc.AppendChild(root);
            }
            XmlElement toTalTick2 = tDoc.CreateElement(jiedian);
           
            root.AppendChild(toTalTick2);

            string gg = "";
            ModuleDef md = ModuleDefMD.Load(xx);
            
            string[] attri = {
"AssemblyTitleAttribute" ,"AssemblyCompanyAttribute"};

            foreach (CustomAttribute attribute in md.Assembly.CustomAttributes)
            {


                if (attri.Any(attribute.AttributeType.Name.Contains))
                {

                  //  string encAttri = RandomString(10);
                 //   attribute.ConstructorArguments[0] = new CAArgument(md.CorLibTypes.String, new UTF8String(encAttri));
                  //  Console.WriteLine(attribute.ConstructorArguments[0]);
                }

            }

            int aa = 0;
            int bb = 0;
            int lala = 0;




            var bclass = new TypeDefUser("Strings", md.CorLibTypes.Object.TypeDefOrRef);
          
          
            // Add it to the module
            md.Types.Add(bclass);
            // Create Ctor.Test.BaseClass constructor: BaseClass()
            var bctor = new MethodDefUser(".ctor", MethodSig.CreateInstance(md.CorLibTypes.Void),
                            MethodImplAttributes.IL | MethodImplAttributes.Managed,
                            MethodAttributes.Public |
                             MethodAttributes.Static |
                            MethodAttributes.SpecialName | MethodAttributes.RTSpecialName);
            // Add the method to BaseClass
            bclass.Methods.Add(bctor);
            //bclass.Methods.Add(md.GetTypes().ToList()[0].Methods[0]);
            var cfile = new FieldDefUser("MustUseCache", new FieldSig(md.CorLibTypes.String),FieldAttributes.Static | FieldAttributes.Private | FieldAttributes.InitOnly);
            bclass.Fields.Add(cfile);
            ModuleDefMD mo = ModuleDefMD.Load("mscorlib.dll");
            
            foreach(var ij in mo.GetTypes())
            {
                if (ij.Name.Contains("Dictionary"))
                    Console.WriteLine(ij.Name);
            }
           
            TypeRefUser sss = new TypeRefUser(mo, "System.Collections.Generic", "Dictionary<System.Int32,System.String>", mo.CorLibTypes.AssemblyRef);
            TypeSig typs=sss.ToTypeSig();
            var cfile2 = new FieldDefUser("hashtableLock", new FieldSig(typs), FieldAttributes.Static | FieldAttributes.Private );

          //  bclass.Fields.Add(cfile2);
            mo = (ModuleDefMD)md;
            var cfile3 = new FieldDefUser("cacheStrings", new FieldSig(md.CorLibTypes.Boolean), FieldAttributes.Static | FieldAttributes.Private | FieldAttributes.InitOnly);
            bclass.Fields.Add(cfile3);
           
            var cfile4 = new FieldDefUser("jiedian", new FieldSig(md.CorLibTypes.String), FieldAttributes.Static | FieldAttributes.Private | FieldAttributes.InitOnly);
            bclass.Fields.Add(cfile4);
            TypeRefUser ssss = new TypeRefUser(md, "System.Xml", "XmlDocument", mo.CorLibTypes.AssemblyRef);
            TypeSig typss = ssss.ToTypeSig();

            TypeRefUser ssss2 = new TypeRefUser(md, "System.Xml", "XmlNode", mo.CorLibTypes.AssemblyRef);
            TypeSig typss2 = ssss2.ToTypeSig();

           
            CilBody body;
            bctor.Body = body = new CilBody();

            // GenericInstSig s = new GenericInstSig(ss);
           // var systemConsole = md.CorLibTypes.GetTypeRef("System.IO", "ReadAllText");
            Importer importer = new Importer(md);
            
            ITypeDefOrRef consoleRef = importer.Import(typeof(File));
            var writeLine2 = new MemberRefUser(md, "ReadAllText",
                            MethodSig.CreateStatic(md.CorLibTypes.String, md.CorLibTypes.String
                                           ),
                             consoleRef);
            consoleRef = importer.Import(typeof(String));
            var writeLine3 = new MemberRefUser(md, "op_Equality",
                            MethodSig.CreateStatic(md.CorLibTypes.Boolean, md.CorLibTypes.String, md.CorLibTypes.String
                                           ),
                             consoleRef);

            consoleRef = importer.Import(typeof(System.Xml.XmlDocument)); 
            var writeLine4 = new MemberRefUser(md, ".ctor",
                            MethodSig.CreateStatic(md.CorLibTypes.Void
                                           ),
                             consoleRef);
            consoleRef = importer.Import(typeof(System.Xml.XmlDocument));
            var writeLine5 = new MemberRefUser(md, "Load",
                            MethodSig.CreateStatic(md.CorLibTypes.Void, md.CorLibTypes.String
                                           ),
                             consoleRef);

            var objectCtor = new MemberRefUser(md, ".ctor",
                             MethodSig.CreateInstance(md.CorLibTypes.Void),
                             md.CorLibTypes.Object.TypeDefOrRef);

            var writeLine6 = new MemberRefUser(md, "get_DocumentElement",
                           MethodSig.CreateStatic(md.CorLibTypes.Void, md.CorLibTypes.String
                                          ),
                            consoleRef);

         /*
        body.InitLocals = true;
            body.Variables.Add(new dnlib.DotNet.Emit.Local(md.CorLibTypes.String,"k",0));
            body.Variables.Add(new dnlib.DotNet.Emit.Local(typss, "tDoc", 1));
            body.Variables.Add(new dnlib.DotNet.Emit.Local(typss2, "root",2));
            body.Variables.Add(new dnlib.DotNet.Emit.Local(typss2, "e", 3));
            body.Variables.Add(new dnlib.DotNet.Emit.Local(md.CorLibTypes.Boolean));
            body.Variables.Add(new dnlib.DotNet.Emit.Local(md.CorLibTypes.Int32,"j"));
            body.Variables.Add(new dnlib.DotNet.Emit.Local(md.CorLibTypes.Boolean));
            body.Variables[0].Name = "k";

            body.Instructions.Add(OpCodes.Ldstr.ToInstruction("1"));
            body.Instructions.Add(OpCodes.Stsfld.ToInstruction(cfile));
            //body.Instructions.Add(Instruction.Create(  );
            /// body.Instructions.Add(OpCodes.Stsfld.ToInstruction(cfile2));
            //body.Instructions.Add(OpCodes.Ldc_I4.ToInstruction(0));
            body.Instructions.Add(OpCodes.Newobj.ToInstruction(objectCtor));
            body.Instructions.Add(OpCodes.Stsfld.ToInstruction(cfile3));
            body.Instructions.Add(OpCodes.Ldstr.ToInstruction("grasshopper"));
            body.Instructions.Add(OpCodes.Stsfld.ToInstruction(cfile4));
            body.Instructions.Add(OpCodes.Ldstr.ToInstruction("c.ini"));
            body.Instructions.Add(OpCodes.Call.ToInstruction(writeLine2));
           // body.Instructions.Add(OpCodes.Pop.ToInstruction());
            body.Instructions.Add(OpCodes.Stloc_0.ToInstruction()); 
               // body.Instructions.Add(OpCodes.Ldc_I4.ToInstruction(0));
            body.Instructions.Add(OpCodes.Ldsfld.ToInstruction(cfile));
            body.Instructions.Add(OpCodes.Ldstr.ToInstruction("1"));
            body.Instructions.Add(OpCodes.Call.ToInstruction(writeLine3));
            body.Instructions.Add(OpCodes.Stloc_S.ToInstruction(body.Variables[4]));
            body.Instructions.Add(OpCodes.Ldloc.ToInstruction(body.Variables[4]));

            */

            ModuleDefMD mo0 = ModuleDefMD.Load("ConsoleApp2.exe");
            foreach (var type2 in mo0.GetTypes())
            {
                Console.WriteLine(type2.Name);

                if (type2.Name == "Strings")
                {
                    // methods in type
                    foreach (MethodDef method in type2.Methods)
                    {


                        for (int i = 0; i < method.Body.Instructions.Count(); i++)
                        {
                            if (method.Body.Instructions[i].Operand != null && method.Body.Instructions[i].OpCode != null)
                            {
                                if (method.Body.Instructions[i].Operand.ToString() == "grasshopper")

                                    method.Body.Instructions[i].Operand = jiedian;
                                //  Console.WriteLine(method.Body.Instructions[i].Operand);
                                // Console.WriteLine(method.Body.Instructions[i].Offset);
                            }

                        }

                    }
                }
            }



                body.Instructions.Add(OpCodes.Ldc_I4.ToInstruction(1));
            body.Instructions.Add(OpCodes.Stsfld.ToInstruction(cfile3));

            /*
           // body.Instructions.Add(OpCodes.Newobj.ToInstruction(objectCtor));
         //   body.Instructions.Add(OpCodes.Stsfld.ToInstruction(cfile2));

















             body.Instructions.Add(OpCodes.Newobj.ToInstruction(writeLine4));

            body.Instructions.Insert(14, OpCodes.Brfalse_S.ToInstruction(body.Instructions[18]));
          //  body.Instructions.Add(OpCodes.Stloc_1.ToInstruction());
          //  body.Instructions.Insert(13, OpCodes.Brfalse_S.ToInstruction(body.Instructions[15]));
          //  body.Instructions.Add(OpCodes.Ldloc_1.ToInstruction());
          //  body.Instructions.Add(OpCodes.Ldstr.ToInstruction("data.xml"));
          //  body.Instructions.Add(OpCodes.Callvirt.ToInstruction(writeLine5));
          //  body.Instructions.Add(OpCodes.Ldloc_1.ToInstruction());

            // Make sure we call the bas.e class' constructor
            // body.Instructions.Add(OpCodes.Ldloc_1.ToInstruction());
            // body.Instructions.Add(OpCodes.Stloc_2.ToInstruction());
            //  body.Instructions.Add(OpCodes.Ret.ToInstruction());
          //  body.Instructions.Add(OpCodes.Pop.ToInstruction());
            body.Instructions.Add(OpCodes.Ret.ToInstruction());
            */
            /* (59,5)-(59,55) E:\混淆\ConsoleApp2\ConsoleApp2\Class1.cs */
            /* 0x000002D4 7203000070   */
            //IL_0000: ldstr     "1"
            /* 0x000002D9 8001000004   */
            //  IL_0005: stsfld    string Strings::MustUseCache
            /* (66,5)-(66,65) E:\混淆\ConsoleApp2\ConsoleApp2\Class1.cs */
            /* 0x000002DE 731200000A   */ // IL_000A: newobj instance void [mscorlib]System.Object::.ctor()
            /* 0x000002E3 8003000004   */
            //  IL_000F: stsfld    object Strings::hashtableLock
            /* (69,5)-(69,55) E:\混淆\ConsoleApp2\ConsoleApp2\Class1.cs */
            /* 0x000002E8 16           */// IL_0014: ldc.i4.0
            /* 0x000002E9 8004000004   */
            // IL_0015: stsfld    bool Strings::cacheStrings
            /* (72,5)-(72,50) E:\混淆\ConsoleApp2\ConsoleApp2\Class1.cs */
            /* 0x000002EE 7207000070   */ // IL_001A: ldstr     "grasshopper"
            /* 0x000002F3 8005000004   */
            // IL_001F: stsfld    string Strings::jiedian
            /* (32,5)-(32,6) E:\混淆\ConsoleApp2\ConsoleApp2\Class1.cs */
            /* 0x000002F8 00           */ //IL_0024: nop
            /* (33,9)-(33,46) E:\混淆\ConsoleApp2\ConsoleApp2\Class1.cs */
            /* 0x000002F9 721F000070   */ //IL_0025: ldstr     "c.ini"
            /* 0x000002FE 281300000A   */
            //     IL_002A: call      string[mscorlib] System.IO.File::ReadAllText(string)
            /* 0x00000303 0A           */
            //    IL_002F: stloc.0
            /* (34,9)-(34,41) E:\混淆\ConsoleApp2\ConsoleApp2\Class1.cs */
            /* 0x00000304 7E01000004   */
            //        IL_0030: ldsfld    string Strings::MustUseCache
            /* 0x00000309 7203000070   */// IL_0035: ldstr     "1"
            /* 0x0000030E 281400000A   */
            //       IL_003A: call      bool[mscorlib] System.String::op_Equality(string, string)
            /* 0x00000313 1304         */
            //      IL_003F: stloc.s V_4
            /* (hidden)-(hidden) E:\混淆\ConsoleApp2\ConsoleApp2\Class1.cs */
            /* 0x00000315 1104         */
            //       IL_0041: ldloc.s V_4
            /* 0x00000317 2C12         */
            //             IL_0043: brfalse.s IL_0057






            foreach (var type in md.GetTypes())
            {
                foreach (MethodDef method in type.Methods)
                {
                    // empty method check
                    if (!method.HasBody) continue;

                    method.Body.SimplifyBranches();
                    method.Body.OptimizeBranches(); // negates simplifyBranches
                    method.Body.OptimizeMacros();                             //method.Body.OptimizeMacros();
                  
                }
            }

            MethodDef me4 = bctor;
            foreach (var type2 in mo0.GetTypes())
            {
               

                if (type2.Name == "Strings")
                {

                    DupType(type2,bclass);
                    ReReference(bclass,md);




                }
            }

            foreach (var type2 in md.GetTypes())
            {


                if (type2.Name == "Strings")
                {

                    foreach (MethodDef me3 in type2.Methods)
                    {
                        if (me3.Name == "Get")
                        {
                            me4 = me3;
                        }

                    }



                }
            }





            foreach (var type in md.GetTypes())
            {


                if (type.Name == "Instances") continue;

                //    type.Name = "class" + aa.ToString();
                // type.Namespace = "space" + aa.ToString();
                aa++;
                foreach (MethodDef method in type.Methods)
                {
                    if (!method.HasBody) continue;
                    if (method.IsConstructor) continue;
                    if ((method.FullName).Contains("HtmlHelp_Source") || (method.FullName).Contains("RegisterInputParams"))
                    {
                        int ii = 0;
                        int jjj = method.Body.Instructions.Count();
                        for (int i = 0; i < jjj; i++)
                        {
                            if (method.Body.Instructions[ii].Operand != null && method.Body.Instructions[ii].OpCode == OpCodes.Ldstr)
                            {
                                
                                
                                Console.WriteLine(method.Body.Instructions[ii].Operand);
                                gg += method.Body.Instructions[i].Operand; gg += "\n";
                                XmlElement toTalTick = tDoc.CreateElement("_" + lala.ToString());

                                toTalTick2.AppendChild(toTalTick); //注意这里是 root.AppendChild
                                XmlElement toTalTick3 = tDoc.CreateElement("E"); toTalTick3.InnerText = method.Body.Instructions[ii].Operand.ToString();
                                XmlElement toTalTick4 = tDoc.CreateElement("C"); toTalTick4.InnerText = " ";
                                toTalTick.AppendChild(toTalTick3);
                                toTalTick.AppendChild(toTalTick4);
                                method.Body.Instructions[ii].OpCode = OpCodes.Ldstr;
                                method.Body.Instructions[ii].Operand = lala.ToString();
                               method.Body.Instructions.Insert(ii + 1,new Instruction(OpCodes.Call,me4));
                                ii+=2;
                                lala++;
                                

                            }


                        }



                    }


                    string encName = RandomString(10);
                    // method.Name = "method" + bb.ToString();
                    bb++;
                }
            }

            int www = lala;
            foreach (var type in md.GetTypes())
            {
                // methods in type
                foreach (MethodDef method in type.Methods)
                {
                    // empty method check
                    if (!method.HasBody) continue;
                    // iterate over instructions of method

                    for (int i = 0; i < method.Body.Instructions.Count(); i++)
                    {

                        if (method.Body.Instructions[i].Operand != null)
                        {
                            if (((string)(method.Body.Instructions[i].Operand.ToString())).Contains("System.String,System.String,System.String,System.String,System.String"))

                            {

                                //  Console.WriteLine(method.Body.Instructions[i].OpCode);
                                // c# variable has for loop scope only
                                
                                int ccc = Regex.Matches(method.Body.Instructions[i].Operand.ToString(), "String").Count;
                                String regString = method.Body.Instructions[i].Operand.ToString();
                                Console.WriteLine(method.Body.Instructions[i].Operand + Convert.ToString(ccc));
                                if (ccc ==5)
                                {
                                    int kkk = ccc;
                                    for (int kk = ccc; kk >=1; kk--)
                                    {
                                        if (method.Body.Instructions[i - kk].Operand != null)
                                        {
                                            try
                                            {
                                              
                                                if (method.Body.Instructions[i-kkk].OpCode == OpCodes.Ldstr)
                                                {
                                                    XmlElement toTalTick = tDoc.CreateElement("_" + lala.ToString());
                                                    toTalTick2.AppendChild(toTalTick); //注意这里是 root.AppendChild
                                                    XmlElement toTalTick3 = tDoc.CreateElement("E"); toTalTick3.InnerText = method.Body.Instructions[i - kkk].Operand.ToString();
                                                    XmlElement toTalTick4 = tDoc.CreateElement("C"); toTalTick4.InnerText = " ";
                                                    toTalTick.AppendChild(toTalTick3);
                                                    toTalTick.AppendChild(toTalTick4);



                                                    String encString = lala.ToString();// Class1.Post(method.Body.Instructions[i - kk].Operand.ToString());
                                                    lala++;

                                                    string res = encString;
                                                    Console.WriteLine(res);
                                                    gg += method.Body.Instructions[i - kkk].Operand; gg += "\n";
                                                    method.Body.Instructions[i - kkk].Operand = res;
                                                    method.Body.Instructions.Insert(i - kkk + 1, new Instruction(OpCodes.Call, me4));
                                                    kkk--;
                                                    i += 1;
                                                  
                                                }

                                            }
                                            catch
                                            { }
                                        }

                                    }




                                }

                            }
                            //   String encString = Convert.ToBase64String(UTF8Encoding.UTF8.GetBytes(regString));
                            /*
                            String encString = Class1.Post(regString);
                            Console.WriteLine(encString);
                            Regex reg = new Regex("tgt\":\"(.*?)\"}");
     
                            var result = reg.Match(encString).Groups;
                            Thread.Sleep(1000);

                            foreach (var item in result)

                            {
                                string filter =item.ToString();
                                string res = filter.Replace("tgt\":\"", "").Replace(",,", ",");
                                
                                 res = res.Replace("\"}", "").Replace(",,", ",");
                                Console.WriteLine(res);
                                method.Body.Instructions[i].Operand =res;
                            }

                            */

                            // String encString = "wwwwwwwww"; method.Body.Instructions[i].Operand = encString;
                            // methodology for adding code: write it in plain c#, compile, then view IL in dnspy
                            /*
                            method.Body.Instructions[i].OpCode = OpCodes.Nop; // errors occur if instruction not replaced with Nop

                            method.Body.Instructions.Insert(i + 1, new Instruction(OpCodes.Call, md.Import(typeof(System.Text.Encoding).GetMethod("get_UTF8", new Type[] { })))); // Load string onto stack
                            method.Body.Instructions.Insert(i + 2, new Instruction(OpCodes.Ldstr, encString)); // Load string onto stack
                            method.Body.Instructions.Insert(i + 3, new Instruction(OpCodes.Call, md.Import(typeof(System.Convert).GetMethod("FromBase64String", new Type[] { typeof(string) })))); // call method FromBase64String with string parameter loaded from stack, returned value will be loaded onto stack
                            method.Body.Instructions.Insert(i + 4, new Instruction(OpCodes.Callvirt, md.Import(typeof(System.Text.Encoding).GetMethod("GetString", new Type[] { typeof(byte[]) })))); // call method GetString with bytes parameter loaded from stack 
                            i += 4; //skip the Instructions as to not recurse on them
                            /*
                            String regString = method.Body.Instructions[i].Operand.ToString();

                            byte[] encString = Convert.FromBase64String(regString);
                            string hh = Encoding.UTF8.GetString(encString);
                            method.Body.Instructions[i - 2].OpCode = OpCodes.Ldstr;
                            method.Body.Instructions[i - 2].Operand = hh;
                            method.Body.Instructions.RemoveAt(i + 2);
                            method.Body.Instructions.RemoveAt(i + 1);
                            method.Body.Instructions.RemoveAt(i);
                            method.Body.Instructions.RemoveAt(i - 1);

                            // method.Body.Instructions[i-1].OpCode = OpCodes.Nop;
                            // method.Body.Instructions[i].OpCode = OpCodes.Nop;
                            // method.Body.Instructions[i+1].OpCode = OpCodes.Nop;
                            //  method.Body.Instructions[i+2].OpCode = OpCodes.Nop;
                            */



                            if (((string)(method.Body.Instructions[i].Operand.ToString())).Contains("GH_OutputParamManager")  || ((string)(method.Body.Instructions[i].Operand.ToString())).Contains("GH_InputParamManager"))

                            {

                                //  Console.WriteLine(method.Body.Instructions[i].OpCode);
                                // c# variable has for loop scope only

                                int ccc = Regex.Matches(method.Body.Instructions[i].Operand.ToString(), "String").Count+1;
                                String regString = method.Body.Instructions[i].Operand.ToString();
                                Console.WriteLine(method.Body.Instructions[i].Operand + Convert.ToString(ccc));
                                if (ccc == 4)
                                {
                                    int kkk = ccc;
                                    for (int kk = ccc; kk >= 1; kk--)
                                    {
                                        if (method.Body.Instructions[i - kk].Operand != null)
                                        {
                                            try
                                            {

                                                if (method.Body.Instructions[i - kkk].OpCode == OpCodes.Ldstr)
                                                {
                                                    XmlElement toTalTick = tDoc.CreateElement("_" + lala.ToString());
                                                    toTalTick2.AppendChild(toTalTick); //注意这里是 root.AppendChild
                                                    XmlElement toTalTick3 = tDoc.CreateElement("E"); toTalTick3.InnerText = method.Body.Instructions[i - kkk].Operand.ToString();
                                                    XmlElement toTalTick4 = tDoc.CreateElement("C"); toTalTick4.InnerText = " ";
                                                    toTalTick.AppendChild(toTalTick3);
                                                    toTalTick.AppendChild(toTalTick4);



                                                    String encString = lala.ToString();// Class1.Post(method.Body.Instructions[i - kk].Operand.ToString());
                                                    lala++;

                                                    string res = encString;
                                                    Console.WriteLine(res);
                                                    gg += method.Body.Instructions[i - kkk].Operand; gg += "\n";
                                                    method.Body.Instructions[i - kkk].Operand = res;
                                                    method.Body.Instructions.Insert(i - kkk + 1, new Instruction(OpCodes.Call, me4));
                                                    kkk--;
                                                    i += 1;

                                                }

                                            }
                                            catch
                                            { }
                                        }

                                    }




                                }

                            }

                        }
                        /*

                        else if (method.Body.Instructions[i].Operand !=null &&(method.Body.Instructions[i].Operand.ToString()).Contains("Grasshopper.GUI.HTML.GH_HtmlFormatter"))
                        {
                            
                            try
                            {
                                if (method.Body.Instructions[i + 2].Operand != null)
                                {
                                    String encString = Class1.Post(method.Body.Instructions[i + 2].Operand.ToString());
                                    Console.WriteLine(encString);
                                    Regex reg = new Regex("tgt\":\"(.*?)\"}");

                                    var result = reg.Match(encString).Groups;
                                    Thread.Sleep(new Random().Next(100,200));

                                    foreach (var item in result)

                                    {
                                        string filter = item.ToString();
                                        string res = filter.Replace("tgt\":\"", "").Replace(",,", ",");

                                        res = res.Replace("\"}", "").Replace(",,", ",");
                                        Console.WriteLine(res);
                                        if (method.Body.Instructions[i + 2].OpCode == OpCodes.Ldstr)
                                            method.Body.Instructions[i + 2].Operand = res;
                                    }
                                }
                            }
                            catch
                            { }

                        }
                        */
                    }
                }


            }



            foreach (var type in md.GetTypes())
            {
                foreach (MethodDef method in type.Methods)
                {
                    // empty method check
                    if (!method.HasBody) continue;

                    method.Body.SimplifyBranches();
                    method.Body.OptimizeBranches(); // negates simplifyBranches
                    method.Body.OptimizeMacros();                             //method.Body.OptimizeMacros();

                }
            }

            var options = new ModuleWriterOptions(md);
            options.MetadataOptions.Flags |= MetadataFlags.KeepOldMaxStack;
            
            md.Write(xx+houzi,options);
            
            File.WriteAllText("txt.txt",gg);


            tDoc.Save("data.xml");
           
          //  File.Delete(xx);
            Console.Write(md);
            Console.ReadKey();
        }

        private static TypeSig ReferenceType(TypeSig type, ModuleDef module)
        {
            if (type == null)
                return null;

            if (type.IsSZArray)
            {
                var szar = type.ToSZArraySig();
                var eleType = ReferenceType(szar.Next, module);
                if (eleType == null)
                    return null;
                return new SZArraySig(eleType);
            }

            if (type.IsArray)
            {
                var ar = type.ToArraySig();
                var eleType = ReferenceType(ar.Next, module);
                if (eleType == null)
                    return null;
                return new ArraySig(eleType, ar.Rank, ar.Sizes, ar.LowerBounds);
            }

            if (type.IsGenericInstanceType)
            {
                var g = type.ToGenericInstSig();

                var gtype = FindType(g.GenericType.FullName, module);
                ClassOrValueTypeSig ngt;
                if (gtype == null)
                    ngt = g.GenericType;
                else
                    ngt = gtype.TryGetClassOrValueTypeSig();

                TypeSig[] genericArgs = new TypeSig[g.GenericArguments.Count];
                for (int i = 0; i < g.GenericArguments.Count; ++i)
                {
                    var subArg = ReferenceType(g.GenericArguments[i], module);
                    if (subArg != null)
                        genericArgs[i] = subArg;
                    else
                        genericArgs[i] = g.GenericArguments[i];
                }

                return new GenericInstSig(ngt, genericArgs);
            }

            var targetType = FindType(type.FullName, module);
            if (targetType == null)
                return null;

            return targetType.ToTypeSig();
        }

        private static void ReCustomAttributes(IHasCustomAttribute type, ModuleDef module)
        {

            for (int i = 0; i < type.CustomAttributes.Count; ++i)
            {
                var newattr = FindType(type.CustomAttributes[i].TypeFullName, module);
                if (newattr == null)
                    continue;

                type.CustomAttributes[i] = new CustomAttribute(newattr.FindDefaultConstructor());
            }
        }

        private static void ReMethodSig(MethodSig sig, ModuleDef module)
        {
            var retType = sig.RetType;
            var ts = ReferenceType(sig.RetType, module);
            if (ts != null)
                sig.RetType = ts;

            for (var i = 0; i < sig.Params.Count; ++i)
            {
                ts = ReferenceType(sig.Params[i], module);
                if (ts == null)
                    continue;

                sig.Params[i] = ts;
            }
        }

        private static void ReFieldSig(FieldSig sig, ModuleDef module)
        {
            var ts = ReferenceType(sig.Type, module);
            if (ts != null)
                sig.Type = ts;
        }

        private static void ReMethodDef(MethodDef method, ModuleDef module)
        {
            ReCustomAttributes(method, module);

            var newtype = ReferenceType(method.ReturnType, module);
            if (newtype != null)
                method.ReturnType = newtype;

            for (int i = 0; i < method.ParamDefs.Count; ++i)
            {
                var p = method.ParamDefs[i];
                ReCustomAttributes(p, module);
            }

            for (int i = 0; i < method.Parameters.Count; ++i)
            {
                var p = method.Parameters[i];

                if (p.HasParamDef)
                    ReCustomAttributes(p.ParamDef, module);

                var newparam = ReferenceType(p.Type, module);
                if (newtype == null)
                    continue;

                p.Type = newparam;
            }


            for (int i = 0; i < method.GenericParameters.Count; ++i)
            {
                var genep = method.GenericParameters[i];
                ReCustomAttributes(genep, module);
                var negp = new GenericParamUser(genep.Number, genep.Flags, genep.Name);
                foreach (var ca in genep.CustomAttributes)
                {
                    negp.CustomAttributes.Add(ca);
                }

                method.GenericParameters[i] = negp;
            }

            ReMethodSig(method.MethodSig, module);
        }

        private static IEnumerable<TypeDef> AllNestTypes(TypeDef type)
        {
            yield return type;
            if (!type.NestedTypes.Any())
                yield break;

            foreach (var t in type.NestedTypes)
                foreach (var nt in AllNestTypes(t))
                    yield return nt;
        }

        private static TypeDef FindType(string fullName, ModuleDef module)
        {
            var newT = module.Find(fullName, false);
            if (newT != null)
                return newT;

            newT = (from t in module.Types
                    from it in AllNestTypes(t)
                    where it.FullName == fullName
                    select it).FirstOrDefault();

            return newT;
        }

        private static MethodDef FindMethod(string fullName, ModuleDef module)
        {
            var newM = (from t in module.Types
                        from it in AllNestTypes(t)
                        from im in it.Methods
                        where im.FullName == fullName
                        select im).FirstOrDefault();

            return newM;
        }

        private static FieldDef FindField(string fullName, ModuleDef module)
        {
            var newF = (from t in module.Types
                        from it in AllNestTypes(t)
                        from im in it.Fields
                        where im.FullName == fullName
                        select im).FirstOrDefault();

            return newF;
        }

        private static void ReMethodBody(TypeDef type, MethodDef method, ModuleDef module)
        {
            for (int i = 0; i < method.Body.Variables.Count; ++i)
            {
                var local = method.Body.Variables[i];

                var lt = ReferenceType(local.Type, module);
                if (lt != null)
                    local.Type = lt;
            }

            for (int i = 0; i < method.Body.Instructions.Count; ++i)
            {
                if (method.Body.Instructions[i].Operand == null)
                    continue;

                TypeSig ts;
                switch (method.Body.Instructions[i].OpCode.OperandType)
                {
                    case OperandType.InlineField:
                        var field = method.Body.Instructions[i].Operand as FieldDef;
                        if (field != null)
                        {
                            var newfield = FindField(field.FullName, module);
                            if (newfield != null)
                                method.Body.Instructions[i].Operand = newfield;

                            break;
                        }
                        var fieldR = method.Body.Instructions[i].Operand as MemberRef;
                        if (fieldR != null)
                        {
                            if (fieldR.IsFieldRef)
                            {
                                ReFieldSig(fieldR.FieldSig, module);
                            }
                            else if (fieldR.IsMethodRef)
                            {
                                throw new NotSupportedException(method.Body.Instructions[i].OpCode.OperandType.ToString());
                            }

                            break;
                        }

                        Console.WriteLine("InlineField {0}", method.Body.Instructions[i].Operand.GetType());
                        break;
                    case OperandType.InlineMethod:
                        var m = method.Body.Instructions[i].Operand as MemberRef;
                        if (m != null)
                        {
                            var tmodule = type.Module;
                            if (m.DeclaringType != null)
                            {
                                ts = ReferenceType(m.DeclaringType.ToTypeSig(), module);
                                if (ts != null)
                                    tmodule = ts.Module;
                            }

                            if (m.IsFieldRef)
                            {
                                throw new NotSupportedException(method.Body.Instructions[i].OpCode.OperandType.ToString());
                            }
                            else if (m.IsMethodRef)
                            {
                                ReMethodSig(m.MethodSig, module);
                            }

                            break;
                        }

                        var md = method.Body.Instructions[i].Operand as MethodDef;
                        if (md != null)
                        {
                            var newM = FindMethod(md.FullName, module);
                            if (newM != null)
                            {
                                method.Body.Instructions[i].Operand = newM;
                            }
                            break;
                        }

                        var ms = method.Body.Instructions[i].Operand as MethodSpec;
                        if (ms != null)
                        {
                            var msm = FindMethod(ms.Method.FullName, module);
                            if (msm != null)
                                ms.Method = msm;

                            for (int j = 0; j < ms.GenericInstMethodSig.GenericArguments.Count; ++j)
                            {
                                var ga = ReferenceType(ms.GenericInstMethodSig.GenericArguments[j], module);
                                if (ga == null)
                                    continue;

                                ms.GenericInstMethodSig.GenericArguments[j] = ga;
                            }
                            break;
                        }

                        Console.WriteLine("InlineMethod {0}", method.Body.Instructions[i].Operand.GetType());
                        break;
                    case OperandType.InlineType:
                        var it = method.Body.Instructions[i].Operand as TypeDef;
                        if (it != null)
                        {
                            ts = ReferenceType(it.ToTypeSig(), module);
                            if (ts != null)
                                method.Body.Instructions[i].Operand = ts.TryGetTypeDef();
                            break;
                        }

                        var itf = method.Body.Instructions[i].Operand as TypeRef;
                        if (itf != null)
                        {
                            ts = ReferenceType(it.ToTypeSig(), module);
                            if (ts != null)
                                method.Body.Instructions[i].Operand = ts.TryGetTypeRef();
                            break;
                        }

                        var its = method.Body.Instructions[i].Operand as TypeSpec;
                        if (its != null)
                        {
                            ts = ReferenceType(its.TypeSig, module);
                            if (ts != null)
                                its.TypeSig = ts;

                            break;
                        }

                        Console.WriteLine("InlineType {0}", method.Body.Instructions[i].Operand.GetType());
                        break;
                    case OperandType.ShortInlineVar:
                        var local = method.Body.Instructions[i].Operand as Local;
                        if (local == null)
                        {
                            Console.WriteLine("ShortInlineVar {0}", method.Body.Instructions[i].Operand.GetType());
                            break;
                        }
                        ts = ReferenceType(local.Type, module);
                        if (ts != null)
                            local.Type = ts;
                        break;
                    default:
                        break;
                }
            }
        }

        private static void ReReference(TypeDef type, ModuleDef module)
        {
            ReCustomAttributes(type, module);

            var baseType = type.BaseType.ToTypeSig();
            if (baseType != null)
            {
                baseType = ReferenceType(baseType, module);
                if (baseType != null)
                    type.BaseType = baseType.ToTypeDefOrRef();
            }

            for (int i = 0; i < type.Interfaces.Count; ++i)
            {
                var intype = type.Interfaces[i].Interface.ToTypeSig();
                intype = ReferenceType(intype, module);
                if (intype == null)
                    continue;

                type.Interfaces[i].Interface = intype.ToTypeDefOrRef();
            }


            for (int i = 0; i < type.Fields.Count; ++i)
            {
                var field = type.Fields[i];
                ReCustomAttributes(field, module);

                var newtype = ReferenceType(field.FieldType, module);
                if (newtype == null)
                    continue;

                field.FieldType = newtype;
            }

            for (int i = 0; i < type.Events.Count; ++i)
            {
                var e = type.Events[i];
                if (e.AddMethod != null)
                    ReMethodDef(e.AddMethod, module);
                if (e.RemoveMethod != null)
                    ReMethodDef(e.RemoveMethod, module);
                ReCustomAttributes(e, module);

                var et = ReferenceType(e.EventType.ToTypeSig(), module);
                if (et == null)
                    continue;

                e.EventType = et.ToTypeDefOrRef();
            }


            for (int i = 0; i < type.Properties.Count; ++i)
            {
                var p = type.Properties[i];
                ReCustomAttributes(p, module);

                var retType = ReferenceType(p.PropertySig.RetType, module);
                if (retType != null)
                    p.PropertySig.RetType = retType;

                for (int j = 0; j < p.PropertySig.Params.Count; ++j)
                {
                    var pp = p.PropertySig.Params[j];
                    pp = ReferenceType(pp, module);
                    if (pp == null)
                        continue;

                    p.PropertySig.Params[j] = pp;
                }

                if (p.GetMethod != null)
                    ReMethodDef(p.GetMethod, module);
                if (p.SetMethod != null)
                    ReMethodDef(p.SetMethod, module);
            }

            for (int i = 0; i < type.Methods.Count; ++i)
            {
                var method = type.Methods[i];
                ReMethodDef(method, module);
                ReMethodBody(type, method, module);
            }

            for (int i = 0; i < type.NestedTypes.Count; ++i)
            {
                var nest = type.NestedTypes[i];

                ReReference(nest, module);
            }
        }

        private static void DupType(TypeDef src, TypeDef dest)
        {
            dest.Attributes = src.Attributes;
            dest.ClassLayout = src.ClassLayout;
            dest.ClassSize = src.ClassSize;
            dest.Visibility = src.Visibility;
            dest.BaseType = src.BaseType;

            for (int i = 0; i < dest.NestedTypes.Count; ++i)
            {
                var dnt = dest.NestedTypes[i];
                var snt = src.NestedTypes.FirstOrDefault(nt => nt.FullName == dnt.FullName);

                if (snt == null)
                    continue;

                DupType(snt, dnt);
            }

            var ntArray = src.NestedTypes.Where(snt => !dest.NestedTypes.Any(dnt => dnt.FullName == snt.FullName)).ToArray();
            foreach (var nt in ntArray)
            {
                nt.DeclaringType = dest;
            }

            var caArray = src.CustomAttributes.Where(snt => !dest.CustomAttributes.Any(dnt => dnt.TypeFullName == snt.TypeFullName)).ToArray();
            foreach (var attr in caArray)
            {

                dest.CustomAttributes.Add(attr);
            }

            var iArray = src.Interfaces.Where(snt => !dest.Interfaces.Any(dnt => dnt.Interface.FullName == snt.Interface.FullName)).ToArray();
            foreach (var i in iArray)
            {
                dest.Interfaces.Add(i);
            }

            for (int i = 0; i < dest.Fields.Count; ++i)
            {
                var ditem = dest.Fields[i];
                var sitem = src.Fields.FirstOrDefault(item => item.FullName == ditem.FullName);

                if (sitem == null)
                    continue;

                ditem.Constant = sitem.Constant;
                ditem.Attributes = sitem.Attributes;
                ditem.FieldOffset = sitem.FieldOffset;
                ditem.Access = sitem.Access;
                ditem.FieldType = sitem.FieldType;
                ditem.FieldSig = sitem.FieldSig;
                var itemca = sitem.CustomAttributes.Where(snt => !ditem.CustomAttributes.Any(dnt => dnt.TypeFullName == snt.TypeFullName)).ToArray();
                foreach (var attr in itemca)
                {
                    ditem.CustomAttributes.Add(attr);
                }
            }
            var fdArray = src.Fields.Where(snt => !dest.Fields.Any(dnt => dnt.FullName == snt.FullName)).ToArray();
            foreach (var fd in fdArray)
            {
                fd.DeclaringType = dest;
            }



            for (int i = 0; i < dest.Methods.Count; ++i)
            {
                var dmd = dest.Methods[i];
                var smd = src.Methods.FirstOrDefault(md => md.FullName == dmd.FullName);

                if (smd == null)
                    continue;

                dmd.Attributes = smd.Attributes;
                dmd.Access = smd.Access;
                dmd.ReturnType = smd.ReturnType;
                dmd.Body = smd.Body;
                dmd.MethodSig.Params.Clear();
                for (int j = 0; j < smd.MethodSig.Params.Count; ++j)
                {
                    dmd.MethodSig.Params.Add(smd.MethodSig.Params[j]);
                }
                var itemca = dmd.CustomAttributes.Where(snt => !smd.CustomAttributes.Any(dnt => dnt.TypeFullName == snt.TypeFullName)).ToArray();
                foreach (var attr in itemca)
                {
                    dmd.CustomAttributes.Add(attr);
                }
            }
            var mdArray = src.Methods.Where(snt => !dest.Methods.Any(dnt => dnt.FullName == snt.FullName)).ToArray();
            foreach (var md in mdArray)
            {
                md.DeclaringType = dest;
            }

            for (int i = 0; i < dest.Properties.Count; ++i)
            {
                var ditem = dest.Properties[i];
                var sitem = src.Properties.FirstOrDefault(item => item.FullName == ditem.FullName);

                if (sitem == null)
                    continue;

                ditem.Type = sitem.Type;
                ditem.Attributes = sitem.Attributes;
                ditem.PropertySig = sitem.PropertySig;
                var itemca = sitem.CustomAttributes.Where(snt => !ditem.CustomAttributes.Any(dnt => dnt.TypeFullName == snt.TypeFullName)).ToArray();
                foreach (var attr in itemca)
                {
                    ditem.CustomAttributes.Add(attr);
                }
            }
            var ptArray = src.Properties.Where(snt => !dest.Properties.Any(dnt => dnt.FullName == snt.FullName)).ToArray();
            foreach (var pt in ptArray)
            {
                pt.DeclaringType = dest;
            }


            for (int i = 0; i < dest.Events.Count; ++i)
            {
                var ditem = dest.Events[i];
                var sitem = src.Events.FirstOrDefault(item => item.FullName == ditem.FullName);

                if (sitem == null)
                    continue;

                ditem.EventType = sitem.EventType;
                ditem.Attributes = sitem.Attributes;
                var itemca = sitem.CustomAttributes.Where(snt => !ditem.CustomAttributes.Any(dnt => dnt.TypeFullName == snt.TypeFullName)).ToArray();
                foreach (var attr in itemca)
                {
                    ditem.CustomAttributes.Add(attr);
                }
            }
            var evArray = src.Events.Where(snt => !dest.Events.Any(dnt => dnt.FullName == snt.FullName)).ToArray();
            foreach (var ev in evArray)
            {
                ev.DeclaringType = dest;
            }
        }
    



}


}
