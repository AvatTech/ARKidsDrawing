using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Categories;
using Categories.Model;
using Categories.Services;
using Sketches.Services;
using UnityEngine;
using Zenject;


namespace UI.Controller
{
    public class MainUIController : MonoBehaviour
    {
        [Header("Panels")] [SerializeField] private GameObject categoryPanel;
        [SerializeField] private GameObject mainPagePanel;

        private CurrentCategoryManager _currentCategoryManager;

        private void Awake()
        {
            _currentCategoryManager = CurrentCategoryManager.Instance;
        }

        public void ShowSketchesPanel()
        {
            mainPagePanel.SetActive(false);
            categoryPanel.SetActive(true);
        }


        public void ShowMainPage()
        {
            _currentCategoryManager = null;

            categoryPanel.SetActive(false);
            mainPagePanel.SetActive(true);
        }
    }
}