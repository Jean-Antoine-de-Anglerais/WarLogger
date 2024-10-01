using System;
using System.IO;
using UnityEngine;

namespace WarLogger_BepInEx
{
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
}
