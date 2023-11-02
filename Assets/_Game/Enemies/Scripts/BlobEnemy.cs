using Entities;
using Main;
using Map;
using System.Linq;
using UnityEngine;

namespace Enemies
{
    internal class BlobEnemy : IEntity {

        private static readonly Vector2Int[] DIRECTIONS =
            new Vector2Int[] { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

        public BlobEnemy(Vector2Int position) {
            Position = position;
        }

        public void Update(IGameState gameState) {
            // does move?
            if (Random.Range(0f, 1f) < 0.5f) {
                return;
            }
            Vector2Int movement;
            int attempts = 0;
            do {
                movement = DIRECTIONS[Random.Range(0, DIRECTIONS.Length)];
            } while (attempts < 10 &&
                (!IsWalkableTile(gameState, Position + movement) ||
                IsOccupied(gameState, Position + movement)));
            Position += movement;
        }

        private bool IsOccupied(IGameState gameState, Vector2Int position) {
            return gameState.Entities.Any(e => e.Position == position);
        }

        private bool IsWalkableTile(IGameState gameState, Vector2Int position) {
            return gameState.Map.GetTile(position) == Tiles.FLOOR;
        }

        public char Appearance => 'o';

        public Color Color => Color.red;

        public Vector2Int Position { get; private set; }
    }
}