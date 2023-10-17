using Entities;
using UnityEngine;

namespace Main {

    public class PlayerEntity : IEntity {

        public void Update(IGameState gameState) {
        }

        public char Appearance => '@';

        public Color Color => Color.yellow;

        public Vector2Int Position { get; set; }
    }
}