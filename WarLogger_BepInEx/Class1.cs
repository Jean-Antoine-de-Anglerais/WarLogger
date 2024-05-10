using BepInEx;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using UnityEngine.Scripting;
using static ConstantNamespace.ConstantClass;

namespace WarLogger_BepInEx
{
    [BepInPlugin(pluginGuid, pluginName, pluginVersion)]
    public class WarLoggerClass : BaseUnityPlugin
    {
        public static Harmony harmony = new Harmony(pluginName);
        private bool _initialized = false;

        public void Awake()
        {
            Logger.LogMessage("ХООООООООЙ");

        }

        public void Start()
        {
            if (global::Config.gameLoaded)
            {
                Logger.LogMessage("Пропатчено");
            }
        }

        // Метод, запускающийся каждый кадр (в моём случае он зависим от загрузки игры)
        public void Update()
        {
            if (global::Config.gameLoaded)
            {
            }

            if (global::Config.gameLoaded && !_initialized)
            {
                harmony.Patch(AccessTools.Method(typeof(WarManager), "endWar"),
                prefix: new HarmonyMethod(AccessTools.Method(typeof(Patches), "endWar_Prefix")));

                _initialized = true;
            }
        }
    }

    public class WarLoggerData : BaseSystemData
    {
        public WarLoggerData(War war)
        {
            var data = war.data;
            this.id = data.id;
            this.name = data.name;
            this.created_time = data.created_time;
            this.main_attacker = data.main_attacker;
            this.main_defender = data.main_defender;
            this.list_attackers = data.list_attackers;
            this.list_defenders = data.list_defenders;
            this.dead = data.dead;
            this.dead_attackers = data.dead_attackers;
            this.dead_defenders = data.dead_defenders;
            this.started_by_king = data.started_by_king;
            this.started_by_kingdom = data.started_by_kingdom;
            this.war_type = LocalizedTextManager.getText(war.getAsset().localized_type, null);
            war_started_data = war.getFoundedDate();
            end_time = World.world.getCurWorldTime();
            war_ended_data = World.world.mapStats.getDate(end_time);
            string_attackers_names = string.Join(", ", war.getAttackers().Select(k => k.name));
            string_defenders_names = string.Join(", ", war.getDefenders().Select(k => k.name));
        }

        #region Существующие переменные
        // Уникальный идиентификатор
        [DefaultValue("")]
        new public string id = "";

        // Название
        [DefaultValue("")]
        new public string name = "";

        // Время начала
        [DefaultValue(0)]
        new public double created_time;

        [Preserve]
        [DefaultValue(1)]
        [Obsolete("Use created_time instead")]
        new public int age = 1;

        // Существует ли
        [DefaultValue(true)]
        new public bool alive = true;
        #endregion

        // Главный атакующтий
        public string main_attacker;

        // Главный защищающийся
        public string main_defender;

        // Список атакующих
        public List<string> list_attackers = new List<string>();

        // Список защищающихся
        public List<string> list_defenders = new List<string>();

        // Всего смертей
        public int dead;

        // Смертей атакующих
        public int dead_attackers;

        // Смертей защищающихся
        public int dead_defenders;

        // Имя начавшего
        public string started_by_king = "-";

        // Название начавшего государства
        public string started_by_kingdom = "-";

        // Тип войны
        public string war_type;

        #region Новые переменные
        // Имя закончившего
        public string ended_by_king = "-";

        // Название закончившего государства
        public string ended_by_kingdom = "-";

        // Как именно была закончена война
        public string war_end_type;

        // Когда была закончена война
        public double end_time;

        // Дата начала войны
        public string war_started_data;

        // Дата окончания войны
        public string war_ended_data;

        public string string_attackers_names;

        public string string_defenders_names;
        #endregion
    }

    public static class Saver
    {
        public static void Prepare(string world_name, War war)
        {
            WarLoggerData war_logged = new WarLoggerData(war);

            string main_folder_path = Path.Combine(Application.streamingAssetsPath, "Saved Wars");
            if (!Directory.Exists(main_folder_path))
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(main_folder_path);
                directoryInfo.Create();
            }

            string example_path = Path.Combine(main_folder_path, world_name);
            if (!Directory.Exists(example_path))
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(example_path);
                directoryInfo.Create();
            }

            string war_file_path = Path.Combine(example_path, war_logged.id + $", с {war_logged.war_started_data} и по {war_logged.war_ended_data}" + "txt");

            try
            {
                string war_statistics = $" ID - {war_logged.id}" +
                $"\r Название - {war_logged.name}" +
                $"\r Дата начала - {war_logged.war_started_data}" +
                $"\r Дата завершения - {war_logged.war_ended_data}" +
                $"\r Тип войны - {war_logged.war_type}" +
                $"\r Все нападающие - {war_logged.string_attackers_names}" +
                $"\r Все защитники - {war_logged.string_defenders_names}" +                
                $"\r Умерло нападающих - {war_logged.dead_attackers}" +
                $"\r Умерло защищающих - {war_logged.dead_defenders}" +
                $"\r Всего умерло - {war_logged.dead}" +
                $"\r Начата королём - {war_logged.started_by_king}" +
                $"\r Начата королевством - {war_logged.started_by_kingdom}";

                if (war.getAttackers().Count > 0)
                {
                    war_statistics += $"\r\r Все короли, участвующие на стороне нападения на момент завершения войны:";

                    foreach (var kingdom in war.getAttackers())
                    {
                        if (kingdom.isAlive())
                        {
                            if (kingdom.hasKing())
                            {
                                war_statistics += $"\r Королевство - {kingdom.name}, король - {kingdom.king.getName()}";
                            }
                            else
                            {
                                war_statistics += $"\r Королевство - {kingdom.name}, короля нет";
                            }
                        }
                    }
                    war_statistics += $"\r";
                }

                if (war.getDefenders().Count > 0)
                {
                    war_statistics += $"\r Все короли, участвующие на стороне защиты на момент завершения войны:";

                    foreach (var kingdom in war.getDefenders())
                    {
                        if (kingdom.isAlive())
                        {
                            if (kingdom.hasKing())
                            {
                                war_statistics += $"\r Королевство - {kingdom.name}, король - {kingdom.king.getName()}";
                            }
                            else
                            {
                                war_statistics += $"\r Королевство - {kingdom.name}, короля нет";
                            }
                        }
                    }
                    war_statistics += $"\r";
                }

                if (war.main_attacker != null)
                {
                    war_statistics += $"\r Главное атакующее государство - {war.main_attacker.name}";
                }

                if (war.main_defender != null)
                {
                    war_statistics += $"\r Главное защищающее государство - {war.main_defender.name}";
                }

                try
                {
                    using (StreamWriter writer = new StreamWriter(war_file_path))
                    {
                        writer.WriteLine(war_statistics);
                    }
                }

                catch (Exception ex)
                {
                    Debug.LogError("Ошибка - что-то связанное с непосредственно записью данных " + ex);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("Ошибка - что-то связанное с созданием данных" + ex); // ОШИБКА ВОЗНИКАЕТ ИМЕННО ТУТ
            }



        }
    }

    public class Patches
    {
        public static void endWar_Prefix(WarManager __instance, War pWar, bool pLog = true)
        {
            if (__instance == World.world.wars)
            {
                
            }

            if (pWar != null && pWar.isAlive())
            {
                Saver.Prepare(World.world.mapStats.name, pWar);
            }
        }
    }
}
