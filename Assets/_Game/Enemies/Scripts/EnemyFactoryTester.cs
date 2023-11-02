using Entities;
using UnityEngine;

namespace Enemies {

    public class EnemyFactoryTester : MonoBehaviour {

        public void Start()
        {
            print("enemy factory test passed = " + doTest());
        }


        public bool doTest()
        {
            bool typeCorrect = false;
            bool positionCorrect = false;

            BlobEnemyFactory enemyFactory = new BlobEnemyFactory();

            Vector2Int randomPos = new Vector2Int((int)Random.Range(0, 1000), (int)Random.Range(0, 1000));

            IEntity testEnemy = enemyFactory.CreateEnemy(randomPos);

            if(testEnemy == null)
            {
                Debug.LogError("no enemy was generated");
                return false;
            }

            if(testEnemy is BlobEnemy)
            {
                typeCorrect = true;
            }
            else
            {
                Debug.LogError("wrong enemy type was generated");
            }

            if(testEnemy.Position == randomPos)
            {
                positionCorrect = true;
            }
            else
            {
                Debug.LogError("position of enemy was not assiged correctly");
            }

            return (typeCorrect && positionCorrect);
        }
    }
}