using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

namespace Main
{
    public class LevelUI : MonoBehaviour, IObserve<GameController>
    {
        public TextMeshProUGUI levelIndicator;
        [SerializeField]
        private GameController gameController; 
       void Awake()
        {

            gameController.AddObserver(this.GetComponent<LevelUI>()); ;
        }

        public void UpdateOberserver(int lvlId)
        {
            levelIndicator.text = "Level: " + lvlId.ToString(); 
        }
    }

    public interface IObserve<T>
    {
        void UpdateOberserver(int message);
    }
}
