using Map;
using System.Text;
using UnityEngine;

namespace MVT {

    internal class MapView : MonoBehaviour, IMapView {

        [SerializeField]
        private TMPro.TMP_Text text;

        public void SetMap(IMap map) {
            MapViewConfig config = MapViewConfig.GetInstance();
            StringBuilder sb = new();
            Tiles[,] tiles = map.GetTiles();
            Debug.Log(tiles.GetLength(0));
            Debug.Log(tiles.GetLength(1));
            for (int y = 0; y < tiles.GetLength(1); y++)
            {
                for (int x = 0; x < tiles.GetLength(0); x++)
                {
                    Tiles tile = tiles[x, y];
                    sb.Append(config.TileToCharacterDic[tile]);
                }
                sb.AppendLine();
            }
            text.text = sb.ToString();
        }
    }
}