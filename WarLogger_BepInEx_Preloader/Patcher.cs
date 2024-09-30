using Mono.Cecil;
using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.Linq;
using WarLogger_Helper;

namespace WarLogger_BepInEx_Preloader
{
    public static class Patcher
    {
        public static IEnumerable<string> TargetDLLs { get; } = new[] { "Assembly-CSharp.dll" };

        public static void Patch(AssemblyDefinition assembly)
        {
            ModuleDefinition mainModule = assembly.MainModule;

            var warClass = mainModule.Types.FirstOrDefault(t => t.Name == "War");

            var warData = mainModule.ImportReference(typeof(CustomWarData));

            var newField = new FieldDefinition("warData", FieldAttributes.Public, warData);

            warClass.Fields.Add(newField);

            var warConstructor = warClass.Methods.FirstOrDefault(m => m.Name == ".ctor");

            var ilProcessor = warConstructor.Body.GetILProcessor();
            ilProcessor.InsertBefore(warConstructor.Body.Instructions[0], Instruction.Create(OpCodes.Ldarg_0));
            ilProcessor.InsertBefore(warConstructor.Body.Instructions[1], Instruction.Create(OpCodes.Newobj, mainModule.ImportReference(warData.Resolve().Methods.First(m => m.Name == ".ctor"))));
            ilProcessor.InsertBefore(warConstructor.Body.Instructions[2], Instruction.Create(OpCodes.Stfld, newField));
        }
    }
}
