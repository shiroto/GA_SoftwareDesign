using Entities;
using System.Collections.Generic;
using UnityEngine;

namespace Enemies
{
    public class EnemyFactoryOdd : IEnemyPool
    {
        private Queue<IEnemy> pool = new();

        public EnemyFactoryOdd()
        {
            for (int i = 0; i < 10; i++)
            {
                pool.Enqueue(new XEnemy(Vector2Int.zero));
            }
        }

        public IEnemy GetRandomEnemy(Vector2Int position)
        {
            IEnemy entity = pool.Dequeue();
            entity.Reinit(position);
            return entity;
        }

        public void ReturnToPool(IEnemy entity)
        {
            pool.Enqueue(entity);
        }
    }
}