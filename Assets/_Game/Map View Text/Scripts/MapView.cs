using Entities;
using Map;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace MVT {

    internal class MapView : MonoBehaviour, IMapView {
        private string[,] baseMap;
        private MapViewConfig config;
        private IMap map;

        [SerializeField]
        private TMPro.TMP_Text text;

        public void Init(IMap map) {
            this.map = map;
            config = MapViewConfig.GetInstance();
            InitBaseMap();
        }

        public void UpdateView(IEnumerable<IEntity> entities) {
            string[,] characters = GetCharacters(entities);
            SetText(characters);
        }

        private string[,] GetCharacters(IEnumerable<IEntity> entities) {
            string[,] characters = (string[,])baseMap.Clone();
            foreach (IEntity e in entities) {
                characters[e.Position.x, e.Position.y] =
                    $"<color=#{ColorUtility.ToHtmlStringRGBA(e.Color)}>{e.Appearance}</color>";
            }
            return characters;
        }

        private void InitBaseMap() {
            Tiles[,] tiles = map.GetTiles();
            baseMap = new string[tiles.GetLength(0), tiles.GetLength(1)];
            for (int y = 0; y < tiles.GetLength(1); y++) {
                for (int x = 0; x < tiles.GetLength(0); x++) {
                    Tiles tile = tiles[x, y];
                    baseMap[x, y] = config.TileToCharacterDic[tile] + "";
                }
            }
        }

        private void SetText(string[,] characters) {
            StringBuilder sb = new();
            for (int y = 0; y < characters.GetLength(1); y++) {
                for (int x = 0; x < characters.GetLength(0); x++) {
                    sb.Append(characters[x, y]);
                }
                sb.AppendLine();
            }
            text.text = sb.ToString();
        }
    }
}