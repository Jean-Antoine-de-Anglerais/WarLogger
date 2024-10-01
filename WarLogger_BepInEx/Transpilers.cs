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
    }
}
