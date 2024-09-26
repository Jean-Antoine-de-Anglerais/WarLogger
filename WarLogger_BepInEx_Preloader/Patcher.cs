using Mono.Cecil;
using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WarLogger_BepInEx_Preloader
{
    public static class Patcher
    {
        // List of assemblies to patch
        public static IEnumerable<string> TargetDLLs { get; } = new[] { "Assembly-CSharp.dll" };

        // Patches the assemblies
        public static void Patch(AssemblyDefinition assembly)
        {
            // Patcher code here

            ModuleDefinition mainModule = assembly.MainModule;

            var customWarData = new TypeDefinition("", "CustomWarData", 
                TypeAttributes.Class | TypeAttributes.Public, 
                mainModule.ImportReference(typeof(object)));

            Console.WriteLine(customWarData.Methods.Count);

            // Create a constructor
            var customWarDataConstructor = new MethodDefinition(
                ".ctor",
                MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName,
                mainModule.ImportReference(typeof(void)));

            // Generate IL code for the constructor
            var customWarDataIlProcessor = customWarDataConstructor.Body.GetILProcessor();

            // Start of the method
            customWarDataIlProcessor.Emit(OpCodes.Ldarg_0); // Загружаем ссылку на текущий объект (this)

            // Call the base constructor (object)
            customWarDataIlProcessor.Emit(OpCodes.Call, mainModule.ImportReference(typeof(object).GetConstructor(Type.EmptyTypes)));

            // End the method
            customWarDataIlProcessor.Emit(OpCodes.Ret);

            customWarData.Methods.Add(customWarDataConstructor);  // Add a constructor to the class

            mainModule.Types.Add(customWarData);

            var warClass = mainModule.Types.FirstOrDefault(t => t.Name == "War");

            var warData = mainModule.ImportReference(customWarData);

            var newField = new FieldDefinition("warData", FieldAttributes.Public, warData);

            warClass.Fields.Add(newField);

            // Add initialization in the constructor
            var warConstructor = warClass.Methods.FirstOrDefault(m => m.Name == ".ctor");

            if (warConstructor != null)
            {
                var ilProcessor = warConstructor.Body.GetILProcessor();
                ilProcessor.InsertBefore(warConstructor.Body.Instructions[0], Instruction.Create(OpCodes.Ldarg_0));
                ilProcessor.InsertBefore(warConstructor.Body.Instructions[1], Instruction.Create(OpCodes.Newobj, mainModule.ImportReference(customWarData.Resolve().Methods.First(m => m.Name == ".ctor"))));
                ilProcessor.InsertBefore(warConstructor.Body.Instructions[2], Instruction.Create(OpCodes.Stfld, newField));
            }
        }


        // public static void PrintTypes(string fileName)
        // {
        //     fileName = @"C:\Program Files (x86)\Steam\steamapps\common\worldbox\worldbox_Data\Managed\" + fileName + ".dll";
        // 
        //     ModuleDefinition module = ModuleDefinition.ReadModule(fileName);
        // 
        //     foreach (TypeDefinition type in module.Types)
        //     {
        //         if (!type.IsPublic)
        //         {
        //             continue;
        //         }
        // 
        //         Console.WriteLine(type.FullName);
        //     }
        // }
    }
}
