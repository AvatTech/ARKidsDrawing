using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ARUIController : MonoBehaviour
{
    public void OnExitButton()
    {
        SceneManager.LoadScene("MainMenu");
    }
}