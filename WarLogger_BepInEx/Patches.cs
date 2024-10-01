using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
