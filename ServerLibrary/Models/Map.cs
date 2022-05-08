using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Library;
using Library.Network;
using Library.SystemModels;
using Server.DBModels;
using Server.Envir;
using Server.Models.Monsters;
using S = Library.Network.ServerPackets;

namespace Server.Models
{
    public sealed class Map
    {
        public MapInfo Info
        {
            get;
        }

        public int Width
        {
            get; private set;
        }
        public int Height
        {
            get; private set;
        }

        public bool HasSafeZone
        {
            get; set;
        }

        public Cell[,] Cells
        {
            get; private set;
        }
        public List<Cell> ValidCells { get; } = new List<Cell>();

        public List<MapObject> Objects { get; } = new List<MapObject>();
        public List<PlayerObject> Players { get; } = new List<PlayerObject>();
        public List<MonsterObject> Bosses { get; } = new List<MonsterObject>();
        public List<MonsterObject> Mobs { get; } = new List<MonsterObject>();
        public List<MonsterObject> Flags { get; } = new List<MonsterObject>();
        public List<NPCObject> NPCs { get; } = new List<NPCObject>();
        public HashSet<MapObject>[] OrderedObjects;

        public DateTime LastProcess;
        public bool IsInstance;
        public DateTime ExpireTime;

        public DateTime HalloweenEventTime, ChristmasEventTime;

        public Map(MapInfo info)
        {
            Info = info;
        }

        public void Load()
        {
            string fileName = $"{Config.MapPath}{Info.FileName}.map";

            if (!File.Exists(fileName))
            {
                SEnvir.Log($"Map: {fileName} not found.");
                return;
            }


            byte[] fileBytes = File.ReadAllBytes(fileName);

            Width = fileBytes[23] << 8 | fileBytes[22];
            Height = fileBytes[25] << 8 | fileBytes[24];

            Cells = new Cell[Width, Height];

            int offSet = 28 + Width * Height / 4 * 3;

            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                {
                    byte flag = fileBytes[offSet + (x * Height + y) * 14];

                    if ((flag & 0x02) != 2 || (flag & 0x01) != 1)
                        continue;

                    ValidCells.Add(Cells[x, y] = new Cell(new Point(x, y)) { Map = this });
                }

            OrderedObjects = new HashSet<MapObject>[Width];
            for (int i = 0; i < OrderedObjects.Length; i++)
                OrderedObjects[i] = new HashSet<MapObject>();
        }
        public void Setup()
        {
            CreateGuards();
            CreateFlags();
        }

        private void CreateGuards()
        {
            foreach (GuardInfo info in Info.Guards)
            {
                MonsterObject mob = MonsterObject.GetMonster(info.Monster);
                mob.Direction = info.Direction;

                if (!mob.Spawn(Info, new Point(info.X, info.Y)))
                {
                    SEnvir.Log($"Failed to spawn Guard Map:{Info.Description}, Location: {info.X}, {info.Y}");
                    continue;
                }

            }
        }
        private void CreateFlags()
        {
            int shape = SEnvir.Random.Next(0, 12);
            Color colour = Color.FromArgb(SEnvir.Random.Next(256), SEnvir.Random.Next(256), SEnvir.Random.Next(256));
            foreach (FlagInfo info in Info.Flags)
            {
                MonsterObject mob = MonsterObject.GetMonster(info.Monster);
                mob.Direction = info.Direction;

                foreach (CastleInfo castle in SEnvir.CastleInfoList.Binding)
                {
                    if (castle.Map != Info)
                        continue;

                    GuildInfo ownerGuild = SEnvir.GuildInfoList.Binding.FirstOrDefault(x => x.Castle == castle);

                    if (ownerGuild != null)
                    {
                        mob.FlagShape = ownerGuild.FlagShape;
                        mob.FlagColour = ownerGuild.FlagColour;
                        mob.Name = ownerGuild.GuildName;
                    }
                    else
                    {
                        mob.FlagColour = colour;
                        mob.FlagShape = shape;
                        mob.Name = castle.Name;
                    }
                }

                if (!mob.Spawn(Info, new Point(info.X, info.Y)))
                {
                    SEnvir.Log($"Failed to spawn Flag Map:{Info.Description}, Location: {info.X}, {info.Y}");
                    continue;
                }
                else
                {
                    Flags.Add(mob);
                    Broadcast(new S.ObjectFlagColour { ObjectID = mob.ObjectID, FlagColour = mob.FlagColour, FlagShape = mob.FlagShape, name = mob.Name });
                }
            }
        }


