using ReflectionUtility;
using WarLogger_Helper;

namespace WarLogger_BepInEx
{
    public static class Transpilers
    {
        public static void stopAllWars_Transpiler(War war)
        {
            CustomWarData сustomWarData = (CustomWarData)Reflection.GetField(war.GetType(), war, "warData");

            сustomWarData.warEndReasons = WarEndReasons.StopAllWars;
        }

        public static void init_Transpiler(Plot plot)
        {
            CustomWarData сustomWarData = (CustomWarData)Reflection.GetField(plot.target_war.GetType(), plot.target_war, "warData");

            сustomWarData.warEndReasons = WarEndReasons.Plot;
        }
    }
}
