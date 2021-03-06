using System.Linq;
using Smod2;
using Smod2.API;
using System.Collections.Generic;
using System;

namespace Gungame
{
    public class Functions
    {
        public static Functions singleton;
        public GunGame GunGame;
        public Functions(GunGame plugin)
        {
            this.GunGame = plugin;
            Functions.singleton = this;
        }

        public void EnableGamemode()
        {
            GunGame.enabled = true;
            if (!GunGame.roundstarted)
            {
                GunGame.Server.Map.ClearBroadcasts();
                GunGame.Server.Map.Broadcast(10, "������� ����� <color=#5D9AAC>����� ����������</color> ����������..", false);
            }
        }
        public void DisableGamemode()
        {
            GunGame.enabled = false;
            GunGame.Server.Map.ClearBroadcasts();
        }
        public void EndGamemodeRound()
        {
            GunGame.Info("EndGamemode Round");
            GunGame.roundstarted = false;
            GunGame.Server.Round.EndRound();
            GunGame.winner = null;
        }
        public IEnumerable<float> Spawn(Player player)
        {
            player.ChangeRole(Role.CLASSD, false, false, false, false);
            player.Teleport(new Vector(GetSpawn().x, (GetSpawn().y + 3), GetSpawn().z));
            yield return 1;
            player.SetGodmode(false);
            player.SetHealth(GunGame.health);
            foreach (Smod2.API.Item item in player.GetInventory())
            {
                item.Remove();
            }
            if (GunGame.reversed)
                player.GiveItem(ItemType.E11_STANDARD_RIFLE);
            else
                player.GiveItem(ItemType.FRAG_GRENADE);
            player.GiveItem(ItemType.MEDKIT);
        }
        public void LockDoors()
        {
            foreach (Smod2.API.Door door in GunGame.Server.Map.GetDoors())
            {
                if (door.Name.Contains("CHECKPOINT") || door.Name.Contains("079") || door.Name.Contains("106"))
                    door.Locked = true;
                else
                    door.Locked = false;
            }
        }
        public void ReplaceGun(Player player)
        {
            if (GunGame.reversed)
            {
                foreach (Smod2.API.Item item in player.GetInventory())
                {
                    switch (item.ItemType)
                    {
                        case ItemType.E11_STANDARD_RIFLE:
                            item.Remove();
                            player.GiveItem(ItemType.P90);
                            break;
                        case ItemType.P90:
                            item.Remove();
                            player.GiveItem(ItemType.LOGICER);
                            break;
                        case ItemType.LOGICER:
                            item.Remove();
                            player.GiveItem(ItemType.MP4);
                            break;
                        case ItemType.MP4:
                            item.Remove();
                            player.GiveItem(ItemType.USP);
                            break;
                        case ItemType.USP:
                            item.Remove();
                            player.GiveItem(ItemType.COM15);
                            break;
                        case ItemType.COM15:
                            item.Remove();
                            player.GiveItem(ItemType.FRAG_GRENADE);
                            break;
                        case ItemType.FRAG_GRENADE:
                            item.Remove();
                            player.GiveItem(ItemType.DISARMER);
                            break;
                    }
                }
            }
            else
                foreach (Smod2.API.Item item in player.GetInventory())
                {
                    switch (item.ItemType)
                    {
                        case ItemType.FRAG_GRENADE:
                            item.Remove();
                            player.GiveItem(ItemType.COM15);
                            break;
                        case ItemType.COM15:
                            item.Remove();
                            player.GiveItem(ItemType.USP);
                            break;
                        case ItemType.USP:
                            item.Remove();
                            player.GiveItem(ItemType.MP4);
                            break;
                        case ItemType.MP4:
                            item.Remove();
                            player.GiveItem(ItemType.LOGICER);
                            break;
                        case ItemType.LOGICER:
                            item.Remove();
                            player.GiveItem(ItemType.P90);
                            break;
                        case ItemType.P90:
                            item.Remove();
                            player.GiveItem(ItemType.E11_STANDARD_RIFLE);
                            break;
                    }
                }
            player.SetAmmo(AmmoType.DROPPED_5, 500);
            player.SetAmmo(AmmoType.DROPPED_7, 500);
            player.SetAmmo(AmmoType.DROPPED_9, 500);
        }
        public void AnnounceWinner(Player player)
        {
            GunGame.Server.Map.ClearBroadcasts();
            GunGame.Server.Map.Broadcast(15, "� ��� ���� ����������! ����������� " + player.Name + "!", false);
            EndGamemodeRound();
        }
        public Room GetRooms(ZoneType zone)
        {
            List<Room> rooms = new List<Room>();
            foreach (Room room in GunGame.Server.Map.Get079InteractionRooms(Scp079InteractionType.CAMERA).Where(rm => rm.ZoneType == zone))
            {
                if (GunGame.validRooms.Contains(room.RoomType))
                    rooms.Add(room);
            }
            int r = GunGame.gen.Next(rooms.Count);
            return rooms[r];

        }
        public Vector GetSpawn()
        {
            switch (GunGame.zone.ToLower())
            {
                case "lcz":
                    return GetRooms(ZoneType.LCZ).Position;
                case "hzc":
                    return GetRooms(ZoneType.HCZ).Position;
                case "enterance":
                case "ent":
                    return GetRooms(ZoneType.ENTRANCE).Position;
                default:
                    return new Vector(53, 1020, -44);
            }
        }
    }
}