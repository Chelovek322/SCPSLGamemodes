using Smod2;
using Smod2.API;
using System.Collections.Generic;

namespace ZombielandGamemode
{
	public class Functions
    {
		public static Functions singleton;
		public Zombieland Zombieland;
		public Functions(Zombieland plugin)
		{
			this.Zombieland = plugin;
			Functions.singleton = this;
		}
        public void EnableGamemode()
        {
            Zombieland.enabled = true;
            if (!Zombieland.roundstarted)
            {
                Zombieland.plugin.pluginManager.Server.Map.ClearBroadcasts();
                Zombieland.plugin.pluginManager.Server.Map.Broadcast(25, "<color=#50c878>Zombieland Gamemode</color> is starting..", false);
            }
        }
        public void DisableGamemode()
        {
            Zombieland.enabled = false;
            Zombieland.plugin.pluginManager.Server.Map.ClearBroadcasts();
        }
        public void EndGamemodeRound()
        {
            if (Zombieland.enabled)
            {
                Zombieland.plugin.Info("EndgameRound Function");
                Zombieland.roundstarted = false;
                Zombieland.plugin.Server.Round.EndRound();
            }

        }

        public IEnumerable<float> SpawnChild(Player player, Player killer)
        {
            Vector spawn = killer.GetPosition();
            player.ChangeRole(Role.SCP_049_2, false, false, false, true);
			yield return 1;
            player.SetHealth(Zombieland.child_health);
            player.Teleport(spawn);

            player.PersonalClearBroadcasts();
            player.PersonalBroadcast(15, "You died and became a <color=#c50000>Zombie</color>! Attacking or killing humans creates more zombies! Death to the living!", false);
        }
        public IEnumerable<float> AliveCounter(float delay)
        {
            while (Zombieland.enabled || Zombieland.roundstarted)
            {
                Zombieland.plugin.Server.Map.ClearBroadcasts();
                int human_count = (Zombieland.plugin.Round.Stats.NTFAlive + Zombieland.plugin.Round.Stats.ScientistsAlive + Zombieland.plugin.Round.Stats.ClassDAlive + Zombieland.plugin.Round.Stats.CiAlive);
                Zombieland.plugin.Server.Map.Broadcast(10, "There are currently " + Zombieland.plugin.Round.Stats.Zombies + " zombies and " + human_count + " humans alive.", false);
                yield return delay;
            }
        }
        public IEnumerable<float> SpawnAlpha(Player player, float delay)
        {
            yield return delay;
            Vector spawn = Zombieland.plugin.Server.Map.GetRandomSpawnPoint(Role.SCP_049);
            player.ChangeRole(Role.SCP_049_2, false, false, true, false);
            yield return 1;
            player.Teleport(spawn);
            Zombieland.Alpha.Add(player);
            player.SetHealth(Zombieland.zombie_health);
            player.PersonalClearBroadcasts();
            player.PersonalBroadcast(15, "You are an alpha <color=#c50000>Zombie</color>! Attacking or killing humans creates more zombies! Death to the living!", false);
        }
    }
}