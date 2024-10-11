using Map;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace MapGen
{
    public static class MapGenerator
    {
        public static IMap GenerateMap(MapGenConfig generationConfig)
        {
            Map map = new(generationConfig.MapWidth, generationConfig.MapHeight);
            GenerateRooms(generationConfig, map);
            AddWalls(map);
            ConnectRooms(generationConfig, map);
            PlaceStaircase(generationConfig, map);
            return map;
        }

        private static void ConnectRooms(MapGenConfig c, Map m)
        {
            HashSet<Vector2Int> r = new();
            HashSet<Vector2Int> u = new();
            // mark all unreachable
            for (int x = 0; x < m.tiles.GetLength(0); x++)
                for (int y = 0; y < m.tiles.GetLength(1); y++)
                    if (m.tiles[x, y] == Tiles.FLOOR)
                        u.Add(new(x, y));

            // random start
            Vector2Int s = GetRandom(u, c);
            Mark(m, r, u, s);
            // make random connections
            while (u.Count > 0)
            {
                Vector2Int end = GetRandom(r, c);
                s = GetRandom(u, c);
                Vector2Int current = s;
                bool wasW = false;
                while (current != end)
                {
                    if (HasAdjacentReachable(r, current))
                    {
                        break;
                    }
                    Vector2Int nextTile;
                    Vector2Int distance = end - current;
                    Vector2Int[] ops = GetVectors(distance);
                    nextTile = current + GetDirection(c, m, wasW, current, ops);
                    Tiles type = m.GetTile(nextTile);
                    if (type == Tiles.NONE || type == Tiles.WALL_HOR || type == Tiles.WALL_VER)
                    {
                        m.SetTile(nextTile, Tiles.PATH);
                        u.Add(nextTile);
                    }
                    wasW = type == Tiles.WALL_HOR || type == Tiles.WALL_VER;
                    current = nextTile;
                }
                Mark(m, r, u, s);
            }
        }

        private static void GenerateRooms(MapGenConfig config, Map map)
        {
            int roomCount = 0;

            for (int attempt = 0; attempt < config.MaxRoomGenerationAttempts && roomCount < config.MaxRoomCount; attempt++)
            {
                int roomWidth = config.Random.Next(config.MinRoomSize, config.MaxRoomSize + 1);
                int roomHeight = config.Random.Next(config.MinRoomSize, config.MaxRoomSize + 1);
                int roomX = config.Random.Next(1, config.MapWidth - roomWidth - 1);
                int roomY = config.Random.Next(1, config.MapHeight - roomHeight - 1);

                // Check if the room overlaps with existing rooms
                bool roomOverlaps = false;
                for (int x = roomX - 1; x <= roomX + roomWidth + 1; x++)
                {
                    for (int y = roomY - 1; y <= roomY + roomHeight + 1; y++)
                    {
                        if (x >= 0 && x < config.MapWidth && y >= 0 && y < config.MapHeight && map.tiles[x, y] == Tiles.FLOOR)
                        {
                            roomOverlaps = true;
                            break;
                        }
                    }
                    if (roomOverlaps)
                    {
                        break;
                    }
                }

                // If the room doesn't overlap, draw it on the map
                if (!roomOverlaps)
                {
                    for (int x = roomX; x < roomX + roomWidth; x++)
                    {
                        for (int y = roomY; y < roomY + roomHeight; y++)
                        {
                            map.tiles[x, y] = Tiles.FLOOR;
                        }
                    }
                    roomCount++;
                }
            }
        }

        private static Vector2Int GetDirection(MapGenConfig c, Map m, bool wasW, Vector2Int cP, Vector2Int[] v)
        {
            // try get direction without wall first
            int randomOffset = c.Random.Next(0, v.Length);
            for (int i = 0; i < v.Length; i++)
            {
                if (m.GetTile(cP + v[(i + randomOffset) % v.Length]) == Tiles.WALL_VER || m.GetTile(cP + v[(i + randomOffset) % v.Length]) == Tiles.WALL_HOR)
                {
                    continue;
                }
                return v[(i + randomOffset) % v.Length];
            }
            return v[c.Random.Next(0, v.Length)];
        }

        private static Vector2Int GetRandom(HashSet<Vector2Int> h, MapGenConfig c)
        {
            int randomIndex = c.Random.Next(h.Count);
            int i = 0;
            foreach (Vector2Int e in h)
            {
                if (randomIndex == i)
                {
                    return e;
                }
                i++;
            }
            throw new Exception();
        }

        private static Vector2Int[] GetVectors(Vector2Int distance)
        {
            Assert.IsTrue(distance != Vector2.zero);
            // case when both are 0
            if (distance.x == 0 ^ distance.y == 0)
            {
                Vector2 tmp = new Vector2(distance.x, distance.y).normalized;
                return new Vector2Int[] { new((int)tmp.x, (int)tmp.y) };
            }
            else
            {
                int x = distance.x / Mathf.Abs(distance.x);
                int y = distance.y / Mathf.Abs(distance.y);
                return new Vector2Int[] { new(x, 0), new(0, y) };
            }
        }

        private static bool HasAdjacent(Map map, Vector2Int tile, Tiles type)
        {
            return map.GetTile(tile + Vector2Int.up) == type ||
                map.GetTile(tile + Vector2Int.down) == type ||
                    map.GetTile(tile + Vector2Int.left) == type ||
                map.GetTile(tile + Vector2Int.right) == type;
        }

        private static bool HasAdjacentReachable(HashSet<Vector2Int> reachable, Vector2Int tile)
        {
            return reachable.Contains(tile + Vector2Int.up) || reachable.Contains(tile + Vector2Int.down)
                || reachable.Contains(tile + Vector2Int.left) || reachable.Contains(tile + Vector2Int.right);
        }

        private static void Mark(Map m, HashSet<Vector2Int> h, HashSet<Vector2Int> h2, Vector2Int v)
        {
            if (v.x < 0 || v.y < 0 || v.x >= m.tiles.GetLength(0) || v.y >= m.tiles.GetLength(1)
                    || !h2.Contains(v))
            {
                return;
            }
            if (
                m.tiles[v.x, v.y] == Tiles.FLOOR || m.tiles[v.x, v.y] == Tiles.PATH)
            {
                h2.Remove(v);
                h.Add(v);
                Mark(m, h, h2, v + Vector2Int.up);
                Mark(m, h, h2, v + Vector2Int.down);
                Mark(m, h, h2, v + Vector2Int.left);
                Mark(m, h, h2, v + Vector2Int.right);
            }
        }

        private static void PlaceStaircase(MapGenConfig config, Map map)
        {
            Vector2Int tile = new();
            do
            {
                tile.x = config.Random.Next(config.MapWidth);
                tile.y = config.Random.Next(config.MapHeight);
            } while (map.GetTile(tile) != Tiles.FLOOR || HasAdjacent(map, tile, Tiles.PATH));
            map.tiles[tile.x, tile.y] = Tiles.STAIRS;
        }

        private static void AddWalls(Map m)
        {
            for (int x = 0; x < m.tiles.GetLength(0); x++)
            {
                for (int y = 0; y < m.tiles.GetLength(1); y++)
                {
                    Vector2Int tile = new(x, y);
                    if (m.GetTile(tile) == Tiles.FLOOR)
                    {
                        if (m.GetTile(tile + Vector2Int.up) == Tiles.NONE)
                        {
                            m.SetTile(tile + Vector2Int.up, Tiles.WALL_HOR);
                        }
                        if (m.GetTile(tile + Vector2Int.down) == Tiles.NONE)
                        {
                            m.SetTile(tile + Vector2Int.down, Tiles.WALL_HOR);
                        }
                        if (m.GetTile(tile + Vector2Int.left) == Tiles.NONE)
                        {
                            m.SetTile(tile + Vector2Int.left, Tiles.WALL_VER);
                        }
                        if (m.GetTile(tile + Vector2Int.right) == Tiles.NONE)
                        {
                            m.SetTile(tile + Vector2Int.right, Tiles.WALL_VER);
                        }
                    }
                }
            }
        }
    }
}