using Map;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MVT {

    [CreateAssetMenu(fileName = "MVT Config", menuName = "New MVT Config")]
    internal class MapViewConfig : ScriptableObject {
        private static MapViewConfig instance;

        [SerializeField]
        private MapView prefab;

        [SerializeField]
        private TileToCharacter[] tileToCharacter;

        private Dictionary<Tiles, char> tileToCharacterDic;

        public static MapViewConfig GetInstance() {
            if (instance == null) {
                instance = Resources.Load<MapViewConfig>("MVT Config");
                instance.tileToCharacterDic = instance.tileToCharacter.ToDictionary(ttc => ttc.tile, ttc => ttc.character);
            }
            return instance;
        }

        public MapView Prefab => prefab;

        public IReadOnlyDictionary<Tiles, char> TileToCharacterDic => tileToCharacterDic;

        [Serializable]
        public struct TileToCharacter {
            public char character;
            public Tiles tile;
        }
    }
}