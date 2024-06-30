using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Story.Controller
{
    public class SplashScreenController : MonoBehaviour
    {
        [SerializeField] private float duration = 2f;

        private void Start()
        {
            StartCoroutine(SplashScreenRoutine());
        }

        private IEnumerator SplashScreenRoutine()
        {
            yield return new WaitForSeconds(duration);

            SceneManager.LoadScene("MainMenu");
        }
    }
}