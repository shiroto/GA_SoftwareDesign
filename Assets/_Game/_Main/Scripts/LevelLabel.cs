using UnityEngine;

namespace Main {

    public class LevelLabel : MonoBehaviour {

        [SerializeField]
        private GameController controller;

        [SerializeField]
        private TMPro.TMP_Text label;

        private void OnEnable() {
            controller.OnLevelChanged += Controller_OnLevelChanged;
        }

        private void OnDisable() {
            controller.OnLevelChanged -= Controller_OnLevelChanged;
        }

        private void Controller_OnLevelChanged(object sender, LevelChangedEventArgs e) {
            label.text = $"Level {e.LevelID}";
        }
    }
}