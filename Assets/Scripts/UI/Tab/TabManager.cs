using System.Collections.Generic;
using UI.Button.Controllers;
using UnityEngine;

namespace UI.Tab
{
    public class TabManager : MonoBehaviour
    {
        [SerializeField] private ButtonController defaultSelectedButton;
        [SerializeField] private List<ButtonController> tabItems = new();


        private void Start()
        {
            foreach (ButtonController b in tabItems)
            {
                b.AddOnClick(() => { setUp(b); });
            }

            defaultSelectedButton.Enable();
            defaultSelectedButton.setModificationType();
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