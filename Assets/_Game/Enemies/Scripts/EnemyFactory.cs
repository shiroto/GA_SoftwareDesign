using Entities;
using UnityEngine;

namespace Enemies {

    public static class EnemyFactory {

        public static IEntity CreateBlobEnemy(Vector2Int position) {
            return new BlobEnemy(position);
        }
    }
}