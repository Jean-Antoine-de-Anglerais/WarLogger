using BepInEx;
using HarmonyLib;
using System.Reflection;
using static ConstantNamespace.ConstantClass;

namespace WarLogger_BepInEx
{
    [BepInPlugin(pluginGuid, pluginName, pluginVersion)]
    public class Main : BaseUnityPlugin
    {
        public static Harmony harmony = new Harmony(pluginName);
        private bool _initialized = false;

        public void Awake()
        {
            // Logger.LogMessage("ХООООООООЙ");

            // Debug.Log($"{MethodBase.GetCurrentMethod().DeclaringType.Namespace} loading...");
            // 
            // Assembly assembly = AccessTools.AllAssemblies().FirstOrDefault(t => t.GetName().Name == "Assembly-CSharp");
            // 
            // if (assembly != null)
            // {
            //     var war = assembly.GetModule("War");
            // 
            //     if (war != null) 
            //     {
            //         war.
            //     }
            // }
            // 
            // 
            // var fieldDefold = 
        }

        public void Start()
        {

        }

        public void Update()
        {
            // if (global::Config.gameLoaded)
            // {
            // }

            if (global::Config.gameLoaded && !_initialized)
            {
                harmony.Patch(AccessTools.Method(typeof(WarManager), nameof(WarManager.endWar)),
                    prefix: new HarmonyMethod(AccessTools.Method(typeof(Patches), nameof(Patches.endWar_Prefix))));

                harmony.Patch(AccessTools.Method(typeof(WarManager), nameof(WarManager.stopAllWars)),
                    transpiler: new HarmonyMethod(AccessTools.Method(typeof(Patches), nameof(Patches.stopAllWars_Transpiler))));

                harmony.Patch(AccessTools.Method(typeof(PlotsLibrary).GetNestedType("<>c", BindingFlags.NonPublic), "<init>b__6_25"),
                    transpiler: new HarmonyMethod(AccessTools.Method(typeof(Patches), "init_Transpiler")));

                _initialized = true;
            }
        }
    }
}
