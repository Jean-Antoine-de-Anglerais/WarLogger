using Mono.Cecil;
using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WarLogger_BepInEx_Preloader
{
    public static class Patcher
    {
        public static IEnumerable<string> TargetDLLs { get; } = new[] { "Assembly-CSharp.dll" };

        public static void Patch(AssemblyDefinition assembly)
        {
            ModuleDefinition mainModule = assembly.MainModule;

            var customWarData = new TypeDefinition("", "CustomWarData", 
                TypeAttributes.Class | TypeAttributes.Public, 
                mainModule.ImportReference(typeof(object)));

            CustomWarDataClassMaking(customWarData, mainModule);

            mainModule.Types.Add(customWarData);

            var warClass = mainModule.Types.FirstOrDefault(t => t.Name == "War");

            var warData = mainModule.ImportReference(customWarData);

            var newField = new FieldDefinition("warData", FieldAttributes.Public, warData);

            warClass.Fields.Add(newField);

            var warConstructor = warClass.Methods.FirstOrDefault(m => m.Name == ".ctor");

            var ilProcessor = warConstructor.Body.GetILProcessor();
            ilProcessor.InsertBefore(warConstructor.Body.Instructions[0], Instruction.Create(OpCodes.Ldarg_0));
            ilProcessor.InsertBefore(warConstructor.Body.Instructions[1], Instruction.Create(OpCodes.Newobj, mainModule.ImportReference(customWarData.Resolve().Methods.First(m => m.Name == ".ctor"))));
            ilProcessor.InsertBefore(warConstructor.Body.Instructions[2], Instruction.Create(OpCodes.Stfld, newField));   
        }

        private static void CustomWarDataClassMaking(TypeDefinition customWarData, ModuleDefinition mainModule)
        {
            var customWarDataConstructor = new MethodDefinition(
                ".ctor",
                MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName,
                mainModule.ImportReference(typeof(void)));

            var customWarDataIlProcessor = customWarDataConstructor.Body.GetILProcessor();

            customWarDataIlProcessor.Emit(OpCodes.Ldarg_0); 
            customWarDataIlProcessor.Emit(OpCodes.Call, mainModule.ImportReference(typeof(object).GetConstructor(Type.EmptyTypes)));
            customWarDataIlProcessor.Emit(OpCodes.Ret);

            customWarData.Methods.Add(customWarDataConstructor);
        }
    }
}