        public void Process()
        {
            if (SEnvir.Now > Info.KillStreakEndTime && Info.KillSteakActive == true)
            {

                if (Info.KillStreakExperienceRate > 0)
                {
                    Broadcast(new S.Chat { Text = ("Kill streak bonus has been reset"), Type = MessageType.System });
                }
                Info.KillStreakExperienceRate = 0;
                foreach (PlayerObject eplayers in Players)
                {
                    eplayers.ApplyMapBuff();
                }
                Info.KillSteakActive = false;

            }
        }

        public void AddObject(MapObject ob)
        {
            Objects.Add(ob);

            switch (ob.Race)
            {
                case ObjectType.Player:
                    Players.Add((PlayerObject)ob);
                    break;
                case ObjectType.Item:
                    break;
                case ObjectType.NPC:
                    NPCs.Add((NPCObject)ob);
                    break;
                case ObjectType.Spell:
                    break;
                case ObjectType.Monster:
                    MonsterObject mob = (MonsterObject)ob;
                    if (mob.MonsterInfo.IsBoss || mob.SuperMob)
                        Bosses.Add(mob);
                    break;
            }
        }
        public void RemoveObject(MapObject ob)
        {
            Objects.Remove(ob);

            switch (ob.Race)
            {
                case ObjectType.Player:
                    Players.Remove((PlayerObject)ob);
                    break;
                case ObjectType.Item:
                    break;
                case ObjectType.NPC:
                    NPCs.Remove((NPCObject)ob);
                    break;
                case ObjectType.Spell:
                    break;
                case ObjectType.Monster:
                    MonsterObject mob = (MonsterObject)ob;
                    if (mob.MonsterInfo.IsBoss || mob.SuperMob)
                        Bosses.Remove(mob);
                    break;
            }
        }

        public Cell GetCell(int x, int y)
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height)
                return null;

