using System;
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
        private int currentIndex = 0;
        public List<StoryPanelController> storyPanelControllers;

        [Inject] private readonly ILocalStorage _localStorage;

        private StoryPanelController mainPage = null;

        private void Awake()
        {
            storyPanelControllers = new();
        }


        private void Start()
        {
            storyPanelControllers.ForEach(controller =>
            {
                if (controller.order == 10)
                {
                    mainPage = controller;
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
                    ShowStory(storyPanelControllers.IndexOf(mainPage));
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
            if (currentIndex + 1 >= storyPanelControllers.Count)
                return;

            currentIndex++;
            ShowStory(currentIndex);
        }

        public void PreviousStory()
        {
            if (currentIndex - 1 >= 0)
                return;

            currentIndex--;
            ShowStory(currentIndex);
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