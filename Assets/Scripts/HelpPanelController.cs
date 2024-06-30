using UnityEngine;

public class HelpPanelController : MonoBehaviour
{
    public void OnBackButton()
    {
        gameObject.SetActive(false);
    }
}