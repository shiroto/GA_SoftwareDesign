using Entities;
using UnityEngine;

namespace Enemies {

    public abstract class EnemyFactory {

        public abstract IEntity CreateBlobEnemy(Vector2Int position);
    }

    public class BlobFactory : EnemyFactory
    {
        public override IEntity CreateBlobEnemy(Vector2Int position)
        {
            BlobEnemy blob = new BlobEnemy(position);
            return blob;
        }
    }

}