            return Cells[x, y];
        }
        public Cell GetCell(Point location)
        {
            return GetCell(location.X, location.Y);
        }
        public List<Cell> GetCells(Point location, int minRadius, int maxRadius)
        {
            List<Cell> cells = new List<Cell>();

            for (int d = 0; d <= maxRadius; d++)
            {
                for (int y = location.Y - d; y <= location.Y + d; y++)
                {
                    if (y < 0)
                        continue;
                    if (y >= Height)
                        break;

                    for (int x = location.X - d; x <= location.X + d; x += Math.Abs(y - location.Y) == d ? 1 : d * 2)
                    {
                        if (x < 0)
                            continue;
                        if (x >= Width)
                            break;

                        Cell cell = Cells[x, y]; //Direct Access we've checked the boudaries.

                        if (cell == null)
                            continue;

                        cells.Add(cell);
                    }
                }
            }

            return cells;
        }


        public Point GetRandomLocation()
        {
            return ValidCells.Count > 0 ? ValidCells[SEnvir.Random.Next(ValidCells.Count)].Location : Point.Empty;
        }

        public Point GetRandomLocation(Point location, int range, int attempts = 25)
        {
            int minX = Math.Max(0, location.X - range);
            int maxX = Math.Min(Width, location.X + range);
            int minY = Math.Max(0, location.Y - range);
            int maxY = Math.Min(Height, location.Y + range);

            for (int i = 0; i < attempts; i++)
            {
                Point test = new Point(SEnvir.Random.Next(minX, maxX), SEnvir.Random.Next(minY, maxY));

                if (GetCell(test) != null)
                    return test;
            }

            return Point.Empty;
        }

        public Point GetRandomLocation(int minX, int maxX, int minY, int maxY, int attempts = 25)
        {
            for (int i = 0; i < attempts; i++)
            {
                Point test = new Point(SEnvir.Random.Next(minX, maxX), SEnvir.Random.Next(minY, maxY));

                if (GetCell(test) != null)
                    return test;
            }

            return Point.Empty;
        }

        public void Broadcast(Point location, Packet p)
        {
            foreach (PlayerObject player in Players)
            {
                if (!Functions.InRange(location, player.CurrentLocation, Config.MaxViewRange))
                    continue;
                player.Enqueue(p);
            }
        }
        public void Broadcast(Packet p)
        {
            foreach (PlayerObject player in Players)
                player.Enqueue(p);
        }
    }

    public class SpawnInfo
    {
        public RespawnInfo Info;
        public Map CurrentMap;

        public DateTime NextSpawn;
        public int AliveCount;

        public DateTime LastCheck;

        public SpawnInfo(RespawnInfo info)
        {
            Info = info;
            CurrentMap = SEnvir.GetMap(info.Region.Map);
            LastCheck = SEnvir.Now;
        }

        public void DoSpawn(bool eventSpawn)
        {
            if (!eventSpawn)
            {
                if (Info.EventSpawn || SEnvir.Now < NextSpawn)
                    return;

                if (Info.Delay >= 1000000)
                {
                    TimeSpan timeofDay = TimeSpan.FromMinutes(Info.Delay - 1000000);

                    if (LastCheck.TimeOfDay >= timeofDay || SEnvir.Now.TimeOfDay < timeofDay)
                    {
                        LastCheck = SEnvir.Now;
                        return;
                    }

                    LastCheck = SEnvir.Now;
                }
                else
                {
                    if (Info.Announce)
                        NextSpawn = SEnvir.Now.AddSeconds(Info.Delay * 60);
                    else
                        NextSpawn = SEnvir.Now.AddSeconds(SEnvir.Random.Next(Info.Delay * 60) + Info.Delay * 30);

                }
            }

            for (int i = AliveCount; i < Info.Count; i++)
            {
                MonsterObject mob = MonsterObject.GetMonster(Info.Monster);

                if (!Info.Monster.IsBoss)
                {
                    if (SEnvir.Now > CurrentMap.HalloweenEventTime && SEnvir.Now <= Config.HalloweenEventEnd)
                    {
                        mob = new HalloweenMonster { MonsterInfo = Info.Monster, HalloweenEventMob = true };
                        CurrentMap.HalloweenEventTime = SEnvir.Now.AddHours(1);
                    }
                    else if (SEnvir.Now > CurrentMap.ChristmasEventTime && SEnvir.Now <= Config.ChristmasEventEnd)
                    {
                        mob.ChristmasEventMob = true;
                        mob.ExtraExperienceRate = 5;
                        CurrentMap.ChristmasEventTime = SEnvir.Now.AddMinutes(20);                        
                    }
                }


                mob.SpawnInfo = this;

                if (!mob.Spawn(Info.Region))
                {
                    mob.SpawnInfo = null;
                    continue;
                }

                if (Info.Announce)
                {
                    if (Info.Delay >= 1000000)
                    {
                        foreach (SConnection con in SEnvir.AuthenticatedConnections)
                            con.ReceiveChat($"{mob.MonsterInfo.MonsterName} has appeared.", MessageType.System);
                    }
                    else
                    {
                        foreach (SConnection con in SEnvir.AuthenticatedConnections)
                            con.ReceiveChat(string.Format(con.Language.BossSpawn, CurrentMap.Info.Description), MessageType.System);
                    }
                }

                mob.DropSet = Info.DropSet;
                AliveCount++;
            }
        }
    }
}
