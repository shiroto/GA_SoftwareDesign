using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    public class gameQuitter : MonoBehaviour
    {
        public void quit()
        {
            print("should close game now if not in editor");
            Application.Quit();
        }
    }
}
