using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.Controller
{
    public class ARUIController : MonoBehaviour
    {
        public void OnExitButton()
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}