using System.Collections.Generic;
using UI.Button.Controllers;
using UnityEngine;

namespace UI.Tab
{
    public class TabManager : MonoBehaviour
    {
        [SerializeField] private List<ButtonController> tabItems = new();


        private void Start()
        {
            foreach (ButtonController b in tabItems)
            {
                b.AddOnClick(() => { setUp(b); });
            }
        }

        private void setUp(ButtonController buttonController)
        {
            buttonController.Enable();

            foreach (var b in tabItems)
            {
                if (b != buttonController)
                {
                    b.Disable();
                }
            }
        }
    }
}