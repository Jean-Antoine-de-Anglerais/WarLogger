using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace WarLogger_BepInEx
{
    public static class Patches
    {
        public static void endWar_Prefix(WarManager __instance, War pWar, bool pLog = true)
        {
            if (__instance == World.world.wars)
            {

            }

            if (pWar != null && pWar.isAlive())
            {
                // Saver.Prepare(World.world.mapStats.name, pWar);


                for (int i = 0; i < 200; i++)
                {
                    Console.WriteLine();
                }
            }
        }

        public static IEnumerable<CodeInstruction> stopAllWars_Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            /* Reference:
                IL_000F: ldloc.0
                IL_0010: call      void Transpilers::stopAllWars_Transpiler(class War)
            */

            var codes = new List<CodeInstruction>(instructions);

            for (int i = 0; i < codes.Count; i++)
            {
                if (codes[i].opcode == OpCodes.Stloc_0)
                {
                    var newCodes = new List<CodeInstruction>
                    {
                        new CodeInstruction(OpCodes.Ldloc_0),
                        new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Transpilers), nameof(Transpilers.stopAllWars_Transpiler))),
                    };

                    codes.InsertRange(i + 1, newCodes);
                }
            }

            return codes.AsEnumerable();
        }

        public static IEnumerable<CodeInstruction> init_Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var codes = new List<CodeInstruction>(instructions);

            var newCodes = new List<CodeInstruction>
            {
                new CodeInstruction(OpCodes.Ldarg_1),
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Transpilers), nameof(Transpilers.init_Transpiler))),
            };

            codes.InsertRange(0, newCodes);

            return codes.AsEnumerable();
        }
    }
}
