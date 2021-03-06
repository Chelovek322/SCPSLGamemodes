using Smod2.API;
using Smod2.EventHandlers;
using Smod2.EventSystem.Events;
using System.Collections.Generic;
using Smod2.Events;
using scp4aiur;

namespace MassacreGamemode
{
    internal class EventsHandler : IEventHandlerTeamRespawn, IEventHandlerSetRole, IEventHandlerCheckRoundEnd, IEventHandlerRoundStart, IEventHandlerPlayerJoin, IEventHandlerRoundEnd, IEventHandlerWaitingForPlayers, IEventHandlerPlayerDie
    {
        private readonly Massacre plugin;
        public static Player winner = null;

        public EventsHandler(Massacre plugin) => this.plugin = plugin;
        public void OnPlayerJoin(PlayerJoinEvent ev)
        {
            if (Massacre.enabled)
            {
                if (!Massacre.roundstarted)
                {
                    Server server = plugin.pluginManager.Server;
                    server.Map.ClearBroadcasts();
                    server.Map.Broadcast(25, "<color=#50c878>�����</color> ����������...", false);
                }
            }
        }
        public void OnSetRole(PlayerSetRoleEvent ev)
        {
            if (Massacre.enabled || Massacre.roundstarted)
            {
                if (ev.TeamRole.Role == Role.SCP_173)
                {
                    ev.Player.SetHealth(Massacre.nut_health);
                }
            }
        }

        public void OnWaitingForPlayers(WaitingForPlayersEvent ev)
        {
            Massacre.SpawnRoom = plugin.GetConfigString("mass_spawn_room");
            Massacre.SpawnLoc = Functions.singleton.SpawnLoc();
            Massacre.nut_health = plugin.GetConfigInt("mass_peanut_health");
            Massacre.nut_count = plugin.GetConfigInt("mass_peanut_count");
        }

        public void OnRoundStart(RoundStartEvent ev)
        {
            if (Massacre.enabled)
            {
                Massacre.roundstarted = true;
                foreach (Door door in ev.Server.Map.GetDoors())
                {
                    door.Locked = true;
                    door.Open = false;
                }
                plugin.pluginManager.Server.Map.ClearBroadcasts();
                plugin.Info("Massacre of the D-Bois Gamemode Started!");
                List<Player> players = ev.Server.GetPlayers();
                List<string> nuts = new List<string>();

                for (int i = 0; i < Massacre.nut_count; i++)
                {
                    int random = Massacre.generator.Next(players.Count);
                    Player randomplayer = players[random];
                    players.Remove(randomplayer);
                    Timing.Run(Functions.singleton.SpawnNut(randomplayer, 0));

                }
                foreach (Player player in players)
                {
                    Timing.Run(Functions.singleton.SpawnDboi(player, 0));
                }
            }
        }

        public void OnRoundEnd(RoundEndEvent ev)
        {
            if (Massacre.enabled || Massacre.roundstarted)
            {
                plugin.Info("Round Ended!");
                Functions.singleton.EndGamemodeRound();
            }
        }

        public void OnPlayerDie(PlayerDeathEvent ev)
        {
            if (Massacre.enabled || Massacre.roundstarted)
            {
                if (ev.Player.TeamRole.Role == Role.CLASSD)
                {
                    plugin.Server.Map.ClearBroadcasts();
                    plugin.Server.Map.Broadcast(5, "� ����� " + (plugin.Server.Round.Stats.ClassDAlive - 1) + " �����-�.", false);
                    ev.Player.PersonalBroadcast(25, "�� ����! �� �� �������������, ������ �� ������ ������������ � ��������, ��� ���� ������ �������!", false);
                }
            }
        }

        public void OnCheckRoundEnd(CheckRoundEndEvent ev)
        {
            if (Massacre.enabled || Massacre.roundstarted)
            {
                bool peanutAlive = false;
                bool humanAlive = false;
                int humanCount = 0;

                foreach (Player player in ev.Server.GetPlayers())
                {
                    if (player.TeamRole.Team == Team.SCP)
                    {
                        peanutAlive = true; continue;
                    }

                    else if (player.TeamRole.Team != Team.SCP && player.TeamRole.Team != Team.SPECTATOR)
                    {
                        humanAlive = true;
                        humanCount++;
                    }

                }
                if (ev.Server.GetPlayers().Count > 1)
                {
                    if (peanutAlive && humanAlive && humanCount > 1)
                    {
                        ev.Status = ROUND_END_STATUS.ON_GOING;
                    }
                    else if (peanutAlive && humanAlive && humanCount == 1)
                    {
                        ev.Status = ROUND_END_STATUS.OTHER_VICTORY; Functions.singleton.EndGamemodeRound();
                        foreach (Player player in ev.Server.GetPlayers())
                        {
                            if (player.TeamRole.Team == Team.CLASSD)
                            {
                                ev.Server.Map.ClearBroadcasts();
                                ev.Server.Map.Broadcast(10, player.Name + " ����������!", false);
                                winner = player;
                            }
                        }
                    }
                    else if (peanutAlive && humanAlive == false)
                    {
                        ev.Status = ROUND_END_STATUS.SCP_VICTORY; Functions.singleton.EndGamemodeRound();
                    }
                    else if (peanutAlive == false && humanAlive)
                    {
                        ev.Status = ROUND_END_STATUS.CI_VICTORY; Functions.singleton.EndGamemodeRound();
                    }
                }
            }
        }

        public void OnTeamRespawn(TeamRespawnEvent ev)
        {
            if (Massacre.enabled || Massacre.roundstarted)
            {
                ev.SpawnChaos = true;
                ev.PlayerList = new List<Player>();
            }
        }
    }
}
