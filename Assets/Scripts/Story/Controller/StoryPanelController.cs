using System;
using Story.Manager;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Story.Controller
{
    public class StoryPanelController : MonoBehaviour
    {
        [SerializeField] public int order;
        [SerializeField] private bool isStoryPanel;


        [Space, SerializeField] private Button nextButton;
        [Inject] private readonly StoryManager _storyManager;


        private void Awake()
        {
            _storyManager.storyPanelControllers.Add(this);
        }

        private void Init()
        {
            if (!isStoryPanel)
                return;

            if (!nextButton)
                return;

            nextButton.onClick.AddListener(OnNextStoryClicked);
        }

        private void Start()
        {
            Init();
        }


//------------------------------------------
        private void OnNextStoryClicked()
        {
            _storyManager.NextStory();
        }
    }
}