using Entities;
using UnityEngine;

namespace Enemies {

    public class RandomEnemyFactory : EnemyFactory {

        public override IEntity CreateEnemy(Vector2Int position) {
            if (Random.Range(0, 2) == 0) {
                return new BlobEnemy(position);
            } else {
                return new BatEnemy(position);
            }
        }
    }
}