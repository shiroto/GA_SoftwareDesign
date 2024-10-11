using Entities;
using UnityEngine;

namespace Enemies
{
    public interface IEnemyPool
    {
        IEnemy GetRandomEnemy(Vector2Int position);

        void ReturnToPool(IEnemy entity);
    }
}