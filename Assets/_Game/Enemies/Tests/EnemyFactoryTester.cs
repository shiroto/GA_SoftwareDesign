using UnityEngine;
using Enemies;
using Entities;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;


namespace Enemies {

    public class EnemyFactoryTester {

        [Test]
        public void doTest()
        {
            bool typeCorrect = false;
            bool positionCorrect = false;

            BlobEnemyFactory enemyFactory = new BlobEnemyFactory();

            Vector2Int randomPos = new Vector2Int((int)Random.Range(0, 1000), (int)Random.Range(0, 1000));

            IEntity testEnemy = enemyFactory.CreateEnemy(randomPos);

            if(testEnemy == null)
            {
                Debug.LogError("no enemy was generated");
                Assert.Fail();
            }

            if(testEnemy is BlobEnemy)
            {
                typeCorrect = true;
            }
            else
            {
                Debug.LogError("wrong enemy type was generated");
                Assert.Fail();
            }

            if(testEnemy.Position == randomPos)
            {
                positionCorrect = true;
            }
            else
            {
                Debug.LogError("position of enemy was not assiged correctly");
                Assert.Fail();
            }

            Assert.IsTrue(positionCorrect);
            Assert.IsTrue(typeCorrect);
        }
    }
}