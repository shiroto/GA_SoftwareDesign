using Enemies;
using Entities;
using Map;
using MapGen;
using MVT;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Main {
        
    internal class GameController : MonoBehaviour, IGameState {
        private MapGenConfig config;
        private List<IEntity> entities;
        private IMap map;
        private Vector2Int movement;
        private PlayerEntity player;

        [SerializeField]
        private int seed, width, height, minRoomSize, maxRoomSize, maxRoomCount, maxRoomGenerationAttempts;

        [SerializeField]
        private float updateInterval;

        private IMapView view;

        private IEnumerator AutoMove(KeyCode key) {
            while (Input.GetKey(key)) {
                if (IsWalkableTile(player.Position + movement) &&
                    !IsOccupied(player.Position + movement)) {
                    player.Position += movement;
                }
                if (map.GetTile(player.Position) == Tiles.STAIRS) {
                    InitNextLevel();
                }
                entities.ForEach(e => e.Update(this));
                view.UpdateView(entities);
                yield return new WaitForSeconds(updateInterval);
            }
            movement = Vector2Int.zero;
        }

        private void Awake() {
            config = new(height, width, minRoomSize, maxRoomSize, maxRoomCount, maxRoomGenerationAttempts, new System.Random(seed));
            player = new();
            entities = new();
            view = MapViewFactory.CreateMapView();
        }

        private Vector2Int GetEnemyStartPosition() {
            Vector2Int startPos = Vector2Int.zero;
            Tiles[,] tiles = map.GetTiles();
            do {
                startPos.x = Random.Range(0, tiles.GetLength(0));
                startPos.y = Random.Range(0, tiles.GetLength(1));
            } while (tiles[startPos.x, startPos.y] != Tiles.FLOOR || IsOccupied(startPos));
            return startPos;
        }

        private void InitNextLevel() {
            map = MapGenerator.GenMap(config);
            entities.Clear();
            entities.Add(player);
            SetPlayerStartPosition();
            InstantiateEnemies();
            view.Init(map);
        }

        private void InstantiateEnemies() {
            for (int i = 0; i < 5; i++) {
                Vector2Int position = GetEnemyStartPosition();
                IEntity enemy = EnemyFactory.CreateBlobEnemy(position);
                entities.Add(enemy);
            }
        }

        private bool IsOccupied(Vector2Int position) {
            return Entities.Any(e => e.Position == position);
        }

        private bool IsWalkableTile(Vector2Int position) {
            Tiles tile = map.GetTile(position);
            return tile == Tiles.FLOOR || tile == Tiles.PATH || tile == Tiles.STAIRS;
        }

        private void OnEnable() {
            InitNextLevel();
            view.UpdateView(entities);
        }

        [ContextMenu("Randomize Seed")]
        private void RandomizeSeed() {
            seed = Random.Range(int.MinValue, int.MaxValue);
        }

        private void SetPlayerStartPosition() {
            Vector2Int startPos = Vector2Int.zero;
            Tiles[,] tiles = map.GetTiles();
            do {
                startPos.x = Random.Range(0, tiles.GetLength(0));
                startPos.y = Random.Range(0, tiles.GetLength(1));
            } while (tiles[startPos.x, startPos.y] != Tiles.FLOOR);
            player.Position = startPos;
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.UpArrow)) {
                StopAllCoroutines();
                movement = Vector2Int.down;
                StartCoroutine(AutoMove(KeyCode.UpArrow));
            } else if (Input.GetKeyDown(KeyCode.DownArrow)) {
                StopAllCoroutines();
                movement = Vector2Int.up;
                StartCoroutine(AutoMove(KeyCode.DownArrow));
            } else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
                StopAllCoroutines();
                movement = Vector2Int.left;
                StartCoroutine(AutoMove(KeyCode.LeftArrow));
            } else if (Input.GetKeyDown(KeyCode.RightArrow)) {
                StopAllCoroutines();
                movement = Vector2Int.right;
                StartCoroutine(AutoMove(KeyCode.RightArrow));
            }
        }

        public IEnumerable<IEntity> Entities => entities;

        public IMap Map => map;
    }
}