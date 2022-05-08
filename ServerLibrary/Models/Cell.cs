using Library;
using Library.SystemModels;
using Server.Envir;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Models
{
    public class Cell
    {
        #region Variables

        private object objectLock = new object();
        private ConcurrentDictionary<uint, MapObject> InternalObjects;
        public List<MovementInfo> Movements;
        public Point Location;
        public Map Map;
        public SafeZoneInfo SafeZone;
        public bool HasMovement;

        #endregion Variables

        #region Properties

        public List<MapObject> Objects
        {
            get
            {
                if (InternalObjects != null)
                {
                    List<MapObject> currentObjects = InternalObjects.Values.ToList();

                    if (currentObjects.Count > 0)
                        return currentObjects;
                }

                return new List<MapObject>();
            }
        }

        #endregion Properties

        public Cell(Point location)
        {
            Location = location;
        }

        public void AddObject(MapObject ob)
        {
            lock (objectLock)
            {
                if (InternalObjects == null)
                    InternalObjects = new ConcurrentDictionary<uint, MapObject>();

                if (!InternalObjects.TryAdd(ob.ObjectID, ob))
                    InternalObjects[ob.ObjectID] = ob;

                ob.CurrentMap = Map;
                ob.CurrentLocation = Location;

                Map.OrderedObjects[Location.X].Add(ob);
            }
        }

        public void RemoveObject(MapObject ob)
        {
            lock (objectLock)
            {
                if (InternalObjects.TryRemove(ob.ObjectID, out ob))
                    Map.OrderedObjects[Location.X].Remove(ob);

                if (InternalObjects.Count == 0)
                    InternalObjects = null;
            }
        }

        public bool IsBlocking(MapObject checker, bool cellTime)
        {
            if (Objects == null)
                return false;

            foreach (MapObject ob in Objects)
            {
                if (!ob.Blocking)
                    continue;

                if (cellTime && SEnvir.Now < ob.CellTime)
                    continue;

                if (ob.Stats == null)
                    return true;

                if (ob.Buffs.Any(x => x.Type == BuffType.Cloak || x.Type == BuffType.Transparency) && ob.Level > checker.Level && !ob.InGroup(checker))
                    continue;

                return true;
            }

            return false;
        }

        public Cell GetMovement(MapObject ob)
        {
            if (Movements == null || Movements.Count == 0)
                return this;

            for (int i = 0; i < 5; i++) //20 Attempts to get movement;
            {
                MovementInfo movement = Movements[SEnvir.Random.Next(Movements.Count)];

                Map map = SEnvir.GetMap(movement.DestinationRegion.Map);


                Cell cell = map.GetCell(movement.DestinationRegion.PointList[SEnvir.Random.Next(movement.DestinationRegion.PointList.Count)]);

                if (cell == null)
                    continue;

                if (ob.Race == ObjectType.Player)
                {
                    PlayerObject player = (PlayerObject)ob;

                    if (movement.DestinationRegion.Map.MinimumLevel > ob.Level && !player.Character.Account.TempAdmin)
                    {
                        player.Connection.ReceiveChat(string.Format(player.Connection.Language.NeedLevel, movement.DestinationRegion.Map.MinimumLevel), MessageType.System);

                        foreach (SConnection con in player.Connection.Observers)
                            con.ReceiveChat(string.Format(con.Language.NeedLevel, movement.DestinationRegion.Map.MinimumLevel), MessageType.System);

                        break;
                    }
                    if (movement.DestinationRegion.Map.MaximumLevel > 0 && movement.DestinationRegion.Map.MaximumLevel < ob.Level && !player.Character.Account.TempAdmin)
                    {
                        player.Connection.ReceiveChat(string.Format(player.Connection.Language.NeedMaxLevel, movement.DestinationRegion.Map.MaximumLevel), MessageType.System);

                        foreach (SConnection con in player.Connection.Observers)
                            con.ReceiveChat(string.Format(con.Language.NeedMaxLevel, movement.DestinationRegion.Map.MaximumLevel), MessageType.System);

                        break;
                    }

                    if (movement.CompletedQuest != null || movement.NotCompletedQuest != null)
                    {
                        bool compmove = true;
                        bool notcompmove = true;

                        if (movement.CompletedQuest != null)
                        {
                            compmove = false;

                            if (player.Character.Quests.Any(x => x.QuestInfo == movement.CompletedQuest && x.Completed))
                                compmove = true;
                        }
                        if (movement.NotCompletedQuest != null)
                        {
                            notcompmove = false;

                            if (player.Character.Quests.Any(x => x.QuestInfo == movement.NotCompletedQuest && x.Completed))
                                notcompmove = false;
                            else
                                notcompmove = true;
                        }

                        if (!compmove || !notcompmove)
                        {
                            player.Connection.ReceiveChat("A mysterious force blocks the entrance", MessageType.System);

                            foreach (SConnection con in player.Connection.Observers)
                                con.ReceiveChat("A mysterious force blocks the entrance", MessageType.System);
                            break;
                        }
                    }

                    if (movement.NeedSpawn != null)
                    {
                        SpawnInfo spawn = SEnvir.Spawns.FirstOrDefault(x => x.Info == movement.NeedSpawn);

                        if (spawn == null)
                            break;

                        if (spawn.AliveCount == 0)
                        {
                            player.Connection.ReceiveChat(player.Connection.Language.NeedMonster, MessageType.System);

                            foreach (SConnection con in player.Connection.Observers)
                                con.ReceiveChat(con.Language.NeedMonster, MessageType.System);

                            break;
                        }

                    }

                    if (movement.NeedItem != null)
                    {
                        if (player.GetItemCount(movement.NeedItem) == 0)
                        {
                            player.Connection.ReceiveChat(string.Format(player.Connection.Language.NeedItem, movement.NeedItem.ItemName), MessageType.System);

                            foreach (SConnection con in player.Connection.Observers)
                                con.ReceiveChat(string.Format(con.Language.NeedItem, movement.NeedItem.ItemName), MessageType.System);
                            break;
                        }

                        player.TakeItem(movement.NeedItem, 1);
                    }

                    switch (movement.Effect)
                    {
                        case MovementEffect.SpecialRepair:
                            player.SpecialRepair(EquipmentSlot.Weapon);
                            player.SpecialRepair(EquipmentSlot.Shield);
                            player.SpecialRepair(EquipmentSlot.Helmet);
                            player.SpecialRepair(EquipmentSlot.Armour);
                            player.SpecialRepair(EquipmentSlot.Necklace);
                            player.SpecialRepair(EquipmentSlot.BraceletL);
                            player.SpecialRepair(EquipmentSlot.BraceletR);
                            player.SpecialRepair(EquipmentSlot.RingL);
                            player.SpecialRepair(EquipmentSlot.RingR);
                            player.SpecialRepair(EquipmentSlot.Shoes);

                            player.RefreshStats();
                            break;
                    }

                }

                return cell.GetMovement(ob);
            }

            return this;
        }
    }
}
