using UnityEngine;

namespace Map {

    public interface IMap {

        Tiles GetTile(int x, int y);

        Tiles GetTile(Vector2Int tile);

        Tiles[,] GetTiles();
    }
}