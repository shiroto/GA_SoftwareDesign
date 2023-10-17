using Map;
using UnityEngine;

namespace MapGen {

    internal class Map : IMap {
        public Tiles[,] tiles;

        public Map(int width, int height) {
            tiles = new Tiles[width, height];
        }

        public Tiles GetTile(int x, int y) {
            return GetTile(new(x, y));
        }

        public Tiles GetTile(Vector2Int tile) {
            if (tile.x < 0 || tile.y < 0 || tile.x >= tiles.GetLength(0) || tile.y >= tiles.GetLength(1)) {
                return Tiles.NONE;
            }
            return tiles[tile.x, tile.y];
        }

        public Tiles[,] GetTiles() {
            return (Tiles[,])tiles.Clone();
        }

        public void SetTile(int x, int y, Tiles type) {
            tiles[x, y] = type;
        }

        public void SetTile(Vector2Int tile, Tiles type) {
            tiles[tile.x, tile.y] = type;
        }
    }
}