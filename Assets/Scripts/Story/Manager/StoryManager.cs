using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Storage;
using Story.Controller;
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
            if (_localStorage.TryLoadInt(Constants.KEY_FIRST_LAUNCH, out int value))
            {
                if (value == 0)
                {
                    SortStories();
                    ShowStory(0);
                    _localStorage.SaveInt(Constants.KEY_FIRST_LAUNCH, 1);
                }
                else
                {
                    //Show main page
                    ShowStory(storyPanelControllers.IndexOf(_mainPage));
                }
            }
            else
            {
                SortStories();
                ShowStory(0);
                _localStorage.SaveInt(Constants.KEY_FIRST_LAUNCH, 1);
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