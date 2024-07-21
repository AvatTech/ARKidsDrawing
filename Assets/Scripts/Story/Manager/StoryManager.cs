using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Storage;
using Story.Controller;
using Unity.VisualScripting;
using UnityEngine;
using Utills;
using Zenject;

namespace Story.Manager
{
    public class StoryManager : MonoBehaviour
    {
        private int _currentIndex;

        public List<StoryPanelController> storyPanelControllers;


        [Inject] private readonly ILocalStorage _localStorage;

        private StoryPanelController _mainPage;


        private void Awake()
        {
            storyPanelControllers = new List<StoryPanelController>();
        }


        private void Start()
        {
            ShowStoryFlow();
        }

        private void ShowStoryFlow()
        {
            storyPanelControllers.ForEach(controller =>
            {
                if (controller.order == 10)
                {
                    _mainPage = controller;
                }
            });


            // get the number saved in player pref
            if (_localStorage.TryLoadInt(Constants.KeyFirstLaunch, out int value))
            {
                if (value == 1)
                {
                    //Show main page
                    Debug.Log("main page index is: " + storyPanelControllers.IndexOf(_mainPage));
                    ShowStory(storyPanelControllers.IndexOf(_mainPage));
                }
                else
                {
                    SortStories();
                    Debug.Log("Showwww stoooory");
                    ShowStory(0);
                    _localStorage.SaveInt(Constants.KeyFirstLaunch, 1);
                }
            }
            else
            {
                Debug.Log("oomad inja");
                _localStorage.SaveInt(Constants.KeyFirstLaunch, 1);
                SortStories();
                ShowStory(0);
            }
        }

        public void NextStory()
        {
            if (_currentIndex + 1 >= storyPanelControllers.Count)
                return;

            _currentIndex++;
            ShowStory(_currentIndex);
        }

        public void PreviousStory()
        {
            if (_currentIndex - 1 >= 0)
                return;

            _currentIndex--;
            ShowStory(_currentIndex);
        }


        private void ShowStory(int index)
        {
            
            Debug.Log("size listo: " + storyPanelControllers.Count);
            storyPanelControllers[index].gameObject.SetActive(true);

            // Ensure that rest of stories are disabled!
            storyPanelControllers.ForEach(controller =>
            {
                if (controller != storyPanelControllers[index])
                    controller.gameObject.SetActive(false);
            });
        }


        //---------------------------------------------------------------------


        private void SortStories()
        {
            var sorted = storyPanelControllers.OrderBy(controller => controller.order);
            storyPanelControllers = sorted.ToList();
        }
    }
}