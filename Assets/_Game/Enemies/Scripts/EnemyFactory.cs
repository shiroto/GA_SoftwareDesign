using Entities;
using UnityEngine;

namespace Enemies {

    public abstract class EnemyFactory {

        public abstract IEntity CreateEnemy(Vector2Int position);
    }
}