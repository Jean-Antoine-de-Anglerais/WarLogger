using Mono.Cecil;
using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using WarLogger_Helper;

namespace WarLogger_BepInEx_Preloader
{
    public static class Patcher
    {
        public static IEnumerable<string> TargetDLLs { get; } = new[] { "Assembly-CSharp.dll" };

        public static void Initialize()
        {
            try { Assembly.LoadFrom(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "WarLogger_Helper.dll")); }

            catch (Exception ex) { throw ex; }
        }

        public static void Finish()
        {

        }

        public static void Patch(AssemblyDefinition assembly)
        {
            ModuleDefinition mainModule = assembly.MainModule;

            var warClass = mainModule.Types.FirstOrDefault(t => t.Name == "War");

            var warData = mainModule.ImportReference(typeof(CustomWarData));

            var newField = new FieldDefinition("warData", Mono.Cecil.FieldAttributes.Public, warData);

            warClass.Fields.Add(newField);

            var warConstructor = warClass.Methods.FirstOrDefault(m => m.Name == ".ctor");

            var ilProcessor = warConstructor.Body.GetILProcessor();
            ilProcessor.InsertBefore(warConstructor.Body.Instructions[0], Instruction.Create(OpCodes.Ldarg_0));
            ilProcessor.InsertBefore(warConstructor.Body.Instructions[1], Instruction.Create(OpCodes.Newobj, mainModule.ImportReference(typeof(CustomWarData).GetConstructor(Type.EmptyTypes))));
            ilProcessor.InsertBefore(warConstructor.Body.Instructions[2], Instruction.Create(OpCodes.Stfld, newField));
        }
    }
}
