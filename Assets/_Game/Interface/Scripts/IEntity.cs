using Main;
using UnityEngine;

namespace Entities {

    public interface IEntity {

        void Update(IGameState gameState);

        char Appearance { get; }

        Color Color { get; }

        Vector2Int Position { get; }
    }
}