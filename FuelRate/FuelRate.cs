using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.Reflection;
using UnityEngine;

namespace FuelRate
{
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    public class Main : BaseUnityPlugin
    {
        const string PluginGuid = "com.example.GUID";
        const string PluginName = "MyPlugin";
        const string PluginVersion = "1.0.0";

        public static readonly bool IsDebug = true;
        private readonly Harmony _harmonyInstance = new Harmony(PluginGuid);
        public static Main Context;
        public static ManualLogSource logger = BepInEx.Logging.Logger.CreateLogSource(PluginName);

        public void Awake()
        {
            Main.logger.LogInfo("Thank you for using my mod!");

            Context = this;
            Assembly assembly = Assembly.GetExecutingAssembly();
            _harmonyInstance.PatchAll(assembly);
        }

        [HarmonyPatch(typeof(Player), nameof(Player.UseStamina))]
        public static class PatchPlayerUseStamina
        {
            private static bool Prefix()
            {
                return false;
            }
        }
        
        public static void Dbgl(string str = "", bool pref = true)
        {
            if (IsDebug)
                Debug.Log((pref ? typeof(Main).Namespace + " " : "") + str);
        }
        
        [HarmonyPatch(typeof(Fireplace), "Start")]
        public static class PatchFireplaceStart
        {
            private static void Postfix(Fireplace __instance)
            {
                /*
                if (!modEnabled.Value)
                    return;
                */
                
                var name = __instance.name;
                Dbgl($"starting fireplace {name}");
                
                var maxFuel = __instance.m_maxFuel * 10f;
                __instance.m_maxFuel = maxFuel;
                
                Dbgl($"Fuel rate set to {maxFuel}");
            }
        }
    }
}