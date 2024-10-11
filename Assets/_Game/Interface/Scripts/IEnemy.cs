using Main;
using UnityEngine;

namespace Entities
{
    public interface IEnemy : IEntity
    {
        void Reinit(Vector2Int position);

        void Update(IGameState gameState);
    }
}