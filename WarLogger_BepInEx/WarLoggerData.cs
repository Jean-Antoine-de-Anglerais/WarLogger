using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Scripting;
using System.ComponentModel;

namespace WarLogger_BepInEx
{
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
}
