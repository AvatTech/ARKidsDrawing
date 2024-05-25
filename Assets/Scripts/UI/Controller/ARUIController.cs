using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Vuforia;

namespace UI.Controller
{
    public class ARUIController : MonoBehaviour
    {
        [SerializeField] private PlaneFinderBehaviour planeFinderBehaviour;
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private GameObject waitingObject;

        private void Start()
        {
            planeFinderBehaviour.OnAutomaticHitTest.AddListener(HandleAutomaticHitTest);
            waitingObject.SetActive(true);
        }


        /// <summary>
        /// Check whether the plane is ready or not?
        /// </summary>
        /// <param name="result"></param>
        void HandleAutomaticHitTest(HitTestResult result)
        {
            // surface is ready!
            if (result != null)
                waitingObject.SetActive(false);
            else // surface is not ready!
                waitingObject.SetActive(true);
        }
        


        public void OnExitButton()
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}