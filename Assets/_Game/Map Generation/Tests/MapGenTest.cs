using Map;
using MapGen;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class MapGenTest {

    [Test]
    public void MapDimensionsTest() {
        MapGenConfig config = new(
            mapHeight: 20, mapWidth: 24, minRoomSize: 2, maxRoomSize: 4,
            maxRoomCount: 4, maxRoomGenerationAttempts: 7, random: new(0));
        IMap map = MapGenerator.GenerateMap(config);
        Assert.AreEqual(24, map.GetTiles().GetLength(0));
        Assert.AreEqual(20, map.GetTiles().GetLength(1));
    }

    [Test]
    public void MapRoomSizeTest() {
        MapGenConfig config = new(
            mapHeight: 64, mapWidth: 64, minRoomSize: 2, maxRoomSize: 4,
            maxRoomCount: 10, maxRoomGenerationAttempts: 100, random: new(123));
        IMap map = MapGenerator.GenerateMap(config);
        List<Room> rooms = GetRooms(map);
        rooms.ForEach(r => Assert.IsTrue(r.Coordinates.Count >= 4));
        rooms.ForEach(r => Assert.IsTrue(r.Coordinates.Count <= 16));
    }

    private static void ExploreRoom(Tiles[,] tiles, int x, int y, bool[,] visited, Room room) {
        if (x < 0 || x >= tiles.GetLength(0) || y < 0 || y >= tiles.GetLength(1) || visited[x, y] || tiles[x, y] != Tiles.FLOOR) {
            return;
        }

        visited[x, y] = true;
        room.AddCoordinate(x, y);

        ExploreRoom(tiles, x - 1, y, visited, room);
        ExploreRoom(tiles, x + 1, y, visited, room);
        ExploreRoom(tiles, x, y - 1, visited, room);
        ExploreRoom(tiles, x, y + 1, visited, room);
    }

    private static List<Room> GetRooms(IMap map) {
        List<Room> rooms = new List<Room>();
        Tiles[,] tiles = map.GetTiles();
        int mapWidth = tiles.GetLength(0);
        int mapHeight = tiles.GetLength(1);

        bool[,] visited = new bool[mapWidth, mapHeight];

        // Depth-First Search to find connected rooms
        for (int x = 0; x < mapWidth; x++) {
            for (int y = 0; y < mapHeight; y++) {
                if (tiles[x, y] == Tiles.FLOOR && !visited[x, y]) {
                    Room room = new Room();
                    ExploreRoom(tiles, x, y, visited, room);
                    rooms.Add(room);
                }
            }
        }

        return rooms;
    }

    private class Room {

        public Room() {
            Coordinates = new List<Vector2Int>();
        }

        public void AddCoordinate(int x, int y) {
            Coordinates.Add(new(x, y));
        }

        public List<Vector2Int> Coordinates { get; private set; }
    }
}