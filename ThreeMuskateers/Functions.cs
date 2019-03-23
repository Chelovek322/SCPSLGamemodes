using Smod2;
using Smod2.API;
using System.Collections.Generic;

namespace MuskateersGamemode
{
    public class Functions
    {
        public static Functions singleton;
        public Muskateers Muskateers;
        public Functions(Muskateers plugin)
        {
            this.Muskateers = plugin;
            Functions.singleton = this;
        }
        public void EnableGamemode()
        {
            Muskateers.enabled = true;
            if (!Muskateers.roundstarted)
            {
                Muskateers.pluginManager.Server.Map.ClearBroadcasts();
                Muskateers.pluginManager.Server.Map.Broadcast(25, "Игровой режим<color=#308ADA> Три Мушкетёра</color> начинается.", false);
            }
        }
        public void DisableGamemode()
        {
            Muskateers.enabled = false;
            Muskateers.pluginManager.Server.Map.ClearBroadcasts();
        }
        public IEnumerable<float> SpawnNTF(Player player)
        {
            player.ChangeRole(Role.NTF_COMMANDER, true, true, false, true);
            yield return 2;
            player.SetHealth(Muskateers.ntf_health);
            player.PersonalClearBroadcasts();
            player.PersonalBroadcast(25, "Ты <color=#308ADA>Мушкетёр</color>. Войди в комплекс и ликвидируй весь Д-Класс.", false);
        }
        public IEnumerable<float> SpawnClassD(Player player)
        {
            player.ChangeRole(Role.CLASSD, true, true, false, true);
            yield return 2;
            player.SetHealth(Muskateers.classd_health);
            player.GiveItem(ItemType.USP);
            player.GiveItem(ItemType.ZONE_MANAGER_KEYCARD);
            player.PersonalClearBroadcasts();
            player.PersonalBroadcast(25, "Ты <color=#DAA130>Д-Класс</color>. Сбеги из комплекса до взрыва автоматической боеголовки, но избегай МТФ, отправленных чтобы убить тебя!", false);
        }
        public void EndGamemodeRound()
        {
            Muskateers.Info("The Gamemode round has ended!");
            Muskateers.roundstarted = false;
            Muskateers.Server.Round.EndRound();
        }
    }
}