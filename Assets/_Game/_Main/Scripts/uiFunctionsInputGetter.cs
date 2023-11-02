using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Main
{
    public class uiFunctionsInputGetter : MonoBehaviour
    {
        public UnityEvent on1Pressed;
        public UnityEvent on2Pressed;

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Alpha1))
            {
                on1Pressed.Invoke();
            }

            if (Input.GetKeyUp(KeyCode.Alpha2))
            {
                on2Pressed.Invoke();
            }
        }
    }
}
