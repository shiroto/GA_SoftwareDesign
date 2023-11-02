using Enemies;
using Entities;
using Map;
using MapGen;
using MVT;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

namespace Main
{
    internal class GameController : MonoBehaviour, IGameState
    {
        private MapGenConfig config;
        private List<IEntity> entities;
        private IMap map;
        private Vector2Int movement;
        private PlayerEntity player;
        List<IObserve<GameController>> observers = new List<IObserve<GameController>>();
        [SerializeField]
        private int levelID = 0;

        [SerializeField]
        private int seed, width, height, minRoomSize, maxRoomSize, maxRoomCount, maxRoomGenerationAttempts;

        [SerializeField]
        private float updateInterval;

        private IMapView view;
        private RemoteControl remoteControl;
        private GameManagement gameManagement;
        private IEnumerator AutoMove(KeyCode key)
        {
            while (Input.GetKey(key))
            {
                if (IsWalkableTile(player.Position + movement) &&
                    !IsOccupied(player.Position + movement))
                {
                    player.Position += movement;
                }
                if (map.GetTile(player.Position) == Tiles.STAIRS)
                {
                    InitNextLevel();
                }
                entities.ForEach(e => e.Update(this));
                view.UpdateView(entities);
                yield return new WaitForSeconds(updateInterval);
            }
            movement = Vector2Int.zero;
        }

        private void Awake()
        {
            config = new(height, width, minRoomSize, maxRoomSize, maxRoomCount, maxRoomGenerationAttempts, new System.Random(seed));
            player = new();
            entities = new();
            view = MapViewFactory.CreateMapView();

            gameManagement = new GameManagement();
            remoteControl = new RemoteControl(new NewGameCommand(gameManagement), new QuitCommand(gameManagement));
        }


        private Vector2Int GetEnemyStartPosition()
        {
            Vector2Int startPos = Vector2Int.zero;
            Tiles[,] tiles = map.GetTiles();
            do
            {
                startPos.x = UnityEngine.Random.Range(0, tiles.GetLength(0));
                startPos.y = UnityEngine.Random.Range(0, tiles.GetLength(1));
            } while (tiles[startPos.x, startPos.y] != Tiles.FLOOR || IsOccupied(startPos));
            return startPos;
        }

        public void AddObserver(IObserve<GameController> observer)
        {
            observers.Add(observer);
        }

        public void RemoveObserver(IObserve<GameController> observer)
        {
            observers.Remove(observer);
        }

        public void NotifyObservers(int lvlID)
        {
            foreach (var observer in observers)
            {
                observer.UpdateOberserver(lvlID);
            }
        }
        [ContextMenu("name")]
        private void InitNextLevel()
        {
            map = MapGenerator.GenMap(config);
            entities.Clear();
            entities.Add(player);
            SetPlayerStartPosition();
            InstantiateEnemies();
            view.Init(map);
            levelID++;
            NotifyObservers(levelID);
        }

        private void InstantiateEnemies()
        {
            for (int i = 0; i < 5; i++)
            {
                Vector2Int position = GetEnemyStartPosition();
                IEntity enemy = EnemyFactory.CreateBlobEnemy(position);
                entities.Add(enemy);
            }
        }

        private bool IsOccupied(Vector2Int position)
        {
            return Entities.Any(e => e.Position == position);
        }

        private bool IsWalkableTile(Vector2Int position)
        {
            Tiles tile = map.GetTile(position);
            return tile == Tiles.FLOOR || tile == Tiles.PATH || tile == Tiles.STAIRS;
        }

        private void OnEnable()
        {
            InitNextLevel();
            view.UpdateView(entities);
        }

        [ContextMenu("Randomize Seed")]
        private void RandomizeSeed()
        {
            seed = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
        }

        private void SetPlayerStartPosition()
        {
            Vector2Int startPos = Vector2Int.zero;
            Tiles[,] tiles = map.GetTiles();
            do
            {
                startPos.x = UnityEngine.Random.Range(0, tiles.GetLength(0));
                startPos.y = UnityEngine.Random.Range(0, tiles.GetLength(1));
            } while (tiles[startPos.x, startPos.y] != Tiles.FLOOR);
            player.Position = startPos;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                StopAllCoroutines();
                movement = Vector2Int.down;
                StartCoroutine(AutoMove(KeyCode.UpArrow));
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                StopAllCoroutines();
                movement = Vector2Int.up;
                StartCoroutine(AutoMove(KeyCode.DownArrow));
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                StopAllCoroutines();
                movement = Vector2Int.left;
                StartCoroutine(AutoMove(KeyCode.LeftArrow));
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                StopAllCoroutines();
                movement = Vector2Int.right;
                StartCoroutine(AutoMove(KeyCode.RightArrow));
            }

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                remoteControl.PressButton(KeyCode.Alpha1); // Starte das Spiel
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                remoteControl.PressButton(KeyCode.Alpha2); // Beende das Spiel
            }
        }

        public IEnumerable<IEntity> Entities => entities;

        public IMap Map => map;
    }


    public class GameManagement : MonoBehaviour
    {
        public void NewGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void QuitGame()
        {
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }

    interface ICommand
    {
        void Execute();
    }

    class NewGameCommand : ICommand
    {
        private GameManagement game;

        public NewGameCommand(GameManagement game)
        {
            this.game = game;
        }

        public void Execute()
        {
            game.NewGame();
        }
    }

    class QuitCommand : ICommand
    {
        private GameManagement game;

        public QuitCommand(GameManagement game)
        {
            this.game = game;
        }

        public void Execute()
        {
            game.QuitGame();
        }
    }

    class RemoteControl
    {
        private ICommand newGameCommand;
        private ICommand quitCommand;

        public RemoteControl(ICommand newGameCommand, ICommand quitCommand)
        {
            this.newGameCommand = newGameCommand;
            this.quitCommand = quitCommand;
        }

        public void PressButton(KeyCode key)
        {
            if (key == KeyCode.Alpha1)
            {
                newGameCommand.Execute();
            }
            else if (key == KeyCode.Alpha2)
            {
                quitCommand.Execute();
            }
        }
    }
}


