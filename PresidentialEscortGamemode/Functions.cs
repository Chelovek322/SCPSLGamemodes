using Smod2;
using UnityEngine;
using Smod2.API;
using System.Collections.Generic;
using System;

namespace PresidentialEscortGamemode
{
    public class Functions
    {
        public static Functions singleton;
        public PresidentialEscort PresidentialEscort;
        public Functions(PresidentialEscort plugin)
        {
            this.PresidentialEscort = plugin;
            Functions.singleton = this;
        }
        public void EnableGamemode()
        {
            PresidentialEscort.enabled = true;
            if (!PresidentialEscort.roundstarted)
            {
                PresidentialEscort.pluginManager.Server.Map.ClearBroadcasts();
                PresidentialEscort.pluginManager.Server.Map.Broadcast(25, "Игровой режим <color=#f8ea56>Президентский Эскорт</color> начинается...", false);
            }
        }
        public void DisableGamemode()
        {
            PresidentialEscort.enabled = false;
            PresidentialEscort.pluginManager.Server.Map.ClearBroadcasts();
        }
        public void EndGamemodeRound()
        {
            PresidentialEscort.Info("EndgameRound Function");
            PresidentialEscort.roundstarted = false;
            PresidentialEscort.Server.Round.EndRound();
            PresidentialEscort.vip = null;
            PresidentialEscort.vipEscaped = false;
        }

        public IEnumerable<float> SpawnVIP(Player player)
        {
            PresidentialEscort.vip = player;
            Vector spawn = PresidentialEscort.Server.Map.GetRandomSpawnPoint(Role.CLASSD);
            player.ChangeRole(Role.SCIENTIST, false, false, true, false);
            yield return 2;
            player.Teleport(spawn);

            foreach (Smod2.API.Item item in player.GetInventory())
            {
                item.Remove();
            }
            player.GiveItem(ItemType.MAJOR_SCIENTIST_KEYCARD);
            player.GiveItem(ItemType.MEDKIT);
            player.GiveItem(ItemType.RADIO);
            player.GiveItem(ItemType.FLASHLIGHT);

            player.PersonalClearBroadcasts();
            player.PersonalBroadcast(15, "Ты <color=#f8ea56>VIP</color>. Сбеги из комплекса кооперируясь с " +
                "<color=#308ADA>МТФ</color>, избегая <color=#e83e25>SCP</color>.", false);

        }

        public IEnumerable<float> SpawnNTF(Player player)
        {
            Vector spawn = PresidentialEscort.Server.Map.GetRandomSpawnPoint(Role.CLASSD);
            player.ChangeRole(Role.FACILITY_GUARD, false, true, false, false);
            yield return 2;
            player.Teleport(spawn);

            foreach (Smod2.API.Item item in player.GetInventory())
            {
                item.Remove();
            }

            player.SetAmmo(AmmoType.DROPPED_5, 500);
            player.SetAmmo(AmmoType.DROPPED_7, 500);
            player.SetAmmo(AmmoType.DROPPED_9, 500);
            player.GiveItem(ItemType.RADIO);
            player.GiveItem(ItemType.P90);
            player.GiveItem(ItemType.FRAG_GRENADE);
            player.GiveItem(ItemType.MEDKIT);
            player.GiveItem(ItemType.SENIOR_GUARD_KEYCARD);
            player.GiveItem(ItemType.FLASHLIGHT);

            player.PersonalClearBroadcasts();
            player.PersonalBroadcast(15, "Ты <color=#308ADA>МТФ Кадет</color>. Сотрудничай с остальными, чтобы помочь " +
                "<color=#f8ea56>VIP</color> сбежать и ликвидировать <color=#e83e25>SCP</color>", false);
        }
    }
}