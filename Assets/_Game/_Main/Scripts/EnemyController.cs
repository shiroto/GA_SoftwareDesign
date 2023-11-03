using Enemies;
using UnityEngine;

namespace Main {

    public class EnemyController : MonoBehaviour {

        [SerializeField]
        private GameController gameController;

        private void GameController_OnLevelChanged(object sender, LevelChangedEventArgs e) {
            if (e.LevelID % 3 == 0) {
                gameController.EnemyFactory = new BlobFactory();
            } else if (e.LevelID % 3 == 1) {
                gameController.EnemyFactory = new BatFactory();
            } else {
                gameController.EnemyFactory = new RandomEnemyFactory();
            }
        }

        private void OnDisable() {
            gameController.OnLevelChanged -= GameController_OnLevelChanged;
        }

        private void OnEnable() {
            gameController.OnLevelChanged += GameController_OnLevelChanged;
        }
    }
}