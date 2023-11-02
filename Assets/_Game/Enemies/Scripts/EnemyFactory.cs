using Entities;
using UnityEngine;

namespace Enemies {

    public abstract class EnemyFactory {

        public abstract IEntity CreateEnemy(Vector2Int position);
    }

    public class BlobEnemyFactory : EnemyFactory
    {
        public override IEntity CreateEnemy(Vector2Int position)
        {
            IEntity newEnemy = new BlobEnemy(position);

            return newEnemy;
        }
    }
}