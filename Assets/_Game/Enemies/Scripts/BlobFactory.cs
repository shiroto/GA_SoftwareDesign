using Entities;
using UnityEngine;

namespace Enemies {

    public class BlobFactory : EnemyFactory {

        public override IEntity CreateEnemy(Vector2Int position) {
            return new BlobEnemy(position);
        }
    }
}