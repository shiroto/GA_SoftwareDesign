using Entities;
using UnityEngine;

namespace Enemies {

    public class BatFactory : EnemyFactory {

        public override IEntity CreateEnemy(Vector2Int position) {
            return new BatEnemy(position);
        }
    }
}